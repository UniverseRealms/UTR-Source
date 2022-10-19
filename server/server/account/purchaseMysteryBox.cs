/*using Anna.Request;
using common;
using common.resources;
using StackExchange.Redis;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace server.account
{
    internal class purchaseMysteryBox : RequestHandler
    {
        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            var status = Database.Verify(query["guid"], query["password"], out DbAccount acc);
            if (status != LoginStatus.OK)
            {
                Write(context, $"<Error>{status.GetInfo()}</Error>");
                return;
            }

            // get box
            var box = Program.Resources.MysteryBoxes[query["boxId"].ToInt32()];
            if (box == null)
            {
                Write(context, "<Error>Invalid box</Error>");
                return;
            }

            // purchase box
            var pResult = box.Purchase(Database, acc, out ITransaction tran);
            if (pResult.Result != MBoxPurchaseResult.Success)
            {
                if (pResult.Result == MBoxPurchaseResult.NotEnoughGold)
                    Write(context, "<Error>Not Enough Gold</Error>");
                else if (pResult.Result == MBoxPurchaseResult.NotEnoughFame)
                    Write(context, "<Error>Not Enough Fame</Error>");
                else if (pResult.Result == MBoxPurchaseResult.InvalidCurrency)
                    Write(context, "<Error>Invalid Currency</Error>");
                else
                    Write(context, "<Error>Unknown</Error>");
                return;
            }

            // save gifts
            Database.AddGifts(acc, pResult.Awards, tran);
            tran.ExecuteAsync().ContinueWith(t =>
            {
                var success = !t.IsCanceled && t.Result;
                if (!success)
                {
                    Write(context, "<Error>Transaction Failed</Error>");
                    return;
                }

                Write(context, pResult.Xml);
            }).ContinueWith(e =>
                Program.Debug(typeof(purchaseMysteryBox), e.Exception.InnerException.ToString(), true),
                TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}*/