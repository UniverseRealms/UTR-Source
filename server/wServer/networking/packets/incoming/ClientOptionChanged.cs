using common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wServer.networking.packets.incoming
{
    public class ClientOptionChanged : IncomingMessage
    {
        public override PacketId ID => PacketId.CLIENT_OPTION_CHANGED;

        public const int ALLY_DAMAGE = 0, ALLY_PROJECTILES = 1;

        public int Type { get; set; }
        public bool Value { get; set; }

        public override Packet CreateInstance()
        {
            return new ClientOptionChanged();
        }

        protected override void Read(NReader rdr)
        {
            Type = rdr.ReadInt32();
            Value = rdr.ReadBoolean();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(Type);
            wtr.Write(Value);
        }
    }
}
