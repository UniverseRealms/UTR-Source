using wServer.realm;

namespace wServer.logic.behaviors
{
    internal class TransitionOnTick : CycleBehavior
    {
        //State storage: none

        private readonly string _targetStateName;
        private State _targetState;

        public TransitionOnTick(string targetState)
        {
            _targetStateName = targetState;
        }

        protected override void TickCore(Entity host, ref object state)
        {
            Status = CycleStatus.InProgress;
            if (_targetState == null)
                _targetState = State.FindState(host.Manager.Behaviors.Definitions[host.ObjectType].Item1, _targetStateName);

            if (!host.CurrentState.Is(_targetState))
            {
                host.SwitchTo(_targetState);
                Status = CycleStatus.Completed;
            }
        }
    }
}
