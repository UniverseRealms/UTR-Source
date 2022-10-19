using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.realm.entities;
using wServer.realm.entities.vendors;

namespace wServer.networking.handlers
{
    internal class BuyHandler : PacketHandlerBase<Buy>
    {
        public override PacketId ID => PacketId.BUY;

        protected override void HandlePacket(Client client, Buy packet)
        {
            client.Manager.Core.addPendingAction(() => Handle(client.Player, packet.ObjectId));
            //Handle(client.Player, packet.ObjectId);
        }

        private void Handle(Player player, int objId)
        {
            if (player?.Owner == null)
                return;

            if (player.isGambling)
            {
                player.SendError("Cannot purchase items while gambling!");
                return;
            }

            if (player.tradeTarget != null)
            {
                player.SendError("Cannot purchase items while trading!");
                return;
            }

            var obj = player.Owner.GetEntity(objId) as SellableObject;
            player.PurchaseTarget = null;
            obj?.Buy(player);
        }
    }
}
