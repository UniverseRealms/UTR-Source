using common.resources;
using wServer.realm;

namespace wServer.logic.behaviors
{
    internal class RemoveConditionalEffect : Behavior
    {
        //State storage: none

        private readonly ConditionEffectIndex _effect;

        public RemoveConditionalEffect(ConditionEffectIndex effect)
        {
            _effect = effect;
        }

        protected override void OnStateEntry(Entity host, ref object state)
        {
            host.ApplyConditionEffect(new ConditionEffect()
            {
                Effect = _effect,
                DurationMS = 0
            });
        }

        protected override void TickCore(Entity host, ref object state)
        {
        }
    }
}
