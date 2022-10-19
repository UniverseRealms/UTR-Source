using System;
using System.Collections.Generic;

namespace wServer.realm
{
    class ForgeList
    {
        public static readonly string FailedForge = "NULL";

        public enum ForgeData
        {
            Other,
            Eternal,
            Rusted,
            Nightmare,
        }

        public static readonly Dictionary<string[], Tuple<string, string>> OtherList = new Dictionary<string[], Tuple<string, string>>
        {
        { new [] { "Empowered Whip" }, new Tuple<string, string>("Whip", "Abyssal Rune") } //kekw
        };

        //public static readonly Dictionary<string[], Tuple<string, string>> GodlyList = new Dictionary<string[], Tuple<string, string>>
        //{
        //    { new [] { "Poseidon's Gauntlet" }, new Tuple<string, string>("Ring of the Seven Seas", "Godly Remnant") },
        //    { new [] { "Shade of the Phoenix" }, new Tuple<string, string>("Blazed Bow", "Godly Remnant") },
        //    { new [] { "Blade of the Angel's Honor" }, new Tuple<string, string>("Dagger of Gilded Pride", "Godly Remnant") },
        //    { new [] { "Drannol's Redemption" }, new Tuple<string, string>("Drannol's Judgement", "Godly Remnant") },
        //    { new [] { "Revenant Claw" }, new Tuple<string, string>("Spiritclaw", "Godly Remnant") },
        //    { new [] { "Omnipotence Ring" }, new Tuple<string, string>("Omnipotent Ring", "Godly Remnant") }
        //};

