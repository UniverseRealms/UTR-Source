using wServer.realm;
using wServer.realm.cores;

namespace wServer.logic.behaviors
{
    //replacement for simple timed transition in sequence
    internal class Timed : CycleBehavior
    {
        //State storage: time

        private Behavior[] behaviors;
        private int period;

        public Timed(int period, params Behavior[] behaviors)
        {
            this.behaviors = behaviors;
            this.period = period;
        }

        protected override void OnStateEntry(Entity host, ref object state)
        {
            foreach (var behavior in behaviors)
                behavior.OnStateEntry(host);
            state = period;
        }

        protected override void TickCore(Entity host, ref object state)
        {
            int period = (int)state;

            foreach (Behavior behavior in behaviors)
            {
                behavior.Tick(host);
                Status = CycleStatus.InProgress;

                period -= (int)CoreConstant.worldLogicTickMs;
                if (period <= 0)
                {
                    period = this.period;
                    Status = CycleStatus.Completed;

                    if (behavior is Prioritize)
                        host.StateStorage[behavior] = -1;
                }
            }
            state = period;
        }
    }
}
