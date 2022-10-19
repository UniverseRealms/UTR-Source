using common.resources;
using wServer.networking.packets.outgoing;
using wServer.realm.cores;

namespace wServer.realm.entities
{
    internal class BuffTrap : StaticObject
    {
        private const int LIFETIME = 20;

        private Player player;
        private float radius;
        private int dmg;
        private ConditionEffectIndex effect;
        private int duration;
        private uint color;

        public BuffTrap(Player player, float radius, int dmg, ConditionEffectIndex eff, float effDuration, uint color)
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

        public override void Tick()
        {
            if (t / 500 == p)
            {
                Owner.BroadcastPacketNearby(new ShowEffect()
                {
                    EffectType = EffectType.Trap,
                    Color = new ARGB(color),
                    TargetObjectId = Id,
                    Pos1 = new Position() { X = radius / 2 }
                }, this, null);

                p++;

                if (p == LIFETIME * 2)
                {
                    Explode();
                    return;
                }
            }

            t += (int)CoreConstant.worldTickMs;

            bool monsterNearby = false;

            this.AOETrap(radius / 2, false, enemy => monsterNearby = true);

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
            this.AOE(radius, true, player =>
            {
                player.ApplyConditionEffect(new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Berserk,
                    DurationMS = duration
                });
                player.ApplyConditionEffect(new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Damaging,
                    DurationMS = duration
                });
            });

            Owner.LeaveWorld(this);
        }
    }
}
