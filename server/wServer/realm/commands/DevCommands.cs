using common;
using common.resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using System.Text;
using wServer.networking.packets.outgoing;
using wServer.realm.entities;
using wServer.realm.setpieces;
using wServer.realm.worlds;
using wServer.realm.worlds.logic;

namespace wServer.realm.commands
{
    public abstract class DevCommands
    {
        #region Utility

        internal class QuakeCommand : Command
        {
            public QuakeCommand() : base("quake", permLevel: Perms.Dev)
            {
            }

            protected override bool Process(Player player, string worldName)
            {
                var worldProtoData = player.Manager.Resources.Worlds.Data;

                if (string.IsNullOrWhiteSpace(worldName))
                {
                    var msg = worldProtoData.Aggregate(
                        "Valid World Names: ", (c, p) => c + ((!p.Value.setpiece) ? (p.Key + ", ") : ""));
                    player.SendInfo(msg.Substring(0, msg.Length - 2) + ".");
                    return false;
                }

                if (player.Owner is Nexus)
                {
                    player.SendError("Cannot use /quake in Nexus.");
                    return false;
                }
                var worldNameProper =
                    player.Manager.Resources.Worlds.Data.FirstOrDefault(
                        p => p.Key.Equals(worldName, StringComparison.InvariantCultureIgnoreCase)
                             || p.Value.sbName.Equals(worldName, StringComparison.InvariantCultureIgnoreCase)).Key;

                ProtoWorld proto;
                if (worldNameProper == null || (proto = worldProtoData[worldNameProper]).setpiece || worldNameProper == "Balancer Station")
                {
                    player.SendError("Invalid world.");
                    return false;
                }

                World world;
                if (proto.persist || proto.id == World.Arena || proto.id == World.DeathArena)
                    world = player.Manager.Worlds[proto.id];
                else
                {
                    DynamicWorld.TryGetWorld(proto, player.Client, out world);
                    world = player.Manager.AddWorld(world ?? new World(proto));
                }

                player.Owner.QuakeToWorld(world);
                return true;
            }
        }

        internal class RebootCommand : Command
        {
            // Command actually closes the program.
            // An external program is used to monitor the world server existance.
            // If !exist it automatically restarts it.

            public RebootCommand() : base("reboot", permLevel: Perms.Dev)
            {
            }

            protected override bool Process(Player player, string name)
            {
                var manager = player.Manager;
                var servers = manager.InterServer.GetServerList();

                // display help if no argument supplied
                if (string.IsNullOrEmpty(name))
                {
                    var sb = new StringBuilder("Current servers available for rebooting:\n");
                    for (var i = 0; i < servers.Length; i++)
                    {
                        if (i != 0)
                            sb.Append(", ");

                        sb.Append($"{servers[i].name} [{servers[i].type}]");
                    }

                    player.SendInfo("Usage: /reboot < server name | $all | $wserver | $account >");
                    player.SendInfo(sb.ToString());
                    return true;
                }

                // attempt to find server match
                foreach (var server in servers)
                {
                    if (!server.name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    RebootServer(player, 0, server.instanceId);
                    player.SendInfo("Reboot command sent.");
                    return true;
                }

                // no match found, attempt to match special cases
                switch (name.ToLower())
                {
                    case "$all":
                        RebootServer(player, 29000, servers
                            .Select(s => s.instanceId)
                            .ToArray());
                        player.SendInfo("Reboot command sent.");
                        return true;

                    case "$wserver":
                        RebootServer(player, 0, servers
                            .Where(s => s.type == ServerType.World)
                            .Select(s => s.instanceId)
                            .ToArray());
                        player.SendInfo("Reboot command sent.");
                        return true;

                    case "$account":
                        RebootServer(player, 0, servers
                            .Where(s => s.type == ServerType.Account)
                            .Select(s => s.instanceId)
                            .ToArray());
                        player.SendInfo("Reboot command sent.");
                        return true;
                }

                player.SendInfo("Server not found.");
                return false;
            }

            private void RebootServer(Player issuer, int delay, params string[] instanceIds)
            {
                foreach (var instanceId in instanceIds)
                {
                    issuer.Manager.InterServer.Publish(Channel.Control, new ControlMsg()
                    {
                        Type = ControlType.Reboot,
                        TargetInst = instanceId,
                        Issuer = issuer.Name,
                        Delay = delay
                    });
                }
            }
        }

