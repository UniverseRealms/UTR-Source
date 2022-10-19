using wServer.networking.packets;
using wServer.networking.packets.incoming;

namespace wServer.networking.handlers
{
    internal class QueuePongHandler : PacketHandlerBase<QueuePong>
    {
        public override PacketId ID => PacketId.QUEUE_PONG;

        protected override void HandlePacket(Client client, QueuePong packet)
        {
            client.Manager.Core.addPendingAction(() => Handle(client, packet));
        }

        private void Handle(Client client, QueuePong packet)
        {
            client.Pong(packet);
        }
    }
}