        public static readonly Dictionary<string[], Tuple<string, string>> EternalList = new Dictionary<string[], Tuple<string, string>>
        {
            { new [] { "Alien Tech", "Molten Bow", "Hornet Sting" }, new Tuple<string, string>("Rusted Bow", "Eternal Essence") },
            { new [] { "The Dragon's Breath", "Lance of the King's Successor", "Bloodseeker's Redemption" }, new Tuple<string, string>("Rusted Lance", "Eternal Essence") },
            { new [] { "Axe of Demise", "Warlord's Sword", "Avaricious Blade" }, new Tuple<string, string>("Rusted Sword", "Eternal Essence") },
            { new [] { "Ancient Wood Cane", "Extraterrestrial Wand", "Wyvern's Touch" }, new Tuple<string, string>("Rusted Wand", "Eternal Essence") },
            { new [] { "Against the World", "Toxin Infused Blade", "Honeycomb Dagger" }, new Tuple<string, string>("Rusted Dagger", "Eternal Essence") },
            { new [] { "Cane of the Devil", "Energized Cane", "A Wizard's Precious Treasure" }, new Tuple<string, string>("Rusted Staff", "Eternal Essence") },
            { new [] { "A Harpy's Fury", "Gold and Silver Blades", "Hot Chilli Blades" }, new Tuple<string, string>("Rusted Blades", "Eternal Essence") },
            { new [] { "Sunrise Katana", "Raven's Claw", "Unsheathed Fury of Yukimura" }, new Tuple<string, string>("Rusted Katana", "Eternal Essence") },
            { new [] { "Overseer Robe", "Inferno Garments", "Skullsplitter Robe", "Luminescent Gown", "Robe of Impenetrable Stone" }, new Tuple<string, string>("Rusted Robe", "Eternal Essence") },
            { new [] { "Angelic Platemail", "Bloodshed Armor", "Beyond Nature's Power", "Armor of Sacrifice", "Armor of Blessings" }, new Tuple<string, string>("Rusted Platemail", "Eternal Essence") },
            { new [] { "Tree-Sap Armor", "Hide of the Defender", "Awakening of Bravery", "Magmatic Fury", "Dragon-Scale Hide" }, new Tuple<string, string>("Rusted Leather Hide", "Eternal Essence") },
            { new [] { "Spectral Globe", "Creation of Anubis" }, new Tuple<string, string>("Rusted Prism", "Eternal Essence") },
            { new [] { "Demonic Siphon", "Advanced Tech Siphon"}, new Tuple<string, string>("Rusted Siphon", "Eternal Essence") },
            { new [] { "Drenched Gowns of Attila", "Spectre's Shroud" }, new Tuple<string, string>("Rusted Cloak", "Eternal Essence") },
            { new [] { "Marvelous Concoction", "Vial of Corrosive Venom" }, new Tuple<string, string>("Rusted Poison", "Eternal Essence") },
            { new [] { "Tarnished Cloths", "Blood-Stained Jacket" }, new Tuple<string, string>("Rusted Jacket", "Eternal Essence") },
            { new [] { "Dice of the Scriptures", "Dice of Resistance" }, new Tuple<string, string>("Rusted Dice", "Eternal Essence") },
            { new [] { "Grasp of the Gods", "Poison-Tip Quiver" }, new Tuple<string, string>("Rusted Quiver", "Eternal Essence") },
            { new [] { "Dragon-Snap Trap", "Devil's Snare Trap" }, new Tuple<string, string>("Rusted Trap", "Eternal Essence") },
            { new [] { "Haunted Talisman", "Angelic Talisman" }, new Tuple<string, string>("Rusted Talisman", "Eternal Essence") },
            { new [] { "Earthquake Tome", "Eternal Glory" }, new Tuple<string, string>("Rusted Tome", "Eternal Essence") },
            { new [] { "Rejoiced Rod of Victory", "Weeping Magicians Calling" }, new Tuple<string, string>("Rusted Scepter", "Eternal Essence") },
            { new [] { "Shooting Star", "Weighted Star" }, new Tuple<string, string>("Rusted Shuriken", "Eternal Essence") },
            { new [] { "Sheath of Dark Deeds", "Inscribed Sheath" }, new Tuple<string, string>("Rusted Sheath", "Eternal Essence") },
            { new [] { "Hellfire Charm", "Purity Charm" }, new Tuple<string, string>("Rusted Charm", "Eternal Essence") },
            { new [] { "Ancient Runestone", "Platonic Lifeline" }, new Tuple<string, string>("Rusted Spell", "Eternal Essence") },
            { new [] { "Nightcrawler's Head", "Ancient Aztec Skull" }, new Tuple<string, string>("Rusted Skull", "Eternal Essence") },
            { new [] { "Cursed Anomaly Orb", "Galactic Arrival" }, new Tuple<string, string>("Rusted Orb", "Eternal Essence") },
            { new [] { "Helm of the Unholy Warrior", "Lucifer's Demise" }, new Tuple<string, string>("Rusted Helmet", "Eternal Essence") },
            { new [] { "Godfrey's Battalion", "Rusted Ancient Shield" }, new Tuple<string, string>("Rusted Shield", "Eternal Essence") },
            { new [] { "Plaguebearer Seal", "Universal Seal" }, new Tuple<string, string>("Rusted Seal", "Eternal Essence") },
            { new [] { "Banner of the Ages", "Banner of Champions" }, new Tuple<string, string>("Rusted Banner", "Eternal Essence") },
            { new [] { "Eden's Apple", "Holy Grail", "Glass Pendant", "Fortified Jewel", "Continuum" }, new Tuple<string, string>("Rusted Ring", "Eternal Essence") }

        };

