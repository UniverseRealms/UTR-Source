using common;
using common.resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using wServer.networking.packets.outgoing;
using wServer.realm.entities;
using wServer.realm.worlds;
using wServer.realm.worlds.logic;
using DiscordWebhook;

namespace wServer.realm.commands
{
    internal class BalanceCommand : Command
    {
        public BalanceCommand() : base("balance", Perms.Content, alias: "nerftime")
        {
        }

        protected override bool Process(Player player, string args)
        {
            World world = player.Manager.AddWorld(new BalancerStation(player.Manager.Resources.Worlds.Data["Balancer Station"]));
            if (world != null)
            {
                player.Reconnect(world);
                return true;
            }
            return false;
        }
    }

    #region BalStationCommands

    internal class EffectCommand : Command
    {
        public EffectCommand() : base("eff", Perms.HeadMod, world: "Balancer Station")
        {
        }

        protected override bool Process(Player player, string args)
        {
            if (!Enum.TryParse(args, true, out ConditionEffectIndex effect))
            {
                player.SendError("Invalid effect!");
                return false;
            }

            var target = player.IsControlling ? player.SpectateTarget : player;
            if ((target.ConditionEffects & (ConditionEffects)((ulong)1 << (int)effect)) != 0)
            {
                //remove
                target.ApplyConditionEffect(new ConditionEffect()
                {
                    Effect = effect,
                    DurationMS = 0
                });
            }
            else
            {
                //add
                target.ApplyConditionEffect(new ConditionEffect()
                {
                    Effect = effect,
                    DurationMS = -1
                });
            }
            return true;
        }
    }

    internal class SpawnCommand : Command
    {
        private struct JsonSpawn
        {
            public string notif;
            public SpawnProperties[] spawns;
        }

        private struct SpawnProperties
        {
            public string name;
            public int? hp;
            public int? size;
            public int? count;
            public int[] x;
            public int[] y;
            public bool? target;
        }

        private const int Delay = 3; // in seconds

        public SpawnCommand() : base("spawn", Perms.HeadMod, world: "Balancer Station")
        {
        }

        protected override bool Process(Player player, string args)
        {
            args = args.Trim();
            return args.StartsWith("{") ?
                SpawnJson(player, args) :
                SpawnBasic(player, args);
        }

        private bool SpawnJson(Player player, string json)
        {
            var gameData = player.Manager.Resources.GameData;

            JsonSpawn props;
            try
            {
                props = JsonConvert.DeserializeObject<JsonSpawn>(json);
            }
            catch (Exception)
            {
                player.SendError("JSON not formatted correctly!");
                return false;
            }

            if (player.Owner.Name == "Nexus")
            {
                player.SendError("Can't spawn in the nexus.");
            }

            if (props.spawns != null)
                foreach (var spawn in props.spawns)
                {
                    if (spawn.name == null)
                    {
                        player.SendError("No mob specified. Every entry needs a name property.");
                        return false;
                    }

                    var objType = GetSpawnObjectType(gameData, spawn.name);
                    if (objType == null)
                    {
                        player.SendError("Unknown entity!");
                        return false;
                    }

                    var desc = gameData.ObjectDescs[objType.Value];

                    var hp = desc.MaxHP;
                    if (spawn.hp > hp && spawn.hp < int.MaxValue)
                        hp = spawn.hp.Value;

                    var size = desc.MinSize;
                    if (spawn.size >= 25 && spawn.size <= 500)
                        size = spawn.size.Value;

                    var count = 1;
                    if (spawn.count > count)
                    {
                        if (spawn.count <= 200 && player.Rank <= 90)
                            count = spawn.count.Value;
                        else
                        {
                            player.SendError("Maximum spawn count is 200.");
                            return false;
                        }
                        if(player.Rank > 90)
                        {
                            count = spawn.count.Value;
                        }
                    }
                     

                    int[] x = null;
                    int[] y = null;

                    if (spawn.x != null)
                        x = new int[spawn.x.Length];

                    if (spawn.y != null)
                        y = new int[spawn.y.Length];

                    if (x != null)
                    {
                        for (var i = 0; i < x.Length && i < count; i++)
                        {
                            if (spawn.x[i] > 0 && spawn.x[i] <= player.Owner.Map.Width)
                            {
                                x[i] = spawn.x[i];
                            }
                        }
                    }

                    if (y != null)
                    {
                        for (var i = 0; i < y.Length && i < count; i++)
                        {
                            if (spawn.y[i] > 0 && spawn.y[i] <= player.Owner.Map.Height)
                            {
                                y[i] = spawn.y[i];
                            }
                        }
                    }

                    var target = false;
                    if (spawn.target != null)
                        target = spawn.target.Value;

                    QueueSpawnEvent(player, count, objType.Value, hp, size, x, y, target);
                }

            if (props.notif != null)
            {
                NotifySpawn(player, props.notif);
            }

            return true;
        }

