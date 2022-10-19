
using common;

namespace wServer.networking.packets.incoming
{
	public class AchievementRequest : IncomingMessage
	{
		public int Name { get; set; }

		public override PacketId ID => PacketId.ACHIEVEMENT;

		public override Packet CreateInstance()
		{
			return new AchievementRequest();
		}
		protected override void Read(NReader rdr)
		{
			Name = rdr.ReadInt32(); 
		}
		protected override void Write(NWriter wtr)
		{
			wtr.Write(Name);
		}
	}
}
