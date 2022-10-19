using wServer.realm;

namespace wServer.logic.transitions
{
    internal class RandomTransition : Transition
    {
        private double _probability;
        private bool _activated;

        public RandomTransition(double probability, params string[] states)
            : base(states)
        {
            _probability = probability;
            _activated = false;
        }

        protected override bool TickCore(Entity host, ref object state)
        {
            if (_activated) return false;

            _activated = true;

            SelectedState = 0;
            return Random.NextDouble() < _probability;
        }
    }
}
