using common.resources;
using wServer.networking.packets.outgoing;
using wServer.realm;
using wServer.realm.cores;
using wServer.realm.entities;

namespace wServer.logic.behaviors
{
    internal class HealSelf : Behavior
    {
        //State storage: cooldown timer

        private Cooldown _coolDown;
        private readonly int? _amount;

        public HealSelf(Cooldown coolDown = new Cooldown(), int? amount = null)
        {
            _coolDown = coolDown.Normalize();
            _amount = amount;
        }

        protected override void OnStateEntry(Entity host, ref object state)
        {
            state = 0;
        }

        protected override void TickCore(Entity host, ref object state)
        {
            var cool = (int)state;

            if (cool <= 0)
            {
                if (host.HasConditionEffect(ConditionEffects.Stunned))
                    return;

                int? increasedHP = _amount;

                if (host.HasConditionEffect(ConditionEffects.Sick))
                {
                    increasedHP = _amount / 2;
                }

                var entity = host as Character;

                if (entity == null)
                    return;

                int newHp = entity.ObjectDesc.MaxHP;
                if (increasedHP != null)
                {
                    var newHealth = (int)increasedHP + entity.HP;
                    if (newHp > newHealth)
                        newHp = newHealth;
                }
                if (newHp != entity.HP)
                {
                    int n = newHp - entity.HP;
                    entity.HP = newHp;
                    entity.Owner.BroadcastPacketNearby(new ShowEffect()
                    {
                        EffectType = EffectType.Potion,
                        TargetObjectId = entity.Id,
                        Color = new ARGB(0xffffffff)
                    }, entity, null);
                    entity.Owner.BroadcastPacketNearby(new Notification()
                    {
                        ObjectId = entity.Id,
                        Message = "+" + n,
                        Color = new ARGB(0xff00ff00)
                    }, entity, null);
                }

                cool = _coolDown.Next(Random);
            }
            else
                cool -= (int)CoreConstant.worldLogicTickMs;

            state = cool;
        }
    }
}
