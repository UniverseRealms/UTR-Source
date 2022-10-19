/*using System;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.realm;
using wServer.realm.entities;
using common.resources;
using System.Xml.Linq;

namespace wServer.networking.handlers
{
    internal class PlayerCheatEngineHandler : PacketHandlerBase<PlayerCheatPacket>
    {
        XElement i;
        public override PacketId ID
        {
            get { return PacketId.PLAYERCHEAT; }
        }

        protected override void HandlePacket(Client client, PlayerCheatPacket packet)
        {
            Handle(client.Player, packet);
        }

        private void Handle(Player player, PlayerCheatPacket packet)
        {
            Item item = player.Inventory[0];
            ProjectileDesc prjDesc = item.Projectiles[0];

            bool CheaterPos =
                (packet.atk_ > player.Stats[2] + 5 || packet.def_ > player.Stats[3] + 5 || packet.spd_ > player.Stats[4] + 5 || packet.vit_ > player.Stats[5] + 5 || packet.wis_ > player.Stats[6] + 5 || packet.dex_ > player.Stats[7] + 5);

            bool CheaterNeg =
                (packet.atk_ < 0 || packet.def_ < -10 || packet.dex_ < 0 || packet.wis_ < 0 || packet.vit_ < 0 || packet.spd_ < 0);
            bool WeaponEdit =
                (packet.mindmg_ > (prjDesc.MinDamage * 1.20) || packet.maxdmg_ > (prjDesc.MaxDamage * 1.20) || packet.firerate_ > (item.RateOfFire + 5));

            if (player.Owner == null || prjDesc == null || item == null)
                return;
            if ((CheaterPos || CheaterNeg || WeaponEdit))
            {
                player.cheatCount++;
                player.Owner.Timers.Add(new WorldTimer(5500, (world, t) =>
                {
                    player.cheatCount--;
                    return;
                }));
            }
        }
    }
}*/