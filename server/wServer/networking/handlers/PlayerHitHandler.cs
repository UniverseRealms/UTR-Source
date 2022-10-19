using System;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.realm.entities;

namespace wServer.networking.handlers
{
    internal class PlayerHitHandler : PacketHandlerBase<PlayerHit>
    {
        public override PacketId ID => PacketId.PLAYERHIT;

        protected override void HandlePacket(Client client, PlayerHit packet)
        {
            client.Manager.Core.addPendingAction(() => Handle(client, packet.ObjectId, packet.BulletId));
        }

        private static void Handle(Client client, int objectId, byte bulletId)
        {
            var player = client.Player;

            if (player?.Owner == null) return;

            var entity = player.Owner.GetEntity(objectId);

            if (player.Owner.Projectiles.TryGetValue(new Tuple<int, byte>(objectId, bulletId), out Projectile prj))
                player.HitByProjectile(prj);
        }
    }
}
