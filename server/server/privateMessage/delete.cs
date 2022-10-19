using Anna.Request;
using common;
using System.Collections.Specialized;

namespace server.privateMessage
{
    internal class delete : RequestHandler
    {
        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            LoginStatus s;
            if ((s = Database.Verify(query["guid"], query["password"], out DbAccount acc)) == LoginStatus.OK)
                acc.PrivateMessages
                    .DelteMessage(Database, int.Parse(query["time"]))
                    .ContinueWith(t => Write(context, "<Success />"));
            else
                Write(context, $"<Error>{s.GetInfo()}</Error>");
        }
    }
}