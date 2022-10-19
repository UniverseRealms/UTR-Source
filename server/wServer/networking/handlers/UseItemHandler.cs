using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.realm.entities;

namespace wServer.networking.handlers
{
    internal class UseItemHandler : PacketHandlerBase<UseItem>
    {
        public override PacketId ID => PacketId.USEITEM;

        protected override void HandlePacket(Client client, UseItem packet)
        {
            client.Manager.Core.addPendingAction(() => Handle(client.Player, packet));
        }

        private void Handle(Player player, UseItem packet)
        {
            if (player.Owner == null) return;

            player.UseItem(packet.SlotObject.ObjectId, packet.SlotObject.SlotId, packet.ItemUsePos);
        }
    }
}
