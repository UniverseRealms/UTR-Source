using common.resources;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.realm.entities;

namespace wServer.networking.handlers
{
    internal class EnemyHitHandler : PacketHandlerBase<EnemyHit>
    {
        public override PacketId ID => PacketId.ENEMYHIT;

        protected override void HandlePacket(Client client, EnemyHit packet)
        {
            client.Manager.Core.addPendingAction(() => Handle(client.Player, packet));
        }

        private static void Handle(Player player, EnemyHit pkt)
        {
            var entity = player?.Owner?.GetEntity(pkt.TargetId);

            if (entity?.Owner == null) return;
            if (player.Client.IsLagging || player.HasConditionEffect(ConditionEffects.Hidden)) return;

            var prj = (player as IProjectileOwner).Projectiles[pkt.BulletId];

            if (prj != null) prj.ForceHit(entity);
            if (pkt.Killed) player.ClientKilledEntity.Enqueue(entity);
        }
    }
}