        private bool SpawnBasic(Player player, string args)
        {
            var gameData = player.Manager.Resources.GameData;

            // split argument
            var index = args.IndexOf(' ');
            var name = args;
            if (args.IndexOf(' ') > 0 && int.TryParse(args.Substring(0, args.IndexOf(' ')), out int num)) //multi
                name = args.Substring(index + 1);
            else
                num = 1;
            if (num > 1 && player.Rank < 80)
            {
                player.SendError("Maximum spawn count is 1.");
                return false;
            }

            if (num > 200)
            {
                player.SendError("Maximum spawn count is 200.");
                return false;
            }

            var objType = GetSpawnObjectType(gameData, name);
            if (objType == null)
            {
                player.SendError("Unknown entity!");
                return false;
            }

            if (num <= 0)
            {
                player.SendInfo("You can not summon negative amounts.");
                return false;
            }

            var id = player.Manager.Resources.GameData.ObjectTypeToId[objType.Value];
            if (player.Client.Account.Rank < 100 &&
                player.Owner is DeathArena &&
                id.Contains("Fountain"))
            {
                player.SendError("Insufficient rank.");
                return false;
            }

            NotifySpawn(player, id, num);
            QueueSpawnEvent(player, num, objType.Value);
            return true;
        }

        private ushort? GetSpawnObjectType(XmlData gameData, string name)
        {
            if (!gameData.IdToObjectType.TryGetValue(name, out ushort objType) ||
                !gameData.ObjectDescs.ContainsKey(objType))
            {
                // no match found, try to get partial match
                var mobs = gameData.IdToObjectType
                    .Where(m => m.Key.ContainsIgnoreCase(name) && gameData.ObjectDescs.ContainsKey(m.Value))
                    .Select(m => gameData.ObjectDescs[m.Value]);

                if (!mobs.Any())
                    return null;

                var maxHp = mobs.Max(e => e.MaxHP);
                objType = mobs.First(e => e.MaxHP == maxHp).ObjectType;
            }

            return objType;
        }

        private void NotifySpawn(Player player, string mob, int? num = null)
        {
            var w = player.Owner;

            var notif = mob;
            if (num != null)
                notif = "Spawning " + ((num > 1) ? num + " " : "") + mob + "...";

            w.BroadcastPacket(new Notification
            {
                Color = new ARGB(0xffff0000),
                ObjectId = (player.IsControlling) ? player.SpectateTarget.Id : player.Id,
                Message = notif
            }, null);

            if (player.IsControlling)
                w.BroadcastPacket(new Text
                {
                    Name = $"#{player.SpectateTarget.ObjectDesc.DisplayId}",
                    NumStars = -1,
                    BubbleTime = 0,
                    Txt = notif
                }, null);
            else
                w.BroadcastPacket(new Text
                {
                    Name = $"#{player.Name}",
                    NumStars = player.Stars,
                    Admin = player.Admin,
                    BubbleTime = 0,
                    Txt = notif
                }, null);
        }

        private void QueueSpawnEvent(
            Player player,
            int num,
            ushort mobObjectType, int? hp = null, int? size = null,
            int[] x = null, int[] y = null,
            bool? target = false)
        {
            var pX = player.X;
            var pY = player.Y;

            player.Owner.Timers.Add(new WorldTimer(Delay * 1000, (w) => // spawn mob in delay seconds
            {
                if (w == null || w.Deleted) return;

                for (var i = 0; i < num && i < 500; i++)
                {
                    Entity entity;

                    try { entity = Entity.Resolve(w.Manager, mobObjectType); }
                    catch (Exception e)
                    {
                        Program.Debug(typeof(SpawnCommand), e.ToString(), true);
                        return;
                    }

                    entity.Spawned = true;

                    if (entity is Character character)
                    {
                        if (hp != null)
                        {
                            character.HP = hp.Value;
                            character.MaximumHP = character.HP;
                            (entity as Enemy).SpawnedHp = hp.Value;
                            
                        }
                        if (size != null) character.SetDefaultSize(size.Value);
                        if (entity is Enemy && target == true) character.AttackTarget = player;
                        if (entity is Ally) character.SetPlayerOwner(player);

                    }

                    var sX = (x != null && i < x.Length) ? x[i] : pX;
                    var sY = (y != null && i < y.Length) ? y[i] : pY;

                    entity.Move(sX, sY);
                    w.EnterWorld(entity);
                }
            }));
        }
    }

    internal class ClearSpawnsCommand : Command
    {
        public ClearSpawnsCommand() : base("clearspawn", Perms.HeadMod, alias: "cs", world: "Balancer Station")
        {
        }

