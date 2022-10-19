using wServer.networking.packets;
using wServer.networking.packets.incoming;
using System;
using System.Linq;
using wServer.realm.entities;
using wServer.realm.entities.vendors;
using common;
using common.resources;
using Newtonsoft.Json;
using wServer.realm;
using log4net;
using System.Collections.Generic;
using System.IO;
using wServer.networking.packets.outgoing;

namespace wServer.networking.handlers
{
    internal class UpdateTreeHandler : PacketHandlerBase<UpdateTree>
    {
        public override PacketId ID => PacketId.UPDATE_TREE;

        protected override void HandlePacket(Client client, UpdateTree packet)
        {
            Handle(client, packet);
        }
        private void Handle(Client client, UpdateTree packet)
        {
            if (client == null)
                return;

            client.SendPacket(new SkillTree()
            {
                Skilltree = client.Character.BoughtSkills
            });
        }
    }
}