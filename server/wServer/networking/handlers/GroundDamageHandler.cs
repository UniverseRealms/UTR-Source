using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.realm.entities;

namespace wServer.networking.handlers
{
    internal class GroundDamageHandler : PacketHandlerBase<GroundDamage>
    {
        public override PacketId ID => PacketId.GROUNDDAMAGE;

        protected override void HandlePacket(Client client, GroundDamage packet)
        {
            client.Manager.Core.addPendingAction(() => Handle(client.Player, packet.Position, packet.Time));
        }

        private void Handle(Player player, Position pos, int timeHit)
        {
            if (player?.Owner == null) return;

            player.ForceGroundHit(pos, timeHit);
        }
    }
}
