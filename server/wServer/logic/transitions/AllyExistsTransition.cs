using wServer.realm;
using wServer.realm.entities;
using System.Linq;

namespace wServer.logic.transitions
{
    internal class AllyExistsTransition : Transition
    {
        //State storage: none

        private readonly double _dist;
        private readonly ushort _target;
        private readonly bool _seeInvis;

        public AllyExistsTransition(string target, double dist, string targetState, bool seeInvis = false)
            : base(targetState)
        {
            _dist = dist;
            _target = Behavior.GetObjType(target);
            _seeInvis = seeInvis;
        }

        protected override bool TickCore(Entity host, ref object state)
        {
            return host.GetNearestAllies(_dist, _target, _seeInvis).Any();
        }
    }
}