using common.resources;
using Mono.Game;
using wServer.realm;
using wServer.realm.cores;

namespace wServer.logic.behaviors
{
    internal class StayBack : CycleBehavior
    {
        //State storage: cooldown timer

        private float speed;
        private float distance;
        private string entity;

        public StayBack(double speed, double distance = 8, string entity = null)
        {
            this.speed = (float)speed;
            this.distance = (float)distance;
            this.entity = entity;
        }

        protected override void TickCore(Entity host, ref object state)
        {
            int cooldown;
            if (state == null) cooldown = 1000;
            else cooldown = (int)state;

            Status = CycleStatus.NotStarted;

            if (host.HasConditionEffect(ConditionEffects.Paralyzed))
                return;

            Entity e = entity != null ?
                host.GetNearestEntityByName(distance, entity) :
                host.GetNearestEntity(distance, null);

            if (e != null)
            {
                Vector2 vect;
                vect = new Vector2(e.X - host.X, e.Y - host.Y);
                vect.Normalize();
                float dist = host.GetSpeed(speed) * (CoreConstant.worldLogicTickMs / 1000f);
                host.ValidateAndMove(host.X + (-vect.X) * dist, host.Y + (-vect.Y) * dist);

                if (cooldown <= 0)
                {
                    Status = CycleStatus.Completed;
                    cooldown = 1000;
                }
                else
                {
                    Status = CycleStatus.InProgress;
                    cooldown -= (int)CoreConstant.worldLogicTickMs;
                }
            }

            state = cooldown;
        }
    }
}
