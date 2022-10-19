using common;
using common.resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using wServer.networking.packets.incoming;
using wServer.networking.packets.outgoing;
using wServer.realm.entities;
using wServer.realm.worlds;
using wServer.realm.worlds.logic;
using File = TagLib.File;
using MarketResult = wServer.realm.entities.MarketResult;

namespace wServer.realm.commands
{
    #region UtilityCommands

    internal class UptimeCommand : Command
    {
        public UptimeCommand() : base("uptime")
        {
        }

        protected override bool Process(Player player, string args)
        {
            var t = TimeSpan.FromMilliseconds(Program.Uptime.ElapsedMilliseconds);

            var answer = t.Days != 0
                ? $"{t.Days:D2} days, {t.Hours:D2} hours, {t.Minutes:D2} minutes"
                : $"{t.Hours:D2} hours, {t.Minutes:D2} minutes";

            player.SendInfo("The server has been up for: " + answer + ".");
            return true;
        }
    }

    internal class PositionCommand : Command
    {
        public PositionCommand() : base("pos", alias: "position")
        {
        }

        protected override bool Process(Player player, string args)
        {
            player.SendInfo("Current Position: " + (int)player.X + ", " + (int)player.Y);
            return true;
        }
    }

    internal class LefttoMaxCommand : Command
    {
        public LefttoMaxCommand() : base("lefttomax", alias: "l2m")
        {
        }

        protected override bool Process(Player player, string args)
        {
            var pd = player.Manager.Resources.GameData.Classes[player.ObjectType];
            var str = "";

            for (var i = 0; i < pd.Stats.Length; i++)
            {
                int l2m;
                if (i != 9)
                {
                    l2m = (player.AscensionEnabled ? pd.Stats[i].MaxValue + (i < 2 ? 50 : 10) : pd.Stats[i].MaxValue) - player.Stats.Base[i];
                }
                else
                {
                    l2m = (player.AscensionEnabled ? pd.Stats[9].MaxValue + 3 : pd.Stats[9].MaxValue) - player.Stats.Base[9];
                }


                if (l2m != 0) str += player.Stats.StatIndexToFullName(i) + ": "
                              + l2m + (i == pd.Stats.Length - 1 ? "" : ", ");

                if (i == pd.Stats.Length - 1) player.SendInfo(str);
            }
            return true;
        }
    }

    internal class GLandCommand : Command
    {
        public GLandCommand() : base("gland", alias: "glands")
        {
        }

        protected override bool Process(Player player, string args)
        {
            if (!(player.Owner is Realm))
            {
                player.SendError("This command requires you to be in realm first.");
                return false;
            }

            player.TeleportPosition(1480, 1050);
            return true;
        }
    }

    internal class WhoCommand : Command
    {
        public WhoCommand() : base("who")
        {
        }

        protected override bool Process(Player player, string args)
        {
            int count = 0;
            foreach (Player plr in player.Owner.Players.Values)
            {
                if (plr.HasConditionEffect(ConditionEffects.Hidden))
                    continue;

                count++;
            }
            player.SendInfo($"There are '{count}' people in your area.");
            return true;
        }
    }

    internal class OnlineCommand : Command
    {
        public OnlineCommand() : base("online")
        {
        }

        protected override bool Process(Player player, string args)
        {
            var servers = player.Manager.InterServer.GetServerList();
            var players =
                (from server in servers
                 from plr in server.playerList
                 where !plr.Hidden || player.Client.Account.Admin
                 select plr.Name)
                .ToArray();

            var sb = $"There are '{players.Length}' people online.";
            player.SendInfo(sb);
            return true;
        }
    }

    internal class ServerCommand : Command
    {
        public ServerCommand() : base("world")
        {
        }

        protected override bool Process(Player player, string args)
        {
            int count = 0;
            foreach (Player plr in player.Owner.Players.Values)
            {
                if (plr.HasConditionEffect(ConditionEffects.Hidden))
                    continue;

                count++;
            }
            player.SendInfo($"[{player.Owner.Id}] {player.Owner.GetDisplayName()} ({count} players)");
            return true;
        }
    }

    internal class WhereCommand : Command
    {
        public WhereCommand() : base("where")
        {
        }

        protected override bool Process(Player player, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                player.SendInfo("Usage: /where <player name>");
                return true;
            }

            var servers = player.Manager.InterServer.GetServerList();

            foreach (var server in servers)
                foreach (PlayerInfo plr in server.playerList)
                {
                    if (!plr.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) ||
                        plr.Hidden && !player.Client.Account.Admin)
                        continue;

                    player.SendInfo($"<{plr.Name}> is playing on '{server.name}' at '{plr.WorldName}'.");
                    return true;
                }

            var pId = player.Manager.Database.ResolveId(name);
            if (pId == 0)
            {
                player.SendInfo($"No player with the name <{name}>.");
                return true;
            }

            var acc = player.Manager.Database.GetAccount(pId, "lastSeen");
            if (acc.LastSeen == 0)
            {
                player.SendInfo($"<{name}> not online. Has not been seen since the dawn of time.");
                return true;
            }

