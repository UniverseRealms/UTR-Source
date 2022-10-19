using common.resources;
using Mono.Game;
using wServer.realm;
using wServer.realm.cores;

namespace wServer.logic.behaviors
{
    internal class StayCloseToSpawn : CycleBehavior
    {
        //State storage: target position
        //assume spawn=state entry position

        private readonly float speed;
        private readonly int range;

        public StayCloseToSpawn(double speed, int range = 5)
        {
            this.speed = (float)speed;
            this.range = range;
        }

        protected override void OnStateEntry(Entity host, ref object state) => state = new Vector2(host.X, host.Y);

        protected override void TickCore(Entity host, ref object state)
        {
            Status = CycleStatus.NotStarted;

            if (host.HasConditionEffect(ConditionEffects.Paralyzed)) return;

            if (!(state is Vector2))
            {
                state = new Vector2(host.X, host.Y);

                Status = CycleStatus.Completed;
                return;
            }

            var vect = (Vector2)state;
            if ((vect - new Vector2(host.X, host.Y)).Length() > range)
            {
                vect -= new Vector2(host.X, host.Y);
                vect.Normalize();

                var dist = host.GetSpeed(speed) * (CoreConstant.worldLogicTickMs / 1000f);

                host.ValidateAndMove(host.X + vect.X * dist, host.Y + vect.Y * dist);

                Status = CycleStatus.InProgress;
            }
            else
                Status = CycleStatus.Completed;
        }
    }
}
