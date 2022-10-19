using Anna.Request;
using common;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace server.@char
{
    internal class list : RequestHandler
    {
        private static List<ServerItem> GetServerList()
        {
            var ret = new List<ServerItem>();
            foreach (var server in Program.ISManager.GetServerList())
            {
                if (server.type != ServerType.World)
                    continue;

                ret.Add(new ServerItem()
                {
                    Name = server.name,
                    Lat = server.coordinates.latitude,
                    Long = server.coordinates.longitude,
                    Port = server.port,
                    DNS = server.address,
                    Usage = server.players / (float)server.maxPlayers,
                    AdminOnly = server.adminOnly
                });
            }
            return ret;
        }

        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            var status = Database.Verify(query["guid"], query["password"], out DbAccount acc);
            if (status == LoginStatus.OK || status == LoginStatus.AccountNotExists)
            {
                if (status == LoginStatus.AccountNotExists)
                    acc = Database.CreateGuestAccount(query["guid"]);

                if (acc.Admin && acc.AccountIdOverride != 0)
                {
                    var accOverride = Database.GetAccount(acc.AccountIdOverride);
                    if (accOverride != null)
                        acc = accOverride;
                }

                var list = CharList.FromDb(Database, acc);
                list.Servers = GetServerList();
                WriteXml(context, list.ToXml().ToString());
            }
            else
                Write(context, "<Error>" + status.GetInfo() + "</Error>");
        }
    }
}