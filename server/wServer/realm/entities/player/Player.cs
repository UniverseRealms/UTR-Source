using common;
using common.resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using wServer.logic;
using wServer.networking;
using wServer.networking.packets;
using wServer.networking.packets.outgoing;
using wServer.realm.cores;
using wServer.realm.terrain;
using wServer.realm.worlds;
using wServer.realm.worlds.logic;

namespace wServer.realm.entities
{
    
    public interface IPlayer
    {
        void Damage(int dmg, Entity src, bool noDef);

        bool IsVisibleToEnemy();

        bool IsInvulnerable();
    }

    public class Timer : System.Timers.Timer
    {
        public int Id { get; set; }

        public Timer(double interval, int id = -1) : base(interval)
        {
            Id = id;
        }
    }

    public partial class Player : Character, IContainer, IPlayer
    {
        private readonly Client _client;

        public Client Client => _client;

        public int[] _BoughtSkills = new int[100]; //100 Skill Slots
        //Stats
        private readonly SV<int> _accountId;

        public int AccountId
        {
            get => _accountId.GetValue();
            set => _accountId.SetValue(value);
        }

        private readonly SV<int> _experience;

        public int Experience
        {
            get => _experience.GetValue();
            set => _experience.SetValue(value);
        }

        private readonly SV<int> _experienceGoal;

        public int ExperienceGoal
        {
            get => _experienceGoal.GetValue();
            set => _experienceGoal.SetValue(value);
        }

        private readonly SV<int> _level;

        public int Level
        {
            get => _level.GetValue();
            set => _level.SetValue(value);
        }

        private readonly SV<int> _currentFame;

        public int CurrentFame
        {
            get => _currentFame.GetValue();
            set => _currentFame.SetValue(value);
        }

        private readonly SV<int> _fame;

        public int Fame
        {
            get => _fame.GetValue();
            set => _fame.SetValue(value);
        }

        private readonly SV<int> _fameGoal;

        public int FameGoal
        {
            get => _fameGoal.GetValue();
            set => _fameGoal.SetValue(value);
        }

        private readonly SV<int> _stars;

        public int Stars
        {
            get => _stars.GetValue();
            set => _stars.SetValue(value);
        }

        private readonly SV<string> _guild;

        public string Guild
        {
            get => _guild.GetValue();
            set => _guild.SetValue(value);
        }

        private readonly SV<int> _guildRank;

        public int GuildRank
        {
            get => _guildRank.GetValue();
            set => _guildRank.SetValue(value);
        }

        private readonly SV<int> _credits;

        public int Credits
        {
            get => _credits.GetValue();
            set => _credits.SetValue(value);
        }

        private readonly SV<int> _sorStorage;

        public int SorStorage
        {
            get => _sorStorage.GetValue();
            set => _sorStorage.SetValue(value);
        }

        private readonly SV<int> _airStorage;

        public int AirStorage
        {
            get => _airStorage.GetValue();
            set => _airStorage.SetValue(value);
        }

        private readonly SV<int> _waterStorage;

        public int WaterStorage
        {
            get => _waterStorage.GetValue();
            set => _waterStorage.SetValue(value);
        }

        private readonly SV<int> _earthStorage;

        public int EarthStorage
        {
            get => _earthStorage.GetValue();
            set => _earthStorage.SetValue(value);
        }

        private readonly SV<int> _fireStorage;

        public int FireStorage
        {
            get => _fireStorage.GetValue();
            set => _fireStorage.SetValue(value);
        }

        private readonly SV<int> _chanceDenom;

        public int ChanceDenom
        {
            get => _chanceDenom.GetValue();
            set => _chanceDenom.SetValue(value);
        }

        private readonly SV<bool> _nameChosen;

        public bool NameChosen
        {
            get => _nameChosen.GetValue();
            set => _nameChosen.SetValue(value);
        }

        private readonly SV<int> _texture1;

        public int Texture1
        {
            get => _texture1.GetValue();
            set => _texture1.SetValue(value);
        }

        private readonly SV<int> _texture2;

        public int Texture2
        {
            get => _texture2.GetValue();
            set => _texture2.SetValue(value);
        }

        private readonly SV<bool> _marksEnabled;

        public bool MarksEnabled
        {
            get => _marksEnabled.GetValue();
            set => _marksEnabled.SetValue(value);
        }

        private readonly SV<bool> _ascensionEnabled;

        public bool AscensionEnabled
        {
            get => _ascensionEnabled.GetValue();
            set => _ascensionEnabled.SetValue(value);
        }

        private readonly SV<int> _mark;

        public int Mark
        {
            get => _mark.GetValue();
            set => _mark.SetValue(value);
        }

        private readonly SV<int> _node1;

        public int Node1
        {
            get => _node1.GetValue();
            set => _node1.SetValue(value);
        }

        private readonly SV<int> _node2;

        public int Node2
        {
            get => _node2.GetValue();
            set => _node2.SetValue(value);
        }

        private readonly SV<int> _node3;

        public int Node3
        {
            get => _node3.GetValue();
            set => _node3.SetValue(value);
        }

        private readonly SV<int> _node4;

        public int Node4
        {
            get => _node4.GetValue();
            set => _node4.SetValue(value);
        }

        private readonly SV<string> _effect;

        public string Effect
        {
            get => _effect.GetValue();
            set => _effect.SetValue(value);
        }

        public string XmlEffect { get; set; }

        private int _originalSkin;
        private readonly SV<int> _skin;

        public int Skin
        {
            get => _skin.GetValue();
            set => _skin.SetValue(value);
        }

        private readonly SV<int> _glow;

        public int Glow
        {
            get => _glow.GetValue();
            set => _glow.SetValue(value);
        }

        private readonly SV<int> _mp;

        public int MP
        {
            get => _mp.GetValue();
            set => _mp.SetValue(value);
        }

        private readonly SV<int> _surge;

        public int Surge
        {
            get => _surge.GetValue();
            set => _surge.SetValue(value);
        }

        private readonly SV<int> _protection;

        public int Protection
        {
            get => _protection.GetValue();
            set => _protection.SetValue(value);
        }

        private readonly SV<int> _protectionMax;

        public int ProtectionMax
        {
            get => _protectionMax.GetValue();
            set => _protectionMax.SetValue(value);
        }

        private readonly SV<bool> _hasBackpack;

        public bool HasBackpack
        {
            get => _hasBackpack.GetValue();
            set => _hasBackpack.SetValue(value);
        }

        private readonly SV<bool> _xpBoosted;

        public bool XPBoosted
        {
            get => _xpBoosted.GetValue();
            set => _xpBoosted.SetValue(value);
        }

        private readonly SV<int> _rageBar;

        public int RageBar
        {
            get => _rageBar.GetValue();
            set => _rageBar.SetValue(value);
        }

        private readonly SV<int> _rank;

        public int Rank
        {
            get => _rank.GetValue();
            set => _rank.SetValue(value);
        }

        private readonly SV<int> _admin;

        public int Admin
        {
            get => _admin.GetValue();
            set => _admin.SetValue(value);
        }

        private readonly SV<int> _tokens;

        public int Tokens
        {
            get => _tokens.GetValue();
            set => _tokens.SetValue(value);
        }

        private readonly SV<int> _partyId;

        public int PartyId
        {
            get { return _partyId.GetValue(); }
            set { _partyId.SetValue(value); }
        }

        private readonly SV<int> _partyRank;

        public int PartyRank
        {
            get { return _partyRank.GetValue(); }
            set { _partyRank.SetValue(value); }
        }

        private readonly SV<int> _onrane;

        public int Onrane
        {
            get => _onrane.GetValue();
            set => _onrane.SetValue(value);
        }

        private readonly SV<int> _kantos;

        public int Kantos
        {
            get => _kantos.GetValue();
            set => _kantos.SetValue(value);
        }

        private readonly SV<int> _raidToken;

        public int AlertToken
        {
            get => _raidToken.GetValue();
            set => _raidToken.SetValue(value);
        }

        private readonly SV<int> _lootbox1;

        public int BronzeLootbox
        {
            get => _lootbox1.GetValue();
            set => _lootbox1.SetValue(value);
        }

        private readonly SV<int> _lootbox2;

        public int SilverLootbox
        {
            get => _lootbox2.GetValue();
            set => _lootbox2.SetValue(value);
        }

        private readonly SV<int> _lootbox3;

        public int GoldLootbox
        {
            get => _lootbox3.GetValue();
            set => _lootbox3.SetValue(value);
        }

        private readonly SV<int> _lootbox4;

        public int EliteLootbox
        {
            get => _lootbox4.GetValue();
            set => _lootbox4.SetValue(value);
        }

        private readonly SV<int> _lootbox5;

        public int PremiumLootbox
        {
            get => _lootbox5.GetValue();
            set => _lootbox5.SetValue(value);
        }

        private readonly SV<int> _lootbox6;

        public int LockerLootbox
        {
            get => _lootbox6.GetValue();
            set => _lootbox6.SetValue(value);
        }

        private readonly SV<int> _lootbox7;

        public int EventLootbox
        {
            get => _lootbox7.GetValue();
            set => _lootbox7.SetValue(value);
        }

        private readonly SV<int> _APotions;

        public int APotions
        {
            get => _APotions.GetValue();
            set => _APotions.SetValue(value);
        }

        private readonly SV<int> _DPotions;

        public int DPotions
        {
            get => _DPotions.GetValue();
            set => _DPotions.SetValue(value);
        }

        private readonly SV<int> _SPotions;

        public int SPotions
        {
            get => _SPotions.GetValue();
            set => _SPotions.SetValue(value);
        }

        private readonly SV<int> _DEPotions;

        public int DEPotions
        {
            get => _DEPotions.GetValue();
            set => _DEPotions.SetValue(value);
        }

        private readonly SV<int> _VPotions;

        public int VPotions
        {
            get => _VPotions.GetValue();
            set => _VPotions.SetValue(value);
        }

        private readonly SV<int> _WPotions;

        public int WPotions
        {
            get => _WPotions.GetValue();
            set => _WPotions.SetValue(value);
        }

        private readonly SV<int> _LPotions;

        public int LPotions
        {
            get => _LPotions.GetValue();
            set => _LPotions.SetValue(value);
        }

        private readonly SV<int> _MPotions;

        public int MPotions
        {
            get => _MPotions.GetValue();
            set => _MPotions.SetValue(value);
        }

        private readonly SV<int> _RPotions;

        public int RPotions
        {
            get => _RPotions.GetValue();
            set => _RPotions.SetValue(value);
        }

        private readonly SV<int> _LUPotions;

