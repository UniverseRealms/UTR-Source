using System.Text.RegularExpressions;
using wServer.realm;
using Player = wServer.realm.entities.Player;

namespace wServer.logic.transitions
{
    internal class PlayerTextTransition : Transition
    {
        //State storage: none

        private readonly double? _distSqr;
        private readonly string _regex;
        private readonly bool _setAttackTarget;
        private readonly bool _ignoreCase;
        private bool newState = false;

        public PlayerTextTransition(string targetState, string regex, double? dist = null, bool setAttackTarget = false, bool ignoreCase = true) : base(targetState)
        {
            if (dist != null)
                _distSqr = dist.Value * dist.Value;
            _regex = regex;
            _setAttackTarget = setAttackTarget;
            _ignoreCase = ignoreCase;
        }

        protected override bool TickCore(Entity host, ref object state) => newState;

        public void OnChatReceived(Player player, string text, Entity host)
        {
            var rgx = (_ignoreCase) ? new Regex(_regex, RegexOptions.IgnoreCase) : new Regex(_regex);

            var match = rgx.Match(text);
            if (match.Success && (_distSqr == null || MathsUtils.DistSqr(player.X, player.Y, host.X, host.Y) <= _distSqr))
            {
                newState = true;
                host.AttackTarget = _setAttackTarget ? player : null;
            }
        }
    }
}
