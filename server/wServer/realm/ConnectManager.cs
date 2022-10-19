using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using wServer.networking;
using wServer.networking.packets;
using wServer.networking.packets.outgoing;
using wServer.realm.worlds;
using wServer.realm.worlds.logic;
using File = System.IO.File;

namespace wServer.realm
{
    public class ReconInfo
    {
        public readonly int Destination;
        public readonly byte[] Key;
        public DateTime Timeout;

        public ReconInfo(int dest, byte[] key, DateTime timeout)
        {
            Destination = dest;
            Key = key;
            Timeout = timeout;
        }
    }

    public class ConnectManager
    {
        private const int ReconTTL = 90; // in seconds
        private const int ConnectingTTL = 15; // in seconds

        private readonly RealmManager _manager;
        private readonly int _maxPlayerCount;
        private readonly int _maxPlayerCountWithPriority;
        private readonly ConnectionQueue _queue;
        private readonly ConcurrentDictionary<int, ReconInfo> _recon;
        private readonly ConcurrentDictionary<Client, DateTime> _connecting;
        private long _lastTick;

        public ConnectManager(RealmManager manager, int maxPlayerCount, int maxPlayerCountWithPriority)
        {
            _manager = manager;
            _maxPlayerCount = maxPlayerCount;
            _maxPlayerCountWithPriority = maxPlayerCountWithPriority;
            _recon = new ConcurrentDictionary<int, ReconInfo>();
            _queue = new ConnectionQueue();
            _connecting = new ConcurrentDictionary<Client, DateTime>();
        }

        public void Add(ConInfo conInfo)
        {
            // instantly connect reconnecting clients
            if (conInfo.Reconnecting)
            {
                //Program.Debug(typeof(ConnectManager), "Instantly connect reconnecting client.");
                Connect(conInfo);
                return;
            }

            // don't use queue for ranked players
            if (conInfo.Account.Rank > 0)
            {
                if (GetPlayerCount() < _maxPlayerCountWithPriority)
                {
                    Connect(conInfo);
                    return;
                }

                conInfo.Client.SendFailure("Server is full.");
                return;
            }

            if (!_queue.Add(conInfo))
            {
                conInfo.Client.SendFailure("Account already in queue.", Failure.MessageWithDisconnect);
                return;
            }

            conInfo.Client.State = ProtocolState.Queued;

            var position = _queue.Position(conInfo);

            if (_maxPlayerCount - GetPlayerCount() >= position)
                return;

            // send server full
            conInfo.Client.SendPacket(new ServerFull()
            {
                Position = position,
                Count = _queue.Count
            });
        }

        public void AddReconnect(Client client, Reconnect rcp)
        {
            if (rcp == null)
            {
                Program.Debug(typeof(ConnectManager), "RCP is null.");
                return;
            }

            var rInfo = new ReconInfo(rcp.GameId, rcp.Key, DateTime.Now.AddSeconds(ReconTTL));

            if (_recon.TryAdd(client.Account.AccountId, rInfo)) {}
            //    Program.Debug(typeof(ConnectManager), $"Adding new key into reconnect dictionary: [gameId: {rcp.GameId}, key: {rcp.Key}, timeout: {rInfo.Timeout}, ReconTTL: {ReconTTL}]");
        }

        public void Tick()
        {
            _queue.KeepAlive();

            while(QueueLength() > 0)
                if (GetPlayerCount() < _maxPlayerCount)
                    Connect(_queue.Remove());
                else
                    break;

            if (_manager.Core.getTotalTickCount() - _lastTick > 5000)
            {
                _lastTick = _manager.Core.getTotalTickCount();

                var dateTime = DateTime.Now;

                // process reconnect timeouts
                foreach (var r in _recon.Where(r => DateTime.Compare(r.Value.Timeout, dateTime) < 0))
                    _recon.TryRemove(r.Key, out ReconInfo ignored);

                // process connecting timeouts
                // for those that go through the connection process but never send a Create or Load packet
                foreach (var c in _connecting.Where(c => DateTime.Compare(c.Value, dateTime) < 0))
                    _connecting.TryRemove(c.Key, out DateTime ignored);
            }
        }

