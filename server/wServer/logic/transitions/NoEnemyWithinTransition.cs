using wServer.realm;
using wServer.realm.entities;
using common.resources;

namespace wServer.logic.transitions
{
    internal class NoEnemyWithinTransition : Transition
    {
        //State storage: none

        private readonly double _dist;

        public NoEnemyWithinTransition(double dist, string targetState)
            : base(targetState)
        {
            _dist = dist;
        }

        protected override bool TickCore(Entity host, ref object state)
        {
            var en = host.GetNearestEntity(_dist, false);
            if (en == null)
                return false;
            


            if (en.HasConditionEffect(ConditionEffects.Invulnerable) || en.HasConditionEffect(ConditionEffects.Invincible) || en.HasConditionEffect(ConditionEffects.Invisible))
                return false;


            return true;
        }
    }
}
