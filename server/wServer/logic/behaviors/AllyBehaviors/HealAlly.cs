using common.resources;
using wServer.networking.packets.outgoing;
using wServer.realm;
using wServer.realm.cores;
using wServer.realm.entities;

namespace wServer.logic.behaviors
{
    internal class HealAlly : Behavior
    {
        //State storage: cooldown timer

        private readonly double _range;
        private readonly ushort _type;
        private Cooldown _coolDown;
        private readonly int? _amount;

        public HealAlly(double range, string name = null, int? healAmount = null, Cooldown coolDown = new Cooldown())
        {
            _range = (float)range;
            _type = (name == null) ? (ushort)0 : GetObjType(name);
            _coolDown = coolDown.Normalize();
            _amount = healAmount;
        }

        protected override void OnStateEntry(Entity host, ref object state)
        {
            state = 0;
        }

        protected override void TickCore(Entity host, ref object state)
        {
            var cool = (int)state;

            int? increasedHP = _amount;

            if (cool <= 0)
            {
                if (host.HasConditionEffect(ConditionEffects.Stunned))
                    return;

                if (host.HasConditionEffect(ConditionEffects.Sick))
                {
                    increasedHP = _amount / 2;
                }
                
                foreach (var en in host.GetNearestAllies(_range, _type, true))
                {
                    var entity = en as Ally;

                    int newHp = entity.MaximumHP;
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
                        entity.Owner.BroadcastPacketNearby(new ShowEffect()
                        {
                            EffectType = EffectType.Trail,
                            TargetObjectId = host.Id,
                            Pos1 = new Position() { X = entity.X, Y = entity.Y },
                            Color = new ARGB(0xffffffff)
                        }, host, null);
                        entity.Owner.BroadcastPacketNearby(new Notification()
                        {
                            ObjectId = entity.Id,
                            Message = "+" + n,
                            Color = new ARGB(0xff00ff00)
                        }, entity, null);
                    }
                }
                cool = _coolDown.Next(Random);
            }
            else
                cool -= (int)CoreConstant.worldLogicTickMs;

            state = cool;
        }
    }
}