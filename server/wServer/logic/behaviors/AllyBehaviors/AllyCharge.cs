using common.resources;
using Mono.Game;
using wServer.realm;
using wServer.realm.cores;
using wServer.realm.entities;

namespace wServer.logic.behaviors
{
    internal class AllyCharge : CycleBehavior
    {
        //State storage: charge state
        private class AllyChargeState
        {
            public Vector2 Direction;
            public int RemainingTime;
        }

        private readonly float _speed;
        private readonly float _range;
        private Cooldown _coolDown;

        public AllyCharge(double speed = 4, float range = 10, Cooldown coolDown = new Cooldown())
        {
            _speed = (float)speed;
            _range = range;
            _coolDown = coolDown.Normalize(2000);
        }

        protected override void TickCore(Entity host, ref object state)
        {
            var s = (state == null) ? new AllyChargeState() : (AllyChargeState)state;

            Status = CycleStatus.NotStarted;

            if (host.HasConditionEffect(ConditionEffects.Paralyzed)) return;
            if (s.RemainingTime <= 0)
            {
                if (s.Direction == Vector2.Zero)
                {
                    var en = host.GetNearestEntity(_range, false, e => e is Enemy);
                    if (en != null && en.X != host.X && en.Y != host.Y)
                    {
                        s.Direction = new Vector2(en.X - host.X, en.Y - host.Y);
                        var d = s.Direction.Length();
                        s.Direction.Normalize();
                        s.RemainingTime = (int)(d / host.GetSpeed(_speed) * 1000);
                        Status = CycleStatus.InProgress;
                    }
                }
                else
                {
                    s.Direction = Vector2.Zero;
                    s.RemainingTime = _coolDown.Next(Random);
                    Status = CycleStatus.Completed;
                }
            }

            if (s.Direction != Vector2.Zero)
            {
                float dist = host.GetSpeed(_speed) * (CoreConstant.worldLogicTickMs / 1000f);
                host.ValidateAndMove(host.X + s.Direction.X * dist, host.Y + s.Direction.Y * dist);
                Status = CycleStatus.InProgress;
            }

            s.RemainingTime -= (int)CoreConstant.worldLogicTickMs;

            state = s;
        }
    }
}
