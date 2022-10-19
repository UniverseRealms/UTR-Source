using common;
using common.resources;
using System;
using System.Linq;
using wServer.networking.packets.outgoing;
using wServer.realm.entities;
using wServer.realm.worlds;
using wServer.realm.worlds.logic;

namespace wServer.realm.commands
{
    public abstract class HeadModCommands
    {
        #region Utility

        internal class OryxSayCommand : Command
        {
            public OryxSayCommand() : base("oryxSay", Perms.HeadMod, alias: "osay")
            {
            }

            protected override bool Process(Player player, string args)
            {
                player.Manager.Chat.Oryx(player.Owner, args);
                return true;
            }
        }
        internal class AnnounceCommand : Command
        {
            public AnnounceCommand() : base("announce", Perms.HeadMod)
            {
            }

            protected override bool Process(Player player, string args)
            {
                player.Manager.Chat.Announce(args);
                return true;
            }
        }

        internal class SummonCommand : Command
        {
            public SummonCommand() : base("summon", Perms.HeadMod)
            {
            }

            protected override bool Process(Player player, string args)
            {
                foreach (var i in player.Owner.Players)
                {
                    if (i.Value.Name.EqualsIgnoreCase(args))
                    {
                        // probably someone hidden doesn't want to be summoned, so we leave it as before here
                        if (i.Value.HasConditionEffect(ConditionEffects.Hidden))
                            break;

                        i.Value.Teleport(player.Id, true);
                        i.Value.SendInfo($"You've been summoned by {player.Name}.");
                        player.SendInfo("Player summoned!");
                        return true;
                    }
                }
                player.SendError($"Player '{args}' could not be found!");
                return false;
            }
        }

        internal class SummonAllCommand : Command
        {
            public SummonAllCommand() : base("summonall", Perms.HeadMod)
            {
            }

            protected override bool Process(Player player, string args)
            {
                foreach (var i in player.Owner.Players)
                {
                    i.Value.Teleport(player.Id, true);
                    i.Value.SendInfo($"You have been summoned by <{player.Name}>.");
                }

                player.SendInfo("All players summoned!");
                return true;
            }
        }

        internal class ClearGravesCommand : Command
        {
            public ClearGravesCommand() : base("cleargraves", Perms.HeadMod, alias: "cgraves")
            {
            }

            protected override bool Process(Player player, string args)
            {
                var removed = 0;
                foreach (var entity in player.Owner.StaticObjects.Values)
                {
                    if (entity is Container || entity.ObjectDesc == null)
                        continue;

                    if (entity.ObjectDesc.ObjectId.StartsWith("Gravestone") && entity.Dist(player) < 15)
                    {
                        player.Owner.LeaveWorld(entity);
                        removed++;
                    }
                }
                player.SendInfo($"{removed} gravestones removed!");
                return true;
            }
        }

        internal class SelfCommand : Command
        {
            public SelfCommand() : base("self", permLevel: 40)
            {
            }

            protected override bool Process(Player player, string name)
            {
                if (player.SpectateTarget != null)
                {
                    player.SpectateTarget.FocusLost -= player.ResetFocus;
                    player.SpectateTarget.Controller = null;
                }

                player.SpectateTarget = null;
                player.Sight.UpdateCount++;
                player.Owner.Timers.Add(new WorldTimer(3000, (w) =>
                {
                    if (w == null || w.Deleted || player == null) return;
                    if (player.SpectateTarget == null) player.ApplyConditionEffect(ConditionEffectIndex.Paused, 0);
                }));
                player.Client.SendPacket(new SetFocus()
                {
                    ObjectId = player.Id
                });
                return true;
            }
        }

        internal class SpectateCommand : Command
        {
            public SpectateCommand() : base("spectate", Perms.Mod)
            {
            }

            protected override bool Process(Player player, string name)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    player.SendError("Usage: /spectate <player name>");
                    return false;
                }

                var owner = player.Owner;
                if (!player.Client.Account.Admin && owner != null &&
                    (owner is Arena || owner is ArenaSolo || owner is DeathArena))
                {
                    player.SendInfo("Can't spectate in Arenas. (Temporary solution till we get spectate working across maps.)");
                    return false;
                }

