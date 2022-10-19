using Anna.Request;
using common;
using common.resources;
using System.Collections.Specialized;
using System.IO;

namespace server.skillTree
{
    internal class getSkillTree : RequestHandler
    {
        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            var character = Database.LoadCharacter(int.Parse(query["accountId"]), int.Parse(query["charId"]));
            if (character == null)
            {
                Write(context, "<Error>Invalid character</Error>");
                return;
            }
            Write(context, character.BoughtSkills.ToString());

        }
    }
}