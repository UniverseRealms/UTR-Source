using common;
using System.Linq;
using System.Threading.Tasks;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.vendors
{
    public class PlayerMerchant : Merchant
    {
        public PlayerShopItem PlayerShopItem { get; set; }

        public PlayerMerchant(RealmManager manager, ushort objType)
            : base(manager, objType)
        {
            RankReq = 2;
        }

        public override void Reload()
        {
            if (Reloading)
                return;
            Reloading = true;
            Manager.Market.Reload(this);
            Reloading = false;
        }

        public override void Buy(Player player)
        {
            if (player.Client.Account.Admin || player.Client.Account.Rank >= 80)
            {
                SendFailed(player, BuyResult.Admin);
                return;
            }

            if (BeingPurchased)
            {
                SendFailed(player, BuyResult.BeingPurchased);
                return;
            }
            BeingPurchased = true;

            var result = ValidateCustomer(player);
            if (result != BuyResult.Ok)
            {
                SendFailed(player, result);
                BeingPurchased = false;
                return;
            }

            PurchaseItem(player);
        }

        private async void PurchaseItem(Player player)
        {
            var db = Manager.Database;
            // acquire price, id and seller here so that the wrong price is not sent to seller after update
            var sellerId = PlayerShopItem.AccountId;
            if(player.AccountId == sellerId)
            {
                player.SendError("You can not buy your own item!");
                BeingPurchased = false;
                return;
            }

            if(player.Client.Account.Rank >= 80 && player.Client.Account.Rank < 100)
            {
                player.SendError("Permissions denied!");
                BeingPurchased = false;
                return;
            }

            var seller = Manager.Clients.FirstOrDefault(
                c => c.Key.Account?.AccountId == sellerId).Key;
            //if (seller.Player.Owner.Name == "Vault")
            //{
            //    player.SendError("Can't process your purchase at this time.");
            //    return;
            //}
            var price = PlayerShopItem.Price;
            var type = PlayerShopItem.ItemId;
            var trans = db.Conn.CreateTransaction();
            var t1 = db.UpdateCurrency(player.Client.Account, -Price, Currency, trans);
            db.AddToTreasury(Tax, trans);
            var invTrans = TransactionItem(player, trans);
            Manager.Market.Remove(PlayerShopItem, trans);
            Task t2 = Task.FromResult(0);
            if (seller?.Account != null)
                t2 = Manager.Database.UpdateCurrency(seller.Account, (int)(Price - Price * 0.05), Currency, trans); // added tax here, anything that uses the public "Tax" is not functioning / null.
            else
                Manager.Database.UpdateCurrency(sellerId, (int)(Price - Price * 0.05), Currency, trans); // added tax here, anything that uses the public "Tax" is not functioning / null.
            var t3 = trans.ExecuteAsync();
            await Task.WhenAll(t1, t2, t3);
            var success = !t3.IsCanceled && t3.Result;
            TransactionItemComplete(player, invTrans, success);
            if (success)
            {
                var itemDesc = Manager.Resources.GameData.Items[type];
                Manager.Chat.SendInfo(sellerId, $"Your {itemDesc.DisplayName} has sold for {price} gold.");
                player.Credits = player.Client.Account.Credits;
                if (seller?.Player != null && seller.Account != null)
                    seller.Player.Credits = seller.Account.Credits;
                Reload();
                BeingPurchased = false;
                AwaitingReload = false;
                return;
            }
            BeingPurchased = false;
        }

        protected override void SendNotifications(Player player, bool gift)
        {
            if (gift)
                player.Client.SendPacket(new GlobalNotification
                {
                    Text = "giftChestOccupied"
                });

            player.Client.SendPacket(new networking.packets.outgoing.BuyResult
            {
                Result = 0,
                ResultString = "{\"key\":\"PackagePurchased.message\"}"
            });
        }
    }
}