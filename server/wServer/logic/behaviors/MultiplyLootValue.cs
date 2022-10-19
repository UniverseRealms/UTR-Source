using wServer.realm;

namespace wServer.logic.behaviors
{
    internal class MultiplyLootValue : Behavior
    {
        //State storage: cooldown timer

        private int multiplier;

        public MultiplyLootValue(int multiplier)
        {
            this.multiplier = multiplier;
        }

        protected override void OnStateEntry(Entity host, ref object state)
        {
            state = false;
        }

        protected override void TickCore(Entity host, ref object state)
        {
            bool multiplied = (bool)state;
            if (!multiplied)
            {
                var newLootValue = host.LootValue * multiplier;
                host.LootValue = newLootValue;
                multiplied = true;
            }
            state = multiplied;
        }
    }
}
