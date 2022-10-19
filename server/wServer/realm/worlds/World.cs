using common;
using common.resources;
using DungeonGenerator;
using DungeonGenerator.Templates;
using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using wServer.logic.loot;
using wServer.networking;
using wServer.networking.packets;
using wServer.networking.packets.outgoing;
using wServer.realm.cores;
using wServer.realm.entities;
using wServer.realm.entities.vendors;
using wServer.realm.terrain;
using wServer.realm.worlds.logic;

namespace wServer.realm.worlds
{
    public class World
    {
        /// <summary>
        /// Since implementation of CA 3.0, several tests were made and we got a stable result to run a world instance without
        /// certain issues like rubberbanding and lag spikes on a large group. Best values found were between 50-60 players into
        /// a single world instance. For now, we recommend to start using most safe value to avoid issues on world instance and
        /// keep server running for hours without crashing.
        /// </summary>
        private const int maxPlayerCapacity = 65;

        private static readonly List<int> whitelistDungeonsCapacity = new List<int>()
        {
            Nexus, Vault, MarketPlace, Tavern, NexusExplanation, Tutorial, GuildHall, PetYard
        };

        protected static readonly ILog Log = LogManager.GetLogger(typeof(World));

        // world loot has a chance to drop with any mob death
        public Loot WorldLoot = new Loot(
            new TierLoot(1, ItemType.Potion, .03)
        );

        protected static readonly Random Rand = new Random((int)DateTime.Now.Ticks);

        public int ChallengeCount = 0;
        public int DreamCount = 0;

        public const int Tutorial = -1;
        public const int Nexus = -2;
        public const int Realm = 1;
        public const int NexusExplanation = -4;
        public const int Vault = -5;
        public const int Test = -6;
        public const int Tinker = -7;
        public const int GuildHall = -8;
        public const int Arena = -9;
        public const int ClothBazaar = -10;
        public const int FreeItems = -11;
        public const int PetYard = -12;
        public const int ArenaSolo = -13;
        public const int DeathArena = -14;
        public const int MarketPlace = -15;
        public const int Station = -16;
        public const int WORLD_BOSS = -17;
        public const int Tavern = -18;

        public bool canConnect => Players.Count < _playerCapacity;

        private RealmManager _manager;

        public RealmManager Manager
        {
            get => _manager;
            internal set
            {
                _manager = value;

                if (_manager != null)
                {
                    _playerCapacity = !whitelistDungeonsCapacity.Contains(Id) ?
                        maxPlayerCapacity :
                        Manager.Config.serverSettings.maxConnections;

                    Init();
                    InitCoreTasks();
                }
            }
        }

        public int Id { get; internal set; }
        public string Name { get; set; }
        public string SBName { get; set; }
        public int Difficulty { get; protected set; }
        public int Background { get; protected set; }
        public bool Limbo { get; protected set; }

        //public bool PvP { get; protected set; }
        public bool AllowTeleport { get; protected set; }

        public bool ShowDisplays { get; protected set; }
        public string[] ExtraXML { get; protected set; }
        public bool Persist { get; protected set; }
        public int Blocking { get; protected set; }
        public string Music { get; set; }
        public bool PlayerDungeon { get; set; }
        public string Opener { get; set; }
        public HashSet<string> Invites { get; set; }
        public HashSet<string> Invited { get; set; }

        public Wmap Map { get; private set; }
        public bool Deleted { get; protected set; }

        private long _elapsedTime;
        private int _totalConnects;
        private int _playerCapacity;
        public int TotalConnects => _totalConnects;
        public bool Closed { get; set; }
        public int EventsKilled = 0;
        public ConcurrentDictionary<int, Player> Players { get; private set; }
        public ConcurrentDictionary<int, Enemy> Enemies { get; private set; }
        public ConcurrentDictionary<int, Enemy> Quests { get; private set; }
        public ConcurrentDictionary<Tuple<int, byte>, Projectile> Projectiles { get; private set; }
        public ConcurrentDictionary<int, StaticObject> StaticObjects { get; private set; }
        public ConcurrentDictionary<int, Ally> Allies { get; private set; }

