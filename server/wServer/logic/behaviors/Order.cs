using System.Linq;
using wServer.realm;

namespace wServer.logic.behaviors
{
    internal class Order : Behavior
    {
        //State storage: none

        private readonly double _range;
        private readonly ushort _children;
        private readonly string _targetStateName;
        private State _targetState;

        public Order(double range, string children, string targetState)
        {
            _range = range;
            _children = GetObjType(children);
            _targetStateName = targetState;
        }

        private static State FindState(State state, string name)
        {
            if (state.Name == name)
                return state;

            return state.States
                .Select(i => FindState(i, name))
                .FirstOrDefault(s => s != null);
        }

        protected override void TickCore(Entity host, ref object state)
        {
            if (_targetState == null)
                _targetState = FindState(host.Manager.Behaviors.Definitions[_children].Item1, _targetStateName);

            foreach (var i in host.GetNearestEntities(_range, _children))
            {
                if (i.CurrentState == null)
                {
                    Program.Debug(typeof(Order), $"Entity current state is null, target state: {_targetStateName}, state ordered by: {host.Name}");
                    return;
                }

                if (!i.CurrentState.Is(_targetState))
                    i.SwitchTo(_targetState);
            }
        }
    }
}
