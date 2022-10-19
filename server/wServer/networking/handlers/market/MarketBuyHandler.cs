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
    class MarketBuyHandler : PacketHandlerBase<MarketBuy>
    {
        public override PacketId ID => PacketId.MARKET_BUY;

        protected override void HandlePacket(Client client, MarketBuy packet)
        {
            client.Manager.Core.addPendingAction(() =>
            {
                var player = client.Player;
                var acc = client.Account;
                if (acc.Admin)
                {
                    player.SendError("You cannot access the market as an admin.");
                    return;
                }
                if (player == null || IsTest(client))
                {
                    return;
                }

                DbMarketData data = client.Manager.Database.GetMarketData(packet.Id);
                if (data == null) /* Make sure the item exist before buying it */
                {
                    client.SendPacket(new MarketBuyResult
                    {
                        Code = MarketBuyResult.ITEM_DOESNT_EXIST,
                        Description = "Item was taken down or bought."
                    });
                    return;
                }

                if (data.SellerId == player.AccountId) /* If we somehow try to buy our own item */
                {
                    client.SendPacket(new MarketBuyResult
                    {
                        Code = MarketBuyResult.MY_ITEM,
                        Description = "You cannot buy your own item."
                    });
                    return;
                }

                if (acc.Credits < data.Price) /* Make sure we have enough to buy the item */
                {
                    client.SendPacket(new MarketBuyResult
                    {
                        Code = MarketBuyResult.CANT_AFFORD,
                        Description = "You cannot afford this item."
                    });
                    return;
                }

                /* Update the sellers currency */

                var sellerAccount = client.Manager.Database.GetAccount(data.SellerId);

                var trans = client.Manager.Database.Conn.CreateTransaction();

                int oldSellerAmount = sellerAccount.Credits;
                int oldBuyerAmount = acc.Credits;

                int changedSellerAmount = (int)(data.Price * 0.95);

                int mil = changedSellerAmount / 1000000;
                int thou100 = changedSellerAmount % 1000000 / 100000;
                int thou10 = changedSellerAmount % 100000 / 10000;
                int thou = changedSellerAmount % 10000 / 1000;

                Item item = player.Manager.Resources.GameData.Items[data.ItemType];

                client.Manager.Database.UpdateCurrency(acc, -data.Price, CurrencyType.Gold, trans);
                client.Manager.Database.AddGift(acc, data.ItemType, trans);
                client.Manager.Database.RemoveMarketData(sellerAccount, data.Id, trans);

                if (!trans.Execute())
                {
                    client.SendPacket(new MarketBuyResult
                    {
                        Code = MarketBuyResult.COULDNT_BUY_ITEM,
                        Description = "Error occured when purchasing item, please try again."
                    });
                    return;
                }

                for (int i = 1; i <= mil; i++)
                    client.Manager.Database.AddGift(sellerAccount, 0x53c6);
                for (int i = 1; i <= thou100; i++)
                    client.Manager.Database.AddGift(sellerAccount, 0x53c5);
                for (int i = 1; i <= thou10; i++)
                    client.Manager.Database.AddGift(sellerAccount, 0x53c4);
                for (int i = 1; i <= thou; i++)
                    client.Manager.Database.AddGift(sellerAccount, 0x53c3);

                int val = changedSellerAmount % 1000;

                client.Manager.Database.UpdateCredit(sellerAccount, val);


                acc.Reload();
                sellerAccount.Reload();

                player.Credits = acc.Credits;
                client.Manager.Database.RemovePlayerOffer(sellerAccount, data.Id);
                var seller = client.Manager.Clients.Keys.SingleOrDefault(_ => _.Account != null && _.Account.AccountId == data.SellerId);
                if (seller != null)
                {
                    seller.Player.SendInfo($"{player.Name} has just bought your {item.ObjectId} for {data.Price} gold! Check your gift chest for any extra gold!");

                    // Dynamically update his fame if hes online.
                    seller.Player.Credits = sellerAccount.Credits;
                }

                List<string> logLines = new List<string>
                {
                    "------------------------------------------",
                    "Item name: " + item.ObjectId,
                    "Item price:" + data.Price + " (" + changedSellerAmount + ")",
                    "Buyer Account Name: " + acc.Name,
                    "Buyer Account gold before: " + oldBuyerAmount,
                    "Buyer Account gold after: " + acc.Credits,
                    "Account gold before: " + oldSellerAmount,
                    "Account gold after: " + sellerAccount.Credits
                };


                System.IO.File.AppendAllLines("C:\\Users\\Administrator\\Desktop\\Market Logs\\" + sellerAccount.Name + ".txt", logLines);

                client.SendPacket(new MarketBuyResult
                {
                    Code = -1,
                    Description = $"Successfully bought {item.ObjectId} for {data.Price} gold!",
                    OfferId = data.Id /* We send back the ID we bought, so we can remove it from the list */
                });
            });
        }
    }
}