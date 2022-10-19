using Mono.Game;
using System;
using wServer.realm;
using wServer.realm.cores;
using wServer.realm.entities;

namespace wServer.logic.behaviors
{
    internal class DecoyMove : CycleBehavior
    {
        private class DecoyMoveState
        {
            public Vector2 Direction;
            public int RemainingTime;
        }

        private static Random rand = new Random();

        private Vector2 _direction;
        private int _duration;
        private float _speed;

        public DecoyMove(int duration, float speed = 4, int? direction = null)
        {
            _duration = duration;
            _speed = speed;

            if (direction != null)
            {
                double angle = (double)(direction / 180.0) * Math.PI;
                _direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            }
        }

        protected override void OnStateEntry(Entity host, ref object state)
        {
            Player player = host.GetPlayerOwner();
            if (player != null)
            {
                if (_direction != null)
                {
                    var history = player.TryGetHistory(1);
                    if (history == null)
                        _direction = GetRandDirection();
                    else
                    {
                        _direction = new Vector2(player.X - history.Value.X, player.Y - history.Value.Y);
                        if (_direction.LengthSquared() == 0)
                            _direction = GetRandDirection();
                        else
                            _direction.Normalize();
                    }
                }
            }
            else
            {
                _direction = GetRandDirection();
            }

            var s = new DecoyMoveState()
            {
                Direction = _direction,
                RemainingTime = _duration
            };
            state = s;
        }

        private Vector2 GetRandDirection()
        {
            double angle = rand.NextDouble() * 2 * Math.PI;
            return new Vector2(
                (float)Math.Cos(angle),
                (float)Math.Sin(angle)
            );
        }

        protected override void TickCore(Entity host, ref object state)
        {
            DecoyMoveState s;
            if (state != null)
                s = (DecoyMoveState)state;
            else
                s = new DecoyMoveState()
                {
                    Direction = GetRandDirection(),
                    RemainingTime = _duration
                };

            Status = CycleStatus.InProgress;

            if (s.RemainingTime > 0)
            {
                host.ValidateAndMove(
                    host.X + s.Direction.X * _speed * CoreConstant.worldLogicTickMs / 1000,
                    host.Y + s.Direction.Y * _speed * CoreConstant.worldLogicTickMs / 1000
                    );
            }
            else
            {
                Status = CycleStatus.Completed;
            }
            s.RemainingTime -= (int)CoreConstant.worldLogicTickMs;
            state = s;
        }
    }
}
