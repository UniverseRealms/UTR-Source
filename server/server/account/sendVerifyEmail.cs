using Anna.Request;
using System.Collections.Specialized;

namespace server.account
{
    internal class sendVerifyEmail : RequestHandler
    {
        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            Write(context, "<Error>Nope.</Error>");
        }
    }
}