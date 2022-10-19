using wServer.realm;
using wServer.realm.worlds.logic;

namespace wServer.logic.behaviors
{
    internal class Announce : Behavior
    {
        private readonly string _announce;

        public Announce(string msg)
        {
            _announce = msg;
        }
        protected override void OnStateEntry(Entity host, ref object state)
        {
            if (host.Spawned || host.Owner is Test) return;
            host.Manager.Chat.Announce(_announce);
        }

        protected override void TickCore(Entity host, ref object state)
        { }
    }
}