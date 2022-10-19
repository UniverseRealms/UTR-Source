#region

using wServer.networking.packets.outgoing;
using wServer.realm;

#endregion

namespace wServer.logic.behaviors
{
    public class WorldBossAnnounce : Behavior
    {
        protected override void OnStateEntry(Entity host, ref object state)
        {
        }

        public RealmManager Manager { get; }
        private bool once { get; set; }

        protected override void TickCore(Entity host, ref object state)
        {
            if (once)
                return;

            once = true;

            foreach (var w in Manager.Worlds.Values)
                foreach (var p in w.Players.Values)
                {
                    p.Client.SendPacket(new Text
                    {
                        BubbleTime = 0,
                        NumStars = -1,
                        Name = "WORLD BOSS",
                        Txt = $"The World Boss has been spawned! Come join and help us fight off {host.Name} , at {host.Owner.Name} ({host.X}, {host.Y}!",
                        NameColor = 0xFF0000,
                        TextColor = 0xff7f7f
                    });
                }
        }
    }
}