        public CollisionMap<Entity> EnemiesCollision { get; private set; }
        public CollisionMap<Entity> PlayersCollision { get; private set; }

        public List<WorldTimer> Timers { get; private set; }

        private static int _entityInc;

        private readonly object _deleteLock = new object();

        public World(ProtoWorld proto)
        {
            Setup();
            Id = proto.id;
            Name = proto.name;
            SBName = proto.sbName;
            Difficulty = proto.difficulty;
            Background = proto.background;
            Limbo = proto.isLimbo;
            Persist = proto.persist;
            AllowTeleport = !proto.restrictTp;
            ShowDisplays = proto.showDisplays;
            Blocking = proto.blocking;
            //PvP = proto.pvp;
            Opener = "";

            var rnd = new Random();
            Music = proto.music != null ?
                proto.music[rnd.Next(0, proto.music.Length)] :
                "Test";
        }

        private void Setup()
        {
            Players = new ConcurrentDictionary<int, Player>();
            Enemies = new ConcurrentDictionary<int, Enemy>();
            Quests = new ConcurrentDictionary<int, Enemy>();
            Projectiles = new ConcurrentDictionary<Tuple<int, byte>, Projectile>();
            StaticObjects = new ConcurrentDictionary<int, StaticObject>();
            Allies = new ConcurrentDictionary<int, Ally>();
            Timers = new List<WorldTimer>();
            ExtraXML = Empty<string>.Array;
            AllowTeleport = true;
            ShowDisplays = true;
            Persist = false; // if false, attempts to delete world with 0 players
            Blocking = 0; // toggles sight block (0 disables sight block)
        }

        public string GetDisplayName()
        {
            if (!string.IsNullOrEmpty(SBName))
            {
                return SBName[0] == '{' ? Name : SBName;
            }
            return Name;
        }

        public bool IsNotCombatMapArea => Id == Nexus || Id == Vault || Id == GuildHall ||
            Id == ClothBazaar || Id == NexusExplanation || Id == Tinker ||
            Id == MarketPlace;

        public virtual bool AllowedAccess(Client client)
        {
            return !Closed || client.Account.Admin;
        }

        public virtual KeyValuePair<IntPoint, TileRegion>[] GetSpawnPoints()
        {
            return Map.Regions.Where(t => t.Value == TileRegion.Spawn).ToArray();
        }

        public virtual World GetInstance(Client client)
        {
            DynamicWorld.TryGetWorld(_manager.Resources.Worlds[Name], client, out var world);

            if (world == null)
                world = new World(_manager.Resources.Worlds[Name]);

            world.Limbo = false;
            return Manager.AddWorld(world);
        }

        public long GetAge()
        {
            return _elapsedTime;
        }

        protected virtual void Init()
        {
            if (Limbo) return;

            var proto = Manager.Resources.Worlds[Name];

            if (proto.maps != null && proto.maps.Length <= 0)
            {
                var template = DungeonTemplates.GetTemplate(Name);
                if (template == null)
                    throw new KeyNotFoundException($"Template for {Name} not found.");
                FromDungeonGen(Rand.Next(), template);
                return;
            }

            var map = Rand.Next(0, proto.maps?.Length ?? 1);
            FromWorldMap(new MemoryStream(proto.wmap[map]));

            InitShops();
        }

