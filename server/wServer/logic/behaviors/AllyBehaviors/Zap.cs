using common.resources;
using wServer.networking.packets.outgoing;
using wServer.realm;
using wServer.realm.cores;
using wServer.realm.entities;

namespace wServer.logic.behaviors
{
    internal class Zap : Behavior
    {
        //State storage: none
        public readonly ConditionEffectIndex _effect;
        private uint _color;
        private float _flashPeriod;
        private int _damage;
        private int _coolDown;
        private bool _targetPlayer;
        private float _radius;
        private bool _ignoreArmor = false;
        private int _effDuration;
        public Zap(int damage, int cooldown, float radius, bool ignoreArmor = false, bool targetPlayer = false, uint color = 0xff9000ff, ConditionEffectIndex effect = ConditionEffectIndex.Dead, int effectDuration = 2000)
        {
            this._color = color;
            this._damage = damage;
            this._coolDown = cooldown;
            this._targetPlayer = targetPlayer;
            this._radius = radius;
            this._effect = effect;
            this._ignoreArmor = ignoreArmor;
            this._effDuration = effectDuration;
        }

        protected override void TickCore(Entity host, ref object state)
        {
            int cool = (int)(state ?? _coolDown);
            if (cool <= 0)
            {
                if (_targetPlayer)
                {
                    System.Console.WriteLine("<ZAP BEHAVIOUR> Targeting players not implemented!");
                    //not implemented
                }
                else
                {
                    var target = host.GetNearestEntity(_radius, false);
                    if (target != null)
                    {
                        if (target is Enemy e)
                        {
                            e.Damage((Player)host.GetPlayerOwner(), _damage, _ignoreArmor, new ConditionEffect()
                            {
                                Effect = _effect,
                                DurationMS = 3000
                            });
                        }
                    }
                }
                cool = _coolDown;
            }
            else
                cool -= (int)CoreConstant.worldLogicTickMs;
            state = cool;
        }


        protected override void OnStateEntry(Entity host, ref object state)
        {
            state = 0;
        }
    }
}
