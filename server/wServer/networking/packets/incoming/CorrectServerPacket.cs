using common;

namespace wServer.networking.packets.incoming
{
    public class CorrectServerPacket : IncomingMessage
    {
        public int correctServer { get; set; }

        public override PacketId ID
        {
            get { return PacketId.CORRECTSERVER; }
        }

        public override Packet CreateInstance()
        {
            return new CorrectServerPacket();
        }

        protected override void Read(NReader rdr)
        {
            correctServer = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(correctServer);
        }
    }
}