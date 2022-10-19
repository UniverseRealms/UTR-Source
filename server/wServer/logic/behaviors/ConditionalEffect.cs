using common.resources;
using wServer.realm;

namespace wServer.logic.behaviors
{
    internal class ConditionalEffect : Behavior
    {
        //State storage: none

        private ConditionEffectIndex effect;
        private bool perm;
        private int duration;

        public ConditionalEffect(ConditionEffectIndex effect, bool perm = false, int duration = -1)
        {
            this.effect = effect;
            this.perm = perm;
            this.duration = duration;
        }

        protected override void OnStateEntry(Entity host, ref object state)
        {
            host.ApplyConditionEffect(new ConditionEffect()
            {
                Effect = effect,
                DurationMS = duration
            });
        }

        protected override void OnStateExit(Entity host, ref object state)
        {
            if (!perm)
            {
                host.ApplyConditionEffect(new ConditionEffect()
                {
                    Effect = effect,
                    DurationMS = 0
                });
            }
        }

        protected override void TickCore(Entity host, ref object state)
        { }
    }
}
