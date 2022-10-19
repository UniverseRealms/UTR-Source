using wServer.realm;
using wServer.realm.cores;

namespace wServer.logic.transitions
{
    internal class TimedTransition : Transition
    {
        //State storage: cooldown timer

        private int time;
        private bool randomized;

        public TimedTransition(int time, string targetState, bool randomized = false)
            : base(targetState)
        {
            this.time = time;
            this.randomized = randomized;
        }

        protected override bool TickCore(Entity host, ref object state)
        {
            int cool;
            if (state == null) cool = randomized ? Random.Next(this.time) : this.time;
            else cool = (int)state;

            bool ret = false;
            if (cool <= 0)
            {
                ret = true;
                cool = this.time;
            }
            else
                cool -= (int)CoreConstant.worldLogicTickMs;

            state = cool;
            return ret;
        }
    }
}
