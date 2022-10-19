using common;

namespace wServer.networking.packets.outgoing
{
    public class UpdateMarks : OutgoingMessage
    {
        public int Mark { get; set; }
        public int Node1 { get; set; }
        public int Node2 { get; set; }
        public int Node3 { get; set; }
        public int Node4 { get; set; }

        public override PacketId ID => PacketId.UPDATEMARKS;

        public override Packet CreateInstance()
        {
            return new UpdateMarks();
        }

        protected override void Read(NReader rdr)
        {
            Mark = rdr.ReadInt32();
            Node1 = rdr.ReadInt32();
            Node2 = rdr.ReadInt32();
            Node3 = rdr.ReadInt32();
            Node4 = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(Mark);
            wtr.Write(Node1);
            wtr.Write(Node2);
            wtr.Write(Node3);
            wtr.Write(Node4);
        }
    }
}
