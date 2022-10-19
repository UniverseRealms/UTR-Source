using Anna.Request;
using common;
using common.resources;
using System.Collections.Specialized;
using System.IO;

namespace server.dailyLogin
{
    internal class fetchCalendar : RequestHandler
    {
        private static byte[] _data;

        public override void InitHandler(Resources resources)
        {
            // TODO
            _data = Utils.Deflate(File.ReadAllBytes($"{resources.ResourcePath}/data/loginRewards.xml"));
        }

        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            Write(context.Response(_data), "application/xml", true);
        }
    }
}