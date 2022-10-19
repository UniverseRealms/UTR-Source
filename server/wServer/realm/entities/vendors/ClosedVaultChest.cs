using common.resources;
using System.Threading.Tasks;
using wServer.realm.worlds.logic;

namespace wServer.realm.entities.vendors
{
    internal class ClosedVaultChest : SellableObject
    {
        public ClosedVaultChest(RealmManager manager, ushort objType) : base(manager, objType)
        {
            Price = 1000000;            //DO NOT MAKE THESE PURCHASABLE UNLESS YOU FIX THE VAULT DUPE
            Currency = CurrencyType.Kantos; //Use the item instead it is safe.
        }

        public override void Buy(Player player)
        {
            var result = ValidateCustomer(player);
            if (result != BuyResult.Ok)
            {
                SendFailed(player, result);
                return;
            }

            var db = Manager.Database;
            var acc = player.Client.Account;

            var trans = db.Conn.CreateTransaction();
            Manager.Database.CreateChest(acc, trans);
            var t1 = db.UpdateCurrency(acc, -Price, Currency, trans);
            var t2 = trans.ExecuteAsync();
            Task.WhenAll(t1, t2).ContinueWith(t =>
            {
                player.Credits -= Price;

                if (t.IsCanceled)
                {
                    SendFailed(player, BuyResult.TransactionFailed);
                    return;
                }

                acc.Reload("vaultCount");

                (Owner as Vault)?.AddChest(this);
                player.Client.SendPacket(new networking.packets.outgoing.BuyResult()
                {
                    Result = 0,
                    ResultString = "Vault chest purchased!"
                });
            }).ContinueWith(e =>
                Program.Debug(typeof(ClosedVaultChest), e.Exception.ToString(), true),
                TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}