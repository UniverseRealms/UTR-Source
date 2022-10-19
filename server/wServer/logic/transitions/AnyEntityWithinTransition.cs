using wServer.realm;

namespace wServer.logic.transitions
{
    internal class AnyEntityWithinTransition : Transition
    {
        //State storage: none

        private readonly int _dist;
        private readonly bool _enemies;

        public AnyEntityWithinTransition(int dist, string targetState, bool enemiesOnly)
            : base(targetState)
        {
            _dist = dist;
            _enemies = enemiesOnly;
        }

        protected override bool TickCore(Entity host, ref object state)
        {
            return host.AnyEnemyNearby(_dist) || (!_enemies && host.AnyPlayerNearby(_dist));
        }
    }
}