        public int LUPotions
        {
            get => _LUPotions.GetValue();
            set => _LUPotions.SetValue(value);
        }

        private readonly SV<int> _SetSkillPoints;
        public int SkillPoints
        {
            get => _SetSkillPoints.GetValue();
            set => _SetSkillPoints.SetValue(value);
        }

        private readonly SV<int> _SetUsedSkillPoints;
        public int UsedSkillPoints
        {
            get => _SetUsedSkillPoints.GetValue();
            set => _SetUsedSkillPoints.SetValue(value);
        }



        private bool CheckHalfHPArmored()
        {
            for (var i = 0; i < 4; i++)
            {
                var item = Inventory[i];
                if (item == null || !item.HalfHPArmored)
                    continue;
                return true;
            }
            return false;
        }

        public int XPBoostTime { get; set; }
        public int LDBoostTime { get; set; }
        public int LTBoostTime { get; set; }
        
        public ushort SetSkin { get; set; }
        public int SetSkinSize { get; set; }
        public Entity Pet { get; set; }
        public int? GuildInvite { get; set; }
        public bool Muted { get; set; }
        public int? PartyInvite { get; set; }
        public long LastAltAttack { get; set; }
        public int OpenedKey { get; set; }
        public RInventory DbLink { get; }
        public int[] SlotTypes { get; }
        public Inventory Inventory { get; }

        public ItemStacker HealthPots { get; private set; }
        public ItemStacker MagicPots { get; private set; }
        public ItemStacker[] Stacks { get; }
        public PlayerShopItem PurchaseTarget { get; set; }


        public readonly StatsManager Stats;
        public int cheatCount = 0;
        public int InstaDeathHits = 0;
        public bool WearingDivine = false;
        public bool WearingSacred = false;
        public bool WearingEternal = false;


        protected override void ImportStats(StatsType stats, object val)
        {
            var items = Manager.Resources.GameData.Items;
            base.ImportStats(stats, val);
            switch (stats)
            {
                case StatsType.AccountId:
                    AccountId = ((string)val).ToInt32();
                    break;

                case StatsType.Experience:
                    Experience = (int)val;
                    break;

                case StatsType.ExperienceGoal:
                    ExperienceGoal = (int)val;
                    break;

                case StatsType.Level:
                    Level = (int)val;
                    break;

                case StatsType.Fame:
                    Fame = (int)val;
                    break;

                case StatsType.CurrentFame:
                    CurrentFame = (int)val;
                    break;

                case StatsType.FameGoal:
                    FameGoal = (int)val;
                    break;

                case StatsType.Stars:
                    Stars = (int)val;
                    break;

                case StatsType.Guild:
                    Guild = (string)val;
                    break;

                case StatsType.GuildRank:
                    GuildRank = (int)val;
                    break;

                case StatsType.Credits:
                    Credits = (int)val;
                    break;

                case StatsType.PartyRank:
                    PartyRank = (int)val;
                    break;

                case StatsType.PartyId:
                    PartyId = (int)val;
                    break;

                case StatsType.NameChosen:
                    NameChosen = (int)val != 0;
                    break;

                case StatsType.Texture1:
                    Texture1 = (int)val;
                    break;

                case StatsType.Texture2:
                    Texture2 = (int)val;
                    break;

                case StatsType.Effect:
                    XmlEffect = (string)val;
                    break;

                case StatsType.MarksEnabled:
                    MarksEnabled = (int)val == 1;
                    break;

                case StatsType.AscensionEnabled:
                    AscensionEnabled = (int)val == 1;
                    break;

                case StatsType.Mark:
                    Mark = (int)val;
                    break;

                case StatsType.Node1:
                    Node1 = (int)val;
                    break;

                case StatsType.Node2:
                    Node2 = (int)val;
                    break;

                case StatsType.Node3:
                    Node3 = (int)val;
                    break;

                case StatsType.Node4:
                    Node4 = (int)val;
                    break;

                case StatsType.Skin:
                    Skin = (int)val;
                    break;

                case StatsType.Glow:
                    Glow = (int)val;
                    break;

                case StatsType.MP:
                    MP = (int)val;
                    break;

                case StatsType.Inventory0:
                    Inventory[0] = (int)val == -1 ? null : items[(ushort)(int)val];
                    break;

                case StatsType.Inventory1:
                    Inventory[1] = (int)val == -1 ? null : items[(ushort)(int)val];
                    break;

                case StatsType.Inventory2:
                    Inventory[2] = (int)val == -1 ? null : items[(ushort)(int)val];
                    break;

                case StatsType.Inventory3:
                    Inventory[3] = (int)val == -1 ? null : items[(ushort)(int)val];
                    break;

                case StatsType.Inventory4:
                    Inventory[4] = (int)val == -1 ? null : items[(ushort)(int)val];
                    break;

                case StatsType.Inventory5:
                    Inventory[5] = (int)val == -1 ? null : items[(ushort)(int)val];
                    break;

                case StatsType.Inventory6:
                    Inventory[6] = (int)val == -1 ? null : items[(ushort)(int)val];
                    break;

                case StatsType.Inventory7:
                    Inventory[7] = (int)val == -1 ? null : items[(ushort)(int)val];
                    break;

                case StatsType.Inventory8:
                    Inventory[8] = (int)val == -1 ? null : items[(ushort)(int)val];
                    break;

                case StatsType.Inventory9:
                    Inventory[9] = (int)val == -1 ? null : items[(ushort)(int)val];
                    break;

                case StatsType.Inventory10:
                    Inventory[10] = (int)val == -1 ? null : items[(ushort)(int)val];
                    break;

                case StatsType.Inventory11:
                    Inventory[11] = (int)val == -1 ? null : items[(ushort)(int)val];
                    break;

                case StatsType.BackPack0:
                    Inventory[12] = (int)val == -1 ? null : items[(ushort)(int)val];
                    break;

                case StatsType.BackPack1:
                    Inventory[13] = (int)val == -1 ? null : items[(ushort)(int)val];
                    break;

                case StatsType.BackPack2:
                    Inventory[14] = (int)val == -1 ? null : items[(ushort)(int)val];
                    break;

                case StatsType.BackPack3:
                    Inventory[15] = (int)val == -1 ? null : items[(ushort)(int)val];
                    break;

                case StatsType.BackPack4:
                    Inventory[16] = (int)val == -1 ? null : items[(ushort)(int)val];
                    break;

                case StatsType.BackPack5:
                    Inventory[17] = (int)val == -1 ? null : items[(ushort)(int)val];
                    break;

                case StatsType.BackPack6:
                    Inventory[18] = (int)val == -1 ? null : items[(ushort)(int)val];
                    break;

                case StatsType.BackPack7:
                    Inventory[19] = (int)val == -1 ? null : items[(ushort)(int)val];
                    break;

                case StatsType.MaximumHP:
                    Stats.Base[0] = (int)val;
                    break;

                case StatsType.MaximumMP:
                    Stats.Base[1] = (int)val;
                    break;

                case StatsType.Attack:
                    Stats.Base[2] = (int)val;
                    break;

                case StatsType.Defense:
                    Stats.Base[3] = (int)val;
                    break;

                case StatsType.Speed:
                    Stats.Base[4] = (int)val;
                    break;

                case StatsType.Dexterity:
                    Stats.Base[5] = (int)val;
                    break;

                case StatsType.Vitality:
                    Stats.Base[6] = (int)val;
                    break;

                case StatsType.Wisdom:
                    Stats.Base[7] = (int)val;
                    break;

                case StatsType.Luck:
                    Stats.Base[9] = (int)val;
                    break;

                case StatsType.Restoration:
                    Stats.Base[10] = (int)val;
                    break;

                case StatsType.DamageMin:
                    Stats.Base[12] = (int)val;
                    break;

                case StatsType.DamageMax:
                    Stats.Base[13] = (int)val;
                    break;

                case StatsType.HealthStackCount:
                    HealthPots.Count = (int)val;
                    break;

                case StatsType.MagicStackCount:
                    MagicPots.Count = (int)val;
                    break;

                case StatsType.HasBackpack:
                    HasBackpack = (int)val == 1;
                    break;

                case StatsType.XPBoost:
                    XPBoosted = (int)val == 1;
                    break;

                case StatsType.XPBoostTime:
                    XPBoostTime = (int)val * 1000;
                    break;

                case StatsType.LDBoostTime:
                    LDBoostTime = (int)val * 1000;
                    break;

                case StatsType.LTBoostTime:
                    LTBoostTime = (int)val * 1000;
                    break;

                case StatsType.Rank:
                    Rank = (int)val;
                    break;

                case StatsType.Admin:
                    Admin = (int)val;
                    break;

                case StatsType.Tokens:
                    Tokens = (int)val;
                    break;

                case StatsType.Onrane:
                    Onrane = (int)val;
                    break;

                case StatsType.Kantos:
                    Kantos = (int)val;
                    break;

                case StatsType.RaidToken:
                    AlertToken = (int)val;
                    break;

                case StatsType.BronzeLootbox:
                    BronzeLootbox = (int)val;
                    break;

                case StatsType.SilverLootbox:
                    SilverLootbox = (int)val;
                    break;

                case StatsType.GoldLootbox:
                    GoldLootbox = (int)val;
                    break;

                case StatsType.EliteLootbox:
                    EliteLootbox = (int)val;
                    break;

                case StatsType.PremiumLootbox:
                    PremiumLootbox = (int)val;
                    break;

                case StatsType.LockerLootbox:
                    LockerLootbox = (int)val;
                    break;

                case StatsType.EventLootbox:
                    EventLootbox = (int)val;
                    break;

                case StatsType.Surge:
                    Surge = (int)val;
                    break;

                case StatsType.ProtectionPoints:
                    Protection = (int)val;
                    break;

                case StatsType.ProtectionPointsMax:
                    ProtectionMax = (int)val;
                    break;

                case StatsType.SorStorage:
                    SorStorage = (int)val;
                    break;

                case StatsType.AirStorage:
                    AirStorage = (int)val;
                    break;

                case StatsType.WaterStorage:
                    WaterStorage = (int)val;
                    break;

                case StatsType.EarthStorage:
                    EarthStorage = (int)val;
                    break;

                case StatsType.FireStorage:
                    FireStorage = (int)val;
                    break;
                case StatsType.ChanceDenom:
                    ChanceDenom = (int)val;
                    break;
                case StatsType.APotions:
                    APotions = (int)val;
                    break;
                case StatsType.DPotions:
                    DPotions = (int)val;
                    break;
                case StatsType.SPotions:
                    SPotions = (int)val;
                    break;
                case StatsType.DEPotions:
                    DEPotions = (int)val;
                    break;
                case StatsType.VPotions:
                    VPotions = (int)val;
                    break;
                case StatsType.WPotions:
                    WPotions = (int)val;
                    break;
                case StatsType.LPotions:
                    LPotions = (int)val;
                    break;
                case StatsType.MPotions:
                    MPotions = (int)val;
                    break;
                case StatsType.SetSkillPoints:
                    SkillPoints = (int)val;
                    break;
                case StatsType.SetUsedSkillPoints:
                    UsedSkillPoints = (int)val;
                    break;
            }
        }

        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            base.ExportStats(stats);
            stats[StatsType.AccountId] = AccountId.ToString();
            stats[StatsType.Experience] = Experience - GetLevelExp(Level);
            stats[StatsType.ExperienceGoal] = ExperienceGoal;
            stats[StatsType.Level] = Level;
            stats[StatsType.CurrentFame] = CurrentFame;
            stats[StatsType.Fame] = Fame;
            stats[StatsType.FameGoal] = FameGoal;
            stats[StatsType.Stars] = Stars;
            stats[StatsType.Guild] = Guild;
            stats[StatsType.GuildRank] = GuildRank;
            stats[StatsType.PartyId] = PartyId;
            stats[StatsType.PartyRank] = PartyRank;
            stats[StatsType.Credits] = Credits;
            stats[StatsType.NameChosen] = // check from account in case ingame registration
                (_client.Account?.NameChosen ?? NameChosen) ? 1 : 0;
            stats[StatsType.Texture1] = Texture1;
            stats[StatsType.Texture2] = Texture2;
            stats[StatsType.Texture2] = Texture2;
            stats[StatsType.Effect] = XmlEffect == "" ? PlayerEffects.GetXML(Effect) : XmlEffect;
            stats[StatsType.MarksEnabled] = (MarksEnabled) ? 1 : 0;
            stats[StatsType.AscensionEnabled] = (AscensionEnabled) ? 1 : 0;
            stats[StatsType.Mark] = Mark;
            stats[StatsType.Node1] = Node1;
            stats[StatsType.Node2] = Node2;
            stats[StatsType.Node3] = Node3;
            stats[StatsType.Node4] = Node4;
            stats[StatsType.Skin] = Skin;
            stats[StatsType.Glow] = Glow;
            stats[StatsType.MP] = MP;
            stats[StatsType.Inventory0] = Inventory[0]?.ObjectType ?? -1;
            stats[StatsType.Inventory1] = Inventory[1]?.ObjectType ?? -1;
            stats[StatsType.Inventory2] = Inventory[2]?.ObjectType ?? -1;
            stats[StatsType.Inventory3] = Inventory[3]?.ObjectType ?? -1;
            stats[StatsType.Inventory4] = Inventory[4]?.ObjectType ?? -1;
            stats[StatsType.Inventory5] = Inventory[5]?.ObjectType ?? -1;
            stats[StatsType.Inventory6] = Inventory[6]?.ObjectType ?? -1;
            stats[StatsType.Inventory7] = Inventory[7]?.ObjectType ?? -1;
            stats[StatsType.Inventory8] = Inventory[8]?.ObjectType ?? -1;
            stats[StatsType.Inventory9] = Inventory[9]?.ObjectType ?? -1;
            stats[StatsType.Inventory10] = Inventory[10]?.ObjectType ?? -1;
            stats[StatsType.Inventory11] = Inventory[11]?.ObjectType ?? -1;
            stats[StatsType.BackPack0] = Inventory[12]?.ObjectType ?? -1;
            stats[StatsType.BackPack1] = Inventory[13]?.ObjectType ?? -1;
            stats[StatsType.BackPack2] = Inventory[14]?.ObjectType ?? -1;
            stats[StatsType.BackPack3] = Inventory[15]?.ObjectType ?? -1;
            stats[StatsType.BackPack4] = Inventory[16]?.ObjectType ?? -1;
            stats[StatsType.BackPack5] = Inventory[17]?.ObjectType ?? -1;
            stats[StatsType.BackPack6] = Inventory[18]?.ObjectType ?? -1;
            stats[StatsType.BackPack7] = Inventory[19]?.ObjectType ?? -1;
            stats[StatsType.MaximumHP] = Stats[0];
            stats[StatsType.MaximumMP] = Stats[1];
            stats[StatsType.Attack] = Stats[2];
            stats[StatsType.Defense] = Stats[3];
            stats[StatsType.Speed] = Stats[4];
            stats[StatsType.Dexterity] = Stats[5];
            stats[StatsType.Vitality] = Stats[6];
            stats[StatsType.Wisdom] = Stats[7];
            stats[StatsType.Luck] = Stats[8];
            stats[StatsType.Restoration] = Stats[9];
            stats[StatsType.DamageMin] = Stats[12];
            stats[StatsType.DamageMax] = Stats[13];
            stats[StatsType.HPBoost] = Stats.Boost[0];
            stats[StatsType.MPBoost] = Stats.Boost[1];
            stats[StatsType.AttackBonus] = Stats.Boost[2];
            stats[StatsType.DefenseBonus] = Stats.Boost[3];
            stats[StatsType.SpeedBonus] = Stats.Boost[4];
            stats[StatsType.DexterityBonus] = Stats.Boost[5];
            stats[StatsType.VitalityBonus] = Stats.Boost[6];
            stats[StatsType.WisdomBonus] = Stats.Boost[7];
            stats[StatsType.LuckBonus] = Stats.Boost[8];
            stats[StatsType.RestorationBonus] = Stats.Boost[9];
            stats[StatsType.DamageMinBonus] = Stats.Boost[12];
            stats[StatsType.DamageMaxBonus] = Stats.Boost[13];
            stats[StatsType.HealthStackCount] = HealthPots.Count;
            stats[StatsType.MagicStackCount] = MagicPots.Count;
            stats[StatsType.HasBackpack] = (HasBackpack) ? 1 : 0;
            stats[StatsType.XPBoost] = (XPBoosted) ? 1 : 0;
            stats[StatsType.Rank] = Rank;
            stats[StatsType.Admin] = Admin;
            stats[StatsType.Tokens] = Tokens;
            stats[StatsType.Onrane] = Onrane;
            stats[StatsType.Kantos] = Kantos;
            stats[StatsType.RaidToken] = AlertToken;
            stats[StatsType.Surge] = Surge;
            stats[StatsType.ProtectionPoints] = Protection;
            stats[StatsType.ProtectionPointsMax] = ProtectionMax;
            stats[StatsType.SorStorage] = SorStorage;
            stats[StatsType.AirStorage] = AirStorage;
            stats[StatsType.WaterStorage] = WaterStorage;
            stats[StatsType.EarthStorage] = EarthStorage;
            stats[StatsType.FireStorage] = FireStorage;
            stats[StatsType.APotions] = APotions;
            stats[StatsType.DPotions] = DPotions;
            stats[StatsType.SPotions] = SPotions;
            stats[StatsType.DEPotions] = DEPotions;
            stats[StatsType.VPotions] = VPotions;
            stats[StatsType.WPotions] = WPotions;
            stats[StatsType.LPotions] = LPotions;
            stats[StatsType.MPotions] = MPotions;
            stats[StatsType.ChanceDenom] = ChanceDenom;
            stats[StatsType.SetSkillPoints] = SkillPoints;
            stats[StatsType.SetUsedSkillPoints] = UsedSkillPoints;

