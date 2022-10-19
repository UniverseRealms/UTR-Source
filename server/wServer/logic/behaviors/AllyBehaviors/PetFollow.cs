using common.resources;
using Mono.Game;
using System;
using wServer.realm;
using wServer.realm.cores;

namespace wServer.logic.behaviors
{
    internal class PetFollow : CycleBehavior
    {
        //State storage: follow state
        private class PetFollowState
        {
            public F State;
            public int RemainingTime;
        }

        private enum F
        {
            ReturnToParent,
            FollowParent,
            Calculate,
        }

        private readonly float speed;
        private readonly float range;
        private int coolDown = 500;
        public PetFollow(double speed, double range = 6)
        {
            this.speed = (float)speed;
            this.range = (float)range;
        }

        protected override void TickCore(Entity host, ref object state)
        {
            var player = host.GetPlayerOwner();
            if (player == null)
                return;

            PetFollowState s;
            
            if (state == null) s = new PetFollowState();
            else s = (PetFollowState)state;

            Status = CycleStatus.NotStarted;


            //if (player.HasConditionEffect(ConditionEffects.Paralyzed)) return; //if Pet Stasis, turn to chicken kekw

            if(coolDown <= 0)
            {
                s.State = F.Calculate;
                coolDown = 1000;
            }
            else
            {
                coolDown -= (int)CoreConstant.worldLogicTickMs;
            }

            Vector2 vect;
            var pX = player.X;
            var pY = player.Y;
            switch (s.State)
            {
                case F.Calculate:
                    Status = CycleStatus.InProgress;
                    var distance = new Vector2(pX - host.X, pY - host.Y);
                    if (distance.Length() > range)
                        goto case F.ReturnToParent;
                    else
                        goto case F.FollowParent;
                case F.ReturnToParent:
                    host.Move(player.X, player.Y); //If further than 10 tiles than just tp
                    goto case F.Calculate;
                case F.FollowParent:
                    vect = new Vector2(pX - host.X, pY - host.Y);
                    vect.X -= Random.Next(-2, 2) / 2f;
                    vect.Y -= Random.Next(-2, 2) / 2f;
                    vect.Normalize();
                    var dist = host.GetSpeed(speed) * (CoreConstant.worldLogicTickMs / 1000f);
                    host.ValidateAndMove(host.X + vect.X * dist, host.Y + vect.Y * dist);
                    Status = CycleStatus.Completed;
                    break;
            }

            state = s;
        }
    }
}