        public static readonly Dictionary<string[], Tuple<string, string>> NightmareList = new Dictionary<string[], Tuple<string, string>>
        {
            //fire
            { new [] { "Sinful Battle-Axe" }, new Tuple<string, string>("Blade of the Guardian", "Fire Essence") },
            { new [] { "Staff of Decades" }, new Tuple<string, string>("Staff of the Blood Moon", "Fire Essence") },
            { new [] { "Bow of Endless Demise" }, new Tuple<string, string>("Cardial Recurve", "Fire Essence") },
            { new [] { "Imperial Quiver" }, new Tuple<string, string>("Bone Quiver", "Fire Essence") },
            { new [] { "An Archangel's Tablet" }, new Tuple<string, string>("Scripture of the Underworld", "Fire Essence") },
            { new [] { "Helmet of Sin" }, new Tuple<string, string>("The Grave's Calling", "Fire Essence") },
            { new [] { "Seal of Sacrifice" }, new Tuple<string, string>("Eruption Seal", "Fire Essence") },
            { new [] { "Blood-Bound" }, new Tuple<string, string>("Tortured Wisp", "Fire Essence") },
            { new [] { "Hestia's Undying Flame" }, new Tuple<string, string>("Crimson Lantern", "Fire Essence") },
            { new [] { "Demon's Prison" }, new Tuple<string, string>("Satanic Prism", "Fire Essence") },
            { new [] { "Tyrant's Coat" }, new Tuple<string, string>("Jacket of Rebirth", "Fire Essence") },
            { new [] { "Jekyll and Hide" }, new Tuple<string, string>("Mercy of Flames", "Fire Essence") },
            { new [] { "Robe of the Devil's Advocate" }, new Tuple<string, string>("Lin's Robes", "Fire Essence") },
            { new [] { "Flaming Bulwark" }, new Tuple<string, string>("Eternal Flame Plate", "Fire Essence") },
            { new [] { "Vampiric Fang" }, new Tuple<string, string>("Tyrant's Pendant", "Fire Essence") },
            { new [] { "Necklace of Completion" }, new Tuple<string, string>("Fragment of the Realm", "Fire Essence") },
            { new [] { "Necklace of Wrath" }, new Tuple<string, string>("Granite Amulet", "Fire Essence") },
            { new [] { "Hellish Remains" }, new Tuple<string, string>("A Demon's Skull", "Fire Essence") },
            { new [] { "Divine" }, new Tuple<string, string>("Sacred", "Fire Essence") },
            //water
            { new [] { "Sword of Virtue" }, new Tuple<string, string>("The Final Stand", "Water Essence") },
            { new [] { "Frostbite" }, new Tuple<string, string>("Mimicry Dagger", "Water Essence") },
            { new [] { "Staff of Heavenly Toxins" }, new Tuple<string, string>("Staff of Consumption", "Water Essence") },
            { new [] { "Deep Sea Wand" }, new Tuple<string, string>("Starlight Wand", "Water Essence") },
            { new [] { "Hydrophilic Katana" }, new Tuple<string, string>("Furious Slasher", "Water Essence") },
            { new [] { "Aqueous Repeater" }, new Tuple<string, string>("Crossbow of the Legion", "Water Essence") },
            { new [] { "Seal of Tsunamis" }, new Tuple<string, string>("Seal of Endless Regret", "Water Essence") },
            { new [] { "Necrotic Toxin" }, new Tuple<string, string>("Heaven's Tears", "Water Essence") },
            { new [] { "Unstable Concoction" }, new Tuple<string, string>("Void Matter", "Water Essence") },
            { new [] { "Ghastly Snare" }, new Tuple<string, string>("Death Essence Trap", "Water Essence") },
            { new [] { "Sapphire Conquest" }, new Tuple<string, string>("Banner of Direction", "Water Essence") },
            { new [] { "Hide of Invocation" }, new Tuple<string, string>("Hunter's Hide", "Water Essence") },
            { new [] { "The Storm's Guidance" }, new Tuple<string, string>("Dream Gown", "Water Essence") },
            { new [] { "Pristine Platemail" }, new Tuple<string, string>("Mad God's Battle-Gear", "Water Essence") },
            { new [] { "Noxious Gemstone" }, new Tuple<string, string>("Zol Medallion", "Water Essence") },
            { new [] { "Band of Purity" }, new Tuple<string, string>("Sunken Treasure", "Water Essence") },
            { new [] { "Necklace of Emptiness" }, new Tuple<string, string>("Belt of Insanity", "Water Essence") },
            { new [] { "Restrainer" }, new Tuple<string, string>("Prophecy Sheath", "Water Essence") },
            //earth
            { new [] { "Murky Wand" }, new Tuple<string, string>("Wand of Command", "Earth Essence") },
            { new [] { "Cloak of Transparency" }, new Tuple<string, string>("Misty Cloak", "Earth Essence") },
            { new [] { "Spell of Petrification" }, new Tuple<string, string>("Everlasting Inferno", "Earth Essence") },
            { new [] { "Impenetrable Tome" }, new Tuple<string, string>("Aquarius Tales", "Earth Essence") },
            { new [] { "Helmet of Royalty" }, new Tuple<string, string>("Helm of the Halls", "Earth Essence") },
            { new [] { "Dust Ripper" }, new Tuple<string, string>("Bloodlust Star", "Earth Essence") },
            { new [] { "Flag of Withdrawal" }, new Tuple<string, string>("Revil's Revenge", "Earth Essence") },
            { new [] { "Dice of Misfortune" }, new Tuple<string, string>("Natural Selection", "Earth Essence") },
            { new [] { "Genocidal Platemail" }, new Tuple<string, string>("Hell-Forged Silvermail", "Earth Essence") },
            { new [] { "Immaculate Gemstone" }, new Tuple<string, string>("Tri-Scale Ring", "Earth Essence") },
            { new [] { "Bracelet of Fortitude" }, new Tuple<string, string>("Belt of the Ancients", "Earth Essence") },
            { new [] { "Dusk-Woven Hide" }, new Tuple<string, string>("Overcoat of Darkness", "Earth Essence") },
            { new [] { "Robe of Nihilism" }, new Tuple<string, string>("Mechanical Coat", "Earth Essence") },
            { new [] { "Blade of Insomnia" }, new Tuple<string, string>("Rapier of Evisceration", "Earth Essence") },
            //air
            { new [] { "Emerald Dirk" }, new Tuple<string, string>("Shiv of the Void", "Air Essence") },
            { new [] { "Immortal Katana" }, new Tuple<string, string>("Flesh-Ripper", "Air Essence") },
            { new [] { "Climactic Resistance" }, new Tuple<string, string>("Crossbow of Gushing Flames", "Air Essence") },
            { new [] { "Starlight Quiver" }, new Tuple<string, string>("Zol Striker", "Air Essence") },
            { new [] { "Corrupted Relations" }, new Tuple<string, string>("Tome of Eternal Slumber", "Air Essence") },
            { new [] { "Shield of Grandeur" }, new Tuple<string, string>("Marble Battalion", "Air Essence") },
            { new [] { "Midas' Heart" }, new Tuple<string, string>("Glider's Prism", "Air Essence") },
            { new [] { "Nanotech Shuriken" }, new Tuple<string, string>("Star of Corruption", "Air Essence") },
            { new [] { "Spectral Vest" }, new Tuple<string, string>("Jacket of Blizzards", "Air Essence") },
            { new [] { "Emerald-Scale Hide" }, new Tuple<string, string>("Chaotic Vanguard", "Air Essence") },
            { new [] { "Blightful Hide" }, new Tuple<string, string>("Herbal Hide", "Air Essence") },
            { new [] { "Necrosis" }, new Tuple<string, string>("Isolation Robe", "Air Essence") },
            { new [] { "Eden's Purity" }, new Tuple<string, string>("Armor of the Zol", "Air Essence") },
            { new [] { "An Angel's Blessing" }, new Tuple<string, string>("Plate of the Colossus", "Air Essence") },
            { new [] { "Holy Remnant" }, new Tuple<string, string>("Medallion of the Moon", "Air Essence") },
            { new [] { "Ring of Devotion" }, new Tuple<string, string>("Oblivion Keepsake", "Air Essence") },
            { new [] { "Dice of Fortunes" }, new Tuple<string, string>("Maniacal Dice", "Air Essence") },
            { new [] { "Divine" }, new Tuple<string, string>("Sacred", "Air Essence") }
        };
    }
}