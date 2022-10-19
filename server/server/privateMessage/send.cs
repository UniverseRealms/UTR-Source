using Anna.Request;
using System.Collections.Specialized;

namespace server.privateMessage
{
    internal class send : RequestHandler
    {
        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            return; //disable pms
            //var recipient = HttpUtility.UrlDecode(query["recipient"]);
            //var subject = HttpUtility.UrlDecode(query["subject"]);
            //var message = HttpUtility.UrlDecode(query["message"]);

            //LoginStatus s;
            //if ((s = Database.Verify(query["guid"], query["password"], out DbAccount acc)) == LoginStatus.OK)
            //{
            //    if (string.Equals(acc.Name, recipient, StringComparison.InvariantCultureIgnoreCase))
            //    {
            //        Write(context, "<Error>Stop sending yourself messages.</Error>");
            //        return;
            //    }

            //    var targetAccount = Database.GetAccount(Database.ResolveId(recipient));
            //    if (targetAccount == null)
            //    {
            //        Write(context, "<Error>Recipient not found. This account does not exist or its an unnamed account, make sure you wrote the name correctly.</Error>");
            //        return;
            //    }

            //    targetAccount.AddPrivateMessage(acc.AccountId, subject, message)
            //        .ContinueWith(t =>
            //        {
            //            Program.ISManager.Publish(Channel.Control, new ControlMsg
            //            {
            //                Type = ControlType.PrivateMessageRefresh,
            //                Payload = recipient
            //            });
            //            Write(context, "Your message has been sent successfully.");
            //        })
            //        .ContinueWith(e =>
            //        {
            //            Program.Debug(typeof(send), e.Exception.InnerExceptions, true);
            //            Write(context, "<Error>Internal server error</Error>");
            //        }, TaskContinuationOptions.OnlyOnFaulted);
            //}
            //else
            //    Write(context, $"<Error>{s.GetInfo()}</Error>");
        }
    }
}