using wServer.realm;

namespace wServer.logic.behaviors
{
    internal class InstaDeath : Behavior
    {
        private bool IsInstaDeath;
        private int MaxCap;
        private string DeathMessage;

        public InstaDeath(bool insta, int cap = 21, string msg = "Too Many projectiles")
        {
            this.IsInstaDeath = insta;
            this.MaxCap = cap;
            this.DeathMessage = msg;
        }

        protected override void TickCore(Entity host, ref object state)
        {
        }

        protected override void OnStateEntry(Entity host, ref object state)
        {
            host.InstaDeath = this.IsInstaDeath;
            host.MaxHitCap = this.MaxCap;
            host.InstaDeathMessage = this.DeathMessage;
        }
    }
}