            if (Owner == null) return;
            if (Owner.Name == "SummoningPoint") stats[StatsType.RageBar] = RageBar;
            if (Owner.Name == "Nexus")
            {
                stats[StatsType.BronzeLootbox] = BronzeLootbox;
                stats[StatsType.SilverLootbox] = SilverLootbox;
                stats[StatsType.GoldLootbox] = GoldLootbox;
                stats[StatsType.EliteLootbox] = EliteLootbox;
                stats[StatsType.PremiumLootbox] = PremiumLootbox;
                stats[StatsType.LockerLootbox] = LockerLootbox;
                stats[StatsType.EventLootbox] = EventLootbox;
            }
        }

        public void SaveToCharacter()
        {
            var chr = _client.Character;
            if (chr == null) return;

            chr.Level = Level;
            chr.Experience = Experience;
            chr.Fame = Fame;
            chr.HP = Math.Max(1, HP);
            chr.MP = MP;
            chr.Stats = Stats.Base.GetStats();
            chr.Tex1 = Texture1;
            chr.Tex2 = Texture2;
            chr.Skin = _originalSkin;
            chr.Effect = Effect;
            chr.MarksEnabled = MarksEnabled;
            chr.AscensionEnabled = AscensionEnabled;
            chr.Mark = Mark;
            chr.Node1 = Node1;
            chr.Node2 = Node2;
            chr.Node3 = Node3;
            chr.Node4 = Node4;
            chr.FameStats = FameCounter.Stats.Write();
            chr.LastSeen = DateTime.Now;
            chr.HealthStackCount = HealthPots.Count;
            chr.MagicStackCount = MagicPots.Count;
            chr.HasBackpack = HasBackpack;
            chr.XPBoosted = XPBoosted;
            chr.XPBoostTime = XPBoostTime;
            chr.LDBoostTime = LDBoostTime;
            chr.LTBoostTime = LTBoostTime;
            chr.SkillPoints = SkillPoints;
            chr.UsedSkillPoints = UsedSkillPoints;
            chr.BoughtSkills = _BoughtSkills;
            chr.Items = Inventory.GetItemTypes();
        }

