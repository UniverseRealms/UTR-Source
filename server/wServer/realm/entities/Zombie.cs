using common.resources;
using Mono.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using wServer.networking.packets;
using wServer.networking.packets.outgoing;
using wServer.realm.cores;

namespace wServer.realm.entities
{
    internal class Zombie : Ally
    {
        private static Random rand = new Random();

        private Player player;
        private int duration;

        public Zombie(Player player, int duration)
            : base(player.Manager, (ushort)0x784a, true)
        {
            this.player = player;
            this.duration = duration;

            this.SetPlayerOwner(player); //Set Owner
        }
        public override void Tick()
        {
            base.Tick();
        }
    }
}
