using wServer.networking.packets;
using wServer.networking.packets.incoming;

namespace wServer.networking.handlers
{
    internal class CorrectServerHandler : PacketHandlerBase<CorrectServerPacket>
    {
        public override PacketId ID => PacketId.CORRECTSERVER;

        protected override void HandlePacket(Client client, CorrectServerPacket packet)
        {
            Handle(client, packet);
        }

        private void Handle(Client client, CorrectServerPacket packet)
        {
            bool CorrectServ =
                (!(packet.correctServer == 1));
            if (CorrectServ)
            {
                client.Disconnect();
            }
        }
    }
}