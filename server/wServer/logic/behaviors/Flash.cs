using wServer.networking.packets.outgoing;
using wServer.realm;

namespace wServer.logic.behaviors
{
    internal class Flash : Behavior
    {
        //State storage: none

        private uint color;
        private float flashPeriod;
        private int flashRepeats;

        public Flash(uint color, double flashPeriod, int flashRepeats)
        {
            this.color = color;
            this.flashPeriod = (float)flashPeriod;
            this.flashRepeats = flashRepeats;
        }

        protected override void TickCore(Entity host, ref object state)
        {
        }

        protected override void OnStateEntry(Entity host, ref object state)
        {
            host.Owner.BroadcastPacketNearby(new ShowEffect()
            {
                EffectType = EffectType.Flashing,
                Pos1 = new Position() { X = flashPeriod, Y = flashRepeats },
                TargetObjectId = host.Id,
                Color = new ARGB(color)
            }, host, null);
        }
    }
}
