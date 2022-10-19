using System;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.realm.entities;

namespace wServer.networking.handlers
{
    internal class AllyHitHandler : PacketHandlerBase<AllyHit>
    {
        public override PacketId ID => PacketId.ALLYHIT;

        protected override void HandlePacket(Client client, AllyHit packet)
        {
            client.Manager.Core.addPendingAction(() => Handle(client, packet));
        }

        private static void Handle(Client c, AllyHit pkt)
        {
            if (c.Player == null || c.Player.Owner == null) return;

            var entity = c.Player.Owner.GetEntity(pkt.TargetId);

            if (!(entity is Ally)) return;

            if (c.Player.Owner.Projectiles.TryGetValue(new Tuple<int, byte>(pkt.ObjectId, pkt.BulletId), out Projectile prj))
                prj.ForceHit(entity);
        }
    }
}
