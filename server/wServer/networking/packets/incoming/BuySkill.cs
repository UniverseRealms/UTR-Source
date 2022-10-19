using common;

namespace wServer.networking.packets.incoming
{
    public class BuySkill : IncomingMessage
    {
        public int SkillId { get; set; }

        public override PacketId ID => PacketId.BUY_SKILL;
        public override Packet CreateInstance() { return new BuySkill(); }

        protected override void Read(NReader rdr)
        {
            SkillId = rdr.ReadInt32();
        }
        protected override void Write(NWriter wtr)
        {
        }
    }
}
