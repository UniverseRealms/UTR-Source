using wServer.networking.packets;
using wServer.networking.packets.incoming;

namespace wServer.networking.handlers
{
    internal class SetConditionHandler : PacketHandlerBase<SetCondition>
    {
        public override PacketId ID => PacketId.SETCONDITION;

        protected override void HandlePacket(Client client, SetCondition packet)
        {
            //TODO: implement something
        }
    }
}