        protected void InitShops()
        {
            foreach (var shop in MerchantLists.Shops)
            {
                var shopItems = new List<ISellableItem>(shop.Value.Item1);
                var mLocations = Map.Regions
                    .Where(r => shop.Key == r.Value)
                    .Select(r => r.Key)
                    .ToArray();

                if (shopItems.Count <= 0 || shopItems.All(i => i.ItemId == ushort.MaxValue))
                    continue;

                var rotate = shopItems.Count > mLocations.Length;

                var reloadOffset = 0;
                foreach (var loc in mLocations)
                {
                    var shopItem = shopItems[0];
                    shopItems.RemoveAt(0);
                    while (shopItem.ItemId == ushort.MaxValue)
                    {
                        if (shopItems.Count <= 0)
                            shopItems.AddRange(shop.Value.Item1);

                        shopItem = shopItems[0];
                        shopItems.RemoveAt(0);
                    }

                    reloadOffset += 500;
                    var m = new WorldMerchant(Manager, 0x01ca)
                    {
                        ShopItem = shopItem,
                        Item = shopItem.ItemId,
                        Price = shopItem.Price,
                        Count = shopItem.Count,
                        Currency = shop.Value.Item2,
                        RankReq = shop.Value.Item3,
                        ItemList = shop.Value.Item1,
                        TimeLeft = -1,
                        ReloadOffset = reloadOffset,
                        Rotate = rotate
                    };

                    m.Move(loc.X + .5f, loc.Y + .5f);
                    EnterWorld(m);

                    if (shopItems.Count <= 0)
                        shopItems.AddRange(shop.Value.Item1);
                }
            }
        }

        private void InitCoreTasks()
        {
            void tick()
            {
                var mre = new ManualResetEvent(false);

                do
                {
                    Tick();
                    mre.WaitOne((int)CoreConstant.worldTickMs);
                } while (Manager != null && !Deleted);
            }
            void tickLogic()
            {
                var mre = new ManualResetEvent(false);

                do
                {
                    TickLogic();
                    mre.WaitOne((int)CoreConstant.worldLogicTickMs);
                } while (Manager != null && !Deleted);
            }
            void tickTimers()
            {
                var mre = new ManualResetEvent(false);

                do
                {
                    TickTimers();
                    mre.WaitOne((int)CoreConstant.worldTickMs);
                } while (Manager != null && !Deleted);
            }

            Task.Factory.StartNew(tick, TaskCreationOptions.LongRunning);
            Task.Factory.StartNew(tickLogic, TaskCreationOptions.LongRunning);
            Task.Factory.StartNew(tickTimers, TaskCreationOptions.LongRunning);
        }

        protected void LoadMap(string embeddedResource)
        {
            if (embeddedResource == null)
                return;
            var stream = typeof(RealmManager).Assembly.GetManifestResourceStream(embeddedResource);
            if (stream == null)
                throw new ArgumentException("Resource not found", nameof(embeddedResource));

            FromWorldMap(stream);
        }

        public bool Delete()
        {
            if (Monitor.TryEnter(_deleteLock, new TimeSpan(0, 0, 1)))
                try
                {
                    if (Players.Count > 0) return false;

                    Deleted = true;

                    Manager.RemoveWorld(this);
                    Id = 0;

                    DisposeEntities(Players);
                    DisposeEntities(Enemies);
                    DisposeEntities(Projectiles);
                    DisposeEntities(StaticObjects);
                    DisposeEntities(Allies);

                    Players = null;
                    Enemies = null;
                    Projectiles = null;
                    StaticObjects = null;
                    Allies = null;
                    Timers = new List<WorldTimer>();

                    return true;
                }
                finally { Monitor.Exit(_deleteLock); }

            return default;
        }

        public void DisposeEntities<T, TU>(ConcurrentDictionary<T, TU> dictionary)
        {
            var entities = dictionary.Values.ToArray();
            foreach (var entity in entities)
                (entity as Entity).Dispose();
        }

        protected void FromDungeonGen(int seed, DungeonTemplate template)
        {
            var gen = new Generator(seed, template);
            gen.Generate();
            var ras = new Rasterizer(seed, gen.ExportGraph());
            ras.Rasterize();
            var dTiles = ras.ExportMap();

            if (Map == null)
            {
                Map = new Wmap(Manager.Resources.GameData);
                Interlocked.Add(ref _entityInc, Map.Load(dTiles, _entityInc));
                if (Blocking == 3)
                    Sight.CalcRegionBlocks(Map);
            }
            else
                Map.ResetTiles();

            InitMap();
        }

