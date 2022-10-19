using wServer.realm;
using wServer.realm.entities;

namespace wServer.logic.behaviors
{
    public class ClearLoot : Behavior
    {

        public ClearLoot()
        {
        }

        protected override void OnStateEntry(Entity host, ref object state)
        {
            Enemy en = host as Enemy;
            en.SetDamageCounter(new DamageCounter(en), en);
        }

        protected override void TickCore(Entity host, ref object state)
        {
        }
    }
}