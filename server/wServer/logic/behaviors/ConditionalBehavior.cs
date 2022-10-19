using common.resources;
using wServer.realm;

namespace wServer.logic.behaviors
{
    public class ConditionalBehavior : Behavior
    {
        private readonly ConditionEffects effect;
        private readonly Behavior behavior;

        public ConditionalBehavior(ConditionEffects effect, Behavior behavior)
        {
            this.effect = effect;
            this.behavior = behavior;
        }

        protected override void OnStateEntry(Entity host, ref object state)
        {
            behavior.OnStateEntry(host);
        }

        protected override void TickCore(Entity host, ref object state)
        {
            if (host.HasConditionEffect(effect)) behavior.Tick(host);
        }
    }
}
