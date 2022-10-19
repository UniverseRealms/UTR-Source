using common.resources;
using System.Linq;
using wServer.networking.packets.outgoing;
using wServer.realm;
using wServer.realm.cores;
using wServer.realm.entities;

namespace wServer.logic.behaviors
{
    internal class HealGroup : Behavior
    {
        //State storage: cooldown timer

        private double range;
        private string group;
        private Cooldown coolDown;
        private int? amount;

        public HealGroup(double range, string group, Cooldown coolDown = new Cooldown(), int? healAmount = null)
        {
            this.range = (float)range;
            this.group = group;
            this.coolDown = coolDown.Normalize();
            amount = healAmount;
        }

        protected override void OnStateEntry(Entity host, ref object state)
        {
            state = 0;
        }

        protected override void TickCore(Entity host, ref object state)
        {
            int cool = (int)state;

            if (cool <= 0)
            {
                if (host.HasConditionEffect(ConditionEffects.Stunned))
                    return;

                int? increasedHP = amount;

                if (host.HasConditionEffect(ConditionEffects.Sick))
                {
                    increasedHP = amount / 2;
                }

                foreach (var entity in host.GetNearestEntitiesByGroup(range, group).OfType<Enemy>())
                {
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
                cool = coolDown.Next(Random);
            }
            else
                cool -= (int)CoreConstant.worldLogicTickMs;

            state = cool;
        }
    }
}