                var target = player.Owner.Players.Values
                    .SingleOrDefault(p => p.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) && p.CanBeSeenBy(player));

                if (target == null)
                {
                    player.SendError("Player not found. Note: Target player must be on the same map.");
                    return false;
                }

                if (!player.Client.Account.Admin &&
                    player.Owner.EnemiesCollision.HitTest(player.X, player.Y, 8).OfType<Enemy>().Any())
                {
                    player.SendError("Enemies cannot be nearby when initiating spectator mode.");
                    return false;
                }

                if (player.SpectateTarget != null)
                {
                    player.SpectateTarget.FocusLost -= player.ResetFocus;
                    player.SpectateTarget.Controller = null;
                }

                if (player != target)
                {
                    player.ApplyConditionEffect(ConditionEffectIndex.Paused);
                    target.FocusLost += player.ResetFocus;
                    player.SpectateTarget = target;
                }
                else
                {
                    player.SpectateTarget = null;
                    player.Owner.Timers.Add(new WorldTimer(3000, (w) =>
                    {
                        if (w == null || w.Deleted || player == null) return;
                        if (player.SpectateTarget == null) player.ApplyConditionEffect(ConditionEffectIndex.Paused, 0);
                    }));
                }

                player.Client.SendPacket(new SetFocus()
                {
                    ObjectId = target.Id
                });

