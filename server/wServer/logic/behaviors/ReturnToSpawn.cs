using common.resources;
using Mono.Game;
using wServer.realm;
using wServer.realm.cores;
using wServer.realm.entities;

namespace wServer.logic.behaviors
{
    internal class ReturnToSpawn : CycleBehavior
    {
        private readonly float _speed;
        private readonly float _returnWithinRadius;

        public ReturnToSpawn(double speed, double returnWithinRadius = 1)
        {
            _speed = (float)speed;
            _returnWithinRadius = (float)returnWithinRadius;
        }

        protected override void TickCore(Entity host, ref object state)
        {
            if (!(host is Enemy)) return;

            if (host.HasConditionEffect(ConditionEffects.Paralyzed)) return;

            var spawn = (host as Enemy).SpawnPoint;
            var vect = new Vector2(spawn.X - host.X, spawn.Y - host.Y);

            if (vect.Length() > _returnWithinRadius && vect.Length() > 1)
            {
                Status = CycleStatus.InProgress;

                vect.Normalize();
                vect *= host.GetSpeed(_speed) * (CoreConstant.worldLogicTickMs / 1000f);
                host.ValidateAndMove(host.X + vect.X, host.Y + vect.Y);
            }
            else
            {
                Status = CycleStatus.Completed;

                host.ValidateAndMove(spawn.X, spawn.Y);
            }
        }
    }
}
