using Anna.Request;
using common;
using log4net;
using System.Collections.Specialized;

namespace server.account
{
    internal class registerDiscord : RequestHandler
    {
        private static readonly ILog RankManagerLog = LogManager.GetLogger("RankManagerLog");

        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            var status = Database.Verify(query["guid"], query["password"], out DbAccount acc);
            if (status == LoginStatus.OK)
            {
                if (!acc.RankManager)
                {
                    Write(context, "<Error>No permission</Error>");
                    return;
                }

                var accId = Database.ResolveId(query["ign"]);
                if (accId == 0)
                {
                    Write(context, "<Error>Account does not exist</Error>");
                    return;
                }

                var nAcc = Database.GetAccount(accId);
                if (nAcc.DiscordId != null)
                    Database.UnregisterDiscord(nAcc.DiscordId, accId);

                var dId = query["dId"];
                if (string.IsNullOrEmpty(dId))
                {
                    Write(context, "<Error>Invalid discord id</Error>");
                    return;
                }

                Database.RegisterDiscord(dId, accId);
                Write(context, "<Success/>");
            }
            else
                Write(context, "<Error>" + status.GetInfo() + "</Error>");
        }
    }
}