                player.SendInfo($"Now spectating {target.Name}. Use the /self command to exit.");
                return true;
            }
        }

        internal class GuildRankCommand : Command
        {
            public GuildRankCommand() : base("grank", Perms.HeadMod)
            {
            }

            protected override bool Process(Player player, string args)
            {
                if (player == null)
                    return false;

                // verify argument
                var index = args.IndexOf(' ');
                if (string.IsNullOrWhiteSpace(args) || index == -1)
                {
                    player.SendInfo("Usage: /grank <player name> <guild rank>");
                    return false;
                }

                // get command args
                var playerName = args.Substring(0, index);
                var rank = args.Substring(index + 1).IsInt() ? args.Substring(index + 1).ToInt32() : RankNumberFromName(args.Substring(index + 1));
                if (rank == -1)
                {
                    player.SendError("Unknown rank!");
                    return false;
                }
                else if (rank % 10 != 0)
                {
                    player.SendError("Valid ranks are multiples of 10!");
                    return false;
                }

                // get player account
                if (Database.GuestNames.Contains(playerName, StringComparer.InvariantCultureIgnoreCase))
                {
                    player.SendError("Cannot rank the unnamed...");
                    return false;
                }
                var id = player.Manager.Database.ResolveId(playerName);
                var acc = player.Manager.Database.GetAccount(id);
                if (id == 0 || acc == null)
                {
                    player.SendError("Account not found!");
                    return false;
                }

                // change rank
                acc.GuildRank = rank;
                acc.FlushAsync();

                // send out success notifications
                player.SendInfo($"You changed the guildrank of player {acc.Name} to {rank}.");
                var target = player.Manager.Clients.Keys.SingleOrDefault(p => p.Account.AccountId == acc.AccountId);
                if (target?.Player == null) return true;
                target.Player.GuildRank = rank;
                target.Player.SendInfo("Your guild rank was changed");
                return true;
            }

            private int RankNumberFromName(string val)
            {
                switch (val.ToLower())
                {
                    case "initiate":
                        return 0;

                    case "member":
                        return 10;

                    case "officer":
                        return 20;

                    case "leader":
                        return 30;

                    case "founder":
                        return 40;
                }
                return -1;
            }
        }

        internal class RenameCommand : Command
        {
            public RenameCommand() : base("rename", Perms.Mod)
            {
            }

            protected override bool Process(Player player, string args)
            {
                var index = args.IndexOf(' ');
                if (string.IsNullOrWhiteSpace(args) || index == -1)
                {
                    player.SendInfo("Usage: /rename <player name> <new player name>");
                    return false;
                }

                var playerName = args.Substring(0, index);
                var newPlayerName = args.Substring(index + 1);

                var id = player.Manager.Database.ResolveId(playerName);
                if (id == 0)
                {
                    player.SendError("Player account not found!");
                    return false;
                }

                if (newPlayerName.Length < 3 || newPlayerName.Length > 15 || !newPlayerName.All(char.IsLetter) ||
                    Database.GuestNames.Contains(newPlayerName, StringComparer.InvariantCultureIgnoreCase))
                {
                    player.SendError("New name is invalid. Must be between 3-15 char long and contain only letters.");
                    return false;
                }

                string lockToken = null;
                var key = Database.NAME_LOCK;
                var db = player.Manager.Database;

                try
                {
                    while ((lockToken = db.AcquireLock(key)) == null) ;

                    if (db.Conn.HashExists("names", newPlayerName.ToUpperInvariant()))
                    {
                        player.SendError("Name already taken");
                        return false;
                    }

                    var acc = db.GetAccount(id);
                    if (acc == null)
                    {
                        player.SendError("Account doesn't exist.");
                        return false;
                    }

                    using (var l = db.Lock(acc))
                        if (db.LockOk(l))
                        {
                            while (!db.RenameIGN(acc, newPlayerName, lockToken)) ;
                            player.SendInfo("Rename successful.");
                            Program.DL.LogToDiscord(WebHooks.ModCommand, "Rename Log", $"Rename Log: [{player.Name}] has renamed [{playerName}] to [{newPlayerName}]");
                        }
                        else
                            player.SendError("Account in use.");
                }
                finally
                {
                    if (lockToken != null)
                        db.ReleaseLock(key, lockToken);
                }

                return true;
            }
        }
        internal class AddVaults : Command
        {
            public AddVaults() : base("addvaults", permLevel: 100) { }

            protected override bool Process(Player player, string args)
            {
                player.Manager.Database.AddVaults();
                return true;
            }
        }
        #endregion Utility

        #region Refunds

        internal class LinkCommand : Command
        {
            public LinkCommand() : base("link", Perms.HeadMod)
            {
            }

            protected override bool Process(Player player, string args)
            {
                if (player?.Owner == null)
                    return false;

                var world = player.Owner;
                if (world.Id < 0)
                {
                    player.SendError("Forbidden.");
                    return false;
                }

                if (!player.Manager.Monitor.AddPortal(world.Id))
                {
                    player.SendError("Link already exists.");
                    return false;
                }

                return true;
            }
        }

        internal class UnLinkCommand : Command
        {
            public UnLinkCommand() : base("unlink", Perms.HeadMod)
            {
            }

            protected override bool Process(Player player, string args)
            {
                if (player?.Owner == null)
                    return false;

                var world = player.Owner;
                if (world.Id < 0)
                {
                    player.SendError("Forbidden.");
                    return false;
                }

                if (!player.Manager.Monitor.RemovePortal(player.Owner.Id))
                    player.SendError("Link not found.");
                else
                    player.SendInfo("Link removed.");

                return true;
            }
        }

        internal class GiftCommand : Command
        {
            public GiftCommand() : base("gift", Perms.Dev)
            {
            }

            protected override bool Process(Player player, string args)
            {
                if (player == null)
                    return false;

                var manager = player.Manager;

                // verify argument
                var index = args.IndexOf(' ');
                if (string.IsNullOrWhiteSpace(args) || index == -1)
                {
                    player.SendInfo("Usage: /gift <player name> <item name>");
                    return false;
                }

                // get command args
                var playerName = args.Substring(0, index);
                var item = GetItem(player, args.Substring(index + 1));
                if (item == null)
                {
                    return false;
                }

                // get player account
                if (Database.GuestNames.Contains(playerName, StringComparer.InvariantCultureIgnoreCase))
                {
                    player.SendError("Cannot gift the unnamed...");
                    return false;
                }
                var id = manager.Database.ResolveId(playerName);
                var acc = manager.Database.GetAccount(id);
                if (id == 0 || acc == null)
                {
                    player.SendError("Account not found!");
                    return false;
                }

                // add gift
                var result = player.Manager.Database.AddGift(acc, item.ObjectType);
                if (!result)
                {
                    player.SendError("Gift not added. Something happened with the adding process.");
                    return false;
                }

                Program.DL.LogToDiscord(WebHooks.ModCommand, "Gift Log", $"Gift Log: [{player.Name}] has gifted [{playerName}] a [{item.DisplayName}]");

                // send out success notifications
                player.SendInfoFormat("You gifted {0} one {1}.", acc.Name, item.DisplayName);
                var gifted = player.Manager.Clients.Keys
                    .SingleOrDefault(p => p.Account.AccountId == acc.AccountId);
                gifted?.Player?.SendInfoFormat(
                    "You received a gift from {0}. Enjoy your {1}.",
                    player.Name,
                    item.DisplayName);
                return true;
            }

            private Item GetItem(Player player, string itemName)
            {
                var gameData = player.Manager.Resources.GameData;

                // allow both DisplayId and Id for query
                if (!gameData.DisplayIdToObjectType.TryGetValue(itemName, out ushort objType))
                {
                    if (!gameData.IdToObjectType.TryGetValue(itemName, out objType))
                        player.SendError("Unknown item type!");
                    return null;
                }

                if (!gameData.Items.ContainsKey(objType))
                {
                    player.SendError("Not an item!");
                    return null;
                }

                return gameData.Items[objType];
            }
        }

        internal class CloseRealmCommand : Command
        {
            public CloseRealmCommand() : base("closerealm", Perms.HeadMod)
            {
            }

            protected override bool Process(Player player, string args)
            {
                if (!(player.Manager.Worlds[World.Realm] is Realm gw))
                {
                    player.SendError("An undefined error occurred.");
                    return false;
                }

                if (gw.IsClosing())
                {
                    player.SendError("Realm already closing.");
                    return false;
                }

                gw.CloseRealm();
                return true;
            }
        }

        #endregion Refunds

        #region Events

        internal class SArenaCommand : Command
        {
            public SArenaCommand() : base("superarena", Perms.HeadMod, alias: "adar")
            {
            }

            protected override bool Process(Player player, string args)
            {
                var entity = Entity.Resolve(player.Manager, 0x47a9);
                var we = player.Manager.GetWorld(player.Owner.Id); //can't use Owner here, as it goes out of scope
                var TimeoutTime = player.Manager.Resources.GameData.Portals[0x47a9].Timeout;

                entity.Move(player.X, player.Y);
                we.EnterWorld(entity);

                var packet = new Text
                {
                    BubbleTime = 0,
                    NumStars = -1,
                    TextColor = 0xFF00FF,
                    Name = "",
                    Txt = "An 'Admin Arena' has been opened by <" + player.Name + ">"
                };
                player.Owner.BroadcastPacket(packet, null);

                we.Timers.Add(new WorldTimer(TimeoutTime * 1000, (w) =>
                {
                    if (w == null || w.Deleted || entity == null) return;

                    try { w.LeaveWorld(entity); }
                    catch (Exception ex) { Program.Debug(typeof(SArenaCommand), $"Couldn't despawn portal.\n{ex.ToString()}", true); }
                }));
                return true;
            }
        }

        internal class SpawnEvent : Command
        {
            public SpawnEvent() : base("spawnevent", Perms.HeadMod)
            {
            }

            protected override bool Process(Player player, string args)
            {
                if (player.Owner is Nexus)
                {
                    return false;
                }
                Entity entity = Entity.Resolve(player.Manager, 0x4083);
                entity.Move(player.X, player.Y + 5);
                //Announcement
                var EventConnected = new Text()
                {
                    Name = "E.V.E.N.T System",
                    NumStars = -1,
                    Txt = $"An Event Master has Spawned!",
                    BubbleTime = 0,
                    NameColor = 0xbe118e,
                    TextColor = 0xbe3910
                };
                player.Manager.GlobalBroadcast(EventConnected);
                //Announcement
                player.SendInfo("Spawning Event Master in 3 seconds");
                entity.ApplyConditionEffect(new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Stasis & ConditionEffectIndex.Invulnerable,
                    DurationMS = 5000
                });
                player.Owner.Timers.Add(new WorldTimer(3000, (w) =>
                {
                    if (w == null || w.Deleted || entity == null) return;

                    w.EnterWorld(entity);
                }));
                return true;
            }
        }

        #endregion Events
    }
}
