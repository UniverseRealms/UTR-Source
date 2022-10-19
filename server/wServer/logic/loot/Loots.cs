using common.resources;
using System;
using System.Collections.Generic;
using System.Linq;
using wServer.networking.packets.outgoing;
using wServer.realm;
using wServer.realm.entities;
using wServer.realm.worlds.logic;
using DiscordWebhook;
using common;

namespace wServer.logic.loot
{
    public struct LootDef
    {
        public LootDef(Item item, double probabilty)
        {
            Item = item;
            Probability = probabilty;
        }

        public readonly Item Item;
        public readonly double Probability;
    }

    public class Loot : List<ILootDef>
    {
        public static Dictionary<int, int> ValuableBags = new Dictionary<int, int>
        {
            {9, 0xb353cc},
            {8, 0x3ea048},
            {11, 0xba1d1d},
            {12, 0xe8cb5a},
            {13, 0xd2b48c},
            {14, 0xfff059}
        };

        public Loot()
        {
        }

        public Loot(params ILootDef[] lootDefs)
        {  //For independent loots(e.g. chests)
            AddRange(lootDefs);
        }

        private static readonly Random Rand = new Random();

        private static readonly string[] ValuableItems = {
            "Nightmare Shard",
            "Luxury Shard",
            "Crimson Shard",
            "Extinction Shard",

            "The Zol Awakening (Token)",
            "Calling of the Titan (Token)",
        };

        public IEnumerable<Item> GetLoots(RealmManager manager, int min, int max)
        {  //For independent loots(e.g. chests)
            var consideration = new List<LootDef>();
            foreach (var i in this)
                i.Populate(manager, null, null, Rand, consideration);

            var retCount = Rand.Next(min, max);
            foreach (var i in consideration)
            {
                if (Rand.NextDouble() < i.Probability)
                {
                    yield return i.Item;
                    retCount--;
                }
                if (retCount == 0)
                    yield break;
            }
        }

        public void Handle(Enemy enemy)
        {
            var consideration = new List<LootDef>();

            //var sharedLoots = new List<Item>(); //no more kendo sticks or fire swords, ha
            /*foreach (var i in consideration)
            {
                if (Rand.NextDouble() < i.Probability)
                    sharedLoots.Add(i.Item);
            }*/

            var dats = enemy.DamageCounter.GetPlayerData();
            var loots = enemy.DamageCounter.GetPlayerData().ToDictionary(
                d => d.Item1, d => (IList<Item>)new List<Item>());

            //foreach (var loot in sharedLoots.Where(item => item.Soulbound))
            //     loots[dats[Rand.Next(dats.Length)].Item1].Add(loot);

            foreach (var dat in dats)
            {
                consideration.Clear();
                foreach (var i in this)
                    i.Populate(enemy.Manager, enemy, dat, Rand, consideration);

                //double lootDropBoost = dat.Item1.LDBoostTime > 0 ? 0.5 : 0;
                double luckStatBoost = dat.Item1.Stats[8] / 100.0;

                double damageBoost = Math.Min(0.5, Math.Max(0, dat.Item2 / (double)enemy.DamageCounter.TotalDamage / 2));

                double qualifyBoost = Math.Min(1, Math.Max(0, dat.Item2 / (double)enemy.DamageCounter.TotalDamage * Math.Max(1, dats.Length * 4)));

                double carryBoost = Math.Min(0.05 * dats.Length, Math.Max(0, dat.Item2 / (double)enemy.DamageCounter.TotalDamage * dats.Length * 0.05));

                //int TH;
                //if (dat.Item1.RollSacredEffect(SacredEffects.TreasureHunter, 8))
                //{
                //    TH = 2;
                //    dat.Item1.Owner.BroadcastPacketPrivate(new Notification
                //    {
                //        Color = new ARGB(0xDEC612),
                //        ObjectId = dat.Item1.Id,
                //        Message = "Treasure Hunter"
                //    }, dat.Item1);

                //} else
                //{
                //    TH = 1;
                //}

                //double dropBoost = qualifyBoost * (1 + carryBoost + damageBoost) * (1 + luckStatBoost);
                double dropBoost = 5f;
                var playerLoot = loots[dat.Item1];
                foreach (var i in consideration)
                {
                    double p = Rand.NextDouble();
                    var dropchance = i.Probability;

                    if (dropchance < 0.004)
                        dropchance *= 2.5;

                    if (p < dropchance * dropBoost)
                    {
                        playerLoot.Add(i.Item);
                    }

                }
            }

            AddBagsToWorld(enemy, /*sharedLoots*/ new List<Item>(), loots);
        }

