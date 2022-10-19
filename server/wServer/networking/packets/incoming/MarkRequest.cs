using common;

namespace wServer.networking.packets.incoming
{
    public class MarkRequest : IncomingMessage
    {

        public int Slot_ { get; set; }
        public int Id_ { get; set; }
        public int Type_ { get; set; }

        public override PacketId ID => PacketId.MARKREQUEST;

        public override Packet CreateInstance()
        {
            return new MarkRequest();
        }

        protected override void Read(NReader rdr)
        {
            Slot_ = rdr.ReadInt32();
            Id_ = rdr.ReadInt32();
            Type_ = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
        }
    }
}