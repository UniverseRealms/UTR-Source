using common.resources;
using System;
using wServer.networking.packets.outgoing;
using wServer.realm;
using wServer.realm.cores;
using wServer.realm.entities;

namespace wServer.logic.behaviors
{
    internal class Grenade : Behavior
    {
        //State storage: cooldown timer

        private readonly double range;
        private float radius;
        private double? fixedAngle;
        private int count;
        private double angleOffset;
        private readonly int damage;
        private Cooldown coolDown;
        private readonly int _coolDownOffset;
        private readonly ConditionEffectIndex effect;
        private readonly int effectDuration;

        public uint Color { get; }

        public Grenade(double radius, int damage, double range = 5,
            double? fixedAngle = null, Cooldown coolDown = new Cooldown(),
            ConditionEffectIndex effect = 0, int coolDownOffset = 0, int effectDuration = 0,
            uint color = 0xffff0000, int count = 1, double angleOffset = 0)
        {
            this.radius = (float)radius;
            this.damage = damage;
            this.range = range;
            this.fixedAngle = fixedAngle * Math.PI / 180;
            this.coolDown = coolDown.Normalize();
            _coolDownOffset = coolDownOffset;
            this.count = count;
            this.angleOffset = angleOffset * Math.PI / 180;
            this.effect = effect;
            this.effectDuration = effectDuration;
            Color = color;
        }

        protected override void OnStateEntry(Entity host, ref object state)
        {
            state = _coolDownOffset;
        }

        protected override void TickCore(Entity host, ref object state)
        {
            int cool = (int)state - (int)CoreConstant.worldLogicTickMs;

            if (cool <= 0)
            {
                if (host.HasConditionEffect(ConditionEffects.Stunned))
                    return;

                var player = host.AttackTarget ?? host.GetNearestEntity(range, true);
                if (player != null || fixedAngle != null)
                {
                    double a = angleOffset;
                    if (fixedAngle != null)
                        a = fixedAngle.Value;
                    else if (angleOffset != 0)
                        a = Math.Atan2(player.Y - host.Y, player.X - host.X);

                    double r = (fixedAngle == null && angleOffset != 0) ? host.Dist(player) : range;

                    a -= angleOffset * (count - 1) / 2;
                    for (int i = 0; i < count; i++)
                    {
                        Position target;
                        if (fixedAngle != null || angleOffset != 0)
                        {
                            target = new Position()
                            {
                                X = (float)(r * Math.Cos(a)) + host.X,
                                Y = (float)(r * Math.Sin(a)) + host.Y,
                            };
                            a += angleOffset;
                        }
                        else
                            target = new Position()
                            {
                                X = player.X,
                                Y = player.Y,
                            };

                        host.Owner.BroadcastPacketNearby(new ShowEffect()
                        {
                            EffectType = EffectType.Throw,
                            Color = new ARGB(Color),
                            TargetObjectId = host.Id,
                            Pos1 = target
                        }, host, null);
                        host.Owner.Timers.Add(new WorldTimer(1500, (w) =>
                        {
                            if (w == null || w.Deleted || host == null) return;

                            w.BroadcastPacketNearby(new Aoe()
                            {
                                Pos = target,
                                Radius = radius,
                                Damage = (ushort)damage,
                                Duration = 0,
                                Effect = 0,
                                OrigType = host.ObjectType
                            }, host, null);

                            w.AOE(target, radius, true, p =>
                            {
                                (p as IPlayer).Damage(damage, host, false);

                                if (!(p as IPlayer).IsInvulnerable()) p.ApplyConditionEffect(effect, effectDuration);
                            });
                        }));
                    }
                }
                cool = coolDown.Next(Random);
            }

            state = cool;
        }
    }
}
