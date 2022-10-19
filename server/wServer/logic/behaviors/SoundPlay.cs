using wServer.networking.packets.outgoing;
using wServer.realm;

namespace wServer.logic.behaviors
{
    public class SoundPlay : Behavior
    {
        private readonly int soundId;
        public SoundPlay(int soundId = 0)
        {
            this.soundId = soundId;
        }

        protected override void OnStateEntry(Entity host, ref object state)
        {
            var owner = host.Owner;
            foreach (var plr in owner.Players.Values)
            {
                plr.Client.SendPacket(new PlaySound
                {
                    OwnerId = host.Id,
                    SoundId = soundId
                });
            }
        }

        protected override void TickCore(Entity host, ref object state) { }
    }
}
