using Anna.Request;
using System.Collections.Specialized;

namespace server.app
{
    internal class getServerXmls : RequestHandler
    {
        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            Write(context, Program.Resources.GameData.ZippedXmls, true);
        }
    }
}