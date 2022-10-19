using Anna.Request;
using common;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Diagnostics;

namespace server.account
{
    internal class register : RequestHandler
    {
        private static Dictionary<string, long> ips = new Dictionary<string, long>();
        private static Stopwatch watch = Stopwatch.StartNew();

        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            if (ips.TryGetValue(context.Request.Url.Host, out long t))
            {
                if (t > watch.ElapsedMilliseconds)
                {
                    Write(context, "<Error>Too many account creation attempts, please try again in 2 minutes.</Error>");
                    ips[context.Request.Url.Host] = watch.ElapsedMilliseconds + 0;//100000;
                    return;
                }
                else
                    ips[context.Request.Url.Host] = watch.ElapsedMilliseconds + 0;//5000;
            }
            else
                ips.Add(context.Request.Url.Host, watch.ElapsedMilliseconds + 0);//5000);

            if (!Utils.IsValidEmail(query["newGUID"]))
                Write(context, "<Error>Invalid email</Error>");
            else
            {
                string key = Database.REG_LOCK;
                string lockToken = null;
                try
                {
                    while ((lockToken = Database.AcquireLock(key)) == null) ;

                    var status = Database.Verify(query["guid"], "", out DbAccount acc);
                    if (status == LoginStatus.OK)
                    {
                        //what? can register in game? kill the account lock
                        if (!Database.RenameUUID(acc, query["newGUID"], lockToken))
                        {
                            Write(context, "<Error>Duplicate Email</Error>");
                            return;
                        }
                        Database.ChangePassword(acc.UUID, query["newPassword"]);
                        Database.Guest(acc, false);
                        Write(context, "<Success />");
                    }
                    else
                    {
                        var s = Database.Register(query["newGUID"], query["newPassword"], false, out acc);
                        if (s == RegisterStatus.OK)
                            Write(context, "<Success />");
                        else
                            Write(context, "<Error>" + s.GetInfo() + "</Error>");
                    }
                }
                finally
                {
                    if (lockToken != null)
                        Database.ReleaseLock(key, lockToken);
                }
            }
        }
    }
}
