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
    internal class UsePotionHandler : PacketHandlerBase<UsePotion>
    {
        public override PacketId ID => PacketId.USE_POTION;

        protected override void HandlePacket(Client client, UsePotion packet)
        {
            Handle(client, packet);
        }

        public void UsePotion(Client client, int Stat, int Amount)
        {
            var player = client.Player;
            var statInfo = player.Manager.Resources.GameData.Classes[player.ObjectType].Stats;
            var gameData = client.Player.Manager.Resources.GameData;

            if (client.Player.Rank < 20)
            {
                if (client.Account.PotionStorage == 0)
                {
                    player.SendError("You need to unlock the potion storage license first, can be found bottem of the nexus.");
                    return;
                }
            }
            if (player.Stats.Base[Stat] >= statInfo[Stat].MaxValue)
            {
                player.SendInfo("You have already been maxed on this stat!");
                return;
            }
            else if (player.Stats.Base[Stat] >= statInfo[Stat].MaxValue)
            {
                player.SendInfo("You have already been maxed on this stat!");
                return;
            }
            else if (Stat == 0)
            {
                if (client.Account.StoredLife >= 1)
                {
                    client.Account.StoredLife = client.Account.StoredLife -= 1;
                    client.Player.ForceUpdate(client.Account.StoredLife);
                    player.Stats.Base[Stat] += Amount;
                    client.Player.SendInfo("Consumed! You now have [" + client.Account.StoredLife + "] life potions left in your potion storage!");
                    return;
                }
                else if (client.Account.StoredLife < 1)
                {
                    client.Player.SendError("You do not have enough to consume a potion!");
                    return;
                }
            }
            else if (Stat == 1)
            {
                if (client.Account.StoredMana >= 1)
                {
                    client.Account.StoredMana = client.Account.StoredMana -= 1;
                    client.Player.ForceUpdate(client.Account.StoredMana);
                    player.Stats.Base[Stat] += Amount;
                    client.Player.SendInfo("Consumed! You now have [" + client.Account.StoredMana + "] mana potions left in your potion storage!");
                    return;
                }
                else if (client.Account.StoredMana < 1)
                {
                    client.Player.SendError("You do not have enough to consume a potion!");
                    return;
                }
            }
            else if (Stat == 2)
            {
                if (client.Account.StoredAttack >= 1)
                {
                    client.Account.StoredAttack = client.Account.StoredAttack -= 1;
                    client.Player.ForceUpdate(client.Account.StoredAttack);
                    player.Stats.Base[Stat] += Amount;
                    client.Player.SendInfo("Consumed! You now have [" + client.Account.StoredAttack + "] attack potions left in your potion storage!");
                    return;
                }
                else if (client.Account.StoredAttack < 1)
                {
                    client.Player.SendError("You do not have enough to consume a potion!");
                    return;
                }
            }
            else if (Stat == 3)
            {
                if (client.Account.StoredDefense >= 1)
                {
                    client.Account.StoredDefense = client.Account.StoredDefense -= 1;
                    client.Player.ForceUpdate(client.Account.StoredDefense);
                    player.Stats.Base[Stat] += Amount;
                    client.Player.SendInfo("Consumed! You now have [" + client.Account.StoredDefense + "] defense potions left in your potion storage!");
                    return;
                }
                else if (client.Account.StoredDefense < 1)
                {
                    client.Player.SendError("You do not have enough to consume a potion!");
                    return;
                }
            }
            else if (Stat == 4)
            {
                if (client.Account.StoredSpeed >= 1)
                {
                    client.Account.StoredSpeed = client.Account.StoredSpeed -= 1;
                    client.Player.ForceUpdate(client.Account.StoredSpeed);
                    player.Stats.Base[Stat] += Amount;
                    client.Player.SendInfo("Consumed! You now have [" + client.Account.StoredSpeed + "] speed potions left in your potion storage!");
                    return;
                }
                else if (client.Account.StoredSpeed < 1)
                {
                    client.Player.SendError("You do not have enough to consume a potion!");
                    return;
                }
            }
            else if (Stat == 5)
            {
                if (client.Account.StoredDexterity >= 1)
                {
                    client.Account.StoredDexterity = client.Account.StoredDexterity -= 1;
                    client.Player.ForceUpdate(client.Account.StoredDexterity);
                    player.Stats.Base[Stat] += Amount;
                    client.Player.SendInfo("Consumed!");
                    return;
                }
                else if (client.Account.StoredDexterity < 1)
                {
                    client.Player.SendError("Consumed! You now have [" + client.Account.StoredDexterity + "] dexterity potions left in your potion storage!");
                    return;
                }
            }
            else if (Stat == 6)
            {
                if (client.Account.StoredVitality >= 1)
                {
                    client.Account.StoredVitality = client.Account.StoredVitality -= 1;
                    client.Player.ForceUpdate(client.Account.StoredVitality);
                    player.Stats.Base[Stat] += Amount;
                    client.Player.SendInfo("Consumed!");
                    return;
                }
                else if (client.Account.StoredVitality < 1)
                {
                    client.Player.SendError("Consumed! You now have [" + client.Account.StoredVitality + "] vitality potions left in your potion storage!");
                    return;
                }
            }
            else if (Stat == 7)
            {
                if (client.Account.StoredWisdom >= 1)
                {
                    client.Account.StoredWisdom = client.Account.StoredWisdom -= 1;
                    client.Player.ForceUpdate(client.Account.StoredWisdom);
                    player.Stats.Base[Stat] += Amount;
                    client.Player.SendInfo("Consumed! You now have [" + client.Account.StoredWisdom + "] wisdom potions left in your potion storage!");
                    return;
                }
                else if (client.Account.StoredWisdom < 1)
                {
                    client.Player.SendError("You do not have enough to consume a potion!");
                    return;

                }
            }
            else if (Stat == 8)
            {
                if (client.Account.StoredRestoration >= 1)
                {
                    client.Account.StoredRestoration = client.Account.StoredRestoration -= 1;
                    client.Player.ForceUpdate(client.Account.StoredRestoration);
                    player.Stats.Base[Stat] += Amount;
                    client.Player.SendInfo("Consumed! You now have [" + client.Account.StoredRestoration + "] restoration potions left in your potion storage!");
                    return;
                }
                else if (client.Account.StoredRestoration < 1)
                {
                    client.Player.SendError("You do not have enough to consume a potion!");
                    return;
                }
            }
            else if (Stat == 9)
            {
                if (client.Account.StoredLuck >= 1)
                {
                    client.Account.StoredLuck = client.Account.StoredLuck -= 1;
                    client.Player.ForceUpdate(client.Account.StoredLuck);
                    player.Stats.Base[Stat] += Amount;
                    client.Player.SendInfo("Consumed! You now have [" + client.Account.StoredLuck + "] luck potions left in your potion storage!");
                    return;
                }
                else if (client.Account.StoredLuck < 1)
                {
                    client.Player.SendError("You do not have enough to consume a potion!");
                    return;
                }
            }
        }

        private void Handle(Client client, UsePotion packet)
        {
            switch (packet.ItemId)
            {
                case 1:
                    UsePotion(client, 2, 1);
                    break;
                case 2:
                    UsePotion(client, 3, 1);
                    break;
                case 3:
                    UsePotion(client, 4, 1);
                    break;
                case 4:
                    UsePotion(client, 5, 1);
                    break;
                case 5:
                    UsePotion(client, 6, 1);
                    break;
                case 6:
                    UsePotion(client, 7, 1);
                    break;
                case 7:
                    UsePotion(client, 0, 5);
                    break;
                case 8:
                    UsePotion(client, 1, 5);
                    break;
                case 9:
                    UsePotion(client, 9, 1);
                    break;
                case 10:
                    UsePotion(client, 8, 1);
                    break;
                default:
                    client.Player.SendError("Error.");
                    break;
            }
        }
    }
}