        public Player(Client client, bool saveInventory = true) : base(client.Manager, client.Character.ObjectType)
        {
            

            var settings = Manager.Resources.Settings;
            var gameData = Manager.Resources.GameData;


            _client = client;
            _BoughtSkills = client.Character.BoughtSkills;
            // found in player.update partial
            Sight = new Sight(this);
            _clientEntities = new UpdatedSet(this);

            _accountId = new SV<int>(this, StatsType.AccountId, client.Account.AccountId, true);
            _experience = new SV<int>(this, StatsType.Experience, client.Character.Experience, true);
            _experienceGoal = new SV<int>(this, StatsType.ExperienceGoal, 0, true);
            _level = new SV<int>(this, StatsType.Level, client.Character.Level);
            _currentFame = new SV<int>(this, StatsType.CurrentFame, client.Account.Fame, true);
            _fame = new SV<int>(this, StatsType.Fame, client.Character.Fame, true);
            _fameGoal = new SV<int>(this, StatsType.FameGoal, 0, true);
            _stars = new SV<int>(this, StatsType.Stars, 0);
            _guild = new SV<string>(this, StatsType.Guild, "");
            _guildRank = new SV<int>(this, StatsType.GuildRank, -1);
            _partyId = new SV<int>(this, StatsType.PartyId, -1);
            _partyRank = new SV<int>(this, StatsType.PartyRank, -1);
            _credits = new SV<int>(this, StatsType.Credits, client.Account.Credits, true);
            _nameChosen = new SV<bool>(this, StatsType.NameChosen, client.Account.NameChosen, false, v => _client.Account?.NameChosen ?? v);
            _texture1 = new SV<int>(this, StatsType.Texture1, client.Character.Tex1);
            _texture2 = new SV<int>(this, StatsType.Texture2, client.Character.Tex2);
            _effect = new SV<string>(this, StatsType.Effect, client.Character.Effect);
            _skin = new SV<int>(this, StatsType.Skin, 0);
            _glow = new SV<int>(this, StatsType.Glow, 0);
            _mp = new SV<int>(this, StatsType.MP, client.Character.MP);
            _hasBackpack = new SV<bool>(this, StatsType.HasBackpack, client.Character.HasBackpack, true);
            _xpBoosted = new SV<bool>(this, StatsType.XPBoost, client.Character.XPBoosted, true);
            _marksEnabled = new SV<bool>(this, StatsType.MarksEnabled, client.Character.MarksEnabled, true);
            _ascensionEnabled = new SV<bool>(this, StatsType.AscensionEnabled, client.Character.AscensionEnabled, true);
            _mark = new SV<int>(this, StatsType.Mark, client.Character.Mark);
            _node1 = new SV<int>(this, StatsType.Node1, client.Character.Node1);
            _node2 = new SV<int>(this, StatsType.Node2, client.Character.Node2);
            _node3 = new SV<int>(this, StatsType.Node3, client.Character.Node3);
            _node4 = new SV<int>(this, StatsType.Node4, client.Character.Node4);
            _xpBoosted = new SV<bool>(this, StatsType.XPBoost, client.Character.XPBoostTime != 0, true);
            _rageBar = new SV<int>(this, StatsType.RageBar, -1, true);
            _rank = new SV<int>(this, StatsType.Rank, client.Account.Rank);
            _admin = new SV<int>(this, StatsType.Admin, client.Account.Admin ? 1 : 0);
            _tokens = new SV<int>(this, StatsType.Tokens, client.Account.Tokens, true);
            _onrane = new SV<int>(this, StatsType.Onrane, client.Account.Onrane, true);
            _kantos = new SV<int>(this, StatsType.Kantos, client.Account.Kantos, true);
            _raidToken = new SV<int>(this, StatsType.RaidToken, client.Account.RaidToken, true);
            _surge = new SV<int>(this, StatsType.Surge, -1);

            _protection = new SV<int>(this, StatsType.ProtectionPoints, -1);
            _protectionMax = new SV<int>(this, StatsType.ProtectionPointsMax, -1);

            _lootbox1 = new SV<int>(this, StatsType.BronzeLootbox, client.Account.BronzeLootbox, true);
            _lootbox2 = new SV<int>(this, StatsType.SilverLootbox, client.Account.SilverLootbox, true);
            _lootbox3 = new SV<int>(this, StatsType.GoldLootbox, client.Account.GoldLootbox, true);
            _lootbox4 = new SV<int>(this, StatsType.EliteLootbox, client.Account.EliteLootbox, true);
            _lootbox5 = new SV<int>(this, StatsType.PremiumLootbox, client.Account.PremiumLootbox, true);
            _lootbox6 = new SV<int>(this, StatsType.LockerLootbox, client.Account.LockerLootbox, true);
            _lootbox7 = new SV<int>(this, StatsType.EventLootbox, client.Account.EventLootbox, true);
            _sorStorage = new SV<int>(this, StatsType.SorStorage, client.Account.SorStorage, true);
            _airStorage = new SV<int>(this, StatsType.AirStorage, client.Account.AirStorage, true);
            _waterStorage = new SV<int>(this, StatsType.WaterStorage, client.Account.WaterStorage, true);
            _earthStorage = new SV<int>(this, StatsType.EarthStorage, client.Account.EarthStorage, true);
            _fireStorage = new SV<int>(this, StatsType.FireStorage, client.Account.FireStorage, true);
            _APotions = new SV<int>(this, StatsType.APotions, client.Account.StoredAttack, true);
            _DPotions = new SV<int>(this, StatsType.DPotions, client.Account.StoredDefense, true);
            _SPotions = new SV<int>(this, StatsType.SPotions, client.Account.StoredSpeed, true);
            _DEPotions = new SV<int>(this, StatsType.DEPotions, client.Account.StoredDexterity, true);
            _VPotions = new SV<int>(this, StatsType.VPotions, client.Account.StoredVitality, true);
            _WPotions = new SV<int>(this, StatsType.WPotions, client.Account.StoredWisdom, true);
            _LPotions = new SV<int>(this, StatsType.LPotions, client.Account.StoredLife, true);
            _MPotions = new SV<int>(this, StatsType.MPotions, client.Account.StoredMana, true);
            _RPotions = new SV<int>(this, StatsType.RPotions, client.Account.StoredRestoration, true);
            _LUPotions = new SV<int>(this, StatsType.LUPotions, client.Account.StoredLuck, true);
            _chanceDenom = new SV<int>(this, StatsType.ChanceDenom, client.Account.ChanceDenom, true);
            _SetSkillPoints = new SV<int>(this, StatsType.SetSkillPoints, client.Character.SkillPoints, true);
            _SetUsedSkillPoints = new SV<int>(this,StatsType.SetUsedSkillPoints, client.Character.UsedSkillPoints, true);


            

            Name = client.Account.Name;
            XmlEffect = "";
            HP = client.Character.HP;
            ConditionEffects = 0;

            XPBoostTime = client.Character.XPBoostTime;
            LDBoostTime = client.Character.LDBoostTime;
            LTBoostTime = client.Character.LTBoostTime;

            var s = (ushort)client.Character.Skin;
            //character aura

            if (gameData.Skins.Keys.Contains(s))
            {
                SetDefaultSkin(s);
                SetDefaultSize(gameData.Skins[s].Size);
            }

            var guild = Manager.Database.GetGuild(client.Account.GuildId);
            if (guild?.Name != null)
            {
                Guild = guild.Name;
                GuildRank = client.Account.GuildRank;
            }

            HealthPots = new ItemStacker(this, 254, 0x0A22, client.Character.HealthStackCount, settings.MaxStackablePotions);
            MagicPots = new ItemStacker(this, 255, 0x0A23, client.Character.MagicStackCount, settings.MaxStackablePotions);
            Stacks = new ItemStacker[] { HealthPots, MagicPots };

            // inventory setup
            DbLink = new DbCharInv(Client.Account, Client.Character.CharId);
            Inventory = new Inventory(this,
                Utils.ResizeArray(
                    (DbLink as DbCharInv).Items
                    .Select(_ => (_ == 0xffff || !gameData.Items.ContainsKey(_)) ? null : gameData.Items[_])
                    .ToArray(),
                    settings.InventorySize)
            );
            if (!saveInventory)
                DbLink = null;
            else
            {
                Inventory.InventoryChanged += (sender, e) =>
                {
                    DbLink.Items = Inventory.GetItemTypes();
                    DbLink.FlushAsync();
                };
            }
            Inventory.InventoryChanged += (sender, e) => Stats.ReCalculateValues(e);
            SlotTypes = Utils.ResizeArray(
                gameData.Classes[ObjectType].SlotTypes,
                settings.InventorySize);
            Stats = new StatsManager(this);

            // set size of player if using set skin
            var skin = (ushort)Skin;
            if (gameData.SkinTypeToEquipSetType.ContainsKey(skin))
            {
                var setType = gameData.SkinTypeToEquipSetType[skin];
                var ae = gameData.EquipmentSets[setType].ActivateOnEquipAll
                    .SingleOrDefault(e => e.SkinType == skin);

                if (ae != null)
                    Size = ae.Size;
            }

            // override size
            if (Client.Account.Size > 0)
                Size = Client.Account.Size;
            try
            {
                Manager.Database.IsMuted(client.IP)
                    .ContinueWith(t =>
                    {
                        Muted = !Client.Account.Admin && t.IsCompleted && t.Result;
                    });
            }
            catch(Exception e)
            {
                Console.WriteLine("Error with Muted" + e.Message);
            }
            try
            {
                Manager.Database.IsLegend(AccountId)
                    .ContinueWith(t =>
                    {
                        Glow = t.Result && client.Account.GlowColor == 0 ?
                            0xFF0000 :
                            client.Account.GlowColor;
                    });
            }
            catch(Exception e)
            {
                Console.WriteLine("Error with Glow color" + e.Message);
            }

            CheckItems(Client.Character.Items);

            RegularTick = false;
        }

        public void CheckItems(ushort[] items)
        {
            for(int i = 0; i < 4; i++)
            {
                if (items[i] == 0 || items[i] == 65535 || items[i] == -1)
                    return;

                if (Manager.Resources.GameData.Items[items[i]].Divine) this.WearingDivine = true;
                if (Manager.Resources.GameData.Items[items[i]].Sacred) this.WearingSacred = true;
                if (Manager.Resources.GameData.Items[items[i]].Eternal) this.WearingEternal = true;
            }
        }

        private byte[,] tiles;
        public FameCounter FameCounter { get; private set; }

        public Entity SpectateTarget { get; set; }
        public bool IsControlling => SpectateTarget != null && !(SpectateTarget is Player);

        public void ResetFocus(object target, EventArgs e)
        {
            var entity = target as Entity;
            entity.FocusLost -= ResetFocus;
            entity.Controller = null;

            if (Owner == null) return;

            SpectateTarget = null;
            Owner.Timers.Add(new WorldTimer(3000, (w) =>
            {
                if (w == null || w.Deleted || this == null) return;

                ApplyConditionEffect(ConditionEffectIndex.Paused, 0);
            }));
            Client.SendPacket(new SetFocus() { ObjectId = Id });
        }

