using Anna.Request;
using common;
using common.resources;
using System.Collections.Specialized;
using System.Text;

namespace server.app
{
    internal class getToken : RequestHandler
    {
        private const string _token = "45a5d02d796b22f1ca08d14b82e1e8d3";
        private static byte[] _data;

        public override void InitHandler(Resources resources)
        {
            _data = Utils.Deflate(Encoding.UTF8.GetBytes(_token));
        }

        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            Write(context, _data, true);
        }
    }
}