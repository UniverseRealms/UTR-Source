using common;

namespace wServer.networking.packets.outgoing
{
    class InvitedToParty : OutgoingMessage
    {
        public string Name;
        public int PartyId;

        public override PacketId ID => PacketId.INVITEDTOPARTY;
        public override Packet CreateInstance() { return new InvitedToParty(); }

        protected override void Read(NReader rdr)
        {
            Name = rdr.ReadUTF();
            PartyId = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.WriteUTF(Name);
            wtr.Write(PartyId);
        }
    }
}