        public override void Init(World owner)
        {
            var x = 0;
            var y = 0;
            var spawnRegions = owner.GetSpawnPoints();

            //MarksActivate();
            if (spawnRegions.Any())
            {
                var rand = new Random();
                var sRegion = spawnRegions.ElementAt(rand.Next(0, spawnRegions.Length));
                x = sRegion.Key.X;
                y = sRegion.Key.Y;
            }
            Move(x + 0.5f, y + 0.5f);
            tiles = new byte[owner.Map.Width, owner.Map.Height];

            FameCounter = new FameCounter(this);
            FameGoal = GetFameGoal(FameCounter.ClassStats[ObjectType].BestFame);
            ExperienceGoal = GetExpGoal(_client.Character.Level);
            Stars = GetStars();

            if (owner.Name.Equals("SummoningPoint"))
                RageBar = 100;
            if (owner.Name.Equals("UltraSummoningPoint"))
                RageBar = 100;
            /* if ((owner.Name.Equals("BastilleofDrannol")
                  || owner.Name.Equals("AldraginesHideout")
                  || owner.Name.Equals("UltraAldraginesHideout"))
                  && owner.Opener != Name) {
                 Client.Manager.Database.UpdateCredit(Client.Account, -1000);
                 this.ForceUpdate(Credits);
             }*/

            


            if (owner.Name.Equals("Nexus"))
            {
                int amount = Math.Min(6, Stats[9] + 1);
                if (amount > HealthPots.Count)
                    HealthPots = new ItemStacker(this, 254, 0x0A22, amount, 6);
                if (amount > MagicPots.Count)
                    MagicPots = new ItemStacker(this, 255, 0x0A23, amount, 6);
                var statInfo = Manager.Resources.GameData.Classes[ObjectType].Stats;
                for (var i = 0; i < statInfo.Length; i++)
                {
                    if (!AscensionEnabled)
                    {
                        if (Stats.Base[i] > statInfo[i].MaxValue)
                            Stats.Base[i] = statInfo[i].MaxValue;
                    }
                    else
                    {
                        if (Stats.Base[i] > statInfo[i].MaxValue + (i < 2 ? 50 : 10))
                            Stats.Base[i] = statInfo[i].MaxValue + (i < 2 ? 50 : 10);
                    }
                }
                SaveToCharacter();
            }



            SetNewbiePeriod();

            if (owner.IsNotCombatMapArea)
            {
                if (DeathArena.Instance?.CurrentState != DeathArena.ArenaState.NotStarted && DeathArena.Instance?.CurrentState != DeathArena.ArenaState.Ended)
                {
                    Client.SendPacket(new GlobalNotification
                    {
                        Type = GlobalNotification.ADD_ARENA,
                        Text = $"{{\"name\":\"Oryx Arena\",\"open\":{DeathArena.Instance?.CurrentState == DeathArena.ArenaState.CountDown}}}"
                    });
                }

                if (worlds.logic.Arena.Instance?.CurrentState != worlds.logic.Arena.ArenaState.NotStarted)
                {
                    Client.SendPacket(new GlobalNotification
                    {
                        Type = GlobalNotification.ADD_ARENA,
                        Text = $"{{\"name\":\"Public Arena\",\"open\":{worlds.logic.Arena.Instance?.CurrentState == worlds.logic.Arena.ArenaState.CountDown}}}"
                    });
                }
            }

            for (int i = 0; i < 4; i++)
                if (Inventory[i] != null) OnEquip(Inventory[i]);

            base.Init(owner);
        }

        public void drawAuras(Graphics g, Player player)
        {
            SolidBrush redFill = new SolidBrush(Color.Red);
            Pen red = new Pen(Color.Red);
            if (player.Client.Account.Rank >= 20)
            {
                g.DrawEllipse(red, x: player.X, y: player.Y, width: 100, height: 100);
                g.FillEllipse(redFill, player.X, player.Y, 100, 100);
            }
        }

        private readonly List<Timer> _timerList = new List<Timer>();
        public int[] StealAmount = { 0, 0 };
        public double LifeStealTrue;
        public double ManaStealTrue;

        private void TimerHandler(int delay, ConditionEffectIndex cei)
        {
            var timer = new Timer(delay, (int)cei);
            timer.Elapsed += (o, e) =>
            {
                Client.Player?.ApplyConditionEffect(cei);
                if (_timerList.Exists(t => t == timer)) _timerList.Remove(timer);
                timer.Dispose();
            };
            timer.Enabled = true;
            _timerList.Add(timer);
        }

        public void OnEquip(Item item)
        {
            if (Client.Player != null && item != null)
            {
                /*foreach (var pair in item.StatReq)
                    if (pair.Value < Stats[pair.Key])
                        Client.Disconnect();*/

                foreach (var pair in item.EffectEquip)
                    if (pair.Key != string.Empty)
                    {
                        TimerHandler(pair.Value * 1000,
                            (ConditionEffectIndex)Enum.Parse(typeof(ConditionEffectIndex),
                                pair.Key.Trim().Replace(" ", ""), true));
                    }

                foreach (var pair in item.Steal)
                    if (pair.Key != "")
                    {
                        if (pair.Key == "life") StealAmount[0] += pair.Value;
                        else StealAmount[1] += pair.Value;
                    }

                if (Inventory[0] != null)
                {
                    LifeStealTrue = 1.0 * StealAmount[0] / Math.Sqrt(Inventory[0].NumProjectiles) / Inventory[0].RateOfFire;
                    ManaStealTrue = 1.0 * StealAmount[1] / Math.Sqrt(Inventory[0].NumProjectiles) / Inventory[0].RateOfFire;
                }
            }
        }

        public void OnUnequip(Item item)
        {
            if (Client.Player != null && item != null)
            {
                foreach (var pair in item.EffectEquip)
                    if (pair.Key != string.Empty)
                    {
                        var cei = (int)Enum.Parse(typeof(ConditionEffectIndex), pair.Key.Trim().Replace(" ", ""), true);
                        foreach (var t in _timerList)
                            if (t.Id == cei)
                            {
                                t.Dispose();
                                _timerList.Remove(t);
                                return; //so that it only clears effs after the delay (unless no delay)
                            }
                        Client.Player.ApplyConditionEffect((ConditionEffectIndex)cei, 0);
                    }
                foreach (var pair in item.Steal)
                {
                    if (pair.Key != "")
                    {
                        if (pair.Key == "life") StealAmount[0] -= pair.Value;
                        else StealAmount[1] -= pair.Value;
                    }
                }

                if (Inventory[0] != null)
                {
                    LifeStealTrue = 1.0 * StealAmount[0] / Math.Sqrt(Inventory[0].NumProjectiles) / Inventory[0].RateOfFire;
                    ManaStealTrue = 1.0 * StealAmount[1] / Math.Sqrt(Inventory[0].NumProjectiles) / Inventory[0].RateOfFire;
                }
            }
        }

        public override void Tick()
        {
            if (!KeepAlive()) return;

            var tickCount = Manager.Core.getTickCount();

            if (tickCount % 3 == 0) HandleBastille();
            if (tickCount % 3 == 0) HandleUltraBastille();
            if (tickCount % 20 == 0)
            {
                CheckTradeTimeout();
                HandleQuest();
            }
            if (!HasConditionEffect(ConditionEffects.Paused) && tickCount % 300 == 0) FameCounter.Tick();

            if (this == null)
                return;

            base.Tick();

            SendUpdate();
            SendNewTick();
            HandleEffects();
            HandleProt();
            HandleRegen(); //moved here so people don't get 'slow' refills

            //todo: perhaps check if hp/mp is at max (and subsequientally remove it from the check)
            if (HP <= 0) Death("Unknown", rekt: true);
        }

        private float _hpRegenCounter;
        private float _mpRegenCounter;
        public double _hpStealCounter;
        public double _mpStealCounter;

        private void HandleRegen()
        {
            if (HasConditionEffect(ConditionEffects.Corrupted)) return;

            if (HP == Stats[0] || !CanHpRegen()) _hpRegenCounter = 0;
            else
            {
                _hpRegenCounter += Stats.GetHPRegen() * CoreConstant.worldTickMs / 1000f;

                var regen = (int)_hpRegenCounter;

                if (regen > 0)
                {
                    HP = Mark == 2 ?
                        Math.Min(Stats[0] + Convert.ToInt32(Stats[0] * 0.25), HP + regen) :
                        Math.Min(Stats[0], HP + regen);
                    _hpRegenCounter -= regen;
                }
            }

            if (CheckHalfHPArmored() && HP <= MaximumHP / 2)
            {
                ApplyConditionEffect(new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Armored,
                    DurationMS = 5000
                });
            }

