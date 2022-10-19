using common;

namespace wServer.networking.packets.incoming
{
    public class ForgeItem : IncomingMessage
    {
        public ObjectSlot Top { get; set; }
        public ObjectSlot Bottom { get; set; }

        public override PacketId ID => PacketId.FORGEITEM;

        public override Packet CreateInstance()
        {
            return new ForgeItem();
        }

        protected override void Read(NReader rdr)
        {
            Top = ObjectSlot.Read(rdr);
            Bottom = ObjectSlot.Read(rdr);
        }

        protected override void Write(NWriter wtr)
        {
            Top.Write(wtr);
            Bottom.Write(wtr);
        }
    }
}