using common.resources;
using wServer.realm;
using wServer.realm.cores;

namespace wServer.logic.behaviors
{
    internal class BackAndForth : CycleBehavior
    {
        //State storage: remaining distance

        private float speed;
        private int distance;

        public BackAndForth(double speed, int distance = 5)
        {
            this.speed = (float)speed;
            this.distance = distance;
        }

        protected override void TickCore(Entity host, ref object state)
        {
            float dist;
            if (state == null) dist = distance;
            else dist = (float)state;

            Status = CycleStatus.NotStarted;

            if (host.HasConditionEffect(ConditionEffects.Paralyzed))
                return;

            float moveDist = host.GetSpeed(speed) * (CoreConstant.worldLogicTickMs / 1000f);
            if (dist > 0)
            {
                Status = CycleStatus.InProgress;
                host.ValidateAndMove(host.X + moveDist, host.Y);
                dist -= moveDist;
                if (dist <= 0)
                {
                    dist = -distance;
                    Status = CycleStatus.Completed;
                }
            }
            else
            {
                Status = CycleStatus.InProgress;
                host.ValidateAndMove(host.X - moveDist, host.Y);
                dist += moveDist;
                if (dist >= 0)
                {
                    dist = distance;
                    Status = CycleStatus.Completed;
                }
            }

            state = dist;
        }
    }
}