            if (MP == Stats[1] || !CanMpRegen()) _mpRegenCounter = 0;
            else
            {
                _mpRegenCounter += Stats.GetMPRegen() * CoreConstant.worldTickMs / 1000f;

                var regen = (int)_mpRegenCounter;

                if (regen <= 0) return;

                MP = Mark == 1 ?
                    Math.Min(Stats[1] + Convert.ToInt32(Stats[1] * 0.25), MP + regen) :
                    Math.Min(Stats[1], MP + regen);

                _mpRegenCounter -= regen;
            }
        }

        public void TeleportPosition(float x, float y, bool ignoreRestrictions = false)
        {
            TeleportPosition(new Position() { X = x, Y = y }, ignoreRestrictions);
        }

        public void TeleportPosition(Position position, bool ignoreRestrictions = false)
        {
            if (!(Owner is Marketplace))
            {
                if (!ignoreRestrictions)
                {
                    if (!TPCooledDown())
                    {
                        SendError("Too soon to teleport again!");
                        return;
                    }

                    SetTPDisabledPeriod();
                    SetNewbiePeriod();
                    FameCounter.Teleport();
                }
            }

            HandleQuest(true, position);

            var id = (IsControlling) ? SpectateTarget.Id : Id;
            var tpPkts = new Packet[] {
                new Goto () {
                ObjectId = id,
                Pos = position
                }
            };

            bool hidden = HasConditionEffect(ConditionEffects.Hidden);

            if (!hidden)
                tpPkts.Append(new ShowEffect()
                {
                    EffectType = EffectType.Teleport,
                    TargetObjectId = id,
                    Pos1 = position,
                    Color = new ARGB(0xFFFFFFFF)
                });

            foreach (var plr in Owner.Players.Values)
            {
                if (hidden && plr.Admin == 0) continue;
                plr.AwaitGotoAck(Manager.Core.getTotalTickCount());
                plr.Client.SendPackets(tpPkts);
            }
        }

        public void UpdateMarks(Player player)
        {
            player.Client.SendPacket(new UpdateMarks()
            {
                Mark = player.Mark,
                Node1 = player.Node1,
                Node2 = player.Node2,
                Node3 = player.Node3,
                Node4 = player.Node4,
            });
        }

        public bool ascendSorCrystal(Player player)
        {
            for (int i = 0; i < Inventory.Length; i++)
            {
                if (Inventory[i] == null)
                    continue;

                if (Inventory[i].ObjectId == "Sor Crystal")
                {
                    Inventory[i] = Manager.Resources.GameData.Items[0x49e6];
                    SaveToCharacter();
                    SendInfo("Your Sor Crystal has been ascended into a Legendary Sor Crystal!");
                    Onrane = Client.Account.Onrane -= 20;
                    player.ForceUpdate(Onrane);
                    return false;
                };
            }
            return true;
        }

        public bool startRaid1(Player player)
        {
            for (int i = 0; i < Inventory.Length; i++)
            {
                if (Inventory[i] == null) continue;
                if (Inventory[i].ObjectId == "The Zol Awakening (Token)")
                {
                    Inventory[i] = null;
                    SaveToCharacter();
                    SendInfo("Your Zol Token has been used!");

                    return false;
                };
            }

            return true;
        }

        public bool startRaid2(Player player)
        {
            for (int i = 0; i < Inventory.Length; i++)
            {
                if (Inventory[i] == null) continue;

                if (Inventory[i].ObjectId == "Calling of the Titan (Token)")
                {
                    Inventory[i] = null;
                    SaveToCharacter();
                    SendInfo("Your Titan Token has been used!");

                    return false;
                };
            }

            return true;
        }

        public bool startRaid3(Player player)
        {
            for (int i = 0; i < Inventory.Length; i++)
            {
                if (Inventory[i] == null) continue;

                if (Inventory[i].ObjectId == "Malgoric Token")
                {
                    Inventory[i] = null;
                    SaveToCharacter();
                    SendInfo("Your Malgoric Token has been used!");

                    return false;
                };
            }

            return true;
        }

        public void CheckHitEffects()
        {
            int check = rng.Next(100);
            if (CheckDRage())
            {
                Stats.Boost.ActivateBoost[2].Push(30, true);
                Stats.ReCalculateValues();

                Owner.Timers.Add(new WorldTimer(5000, (w) =>
                {
                    if (w == null || w.Deleted) return;
                    Stats.Boost.ActivateBoost[2].Pop(30, true);
                    Stats.ReCalculateValues();
                }));
            }

            if (CheckNil())
            {
                Stats.Boost.ActivateBoost[0].Push(10, false);
                Stats.ReCalculateValues();

                Owner.Timers.Add(new WorldTimer(5000, (w) =>
                {
                    if (w == null || w.Deleted) return;
                    Stats.Boost.ActivateBoost[0].Pop(10, false);
                    Stats.ReCalculateValues();
                }));
            }

            if (CheckSource())
            {
                Stats.Boost.ActivateBoost[4].Push(-10, true);
                Stats.ReCalculateValues();

                Owner.Timers.Add(new WorldTimer(5000, (w) =>
                {
                    if (w == null || w.Deleted) return;
                    Stats.Boost.ActivateBoost[4].Pop(-10, true);
                    Stats.ReCalculateValues();
                }));
            }


            if (CheckLode() && check < 5)
            {

                Stats.Boost.ActivateBoost[0].Push(100, false);
                Stats.Boost.ActivateBoost[1].Push(100, false);
                Stats.Boost.ActivateBoost[2].Push(10, false);
                Stats.Boost.ActivateBoost[3].Push(10, false);
                Stats.Boost.ActivateBoost[4].Push(10, false);
                Stats.Boost.ActivateBoost[5].Push(10, false);
                Stats.Boost.ActivateBoost[6].Push(10, false);
                Stats.Boost.ActivateBoost[7].Push(10, false);
                Stats.Boost.ActivateBoost[8].Push(10, false);
                Stats.Boost.ActivateBoost[9].Push(10, false);
                Stats.Boost.ActivateBoost[10].Push(10, false);
                Stats.Boost.ActivateBoost[11].Push(10, false);
                Stats.ReCalculateValues();

                Owner.Timers.Add(new WorldTimer(5000, (w) =>
                {
                    if (w == null || w.Deleted) return;
                    Stats.Boost.ActivateBoost[0].Pop(100, false);
                    Stats.Boost.ActivateBoost[1].Pop(100, false);
                    Stats.Boost.ActivateBoost[2].Pop(10, false);
                    Stats.Boost.ActivateBoost[3].Pop(10, false);
                    Stats.Boost.ActivateBoost[4].Pop(10, false);
                    Stats.Boost.ActivateBoost[5].Pop(10, false);
                    Stats.Boost.ActivateBoost[6].Pop(10, false);
                    Stats.Boost.ActivateBoost[7].Pop(10, false);
                    Stats.Boost.ActivateBoost[8].Pop(10, false);
                    Stats.Boost.ActivateBoost[9].Pop(10, false);
                    Stats.Boost.ActivateBoost[10].Pop(10, false);
                    Stats.Boost.ActivateBoost[11].Pop(10, false);
                    Stats.ReCalculateValues();
                }));
            }
        }

        public void Teleport(int objId, bool ignoreRestrictions = false)
        {
            var obj = Owner.GetEntity(objId);

            if (obj == null)
            {
                SendError("Target does not exist.");
                return;
            }
            if (!ignoreRestrictions)
            {
                if (Id == objId)
                {
                    SendInfo("You are already at yourself, and always will be!");
                    return;
                }
                if (!Owner.AllowTeleport)
                {
                    SendError("Cannot teleport here.");
                    return;
                }
                if (HasConditionEffect(ConditionEffects.Paused))
                {
                    SendError("Cannot teleport while paused.");
                    return;
                }
                if (!(obj is Player))
                {
                    SendError("Can only teleport to players.");
                    return;
                }
                if (obj.HasConditionEffect(ConditionEffects.Invisible))
                {
                    SendError("Cannot teleport to an invisible player.");
                    return;
                }
                if (obj.HasConditionEffect(ConditionEffects.Paused))
                {
                    SendError("Cannot teleport to a paused player.");
                    return;
                }
                if (!TPCooledDown())
                {
                    SendError("Too soon to teleport again!");
                    return;
                }
                ApplyConditionEffect(ConditionEffectIndex.Invisible, 2000);
                ApplyConditionEffect(ConditionEffectIndex.Invincible, 2000);
                ApplyConditionEffect(ConditionEffectIndex.Stunned, 2000);
                TeleportPosition(obj.X, obj.Y, ignoreRestrictions);
            }
            else
            {
                ApplyConditionEffect(ConditionEffectIndex.Invisible, 2000);
                ApplyConditionEffect(ConditionEffectIndex.Invincible, 2000);
                ApplyConditionEffect(ConditionEffectIndex.Stunned, 2000);
                TeleportPosition(obj.X, obj.Y, ignoreRestrictions);
            }
            
        }

        public bool IsInvulnerable() =>
            HasConditionEffect(ConditionEffects.Paused) ||
            HasConditionEffect(ConditionEffects.Stasis) ||
            HasConditionEffect(ConditionEffects.Invincible) ||
            HasConditionEffect(ConditionEffects.Invulnerable);

        private int RestorationHeal()
        {
            if (Stats[9] == 6)
                return 150;
            return 100 + Stats[9] * 5;
        }

        public override bool HitByProjectile(Projectile projectile)
        {
            if (projectile.ProjectileOwner is Player)
            {
                var p = projectile.ProjectileOwner as Player;
                Console.WriteLine($"{p.Name} is attempting to damage {Name}!");
                System.IO.File.AppendAllText(@"logs\pvp.log", $"{p.Name} is attempting to damage {Name}!");
                p.Client.Disconnect();

                return false;
            }

            ApplyConditionEffect(projectile.ProjDesc.Effects);

            if (IsInvulnerable()) return false;

            var dmg = (int)Stats.GetDefenseDamage(projectile.Damage, projectile.ProjDesc.ArmorPiercing);

            CheckHitEffects();

            if (!HasConditionEffect(ConditionEffects.Invulnerable))
            {
                if (Protection > 0)
                {
                    if (dmg > Protection)
                    {
                        int overdmg = dmg - Protection;
                        HP -= overdmg;
                    }
                    Protection -= dmg;
                    //if (RollSacredEffect(SacredEffects.RunicOffense, 8)) RunicOff(dmg);
                    //if (RollSacredEffect(SacredEffects.RunicDefense, 8)) RunicDef(dmg);
                }
                else
                {
                    HP -= dmg;

                    //if (RollSacredEffect(SacredEffects.RunicOffense, 8)) RunicOff(dmg);
                    //if (RollSacredEffect(SacredEffects.RunicDefense, 8)) RunicDef(dmg);
                }
                SetProtRegenCooldown();
            }

            Owner.BroadcastPacketNearby(new Damage()
            {
                TargetId = Id,
                Effects = 0,
                DamageAmount = (ushort)dmg,
                Kill = HP <= 0,
                BulletId = 0,
                ObjectId = projectile.ProjectileOwner.Self.Id
            }, this, this);

            if (projectile.ProjDesc.LifeLeech && projectile.ProjectileOwner.Self is Character i && i != null)
            {
                var heal = Math.Min(dmg, i.MaximumHP - i.HP);

                i.HP += heal;
                i.SendHealPacket(heal);
            }

            if (projectile.ProjDesc.GroupLifeLeech && projectile.ProjectileOwner.Self is Character j && j != null && j.ObjectDesc.Group != null)
            {
                foreach (Character k in j.GetNearestEntitiesByGroup(25, j.ObjectDesc.Group))
                {
                    var heal = Math.Min(dmg, k.MaximumHP - k.HP);

                    k.HP += heal;
                    k.SendHealPacket(heal);
                }
            }

            if (HP <= 0) Death(projectile.ProjectileOwner.Self.ObjectDesc.ObjectId, projectile.ProjectileOwner.Self);

            return base.HitByProjectile(projectile);
        }

        public void Damage(int dmg, Entity src, bool noDef)
        {
            if (src is Player)
            {
                System.Console.WriteLine($"{(src as Player).Name} attempted to damage {Name}.");
                System.IO.File.WriteAllText(@"logs/pvp.log", $"{(src as Player).Name} attempted to damage {Name}.");
                (src as Player).Client.Disconnect();
            }

            if (IsInvulnerable()) return;

            //if (RollSacredEffect(SacredEffects.AdrenalineRush, 8)) return;

            CheckHitEffects();

            var d = (int)Stats.GetDefenseDamage(dmg, noDef);

            SetProtRegenCooldown();
            if (Protection > 0)
            {
                if (d > Protection)
                {
                    int overdmg = d - Protection;
                    HP -= overdmg;
                }
                Protection -= d;
                //if (RollSacredEffect(SacredEffects.RunicOffense, 8)) RunicOff(d);
                //if (RollSacredEffect(SacredEffects.RunicDefense, 8)) RunicDef(d);
            }
            else
            {
                HP -= d;

                //if (RollSacredEffect(SacredEffects.RunicOffense, 8)) RunicOff(d);
                //if (RollSacredEffect(SacredEffects.RunicDefense, 8)) RunicDef(d);
            }

            Owner.BroadcastPacketNearby(new Damage()
            {
                TargetId = Id,
                Effects = 0,
                DamageAmount = (ushort)d,
                Kill = HP <= 0,
                BulletId = 0,
                ObjectId = src.Id
            }, this, this);

            if (HP <= 0)
                Death(src.ObjectDesc.ObjectId, src);
        }

        public void Unbox(int type)
        {

            var dude = Client.Player;

            var acc = Client.Account;
            Random rand1 = new Random();
            ushort[] items = new ushort[50];
            for (int x = 0; x < 50; x++)
            {
                var result = GetUnboxResult(type, rand1);
                items[x] = result.Item1.ObjectType;
            }
            Manager.Database.AddGift(acc, items[45]);
            
            Client.SendPacket(new UnboxResult()
            {
                Items = items
            });

            Owner.Timers.Add(new WorldTimer(10000, (w) =>
            {
                if (w == null || w.Deleted) return;

                foreach (var player in w.Players.Values.Where(p => p != null))
                    if(Manager.Resources.GameData.Items[items[45]].BagType >= 13)
                    player.SendHelp("<" + Name + "> unboxed a " +
                        "'" + Manager.Resources.GameData.Items[items[45]].ObjectId + "' " +
                        "from the " + LootboxType(type) + "!");
            }));
        }

        //private void RunicOff(int damage)
        //{
        //    var dmg = (1.25 + SacredBoost(SacredEffects.RunicOffense, 0.25)) * damage;
        //    var r = 3 + SacredBoost(SacredEffects.RunicOffense, 1);
        //    var pkts = new List<Packet>();
        //    this.AOE((float)r, false, enemy => (enemy as Enemy).Damage(this, (int)dmg, false));
        //    pkts.Add(new ShowEffect
        //    {
        //        EffectType = EffectType.AreaBlast,
        //        TargetObjectId = Id,
        //        Color = new ARGB(0xe03434),
        //        Pos1 = new Position { X = (float)r }
        //    });
        //    BroadcastSync(pkts, p => this.DistSqr(p) < RadiusSqr);
        //}

        //private void RunicDef(int damage)
        //{
        //    var r = 3 + SacredBoost(SacredEffects.RunicDefense, 1);
        //    var pkts = new List<Packet>();
        //    this.AOE((float)r, true, player =>
        //    {
        //        if (!player.HasConditionEffect(ConditionEffects.Sick) ||
        //            !player.HasConditionEffect(ConditionEffects.Corrupted))
        //        {
        //            var heal = damage;
        //            if (heal <= 0) heal = 1;
        //            ActivateHealHp(player as Player, heal, pkts);
        //        }
        //    });
        //    pkts.Add(new ShowEffect
        //    {
        //        EffectType = EffectType.AreaBlast,
        //        TargetObjectId = Id,
        //        Color = new ARGB(0xe03434),
        //        Pos1 = new Position { X = (float)r }
        //    });
        //    BroadcastSync(pkts, p => this.DistSqr(p) < RadiusSqr);
        //}

        private void GenerateGravestone()
        {
            var playerDesc = Manager.Resources.GameData.Classes[ObjectType];
            var maxed = playerDesc.Stats.Where((t, i) => Stats.Base[i] >= t.MaxValue).Count();
            ushort objType;
            int time;
            if (Client.Account.Rank >= 20)
            {
                switch (maxed)
                {
                    case 12: objType = 0x7264; time = 600000; break;
                    case 11: objType = 0x7263; time = 600000; break;
                    case 10: objType = 0x7262; time = 600000; break;
                    case 9: objType = 0x7262; time = 600000; break;
                    case 8: objType = 0x7262; time = 600000; break;
                    case 7: objType = 0x7261; time = 600000; break;
                    case 6: objType = 0x7260; time = 600000; break;
                    case 5: objType = 0x7259; time = 600000; break;
                    case 4: objType = 0x7258; time = 600000; break;
                    case 3: objType = 0x7257; time = 600000; break;
                    case 2: objType = 0x7256; time = 600000; break;
                    case 1: objType = 0x7255; time = 600000; break;
                    default:
                        objType = 0x7254; time = 300000;
                        if (Level < 20) { objType = 0x7253; time = 60000; }
                        if (Level <= 1) { objType = 0x7252; time = 30000; }
                        break;
                }
            }
            else
            {
                switch (maxed)
                {
                    case 12: objType = 0x79cf; time = 600000; break;
                    case 11: objType = 0x79ce; time = 600000; break;
                    case 10: objType = 0x785d; time = 600000; break;
                    case 9: objType = 0x785c; time = 600000; break;
                    case 8: objType = 0x7735; time = 600000; break;
                    case 7: objType = 0x7734; time = 600000; break;
                    case 6: objType = 0x792b; time = 600000; break;
                    case 5: objType = 0x792a; time = 600000; break;
                    case 4: objType = 0x7729; time = 600000; break;
                    case 3: objType = 0x7728; time = 600000; break;
                    case 2: objType = 0x7727; time = 600000; break;
                    case 1: objType = 0x7726; time = 600000; break;
                    default:
                        objType = 0x7725; time = 300000;
                        if (Level < 20) { objType = 0x7724; time = 60000; }
                        if (Level <= 1) { objType = 0x7723; time = 30000; }
                        break;
                }
            }
            if (Client.Account.Rank >= 20)
            {
                var obj = new StaticObject(Manager, objType, time, true, true, false);
                obj.Move(X, Y);
                obj.Name = Name;
                Owner.EnterWorld(obj);
            }
            else
            {
                var obj = new StaticObject(Manager, objType, time, true, true, false);
                obj.Move(X, Y);
                obj.Name = Name;
                Owner.EnterWorld(obj);
            }
        }

        public double thresholdBoost()
        {
            if (SupportScore >= 1000 && SupportScore <= 2000)
            {
                return .9;
            }
            else if (SupportScore >= 2001 && SupportScore <= 3000)
            {
                return .8;
            }
            else if (SupportScore >= 4001 && SupportScore <= 5000)
            {
                return .7;
            }
            else if (SupportScore >= 5001 && SupportScore <= 6000)
            {
                return .6;
            }
            else if (SupportScore >= 6001 && SupportScore <= 7000)
            {
                return .5;
            }
            else if (SupportScore >= 7001 && SupportScore <= 8000)
            {
                return .4;
            }
            else if (SupportScore >= 8001 && SupportScore <= 9000)
            {
                return .3;
            }
            else if (SupportScore >= 9001 && SupportScore <= 10000)
            {
                return .2;
            }
            else if (SupportScore >= 10001 && SupportScore <= 11000)
            {
                return .1;
            }
            else if (SupportScore >= 11001 && SupportScore <= 12000)
            {
                return .09;
            }
            else if (SupportScore >= 12001 && SupportScore <= 13001)
            {
                return .08;
            }
            else if (SupportScore >= 14001 && SupportScore <= 15000)
            {
                return .07;
            }
            else if (SupportScore >= 14001 && SupportScore <= 15000)
            {
                return .06;
            }
            else if (SupportScore >= 15001 && SupportScore <= 16000)
            {
                return .05;
            }
            else if (SupportScore >= 16001 && SupportScore <= 17000)
            {
                return .045;
            }
            else if (SupportScore >= 17001 && SupportScore <= 18000)
            {
                return .040;
            }
            else if (SupportScore >= 18001 && SupportScore <= 19000)
            {
                return .035;
            }
            else if (SupportScore >= 19001 && SupportScore <= 20000)
            {
                return .030;
            }
            else if (SupportScore > 20000)
            {
                return .025;
            }
            return 1;
        }

        private bool Arena(string killer)
        {
            if (!(Owner is Arena) && !(Owner is ArenaSolo))
                return false;

            foreach (var player in Owner.Players.Values)
                player.SendInfo("{\"key\":\"{arena.death}\",\"tokens\":{\"player\":\"" + Name + "\",\"enemy\":\"" + killer + "\"}}");

            ReconnectToNexus();
            return true;
        }

        private bool NonPermaKillEnemy(Entity entity, string killer)
        {
            if (entity == null)
            {
                return false;
            }

            if (!entity.Spawned && entity.Controller == null)
                return false;
           
            if (entity.Spawned)
                return false;
            ReconnectToNexus();
            return true;
        }

        private bool Rekted(bool rekt)
        {
            if (!rekt)
                return false;
            ReconnectToNexus();
            return true;
        }

        private bool TestWorld(string killer)
        {
            if (!(Owner is Test))
                return false;

            GenerateGravestone();
            ReconnectToNexus();
            return true;
        }

        private bool _dead;
        private Client client;

        private bool Resurrection()
        {
            for (int i = 0; i < 4; i++)
            {
                var item = Inventory[i];
                Random rnd = new Random();
                int chance = rnd.Next(1, 10);
                if (item == null || !item.Resurrects)
                    continue;

                Inventory[i] = null;
                if (!(chance == 1))
                {
                    foreach (var player in Owner.Players.Values)
                        player.SendInfo($"{Name}'s {item.DisplayName} breaks and he disappears");

                    ReconnectToNexus();
                    return true;
                }
                else 
                {
                    foreach (var player in Owner.Players.Values)
                        player.SendInfo($"{Name}'s {item.DisplayName} broke but failed to protect them.");
                }
            }
            return false;
        }

        private bool SecondChance()
        {
            Random rnd = new Random();
            int chance = rnd.Next(1, 5);
            if (Mark == 5)
            {
                if (chance == 1)
                {
                    foreach (var player in Owner.Players.Values)
                        player.SendInfo($"{Name} suddenly vanishes..");
                    ReconnectToNexus();
                    return true;
                }
            }
            return false;
        }

        private bool isNexus()
        {
            if (Owner is Nexus)
            {
                ReconnectToNexus();
                return true;
            }
            return false;
        }

        private bool isAlertArea()
        {
            var amount = (int)(Credits * 0.1);
            if (Owner.Name == "KrakenLair" || Owner.Name == "TheHollows" || Owner.Name == "HiddenTempleBoss" || Owner.Name == "FrozenIsland")
            {
                Client.Manager.Database.UpdateCredit(Client.Account, -amount);
                Credits = Client.Account.Credits - amount;
                foreach (var player in Owner.Players.Values)
                    player.SendInfo("isAlert");
                ReconnectToNexus();
                return true;
            }
            return false;
        }

        private bool isAdminsArena()
        {
            if (Owner.Name.Equals("Admins Arena") || Owner.Name.Equals("Balancer Station") || Owner.Name.Equals("Test Dream World"))
            {
                Random rnd = new Random();
                int num = rnd.Next(1, 11);
                switch (num)
                {
                    case 1:
                        foreach (var player in Owner.Players.Values)
                            player.SendInfo($"{Name} was utterly destroyed..");
                        break;

                    case 2:
                        foreach (var player in Owner.Players.Values)
                            player.SendInfo($"{Name} got bopped.");
                        break;

                    case 3:
                        foreach (var player in Owner.Players.Values)
                            player.SendInfo($"Later, {Name}!");
                        break;

                    case 4:
                        foreach (var player in Owner.Players.Values)
                            player.SendInfo($"{Name} couldn't take the fight.");
                        break;

                    case 5:
                        foreach (var player in Owner.Players.Values)
                            player.SendInfo($"{Name} got overwhelmed.");
                        break;

                    case 6:
                        foreach (var player in Owner.Players.Values)
                            player.SendInfo($"Sidon spares his mercy to {Name}.");
                        break;

                    case 7:
                        foreach (var player in Owner.Players.Values)
                            player.SendInfo($"Goodnight, {Name}.");
                        break;

                    case 8:
                        foreach (var player in Owner.Players.Values)
                            player.SendInfo($"{Name} IS OUTTA HERE!");
                        break;

                    case 9:
                        foreach (var player in Owner.Players.Values)
                            player.SendInfo($"{Name} has been E L I M I N A T E D.");
                        break;

                    case 10:
                        foreach (var player in Owner.Players.Values)
                            player.SendInfo($"CY@, {Name}");
                        break;
                }
                foreach (var player in Owner.Players.Values)
                    player.SendInfo("isAdminArena");
                ReconnectToNexus();
                return true;
            }
            return false;
        }

        private void ReconnectToNexus()
        {
            HP = 1;
            _client.Reconnect(new Reconnect()
            {
                Host = "",
                Port = 2050,
                GameId = World.Nexus,
                Name = "Nexus"
            });
        }

        private void AnnounceDeath(string killer)
        {
            var playerDesc = Manager.Resources.GameData.Classes[ObjectType];
            var maxed = playerDesc.Stats.Where((t, i) => Stats.Base[i] >= t.MaxValue).Count();
            if (AscensionEnabled)
            {
                if (Stats.Base[0] >= playerDesc.Stats[0].MaxValue + 50) maxed++;
                if (Stats.Base[1] >= playerDesc.Stats[1].MaxValue + 50) maxed++;

                for (int i = 2; i < 9; i++)
                {
                    if (Stats.Base[i] >= playerDesc.Stats[i].MaxValue + 10)
                    {
                        maxed++;
                    }
                }
                if (Stats.Base[9] >= playerDesc.Stats[9].MaxValue + 3) maxed++;
            }

            var deathMessage = "<" + Name + "> died to " + killer +
                " (" + maxed + (AscensionEnabled ? "/20 " : "/10 ") + playerDesc.ObjectId +
                ", " + Fame + " BF)";

            // notable deaths
            if ((maxed >= 10 || Fame >= 5000) && !Client.Account.Admin)
            {
                foreach (var w in Manager.Worlds.Values)
                    foreach (var p in w.Players.Values)
                        p.SendHelp(deathMessage);
                return;
            }

            var pGuild = Client.Account.GuildId;

            // guild case, only for level 20
            if (pGuild > 0 && Level == 20)
            {
                foreach (var w in Manager.Worlds.Values)
                {
                    foreach (var p in w.Players.Values)
                    {
                        if (p.Client.Account.GuildId == pGuild)
                        {
                            p.SendGuildAnnounce(deathMessage);
                        }
                    }
                }

                foreach (var i in Owner.Players.Values)
                {
                    if (i.Client.Account.GuildId != pGuild)
                    {
                        i.SendInfo(deathMessage);
                    }
                }
            }
            // guild less case
            else
            {
                foreach (var i in Owner.Players.Values)
                {
                    i.SendInfo(deathMessage);
                }
            }
        }

        public void Death(string killer, Entity entity = null, WmapTile tile = null, bool rekt = false)
        {
            if (_client.State == ProtocolState.Disconnected || _dead)
                return;

            if (tile != null && tile.Spawned)
            {
                rekt = true;
            }

            _dead = true;
            if (Rekted(rekt))
                return;
            if (Arena(killer))
                return;
            if (NonPermaKillEnemy(entity, killer))
                return;
            //if (TestWorld(killer))
               // return;
            if (isNexus())
                return;
            if (isAlertArea())
                return;
            if (isAdminsArena())
                return;
            if (SecondChance())
                return;
            if (Resurrection())
                return;

            SaveToCharacter();

            if ((entity is Player))
            {
                Manager.Database.Death(Manager.Resources.GameData, _client.Account,
               _client.Character, FameCounter.Stats, (entity as Player).Name + " the " + entity.ObjectDesc.ObjectId);
            }
            else
            {
                Manager.Database.Death(Manager.Resources.GameData, _client.Account,
                _client.Character, FameCounter.Stats, killer);
            }
            var playerDesc = Manager.Resources.GameData.Classes[ObjectType];
            var maxed = playerDesc.Stats.Where((t, i) => Stats.Base[i] >= t.MaxValue).Count();
            if (AscensionEnabled)
            {
                if (Stats.Base[0] >= playerDesc.Stats[0].MaxValue + 50) maxed++;
                if (Stats.Base[1] >= playerDesc.Stats[1].MaxValue + 50) maxed++;

                for (int i = 2; i < 9; i++)
                {
                    if (Stats.Base[i] >= playerDesc.Stats[i].MaxValue + 10)
                    {
                        maxed++;
                    }
                }
                if (Stats.Base[9] >= playerDesc.Stats[9].MaxValue + 3) maxed++;
            }

            GenerateGravestone();

            if ((entity is Player))
            {
                AnnounceDeath((entity as Player).Name + " the " + entity.ObjectDesc.ObjectId);
            }
            else
            {
                AnnounceDeath(killer);
            }

            if ((entity is Player))
            {
                _client.SendPacket(new Death()
                {
                    AccountId = AccountId.ToString(),
                    CharId = _client.Character.CharId,
                    KilledBy = (entity as Player).Name + " the " + entity.ObjectDesc.ObjectId,
                    ZombieId = -1
                });
            }
            else
            {
                _client.SendPacket(new Death()
                {
                    AccountId = AccountId.ToString(),
                    CharId = _client.Character.CharId,
                    KilledBy = killer,
                    ZombieId = -1
                });
            }

            Owner.Timers.Add(new WorldTimer(1000, (w) =>
            {
                if (w == null || w.Deleted || this == null || _client.Player != this || _client == null) return;

                _client.Disconnect("Death");
            }));
        }

        public void CheckZolReborn(string entity)
        {
            if (Owner.Name == "Aldragine's Hideout" ||
                Owner.Name == "Ultra Aldragine's Hideout" ||
                Owner.Name == "Sincryer's Gate" ||
                Owner.Name == "Ultra Sincryer's Gate" ||
                Owner.Name == "Nontridus" ||
                Owner.Name == "NontridusUltra" ||
                Owner.Name == "Core of the Hideout" ||
                Owner.Name == "Ultra Core of the Hideout" ||
                Owner.Name == "Keeping of Aldragine" ||
                Owner.Name == "KeepingUltra")
            {
                Entity en = Entity.Resolve(Owner.Manager, entity);
                en.Move((int)X, (int)Y);
                Owner.EnterWorld(en);
            }
        }

        public void Reconnect(World world)
        {
            //Log.Info($"[Method] Reconnecting client to world instance {world.Name} [{world.Id}].");

            Client.Reconnect(new Reconnect()
            {
                Host = "",
                Port = 2050,
                GameId = world.Id,
                Name = world.Name
            });
        }

        public void Reconnect(object portal, World world)
        {
            ((Portal)portal).WorldInstanceSet -= Reconnect;

            if (((Portal)portal).Locked)
            {
                SendError("Portal is locked.");
                return;
            }

            if (world == null)
            {
                SendError("Portal is not implemented.");
                return;
            }

            //Log.Info($"[Event] Reconnecting client to world instance {world.Name} [{world.Id}].");

            Client.Reconnect(new Reconnect()
            {
                Host = "",
                Port = 2050,
                GameId = world.Id,
                Name = world.Name
            });
        }

        public int GetCurrency(CurrencyType currency)
        {
            switch (currency)
            {
                case CurrencyType.Gold:
                    return Credits;

                case CurrencyType.Fame:
                    return CurrentFame;

                case CurrencyType.Tokens:
                    return Tokens;

                case CurrencyType.Kantos:
                    return Kantos;

                case CurrencyType.Onrane:
                    return Onrane;

                default:
                    return 0;
            }
        }

        public void SetCurrency(CurrencyType currency, int amount)
        {
            switch (currency)
            {
                case CurrencyType.Gold:
                    Credits = amount;
                    break;

                case CurrencyType.Fame:
                    CurrentFame = amount;
                    break;

                case CurrencyType.Tokens:
                    Tokens = amount;
                    break;

                case CurrencyType.Kantos:
                    Kantos = amount;
                    break;

                case CurrencyType.Onrane:
                    Onrane = amount;
                    break;
            }
        }

        public override void Move(float x, float y)
        {
            if (SpectateTarget != null && !(SpectateTarget is Player)) SpectateTarget.MoveEntity(x, y);
            else base.Move(x, y);

            if ((int)X != Sight.LastX || (int)Y != Sight.LastY)
            {
                if (IsNoClipping()) _client.Manager.Core.addPendingAction(() => _client.Disconnect());

                Sight.UpdateCount++;
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            if (SpectateTarget != null)
            {
                SpectateTarget.FocusLost -= ResetFocus;
                SpectateTarget.Controller = null;
            }

            _clientEntities.Dispose();
        }

        // allow other admins to see hidden people
        public override bool CanBeSeenBy(Player player) =>
            Client?.Account != null && Client.Account.Hidden ? player.Admin != 0 : true;

        public void SetDefaultSkin(int skin)
        {
            _originalSkin = skin;
            Skin = skin;
        }

        public void RestoreDefaultSkin()
        {
            Skin = _originalSkin;
        }

        public void DropNextRandom()
        {
            Client.Random.NextInt();
        }
    }
}
