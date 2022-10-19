using common;

namespace wServer.networking.packets.incoming
{
    public class RequestToken : IncomingMessage
    {
        public string Token { get; set; }
        public bool IsAir { get; set; }

        public override PacketId ID => PacketId.REQUESTTOKEN;

        public override Packet CreateInstance()
        {
            return new RequestToken();
        }

        protected override void Read(NReader rdr)
        {
            Token = rdr.ReadUTF();
            IsAir = rdr.ReadBoolean();

        }

        protected override void Write(NWriter wtr)
        {
            wtr.WriteUTF(Token);
            wtr.Write(IsAir);
        }
    }
}