        internal class TpQuestCommand : Command
        {
            public TpQuestCommand() : base("tq", permLevel: Perms.Dev)
            {
            }

            protected override bool Process(Player player, string args)
            {
                if (player.Quest == null)
                {
                    player.SendError("You do not have a quest!");
                    return false;
                }

                player.SetNewbiePeriod();
                player.TeleportPosition(player.Quest.RealX, player.Quest.RealY, true);
                player.SendInfo("Teleported to Quest Location: (" + player.Quest.X + ", " + player.Quest.Y + ")");
                return true;
            }
        }

        internal class SuppScoreCommand : Command
        {
            public SuppScoreCommand() : base("alert", permLevel: 0)
            {
            }

            protected override bool Process(Player player, string args)
            {
                player.SendInfo(player.Client.Account.ChanceDenom + " " + player.ChanceDenom);
                return true;
            }
        }

        internal class LBCCommand : Command
        {
            public LBCCommand() : base("lbc", permLevel: Perms.Dev)
            {
            }

            protected override bool Process(Player player, string args)
            {
                var Manager = player.Manager;
                var playerDesc = Manager.Resources.GameData.Classes[player.ObjectType];
                var maxed = playerDesc.Stats.Where((t, i) => player.Stats.Base[i] >= t.MaxValue).Count();
                if (player.AscensionEnabled)
                {
                    if (player.Stats.Base[0] >= playerDesc.Stats[0].MaxValue + 50) maxed++;
                    if (player.Stats.Base[1] >= playerDesc.Stats[1].MaxValue + 50) maxed++;

                    for (int i = 2; i < 9; i++)
                    {
                        if (player.Stats.Base[i] >= playerDesc.Stats[i].MaxValue + 10)
                        {
                            maxed++;
                        }
                    }
                    if (player.Stats.Base[9] >= playerDesc.Stats[9].MaxValue + 3) maxed++;
                }

                player.SendInfo("" + maxed);
                return true;

            }
        }

        internal class TryClearCommand : Command
        {
            public TryClearCommand() : base("tryClear", permLevel: Perms.Dev, listCommand: false)
            {
            }

            protected override bool Process(Player player, string name)
            {
                GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
                GC.Collect();
                return true;
            }
        }

        private class SetGoldCommand : Command
        {
            public SetGoldCommand() : base("setgold", permLevel: Perms.Dev, alias: "gold")
            {
            }

            protected override bool Process(Player player, string args)
            {
                var amount = int.Parse(args);

                if (string.IsNullOrEmpty(args))
                {
                    player.SendInfo("/gold <amount>");
                    return false;
                }
                player.Credits = player.Client.Account.Credits = amount;
                player.ForceUpdate(player.Credits);
                return true;
            }
        }
        private class SetOnraneCommand : Command
        {
            public SetOnraneCommand() : base("setonrane", permLevel: Perms.Dev, alias: "onrane")
            {
            }

            protected override bool Process(Player player, string args)
            {
                var amount = int.Parse(args);

                if (string.IsNullOrEmpty(args))
                {
                    player.SendInfo("/setonrane <amount>");
                    return false;
                }
                player.Onrane = player.Client.Account.Onrane = amount;
                player.ForceUpdate(player.Onrane);
                return true;
            }
        }

        internal class SetpieceCommand : Command
        {
            public SetpieceCommand() : base("setpiece", permLevel: Perms.Dev)
            {
            }

            protected override bool Process(Player player, string setPiece)
            {
                if (string.IsNullOrWhiteSpace(setPiece))
                {
                    var type = typeof(ISetPiece);
                    var types = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(s => s.GetTypes())
                        .Where(p => type.IsAssignableFrom(p) && !p.IsAbstract);
                    var msg = types.Aggregate(
                        "Valid SetPieces: ", (c, p) => c + (p.Name) + ", ");
                    player.SendInfo(msg.Substring(0, msg.Length - 2) + ".");
                    return false;
                }

                if (!player.Owner.Name.Equals("Nexus"))
                {
                    try
                    {
                        var piece = (ISetPiece)Activator.CreateInstance(Type.GetType(
                        "wServer.realm.setpieces." + setPiece, true, true));
                        piece.RenderSetPiece(player.Owner, new IntPoint((int)player.X + 1, (int)player.Y + 1));
                        return true;
                    }
                    catch (Exception)
                    {
                        player.SendError("Invalid SetPiece.");
                        return false;
                    }
                }
                else
                {
                    player.SendInfo("/setpiece not allowed in Nexus.");
                    return false;
                }
            }
        }

