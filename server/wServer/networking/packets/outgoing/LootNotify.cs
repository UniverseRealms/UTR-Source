using common;

namespace wServer.networking.packets.outgoing
{
    public class LootNotify : OutgoingMessage
    {
        public byte BagType { get; set; }
        public string Text { get; set; }

        public override PacketId ID => PacketId.LOOT_NOTIFY;
        public override Packet CreateInstance() { return new LootNotify(); }

        protected override void Read(NReader rdr)
        {
            BagType = rdr.ReadByte();
            Text = rdr.ReadUTF();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(BagType);
            wtr.WriteUTF(Text);
        }
    }
}
