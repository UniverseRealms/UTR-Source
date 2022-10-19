using common.resources;
using wServer.networking.packets.outgoing;
using wServer.realm;
using wServer.realm.cores;
using wServer.realm.entities;

namespace wServer.logic.behaviors
{
    internal class TalismanAttack : Behavior
    {
        private readonly int _damage;
        private readonly int _duration;
        private ConditionEffectIndex _effect;

        public TalismanAttack(int damage, ConditionEffectIndex effect, int duration = 0)
        {
            _damage = damage;
            _duration = duration;
            _effect = effect;
        }

        protected override void OnStateEntry(Entity host, ref object state)
        {
            state = 0;
        }

        protected override void TickCore(Entity host, ref object state)
        {
            int cool = (int)(state ?? 600);
            if (cool <= 0)
            {
                var en = (Enemy)host.GetNearestEntity(6, false, (e => e is Enemy));

                if (en != null && en.ObjectDesc.Enemy)
                {
                    en.Owner.BroadcastPacket(new ShowEffect()
                    {
                        EffectType = EffectType.AreaBlast,
                        Color = new ARGB(0x3E3A78),
                        TargetObjectId = en.Id,
                        Pos1 = new Position { X = 1, }
                    }, null);

                    en.Owner.BroadcastPacket(new ShowEffect()
                    {
                        EffectType = EffectType.Trail,
                        TargetObjectId = host.Id,
                        Pos1 = new Position { X = en.X, Y = en.Y },
                        Color = new ARGB(0x3E3A78)
                    }, null);

                    en.Damage(host.GetPlayerOwner(), _damage, true);

                    en.ApplyConditionEffect(new ConditionEffect
                    {
                        Effect = _effect,
                        DurationMS = _duration
                    });
                }
                cool = 600;
            }
            else
                cool -= (int)CoreConstant.worldLogicTickMs;

            state = cool;
        }
    }
}