        internal class TpPosCommand : Command
        {
            public TpPosCommand() : base("tpPos", Perms.Dev, alias: "goto")
            {
            }

            protected override bool Process(Player player, string args)
            {
                var coordinates = args.Split(' ');
                if (coordinates.Length != 2)
                {
                    player.SendError("Invalid coordinates!");
                    return false;
                }

                if (!int.TryParse(coordinates[0], out int x) ||
                    !int.TryParse(coordinates[1], out int y))
                {
                    player.SendError("Invalid coordinates!");
                    return false;
                }

                player.SetNewbiePeriod();
                player.TeleportPosition(x + 0.5f, y + 0.5f, true);
                return true;
            }
        }

        internal class MusicCommand : Command
        {
            public MusicCommand() : base("music", Perms.Dev)
            {
            }

            protected override bool Process(Player player, string music)
            {
                var resources = player.Manager.Resources;

                if (string.IsNullOrWhiteSpace(music))
                {
                    var msg = resources.MusicNames.Aggregate(
                        "Music Choices: ", (c, p) => c + (p + ", "));
                    player.SendInfo(msg.Substring(0, msg.Length - 2) + ".");
                    return false;
                }

                var properName = resources.MusicNames
                    .FirstOrDefault(s => s.Equals(music, StringComparison.InvariantCultureIgnoreCase));
                if (properName == null)
                {
                    player.SendError($"Music \"{music}\" not found!");
                    return false;
                }

                var owner = player.Owner;
                owner.Music = properName;

                foreach (var plr in owner.Players.Values)
                    plr.SendInfo($"World music changed to {properName}.");

                var i = 0;
                foreach (var plr in owner.Players.Values)
                {
                    owner.Timers.Add(new WorldTimer(100 * i, (w) =>
                    {
                        if (w == null || w.Deleted || plr == null || plr.Client == null) return;

                        plr.Client.SendPacket(new SwitchMusic { Music = properName });
                    }));
                    i++;
                }
                return true;
            }
        }

        internal class WargCommand : Command
        {
            public WargCommand() : base("warg", Perms.Dev)
            {
            }

            protected override bool Process(Player player, string name)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    player.SendError("Usage: /warg <mob name>");
                    return false;
                }

                var target = player.GetNearestEntityByName(2900, name);
                if (target == null)
                {
                    player.SendError("Mob not found.");
                    return false;
                }

                if (target.Controller != null)
                {
                    player.SendError("Only one person can control a mob at a time.");
                    return false;
                }

                if (player.SpectateTarget != null)
                {
                    player.SpectateTarget.FocusLost -= player.ResetFocus;
                    player.SpectateTarget.Controller = null;
                }

                target.FocusLost += player.ResetFocus;
                target.Controller = player;
                player.SpectateTarget = target;
                player.Sight.UpdateCount++;

                player.Owner.Timers.Add(new WorldTimer(500, (w) =>
                {
                    if (w == null || w.Deleted || player == null || player.Client == null) return;

                    player.Client.SendPacket(new SetFocus { ObjectId = target.Id });
                }));
                return true;
            }
        }

        internal class CoreAlgorithmDebugCommand : Command
        {
            private readonly Dictionary<string, string> commands = new Dictionary<string, string>()
            {
                { "-help", "Display a help message for this command." },
                { "-ct debug", "[CoreTimer] Creates 250.000 entries to being processed on current world instance within 3 seconds." },
                { "-ct count", "[CoreTimer] Display total of pending entries that exist into CoreTimer of current world instance." }
            };

            public CoreAlgorithmDebugCommand() : base("cad", permLevel: Perms.Dev)
            {
            }

