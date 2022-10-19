using common.resources;
using System.Collections.Generic;
using wServer.networking.packets;
using wServer.networking.packets.outgoing;
using wServer.realm.cores;

namespace wServer.realm.entities
{
    internal class SpecialTrap : StaticObject
    {
        private const int LIFETIME = 30;

        private Player player;
        private float radius;
        private int dmg;
        private ConditionEffectIndex effect;
        private int duration;
        private uint color;

        public SpecialTrap(Player player, float radius, int dmg, ConditionEffectIndex eff, float effDuration, uint color)
            : base(player.Manager, 0x0711, LIFETIME * 1000, true, true, false)
        {
            this.player = player;
            this.radius = radius;
            this.dmg = dmg;
            this.color = color;
            effect = eff;
            duration = (int)(effDuration * 1000);
        }

        private int t = 0;
        private int p = 0;
        private int q = 0;

        public override void Tick()
        {
            if (t / 500 == p)
            {
                Owner.BroadcastPacketNearby(new ShowEffect()
                {
                    EffectType = EffectType.Trap,
                    Color = new ARGB(0xFFFF0000),
                    TargetObjectId = Id,
                    Pos1 = new Position() { X = radius / 2 }
                }, this, null);

                Owner.BroadcastPacketNearby(new ShowEffect()
                {
                    EffectType = EffectType.Trap,
                    Color = new ARGB(0xFF0000FF),
                    TargetObjectId = Id,
                    Pos1 = new Position() { X = radius }
                }, this, null);

                p++;

                if (p == LIFETIME * 2)
                {
                    Explode();
                    return;
                }
            }

            if (t / 1000 == q)
            {
                this.AOE(radius / 2, true, player =>
                {
                    player.ApplyConditionEffect(new ConditionEffect
                    {
                        Effect = ConditionEffectIndex.Damaging,
                        DurationMS = 2000
                    });
                    player.ApplyConditionEffect(new ConditionEffect
                    {
                        Effect = ConditionEffectIndex.Bravery,
                        DurationMS = 2000
                    });
                    player.ApplyConditionEffect(new ConditionEffect
                    {
                        Effect = ConditionEffectIndex.Healing,
                        DurationMS = 2000
                    });
                });
                q++;
            }

            t += (int)CoreConstant.worldTickMs;

            bool monsterNearby = false;

            this.AOETrap(radius, false, enemy => monsterNearby = true);

            if (monsterNearby) Explode();

            base.Tick();
        }

        private void Explode()
        {
            Owner.BroadcastPacketNearby(new ShowEffect()
            {
                EffectType = EffectType.AreaBlast,
                Color = new ARGB(color),
                TargetObjectId = Id,
                Pos1 = new Position() { X = radius }
            }, this, null);

            this.AOE(radius, false, enemy =>
            {
                (enemy as Enemy).Damage(player, dmg, false, new ConditionEffect()
                {
                    Effect = effect,
                    DurationMS = duration
                });
            });

            Owner.LeaveWorld(this);
        }
    }
}
