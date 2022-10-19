using Anna.Request;
using common;
using common.resources;
using System.Collections.Specialized;
using System.IO;

namespace server.app
{
    internal class globalNews : RequestHandler
    {
        private static byte[] _data;

        public override void InitHandler(Resources resources)
        {
            _data = Utils.Deflate(File.ReadAllBytes($"{resources.ResourcePath}/data/news.txt"));
        }

        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            Write(context, _data, true);
        }
    }
}