        private static void AddBagsToWorld(Enemy enemy, IList<Item> shared, IDictionary<Player, IList<Item>> soulbound)
        {
            var pub = new List<Player>();  //only people not getting soulbound
            foreach (var i in soulbound)
            {
                if (i.Value.Count > 0)
                    ShowBags(enemy, i.Value, i.Key);
                //else
                //    pub.Add(i.Key);
            }
            if (pub.Count > 0 && shared.Count > 0)
                ShowBags(enemy, shared, pub.ToArray());
        }
        public static void Chatlog(Webhook hook, string username, string text)
        {
            if (!common.ServerConfig.EnableDebug)
            {
                hook.PostData(new WebhookObject()
                {
                    username = username,
                    content = text,
                });
            }
        }
        private static void ShowBags(Enemy enemy, IEnumerable<Item> loots, params Player[] owners)
        {
            var ownerIds = owners.Select(x => x.AccountId).ToArray();
            var bagType = 0;
            var items = new Item[8];
            var idx = 0;

            foreach (var i in loots)
            {
                if (i.BagType > bagType) bagType = i.BagType;
                items[idx] = i;
                idx++;

                if (ValuableBags.TryGetValue(i.BagType, out int color) || ValuableItems.Contains(i.ObjectId))
                {
                    string PlayerName = owners[0].Name;
                    string ItemName = i.ObjectId;

                    if(i.Heroic)
                        color = 0x10d000;

                    if (i.Ancestral)
                        color = 0x5080f0;

                    if (i.Sacred)
                        color = 0xff9000;

                    var percent = Math.Round(100.0 * (enemy.DamageCounter.DamageFrom(owners[0]) / (double)enemy.DamageCounter.TotalDamage), 2);

                    Text Valuable = new Text()
                    {
                        Name = "Loot Notifier",
                        NumStars = -1,
                        Txt = $"{PlayerName} has obtained a{(ItemName.ToLower().IndexOfAny("aeiou".ToCharArray()) == 0 ? "n" : "")} {ItemName}"
                              + $", with {percent}% damage dealt!",
                        BubbleTime = 0,
                        NameColor = 0xffbf00,
                        TextColor = color == 0 ? 0xdb88ef : color
                    };

                    Program.DL.LogToDiscord(WebHooks.Loot, "Loot Bot", $"{PlayerName} has obtained a {(ItemName.ToLower().IndexOfAny("aeiou".ToCharArray()) == 0 ? "n" : "")} {ItemName}"
                              + $", with {percent}% damage dealt!");

                    if (i.BagType == 9 || enemy.Owner is Test)
                        enemy.CurrWorld(Valuable);
                    else if (i.BagType > 9)
                    {
                        enemy.Manager.GlobalBroadcast(Valuable);
                    }
                }

                if (idx == 8)
                {
                    ShowBag(enemy, ownerIds, bagType, items);

                    bagType = 0;
                    items = new Item[8];
                    idx = 0;
                }
            }

            if (idx > 0)
                ShowBag(enemy, ownerIds, bagType, items);
        }

        private static void ShowBag(Enemy enemy, int[] owners, int bagType, Item[] items)
        {
            ushort bag = 0x0503; // 0x0500; for brown bag default
            switch (bagType)
            {
                case 0:
                    //bag = 0x500;
                    break;

                case 1:
                    //bag = 0x506;
                    break;

                case 2:
                    bag = 0x503;
                    break;

                case 3:
                    bag = 0x508;
                    break;

                case 4:
                    bag = 0x509;
                    break;

                case 5:
                    bag = 0x050B;
                    break;

                case 6:
                    bag = 0x169a;
                    break;

                case 7:
                    bag = 0xfff;
                    break;

                case 8:
                    bag = 0x1861;
                    break;

                case 9:
                    bag = 0x050C;
                    break;

                case 10:
                    bag = 0x506f;
                    break;

                case 11:
                    bag = 0x44d4;
                    break;

                case 12:
                    bag = 0x44d5;
                    break;

                case 13:
                    bag = 0x7246;
                    break;

                case 14:
                    bag = 0x7245;
                    break;
            }
            var container = new Container(enemy.Manager, bag, 1000 * 60, true);
            for (var j = 0; j < 8; j++)
                container.Inventory[j] = items[j];
            container.BagOwners = owners;
            container.Move(
                enemy.X + (float)((Rand.NextDouble() * 2 - 1) * 0.5),
                enemy.Y + (float)((Rand.NextDouble() * 2 - 1) * 0.5));
            container.SetDefaultSize(bagType == 11 ? 120 : (bagType > 3 ? 110 : 80));
            enemy.Owner.EnterWorld(container);
        }
    }
}
