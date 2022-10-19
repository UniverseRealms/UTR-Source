using common;

namespace wServer.networking.packets.outgoing
{
    public class ReceiveToken : OutgoingMessage
    {
        public string Token { get; set; }

        public override PacketId ID => PacketId.RECEIVETOKEN;

        public override Packet CreateInstance()
        {
            return new ReceiveToken();
        }

        protected override void Read(NReader rdr)
        {
            Token = rdr.ReadUTF();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.WriteUTF(Token);
        }
    }
}
