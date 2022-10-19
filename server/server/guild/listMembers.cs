using Anna.Request;
using common;
using System.Collections.Specialized;

namespace server.guild
{
    internal class listMembers : RequestHandler
    {
        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            var status = Database.Verify(query["guid"], query["password"], out DbAccount acc);
            if (status == LoginStatus.OK)
            {
                if (acc.GuildId <= 0)
                {
                    Write(context, "<Error>Not in guild</Error>");
                    return;
                }

                var guild = Database.GetGuild(acc.GuildId);
                WriteXml(context, Guild.FromDb(Database, guild).ToXml().ToString());
            }
            else
                Write(context, "<Error>" + status.GetInfo() + "</Error>");
        }
    }
}