        protected override bool Process(Player player, string args)
        {
            var iterations = 0;
            var lastKilled = -1;
            var removed = 0;
            while (removed != lastKilled)
            {
                lastKilled = removed;
                foreach (var entity in player.Owner.Enemies.Values.Where(e => e.Spawned == true))
                {
                    entity.Death();
                    removed++;
                }
                foreach (var entity in player.Owner.StaticObjects.Values.Where(e => e.Spawned == true))
                {
                    player.Owner.LeaveWorld(entity);
                    removed++;
                }
                foreach (var entity in player.Owner.Allies.Values.Where(e => e.Spawned == true))
                {
                    player.Owner.LeaveWorld(entity);
                    removed++;
                }
                if (++iterations >= 5)
                    break;
            }

            player.SendInfo($"{removed} spawned entities removed!");
            return true;
        }
    }

    internal class KillAllCommand : Command
    {
        public KillAllCommand() : base("killAll", Perms.HeadMod, alias: "ka", world: "Balancer Station")
        {
        }

        protected override bool Process(Player player, string args)
        {
            var iterations = 0;
            var lastKilled = -1;
            var killed = 0;
            while (killed != lastKilled)
            {
                lastKilled = killed;
                foreach (var i in player.Owner.Enemies.Values.Where(e =>
                    e.ObjectDesc?.ObjectId != null
                    && e.ObjectDesc.Enemy
                    && e.ObjectDesc.ObjectId.ContainsIgnoreCase(args)))
                {
                    i.Spawned = true;
                    i.Death();
                    killed++;
                }
                if (++iterations >= 5)
                    break;
            }

            player.SendInfo($"{killed} enemy killed!");
            return true;
        }
    }
    internal class GimmeCommand : Command
    {
        public GimmeCommand() : base("gimme", Perms.HeadMod, alias: "give", world: "Balancer Station")
        {
        }

        private List<string> Blacklist = new List<string>
            {
            "admin sword", "admin wand", "admin staff", "admin dagger", "admin bow", "admin katana", "godly crown",
            "dream essence", "boshy gun", "crown", "test essence"
            };

        protected override bool Process(Player player, string args)
        {
            var gameData = player.Manager.Resources.GameData;

            string name = args.ToLower();

            if (Blacklist.Any(e => e.Equals(name)) && player.Rank < 90)
            {
                player.SendError("Yeah, no.");
                return false;
            }

            // allow both DisplayId and Id for query
            if (!gameData.DisplayIdToObjectType.TryGetValue(args, out var objType))
            {
                if (!gameData.IdToObjectType.TryGetValue(args, out objType))
                {
                    player.SendError("Unknown item type!");
                    return false;
                }
            }

            if (!gameData.Items.ContainsKey(objType))
            {
                player.SendError("Not an item!");
                return false;
            }

            var item = gameData.Items[objType];

            var availableSlot = player.Inventory.GetAvailableInventorySlot(item);
            if (availableSlot != -1)
            {
                player.Inventory[availableSlot] = item;
                Program.DL.LogToDiscord(WebHooks.GiveItem, "Give Log", $"Give Log: [{player.Name}] has given themselves a [{item.DisplayName}]");
                return true;
            }

            player.SendError("Not enough space in inventory!");
            return false;
        }
    }
    internal class PowerAscendCommand : Command
    {
        public PowerAscendCommand() : base("powerascend", Perms.Mod)
        {
        }

        protected override bool Process(Player player, string args)
        {
            player.AscensionEnabled = true;
            var pd = player.Manager.Resources.GameData.Classes[player.ObjectType];
            player.Stats.Base[0] = pd.Stats[0].MaxValue + 50;
            player.Stats.Base[1] = pd.Stats[1].MaxValue + 50;
            player.Stats.Base[2] = pd.Stats[2].MaxValue + 10;
            player.Stats.Base[3] = pd.Stats[3].MaxValue + 10;
            player.Stats.Base[4] = pd.Stats[4].MaxValue + 10;
            player.Stats.Base[5] = pd.Stats[5].MaxValue + 10;
            player.Stats.Base[6] = pd.Stats[6].MaxValue + 10;
            player.Stats.Base[7] = pd.Stats[7].MaxValue + 10;
            player.Stats.Base[8] = pd.Stats[8].MaxValue + 10;
            player.Stats.Base[9] = pd.Stats[9].MaxValue + 3;
            player.SendInfo("Your character stats have been fully ascended.");
            return true;
        }
    }

    internal class ClearInvCommand : Command
    {
        public ClearInvCommand() : base("clearinv", Perms.HeadMod, world: "Balancer Station")
        {
        }

        protected override bool Process(Player player, string args)
        {
            for (var i = 4; i < 12; i++)
                player.Inventory[i] = null;
            player.SendInfo("Inventory Cleared.");
            return true;
        }
    }

    #endregion BalStationCommands
}
