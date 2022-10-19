using common;

namespace wServer.networking.packets.outgoing
{
    public class SkillTree : OutgoingMessage
    {
        public int[] Skilltree { get; set; }

        public override PacketId ID => PacketId.SKILL_TREE;

        public override Packet CreateInstance()
        {
            return new SkillTree();
        }

        protected override void Read(NReader rdr)
        {
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write((short)Skilltree.Length);
            foreach (var i in Skilltree)
                wtr.Write(i);
        }
    }
}