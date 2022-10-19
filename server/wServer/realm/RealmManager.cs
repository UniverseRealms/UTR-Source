using common;
using common.resources;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using wServer.logic;
using wServer.networking;
using wServer.networking.packets;
using wServer.realm.commands;
using wServer.realm.cores;
using wServer.realm.entities.vendors;
using wServer.realm.worlds;
using wServer.realm.worlds.logic;

namespace wServer.realm
{
    public class RealmManager
    {
        private readonly bool _initialized;
        public string InstanceId { get; }
        public bool Terminating { get; private set; }

        public Resources Resources { get; }
        public Database Database { get; }
        public ServerConfig Config { get; }
        public int TPS { get; }

        public ConnectManager ConMan { get; }
        public BehaviorDb Behaviors { get; }
        public ISManager InterServer { get; }
        public ISControl ISControl { get; }
        public ChatManager Chat { get; }
        public DbServerManager DbServerController { get; }
        public CommandManager Commands { get; }
        public Market Market { get; }
        public DbTinker Tinker { get; }
        public PortalMonitor Monitor { get; }
        public DbEvents DbEvents { get; }

        public NetworkTicker Network { get; private set; }
        public CoreManager Core { get; private set; }

        public object GameData { get; internal set; }

        public readonly ConcurrentDictionary<int, World> Worlds = new ConcurrentDictionary<int, World>();
        public readonly ConcurrentDictionary<Client, PlayerInfo> Clients = new ConcurrentDictionary<Client, PlayerInfo>();

        private int _nextWorldId;
        private int _nextClientId;
        public bool _isRaidLaunched = false;
        public bool _isChallengeLaunched = false;
        public bool _isDreamLaunched = false;

        public RealmManager(Resources resources, Database db, ServerConfig config)
        {
            InstanceId = Guid.NewGuid().ToString();
            Database = db;
            Resources = resources;
            Config = config;
            Config.serverInfo.instanceId = InstanceId;
            TPS = config.serverSettings.tps;

            // all these deal with db pub/sub... probably should put more thought into their structure...
            InterServer = new ISManager(Database, config);
            ISControl = new ISControl(this);
            Chat = new ChatManager(this);
            DbServerController = new DbServerManager(this); // probably could integrate this with ChatManager and rename...
            DbEvents = new DbEvents(this);

            // basic server necessities
            ConMan = new ConnectManager(this,
                config.serverSettings.maxPlayers,
                config.serverSettings.maxPlayersWithPriority);
            Behaviors = new BehaviorDb(this);
            Commands = new CommandManager(this);

            // some necessities that shouldn't be (will work this out later)
            MerchantLists.Init(this);
            Tinker = new DbTinker(db.Conn);
            if (Config.serverSettings.enableMarket)
                Market = new Market(this);

            var serverMode = config.serverSettings.mode;

            switch (serverMode)
            {
                case ServerMode.Single:
                    InitializeNexusHub();
                    AddWorld("Realm");
                    AddWorld("RealmTwo");
                    break;

                case ServerMode.Nexus:
                    InitializeNexusHub();
                    break;

                case ServerMode.Realm:
                    AddWorld("Realm");
                    AddWorld("RealmTwo");
                    break;

                case ServerMode.Marketplace:
                    AddWorld("Marketplace", true);
                    AddWorld("Vault");
                    AddWorld("ClothBazaar");
                    break;
            }

            // add portal monitor to nexus and initialize worlds
            if (Worlds.ContainsKey(World.Nexus))
                Monitor = new PortalMonitor(this, Worlds[World.Nexus]);
            foreach (var world in Worlds.Values)
                OnWorldAdded(world);

            _initialized = true;
        }

        private void InitializeNexusHub()
        {
            // load world data
            foreach (var wData in Resources.Worlds.Data.Values)
                if (wData.id < 0)
                    AddWorld(wData);
        }

        public void Run()
        {
            // start received packet processor
            Network = new NetworkTicker(this);
            var network = new Task(() => Network.TickLoop(), TaskCreationOptions.LongRunning);
            network.ContinueWith(exception => Program.Log.Fatal(exception.Exception.InnerException),
                        TaskContinuationOptions.OnlyOnFaulted);
            network.Start();

            // start CA logic management
            Core = new CoreManager(this);
            Core.init();
            Core.start();
        }

