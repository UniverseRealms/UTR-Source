#region

using common.resources;
using System.Collections.Generic;
using wServer.networking.packets;
using wServer.networking.packets.outgoing;
using wServer.realm.cores;

#endregion

namespace wServer.realm.entities
{
    internal class Banner : StaticObject
    {
        private readonly float radius;
        private int lifetime;
        private readonly int duration;
        private int p;
        private int p2;
        private Player player;
        private int t;
        private uint color;

        public Banner(Player player, float radius, int lifetime, int duration, uint color)
            : base(player.Manager, 0x0711, lifetime * 1000, true, true, false)
        {
            this.player = player;
            this.radius = radius;
            this.lifetime = lifetime;
            this.duration = duration;
            this.color = color;
        }

        public override void Tick()
        {
            if (t / 500 == p2)
            {
                Owner.BroadcastPacket(new ShowEffect()
                {
                    EffectType = EffectType.Trap,
                    Color = new ARGB(color),
                    TargetObjectId = Id,
                    Pos1 = new Position { X = radius }
                }, null);
                p2++;
                //Stuff
            }
            if (t / 1000 == p)
            {
                this.AOE(radius, true, player =>
                {
                    player.ApplyConditionEffect(new ConditionEffect
                    {
                        Effect = ConditionEffectIndex.Empowered,
                        DurationMS = duration
                    });
                    player.ApplyConditionEffect(new ConditionEffect
                    {
                        Effect = ConditionEffectIndex.Healing,
                        DurationMS = duration
                    });
                });
                p++;
            }

            t += (int)CoreConstant.worldLogicTickMs;

            base.Tick();
        }
    }
}
