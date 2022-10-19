using common;
using System.Collections.Generic;
using System.Xml.Linq;
using wServer.networking.packets.outgoing;
using wServer.realm.cores;

namespace wServer.realm.entities
{
    public class StaticObject : Entity
    {
        private const int startBlinkWithin = 20;
        private const int startFastBlinkWithin = 27;

        //Stats
        public bool Vulnerable { get; }

        public bool Static { get; }
        public bool Hittestable { get; }
        public bool Dying { get; }

        private readonly bool[] isSet = { false, false };
        private Timer timer;

        private readonly SV<int> _hp;

        public int HP
        {
            get => _hp.GetValue();
            set => _hp.SetValue(value);
        }

        public static int? GetHP(XElement elem)
        {
            var n = elem.Element("MaxHitPoints");
            if (n != null)
                return Utils.FromString(n.Value);
            return null;
        }

        public StaticObject(RealmManager manager, ushort objType, int? life, bool stat, bool dying, bool hittestable)
            : base(manager, objType)
        {
            _hp = new SV<int>(this, StatsType.HP, 0, dying);

            if (Vulnerable = life == 0 || life.HasValue) HP = life.Value;
            else if (!isSet[0] && this is Portal)
            {
                //todo: make this not shit
                var loops = 0;

                //Check for Vault, Realm, MarketPlace, Guildhall etc



                timer = new Timer(1000) { AutoReset = true };
                timer.Elapsed += (o, e) =>
                {
                    if (Owner != null)
                    {
                        if (loops == startBlinkWithin)
                            Owner.BroadcastPacketNearby(new ShowEffect()
                            {
                                EffectType = EffectType.Flashing,
                                Pos1 = new Position() { X = 0.5f, Y = 14 },
                                TargetObjectId = Id,
                                Color = new ARGB(0xA9A9A9)
                            }, this, null);
                        if (loops == startFastBlinkWithin)
                            Owner.BroadcastPacketNearby(new ShowEffect()
                            {
                                EffectType = EffectType.Flashing,
                                Pos1 = new Position() { X = 0.33f, Y = 9 },
                                TargetObjectId = Id,
                                Color = new ARGB(0xA9A9A9)
                            }, this, null);
                    }

                    if (loops < 30) loops++;
                    else timer.Dispose();
                };
                timer.Start();
                isSet[0] = true;
            }
            Dying = dying;
            Static = stat;
            Hittestable = hittestable;
        }

        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            stats[StatsType.HP] = (!Vulnerable) ? int.MaxValue : HP;
            base.ExportStats(stats);
        }

        protected override void ImportStats(StatsType stats, object val)
        {
            if (stats == StatsType.HP) HP = (int)val;
            base.ImportStats(stats, val);
        }

        public override bool HitByProjectile(Projectile projectile)
        {
            if (Vulnerable && projectile.ProjectileOwner is Player)
            {
                var def = ObjectDesc.Defense;

                if (projectile.ProjDesc.ArmorPiercing) def = 0;

                var dmg = (int)StatsManager.GetDefenseDamage(this, projectile.Damage, def);

                HP -= dmg;

                Owner.BroadcastPacketNearby(new Damage()
                {
                    TargetId = Id,
                    Effects = 0,
                    DamageAmount = (ushort)dmg,
                    Kill = !CheckHP(),
                    BulletId = projectile.ProjectileId,
                    ObjectId = projectile.ProjectileOwner.Self.Id
                }, this, (projectile.ProjectileOwner as Player));
            }

            return true;
        }

        protected bool CheckHP()
        {
            if (this is Container || this is Portal)
            {
                if (!isSet[0] && HP <= 10000)
                {
                    Owner.BroadcastPacketNearby(new ShowEffect()
                    {
                        EffectType = EffectType.Flashing,
                        Pos1 = new Position() { X = 0.5f, Y = 14 },
                        TargetObjectId = Id,
                        Color = new ARGB(0xA9A9A9)
                    }, this, null);
                    isSet[0] = true;
                }
                if (!isSet[1] && HP <= 3000)
                {
                    Owner.BroadcastPacketNearby(new ShowEffect()
                    {
                        EffectType = EffectType.Flashing,
                        Pos1 = new Position() { X = 0.33f, Y = 9 },
                        TargetObjectId = Id,
                        Color = new ARGB(0xA9A9A9)
                    }, this, null);
                    isSet[1] = true;
                }
            }

            if (HP <= 0)
            {
                var x = (int)(X - 0.5);
                var y = (int)(Y - 0.5);

                if (Owner.Map.Contains(new IntPoint(x, y)))
                    if (ObjectDesc != null && Owner.Map[x, y].ObjType == ObjectType)
                    {
                        var tile = Owner.Map[x, y];
                        tile.ObjType = 0;
                        tile.UpdateCount++;
                    }

                Owner.LeaveWorld(this);

                return false;
            }

            return true;
        }

        public override void Tick()
        {
            if (Vulnerable)
            {
                if (Dying) HP -= (int)CoreConstant.worldLogicTickMs;

                CheckHP();
            }

            base.Tick();
        }
    }
}
