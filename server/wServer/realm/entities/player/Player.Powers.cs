using common.resources;
using System;

namespace wServer.realm.entities
{
    partial class Player
    {
        public bool CheckInsurgency()
        {
            return Inventory[3] != null && Inventory[3].ObjectId == "Herculean Amulet";
        }

        public bool CheckDRage()
        {
            return Inventory[2] != null && Inventory[2].ObjectId == "Drannol's Fury";
        }

        public bool CheckFRage()
        {
            return Inventory[1] != null && Inventory[1].ObjectId == "Fortification Shield";
        }

        public bool CheckSunMoon()
        {
            return Inventory[1] != null && Inventory[1].ObjectId == "Grand Curse" && Surge >= 50;
        }

        public bool CheckCrescent()
        {
            return Inventory[0] != null && Inventory[0].ObjectId == "Lance of Honour";
        }

        public bool CheckGHelm()
        {
            return Inventory[1] != null && Inventory[1].ObjectId == "The Kingdoms Pride";
        }

        public bool CheckAnguish()
        {
            return Inventory[1] != null && Inventory[1].ObjectId == "Skull of the Titan";
        }

        public bool CheckAnubis()
        {
            return Inventory[2] != null && Inventory[2].ObjectId == "Khonsu's Breastplate" && Surge >= 20;
        }

        public bool CheckForce()
        {
            return Inventory[2] != null && Inventory[2].ObjectId == "Steel-Woven Hide" && HP <= Stats[0] / 2;
        }

        public bool CheckMerc()
        {
            return Inventory[2] != null && Inventory[2].ObjectId == "Grace of the Colossus" && MP >= Stats[1] / 2;
        }

        public bool CheckRoyal()
        {
            return Inventory[0] != null && Inventory[0].ObjectId == "The Monarch's Grasp" && this.AnyEnemyNearby(10) == false;
        }

        public bool CheckDemo()
        {
            return Inventory[3] != null && Inventory[3].ObjectId == "Band of Exoneration" && HP >= (int)(Stats[0] * 0.8);
        }

        public bool CheckKar()
        {
            return Inventory[0] != null && Inventory[0].ObjectId == "Evanescence";
        }

        public bool CheckTinda()
        {
            return Inventory[0] != null && Inventory[0].ObjectId == "Endless Destruction" && MP >= Stats[1] / 2;
        }

        public bool CheckGilded()
        {
            return Inventory[0] != null && Inventory[0].ObjectId == "Royal Dagger" && this.CountEntity(6, objType: null) >= 2;
        }

        public bool CheckAxe()
        {
            return Inventory[0] != null && Inventory[0].ObjectId == "Scarlet Battle-Axe" && Surge >= 25;
        }

        public bool CheckMocking()
        {
            return Inventory[0] != null && Inventory[0].ObjectId == "The Mocking Raven";
        }

        public bool CheckResistance()
        {
            return Inventory[2] != null && Inventory[2].ObjectId == "Interstellar Force";
        }

        public bool CheckAegis()
        {
            return Inventory[2] != null && Inventory[2].ObjectId == "Aegis of the Devourer";
        }

        public bool CheckMoonlight()
        {
            return Inventory[0] != null && Inventory[0].ObjectId == "Ascension";
        }

        public bool CheckNil()
        {
            return Inventory[2] != null && Inventory[2].ObjectId.Equals("Armor of Nil");
        }

        public bool CheckSource()
        {
            return Inventory[3] != null && Inventory[3].ObjectId.Equals("Sourcestone");
        }

        public bool CheckLode()
        {
            return Inventory[3] != null && Inventory[3].ObjectId.Equals("Magical Lodestone");
        }

        public bool CheckBloodshed()
        {
            return Inventory[3] != null && Inventory[3].ObjectId.Equals("Bloodshed Ring");
        }

        public bool CheckOmni()
        {
            return Inventory[3] != null && Inventory[3].ObjectId.Equals("Omnipotence Ring");
        }

        public bool CheckSeal()
        {
            return Inventory[1] != null && Inventory[1].ObjectId.Equals("Marble Seal");
        }

