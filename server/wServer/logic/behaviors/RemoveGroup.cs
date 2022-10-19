using System.Linq;
using wServer.realm;
using wServer.realm.entities;

namespace wServer.logic.behaviors
{
    internal class RemoveGroup : Behavior
    {
        private readonly float dist;
        private readonly string group;

        public RemoveGroup(double dist, string group)
        {
            this.dist = (float)dist;
            this.group = group;
        }

        protected override void OnStateEntry(Entity host, ref object state)
        {
            var lastKilled = -1;
            var killed = 0;
            while (killed != lastKilled)
            {
                lastKilled = killed;
                foreach (var entity in host.GetNearestEntitiesByGroup(dist, group).OfType<Enemy>())
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
