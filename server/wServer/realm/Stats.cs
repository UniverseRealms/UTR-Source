﻿using System;
using Player = wServer.realm.entities.Player;

namespace wServer.realm
{
    public enum StatsType : byte
    {
        MaximumHP = 0,
        HP = 1,
        Size = 2,
        MaximumMP = 3,
        MP = 4,
        ExperienceGoal = 5,
        Experience = 6,
        Level = 7,
        Inventory0 = 8,
        Inventory1 = 9,
        Inventory2 = 10,
        Inventory3 = 11,
        Inventory4 = 12,
        Inventory5 = 13,
        Inventory6 = 14,
        Inventory7 = 15,
        Inventory8 = 16,
        Inventory9 = 17,
        Inventory10 = 18,
        Inventory11 = 19,
        Attack = 20,
        Defense = 21,
        Speed = 22,
        Vitality = 26,
        Wisdom = 27,
        Dexterity = 28,
        Effects = 29,
        Stars = 30,
        Name = 31,
        Texture1 = 32,
        Texture2 = 33,
        MerchantMerchandiseType = 34,
        Credits = 35,
        SellablePrice = 36,
        PortalUsable = 37,
        AccountId = 38,
        CurrentFame = 39,
        SellablePriceCurrency = 40,
        ObjectConnection = 41,
        /*
         * Mask :F0F0F0F0
         * each byte -> type
         * 0:Dot
         * 1:ShortLine
         * 2:L
         * 3:Line
         * 4:T
         * 5:Cross
         * 0x21222112
        */
        MerchantRemainingCount = 42,
        MerchantRemainingMinute = 43,
        MerchantDiscount = 44,
        SellableRankRequirement = 45,
        HPBoost = 46,
        MPBoost = 47,
        AttackBonus = 48,
        DefenseBonus = 49,
        SpeedBonus = 50,
        VitalityBonus = 51,
        WisdomBonus = 52,
        DexterityBonus = 53,
        OwnerAccountId = 54,
        NameChangerStar = 55,
        NameChosen = 56,
        Fame = 57,
        FameGoal = 58,
        Glow = 59,
        SinkOffset = 60,
        AltTextureIndex = 61,
        Guild = 62,
        GuildRank = 63,
        XPBoost = 65,
        XPBoostTime = 66,
        LDBoostTime = 67,
        LTBoostTime = 68,
        HealthStackCount = 69,
        MagicStackCount = 70,
        BackPack0 = 71,
        BackPack1 = 72,
        BackPack2 = 73,
        BackPack3 = 74,
        BackPack4 = 75,
        BackPack5 = 76,
        BackPack6 = 77,
        BackPack7 = 78,
        HasBackpack = 79,
        Skin = 80,
        PetId = 81,
        PetSkin = 82,
        PetType = 83,
        PetRarity = 84,
        PetMaxLevel = 85,
        PetUnk1 = 86,
        PetAbilityPower1 = 87,
        PetAbilityPower2 = 88,
        PetAbilityPower3 = 89,
        PetAbilityLevel1 = 90,
        PetAbilityLevel2 = 91,
        PetAbilityLevel3 = 92,
        PetAbilityType1 = 93,
        PetAbilityType2 = 94,
        PetAbilityType3 = 95,
        Effects2 = 96,
        Tokens = 97,
        DamageMin = 98,
        DamageMax = 99,
        DamageMinBonus = 100,
        DamageMaxBonus = 101,
        FortuneBonus = 105,
        Rank = 103,
        Admin = 104,
        Onrane = 106,
        Kantos = 107,
        RaidToken = 108,
        Surge = 110,
        //SurgeCounter = 111,
        Luck = 114,
        LuckBonus = 115,
        BronzeLootbox = 116,
        SilverLootbox = 117,
        GoldLootbox = 118,
        EliteLootbox = 119,
        PremiumLootbox = 120,
        Restoration = 121,
        RestorationBonus = 123,
        ProtectionPoints = 125,
        ProtectionPointsMax = 126,
        Effect = 127,
        MarksEnabled = 128,
        Mark = 129,
        Node1 = 130,
        Node2 = 131,
        Node3 = 132,
        Node4 = 133,
        AscensionEnabled = 146,
        RageBar = 147,
        SorStorage = 148,
        ChanceDenom = 149,
        AirStorage = 161,
        WaterStorage = 162,
        EarthStorage = 163,
        FireStorage = 164,

        APotions = 171,
        DPotions = 172,
        SPotions = 173,
        DEPotions = 174,
        VPotions = 175,
        WPotions = 176,
        LPotions = 177,
        MPotions = 178,
        RPotions = 179,
        LUPotions = 180,

        //(unused) Elite = 150,
        //(unused) PvP = 151,
        LockerLootbox = 152,
        EventLootbox = 153,
        PartyId = 154,
        PartyRank = 155,
        SetSkillPoints = 181,
        SetUsedSkillPoints = 182,
        BoughtSkills = 183,

        None = 255
    }

    public class SV<T>
    {
        private readonly Entity _owner;
        private readonly StatsType _type;
        private readonly bool _updateSelfOnly;
        private readonly Func<T, T> _transform;
        private T _value;
        private T _tValue;

        public SV(Entity e, StatsType type, T value, bool updateSelfOnly = false, Func<T, T> transform = null)
        {
            _owner = e;
            _type = type;
            _updateSelfOnly = updateSelfOnly;
            _transform = transform;

            _value = value;
            _tValue = Transform(value);
        }

        public T GetValue() => _value;

        public void SetValue(T value)
        {
            if (_value != null && _value.Equals(value))
                return;

            _value = value;

            var tVal = Transform(value);

            if (_tValue != null && _tValue.Equals(tVal))
                return;

            _tValue = tVal;

            if (_owner is Player && _type == StatsType.Experience)
                _owner.InvokeStatChange(_type, (int)(object)tVal - Player.GetLevelExp((_owner as Player).Level), _updateSelfOnly);
            else
                _owner.InvokeStatChange(_type, tVal, _updateSelfOnly);
        }

        public override string ToString() => _value.ToString();

        private T Transform(T value) => (_transform == null) ? value : _transform(value);
    }

    public class StatChangedEventArgs : EventArgs
    {
        public StatChangedEventArgs(StatsType stat, object value, bool updateSelfOnly = false)
        {
            Stat = stat;
            Value = value;
            UpdateSelfOnly = updateSelfOnly;
        }

        public StatsType Stat { get; private set; }
        public object Value { get; private set; }
        public bool UpdateSelfOnly { get; private set; }
    }
}