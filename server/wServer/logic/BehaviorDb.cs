using common.resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using wServer.logic.loot;
using wServer.networking.packets.outgoing;
using wServer.realm;
using wServer.realm.entities;

namespace wServer.logic
{
    public partial class BehaviorDb
    {
        public RealmManager Manager { get; }

        private static int _initializing;
        private static Random rand = new Random();
        internal static BehaviorDb InitDb;
        internal static XmlData InitGameData => InitDb.Manager.Resources.GameData;

        public BehaviorDb(RealmManager manager)
        {
            Manager = manager;

            Definitions = new Dictionary<ushort, Tuple<State, Loot>>();

            if (Interlocked.Exchange(ref _initializing, 1) == 1)
            {
                throw new InvalidOperationException("Attempted to initialize multiple BehaviorDb at the same time.");
            }
            InitDb = this;

            var fields = GetType()
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(field => field.FieldType == typeof(_))
                .ToArray();
            for (var i = 0; i < fields.Length; i++)
            {
                var field = fields[i];
                ((_)field.GetValue(this))();
                field.SetValue(this, null);
            }

            InitDb = null;
            _initializing = 0;
        }

        public void ResolveBehavior(Entity entity)
        {
            if (Definitions.TryGetValue(entity.ObjectType, out var def))
                entity.SwitchTo(def.Item1);
        }

        private delegate ctor _();

        private struct ctor
        {
            public ctor Init(string objType, State rootState, params ILootDef[] defs)
            {
                var d = new Dictionary<string, State>();
                rootState.Resolve(d);
                rootState.ResolveChildren(d);
                var dat = InitDb.Manager.Resources.GameData;
                rootState.Death += (sender, e) => { if (e.Host is Enemy) handleExtraDrops(((Enemy)e.Host).DamageCounter.GetPlayerData(), (Enemy)e.Host); };
                if (defs.Length > 0)
                {
                    var loot = new Loot(defs);
                    rootState.Death += (sender, e) => { if (e.Host is Enemy) loot.Handle((Enemy)e.Host); };
                    if (dat.IdToObjectType.ContainsKey(objType))
                        InitDb.Definitions.Add(dat.IdToObjectType[objType], new Tuple<State, Loot>(rootState, loot));
                }
                else
                {
                    if (dat.IdToObjectType.ContainsKey(objType))
                        InitDb.Definitions.Add(dat.IdToObjectType[objType], new Tuple<State, Loot>(rootState, null));
                }
                return this;
            }
        }

        private static ctor Behav()
        {
            return new ctor();
        }

        public Dictionary<ushort, Tuple<State, Loot>> Definitions { get; }