        public int GetPlayerCount() => _manager.Clients.Count + _recon.Count;

        private void Connect(ConInfo conInfo)
        {
            var invalidWorld = true;
            var client = conInfo.Client;
            var acc = conInfo.Account;

            // configure override
            if (acc.Admin && acc.AccountIdOverride != 0)
            {
                var accOverride = client.Manager.Database.GetAccount(acc.AccountIdOverride);
                if (accOverride == null)
                {
                    client.SendPacket(new Text()
                    {
                        BubbleTime = 0,
                        NumStars = -1,
                        Name = "*Error*",
                        Txt = "Account does not exist."
                    });
                }
                else
                {
                    accOverride.AccountIdOverrider = acc.AccountId;
                    acc = accOverride;
                }
            }

            var gameId = conInfo.GameId;

            if (_recon.ContainsKey(acc.AccountId))
            {
                invalidWorld = false;
                gameId = _recon[acc.AccountId].Destination;
            }

            //Program.Debug(typeof(ConnectManager), $"Connecting to gameId '{gameId}'");

            if (conInfo.Reconnecting)
            {
                if (!_recon.TryRemove(acc.AccountId, out ReconInfo rInfo))
                {
                    client.SendFailure("Invalid reconnect.", Failure.MessageWithDisconnect);
                    return;
                }
                //else
                //    Program.Debug(typeof(ConnectManager), "Removed key from reconnect dictionary with success!");

                if (!gameId.Equals(rInfo.Destination))
                {
                    client.SendFailure("Invalid reconnect destination.", Failure.MessageWithDisconnect);
                    return;
                }

                if (!conInfo.Key.SequenceEqual(rInfo.Key))
                {
                    client.SendFailure("Invalid reconnect key.", Failure.MessageWithDisconnect);
                    return;
                }
            }
            else
            {
                if (invalidWorld)
                {
                    //Program.Debug(typeof(ConnectManager), "Connect method to regular gameId (Nexus world instance).");

                    if (gameId != World.Test)
                        gameId = World.Nexus;
                }
            }

            if (!client.Manager.Database.AcquireLock(acc))
            {
                // disconnect current connected client (if any)
                var otherClients = client.Manager.Clients.Keys
                    .Where(c => c == client || c.Account != null && (c.Account.AccountId == acc.AccountId || c.Account.DiscordId != null && c.Account.DiscordId == acc.DiscordId));
                foreach (var otherClient in otherClients)
                    otherClient.Disconnect("Other client disconnect.");

                // try again...
                if (!client.Manager.Database.AcquireLock(acc))
                {
                    client.SendFailure($"Account in use ({client.Manager.Database.GetLockTime(acc)?.ToString("%s")} seconds until timeout)");
                    return;
                }
            }

            acc.Reload(); // make sure we have the latest data
            client.Account = acc;

            // connect client to realm manager
            if (!client.Manager.TryConnect(client))
            {
                client.SendFailure("Failed to connect");
                return;
            }

            var world = client.Manager.GetWorld(gameId);

            //Program.Debug(typeof(ConnectManager), $"Connecting to world {world?.Name} [{world?.Id}]"); //make sure that "world" isn't null

            if (world == null || world.Deleted)
            {
                client.SendPacket(new Text()
                {
                    BubbleTime = 0,
                    NumStars = -1,
                    Name = "*Error*",
                    Txt = "World does not exist."
                });
                world = client.Manager.GetWorld(World.Nexus);
            }

            if (world is Test && !(world as Test).JsonLoaded && acc.Rank < 70) //client.Manager.Resources.Settings.EditorMinRank)
            {
                client.SendFailure("Only players with a rank of 70 and above can make test maps.",Failure.MessageWithDisconnect);
                return;
            }

            if (world.Limbo)
                world = world.GetInstance(client);

            if (acc.Rank >= 100)
                world.AllowedAccess(client);
            //else
            //    Program.Debug(typeof(ConnectManager), $"Receiving a new connect method with rank '{conInfo.Account.Rank}' (allowed access: {world.AllowedAccess(client)}).");

            if (!world.AllowedAccess(client))
            {
                if (!world.Persist && world.TotalConnects <= 0)
                    client.Manager.RemoveWorld(world);

                client.SendPacket(new Text()
                {
                    BubbleTime = 0,
                    NumStars = -1,
                    Name = "*Error*",
                    Txt = "Access denied"
                });

                if (!(world is Nexus))
                    world = client.Manager.GetWorld(World.Nexus);
                else
                {
                    client.Disconnect("Access denied.");
                    return;
                }
            }

            if (world is Test && !(world as Test).JsonLoaded)
            {
                // save map
                var mapFolder = $"{_manager.Config.serverSettings.logFolder}/maps";

                if (!Directory.Exists(mapFolder))
                    Directory.CreateDirectory(mapFolder);

                File.WriteAllText($"{mapFolder}/{acc.Name}_{DateTime.Now.Ticks}.jm", conInfo.MapInfo);

                (world as Test).LoadJson(conInfo.MapInfo);

                var dreamName = client.Account.Name.ToLower().EndsWith("s") ? client.Account.Name + "' Dream World" : client.Account.Name + "'s Dream World";

                world.SBName = dreamName;
                world.Name = dreamName;
                //client.Manager.Monitor.AddPortal(world.Id);
            }

            var seed = (uint)((long)Environment.TickCount * conInfo.GUID.GetHashCode()) % uint.MaxValue;
            client.Random = new wRandom(seed);
            client.TargetWorld = world.Id;

            var now = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

            if (acc.GuildId > 0 && now - acc.LastSeen > 1800)
                client.Manager.Chat.GuildAnnounce(acc, "<" + acc.Name + "> came online.");

            if (!acc.Hidden && acc.AccountIdOverrider == 0)
            {
                acc.RefreshLastSeen();
                acc.FlushAsync();
            }

            // send out map info
            var mapSize = Math.Max(world.Map.Width, world.Map.Height);

            client.SendPackets(new List<Packet>
            {
                new MapInfo()
                {
                    Music = world.Music,
                    Width = mapSize,
                    Height = mapSize,
                    Name = world.Name,
                    DisplayName = world.SBName,
                    Seed = seed,
                    Background = world.Background,
                    Difficulty = world.Difficulty,
                    AllowPlayerTeleport = world.AllowTeleport,
                    ShowDisplays = world.ShowDisplays,
                    ClientXML = Empty<string>.Array,//client.Manager.Resources.GameData.AdditionXml,
                    ExtraXML = world.ExtraXML
                },
                // send out account lock/ignore list
                new AccountList()
                {
                    AccountListId = 0, // locked list
                    AccountIds = client.Account.LockList.Select(i => i.ToString()).ToArray(),
                    LockAction = 1
                },
                new AccountList()
                {
                    AccountListId = 1, // ignore list
                    AccountIds = client.Account.IgnoreList.Select(i => i.ToString()).ToArray()
                }
            });
            client.State = ProtocolState.Handshaked;

            _connecting.TryAdd(client, DateTime.Now.AddSeconds(ConnectingTTL));

            if (_recon.ContainsKey(acc.AccountId) && !invalidWorld)
                _recon.TryRemove(acc.AccountId, out var recon); // remove last entry to validate the reconnect
        }

        public void ClientConnected(Client client)
        {
            _connecting.TryRemove(client, out DateTime to);

            // update PlayerInfo with world data
            var plrInfo = client.Manager.Clients[client];
            plrInfo.WorldInstance = client.Player.Owner.Id;
            plrInfo.WorldName = client.Player.Owner.Name;
        }

        public int QueueLength() => _queue.Count;
    }
}
