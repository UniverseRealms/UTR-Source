using wServer.realm;
using wServer.realm.entities;
using System.Linq;

namespace wServer.logic.transitions
{
    internal class AllyCountLessTransition : Transition
    {
        //State storage: none

        private readonly double _dist;
        private readonly ushort _target;
        private readonly bool _seeInvis;
        private readonly int _count;

        public AllyCountLessTransition(string target, double dist, string targetState, int count, bool seeInvis = false)
            : base(targetState)
        {
            _dist = dist;
            _target = Behavior.GetObjType(target);
            _seeInvis = seeInvis;
            _count = count;
        }

        protected override bool TickCore(Entity host, ref object state)
        {
            return host.GetNearestAllies(_dist, null, _seeInvis).ToArray().Length < _count;
        }
    }
}