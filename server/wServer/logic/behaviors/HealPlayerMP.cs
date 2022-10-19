using common.resources;
using System;
using System.Linq;
using wServer.networking.packets.outgoing;
using wServer.realm;
using wServer.realm.cores;
using Player = wServer.realm.entities.Player;

namespace wServer.logic.behaviors
{
    internal class HealPlayerMP : Behavior
    {
        private double _range;
        private Cooldown _coolDown;
        private int _healAmount;

        public HealPlayerMP(double range, Cooldown coolDown = new Cooldown(), int healAmount = 100)
        {
            _range = range;
            _coolDown = coolDown.Normalize();
            _healAmount = healAmount;
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
                foreach (var entity in host.GetNearestEntities(_range, null, true).OfType<Player>())
                {
                    if ((host.AttackTarget != null && host.AttackTarget != entity) || entity.HasConditionEffect(ConditionEffects.Quiet))
                        continue;
                    int maxMp = entity.Stats[1];
                    int newMp = Math.Min(entity.MP + _healAmount, maxMp);

                    if (newMp != entity.MP)
                    {
                        int n = newMp - entity.MP;
                        entity.MP = newMp;
                        entity.Owner.BroadcastPacketNearby(new ShowEffect()
                        {
                            EffectType = EffectType.Potion,
                            TargetObjectId = entity.Id,
                            Color = new ARGB(0x6084e0)
                        }, entity, null);
                        entity.Owner.BroadcastPacketNearby(new ShowEffect()
                        {
                            EffectType = EffectType.Trail,
                            TargetObjectId = host.Id,
                            Pos1 = new Position { X = entity.X, Y = entity.Y },
                            Color = new ARGB(0xFFFFFFFF)
                        }, host, null);
                        entity.Owner.BroadcastPacketNearby(new Notification()
                        {
                            ObjectId = entity.Id,
                            Message = "+" + n,
                            Color = new ARGB(0x6084e0)
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
