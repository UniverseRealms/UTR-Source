using System.Linq;
using wServer.realm;
using wServer.realm.entities;

namespace wServer.logic.behaviors
{
    internal class RemoveEntity : Behavior
    {
        private readonly float dist;
        private readonly string children;

        public RemoveEntity(double dist, string children)
        {
            this.dist = (float)dist;
            this.children = children;
        }

        protected override void OnStateEntry(Entity host, ref object state)
        {
            var lastKilled = -1;
            var killed = 0;
            while (killed != lastKilled)
            {
                lastKilled = killed;
                foreach (var entity in host.GetNearestEntitiesByName(dist, children).OfType<Enemy>())
                {
                    entity.Spawned = true;
                    entity.Death();
                    killed++;
                }
            }
        }

        protected override void TickCore(Entity host, ref object state)
        {
        }
    }
}
