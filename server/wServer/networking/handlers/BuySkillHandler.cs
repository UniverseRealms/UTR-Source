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

namespace wServer.networking.handlers
{
    internal class BuySkillHandler : PacketHandlerBase<BuySkill>
    {
        public override PacketId ID => PacketId.BUY_SKILL;

        protected override void HandlePacket(Client client, BuySkill packet)
        {
            
            Handle(client, packet);
        }

        public void BuySkill(Client client, int id)
        {
            client.Player._BoughtSkills[id] = 1;
            client.Player.UsedSkillPoints++;
            client.Player.SaveToCharacter();

            //client.Character.BoughtSkills[id] = 1;
            //client.Character.FlushAsync();
        }

        private void Handle(Client client, BuySkill packet)
        {
            var player = client.Player;//shorter

            if (player.SkillPoints == player.UsedSkillPoints)
            {
                player.SendError("No skill points!");
                return;
            }

            if(player.SkillPoints < player.UsedSkillPoints) //Should be impossible but SkillPoints less than usedSkillPoints 
            {
                player.SendError("Error, ask for staff help\nError Code: ST0");
                return;
            }

            if(player._BoughtSkills[packet.SkillId] == 1)//Check if skill is bought already
            {
                player.SendError("You already own this skill!");
                return;
            }
            //Check for gold skills


            BuySkill(client, packet.SkillId);
        }
    }
}