        protected void FromWorldMap(Stream dat)
        {
            if (Map == null)
            {
                Map = new Wmap(Manager.Resources.GameData);
                Interlocked.Add(ref _entityInc, Map.Load(dat, _entityInc));
                if (Blocking == 3)
                    Sight.CalcRegionBlocks(Map);
            }
            else
                Map.ResetTiles();

            InitMap();
        }

        private void InitMap()
        {
            int w = Map.Width, h = Map.Height;
            EnemiesCollision = new CollisionMap<Entity>(0, w, h);
            PlayersCollision = new CollisionMap<Entity>(1, w, h);

            Timers.Clear();
            Projectiles.Clear();
            StaticObjects.Clear();
            Enemies.Clear();
            Players.Clear();
            Allies.Clear();
            Quests.Clear();

            foreach (var i in Map.InstantiateEntities(Manager))
                EnterWorld(i);
        }

        public virtual int EnterWorld(Entity entity)
        {
            switch (entity)
            {
                case Player player:
                    if (DateTime.UtcNow.Hour == 730)
                    {
                        player.SendWorldBossOne("Heros of UT Reborn, the World Boss's gates has been unsealed!");
                    }
                    player.Id = GetNextEntityId();
                    player.Init(this);
                    Players.TryAdd(player.Id, player);
                    PlayersCollision.Insert(player);
                    Interlocked.Increment(ref _totalConnects);
                    break;

                case Enemy enemy:
                    enemy.Id = GetNextEntityId();
                    enemy.Init(this);
                    Enemies.TryAdd(enemy.Id, enemy);
                    EnemiesCollision.Insert(enemy);
                    if (enemy.ObjectDesc.Quest)
                        Quests.TryAdd(enemy.Id, enemy);
                    break;

                case Projectile prj:
                    prj.Init(this);
                    Projectiles[new Tuple<int, byte>(prj.ProjectileOwner.Self.Id, prj.ProjectileId)] = prj;
                    break;

                case StaticObject obj:
                    obj.Id = GetNextEntityId();
                    obj.Init(this);
                    StaticObjects.TryAdd(obj.Id, obj);
                    EnemiesCollision.Insert(obj);
                    break;

                case Ally obj:
                    obj.Id = GetNextEntityId();
                    obj.Init(this);
                    Allies.TryAdd(obj.Id, obj);
                    PlayersCollision.Insert(obj);
                    break;
            }
            return entity.Id;
        }

        public static readonly string EVENT_MESSAGE = $"[Server Time: {DateTime.Now.ToString("MM/dd/yyyy")}]The World Boss has spawned! Go to the World Boss Portal to fight!";

        public virtual void LeaveWorld(Entity entity)
        {
            switch (entity)
            {
                case Player _:
                    {
                        Players.TryRemove(entity.Id, out var dummy);
                        PlayersCollision.Remove(entity);

                        // if in trade, cancel it...
                        if (dummy.tradeTarget != null)
                            dummy.CancelTrade();

                        if (dummy.Pet != null)
                            LeaveWorld(dummy.Pet);
                        break;
                    }
                case Enemy _:
                    {
                        Enemies.TryRemove(entity.Id, out var dummy);
                        EnemiesCollision.Remove(entity);
                        if (entity.ObjectDesc.Quest)
                            Quests.TryRemove(entity.Id, out dummy);
                        break;
                    }
                case Projectile prj:
                    Projectiles.TryRemove(new Tuple<int, byte>(prj.ProjectileOwner.Self.Id, prj.ProjectileId), out prj);
                    break;

                case StaticObject _:
                    {
                        StaticObjects.TryRemove(entity.Id, out var dummy);

                        if (entity.ObjectDesc?.BlocksSight == true)
                        {
                            if (Blocking == 3)
                                Sight.UpdateRegion(Map, (int)entity.X, (int)entity.Y);

                            foreach (var plr in Players.Values
                                    .Where(p => MathsUtils.DistSqr(p.X, p.Y, entity.X, entity.Y) < Player.RadiusSqr))
                                plr.Sight.UpdateCount++;
                        }

                        EnemiesCollision.Remove(entity);
                        break;
                    }

                case Ally _:
                    {
                        Allies.TryRemove(entity.Id, out var dummy);
                        PlayersCollision.Remove(entity);
                        break;
                    }
            }
            entity.Dispose();
        }

