using wServer.realm;
using System.Linq;
using wServer.realm.entities;

namespace wServer.logic.transitions
{
    internal class NoPlayerWithinTransition : Transition
    {
        //State storage: none

        private double dist;
        private bool seeInvis;

        public NoPlayerWithinTransition(double dist, string targetState, bool seeInvis = false)
            : base(targetState)
        {
            this.dist = dist;
            this.seeInvis = seeInvis;
        }

        protected override bool TickCore(Entity host, ref object state)
        {
            return !host.GetNearestEntities(dist, null, seeInvis).Any(e => e is Player);
        }
    }
}
