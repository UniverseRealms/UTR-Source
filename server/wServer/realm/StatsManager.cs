using common.resources;
using System;
using wServer.realm.entities;

namespace wServer.realm
{
    public class StatsManager
    {
        internal const int NUM_STAT_TYPES = 15; // change this to add more stats
        private const float MIN_ATTACK_MULT = 0.5f;
        private const float MAX_ATTACK_MULT = 2f;
        private const float MIN_ATTACK_FREQ = 0.0015f;
        private const float MAX_ATTACK_FREQ = 0.008f;

        internal readonly Player Owner;
        internal readonly BaseStatManager Base;
        internal readonly BoostStatManager Boost;

        private readonly SV<int>[] _stats;

        public int this[int index] => Base[index] + Boost[index];

        public StatsManager(Player owner)
        {
            Owner = owner;
            Base = new BaseStatManager(this);
            Boost = new BoostStatManager(this);

            _stats = new SV<int>[NUM_STAT_TYPES];
            for (var i = 0; i < NUM_STAT_TYPES; i++)
                _stats[i] = new SV<int>(Owner, GetStatType(i), this[i], i != 0 && i != 1); // make maxHP and maxMP global update
        }

        public int GetStats(int id) => Owner.Stats[id] + Owner.Stats.Boost[id];

        public void ReCalculateValues(InventoryChangedEventArgs e = null)
        {
            Base.ReCalculateValues(e);
            Boost.ReCalculateValues(e);

            for (var i = 0; i < _stats.Length; i++)
                _stats[i].SetValue(this[i]);
        }

        internal void StatChanged(int index)
        {
            _stats[index].SetValue(this[index]);
        }

        public int GetAttackDamage(int min, int max, bool isAbility = false)
        {
            double dmg = Owner.Client.Random.NextIntRange((uint)min, (uint)max);

            int critRoll = (int)Owner.Client.Random.NextIntRange(1, 1001);

            dmg *= GetAttackMult(isAbility);

            if (Owner.HasConditionEffect(ConditionEffects.Bravery) && IsCritical(critRoll))
                dmg *= 2;

            if (Owner.CheckDemo())
                dmg *= 1.3;

            if (Owner.Mark == 3 && Owner.Surge < 50)
                dmg *= (2.0 - Owner.Surge / 50);
            dmg += BonusDamage();

            return (int)dmg;
        }

        private bool IsCritical(int roll)
        {
            var critChance = 0;

            if (Owner.HasConditionEffect(ConditionEffects.Bravery))
                critChance = 300;

            return roll <= critChance;
        }

        private int BonusDamage()
        {
            int dmg = 0;

            if (Owner.HasConditionEffect(ConditionEffects.Vengeance))
                dmg += (Owner.Stats[0] - Owner.HP) / 2;

            if (Owner.HasConditionEffect(ConditionEffects.Relentless))
                dmg += Owner.Surge * 6;

            if (Owner.CheckKar() && Owner.HasConditionEffect(ConditionEffects.Invisible))
                dmg += Owner.Stats[7] * 5 + 20;

            return dmg;
        }

        public float GetTilesPerSecSqr()
        {
            var ret = 4 + 5.6f * (this[4] / 75f);

            if (Owner.HasConditionEffect(ConditionEffects.Slowed))
                return ret / 2;

            if (Owner.HasConditionEffect(ConditionEffects.Speedy))
                return ret * 1.5f;

            return ret * ret;
        }

        public float GetAttackMult(bool isAbility)
        {
            if (isAbility)
                return 1;
            float mult = 1;
            if (Owner.HasConditionEffect(ConditionEffects.Weak))
            {
                mult = MIN_ATTACK_MULT + (this[2] / 4 / 50f);
            } else
            {
                mult = MIN_ATTACK_MULT + (this[2] / 50f);
            }
                

            if (Owner.HasConditionEffect(ConditionEffects.Damaging))
                mult *= 1.3f;

            if (Owner.CheckMerc())
                mult *= 1.15f;

            return mult;
        }

