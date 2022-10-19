using Anna.Request;
using common;
using System;
using System.Collections.Specialized;

namespace server.privateMessage
{
    internal class list : RequestHandler
    {
        private static readonly byte[] NoMessages = "{\"messages\":[]}".ToUtf8Bytes();

        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            try
            {
                if (Database.Verify(query["guid"], query["password"], out DbAccount acc) != LoginStatus.OK)
                {
                    Write(context, NoMessages);
                    return;
                }

                if (acc.Admin && acc.AccountIdOverride != 0)
                    acc = Database.GetAccount(acc.AccountIdOverride);

                var messages = acc.PrivateMessages;
                messages?.PrepareForSend(Database);

                Write(context, messages != null ? messages.ToJson().ToUtf8Bytes() : NoMessages);
            }
            catch (Exception e)
            {
                Program.Log.Warn(e.Message + "\r\n" + e.StackTrace);
                Write(context, NoMessages);
            }
        }
    }
}