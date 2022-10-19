using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.realm.entities;

namespace wServer.networking.handlers
{
    internal class SorForgeRequestHandler : PacketHandlerBase<SorForgeRequest>
    {
        public override PacketId ID => PacketId.SORFORGEREQUEST;

        protected override void HandlePacket(Client client, SorForgeRequest packet)
        {
            client.Manager.Core.addPendingAction(() => Handle(client.Player, packet));
        }

        private void Handle(Player player, SorForgeRequest packet)
        {
            if (player.Onrane >= 20) player.ascendSorCrystal(player);
            else player.SendInfo("You do not have enough onrane in order to ascend your Sor Crystal!");
        }
    }
}
