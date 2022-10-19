using common.resources;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using Player = wServer.realm.entities.Player;

namespace wServer.networking.handlers
{
    internal class TeleportHandler : PacketHandlerBase<Teleport>
    {
        public override PacketId ID => PacketId.TELEPORT;

        protected override void HandlePacket(Client client, Teleport packet)
        {
            client.Manager.Core.addPendingAction(() => Handle(client.Player, packet.ObjectId));
        }

        private static void Handle(Player player, int objId)
        {
            if (player?.Owner == null) return;

            player.Teleport(objId, player.HasConditionEffect(ConditionEffects.Hidden));
        }
    }
}
