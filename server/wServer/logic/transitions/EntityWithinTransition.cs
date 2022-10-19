using wServer.realm;

namespace wServer.logic.transitions
{
    internal class EntityWithinTransition : Transition
    {
        //State storage: none

        private double _distance;
        private string _entity;

        public EntityWithinTransition(double dist, string entity, string targetState)
            : base(targetState)
        {
            _distance = dist;
            _entity = entity;
        }

        protected override bool TickCore(Entity host, ref object state)
        {
            return host.GetNearestEntityByName(_distance, _entity) != null;
        }
    }
}