        private static void handleExtraDrops(Tuple<Player, int>[] playerDamage, Enemy e)
        {
            double val;


            if (e.Owner is realm.worlds.logic.Test)
                return;

            foreach (var playerInfo in playerDamage)
            {
                if (e.GivesNoXp || e.Spawned || e.ObjectDesc.ExpMultiplier == 0)
                    continue;

                

                Player player = playerInfo.Item1;
                val = rand.NextDouble();
                if (player.Client.Account.ChanceDenom <= 0 || player.ChanceDenom <= 0)
                {
                    player.Client.Account.ChanceDenom = player.ChanceDenom = 2000;
                    player.ForceUpdate(player.ChanceDenom);
                }
                if (val <= 1.0 / player.ChanceDenom)
                {
                    int remaining = player.ChanceDenom;
                    player.Client.Account.ChanceDenom = player.ChanceDenom = 2000;
                    player.ForceUpdate(player.ChanceDenom);
                    player.Client.Manager.Database.UpdateEliteLootbox(player.Client.Account, 1);
                    player.EliteLootbox += 1;
                    player.ForceUpdate(player.EliteLootbox);
                    var packet = new Text
                    {
                        BubbleTime = 0,
                        NumStars = -1,
                        TextColor = 0x8700d6,
                        Name = "ALERT",
                        Txt = "An Elite Lootbox awaits you. You had " + remaining + " expected kills left."
                    };
                    player.Client.SendPacket(packet);
                }
                else
                {
                    int amount = player.ChanceDenom;
                    if (e.ObjectDesc.Quest)
                        amount -= 30;
                    else
                    amount--;
                    player.Client.Account.ChanceDenom = player.ChanceDenom = Math.Max(1, amount);
                }
            }

            if (e.ObjectDesc.AirDrop > 0)
            {
                Text Valuable = new Text()
                {
                    Name = "Elemental Aspect",
                    NumStars = -1,
                    Txt = "",
                    BubbleTime = 0,
                    NameColor = 0xFFFFFF,
                    TextColor = 0x0B8D45
                };
                foreach (var playerInfo in playerDamage)
                {
                    var boost = rand.Next(5, 15);
                    boost += e.ObjectDesc.AirDrop * 3;
                    Player player = playerInfo.Item1;
                    player.SendInfo("You have obtained " + boost + " Air Fragments");
                    player.Client.Manager.Database.UpdateAirStorage(player.Client.Account, boost);
                    player.AirStorage += e.ObjectDesc.AirDrop + boost;
                    player.ForceUpdate(player.AirStorage);
                    val = rand.NextDouble();
                    if (val < e.ObjectDesc.AirDrop / 2500.0)
                    {
                        Valuable.Txt = $"{player.Name} is sheared in wind, but emerges with a strange power.";
                        player.CurrWorld(Valuable);
                        player.Client.Manager.Database.UpdateAirStorage(player.Client.Account, 250);
                        player.AirStorage += 250;
                        player.ForceUpdate(player.AirStorage);
                        player.Owner.BroadcastPacketPrivate(new ShowEffect()
                        {
                            EffectType = EffectType.Flashing,
                            Pos1 = new Position() { X = 1, Y = 5 },
                            TargetObjectId = player.Id,
                            Color = new ARGB(0x0B8D45)
                        }, player);
                    }
                }
            }
            if (e.ObjectDesc.WaterDrop > 0)
            {
                Text Valuable = new Text()
                {
                    Name = "Elemental Aspect",
                    NumStars = -1,
                    Txt = "",
                    BubbleTime = 0,
                    NameColor = 0xFFFFFF,
                    TextColor = 0x43DFEB
                };
                foreach (var playerInfo in playerDamage)
                {
                    var boost = rand.Next(5, 15);
                    boost += e.ObjectDesc.WaterDrop * 3;
                    Player player = playerInfo.Item1;
                    player.SendInfo("You have obtained " + boost + " Water Fragments");
                    player.Client.Manager.Database.UpdateWaterStorage(player.Client.Account, boost);
                    player.WaterStorage += e.ObjectDesc.WaterDrop + boost;
                    player.ForceUpdate(player.WaterStorage);
                    val = rand.NextDouble();
                    if (val < e.ObjectDesc.WaterDrop / 2500.0)
                    {
                        Valuable.Txt = $"{player.Name} is submerged in waves, but emerges with a strange power.";
                        player.CurrWorld(Valuable);
                        player.Client.Manager.Database.UpdateWaterStorage(player.Client.Account, 250);
                        player.WaterStorage += 250;
                        player.ForceUpdate(player.WaterStorage);
                        player.Owner.BroadcastPacketPrivate(new ShowEffect()
                        {
                            EffectType = EffectType.Flashing,
                            Pos1 = new Position() { X = 1, Y = 5 },
                            TargetObjectId = player.Id,
                            Color = new ARGB(0x43DFEB)
                        }, player);
                    }
                }
            }

            if (e.ObjectDesc.EarthDrop > 0)
            {
                Text Valuable = new Text()
                {
                    Name = "Elemental Aspect",
                    NumStars = -1,
                    Txt = "",
                    BubbleTime = 0,
                    NameColor = 0xFFFFFF,
                    TextColor = 0x9B5523
                };
                foreach (var playerInfo in playerDamage)
                {
                    var boost = rand.Next(5, 15);
                    boost += e.ObjectDesc.EarthDrop * 3;
                    Player player = playerInfo.Item1;
                    player.SendInfo("You have obtained " + boost + " Earth Fragments");
                    player.Client.Manager.Database.UpdateEarthStorage(player.Client.Account, boost);
                    player.EarthStorage += e.ObjectDesc.EarthDrop + boost;
                    player.ForceUpdate(player.EarthStorage);
                    val = rand.NextDouble();
                    if (val < e.ObjectDesc.EarthDrop / 2500.0)
                    {
                        Valuable.Txt = $"{player.Name} is entombed in stone, but emerges with a strange power.";
                        player.CurrWorld(Valuable);
                        player.Client.Manager.Database.UpdateEarthStorage(player.Client.Account, 250);
                        player.EarthStorage += 250;
                        player.ForceUpdate(player.EarthStorage);
                        player.Owner.BroadcastPacketPrivate(new ShowEffect()
                        {
                            EffectType = EffectType.Flashing,
                            Pos1 = new Position() { X = 1, Y = 5 },
                            TargetObjectId = player.Id,
                            Color = new ARGB(0x9B5523)
                        }, player);
                    }
                }
            }

            if (e.ObjectDesc.FireDrop > 0)
            {
                Text Valuable = new Text()
                {
                    Name = "Elemental Aspect",
                    NumStars = -1,
                    Txt = "",
                    BubbleTime = 0,
                    NameColor = 0xFFFFFF,
                    TextColor = 0xFF2400
                };
                foreach (var playerInfo in playerDamage)
                {
                    var boost = rand.Next(5, 15);
                    boost += e.ObjectDesc.FireDrop * 3;
                    Player player = playerInfo.Item1;
                    player.SendInfo("You have obtained " + boost + " Fire Fragments");
                    player.Client.Manager.Database.UpdateFireStorage(player.Client.Account, boost);
                    player.FireStorage += e.ObjectDesc.FireDrop + boost;
                    player.ForceUpdate(player.FireStorage);
                    val = rand.NextDouble();
                    if (val < e.ObjectDesc.FireDrop / 2500.0)
                    {
                        Valuable.Txt = $"{player.Name} is engulfed in flame, but emerges with a strange power.";
                        player.CurrWorld(Valuable);
                        player.Client.Manager.Database.UpdateFireStorage(player.Client.Account, 250);
                        player.FireStorage += 250;
                        player.ForceUpdate(player.FireStorage);
                        player.Owner.BroadcastPacketPrivate(new ShowEffect()
                        {
                            EffectType = EffectType.Flashing,
                            Pos1 = new Position() { X = 1, Y = 5 },
                            TargetObjectId = player.Id,
                            Color = new ARGB(0xFF2400)
                        }, player);
                    }
                }
            }

            if (e.ObjectDesc.GoldDrop > 0)
            {
                foreach (var playerInfo in playerDamage)
                {
                    var boost = rand.Next(1, 50);
                    boost += e.ObjectDesc.GoldDrop;
                    Player player = playerInfo.Item1;
                    player.SendInfo("You have obtained " + boost + " Coins");
                    player.Client.Manager.Database.UpdateCredit(player.Client.Account, e.ObjectDesc.GoldDrop + boost);
                    player.Credits += e.ObjectDesc.GoldDrop + boost;
                    player.ForceUpdate(player.Credits);
                }
            }
            if (e.ObjectDesc.OnraneDrop > 0)
            {
                foreach (var playerInfo in playerDamage)
                {
                    var boost = rand.Next(1, 10);
                    boost += e.ObjectDesc.OnraneDrop;
                    Player player = playerInfo.Item1;
                    player.SendInfo("You have obtained " + boost + " Onrane");
                    player.Client.Manager.Database.UpdateOnrane(player.Client.Account, e.ObjectDesc.OnraneDrop + boost);
                    player.Onrane += e.ObjectDesc.OnraneDrop + boost;
                    player.ForceUpdate(player.Onrane);
                }
            }

            if (e.ObjectDesc.EternalDrop > 0)
            {
                foreach (var playerInfo in playerDamage)
                {
                    var boost = rand.Next(1, 10);
                    boost += e.ObjectDesc.EternalDrop;
                    Player player = playerInfo.Item1;
                    player.SendInfo("You have obtained " + boost + " Eternal Fragments");
                    player.Client.Manager.Database.UpdateSorStorage(player.Client.Account, e.ObjectDesc.EternalDrop + boost);
                    player.SorStorage += e.ObjectDesc.EternalDrop + boost;
                    player.ForceUpdate(player.SorStorage);
                }
            }

            if (e.ObjectDesc.Lootdrop)
            {
                foreach (var playerInfo in playerDamage)
                {
                    val = rand.NextDouble();
                    Player player = playerInfo.Item1;
                    if (val <= (0.25))
                    {
                        player.Client.Manager.Database.UpdateGoldLootbox(player.Client.Account, 1);
                        player.GoldLootbox++;
                        player.ForceUpdate(player.GoldLootbox);
                        player.Owner.BroadcastPacketPrivate(new Notification
                        {
                            Color = new ARGB(0xDEC612),
                            ObjectId = player.Id,
                            Message = "Gold Lootbox"
                        }, player);
                    }
                }
            }



            //if (e.ObjectDesc.Elitedrop)
            //{
            //    foreach (var playerInfo in playerDamage)
            //    {
            //        val = rand.NextDouble();
            //        double baseEliteBoxChance = 0.6;
            //        double changePerBoxReceived = 0.4;
            //        Player player = playerInfo.Item1;
            //        int count = 0;
            //        while (val <= baseEliteBoxChance)
            //        {
            //            count++;
            //            baseEliteBoxChance *= changePerBoxReceived;
            //            val = rand.NextDouble();
            //        }
            //        if (count > 0)
            //        {
            //            player.Client.Manager.Database.UpdateEliteLootbox(player.Client.Account, count);
            //            player.EliteLootbox += count;
            //            player.ForceUpdate(player.EliteLootbox);
            //            var packet = new Text
            //            {
            //                BubbleTime = 0,
            //                NumStars = -1,
            //                TextColor = 0x04ff89,
            //                Name = "#Elite Lootbox Notifier",
            //                Txt = $"You have received {(count == 1 ? "an Elite Lootbox Key" : $"{count} Elite Lootbox Keys")}!"
            //            };
            //            player.Client.SendPacket(packet);
            //        }
            //    }
            //}

            //if (e.ObjectDesc.UElitedrop)
            //{
            //    foreach (var playerInfo in playerDamage)
            //    {
            //        val = rand.NextDouble();
            //        double baseEliteBoxChance = 1;
            //        double changePerBoxReceived = 0.5;
            //        Player player = playerInfo.Item1;
            //        int count = 0;
            //        while (val <= baseEliteBoxChance)
            //        {
            //            count++;
            //            baseEliteBoxChance *= changePerBoxReceived;
            //            val = rand.NextDouble();
            //        }
            //        if (count > 0)
            //        {
            //            player.Client.Manager.Database.UpdateEliteLootbox(player.Client.Account, count);
            //            player.EliteLootbox += count;
            //            player.ForceUpdate(player.EliteLootbox);
            //            var packet = new Text
            //            {
            //                BubbleTime = 0,
            //                NumStars = -1,
            //                TextColor = 0x04ff89,
            //                Name = "#Elite Lootbox Notifier",
            //                Txt = $"You have received {(count == 1 ? "an Elite Lootbox Key" : $"{count} Elite Lootbox Keys")}!"
            //            };
            //            player.Client.SendPacket(packet);
            //        }
            //    }
            //}

            if (e.ObjectDesc.EventLootboxDrop)
            {
                foreach (var playerInfo in playerDamage)
                {
                    val = rand.NextDouble();
                    double baseEventBoxChance = 0.7;
                    double changePerBoxReceived = 0.3;
                    Player player = playerInfo.Item1;
                    int count = 0;
                    while (val <= baseEventBoxChance)
                    {
                        count++;
                        baseEventBoxChance *= changePerBoxReceived;
                        val = rand.NextDouble();
                    }
                    if (count > 0)
                    {
                        player.Client.Manager.Database.UpdateEventLootbox(player.Client.Account, count);
                        player.EventLootbox += count;
                        player.ForceUpdate(player.EventLootbox);
                        var packet = new Text
                        {
                            BubbleTime = 0,
                            NumStars = -1,
                            TextColor = 0x72ff66,
                            Name = "#Event Lootbox Notifier",
                            Txt = $"You have received {(count == 1 ? "an Event Lootbox Key" : $"{count} Event Lootbox Keys")}!"
                        };
                        player.Client.SendPacket(packet);
                    }
                }
            }
        }
    }
}
