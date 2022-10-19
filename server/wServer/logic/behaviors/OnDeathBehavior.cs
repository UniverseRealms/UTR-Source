using wServer.realm;
using System;

namespace wServer.logic.behaviors
{
    public class OnDeathBehavior : Behavior
    {
        private readonly Behavior behavior;

        public OnDeathBehavior(Behavior behavior)
        {
            this.behavior = behavior;
        }

        protected internal override void Resolve(State parent)
        {
            parent.Death += (s, e) => behavior.OnStateEntry(e.Host);
        }

        protected override void TickCore(Entity host, ref object state)
        {
        }
    }
}
