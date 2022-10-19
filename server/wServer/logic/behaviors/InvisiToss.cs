#region

using common.resources;
using System;
using wServer.realm;
using wServer.realm.cores;
using wServer.realm.entities;

#endregion

namespace wServer.logic.behaviors
{
    internal class InvisiToss : Behavior
    {
        //State storage: cooldown timer

        private readonly ushort child;
        private readonly int coolDownOffset;
        private readonly double range;
        private double? angle;
        private int count;
        private double angleOffset;
        private Cooldown coolDown;

        public InvisiToss(string child, double range = 5, double? angle = null,
            Cooldown coolDown = new Cooldown(), int coolDownOffset = 0,
            int count = 1, double angleOffset = 0)
        {
            this.child = BehaviorDb.InitGameData.IdToObjectType[child];
            this.range = range;
            this.angle = angle * Math.PI / 180;
            this.coolDown = coolDown.Normalize();
            this.coolDownOffset = coolDownOffset;
            this.count = count;
            this.angleOffset = angleOffset * Math.PI / 180;
        }

        protected override void OnStateEntry(Entity host, ref object state)
        {
            state = coolDownOffset;
        }

        protected override void TickCore(Entity host, ref object state)
        {
            int cool = (int)state;
            double a = angle.Value - angleOffset * (count - 1) / 2;
            if (cool <= 0)
            {
                if (host.HasConditionEffect(ConditionEffects.Stunned))
                    return;

                for (int i = 0; i < count; i++)
                {
                    var target = new Position
                    {
                        X = host.X + (float)(range * Math.Cos(a)),
                        Y = host.Y + (float)(range * Math.Sin(a)),
                    };

                    host.Owner.Timers.Add(new WorldTimer(0, (w) =>
                    {
                        if (w == null || w.Deleted || host == null) return;

                        var entity = Entity.Resolve(w.Manager, child);
                        entity.Move(target.X, target.Y);
                        entity.GivesNoXp = true;
                        (entity as Enemy).Terrain = (host as Enemy).Terrain;
                        w.EnterWorld(entity);
                    }));
                    a += angleOffset;
                }
                cool = coolDown.Next(Random);
            }
            else
                cool -= (int)CoreConstant.worldLogicTickMs;

            state = cool;
        }
    }
}
