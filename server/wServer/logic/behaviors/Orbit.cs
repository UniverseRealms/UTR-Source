using common.resources;
using Mono.Game;
using System;
using wServer.realm;
using wServer.realm.cores;

namespace wServer.logic.behaviors
{
    internal class Orbit : CycleBehavior
    {
        //State storage: orbit state
        private class OrbitState
        {
            public float Speed;
            public float Radius;
            public int Direction;
        }

        private float speed;
        private float acquireRange;
        private float radius;
        private ushort? target;
        private float speedVariance;
        private float radiusVariance;
        private bool? orbitClockwise;
        private bool seeInvis;

        public Orbit(double speed, double radius, double acquireRange = 10,
            string target = null, double? speedVariance = null, double? radiusVariance = null,
            bool? orbitClockwise = false, bool seeInvis = false)
        {
            this.speed = (float)speed;
            this.radius = (float)radius;
            this.acquireRange = (float)acquireRange;
            this.target = target == null ? null : (ushort?)GetObjType(target);
            this.speedVariance = (float)(speedVariance ?? speed * 0.1);
            this.radiusVariance = (float)(radiusVariance ?? speed * 0.1);
            this.orbitClockwise = orbitClockwise;
            this.seeInvis = seeInvis;
        }

        protected override void OnStateEntry(Entity host, ref object state)
        {
            int orbitDir;
            if (orbitClockwise == null)
                orbitDir = (Random.Next(1, 3) == 1) ? 1 : -1;
            else
                orbitDir = ((bool)orbitClockwise) ? 1 : -1;

            state = new OrbitState()
            {
                Speed = speed + speedVariance * (float)(Random.NextDouble() * 2 - 1),
                Radius = radius + radiusVariance * (float)(Random.NextDouble() * 2 - 1),
                Direction = orbitDir
            };
        }

        protected override void TickCore(Entity host, ref object state)
        {
            OrbitState s = (OrbitState)state;

            Status = CycleStatus.NotStarted;

            if (host.HasConditionEffect(ConditionEffects.Paralyzed))
                return;

            var entity = host.AttackTarget ?? host.GetNearestEntity(acquireRange, target, seeInvis);

            if (entity != null)
            {
                double angle;
                if (host.Y == entity.Y && host.X == entity.X)//small offset
                    angle = Math.Atan2(host.Y - entity.Y + (Random.NextDouble() * 2 - 1), host.X - entity.X + (Random.NextDouble() * 2 - 1));
                else
                    angle = Math.Atan2(host.Y - entity.Y, host.X - entity.X);
                var angularSpd = s.Direction * host.GetSpeed(s.Speed) / s.Radius;
                angle += angularSpd * (CoreConstant.worldLogicTickMs / 1000f);

                double x = entity.X + Math.Cos(angle) * s.Radius;
                double y = entity.Y + Math.Sin(angle) * s.Radius;
                Vector2 vect = new Vector2((float)x, (float)y) - new Vector2(host.X, host.Y);
                vect.Normalize();
                vect *= host.GetSpeed(s.Speed) * (CoreConstant.worldLogicTickMs / 1000f);

                host.ValidateAndMove(host.X + vect.X, host.Y + vect.Y);

                Status = CycleStatus.InProgress;
            }

            state = s;
        }
    }
}
