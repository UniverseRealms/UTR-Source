using Anna.Request;
using common;
using System.Collections.Specialized;

namespace server.account
{
    internal class purchaseSkin : RequestHandler
    {
        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            var status = Database.Verify(query["guid"], query["password"], out DbAccount acc);
            if (status == LoginStatus.OK)
            {
                // perhaps the checks should be moved into the purchas skin routine...
                var skinType = (ushort)Utils.FromString(query["skinType"]);
                var skinDesc = Program.Resources.GameData.Skins[skinType];
                var classStats = Program.Database.ReadClassStats(acc);

                if (skinDesc.Cost > acc.Onrane)
                {
                    Write(context, "<Error>Failed to purchase skin</Error>");
                    return;
                }
                if (acc.Onrane >= skinDesc.Cost)
                {
                    Program.Database.PurchaseSkin(acc, skinType, skinDesc.Cost);
                }

                Write(context, "<Success />");
            }
            else
                Write(context, "<Error>" + status.GetInfo() + "</Error>");
        }
    }
}