            var dt = Utils.FromUnixTimestamp(acc.LastSeen);
            player.SendInfo($"<{name}> not online. Player last seen {Utils.TimeAgo(dt)}.");
            return true;
        }
    }

    internal class RemoveAccountOverrideCommand : Command
    {
        public RemoveAccountOverrideCommand() : base("removeOverride", 0, listCommand: false)
        {
        }

        protected override bool Process(Player player, string args)
        {
            var acc = player.Client.Account;
            if (acc.AccountIdOverrider == 0)
            {
                player.SendError("Account isn't overridden.");
                return false;
            }

            var overriderAcc = player.Manager.Database.GetAccount(acc.AccountIdOverrider);
            if (overriderAcc == null)
            {
                player.SendError("Account not found!");
                return false;
            }

            overriderAcc.AccountIdOverride = 0;
            overriderAcc.FlushAsync();
            player.SendInfo("Account override removed.");
            return true;
        }
    }

    internal class CurrentSongCommand : Command
    {
        public CurrentSongCommand() : base("currentsong", alias: "song")
        {
        }

        protected override bool Process(Player player, string args)
        {
            var properName = player.Owner.Music;
            var file = File.Create(Environment.CurrentDirectory + $"/resources/web/music/{properName}.mp3");
            var artist = file.Tag.FirstPerformer ?? "Unknown";
            var title = file.Tag.Title ?? properName;
            var album = file.Tag.Album != null ? $" from {file.Tag.Album}" : "";
            var filename = $" ({properName}.mp3)";

            player.SendInfo($"Current Song: {title} by {artist}{album}{filename}.");
            return true;
        }
    }

    internal class ServersCommand : Command
    {
        public ServersCommand() : base("servers", alias: "svrs")
        {
        }

        protected override bool Process(Player player, string args)
        {
            var playerSvr = player.Manager.Config.serverInfo.name;
            var servers = player.Manager.InterServer
                .GetServerList()
                .Where(s => s.type == ServerType.World)
                .ToArray();

            var sb = new StringBuilder($"Servers online ({servers.Length}):\n");
            foreach (var server in servers)
            {
                var currentSvr = server.name.Equals(playerSvr);
                if (currentSvr)
                {
                    sb.Append("[");
                }
                sb.Append(server.name);
                if (currentSvr)
                {
                    sb.Append("]");
                }
                sb.Append($" ({server.players}/{server.maxPlayers}");
                if (server.queueLength > 0)
                {
                    sb.Append($" + {server.queueLength} queued");
                }
                sb.Append(")");
                if (server.adminOnly)
                {
                    sb.Append(" Admin only");
                }
                sb.Append("\n");
            }

            player.SendInfo(sb.ToString());
            return true;
        }
    }

    internal class MyAccIdCommand : Command
    {
        public MyAccIdCommand() : base("myaccid")
        {
        }

        protected override bool Process(Player player, string args)
        {
            string AccID = player.AccountId.ToString();
            if (string.IsNullOrEmpty(args))
            {
                player.SendInfo("Your Account ID is" + " : " + AccID);
            }
            return true;
        }
    }

    internal class AscendCommand : Command
    {
        public AscendCommand() : base("ascend")
        {
        }

        protected override bool Process(Player player, string args)
        {
            if (player.AscensionEnabled == true)
            {
                player.SendError("You are already ascended.");
                return false;
            }
            if (player.Fame < 2500)
            {
                player.SendError("You must have at least 2500 base fame to ascend.");
                return false;
            }
            var Manager = player.Manager;
            var playerDesc = Manager.Resources.GameData.Classes[player.ObjectType];
            var maxed = playerDesc.Stats.Where((t, i) => player.Stats.Base[i] >= t.MaxValue).Count();
            if (maxed < 8)
            {
                player.SendError("You must be at least 8/10 to ascend.");
                return false;
            }

            player.AscensionEnabled = true;
            return true;
        }
    }

    internal class FixMeCommand : Command
    {
        public FixMeCommand() : base("fix")
        {
        }

        protected override bool Process(Player player, string args)
        {
            player.ApplyConditionEffect(ConditionEffectIndex.Stunned, 1000);
            return true;
        }
    }



    //internal class Set : Command
    //{
    //    private readonly Dictionary<int, int[]> VIPItems = new Dictionary<int, int[]>
    //    {
    //        //Rogue
    //        { 768, new [] { 0xaff, 0xb27, 0xaf9, 0xba9 } },
    //        //Archer
    //        { 775, new [] { 0xb02, 0xb28, 0xaf9, 0xba9 } },
    //        //Wizard
    //        { 782, new [] { 0xb08, 0xb24, 0xb05, 0xba9 } },
    //        //Priest
    //        { 784, new [] { 0xaf6, 0xb25, 0xb05, 0xba9 } },
    //        //Warrior
    //        { 797, new [] { 0xb0b, 0xb29, 0xafc, 0xba9 } },
    //        //Knight
    //        { 798, new [] { 0xb0b, 0xb22, 0xafc, 0xba9 } },
    //        //Paladin
    //        { 799, new [] { 0xb0b, 0xb26, 0xafc, 0xba9 } },
    //        //Assassin
    //        { 800, new [] { 0xaff, 0xb2a, 0xaf9, 0xba9 } },
    //        //Necro
    //        { 801, new [] { 0xb08, 0xb2b, 0xb05, 0xba9 } },
    //        //Huntress
    //        { 802, new [] { 0xb02, 0xb2c, 0xaf9, 0xba9 } },
    //        //Mystic
    //        { 803, new [] { 0xb08, 0xb2d, 0xb05, 0xba9 } },
    //        //Trickster
    //        { 804, new [] { 0xaff, 0xb23, 0xaf9, 0xba9 } },
    //        //Sorcerer
    //        { 805, new [] { 0xaf6, 0xb33, 0xb05, 0xba9 } },
    //        //Ninja
    //        { 806, new [] { 0xc50, 0xc59, 0xaf9, 0xba9 } },
    //        //Samurai
    //        { 807, new [] { 0xc50, 0x1862, 0xafc, 0xba9 } },
    //        //Templar
    //        { 808, new [] { 0x52bd, 0x51a1, 0xafc, 0xba9 } },
    //        //Drakzix
    //        { 809, new [] { 0xaf6, 0x52c1, 0xb05, 0xba9 } },
    //        //Shrine Maiden
    //        { 22570, new [] { 0xc50, 0x5816, 0xb05, 0xba9 } },
    //        //Gambler
    //        { 22566, new [] { 0xaff, 0x583f, 0xaf9, 0xba9 } },
    //        //Blademaster
    //        { 24896, new [] { 0x6154, 0x6147, 0xaf9, 0xba9 } },
    //        //Spirit Hunter
    //        { 21945, new [] { 0xb02, 0x670e, 0xb05, 0xba9 } }
    //    };

    //    private readonly Dictionary<int, int[]> DonorItems = new Dictionary<int, int[]>
    //    {
    //        //Rogue
    //        { 768, new [] { 0xa8a, 0xae1, 0xa8f, 0xacd } },
    //        //Archer
    //        { 775, new [] { 0xa8d, 0xa65, 0xa8f, 0xacd } },
    //        //Wizard
    //        { 782, new [] { 0xaa2, 0xa30, 0xa95, 0xacd } },
    //        //Priest
    //        { 784, new [] { 0xa87, 0xa5b, 0xa95, 0xacd } },
    //        //Warrior
    //        { 797, new [] { 0xa47, 0xa6b, 0xa92, 0xacd } },
    //        //Knight
    //        { 798, new [] { 0xa47, 0xa0c, 0xa92, 0xacd } },
    //        //Paladin
    //        { 799, new [] { 0xa47, 0xa55, 0xa92, 0xacd } },
    //        //Assassin
    //        { 800, new [] { 0xa8a, 0xaa8, 0xa8f, 0xacd } },
    //        //Necro
    //        { 801, new [] { 0xaa2, 0xaaf, 0xa95, 0xacd } },
    //        //Huntress
    //        { 802, new [] { 0xa8d, 0xab6, 0xa8f, 0xacd } },
    //        //Mystic
    //        { 803, new [] { 0xaa2, 0xa46, 0xa95, 0xacd } },
    //        //Trickster
    //        { 804, new [] { 0xa8a, 0xb20, 0xa8f, 0xacd } },
    //        //Sorcerer
    //        { 805, new [] { 0xa87, 0xb32, 0xa95, 0xacd } },
    //        //Ninja
    //        { 806, new [] { 0xc4f, 0xc58, 0xa8f, 0xacd } },
    //        //Samurai
    //        { 807, new [] { 0xc4f, 0x185f, 0xa92, 0xacd } },
    //        //Templar
    //        { 808, new [] { 0x52bc, 0x51af, 0xa92, 0xacd } },
    //        //Drakzix
    //        { 809, new [] { 0xa87, 0x52cf, 0xa95, 0xacd } },
    //        //Shrine Maiden
    //        { 22570, new [] { 0xc4f, 0x5815, 0xa95, 0xacd } },
    //        //Gambler
    //        { 22566, new [] { 0xa8a, 0x583e, 0xa8f, 0xacd } },
    //        //Blademaster
    //        { 24896, new [] { 0x6153, 0x6146, 0xa8f, 0xacd } },
    //        //Spirit Hunter
    //        { 21945, new [] { 0xa8d, 0x670d, 0xa95, 0xacd } }
    //    };

    //    private readonly Dictionary<int, int[]> NormalItems = new Dictionary<int, int[]>
    //    {
    //        //Rogue
    //        { 768, new [] { 0xa18, 0xadb, 0xad2, 0xabd } },
    //        //Archer
    //        { 775, new [] { 0xa3a, 0xade, 0xad2, 0xabd } },
    //        //Wizard
    //        { 782, new [] { 0xa9e, 0xad5, 0xa5f, 0xabd } },
    //        //Priest
    //        { 784, new [] { 0xae0, 0xad8, 0xa5f, 0xabd} },
    //        //Warrior
    //        { 797, new [] { 0xa02, 0xa69, 0xadf, 0xabd } },
    //        //Knight
    //        { 798, new [] { 0xa02, 0xacf, 0xadf, 0xabd } },
    //        //Paladin
    //        { 799, new [] { 0xa02, 0xada, 0xadf, 0xabd } },
    //        //Assassin
    //        { 800, new [] { 0xa18, 0xaa6, 0xad2, 0xabd } },
    //        //Necro
    //        { 801, new [] { 0xa9e, 0xaad, 0xa5f, 0xabd } },
    //        //Huntress
    //        { 802, new [] { 0xa3a, 0xab4, 0xad2, 0xabd } },
    //        //Mystic
    //        { 803, new [] { 0xa9e, 0xa44, 0xa5f, 0xabd } },
    //        //Trickster
    //        { 804, new [] { 0xa18, 0xb1e, 0xad2, 0xabd } },
    //        //Sorcerer
    //        { 805, new [] { 0xae0, 0xb30, 0xa5f, 0xabd } },
    //        //Ninja
    //        { 806, new [] { 0xc4b, 0xc56, 0xad2, 0xabd } },
    //        //Samurai
    //        { 807, new [] { 0xc4b, 0x185d, 0xadf, 0xabd } },
    //        //Templar
    //        { 808, new [] { 0x52b8, 0x51ad, 0xadf, 0xabd } },
    //        //Drakzix
    //        { 809, new [] { 0xae0, 0x52cd, 0xa5f, 0xabd } },
    //        //Shrine Maiden
    //        { 22570, new [] { 0xc4b, 0x5813, 0xa5f, 0xabd } },
    //        //Gambler
    //        { 22566, new [] { 0xa18, 0x583c, 0xad2, 0xabd } },
    //        //Blademaster
    //        { 24896, new [] { 0x614f, 0x6144, 0xad2, 0xabd } },
    //        //Spirit Hunter
    //        { 21945, new [] { 0xa3a, 0x670b, 0xa5f, 0xabd } }
    //    };

    //    public Set() : base("Set")
    //    {
    //    }

    //    protected override bool Process(Player player, string args)
    //    {
    //        player.Client.Player.XPBoosted = true;
    //        player.Client.Character.XPBoostTime = -1;
    //        player.Client.Player.XPBoostTime = -1;
    //        player.ForceUpdate(player.XPBoostTime);
    //        player.SaveToCharacter();
    //        player.InvokeStatChange(StatsType.XPBoost, -1, true);

    //        if (player.Inventory[4] != null || player.Inventory[5] != null || player.Inventory[6] != null || player.Inventory[7] != null)
    //        {
    //            player.SendInfo("Please remove the items from the inventory slots 1, 2, 3, 4 to get a set!");
    //            return false;
    //        }

    //        Dictionary<int, int[]> set = NormalItems;
    //        if (player.Rank >= 20)
    //            set = DonorItems;
    //        if (player.Rank >= 30)
    //            set = VIPItems;

    //        foreach (var s in set)
    //        {
    //            if (player.ObjectType == s.Key)
    //            {
    //                player.Inventory[4] = player.Manager.Resources.GameData.Items[(ushort)s.Value[0]];
    //                player.Inventory[5] = player.Manager.Resources.GameData.Items[(ushort)s.Value[1]];
    //                player.Inventory[6] = player.Manager.Resources.GameData.Items[(ushort)s.Value[2]];
    //                player.Inventory[7] = player.Manager.Resources.GameData.Items[(ushort)s.Value[3]];
    //                player.SaveToCharacter();
    //                player.SendInfo("Set Given");
    //                return true;
    //            }
    //        }

    //        player.SendError("Error, Unknown class");
    //        return false;
    //    }
    //}

    #endregion UtilityCommands

    #region ClientMenuCommands

    internal class PauseCommand : Command
    {
        public PauseCommand() : base("pause")
        {
        }

        protected override bool Process(Player player, string args)
        {
            if (player.SpectateTarget != null)
            {
                player.SendError("The use of pause is disabled while spectating.");
                return false;
            }

            if (player.HasConditionEffect(ConditionEffects.Paused))
            {
                player.ApplyConditionEffect(new ConditionEffect()
                {
                    Effect = ConditionEffectIndex.Paused,
                    DurationMS = 0
                });
                player.SendInfo("Game resumed.");
                return true;
            }

            var owner = player.Owner;
            if (owner != null && (owner is Arena || owner is ArenaSolo || owner is DeathArena))
            {
                player.SendInfo("Can't pause in arena.");
                return false;
            }
            if (owner.Difficulty >= 4)
            {
                player.SendInfo("Can't pause in a dungeon greater than difficulty 4!");
                return false;
            }
            if (player.Owner.EnemiesCollision.HitTest(player.X, player.Y, 8).OfType<Enemy>().Any())
            {
                player.SendError("Not safe to pause.");
                return false;
            }

            player.ApplyConditionEffect(new ConditionEffect()
            {
                Effect = ConditionEffectIndex.Paused,
                DurationMS = -1
            });
            player.SendInfo("Game paused.");
            return true;
        }
    }

    internal class TeleportCommand : Command
    {
        public TeleportCommand() : base("tp", alias: "teleport")
        {
        }

        protected override bool Process(Player player, string args)
        {
            foreach (var i in player.Owner.Players.Values)
            {
                if (!i.Name.EqualsIgnoreCase(args))
                    continue;

                if (!i.CanBeSeenBy(player))
                    break;

                player.Teleport(i.Id, player.Rank >= 70);
                return true;
            }

            player.SendError($"Unable to find player: <{args}>");
            return false;
        }
    }

    internal class TellCommand : Command
    {
        public TellCommand() : base("tell", alias: "t")
        {
        }

        protected override bool Process(Player player, string args)
        {
            if (!player.NameChosen)
            {
                player.SendError("Choose a name!");
                return false;
            }

            if (player.Muted)
            {
                player.SendError("You are muted. You can not tell at this time.");
                return false;
            }

            int index = args.IndexOf(' ');
            if (index == -1)
            {
                player.SendError("Usage: /tell <player name> <text>");
                return false;
            }

            string playername = args.Substring(0, index);
            string msg = args.Substring(index + 1);

            if (player.Name.ToLower() == playername.ToLower())
            {
                player.SendInfo("Quit telling yourself!");
                return false;
            }

            if (!player.Manager.Chat.Tell(player, playername, msg))
            {
                player.SendError($"<{playername}> not found.");
                return false;
            }
            return true;
        }
    }

    internal class LocalCommand : Command
    {
        public LocalCommand() : base("l")
        {
        }

        protected override bool Process(Player player, string args)
        {
            if (!player.NameChosen)
            {
                player.SendError("Choose a name!");
                return false;
            }

            if (player.Muted)
            {
                player.SendError("Muted. You can not local chat at this time.");
                return false;
            }

            if (player.CompareAndCheckSpam(args, player.Manager.Core.getTotalTickCount()))
            {
                return false;
            }

            var sent = player.Manager.Chat.Local(player, args);
            if (!sent)
            {
                player.SendError("Failed to send message. Use of extended ascii characters and ascii whitespace (other than space) is not allowed.");
            }
            else
            {
                player.Owner.ChatReceived(player, args);
            }

            return sent;
        }
    }

    internal class HelpCommand : Command
    {
        //actually the command is 'help', but /help is intercepted by client
        public HelpCommand() : base("commands") { }

        protected override bool Process(Player player, string args)
        {
            StringBuilder sb = new StringBuilder("Available commands: ");
            var cmds = player.Manager.Commands.Commands.Values.Distinct()
                .Where(x => x.HasPermission(player) && x.ListCommand)
                .ToArray();
            Array.Sort(cmds, (c1, c2) => c1.CommandName.CompareTo(c2.CommandName));
            for (int i = 0; i < cmds.Length; i++)
            {
                if (i != 0) sb.Append(", ");
                sb.Append(cmds[i].CommandName);
            }

            player.SendInfo(sb.ToString());
            return true;
        }
    }

    internal class IgnoreCommand : Command
    {
        public IgnoreCommand() : base("ignore")
        {
        }

        protected override bool Process(Player player, string playerName)
        {
            if (player.Owner is Test)
                return false;

            if (String.IsNullOrEmpty(playerName))
            {
                player.SendError("Usage: /ignore <player name>");
                return false;
            }

            if (player.Name.ToLower() == playerName.ToLower())
            {
                player.SendInfo("Can't ignore yourself!");
                return false;
            }

            var target = player.Manager.Database.ResolveId(playerName);
            var targetAccount = player.Manager.Database.GetAccount(target);
            var srcAccount = player.Client.Account;

            if (target == 0 || targetAccount == null || targetAccount.Hidden && player.Admin == 0)
            {
                player.SendError("Player not found.");
                return false;
            }

            player.Manager.Database.IgnoreAccount(srcAccount, targetAccount, true);

            player.Client.SendPacket(new AccountList()
            {
                AccountListId = 1, // ignore list
                AccountIds = srcAccount.IgnoreList
                    .Select(i => i.ToString())
                    .ToArray()
            });

            player.SendInfo("<" + playerName + "> has been added to your ignore list.");
            return true;
        }
    }

    internal class UnignoreCommand : Command
    {
        public UnignoreCommand() : base("unignore")
        {
        }

        protected override bool Process(Player player, string playerName)
        {
            if (player.Owner is Test)
                return false;

            if (String.IsNullOrEmpty(playerName))
            {
                player.SendError("Usage: /unignore <player name>");
                return false;
            }

            if (player.Name.ToLower() == playerName.ToLower())
            {
                player.SendInfo("You are no longer ignoring yourself. Good job.");
                return false;
            }

            var target = player.Manager.Database.ResolveId(playerName);
            var targetAccount = player.Manager.Database.GetAccount(target);
            var srcAccount = player.Client.Account;

            if (target == 0 || targetAccount == null || targetAccount.Hidden && player.Admin == 0)
            {
                player.SendError("Player not found.");
                return false;
            }

            player.Manager.Database.IgnoreAccount(srcAccount, targetAccount, false);

            player.Client.SendPacket(new AccountList()
            {
                AccountListId = 1, // ignore list
                AccountIds = srcAccount.IgnoreList
                    .Select(i => i.ToString())
                    .ToArray()
            });

            player.SendInfo("<" + playerName + "> no longer ignored.");
            return true;
        }
    }

    internal class LockCommand : Command
    {
        public LockCommand() : base("lock")
        {
        }

        protected override bool Process(Player player, string playerName)
        {
            if (player.Owner is Test)
                return false;

            if (String.IsNullOrEmpty(playerName))
            {
                player.SendError("Usage: /lock <player name>");
                return false;
            }

            if (player.Name.ToLower() == playerName.ToLower())
            {
                player.SendInfo("Can't lock yourself!");
                return false;
            }

            var target = player.Manager.Database.ResolveId(playerName);
            var targetAccount = player.Manager.Database.GetAccount(target);
            var srcAccount = player.Client.Account;

            if (target == 0 || targetAccount == null || targetAccount.Hidden && player.Admin == 0)
            {
                player.SendError("Player not found.");
                return false;
            }

            player.Manager.Database.LockAccount(srcAccount, targetAccount, true);

            player.Client.SendPacket(new AccountList()
            {
                AccountListId = 0, // locked list
                AccountIds = player.Client.Account.LockList
                    .Select(i => i.ToString())
                    .ToArray(),
                LockAction = 1
            });

            player.SendInfo("<" + playerName + "> is now locked.");
            return true;
        }
    }

    internal class UnlockCommand : Command
    {
        public UnlockCommand() : base("unlock")
        {
        }

        protected override bool Process(Player player, string playerName)
        {
            if (player.Owner is Test)
                return false;

            if (String.IsNullOrEmpty(playerName))
            {
                player.SendError("Usage: /unlock <player name>");
                return false;
            }

            if (player.Name.ToLower() == playerName.ToLower())
            {
                player.SendInfo("You are no longer locking yourself. Nice!");
                return false;
            }

            var target = player.Manager.Database.ResolveId(playerName);
            var targetAccount = player.Manager.Database.GetAccount(target);
            var srcAccount = player.Client.Account;

            if (target == 0 || targetAccount == null || targetAccount.Hidden && player.Admin == 0)
            {
                player.SendError("Player not found.");
                return false;
            }

            player.Manager.Database.LockAccount(srcAccount, targetAccount, false);

            player.Client.SendPacket(new AccountList()
            {
                AccountListId = 0, // locked list
                AccountIds = player.Client.Account.LockList
                    .Select(i => i.ToString())
                    .ToArray(),
                LockAction = 0
            });

            player.SendInfo("<" + playerName + "> is no longer locked.");
            return true;
        }
    }

    internal class TradeCommand : Command
    {
        public TradeCommand() : base("trade")
        {
        }

        protected override bool Process(Player player, string args)
        {
            if (String.IsNullOrWhiteSpace(args))
            {
                player.SendError("Usage: /trade <player name>");
                return false;
            }

            player.RequestTrade(args);
            return true;
        }
    }

    internal class ReportCommand : Command
    {
        public ReportCommand() : base("report")
        {
        }
        protected override bool Process(Player player, string args)
        {
            if (!player.NameChosen)
            {
                player.SendError("Choose a name!");
                return false;
            }

            if (player.Muted)
            {
                player.SendError("You are muted. You can not report at this time.");
                return false;
            }

            int index = args.IndexOf(' ');
            if (index == -1)
            {
                player.SendError("Usage: /report <player name> <report reason>");
                return false;
            }

            string playername = args.Substring(0, index);
            string msg = args.Substring(index + 1);

            if (player.Name.ToLower() == playername.ToLower())
            {
                player.SendInfo("Quit reporting yourself!");
                return false;
            }

            /*if (!player.Manager.Chat.Tell(player, playername, msg))
            {
                player.SendError($"<{playername}> not found.");
                return false;
            }*/
            foreach (var i in player.Manager.Clients.Keys)
            {
                if (i.Account.Name.EqualsIgnoreCase(playername))
                {
                    // probably if someone is hidden doesn't want to be kicked, so we leave it as before
                    if (i.Account.Hidden)
                        break;

                    Program.DL.LogToDiscord(WebHooks.Test, "Report Log", $"Report Log: [{player.Name}] has reported [{playername}] for [{msg}]");
                    player.SendInfo("Player reported!");

                    return true;
                }
            }
            return false;

        }
    }


    #endregion ClientMenuCommands

    #region Gambling

    internal class GambleCommand : Command
    {
        public GambleCommand() : base("gamble")
        {
        }

        protected override bool Process(Player player, string args)
        {
            if (player.isGambling)
            {
                player.SendError("You are already gambling!");
                return false;
            }

            if (player.Rank >= 80)
            {
                player.SendError("You're not allowed to gamble with your current permissions.");
                return false;
            }

            if (String.IsNullOrWhiteSpace(args))
            {
                player.SendError("Invalid player!");
                return false;
            }

            player.RequestGamble(args, player.betAmount);

            return true;
        }
    }

    internal class SetGambleCommand : Command
    {
        public SetGambleCommand() : base("setgamble")
        {
        }

        protected override bool Process(Player player, string args)
        {
            var amount2 = int.Parse(args);

            if (player.Rank >= 80)
            {
                player.SendError("No.");
                return false;
            }

            if (player.isGambling == true)
            {
                player.SendInfo("You can't change the amount while gambling already!");
                return false;
            }
            if (string.IsNullOrEmpty(args))
            {
                player.SendInfo("/setgamble <amount>");
                return false;
            }

            if (!(amount2 <= 100000))
            {
                player.SendError("You need to lower your gamble amount..");
                return false;
            }

            if (amount2 < 0)
            {
                player.SendError("You need to set your gamble to a positive integer.");
                return false;
            }

            player.betAmount = amount2;
            return true;
        }
    }

    internal class CheckGambleCommand : Command
    {
        public CheckGambleCommand() : base("checkgamble")
        {
        }

        protected override bool Process(Player player, string args)
        {
            player.SendInfo("Your set gamble is currently " + player.betAmount);
            return true;
        }
    }

    internal class PickGambleCommand : Command
    {
        public PickGambleCommand() : base("pg")
        {
        }

        protected override bool Process(Player player, string args)
        {
            if (string.IsNullOrWhiteSpace(args))
            {
                player.SendError("You didnt choose rock, paper or scissors!");
                return false;
            }
            switch (args)
            {
                case "r":
                    player.gamble = Player.GambleType.Rock;
                    break;

                case "p":
                    player.gamble = Player.GambleType.Paper;
                    break;

                case "s":
                    player.gamble = Player.GambleType.Scissors;
                    break;

                default:
                    player.SendError("You didnt choose rock, paper or scissors!");
                    break;
            }
            return true;
        }
    }

    #endregion Gambling

    #region GuildCommands

    internal class GhallCommand : Command
    {
        public GhallCommand() : base("ghall") { }


        protected override bool Process(Player player, string args)
        {
            if (player.GuildRank < 0)
            {
                player.SendError("You need to be in a guild.");
                return false;
            }

            var proto = player.Manager.Resources.Worlds["GuildHall"];
            var world = player.Manager.GetWorld(proto.id);
            player.Reconnect(world.GetInstance(player.Client));
            return true;
        }
    }

    internal class GuildKickCommand : Command
    {
        public GuildKickCommand() : base("gkick")
        {
        }

        protected override bool Process(Player player, string name)
        {
            if (player.Owner is Test)
                return false;

            var manager = player.Client.Manager;

            // if resigning
            if (player.Name.Equals(name))
            {
                // chat needs to be done before removal so we can use
                // srcPlayer as a source for guild info
                manager.Chat.Guild(player, "<" + player.Name + "> has left the guild.", true);

                if (!manager.Database.RemoveFromGuild(player.Client.Account))
                {
                    player.SendError("Guild not found.");
                    return false;
                }

                player.Guild = "";
                player.GuildRank = 0;

                return true;
            }

            // get target account id
            var targetAccId = manager.Database.ResolveId(name);
            if (targetAccId == 0)
            {
                player.SendError("Player not found");
                return false;
            }

            // find target player (if connected)
            var targetClient = (from client in manager.Clients.Keys
                                where client.Account != null
                                where client.Account.AccountId == targetAccId
                                select client)
                                .FirstOrDefault();

            // try to remove connected member
            if (targetClient != null)
            {
                if (player.Client.Account.GuildRank >= 20 &&
                    player.Client.Account.GuildId == targetClient.Account.GuildId &&
                    player.Client.Account.GuildRank > targetClient.Account.GuildRank)
                {
                    var targetPlayer = targetClient.Player;

                    if (!manager.Database.RemoveFromGuild(targetClient.Account))
                    {
                        player.SendError("Guild not found.");
                        return false;
                    }

                    targetPlayer.Guild = "";
                    targetPlayer.GuildRank = 0;

                    manager.Chat.Guild(player,
                        "<" + targetPlayer.Name + "> has been kicked from the guild by <" + player.Name + ">", true);
                    targetPlayer.SendInfo("You have been kicked from the guild.");
                    return true;
                }

                player.SendError("Can't remove member. Insufficient privileges.");
                return false;
            }

            // try to remove member via database
            var targetAccount = manager.Database.GetAccount(targetAccId);

            if (player.Client.Account.GuildRank >= 20 &&
                player.Client.Account.GuildId == targetAccount.GuildId &&
                player.Client.Account.GuildRank > targetAccount.GuildRank)
            {
                if (!manager.Database.RemoveFromGuild(targetAccount))
                {
                    player.SendError("Guild not found.");
                    return false;
                }

                manager.Chat.Guild(player,
                    "<" + targetAccount.Name + "> has been kicked from the guild by <" + player.Name + ">", true);
                return true;
            }

            player.SendError("Can't remove member. Insufficient privileges.");
            return false;
        }
    }

    internal class GuildInviteCommand : Command
    {
        public GuildInviteCommand() : base("invite", alias: "ginvite")
        {
        }

        protected override bool Process(Player player, string playerName)
        {
            if (player.Owner is Test)
                return false;

            if (player.Client.Account.GuildRank < 20)
            {
                player.SendError("Insufficient privileges.");
                return false;
            }

            var targetAccId = player.Client.Manager.Database.ResolveId(playerName);
            if (targetAccId == 0)
            {
                player.SendError("Player not found");
                return false;
            }

            var targetClient = (from client in player.Client.Manager.Clients.Keys
                                where client.Account != null
                                where client.Account.AccountId == targetAccId
                                select client)
                    .FirstOrDefault();

            if (targetClient != null)
            {
                if (targetClient.Player == null ||
                    targetClient.Account == null ||
                    !targetClient.Account.Name.Equals(playerName))
                {
                    player.SendError("Could not find the player to invite.");
                    return false;
                }

                if (!targetClient.Account.NameChosen)
                {
                    player.SendError("Player needs to choose a name first.");
                    return false;
                }

                if (targetClient.Account.GuildId > 0)
                {
                    player.SendError("Player is already in a guild.");
                    return false;
                }

                targetClient.Player.GuildInvite = player.Client.Account.GuildId;

                targetClient.SendPacket(new InvitedToGuild()
                {
                    Name = player.Name,
                    GuildName = player.Guild
                });
                return true;
            }

            player.SendError("Could not find the player to invite.");
            return false;
        }
    }

    internal class GuildWhoCommand : Command
    {
        public GuildWhoCommand() : base("gwho", alias: "mates")
        {
        }

        protected override bool Process(Player player, string args)
        {
            if (player.Client.Account.GuildId == 0)
            {
                player.SendError("You are not in a guild!");
                return false;
            }

            var pServer = player.Manager.Config.serverInfo.name;
            var pGuild = player.Client.Account.GuildId;
            var servers = player.Manager.InterServer.GetServerList();
            var result =
                (from server in servers
                 from plr in server.playerList
                 where plr.GuildId == pGuild
                 where !plr.Hidden
                 group plr by server);

            player.SendInfo("Guild members online:");

            foreach (var group in result)
            {
                var server = (pServer == group.Key.name) ? $"[{group.Key.name}]" : group.Key.name;
                var players = group.ToArray();
                var sb = new StringBuilder($"{server}: ");
                for (var i = 0; i < players.Length; i++)
                {
                    if (i != 0)
                        sb.Append(", ");

                    sb.Append(players[i].Name);
                }
                player.SendInfo(sb.ToString());
            }
            return true;
        }
    }

    internal class GCommand : Command
    {
        public GCommand() : base("g", alias: "guild")
        {
        }

        protected override bool Process(Player player, string args)
        {
            if (!player.NameChosen)
            {
                player.SendError("Choose a name!");
                return false;
            }

            if (player.Muted)
            {
                player.SendError("Muted. You can not guild chat at this time.");
                return false;
            }

            if (String.IsNullOrEmpty(player.Guild))
            {
                player.SendError("You need to be in a guild to guild chat.");
                return false;
            }

            return player.Manager.Chat.Guild(player, args);
        }
    }
    internal class JoinGuildCommand : Command
    {
        public JoinGuildCommand() : base("join")
        {
        }

        protected override bool Process(Player player, string args)
        {
            player.Client.ProcessPacket(new JoinGuild()
            {
                GuildName = args
            });
            return true;
        }
    }


    internal class GuildSummonCommand : Command
    {
        public GuildSummonCommand() : base("gsummon")
        {
        }

        protected override bool Process(Player player, string args)
        {
            if (player.Client.Account.GuildId == 0)
            {
                player.SendError("You need to be in a guild to use guild summon");
                return false;
            }
            var guild = player.Manager.Database.GetGuild(player.Client.Account.GuildId);
            if (guild.Level != 3) //Comment out for testing if you don't have a good enough guild
            {
                player.SendError("You need a level 4 guild to unlock guild summon!");
                return false;
            }
            if (player.GuildRank < 20)
            {
                player.SendError("Insufficient privileges.");
                return false;
            }
            if (player.Owner is Realm || player.Owner is Nexus || player.Owner is GuildHall
                || player.Owner is Vault || player.Owner is Marketplace || player.Rank < 40 && player.Owner is BalancerStation)
            {
                player.SendError("Can't summon here!");
                return false;
            }

            HashSet<string> invited = new HashSet<string>();

            foreach (var i in player.Manager.Clients.Keys
                    .Where(x => x.Player != null)
                    .Where(x => x.Account.GuildId == player.Client.Account.GuildId)
                    .Select(x => x.Player))
            {
                if (i.Owner.Id == player.Owner.Id)
                {
                    player.Owner.Invited.Add(i.Name.ToLower());
                    continue;
                }
                else if (player.Manager.Chat.Invite(player, i.Name,
                    "Your guildie is inviting you to join his " + player.Owner.Name + ".Type /gaccept to accept invite."))
                {
                    player.Owner.Invited.Add(i.Name.ToLower());
                    player.Owner.Invites.Add(i.Name.ToLower());
                    invited.Add(i.Name);
                }
            }
            guild.MemberInviting = player.Id;
            guild.FlushAsync();
            player.SendInfo($"Invited {invited.Count} members from the guild.");
            return true;
        }
    }
    internal class GuildSummonAcceptCommand : Command
    {
        public GuildSummonAcceptCommand() : base("gaccept")
        {
        }

        protected override bool Process(Player player, string args)
        {
            var guild = player.Manager.Database.GetGuild(player.Client.Account.GuildId);
            var inviter = guild.MemberInviting;//Inviter =  Inviter ID from player's Guild


            foreach (var i in player.Manager.Clients.Keys
                .Where(x => x.Player != null)
                .Where(x => x.Account.GuildId == player.Client.Account.GuildId)
                .Select(x => x.Player))
            {
                if (i.Id != inviter)//checking if the account id matches the one from database
                {
                    continue;
                }
                else//if it does we try to connect to their world
                {


                    var world = i.Manager.GetWorld(i.Owner.Id);
                    Console.WriteLine("World Invites: " + world.Invites + " World Invited: " + world.Invited);
                    if (world != null)
                    {
                        Console.WriteLine("World is not Null");
                        if (world.Invites.Contains(player.Name.ToLower()))
                        {
                            Console.WriteLine("World Invites contain player.Name");
                            world.Invites.Remove(player.Name.ToLower());
                            if (world is BalancerStation && player.Rank < 40 || world.Name is "KrakenLair" || world.Name is "TheHollows" || world.Name is "HiddenTempleBoss" || world.Name is "FrozenIsland" || world.Name is "Admins Arena" || world.Name is "Castle" || world.Name is "Oryx's Chamber" || world.Name is "Wine Cellar" || world.Name is "AldraginesHideout" || world.Name is "UltraAldraginesHideout" || world.Name is "Sincryer's Gate" || world.Name is "Ultra Sincryer's Gate" || world.Name is "Nontridus" || world.Name is "NontridusUltra" || world.Name is "Core of the Hideout" || world.Name is "Ultra Core of the Hideout" || world.Name is "Keeping of Aldragine" || world.Name is "KeepingUltra" || world.Name is "Zol Secret Shop" || world.Name is "BastilleofDrannol" || world.Name is "Hunter Cave" || world.Name is "Twisted Trials" || world.Name is "Twisted Trials Cont" || world.Name is "Twisted Domain" || world.Name is "The Steps" || world.Name is "The Steps 2" || world.Name is "Drannol Secret Shop" || world.Name is "Ultra Drannol Secret Shop" || world.Name is "The Void")
                            {
                                player.SendErrorFormat("You are not allowed in this map!");
                                return false;
                            }
                            player.Client.Reconnect(new Reconnect()
                            {
                                Host = "",
                                Port = 2050,
                                GameId = world.Id,
                                Name = world.SBName ?? world.Name,
                            });

                            i.SendInfo($"{player.Name} accepted your request and is now connecting to your world."); //This will msg the player who invites his guild, once per player that accepts the invite. Maybe cool/funny idk
                            return true;
                        }
                        player.SendError("Your name was not found in the invites list.");
                        return false;
                    }
                    else
                    {
                        player.SendError("No invites found.");
                        return false;
                    }
                }
            }
            return false;
        }
    }

    #endregion GuildCommands

    #region PartyCommands

    internal class PartyMakeCommand : Command
    {
        public PartyMakeCommand() : base("pmake")
        {
        }

        protected override bool Process(Player player, string args)
        {
            var manager = player.Client.Manager;
            DbParty party = new DbParty(player.Client.Account);

            if (player.Client.Account.NameChosen && player.Client.Account.PartyId > 0)
            {
                player.SendError("You're already in a party!");
                return false;
            }
            if (!player.Client.Account.NameChosen)
            {
                player.SendError("You need to choose a name before creating a party!");
                return false;
            }

            manager.Database.CreateParty(out party, player.Client.Account);

            player.Client.Account.PartyId = party.Id;
            player.Client.Account.PartyRank = 2;
            player.Client.Account.FlushAsync();

            player.PartyId = party.Id;
            player.PartyRank = 2;

            player.SendInfo("Party Created Successfully");
            player.SendHelp("To invite your friends, type '/pinv <name>'");
            player.SendHelp("To kick members, type '/pkick <name>'");
            player.SendHelp("To invite members to your world, type '/psummon'");
            player.SendHelp("To close the party, type '/pclose'");
            return true;
        }
    }

    internal class PartyCloseCommand : Command
    {
        public PartyCloseCommand() : base("pclose")
        {
        }

        protected override bool Process(Player player, string args)
        {
            var manager = player.Client.Manager;
            var party = manager.Database.GetParty(player.Client.Account.PartyId);
            if (player.Client.Account.PartyId < 1)
            {
                player.SendError("You're not in a party!");
                return false;
            }
            if (player.Client.Account.PartyId > 0 && player.Client.Account.PartyRank < 2)
            {
                player.SendError("Only the party leader can perform this action.");
                return false;
            }
            if (party.Members.Length > 1)
            {
                var targetClients = (from client in player.Client.Manager.Clients.Keys
                                     where client.Account != null
                                     where client.Account.PartyId == player.Client.Account.PartyId
                                     select client)
                        .FirstOrDefault();
                targetClients.Player.Client.Account.PartyId = 0;
                targetClients.Player.Client.Account.PartyRank = 0;
                targetClients.Player.Client.Account.FlushAsync();
                targetClients.Player.PartyId = -1;
                targetClients.Player.PartyRank = 0;
                targetClients.Player.SendInfo("The party you were in was closed by the leader!");
            }

            player.PartyId = -1;
            player.PartyRank = 0;
            player.Client.Account.PartyId = 0;
            player.Client.Account.PartyRank = 0;
            player.Client.Account.FlushAsync();
            manager.Database.Conn.KeyDelete($"party.{player.Client.Account.PartyId}");
            player.SendInfo("Party successfully closed.");
            return true;
        }
    }

    internal class PartyInviteCommand : Command
    {
        public PartyInviteCommand() : base("pinv")
        {
        }

        protected override bool Process(Player player, string playerName)
        {
            DbParty party = player.Manager.Database.GetParty(player.Client.Account.PartyId);

            var targetAccId = player.Client.Manager.Database.ResolveId(playerName);
            if (targetAccId == 0)
            {
                player.SendError("Player not found");
                return false;
            }
            if (player.Client.Account.PartyId > 0 && player.Client.Account.PartyRank < 2)
            {
                player.SendError("Only the party leader can perform this action.");
                return false;
            }
            if (player.Client.Account.PartyId < 1)
            {
                player.SendError("You're not in a party!");
                return false;
            }

            var targetClient = (from client in player.Client.Manager.Clients.Keys
                                where client.Account != null
                                where client.Account.AccountId == targetAccId
                                select client)
                    .FirstOrDefault();

            if (targetClient != null)
            {
                if (targetClient.Player == null ||
                    targetClient.Account == null ||
                    !targetClient.Account.Name.EqualsIgnoreCase(playerName))
                {
                    player.SendError("Could not find the player to invite.");
                    return false;
                }

                if (!targetClient.Account.NameChosen)
                {
                    player.SendError("Player needs to choose a name first.");
                    return false;
                }

                if (targetClient.Account.PartyId > 0)
                {
                    player.SendError("Player is already in a party.");
                    return false;
                }

                int partySize = player.Rank >= 30 ? 8 : (player.Rank >= 20 ? 6 : (player.Rank >= 10 ? 5 : 4));
                if (party.Members.Length >= partySize)
                {
                    player.SendError("Party is full!");
                    return false;
                }
                targetClient.Player.PartyInvite = player.Client.Account.PartyId;

                targetClient.SendPacket(new InvitedToParty()
                {
                    Name = player.Name,
                    PartyId = player.Client.Account.PartyId
                });
                player.SendInfo($"{targetClient.Player.Name} was invited to the party!");
                return true;
            }
            player.SendError("Couldn't find a player to invite.");
            return false;
        }
    }

    internal class PartyKickCommand : Command
    {
        public PartyKickCommand() : base("pkick")
        {
        }

        protected override bool Process(Player player, string name)
        {
            if (player.Owner is Test)
                return false;

            var manager = player.Client.Manager;

            var targetAccId = manager.Database.ResolveId(name);
            if (targetAccId == 0)
            {
                player.SendError("Player not found");
                return false;
            }

            var targetClient = (from client in manager.Clients.Keys
                                where client.Account != null
                                where client.Account.AccountId == targetAccId
                                select client)
                                .FirstOrDefault();

            if (targetClient != null)
            {
                if (player.Client.Account.PartyRank >= 2 &&
                    player.Client.Account.PartyId == targetClient.Account.PartyId &&
                    player.Client.Account.PartyRank > targetClient.Account.PartyRank)
                {
                    var targetPlayer = targetClient.Player;

                    if (!manager.Database.KickFromParty(targetClient.Account))
                    {
                        player.SendError("Party not found.");
                        return false;
                    }

                    targetPlayer.Client.Account.PartyId = 0;
                    targetPlayer.Client.Account.PartyRank = 0;
                    targetPlayer.PartyId = -1;
                    targetPlayer.PartyRank = 0;

                    manager.Chat.PartyAnnounce(player.Client.Account,
                        targetPlayer.Name + " has been kicked from the party.");
                    targetPlayer.SendInfo("You have been kicked from the party.");
                    targetPlayer.Client.Account.FlushAsync();
                    return true;
                }

                player.SendError("Only the party leader can perform this action.");
                return false;
            }
            player.SendError("You need to be party leader to kick members.");
            return false;
        }
    }

    internal class PartyAcceptInviteCommand : Command
    {
        public PartyAcceptInviteCommand() : base("paccept")
        {
        }

        protected override bool Process(Player player, string args)
        {
            var client = player.Client;
            var manager = player.Manager;
            DbParty party = manager.Database.GetParty((int)player.PartyInvite);

            if (!player.NameChosen)
            {
                player.SendError("You need to chose a name first!");
                return false;
            }

            if (client.Account.PartyId > 0)
            {
                player.SendError("Already in a party.");
                return false;
            }

            foreach (var server in player.Manager.InterServer.GetServerList())
                foreach (PlayerInfo plr in server.playerList)
                    if (plr.Name.Equals(party.Leader, StringComparison.InvariantCultureIgnoreCase))
                    {
                        int partySize = player.Rank >= 30 ? 8 : (player.Rank >= 20 ? 6 : (player.Rank >= 10 ? 5 : 4));
                        if (party.Members.Length > partySize)
                            player.SendError("Party is full!");
                        break;
                    }

            if (args.ToInt32() == player.PartyInvite)
            {
                manager.Database.AddPartyMember(party, client.Account);
                player.PartyId = party.Id;
                player.PartyRank = 0;
                manager.Chat.PartyAnnounce(player.Client.Account, player.Name + " has joined the party!");
                return true;
            }
            player.SendError("Invalid party token.");
            return false;
        }
    }

    internal class PartyLeaveCommand : Command
    {
        public PartyLeaveCommand() : base("pleave")
        {
        }

        protected override bool Process(Player player, string args)
        {
            var manager = player.Client.Manager;

            if (player.Client.Account.PartyId < 1)
            {
                player.SendError("You're not in a party!");
                return false;
            }

            if (player.Client.Account.PartyId > 0)
            {
                if (player.Client.Account.PartyRank != 2)
                {
                    if (!manager.Database.KickFromParty(player.Client.Account))
                    {
                        player.SendError("Party not found.");
                        return false;
                    }
                    player.Client.Account.PartyId = 0;
                    player.Client.Account.PartyRank = 0;
                    player.Client.Account.FlushAsync();
                    player.PartyId = -1;
                    player.PartyRank = 0;
                    manager.Chat.PartyAnnounce(player.Client.Account, player.Name + " has left the party.");
                    player.SendInfo("You have left the party.");
                    return true;
                }
                player.SendError("You are the party leader, you can't leave the party.");
                player.SendError("To close the party, type '/pclose'");
                return false;
            }
            return false;
        }
    }

    internal class PartySummonCommand : Command
    {
        public PartySummonCommand() : base("psummon")
        {
        }

        protected override bool Process(Player player, string args)
        {
            if (player.Owner is Realm || player.Owner is Nexus || player.Owner is GuildHall
                || player.Owner is Vault || player.Owner is Marketplace || player.Rank < 40 && player.Owner is BalancerStation)
            {
                player.SendError("Can't summon here!");
                return false;
            }

            if (player.Client.Account.PartyId < 1)
            {
                player.SendError("You're not in a party!");
                return false;
            }

            if (player.Client.Account.PartyId > 0 && player.Client.Account.PartyRank < 2)
            {
                player.SendError("Only the party leader can perform this action.");
                return false;
            }

            HashSet<string> invited = new HashSet<string>();

            foreach (var i in player.Manager.Clients.Keys
                    .Where(x => x.Player != null)
                    .Where(x => x.Account.PartyId > 0)
                    .Where(x => x.Account.PartyId == player.Client.Account.PartyId)
                    .Select(x => x.Player))
            {
                if (i.Owner.Id == player.Owner.Id)
                {
                    player.Owner.Invited.Add(i.Name.ToLower());
                    continue;
                }
                else if (player.Manager.Chat.Invite(player, i.Name,
                    "Your party leader is inviting you to join his " + player.Owner.Name + " .Type /accept to accept invite."))
                {
                    player.Owner.Invited.Add(i.Name.ToLower());
                    player.Owner.Invites.Add(i.Name.ToLower());
                    invited.Add(i.Name);
                }
            }

            player.SendInfo($"Invited {invited.Count} members from the party.");
            return true;
        }
    }



    internal class PartyAcceptCommand : Command
    {
        public PartyAcceptCommand() : base("accept")
        {
        }

        protected override bool Process(Player player, string args)
        {
            var party = player.Manager.Database.GetParty(player.Client.Account.PartyId);
            var leader = player.Manager.Clients.Keys
                .SingleOrDefault(c => c.Account != null &&
                                      c.Account.Name.Equals(party.Leader, StringComparison.InvariantCultureIgnoreCase));

            var world = player.Manager.GetWorld(leader.Player.Owner.Id);
            if (world != null)
            {
                if (world.Invites.Contains(player.Name.ToLower()))
                {
                    world.Invites.Remove(player.Name.ToLower());
                    if (world is BalancerStation && player.Rank < 40 || world.Name is "KrakenLair" || world.Name is "TheHollows" || world.Name is "HiddenTempleBoss" || world.Name is "FrozenIsland" || world.Name is "Admins Arena" || world.Name is "Castle" || world.Name is "Oryx's Chamber" || world.Name is "Wine Cellar" || world.Name is "AldraginesHideout" || world.Name is "UltraAldraginesHideout" || world.Name is "Sincryer's Gate" || world.Name is "Ultra Sincryer's Gate" || world.Name is "Nontridus" || world.Name is "NontridusUltra" || world.Name is "Core of the Hideout" || world.Name is "Ultra Core of the Hideout" || world.Name is "Keeping of Aldragine" || world.Name is "KeepingUltra" || world.Name is "Zol Secret Shop" || world.Name is "BastilleofDrannol" || world.Name is "Hunter Cave" || world.Name is "Twisted Trials" || world.Name is "Twisted Trials Cont" || world.Name is "Twisted Domain" || world.Name is "The Steps" || world.Name is "The Steps 2" || world.Name is "Drannol Secret Shop" || world.Name is "Ultra Drannol Secret Shop" || world.Name is "The Void")
                    {
                        player.SendErrorFormat("You are not allowed in this map!");
                        return false;
                    }
                    player.Client.Reconnect(new Reconnect()
                    {
                        Host = "",
                        Port = 2050,
                        GameId = world.Id,
                        Name = world.SBName ?? world.Name,
                    });
                    leader.Player.SendInfo($"{player.Name} accepted your request and is now connecting to your world.");
                    return true;
                }
                player.SendError("Your name was not found in the invites list.");
                return false;
            }
            else
            {
                player.SendError("No invites found.");
                return false;
            }
        }
    }

    internal class PartyWhoCommand : Command
    {
        public PartyWhoCommand() : base("pwho")
        {
        }

        protected override bool Process(Player player, string args)
        {
            if (player.Client.Account.PartyId == 0)
            {
                player.SendError("You are not in a party!");
                return false;
            }

            var pServer = player.Manager.Config.serverInfo.name;
            var pParty = player.Client.Account.PartyId;
            var servers = player.Manager.InterServer.GetServerList();
            var result =
                (from server in servers
                 from plr in server.playerList
                 where plr.PartyId == pParty
                 group plr by server);

            player.SendInfo($"Party members online:");

            foreach (var group in result)
            {
                var players = group.ToArray();
                var sb = new StringBuilder("");
                for (var i = 0; i < players.Length; i++)
                {
                    if (i != 0)
                        sb.Append(", ");

                    sb.Append(players[i].Name);
                }
                player.SendInfo($"[{sb.ToString()}]");
            }
            return true;
        }
    }

    internal class PartyChatCommand : Command
    {
        public PartyChatCommand() : base("pchat", alias: "p")
        {
        }

        protected override bool Process(Player player, string args)
        {
            if (!player.NameChosen)
            {
                player.SendError("Choose a name!");
                return false;
            }

            if (player.Muted)
            {
                player.SendError("Muted. You can not use party chat at this time.");
                return false;
            }

            if (player.Client.Account.PartyId < 1)
            {
                player.SendError("You need to be in a party to use party chat.");
                return false;
            }

            return player.Manager.Chat.Party(player, args);
        }
    }

    #endregion PartyCommands

    #region MarketCommands

    internal class MarketplaceCommand : Command
    {
        public MarketplaceCommand() : base("marketplace")
        {
        }

        protected override bool Process(Player player, string args)
        {
            player.Client.Reconnect(new Reconnect()
            {
                Host = "",
                Port = 2050,
                GameId = World.MarketPlace,
                Name = "Marketplace"
            });
            return true;
        }
    }

    internal class RMarketCommand : Command
    {
        public RMarketCommand() : base("rmarket")
        {
        }

        protected override bool Process(Player player, string args)
        {
            if (string.IsNullOrEmpty(args) ||
                !uint.TryParse(args, out uint id))
            {
                player.SendError("Usage: /rmarket <id>. Ids for your listed items can be found with the /mymarket command.");
                return false;
            }

            player.RemoveItemFromMarketAsync(id)
                .ContinueWith(t =>
                {
                    if (t.Result != MarketResult.Success)
                    {
                        player.SendError(t.Result.GetDescription());
                        return;
                    }

                    player.SendInfo("Removal succeeded. The item has been placed in your gift chest.");
                    player.Client.SendPacket(new GlobalNotification
                    {
                        Text = "giftChestOccupied"
                    });
                })
                .ContinueWith(e =>
                    Program.Debug(typeof(RMarketCommand), e.Exception.ToString(), true),
                    TaskContinuationOptions.OnlyOnFaulted);

            return true;
        }
    }

    //internal class OopsCommand : Command
    //{
    //    public OopsCommand() : base("oops")
    //    {
    //    }

    //    protected override bool Process(Player player, string args)
    //    {
    //        player.RemoveItemFromMarketAsync(player.Client.Account.LastMarketId)
    //            .ContinueWith(t =>
    //            {
    //                if (t.Result != MarketResult.Success)
    //                {
    //                    player.SendError(t.Result.GetDescription());
    //                    return;
    //                }

    //                player.SendInfo("Removal succeeded. The item has been placed in your gift chest.");
    //                player.Client.SendPacket(new GlobalNotification
    //                {
    //                    Text = "giftChestOccupied"
    //                });
    //            })
    //            .ContinueWith(e =>
    //                Program.Debug(typeof(OopsCommand), e.Exception.ToString(), true),
    //                TaskContinuationOptions.OnlyOnFaulted);

    //        return true;
    //    }
    //}

    internal class MegaOopsCommand : Command
    {
        public MegaOopsCommand() : base("megaoops")
        {
        }

        protected override bool Process(Player player, string args)
        {
            var shopItems = player.GetMarketItems();
            if (shopItems.Length <= 0)
            {
                player.SendInfo("You have no items currently listed on the market, and thus, none have been removed");
                return true;
            }
            var shopItem = shopItems[0];
            player.RemoveItemFromMarketAsync(shopItem.Id)
                .ContinueWith(t =>
                {
                    if (t.Result != MarketResult.Success)
                    {
                        player.SendError(t.Result.GetDescription());
                        return;
                    }

                    player.SendInfo("Removal succeeded. The item has been placed in your gift chest.");
                    player.Client.SendPacket(new GlobalNotification
                    {
                        Text = "giftChestOccupied"
                    });
                })
                .ContinueWith(e =>
                    Program.Debug(typeof(RMarketCommand), e.Exception.ToString(), true),
                    TaskContinuationOptions.OnlyOnFaulted);
            return true;
        }
    }

    //internal class MarketCommand : Command
    //{
    //    public MarketCommand() : base("market")
    //    {
    //    }

    //    private static readonly Regex _regex = new Regex(@"^(\d+) (\d+)$", RegexOptions.IgnoreCase);

    //    protected override bool Process(Player player, string args)
    //    {
    //        if (!(player.Owner is Marketplace))
    //        {
    //            player.SendError("Can only market items in Marketplace.");
    //            return false;
    //        }

    //        var match = _regex.Match(args);
    //        if (!match.Success || (match.Groups[1].Value.ToInt32()) > 16 || (match.Groups[1].Value.ToInt32()) < 1)
    //        {
    //            player.SendError("Usage: /market <slot> <amount>. Only slot numbers 1-16 are valid and amount must be a positive value.");
    //            return false;
    //        }

    //        if (!int.TryParse(match.Groups[2].Value, out var amount))
    //        {
    //            player.SendError("Amount is too large. Try something below 2147483648...");
    //            return false;
    //        }

    //        var slot = match.Groups[1].Value.ToInt32() + 3;

    //        var result = player.AddToMarket(slot, amount);
    //        if (result != MarketResult.Success)
    //        {
    //            player.SendError(result.GetDescription());
    //            return false;
    //        }

    //        player.SendInfo("Success! Your item has been placed on the market.");
    //        return true;
    //    }
    //}

    //internal class MarketAllCommand : Command
    //{
    //    public MarketAllCommand() : base("marketall", alias: "mall")
    //    {
    //    }

    //    private static readonly Regex Regex = new Regex(@"^([A-Za-z0-9 ]+) (\d+)$", RegexOptions.IgnoreCase);

    //    protected override bool Process(Player player, string args)
    //    {
    //        if (!(player.Owner is Marketplace))
    //        {
    //            player.SendError("Can only market items in Marketplace.");
    //            return false;
    //        }

    //        var match = Regex.Match(args);
    //        var gameData = player.Manager.Resources.GameData;
    //        var sold = 0;
    //        var err = false;

    //        if (!match.Success)
    //        {
    //            player.SendError("Usage: /marketall <item name> <price>.");
    //            return false;
    //        }

    //        string itemName = match.Groups[1].Value;

    //        // allow both DisplayId and Id for query
    //        if (!gameData.DisplayIdToObjectType.TryGetValue(itemName, out var objType))
    //        {
    //            if (!gameData.IdToObjectType.TryGetValue(itemName, out objType))
    //            {
    //                player.SendError("Unknown item type!");
    //                return false;
    //            }
    //        }

    //        if (!gameData.Items.ContainsKey(objType))
    //        {
    //            player.SendError("Unknown item type!");
    //            return false;
    //        }

    //        if (gameData.Items[objType].Soulbound)
    //        {
    //            player.SendError("Can't market soulbound items!");
    //            return false;
    //        }

    //        if (!int.TryParse(match.Groups[2].Value, out var price))
    //        {
    //            player.SendError("Price is too large. Try something below 2147483648...");
    //            return false;
    //        }

    //        for (var i = 4; i < player.Inventory.Length; i++)
    //        {
    //            if (player.Inventory[i]?.ObjectType != null && player.Inventory[i]?.ObjectType == objType)
    //            {
    //                var result = player.AddToMarket(i, price);
    //                if (result != MarketResult.Success)
    //                {
    //                    player.SendError(result.GetDescription());
    //                    err = true;
    //                }
    //                else
    //                {
    //                    sold++;
    //                }
    //            }
    //        }

    //        if (err)
    //        {
    //            if (sold > 0)
    //            {
    //                player.SendErrorFormat("Errors occurred, only {0} item{1} sold.", sold, sold > 1 ? "s" : "");
    //            }
    //            else
    //            {
    //                player.SendError("Errors occurred, couldn't market items.");
    //            }
    //        }
    //        else if (sold > 0)
    //        {
    //            player.SendInfoFormat("Success! Your {0} item{1} ha{2} been placed on the market.", sold, sold > 1 ? "s" : "", sold > 1 ? "ve" : "s");
    //        }
    //        else
    //        {
    //            player.SendErrorFormat("No {0} found in your inventory.", gameData.Items[objType].DisplayName);
    //        }

    //        return true;
    //    }
    //}

    //internal class MyMarketCommand : Command
    //{
    //    public MyMarketCommand() : base("myMarket")
    //    {
    //    }

    //    protected override bool Process(Player player, string args)
    //    {
    //        var shopItems = player.GetMarketItems();
    //        if (shopItems.Length <= 0)
    //        {
    //            player.SendInfo("You have no items currently listed on the market.");
    //            return true;
    //        }

    //        player.SendInfo($"Your items ({shopItems.Length}): (format: [id] Name, fame)");
    //        foreach (var shopItem in shopItems)
    //        {
    //            var item = player.Manager.Resources.GameData.Items[shopItem.ItemId];
    //            player.SendInfo($"[{shopItem.Id}] {item.DisplayName}, {shopItem.Price}");
    //        }
    //        return true;
    //    }
    //}

    internal class PriceCheckCommand : Command
    {
        public PriceCheckCommand() : base("pricecheck", alias: "pc")
        {
        }

        protected override bool Process(Player player, string args)
        {
            if (player?.Owner == null)
                return false;


            if (args.Length == 0)
            {
                player.SendInfo("Usage: /pc [item name]");
                return false;
            }

            var gameData = player.Manager.Resources.GameData;

            if (!gameData.DisplayIdToObjectType.TryGetValue(args, out var objType))
            {
                if (!gameData.IdToObjectType.TryGetValue(args, out objType))
                {
                    try
                    {
                        int slot = int.Parse(args) - 1;
                        // equips are slots 0-3; equipped items are used if the command input is 17-20
                        objType = player.Inventory[slot > 15 ? slot - 16 : slot + 4].ObjectType;
                    }
                    catch
                    {
                        player.SendError("Unknown item type: " + args);
                        return false;
                    }
                }
            }

            if (!gameData.Items.TryGetValue(objType, out var item))
            {
                player.SendError("Not an item!");
                return false;
            }

            DbMarketData[] items = DbMarketData.Get(player.Manager.Database.Conn, objType);
            if (items.Length == 0)
            {
                player.SendInfo(item.ObjectId + " is not currently being sold.");
                return false;
            }

            DbMarketData data = items.Aggregate((curmin, it) => (curmin == null || it.Price < curmin.Price ? it : curmin));
            string sellerName = player.AccountId == data.SellerId ? "you" : data.SellerName;
            player.SendInfo($"The cheapest {item.ObjectId} is being sold by {sellerName} for {data.Price} gold.");

            return true;
        }
    }

    #endregion MarketCommands
}
