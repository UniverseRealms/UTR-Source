using Anna.Request;
using common;
using System.Collections.Specialized;

namespace server.account
{
    internal class verifyage : RequestHandler
    {
        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            var status = Database.Verify(query["guid"], query["password"], out DbAccount acc);
            if (status == LoginStatus.OK)
            {
                if (query["isAgeVerified"].Equals("1"))
                    Database.ChangeAgeVerified(acc, true);
                else
                    Database.ChangeAgeVerified(acc, false);

                Write(context, "<Success />");
            }
            else
                Write(context, "<Error>" + status.GetInfo() + "</Error>");
        }
    }
}