        public void GlobalBroadcast(params Packet[] packets)
        {
            foreach (var w in Worlds)
            {
                foreach (var p in w.Value.Players.Values)
                {
                    p.Client.SendPackets(packets);
                }
            }
        }

        public void Stop()
        {
            Terminating = true;
            InterServer.Dispose();
            Resources.Dispose();
            Core.stop();
            Network.Shutdown();
        }

        public bool TryConnect(Client client)
        {
            if (client?.Account == null)
                return false;

            client.Id = Interlocked.Increment(ref _nextClientId);
            var plrInfo = new PlayerInfo
            {
                AccountId = client.Account.AccountId,
                GuildId = client.Account.GuildId,
                PartyId = client.Account.PartyId,
                Name = client.Account.Name,
                WorldInstance = -1
            };
            Clients[client] = plrInfo;

            // recalculate usage statistics
            Config.serverInfo.players = ConMan.GetPlayerCount();
            Config.serverInfo.maxPlayers = Config.serverSettings.maxPlayers;
            Config.serverInfo.queueLength = ConMan.QueueLength();
            Config.serverInfo.playerList.Add(plrInfo);
            return true;
        }

        public void Disconnect(Client client)
        {
            var player = client.Player;
            player?.Owner?.LeaveWorld(player);

            Clients.TryRemove(client, out var plrInfo);

            // recalculate usage statistics
            Config.serverInfo.players = ConMan.GetPlayerCount();
            Config.serverInfo.maxPlayers = Config.serverSettings.maxPlayers;
            Config.serverInfo.queueLength = ConMan.QueueLength();
            Config.serverInfo.playerList.Remove(plrInfo);
        }

        private void AddWorld(string name, bool actAsNexus = false)
        {
            AddWorld(Resources.Worlds.Data[name], actAsNexus);
        }

        private void AddWorld(ProtoWorld proto, bool actAsNexus = false)
        {
            int id;
            if (actAsNexus)
                id = World.Nexus;
            else
                id = (proto.id < 0) ? proto.id : Interlocked.Increment(ref _nextWorldId);

            DynamicWorld.TryGetWorld(proto, null, out var world);

            if (world != null)
            {
                if (world is Marketplace && !Config.serverSettings.enableMarket)
                    return;

                AddWorld(id, world);
                return;
            }

            AddWorld(id, new World(proto));
        }

        private void AddWorld(int id, World world)
        {
            if (world.Manager != null)
                throw new InvalidOperationException("World already added.");

            world.Id = id;
            Worlds[id] = world;

            if (_initialized)
                OnWorldAdded(world);
        }

        public World AddWorld(World world)
        {
            if (world.Manager != null)
                throw new InvalidOperationException("World already added.");

            world.Id = Interlocked.Increment(ref _nextWorldId);
            Worlds[world.Id] = world;

            if (_initialized)
                OnWorldAdded(world);

            return world;
        }

        public World GetWorld(int id) => !Worlds.TryGetValue(id, out var ret) ? null : (ret.Id == 0 ? null : ret);

        public bool RemoveWorld(World world)
        {
            if (world.Manager == null)
                throw new InvalidOperationException("World is not added.");

            if (Worlds.TryRemove(world.Id, out world))
            {
                OnWorldRemoved(world);
                return true;
            }

            return false;
        }

        private void OnWorldAdded(World world)
        {
            Program.Debug(typeof(RealmManager), $"Adding world {world.Name} [{world.Id}].");

            world.Manager = this;
        }

        private void OnWorldRemoved(World world)
        {
            if (!(world is DeathArena))
                world.Manager = null;

            Monitor.RemovePortal(world.Id);
        }

        public World GetRandomGameWorld()
        {
            var realms = Worlds.Values.OfType<Realm>().Where(w => !w.Closed).ToArray();
            return realms.Length == 0 ? Worlds[World.Nexus] : realms[Environment.TickCount % realms.Length];
        }
    }
}
