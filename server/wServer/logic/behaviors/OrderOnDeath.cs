using System.Linq;
using wServer.realm;

namespace wServer.logic.behaviors
{
    internal class OrderOnDeath : Behavior
    {
        private readonly double _range;
        private readonly ushort _target;
        private readonly string _targetName;
        private readonly string _stateName;
        private readonly float _probability;

        private State _targetState;

        public OrderOnDeath(double range, string target, string state, double probability = 1)
        {
            _range = range;
            _target = GetObjType(target);
            _targetName = target;
            _stateName = state;
            _probability = (float)probability;
        }

        private static State FindState(State state, string name)
        {
            if (state.Name == name)
                return state;

            return state.States
                .Select(i => FindState(i, name))
                .FirstOrDefault(s => s != null);
        }

        protected internal override void Resolve(State parent)
        {
            parent.Death += (sender, e) =>
            {
                if (_targetState == null)
                    _targetState = FindState(e.Host.Manager.Behaviors.Definitions[_target].Item1, _stateName);

                if (_targetState == null)
                {
                    Program.Debug(typeof(OrderOnDeath), $"State '{_stateName}' doesn't exist in BehaviorDb of entity: '{_targetName}'.");
                    return;
                }

                var rnd = Random.NextDouble();
                var candidates = e.Host.Owner.Enemies.Values.Where(en => en.ObjectType == _target && en.CurrentState != null).ToList();

                if (rnd > _probability)
                    return;

                candidates.Select(enemy =>
                {
                    var lastState = enemy.CurrentState.Name;

                    if (!enemy.CurrentState.Is(_targetState))
                        enemy.SwitchTo(_targetState);

                    return enemy;
                }).ToList();
            };
        }

        protected override void TickCore(Entity host, ref object state)
        {
        }
    }
}
