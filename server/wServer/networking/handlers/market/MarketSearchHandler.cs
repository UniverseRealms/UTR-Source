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
    class MarketSearchHandler : PacketHandlerBase<MarketSearch>
    {
        public override PacketId ID => PacketId.MARKET_SEARCH;

        protected override void HandlePacket(Client client, MarketSearch packet)
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

                DbMarketData[] query = DbMarketData.Get(player.Manager.Database.Conn, (ushort)packet.ItemType); /* Get all offers based on given type */

                /* There should probably be a max limit of how many items we can send, though since client does all the sorting
                 * its harder to add the limit. Though, if you were to make sorting client sided (prevents the user from choosing sorttypes) 
                 * then yes, you could add a limit */
                List<MarketData> results = new List<MarketData>(); 
                foreach (var i in query)
                {
                    if (i.SellerId == player.AccountId)
                    {
                        continue; /* Dont send our own items */
                    }

                    results.Add(new MarketData
                    {
                        Id = i.Id,
                        ItemType = i.ItemType,
                        SellerName = i.SellerName,
                        SellerId = i.SellerId,
                        Price = i.Price,
                        TimeLeft = i.TimeLeft,
                        StartTime = i.StartTime,
                        Currency = (int)i.Currency
                    });
                }

                if (results.Count == 0) /* No items found */
                {
                    client.SendPacket(new MarketSearchResult
                    {
                        Results = new MarketData[0],
                        Description = "There is no items currently being sold with this type."
                    });
                    return;
                }

                client.SendPacket(new MarketSearchResult
                {
                    Results = results.ToArray(), 
                    Description = "" /* Has to be empty, if not client will count it as an error */
                });
            });
        }
    }
}
