﻿using common.resources;
using Mono.Game;
using System;
using wServer.realm;
using wServer.realm.cores;

namespace wServer.logic.behaviors
{
    internal class Follow : CycleBehavior
    {
        //State storage: follow state
        private class FollowState
        {
            public F State;
            public int RemainingTime;
        }

        private enum F
        {
            DontKnowWhere,
            Acquired,
            Resting
        }

        private readonly float speed;
        private readonly float acquireRange;
        private readonly float range;
        private readonly int duration;
        private Cooldown coolDown;
        private readonly bool predictive;

        public Follow(double speed, double acquireRange = 10, double range = 6,
            int duration = 0, Cooldown coolDown = new Cooldown(), bool predictive = false)
        {
            this.speed = (float)speed;
            this.acquireRange = (float)acquireRange;
            this.range = (float)range;
            this.duration = duration;
            this.coolDown = coolDown.Normalize(duration == 0 ? 0 : 1000);
            this.predictive = predictive;
        }

        protected override void TickCore(Entity host, ref object state)
        {
            FollowState s;

            if (state == null) s = new FollowState();
            else s = (FollowState)state;

            Status = CycleStatus.NotStarted;

            if (host.HasConditionEffect(ConditionEffects.Paralyzed)) return;

            var player = host.AttackTarget ?? host.GetNearestEntity(acquireRange, null);

            Vector2 vect;

            switch (s.State)
            {
                case F.DontKnowWhere:
                    if (player != null && s.RemainingTime <= 0)
                    {
                        s.State = F.Acquired;

                        if (duration > 0) s.RemainingTime = duration;

                        goto case F.Acquired;
                    }
                    else if (s.RemainingTime > 0) s.RemainingTime -= (int)CoreConstant.worldLogicTickMs;
                    break;

                case F.Acquired:
                    if (player == null)
                    {
                        s.State = F.DontKnowWhere;
                        s.RemainingTime = 0;
                        break;
                    }
                    else if (s.RemainingTime <= 0 && duration > 0)
                    {
                        s.State = F.DontKnowWhere;
                        s.RemainingTime = coolDown.Next(Random);

                        Status = CycleStatus.Completed;
                        break;
                    }

                    if (s.RemainingTime > 0) s.RemainingTime -= (int)CoreConstant.worldLogicTickMs;

                    var pX = player.X;
                    var pY = player.Y;

                    if (predictive)
                    {
                        var history = player.TryGetHistory(1);

                        pX += 4 * (player.X - history.Value.X);
                        pY += 4 * (player.X - history.Value.Y);
                    }

                    vect = new Vector2(pX - host.X, pY - host.Y);

                    if (vect.Length() > range)
                    {
                        Status = CycleStatus.InProgress;

                        vect.X -= Random.Next(-2, 2) / 2f;
                        vect.Y -= Random.Next(-2, 2) / 2f;
                        vect.Normalize();

                        var dist = host.GetSpeed(speed) * (CoreConstant.worldLogicTickMs / 1000f);
                        host.ValidateAndMove(host.X + vect.X * dist, host.Y + vect.Y * dist);
                    }
                    else
                    {
                        Status = CycleStatus.Completed;

                        s.State = F.Resting;
                        s.RemainingTime = 0;
                    }
                    break;

                case F.Resting:
                    if (player == null)
                    {
                        s.State = F.DontKnowWhere;

                        if (duration > 0) s.RemainingTime = duration;
                        break;
                    }

                    Status = CycleStatus.Completed;

                    vect = new Vector2(player.X - host.X, player.Y - host.Y);

                    if (vect.Length() > range + 1)
                    {
                        s.State = F.Acquired;
                        s.RemainingTime = duration;

                        goto case F.Acquired;
                    }
                    break;
            }

            state = s;
        }
    }
}
