using wServer.realm;
using wServer.realm.entities;

namespace wServer.logic.transitions
{
    public class DamageTakenTransition : Transition
    {
        //State storage: none

        private int damage;
        private int startDamage;

        public DamageTakenTransition(int damage, string targetState)
            : base(targetState)
        {
            this.damage = damage;
        }

        protected override void OnStateEntry(Entity host, ref object state) {
            startDamage = (host as Enemy).DamageCounter.TotalDamage;
        }

        protected override bool TickCore(Entity host, ref object state)
        {
            if ((host as Enemy).DamageCounter.TotalDamage >= startDamage + damage)
                return true;
            return false;
        }
    }
}
