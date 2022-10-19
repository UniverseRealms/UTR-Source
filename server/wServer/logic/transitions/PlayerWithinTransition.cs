using wServer.realm;
using System.Linq;
using wServer.realm.entities;

namespace wServer.logic.transitions
{
    internal class PlayerWithinTransition : Transition
    {
        //State storage: none

        private readonly double _dist;
        private readonly bool _seeInvis;

        public PlayerWithinTransition(double dist, string targetState, bool seeInvis = false)
            : base(targetState)
        {
            _dist = dist;
            _seeInvis = seeInvis;
        }

        protected override bool TickCore(Entity host, ref object state)
        {
            return host.GetNearestEntities(_dist, null, _seeInvis).Any(e => e is Player);
        }
    }
}