        public float GetAttackFrequency()
        {
            float rof = 1;
            if (Owner.HasConditionEffect(ConditionEffects.Dazed))
            {
                rof = MIN_ATTACK_FREQ + (this[5] / 75f / 4) * (MAX_ATTACK_FREQ - MIN_ATTACK_FREQ);
            } else
            {
                rof = MIN_ATTACK_FREQ + (this[5] / 75f) * (MAX_ATTACK_FREQ - MIN_ATTACK_FREQ);
            }

            if (Owner.HasConditionEffect(ConditionEffects.Berserk))
                rof *= 1.3f;

            if (Owner.HasConditionEffect(ConditionEffects.SamuraiBerserk))
                rof *= 1.3f;

            if (Owner.HasConditionEffect(ConditionEffects.GraspofZol))
                rof *= 1.5f;

            if (Owner.HasConditionEffect(ConditionEffects.Alliance))
                rof *= 1.8f;

            rof *= 0.75f;

            return rof;
        }

        public static float GetDefenseDamage(Entity host, int dmg, int def)
        {
            if (host.HasConditionEffect(ConditionEffects.Armored))
                def *= 2;

            if (host.HasConditionEffect(ConditionEffects.ArmorBroken))
                def = (int)(def / 4);

            float limit = dmg * 0.15f;//0.15f;

            float ret;
            if (dmg - def < limit)
                ret = limit;
            else
                ret = dmg - def;

            if (host.HasConditionEffect(ConditionEffects.Curse))
                ret = (int)(ret * 1.20);

            if (host.HasConditionEffect(ConditionEffects.Invulnerable) || host.HasConditionEffect(ConditionEffects.Invincible))
                ret = 0;

            return ret;
        }

        public float GetDefenseDamage(int dmg, bool noDef)
        {
            var def = this[3];

            if (Owner.HasConditionEffect(ConditionEffects.Armored))
                def *= 2;

            if (Owner.HasConditionEffect(ConditionEffects.ArmorBroken) || noDef)
                def = (int)(def / 4);

            float limit = dmg * 0.15f;//0.15f;

            float ret;
            if (dmg - def < limit)
                ret = limit;
            else
                ret = dmg - def;

            if (Owner.HasConditionEffect(ConditionEffects.Petrify))
                ret = (int)(ret * .9);
            if (Owner.HasConditionEffect(ConditionEffects.Curse))
                ret = (int)(ret * 1.20);
            if (Owner.HasConditionEffect(ConditionEffects.Invulnerable) ||
                Owner.HasConditionEffect(ConditionEffects.Invincible))
                ret = 0;
            return ret;
        }

        public static float GetSpeed(Entity entity, float stat)
        {
            var ret = 4 + 5.6f * (stat / 75f);

            if (entity.HasConditionEffect(ConditionEffects.Speedy))
                ret *= 1.5f;

            if (entity.HasConditionEffect(ConditionEffects.Swiftness))
                ret *= 1.7f;

            if (entity.HasConditionEffect(ConditionEffects.Slowed))
                ret = 4 + 5.6f * (stat / 75f / 4);

            if (entity.HasConditionEffect(ConditionEffects.Paralyzed))
                ret = 0;

            return ret;
        }

        public float GetSpeed() => GetSpeed(Owner, this[4]);

        public float GetHPRegen()
        {
            if (Owner.HasConditionEffect(ConditionEffects.Corrupted) || Owner.HasConditionEffect(ConditionEffects.Sick))
                return 0;

            int vit = this[6];


            float multi = 1f;
            if (Owner.HasConditionEffect(ConditionEffects.Healing))
                multi *= 2;
            //float boost = (float)Owner.SacredBoost(SacredEffects.EnhancedGrowth, 0.1) + 1;

            return Math.Max(multi * (0.21f * vit + 5), 0);
        }

