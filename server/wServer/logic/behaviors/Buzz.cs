using common.resources;
using Mono.Game;
using wServer.realm;
using wServer.realm.cores;

namespace wServer.logic.behaviors
{
    internal class Buzz : CycleBehavior
    {
        //State storage: direction & remain
        private class BuzzStorage
        {
            public Vector2 Direction;
            public float RemainingDistance;
            public int RemainingTime;
        }

        private float speed;
        private float dist;
        private Cooldown coolDown;

        public Buzz(double speed = 2, double dist = 0.5, Cooldown coolDown = new Cooldown())
        {
            this.speed = (float)speed;
            this.dist = (float)dist;
            this.coolDown = coolDown.Normalize(1);
        }

        protected override void OnStateEntry(Entity host, ref object state)
        {
            state = new BuzzStorage();
        }

        protected override void TickCore(Entity host, ref object state)
        {
            BuzzStorage storage = (BuzzStorage)state;

            Status = CycleStatus.NotStarted;

            if (host.HasConditionEffect(ConditionEffects.Paralyzed))
                return;

            if (storage.RemainingTime > 0)
            {
                storage.RemainingTime -= (int)CoreConstant.worldLogicTickMs;
                Status = CycleStatus.NotStarted;
            }
            else
            {
                Status = CycleStatus.InProgress;
                if (storage.RemainingDistance <= 0)
                {
                    do
                    {
                        storage.Direction = new Vector2(Random.Next(-1, 2), Random.Next(-1, 2));
                    } while (storage.Direction.X == 0 && storage.Direction.Y == 0);
                    storage.Direction.Normalize();
                    storage.RemainingDistance = this.dist;
                    Status = CycleStatus.Completed;
                }
                float dist = host.GetSpeed(speed) * (CoreConstant.worldLogicTickMs / 1000f);
                host.ValidateAndMove(host.X + storage.Direction.X * dist, host.Y + storage.Direction.Y * dist);

                storage.RemainingDistance -= dist;
            }

            state = storage;
        }
    }
}
