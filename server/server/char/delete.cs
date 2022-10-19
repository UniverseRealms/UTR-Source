using Anna.Request;
using common;
using System.Collections.Specialized;

namespace server.@char
{
    internal class delete : RequestHandler
    {
        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            var status = Database.Verify(query["guid"], query["password"], out DbAccount acc);
            if (status == LoginStatus.OK)
            {
                using (var l = Database.Lock(acc))
                    if (Database.LockOk(l))
                    {
                        Database.DeleteCharacter(acc, int.Parse(query["charId"]));
                        Write(context, "<Success />");
                    }
                    else
                        Write(context, "<Error>Account in Use</Error>");
            }
            else
                Write(context, "<Error>" + status.GetInfo() + "</Error>");
        }
    }
}