            protected override bool Process(Player player, string args)
            {
                if (string.IsNullOrEmpty(args))
                {
                    player.SendHelp("Usage: /cad -help");
                    return false;
                }

                if (args == "-help")
                {
                    player.SendInfo("Available commands:");

                    foreach (var command in commands)
                        player.SendInfoFormat("{0}: {1}", command.Key, command.Value);

                    return false;
                }

                if (args == "-ct debug")
                {
                    player.SendInfoFormat("Creating 250.000 entries to being processed on world {0} (ID: {1})...",
                        player.Owner.Name, player.Owner.Id);

                    var entries = new WorldTimer[250000];

                    for (var i = 0; i < entries.Length; i++)
                        entries[i] = new WorldTimer(3000, (w) => { });

                    player.SendInfo("All entries have been created! Staging into current world...");

                    var watch = Stopwatch.StartNew();

                    for (var i = 0; i < entries.Length; i++)
                        player.Owner.Timers.Add(entries[i]);

                    watch.Stop();

                    player.SendInfoFormat("All entries have been processed! This process took: {0} ({1} ms)",
                        watch.Elapsed, watch.ElapsedMilliseconds);
                    return true;
                }

                if (args == "-ct count")
                {
                    var amount = player.Owner.Timers.Count;

                    if (amount == 0)
                        player.SendInfoFormat("There is no pending timer to being processed on world {0} (ID: {1}).",
                            player.Owner.Name, player.Owner.Id);
                    else
                        player.SendInfoFormat("There {0} {1} pending timer{2} to being processed on world {3} (ID: {4}).",
                            amount > 1 ? "are" : "is", amount, amount > 1 ? "s" : "", player.Owner.Name, player.Owner.Id);

                    return true;
                }

                player.SendError("Invalid action type for this command, try: /cad -help");
                return false;
            }
        }

        internal class DeveloperUtilsCommand : Command
        {
            private readonly Dictionary<string, string> commands = new Dictionary<string, string>()
            {
                { "-help", "Display a help message for this command." },
                { "-addeff", "Add a one or group of effects (you can use effect ID or name for this operation). Usage: /dutils -addeff damaging speedy 23" }
            };

            public DeveloperUtilsCommand() : base("dutils", permLevel: Perms.Dev)
            {
            }

            protected override bool Process(Player player, string args)
            {
                if (string.IsNullOrEmpty(args))
                {
                    player.SendHelp("Usage: /dutils -help");
                    return false;
                }

                if (args == "-help")
                {
                    player.SendInfo("Available commands:");

                    foreach (var command in commands)
                        player.SendInfoFormat("{0}: {1}", command.Key, command.Value);

                    return false;
                }

                if (args.StartsWith("-addeff"))
                {
                    args = args.Remove(0, "-addeff".Length);

                    if (string.IsNullOrWhiteSpace(args))
                    {
                        player.SendError("This command require extra arguments to execute.");
                        return false;
                    }

                    var possibleEffects = args.Split(' ').Where(eff => !string.IsNullOrWhiteSpace(eff)).ToList();
                    var eligibleEffects = new List<ConditionEffectIndex>();
                    var nonEligibleEffects = new List<string>();

                    foreach (var possibleEffect in possibleEffects)
                        if (Enum.TryParse(possibleEffect, true, out ConditionEffectIndex effect)) eligibleEffects.Add(effect);
                        else nonEligibleEffects.Add(possibleEffect);

                    if (eligibleEffects.Count == 0)
                    {
                        player.SendInfo("There is no effect to apply!");
                        return false;
                    }

                    var addedEffects = new List<ConditionEffectIndex>();

                    foreach (var eligibleEffect in eligibleEffects)
                        if (!((player.ConditionEffects & (ConditionEffects)((ulong)1 << (int)eligibleEffect)) != 0))
                        {
                            player.ApplyConditionEffect(new ConditionEffect
                            {
                                Effect = eligibleEffect,
                                DurationMS = -1
                            });
                            addedEffects.Add(eligibleEffect);
                        }

                    var addedEffectNotified = false;
                    var nonEligibleEffectsNotified = false;

                    if (addedEffects.Count != 0)
                    {
                        addedEffectNotified = true;

                        player.SendInfoFormat("You received following effect{0}:", addedEffects.Count > 1 ? "s" : "");

                        foreach (var addedEffect in addedEffects)
                            player.SendInfoFormat("- {0}", addedEffect.ToString().ToLower());
                    }

                    if (nonEligibleEffects.Count != 0)
                    {
                        nonEligibleEffectsNotified = true;

                        player.SendErrorFormat("Following effect{0} supported:", nonEligibleEffects.Count > 1 ? "s aren't" : " isn't");

                        foreach (var nonEligibleEffect in nonEligibleEffects)
                            player.SendErrorFormat("- {0}", nonEligibleEffect);
                    }

                    if (!addedEffectNotified && !nonEligibleEffectsNotified)
                    {
                        player.SendInfo("Nothing happened.");
                        return false;
                    }

                    return true;
                }

                player.SendError("Invalid action type for this command, try: /dutils -help");
                return false;
            }
        }

        #endregion Utility
    }
}
