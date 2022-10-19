using wServer.networking.packets.outgoing;
using wServer.realm;

namespace wServer.logic.behaviors
{
    internal class ChangeMusicOnDeath : Behavior
    {
        private readonly string _music;

        public ChangeMusicOnDeath(string file)
        {
            _music = file;
        }

        protected internal override void Resolve(State parent)
        {
            parent.Death += (sender, e) =>
            {
                if (e.Host.Owner.Music != _music)
                {
                    var owner = e.Host.Owner;

                    owner.Music = _music;

                    var i = 0;
                    foreach (var plr in owner.Players.Values)
                    {
                        owner.Timers.Add(new WorldTimer(100 * i, (w) =>
                        {
                            if (w == null || w.Deleted || plr == null || plr.Client == null) return;

                            plr.Client.SendPacket(new SwitchMusic { Music = _music });
                        }));

                        i++;
                    }
                }
            };
        }

        protected override void TickCore(Entity host, ref object state)
        { }
    }
}
