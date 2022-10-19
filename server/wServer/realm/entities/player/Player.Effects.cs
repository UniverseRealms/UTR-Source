using common.resources;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using wServer.networking.packets;
using wServer.networking.packets.outgoing;
using wServer.realm.cores;

namespace wServer.realm.entities
{
    partial class Player
    {
        private float _bleeding;
        private int _newbieTime;
        private int _canTpCooldownTime;
        private double _protDelay = 0;
        private double _protRegen = 0;

        private void permdebufftest(ConditionEffect condeff)
        {
            if (condeff.DurationMS <= 0)
            {
                new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Slowed,

                    DurationMS = 0
                };
                new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Paralyzed,
                    DurationMS = 0
                };
                new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Weak,
                    DurationMS = 0
                };
                new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Stunned,
                    DurationMS = 0
                };
                new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Confused,
                    DurationMS = 0
                };
                new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Blind,
                    DurationMS = 0
                };
                new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Quiet,
                    DurationMS = 0
                };
                new ConditionEffect
                {
                    Effect = ConditionEffectIndex.ArmorBroken,
                    DurationMS = 0
                };
                new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Bleeding,
                    DurationMS = 0
                };
                new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Dazed,
                    DurationMS = 0
                };
                new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Sick,
                    DurationMS = 0
                };
                new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Drunk,
                    DurationMS = 0
                };
                new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Hallucinating,
                    DurationMS = 0
                };
                new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Hexed,
                    DurationMS = 0
                };
                new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Unstable,
                    DurationMS = 0
                };
                new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Darkness,
                    DurationMS = 0
                };
                new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Curse,
                    DurationMS = 0
                };
                new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Nullified,
                    DurationMS = 0
                };
            }
        }

        private void HandleEffects()
        {
            Player player = null;
            if (_client == null || _client.Account == null) return;
            if (Manager.Core.getTickCount() % 50 == 0)
            {
                if (CheckAxe())
                {
                    Stats.Boost.ActivateBoost[0].Push(300, true);
                    Stats.ReCalculateValues();
                }
                else
                {
                    Stats.Boost.ActivateBoost[0].Pop(300, true);
                    Stats.ReCalculateValues();
                }

                if (CheckSunMoon())
                {
                    Stats.Boost.ActivateBoost[1].Push(100);
                    Stats.ReCalculateValues();
                }
                else
                {
                    Stats.Boost.ActivateBoost[1].Pop(100);
                    Stats.ReCalculateValues();
                }

                if (CheckAnubis())
                {
                    Stats.Boost.ActivateBoost[1].Push(60);
                    Stats.ReCalculateValues();
                }
                else
                {
                    Stats.Boost.ActivateBoost[1].Pop(60);
                    Stats.ReCalculateValues();
                }
                
                if (CheckSeal())
                {
                    var pkts = new List<Packet>();
                    Owner.Timers.Add(new WorldTimer(1000, (w) =>
                    {
                        if (w == null || w.Deleted) return;
                        Stats.Boost.ActivateBoost[0].Push(150, false);
                        Stats.Boost.ActivateBoost[2].Push(25, false);
                        Stats.Boost.ActivateBoost[3].Push(25, false);
                        Stats.ReCalculateValues();
                    }));

                    Owner.Timers.Add(new WorldTimer(5000, (w) =>
                    {
                        if (w == null || w.Deleted) return;
                        Stats.Boost.ActivateBoost[0].Pop(150, false);
                        Stats.Boost.ActivateBoost[2].Pop(25, false);
                        Stats.Boost.ActivateBoost[3].Pop(25, false);
                        Stats.ReCalculateValues();
                    }));
                }
                else
                {
                    Stats.Boost.ActivateBoost[0].Pop(150, false);
                    Stats.Boost.ActivateBoost[3].Pop(25, false);
                    Stats.Boost.ActivateBoost[6].Pop(25, false);
                    Stats.ReCalculateValues();
                }

                if (CheckMocking()) ApplyConditionEffect(ConditionEffectIndex.Relentless);
                else ApplyConditionEffect(ConditionEffectIndex.Relentless, 0);

                if (CheckCrescent()) ApplyConditionEffect(ConditionEffectIndex.SlowedImmune);
                else ApplyConditionEffect(ConditionEffectIndex.SlowedImmune, 0);

                if (CheckForce()) ApplyConditionEffect(ConditionEffectIndex.ArmorBreakImmune);
                else ApplyConditionEffect(ConditionEffectIndex.ArmorBreakImmune, 0);

                if (CheckRoyal()) ApplyConditionEffect(ConditionEffectIndex.HealthRecovery);
                else ApplyConditionEffect(ConditionEffectIndex.HealthRecovery, 0);

                if (CheckResistance()) ApplyConditionEffect(ConditionEffectIndex.SlowedImmune);
                else ApplyConditionEffect(ConditionEffectIndex.SlowedImmune, 0);
                if (CheckAegis()) ApplyConditionEffect(ConditionEffectIndex.Vengeance);
                else ApplyConditionEffect(ConditionEffectIndex.Vengeance, 0);

                if (CheckGilded()) ApplyConditionEffect(ConditionEffectIndex.Alliance);
                else ApplyConditionEffect(ConditionEffectIndex.Alliance, 0);
            }
            if (HasConditionEffect(ConditionEffects.Nullified)) ApplyConditionEffect(PositiveEffs);

            if (_client.Account.Hidden && !HasConditionEffect(ConditionEffects.Hidden))
            {
                ApplyConditionEffect(ConditionEffectIndex.Hidden);
                ApplyConditionEffect(ConditionEffectIndex.Invincible);
                ApplyConditionEffect(ConditionEffectIndex.Swiftness);

                Manager.Clients[Client].Hidden = true;
            }
            if (!_client.Account.Hidden && HasConditionEffect(ConditionEffects.Hidden))
            {
                ApplyConditionEffect(ConditionEffectIndex.Hidden, 0);
                ApplyConditionEffect(ConditionEffectIndex.Invincible, 0);
                ApplyConditionEffect(ConditionEffectIndex.Swiftness, 0);

                Manager.Clients[Client].Hidden = false;
            }
            if (Muted && !HasConditionEffect(ConditionEffects.Muted)) ApplyConditionEffect(ConditionEffectIndex.Muted);
            if (HasConditionEffect(ConditionEffects.Quiet) && MP > 0) MP = 0;
            if (HasConditionEffect(ConditionEffects.Bleeding) && HP > 1)
            {
                if (_bleeding > 1)
                {
                    HP -= (int)_bleeding;
                    if (HP < 1)
                        HP = 1;
                    _bleeding -= (int)_bleeding;
                }

                _bleeding += 28 * (CoreConstant.worldTickMs / 1000f);
            }
            
            if (HasConditionEffect(ConditionEffects.NinjaSpeedy))
            {
                MP = Math.Max(0, (int)(MP - 10 * CoreConstant.worldTickMs / 1000f));

                if (MP == 0)
                {
                    ApplyConditionEffect(ConditionEffectIndex.NinjaSpeedy, 0);
                    abilityOngoing = false;
                }
            }
            if (HasConditionEffect(ConditionEffects.SamuraiBerserk))
            {
                MP = Math.Max(0, (int)(MP - 10 * CoreConstant.worldTickMs / 1000f));

                if (MP == 0)
                {
                    ApplyConditionEffect(ConditionEffectIndex.SamuraiBerserk, 0);
                    abilityOngoing = false;
                }
            }
            if (HasConditionEffect(ConditionEffects.DrakzixCharging))
                Owner.Timers.Add(new WorldTimer(100, (w) =>
                {
                    if (w == null || w.Deleted || this == null) return;

                    HP -= DrakzixHPDrain / 10;
                    DrainedHP += 1;
                }));
            if (_newbieTime > 0)
            {
                _newbieTime -= (int)CoreConstant.worldTickMs;

                if (_newbieTime < 0) _newbieTime = 0;
            }
            if (_canTpCooldownTime > 0)
            {
                _canTpCooldownTime -= (int)CoreConstant.worldTickMs + 10;

                if (_canTpCooldownTime < 0) _canTpCooldownTime = 0;
            }
        }

        private void HandleProt()
        {
            if (AscensionEnabled)
                ProtectionMax = (int)Stats[0] / 5;
            else
                ProtectionMax = 0;
            if (Protection > ProtectionMax) Protection = ProtectionMax;
            if (HasConditionEffect(ConditionEffects.Surged) && _protDelay > 0) _protDelay = 0;
            if (_protDelay <= 0 && !(Protection >= ProtectionMax))
            {
                if (Protection < 0) Protection = 0;

                _protRegen += ProtectionMax / 10 * (CoreConstant.worldTickMs / 1000f);

                if (_protRegen > 1)
                {
                    Protection += (int)_protRegen;
                    if (Protection > ProtectionMax)
                        Protection = ProtectionMax;
                    _protRegen -= (int)_protRegen;
                }
            }
            if (_protDelay > 0)
            {
                _protDelay -= (CoreConstant.worldTickMs / 1000f);

                if (_protDelay < 0) _protDelay = 0;
            }
        }

        private bool CanHpRegen()
        {
            if (HasConditionEffect(ConditionEffects.Sick) ||
                        HasConditionEffect(ConditionEffects.Corrupted))
                return false;

            return true;
        }

        private bool CanMpRegen()
        {
            if (HasConditionEffect(ConditionEffects.Quiet) ||
                    HasConditionEffect(ConditionEffects.NinjaSpeedy) ||
                        HasConditionEffect(ConditionEffects.SamuraiBerserk) ||
                            HasConditionEffect(ConditionEffects.Corrupted))
                return false;

            return true;
        }

        internal void SetNewbiePeriod()
        {
            _newbieTime = 3000;
        }

        internal void SetTPDisabledPeriod()
        {
            _canTpCooldownTime = 8 * 1000; // 8 seconds
        }

        internal void SetProtRegenCooldown()
        {
            _protDelay = 10.0;
        }

        public bool IsVisibleToEnemy()
        {
            if (HasConditionEffect(ConditionEffects.Paused))
                return false;
            if (HasConditionEffect(ConditionEffects.Invisible))
                return false;
            if (HasConditionEffect(ConditionEffects.Hidden))
                return false;
            if (_newbieTime > 0)
                return false;
            return true;
        }

        public bool TPCooledDown()
        {
            return _canTpCooldownTime <= 0;
        }
    }
}
