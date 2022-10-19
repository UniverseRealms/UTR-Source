using common;

namespace wServer.networking.packets.incoming
{
    public class UsePotion : IncomingMessage
    {
        public int ItemId { get; set; }

        public override PacketId ID => PacketId.USE_POTION;
        public override Packet CreateInstance() { return new UsePotion(); }

        protected override void Read(NReader rdr)
        {
            ItemId = rdr.ReadInt32();
        }
        protected override void Write(NWriter wtr)
        {
        }
    }
}
