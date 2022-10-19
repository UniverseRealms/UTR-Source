using System.Linq;
using wServer.realm;
using wServer.realm.entities;

namespace wServer.logic.behaviors
{
    internal class BringEnemy : Behavior
    {
        private string name;
        private double range;

        public BringEnemy(string name, double range)
        {
            this.name = name;
            this.range = range;
        }

        protected override void OnStateEntry(Entity host, ref object state)
        {
            foreach (var entity in host.GetNearestEntitiesByName(range, name).OfType<Enemy>())
                entity.Move(host.X, host.Y);
        }

        protected override void TickCore(Entity host, ref object state)
        {
        }
    }
}
