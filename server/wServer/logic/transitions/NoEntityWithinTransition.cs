using wServer.realm;

namespace wServer.logic.transitions
{
    internal class NoEntityWithinTransition : Transition
    {
        //State storage: none

        private readonly int _dist;
        private readonly bool _enemies;

        public NoEntityWithinTransition(int dist, string targetState, bool enemies)
            : base(targetState)
        {
            _dist = dist;
            _enemies = enemies;
        }

        protected override bool TickCore(Entity host, ref object state)
        {
            return !host.AnyEnemyNearby(_dist) && (!_enemies && !host.AnyPlayerNearby(_dist));
        }
    }
}
