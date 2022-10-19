using wServer.realm;

namespace wServer.logic.behaviors
{
    internal class WhileEntityWithin : Behavior
    {
        private Behavior child;
        private string entityName;
        private double range;

        public WhileEntityWithin(Behavior child, string entityName, double range)
        {
            this.child = child;
            this.entityName = entityName;
            this.range = range;
        }

        protected override void OnStateEntry(Entity host, ref object state)
        {
            child.OnStateEntry(host);
        }

        protected override void TickCore(Entity host, ref object state)
        {
            if (host.GetNearestEntityByName(range, entityName) != null) child.Tick(host);
        }
    }
}
