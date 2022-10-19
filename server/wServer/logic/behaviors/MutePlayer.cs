using wServer.realm;
using wServer.realm.entities;

namespace wServer.logic.behaviors
{
    internal class MutePlayer : Behavior
    {
        private readonly int _timeout;

        public MutePlayer(int durationMin = 0)
        {
            _timeout = durationMin;
        }

        protected override void OnStateEntry(Entity host, ref object state)
        {
            if (host.AttackTarget?.Owner == null || (host.AttackTarget as Player).Muted)
                return;

            var muteCmd = host.Manager.Commands.Commands["mute"];
            muteCmd.Execute(null, $"{host.AttackTarget.Name} {_timeout}", true);
        }

        protected override void TickCore(Entity host, ref object state)
        {
        }
    }
}
