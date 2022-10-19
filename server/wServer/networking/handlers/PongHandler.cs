using wServer.networking.packets;
using wServer.networking.packets.incoming;

namespace wServer.networking.handlers
{
    internal class PongHandler : PacketHandlerBase<Pong>
    {
        public override PacketId ID => PacketId.PONG;

        protected override void HandlePacket(Client client, Pong packet)
        {
            client.Manager.Core.addPendingAction(() => Handle(client, packet));
        }

        private void Handle(Client client, Pong packet)
        {
            client.Player?.Pong(packet);
        }
    }
}
