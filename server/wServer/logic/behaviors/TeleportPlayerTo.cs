using System.Linq;
using wServer.realm;
using Player = wServer.realm.entities.Player;

namespace wServer.logic.behaviors
{
    internal class TeleportPlayerTo : Behavior
    {
        private float x;
        private float y;
        private readonly double _dist;

        public TeleportPlayerTo(float X, float Y, double dist)
        {
            _dist = dist;
            x = X;
            y = Y;
        }

        protected override void OnStateEntry(Entity host, ref object state)
        {
        }

        protected override void TickCore(Entity host, ref object state)
        {
            bool isclose = true;
            foreach (var ply in host.GetNearestEntities(_dist, null, true).OfType<Player>())
            {
                state = CycleStatus.NotStarted;
                if (isclose)
                {
                    ply.TeleportPosition(x, y, ignoreRestrictions: true);
                    isclose = false;
                    state = CycleStatus.InProgress;
                }
                else
                {
                    state = CycleStatus.Completed;
                }
            }
        }
    }
}