        public int GetNextEntityId()
        {
            return Interlocked.Increment(ref _entityInc);
        }

        public Entity GetEntity(int id)
        {
            if (Players.TryGetValue(id, out var plr)) return plr;
            if (Enemies.TryGetValue(id, out var enemy)) return enemy;
            if (StaticObjects.TryGetValue(id, out var obj)) return obj;
            return Allies.TryGetValue(id, out var ally) ? ally : null;
        }

        public Player GetUniqueNamedPlayer(string name)
        {
            if (Database.GuestNames.Contains(name))
                return null;

            foreach (var i in Players)
            {
                if (i.Value.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (!i.Value.NameChosen && !(this is Test))
                        Manager.Database.ReloadAccount(i.Value.Client.Account);

                    if (i.Value.Client.Account.NameChosen)
                        return i.Value;

                    break;
                }
            }

            return null;
        }

        public bool IsPassable(double x, double y, bool spawning = false)
        {
            var x_ = (int)x;
            var y_ = (int)y;

            if (!Map.Contains(x_, y_))
                return false;

            var tile = Map[x_, y_];

            if (tile.TileDesc.NoWalk)
                return false;

            if (tile.ObjType != 0 && tile.ObjDesc != null)
            {
                if (tile.ObjDesc.FullOccupy || tile.ObjDesc.EnemyOccupySquare || (spawning && tile.ObjDesc.OccupySquare))
                    return false;
            }

            return true;
        }

        public void BroadcastPacket(Packet pkt, Player exclude)
        {
            foreach (var i in Players)
                if (i.Value != exclude)
                    i.Value.Client.SendPacket(pkt);
        }

        public void BroadcastPackets(IEnumerable<Packet> pkts, Player exclude)
        {
            foreach (var i in Players)
                if (i.Value != exclude)
                    i.Value.Client.SendPackets(pkts);
        }

        public void BroadcastPacketNearby(Packet pkt, Entity entity, Player exclude = null)
        {
            if (exclude == null)
                BroadcastPacketConditional(pkt, p => p.DistSqr(entity) < Player.RadiusSqr);
            else
                BroadcastPacketConditional(pkt, p => p != exclude && p.DistSqr(entity) < Player.RadiusSqr);
        }

        public void BroadcastPacketPrivate(Packet pkt, Player entity)
        {
            entity.Client.SendPacket(pkt);
        }

        public void BroadcastPacketNearby(Packet pkt, Position pos)
        {
            BroadcastPacketConditional(pkt, p => MathsUtils.DistSqr(p.X, p.Y, pos.X, pos.Y) < Player.RadiusSqr);
        }

        public void BroadcastPacketConditional(Packet pkt, Predicate<Player> cond)
        {
            foreach (var i in Players)
                if (cond(i.Value))
                    i.Value.Client.SendPacket(pkt);
        }

        public void WorldAnnouncement(string msg)
        {
            var announcement = string.Concat("<ANNOUNCEMENT> ", msg);
            foreach (var i in Players)
                i.Value.SendInfo(announcement);
        }

