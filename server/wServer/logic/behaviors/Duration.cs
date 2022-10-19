using wServer.realm;
using wServer.realm.cores;

namespace wServer.logic.behaviors
{
    internal class Duration : Behavior
    {
        private Behavior child;
        private int duration;

        public Duration(Behavior child, int duration)
        {
            this.child = child;
            this.duration = duration;
        }

        protected override void OnStateEntry(Entity host, ref object state)
        {
            child.OnStateEntry(host);
            state = 0;
        }

        protected override void TickCore(Entity host, ref object state)
        {
            int timeElapsed = (int)state;

            if (timeElapsed <= duration)
            {
                child.Tick(host);
                timeElapsed += (int)CoreConstant.worldLogicTickMs;
            }

            state = timeElapsed;
        }
    }
}
