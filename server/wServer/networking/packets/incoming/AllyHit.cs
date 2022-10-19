using common;

namespace wServer.networking.packets.incoming
{
    public class AllyHit : IncomingMessage
    {
        public int Time { get; set; }
        public byte BulletId { get; set; }
        public int TargetId { get; set; }
        public int ObjectId { get; set; }

        public override PacketId ID => PacketId.ALLYHIT;

        public override Packet CreateInstance()
        {
            return new AllyHit();
        }

        protected override void Read(NReader rdr)
        {
            Time = rdr.ReadInt32();
            BulletId = rdr.ReadByte();
            TargetId = rdr.ReadInt32();
            ObjectId = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(Time);
            wtr.Write(BulletId);
            wtr.Write(TargetId);
            wtr.Write(ObjectId);
        }
    }
}