        public float GetMPRegen()
        {
            if (Owner.HasConditionEffect(ConditionEffects.Corrupted) || Owner.HasConditionEffect(ConditionEffects.Quiet))
                return 0;

            int wis = this[7];
            float multi = 1f;
            //float boost = (float)Owner.SacredBoost(SacredEffects.OpenMind, 0.1) + 1;

            if (Owner.Mark == 4 && Owner.Surge >= 25)
                multi *= 1.75f;

            if (Owner.HasConditionEffect(ConditionEffects.ManaRecovery))
                multi *= 2f;

            if (Owner.HasConditionEffect(ConditionEffects.Empowered) && !(Owner.ObjectType == 0x0328))
                multi *= 2f;

            return Math.Max(multi * (wis * 0.14f + 2.4f), 0); //Math.Max(multi * (wis * 0.07f + 1.6f), 0)
        }

        public string StatIndexToFullName(int index)
        {
            switch (index)
            {
                case 0: return "HP";
                case 1: return "MP";
                case 2: return "Attack";
                case 3: return "Defense";
                case 4: return "Speed";
                case 5: return "Dexterity";
                case 6: return "Vitality";
                case 7: return "Wisdom";
                case 8: return "Luck";
                case 9: return "Restoration";
                case 12: return "DamageMin";
                case 13: return "DamageMax";
            }
            return null;
        }

        public static int GetStatIndex(string name)
        {
            switch (name)
            {
                case "MaxHitPoints": return 0;
                case "MaxMagicPoints": return 1;
                case "Attack": return 2;
                case "Defense": return 3;
                case "Speed": return 4;
                case "Dexterity": return 5;
                case "HpRegen": return 6;
                case "MpRegen": return 7;
                case "Luck": return 8;
                case "Restoration": return 9;
                case "DamageMin": return 12;
                case "DamageMax": return 13;
            }
            return -1;
        }

        public static int GetStatIndex(StatsType stat)
        {
            switch (stat)
            {
                case StatsType.MaximumHP:
                    return 0;

                case StatsType.MaximumMP:
                    return 1;

                case StatsType.Attack:
                    return 2;

                case StatsType.Defense:
                    return 3;

                case StatsType.Speed:
                    return 4;

                case StatsType.Dexterity:
                    return 5;

                case StatsType.Vitality:
                    return 6;

                case StatsType.Wisdom:
                    return 7;

                case StatsType.Luck:
                    return 8;

                case StatsType.Restoration:
                    return 9;

                case StatsType.DamageMin:
                    return 12;

                case StatsType.DamageMax:
                    return 13;

                default:
                    return -1;
            }
        }

        public static StatsType GetStatType(int stat)
        {
            switch (stat)
            {
                case 0:
                    return StatsType.MaximumHP;

                case 1:
                    return StatsType.MaximumMP;

                case 2:
                    return StatsType.Attack;

                case 3:
                    return StatsType.Defense;

                case 4:
                    return StatsType.Speed;

                case 5:
                    return StatsType.Dexterity;

                case 6:
                    return StatsType.Vitality;

                case 7:
                    return StatsType.Wisdom;

                case 8:
                    return StatsType.Luck;

                case 9:
                    return StatsType.Restoration;

                case 12:
                    return StatsType.DamageMin;

                case 13:
                    return StatsType.DamageMax;

                default:
                    return StatsType.None;
            }
        }

        public static StatsType GetBoostStatType(int stat)
        {
            switch (stat)
            {
                case 0:
                    return StatsType.HPBoost;

                case 1:
                    return StatsType.MPBoost;

                case 2:
                    return StatsType.AttackBonus;

                case 3:
                    return StatsType.DefenseBonus;

                case 4:
                    return StatsType.SpeedBonus;

                case 5:
                    return StatsType.DexterityBonus;

                case 6:
                    return StatsType.VitalityBonus;

                case 7:
                    return StatsType.WisdomBonus;

                case 8:
                    return StatsType.LuckBonus;

                case 9:
                    return StatsType.RestorationBonus;

                case 12:
                    return StatsType.DamageMinBonus;

                case 13:
                    return StatsType.DamageMaxBonus;

                default:
                    return StatsType.None;
            }
        }
    }
}