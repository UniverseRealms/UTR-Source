using common;

namespace wServer.networking.packets.incoming
{
	public class BountyRequest : IncomingMessage
	{
		public int BountyId { get; set; }

		public override PacketId ID => PacketId.BOUNTYREQUEST;

		public override Packet CreateInstance()
		{
			return new BountyRequest();
		}

		protected override void Read(NReader rdr)
		{
            BountyId = rdr.ReadInt32();
		}

		protected override void Write(NWriter wtr)
		{
            wtr.Write(BountyId);
		}
	}
}