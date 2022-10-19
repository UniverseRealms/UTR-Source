using common.resources;
using System;
using System.Collections.Generic;
using wServer.logic;
using wServer.networking.packets.outgoing;
using wServer.realm.cores;
using wServer.realm.terrain;
using wServer.realm.worlds;

namespace wServer.realm.entities
{
    public class Ally : Character, IPlayer
    {
        private readonly bool _stat;

        public WmapTerrain Terrain { get; set; }

        private Position? pos;
        public Position SpawnPoint => pos ?? new Position { X = X, Y = Y };

        public Ally(RealmManager Manager, ushort objType, bool stat)
            : base(Manager, objType)
        {
            _stat = ObjectDesc.MaxHP == 0;
        }

        public override void Init(World owner)
        {
            base.Init(owner);
            if (ObjectDesc.StasisImmune)
                ApplyConditionEffect(ConditionEffectIndex.StasisImmune, -1);

            if (_stat)
                ApplyConditionEffect(ConditionEffectIndex.Invincible, -1);
        }

        public event EventHandler<BehaviorEventArgs> OnDeath;

        public void Death()
        {
            CurrentState?.OnDeath(new BehaviorEventArgs(this));
            OnDeath?.Invoke(this, new BehaviorEventArgs(this));
            Owner.LeaveWorld(this);
        }

        public void Damage(int dmg, Entity src, bool noDef)
        {
            if (_stat) return;
            if (HasConditionEffect(ConditionEffects.Invincible)) return;
            if (!HasConditionEffect(ConditionEffects.Paused) && !HasConditionEffect(ConditionEffects.Stasis))
            {
                var def = ObjectDesc.Defense;

                if (noDef) def = def / 4;

                dmg = (int)StatsManager.GetDefenseDamage(this, dmg, def);

                if (!HasConditionEffect(ConditionEffects.Invulnerable)) HP -= dmg;

                Owner.BroadcastPacketNearby(new Damage()
                {
                    TargetId = Id,
                    Effects = 0,
                    DamageAmount = (ushort)dmg,
                    Kill = HP < 0,
                    BulletId = 0,
                    ObjectId = src.Id
                }, this, null);

                if (HP < 0 && Owner != null) Death();

                return;
            }
            return;
        }

        public override bool HitByProjectile(Projectile projectile)
        {
            if (_stat) return false;
            if (HasConditionEffect(ConditionEffects.Invincible)) return false;
            if (projectile.ProjectileOwner is Enemy e && !HasConditionEffect(ConditionEffects.Paused) && !HasConditionEffect(ConditionEffects.Stasis))
            {
                var def = ObjectDesc.Defense;

                if (projectile.ProjDesc.ArmorPiercing) def = def / 4;

                var dmg = (int)StatsManager.GetDefenseDamage(this, projectile.Damage, def);

                if (!HasConditionEffect(ConditionEffects.Invulnerable)) HP -= dmg;

                ConditionEffect[] effs = null;

                foreach (var pair in projectile.ProjDesc.CondChance)
                {
                    if (pair.Value == 0 || pair.Key == default(ConditionEffect)) continue;
                    if (pair.Value / 100d > new Random().NextDouble())
                    {
                        var effList = new List<ConditionEffect>(projectile.ProjDesc.Effects) { pair.Key };
                        effs = effList.ToArray();
                    }
                }

                ApplyConditionEffect(effs ?? projectile.ProjDesc.Effects);
                Owner.BroadcastPacketNearby(new Damage
                {
                    TargetId = Id,
                    Effects = projectile.ConditionEffects,
                    DamageAmount = (ushort)dmg,
                    Kill = HP < 0,
                    BulletId = projectile.ProjectileId,
                    ObjectId = projectile.ProjectileOwner.Self.Id
                }, this);

                if (HP < 0 && Owner != null) Death();

                return true;
            }
            return false;
        }

        public override void Tick()
        {
            if (pos == null)
                pos = new Position() { X = X, Y = Y };

            if (!_stat && HasConditionEffect(ConditionEffects.Bleeding)) HP -= (int)(MaximumHP / 500f * CoreConstant.worldLogicTickMs / 1000f);
            if (HP < 0 && Owner != null) Death();

            base.Tick();
        }

        public bool IsInvulnerable() =>
            HasConditionEffect(ConditionEffects.Paused) ||
            HasConditionEffect(ConditionEffects.Stasis) ||
            HasConditionEffect(ConditionEffects.Invincible) ||
            HasConditionEffect(ConditionEffects.Invulnerable);

        public bool IsVisibleToEnemy() => !HasConditionEffect(ConditionEffects.Invisible);
    }
}
