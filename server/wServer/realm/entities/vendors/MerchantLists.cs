using common;
using common.resources;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using wServer.realm.terrain;

namespace wServer.realm.entities.vendors
{
    public class ShopItem : ISellableItem
    {
        public ushort ItemId { get; private set; }
        public int Price { get; }
        public int Count { get; }
        public string Name { get; }

        public ShopItem(string name, int price, int count = -1)
        {
            ItemId = ushort.MaxValue;
            Price = price;
            Count = count;
            Name = name;
        }

        public void SetItem(ushort item)
        {
            if (ItemId != ushort.MaxValue)
                throw new AccessViolationException("Can't change item after it has been set.");

            ItemId = item;
        }
    }

    internal static class MerchantLists
    {
        private static readonly List<ISellableItem> Weapon = new List<ISellableItem> {
            new ShopItem("Backpack", 25),
            new ShopItem("Vault Chest Unlocker", 50),
            new ShopItem("Potion Storage Unlocker", 500)
        };

        private static readonly List<ISellableItem> Ability = new List<ISellableItem> {
            new ShopItem("Purification Crystal", 10),
            new ShopItem("Holy Water", 1)
        };

        private static readonly List<ISellableItem> Armor = new List<ISellableItem> {
            new ShopItem("Spirit of the Past", 25),
            new ShopItem("The Zol Awakening (Token)", 50),
            new ShopItem("Calling of the Titan (Token)", 50),
            new ShopItem("Malgoric Token", 100),
            new ShopItem("Lost Halls Key", 50)
        };

        private static readonly List<ISellableItem> Rings = new List<ISellableItem> {
             new ShopItem("Potion of Life", 500),
             new ShopItem("Potion of Mana", 500),
             new ShopItem("Potion of Attack", 100),
             new ShopItem("Potion of Defense", 100),
             new ShopItem("Potion of Speed", 100),
             new ShopItem("Potion of Dexterity", 100),
             new ShopItem("Potion of Vitality", 100),
             new ShopItem("Potion of Wisdom", 100),
             new ShopItem("Potion of Luck", 500),
             new ShopItem("Potion of Restoration", 650)
        };

        private static readonly List<ISellableItem> Aldragine = new List<ISellableItem> {
            new ShopItem("Scepter of the Other", 150),
            new ShopItem("Burden of the Warpawn", 160),
            new ShopItem("The Odyssey", 120),
            new ShopItem("The Executioner", 120),
            new ShopItem("Rip of Soul", 130),
            new ShopItem("Sincryer's Demise", 140),
            new ShopItem("Aegis of the Devourer", 130),
            new ShopItem("Drannol's Fury", 120),
            new ShopItem("Grasp of Elysium", 140),
            new ShopItem("Fortification Shield", 160),
            new ShopItem("Never Before Seen", 120),
            new ShopItem("The Master's Betrayal", 110)
        };

        private static readonly List<ISellableItem> Special = new List<ISellableItem> {
        };

        private static readonly List<ISellableItem> LegendaryConsumables = new List<ISellableItem> {
        };


        public static readonly Dictionary<TileRegion, Tuple<List<ISellableItem>, CurrencyType, int>> Shops =
            new Dictionary<TileRegion, Tuple<List<ISellableItem>, CurrencyType, int>>
        {
            { TileRegion.Store_1, new Tuple<List<ISellableItem>, CurrencyType, int>(Weapon, CurrencyType.Onrane, 0) },
            { TileRegion.Store_2, new Tuple<List<ISellableItem>, CurrencyType, int>(Ability, CurrencyType.Onrane, 0) },
            { TileRegion.Store_3, new Tuple<List<ISellableItem>, CurrencyType, int>(Armor, CurrencyType.Onrane, 0) },
            { TileRegion.Store_4, new Tuple<List<ISellableItem>, CurrencyType, int>(Rings, CurrencyType.Gold, 0) },
          //{ TileRegion.Store_5, new Tuple<List<ISellableItem>, CurrencyType, int>(Coins, CurrencyType.Gold, 0) }, store may be used later.
            { TileRegion.Store_7, new Tuple<List<ISellableItem>, CurrencyType, int>(Special, CurrencyType.Onrane, 0) },
            { TileRegion.Store_8, new Tuple<List<ISellableItem>, CurrencyType, int>(LegendaryConsumables, CurrencyType.Fame, 0) },
            { TileRegion.Store_15, new Tuple<List<ISellableItem>, CurrencyType, int>(Aldragine, CurrencyType.Onrane, 20) },
        };

        public static void Init(RealmManager manager)
        {
            foreach (var shop in Shops)
                foreach (var shopItem in shop.Value.Item1.OfType<ShopItem>())
                {
                    if (manager.Resources.GameData.IdToObjectType.TryGetValue(shopItem.Name, out var id))
                        shopItem.SetItem(id);
                }
        }
    }
}