using wServer.realm;

namespace wServer.logic.behaviors
{
    internal class SetNoXP : Behavior
    {
        //State storage: timer

        public SetNoXP()
        {
        }

        protected override void TickCore(Entity host, ref object state)
        {
            host.GivesNoXp = true;
        }
    }
}
