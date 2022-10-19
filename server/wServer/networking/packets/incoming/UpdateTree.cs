using common;

namespace wServer.networking.packets.incoming
{
    public class UpdateTree : IncomingMessage
    {
        public override PacketId ID => PacketId.UPDATE_TREE;
        public override Packet CreateInstance() { return new UpdateTree(); }

        protected override void Read(NReader rdr)
        {
        }
        protected override void Write(NWriter wtr)
        {
        }
    }
}
