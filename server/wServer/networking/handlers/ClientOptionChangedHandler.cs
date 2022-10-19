using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.networking.packets;
using wServer.networking.packets.incoming;

namespace wServer.networking.handlers
{
    class ClientOptionChangedHandler : PacketHandlerBase<ClientOptionChanged>
    {
        public override PacketId ID => PacketId.CLIENT_OPTION_CHANGED;

        protected override void HandlePacket(Client client, ClientOptionChanged packet)
        {
            client.Manager.Core.addPendingAction(() => Handle(client, packet));
        }

        private static void Handle(Client c, ClientOptionChanged packet)
        {
            switch(packet.Type)
            {
                case ClientOptionChanged.ALLY_DAMAGE: c.IgnoreAllyDamageText = packet.Value;
                    break;
                case ClientOptionChanged.ALLY_PROJECTILES: c.IgnoreAllyProjectiles = packet.Value;
                    break;
            }
        }
    }
}
