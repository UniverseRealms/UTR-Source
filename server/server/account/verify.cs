using Anna.Request;
using common;
using System.Collections.Specialized;

namespace server.account
{
    internal class verify : RequestHandler
    {
        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            var status = Database.Verify(query["guid"], query["password"], out DbAccount acc);
            if (status == LoginStatus.OK)
                Write(context, Account.FromDb(acc).ToXml().ToString());
            else
                Write(context, "<Error>" + status.GetInfo() + "</Error>");
        }
    }
}