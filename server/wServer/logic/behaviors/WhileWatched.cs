using System.Linq;
using wServer.realm;
using Player = wServer.realm.entities.Player;

namespace wServer.logic.behaviors
{
    internal class WhileWatched : CycleBehavior
    {
        private Behavior child;

        public WhileWatched(Behavior child)
        {
            this.child = child;
        }

        protected override void OnStateEntry(Entity host, ref object state)
        {
            foreach (var player in host.GetNearestEntities(Player.Radius, null, true).OfType<Player>())
                if (player.clientEntities.Contains(host))
                {
                    child.OnStateEntry(host);
                    if (child is CycleBehavior)
                        Status = (child as CycleBehavior).Status;
                    else
                        Status = CycleStatus.InProgress;
                    return;
                }
            Status = CycleStatus.NotStarted;
        }

        protected override void TickCore(Entity host, ref object state)
        {
            foreach (var player in host.GetNearestEntities(Player.Radius, null, true).OfType<Player>())
                if (player.clientEntities.Contains(host))
                {
                    child.Tick(host);
                    if (child is CycleBehavior)
                        Status = (child as CycleBehavior).Status;
                    else
                        Status = CycleStatus.InProgress;
                    return;
                }
            Status = CycleStatus.NotStarted;
        }

        protected override void OnStateExit(Entity host, ref object state)
        {
            foreach (var player in host.GetNearestEntities(Player.Radius, null, true).OfType<Player>())
                if (player.clientEntities.Contains(host))
                {
                    child.OnStateExit(host);
                    if (child is CycleBehavior)
                        Status = (child as CycleBehavior).Status;
                    else
                        Status = CycleStatus.InProgress;
                    return;
                }
            Status = CycleStatus.NotStarted;
        }
    }
}