        public void QuakeToWorld(World newWorld)
        {
            if (!Persist || Id == Realm) Closed = true;

            BroadcastPacket(new ShowEffect { EffectType = EffectType.Earthquake }, null);
            Timers.Add(new WorldTimer(8000, (w) =>
            {
                if (w == null || w.Deleted || newWorld == null || newWorld.Deleted) return;

                var rcpNotPaused = new Reconnect
                {
                    Host = "",
                    Port = 2050,
                    GameId = newWorld.Id,
                    Name = newWorld.SBName
                };
                var rcpPaused = new Reconnect
                {
                    Host = "",
                    Port = 2050,
                    GameId = Nexus,
                    Name = "Nexus"
                };

                foreach (var plr in w.Players.Values.Where(p => p != null))
                    if (plr.Client != null)
                        plr.Client.Reconnect(plr.HasConditionEffect(ConditionEffects.Paused) && plr.SpectateTarget == null ?
                            rcpPaused :
                            rcpNotPaused);
            }));

            if (!Persist)
                Timers.Add(new WorldTimer(20000, (w) =>
                {
                    if (w == null || w.Deleted) return;

                    foreach (var plr in w.Players.Values.Where(p => p != null))
                        if (plr.Client != null)
                            plr.Client.Disconnect();
                }));
        }

        public void ChatReceived(Player player, string text)
        {
            foreach (var en in Enemies) en.Value.OnChatTextReceived(player, text);
            foreach (var en in StaticObjects) en.Value.OnChatTextReceived(player, text);
            foreach (var en in Allies) en.Value.OnChatTextReceived(player, text);
        }

        public Position? GetRegionPosition(TileRegion region)
        {
            if (Map.Regions.All(t => t.Value != region)) return null;

            var reg = Map.Regions.Single(t => t.Value == region);

            return new Position { X = reg.Key.X, Y = reg.Key.Y };
        }

        private int elapsedCondition
        {
            get
            {
                return (int)(Name == "Vault" ?
                    CoreConstant.worldCloseInMilliseconds * 0.25 :
                    CoreConstant.worldCloseInMilliseconds);
            }
        }

        /// <summary>
        /// If Tick is overrided and you make a call to this function make sure not to do anything after the call (or at least check)
        /// as it is possible for the world to have been removed at that point.
        /// </summary>
        public virtual void Tick()
        {
            if (Deleted) return;

            try
            {
                _elapsedTime += (int)CoreConstant.worldTickMs;

                if (Limbo) return;
                if (!Persist && _elapsedTime >= elapsedCondition && Players.Count <= 0)
                {
                    Delete();
                    return;
                }

                foreach (var i in Players) i.Value.Tick();
                foreach (var i in Projectiles) i.Value.Tick();
            }
            catch (Exception e)
            {
                var msg = e.Message + "\n" + e.StackTrace;
                Log.Error(msg);
            }
        }

        public void TickLogic()
        {
            if (Monitor.TryEnter(_deleteLock, new TimeSpan(0, 0, 1)))
                try
                {
                    if (Deleted) return;
                    if (EnemiesCollision != null)
                    {
                        foreach (var i in EnemiesCollision.GetActiveChunks(PlayersCollision))
                            try { i.Tick(); }
                            catch { continue; }
                        foreach (var i in Allies)
                            try { i.Value.Tick(); }
                            catch { continue; }
                    }
                    else
                    {
                        foreach (var i in Enemies)
                            try { i.Value.Tick(); }
                            catch { continue; }
                        foreach (var i in StaticObjects)
                            try { i.Value.Tick(); }
                            catch { continue; }
                    }
                }
                finally { Monitor.Exit(_deleteLock); }
        }

        public void TickTimers()
        {
            for (var i = 0; i < Timers.Count; i++)
            {
                try
                {
                    if (Timers[i] == null) continue;

                    var timer = Timers[i].Tick(this);

                    if (timer.HasValue && timer == false) continue;

                    Timers.RemoveAt(i);

                    i--;
                }
                catch (Exception e) { Log.Error(e); }
            }
        }

        public Projectile GetProjectile(int objectId, int bulletId)
        {
            var entity = GetEntity(objectId);
            return entity != null ?
                ((IProjectileOwner)entity).Projectiles[bulletId] :
                Projectiles.SingleOrDefault(p =>
                   p.Value.ProjectileOwner.Self.Id == objectId &&
                   p.Value.ProjectileId == bulletId).Value;
        }
    }
}
