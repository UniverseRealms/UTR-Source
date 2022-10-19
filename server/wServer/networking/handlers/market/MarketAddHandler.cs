using System;
using System.Collections.Generic;
using System.Linq;
using common;
using common.resources;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.networking.packets.incoming.market;
using wServer.networking.packets.outgoing;
using wServer.networking.packets.outgoing.market;
using wServer.realm;
using wServer.realm.entities;

namespace wServer.networking.handlers.market
{
    class MarketAddHandler : PacketHandlerBase<MarketAdd>
    {
        public override PacketId ID => PacketId.MARKET_ADD;

        protected override void HandlePacket(Client client, MarketAdd packet)
        {
            client.Manager.Core.addPendingAction(() =>
            {
                var player = client.Player;
                if (client.Account.Admin)
                {
                    player.SendError("You cannot access the market as an admin.");
                    return;
                }
                if (player == null || IsTest(client))
                {
                    return;
                }

                if (packet.Hours <= 0 || packet.Hours > 72) /* Client only has the options 3, 6, 12 and 24 hours, though if someone wanted they could change that */
                {
                    client.SendPacket(new MarketAddResult
                    {
                        Code = MarketAddResult.INVALID_UPTIME,
                        Description = "Only 1-72 hours uptime allowed."
                    });
                    return;
                }

                for (var i = 0; i < packet.Slots.Length; i++)
                {
                    byte slotId = packet.Slots[i];

                    if (player.Inventory[slotId] == null) /* Make sure they are selling valid items */
                    {
                        client.SendPacket(new MarketAddResult
                        {
                            Code = MarketAddResult.SLOT_IS_NULL,
                            Description = $"The slot {slotId - 4} is empty or invalid."
                        });
                        return;
                    }

                    Item item = player.Inventory[slotId];
                    if (Banned(item)) /* Client has this check, but check it incase it was modified */
                    {
                        client.SendPacket(new MarketAddResult
                        {
                            Code = MarketAddResult.ITEM_IS_SOULBOUND,
                            Description = "You cannot sell banned items."
                        });
                        return;
                    }

                    if (packet.Price <= 0) /* Client has this check, but check it incase it was modified */
                    {
                        client.SendPacket(new MarketAddResult
                        {
                            Code = MarketAddResult.INVALID_PRICE,
                            Description = "You cannot sell items for 0 or less gold."
                        });
                        return;
                    }

                    //if (!Enum.IsDefined(typeof(CurrencyType), packet.Currency) || packet.Currency == (int)CurrencyType.GuildFame) /* Make sure its a valid currency and its NOT GuildFame */
                    //{
                    //    client.SendPacket(new MarketAddResult
                    //    {
                    //        Code = MarketAddResult.INVALID_CURRENCY,
                    //        Description = "Invalid currency."
                    //    });
                    //    return;
                    //} 

                    if(client.Account.MarketOffers.Length > 30)
                    {
                        client.SendPacket(new MarketAddResult
                        {
                            Code = MarketAddResult.INVALID_PRICE,
                            Description = "You have listed too many items, wait for some to sell."
                        });
                        return;
                    }

                    player.Inventory[slotId] = null; /* Set the slot to null */
                    player.Manager.Database.AddMarketData(client.Account, item.ObjectType, player.AccountId, player.Name, packet.Price, DateTime.UtcNow.AddHours(packet.Hours).ToUnixTimestamp(), CurrencyType.Gold); /* Add it to market */
                }

                client.SendPacket(new MarketAddResult
                {
                    Code = -1,
                    Description = $"Successfully added {packet.Slots.Length} items to the market."
                });
            });
        }

        private static bool Banned(Item item) /* What you add here you must add client sided too */
        {
            bool checkBanned = item.Soulbound || (item.Tier >= 0);
            return checkBanned;
        }
    }
}