        public bool CheckDJudgement()
        {
            return Inventory[2] != null && Inventory[2].ObjectId == "Drannol's Judgement";
        }

        public bool CheckIoksRelief()
        {
            return Inventory[1] != null && Inventory[1].ObjectId == "Banner of Immense Protection" && Surge >= 5;
        }

        public bool CheckBleedingFang()
        {
            return Inventory[0] != null && Inventory[0].ObjectId == "Blood-Fang" && Surge >= 2;
        }

        public bool CheckBifierce()
        {
            return Inventory[0] != null && Inventory[0].ObjectId == "Eternal Deficit" && HP <= Stats[0] / 2;
        }

        public bool CheckIoksCourage()
        {
            return Inventory[0] != null && Inventory[0].ObjectId == "Lance of Immense Protection" && Protection <= 0;
        }

        public bool CheckStarmind()
        {
            return Inventory[3] != null && Inventory[3].ObjectId == "Immunity Gauntlet" && Surge >= 60 && Inventory[1].MpCost > 0;
        }

        public bool CheckDranbielGarbs()
        {
            return Inventory[2] != null && Inventory[2].ObjectId == "Cloth of the Battle-Mage" && MP >= Stats[1] / 2 && Inventory[1].MpCost > 0;
        }

        public bool CheckStarCrashRing()
        {
            return Inventory[3] != null && Inventory[3].ObjectId == "Asteroid" && Inventory[1].MpCost > 0;
        }

        public bool CheckTheInfernus()
        {
            return Inventory[2] != null && Inventory[2].ObjectId == "Flash-Fire" && Inventory[1].MpCost > 0;
        }

        public bool CheckMeteor()
        {
            return Inventory[0] != null && Inventory[0].ObjectId == "Sky-Striker" && Inventory[1]?.ObjectId == "Book of Burning Souls" || Inventory[1]?.ObjectId == "Volcanic Vengeance" && Inventory[1].MpCost > 0 && new Random().NextDouble() < 0.14f;
        }

        public bool CheckDimensionalPrism()
        {
            return Inventory[1] != null && Inventory[1].ObjectId == "Prism of Mirrors" && Surge > 10; //&& Surge > 10
        }

        public bool CheckUrumi()
        {
            return Inventory[0] != null && Inventory[0].ObjectId == "Blades of Majesty" && Surge > 10;
        }

        //public bool CheckSacredEffect(string eff)
        //{
        //    for (var i = 0; i < 4; i++)
        //        if (Inventory[i] != null && Inventory[i].Sacred)
        //            if (Inventory[i].SacredEffect == eff)
        //                return true;
        //    return false;
        //}

        //public bool RollSacredEffect(string eff, int c)
        //{
        //    int count = 0;
        //    for (var i = 0; i < 4; i++)
        //        if (Inventory[i] != null && Inventory[i].Sacred)
        //            if (Inventory[i].SacredEffect == eff)
        //                count++;
        //    int chance = count * c;
        //    if (Random.Next(100) < chance)
        //        return true;
        //    return false;
        //}

        //public double SacredBoost(string eff, double c)
        //{
        //    int count = 0;
        //    for (var i = 0; i < 4; i++)
        //        if (Inventory[i] != null && Inventory[i].Sacred)
        //            if (Inventory[i].SacredEffect == eff)
        //                count++;
        //    return c * count;
        //}

        public int[] PosEffects = { 12, 14, 17, 18, 19, 25, 49, 50, 52, 58, 59, 60, 61, 62 };

        //public bool ResolveSlot0()
        //{
        //    return Inventory[0] != null && Inventory[0].Legendary;
        //}

        //public bool ResolveSlot1()
        //{
        //    return Inventory[1] != null && Inventory[1].Legendary;
        //}

        //public bool ResolveSlot2()
        //{
        //    return Inventory[2] != null && Inventory[2].Legendary;
        //}

        //public bool ResolveSlot3()
        //{
        //    return Inventory[3] != null && Inventory[3].Legendary;
        //}
    }
}