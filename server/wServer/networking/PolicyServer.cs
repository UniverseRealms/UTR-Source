using common;
using log4net;
using System;
using System.Net;
using System.Net.Sockets;

namespace wServer.networking
{
    internal class PolicyServer
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PolicyServer));

        private readonly TcpListener _listener;
        private bool _started;

        public PolicyServer()
        {
            _listener = new TcpListener(IPAddress.Any, 843);
        }

        private static void ServePolicyFile(IAsyncResult ar)
        {
            try
            {
                var cli = (ar.AsyncState as TcpListener).EndAcceptTcpClient(ar);
                (ar.AsyncState as TcpListener).BeginAcceptTcpClient(ServePolicyFile, ar.AsyncState);

                var s = cli.GetStream();
                var rdr = new NReader(s);
                var wtr = new NWriter(s);
                if (rdr.ReadNullTerminatedString() == "<policy-file-request/>")
                {
                    wtr.WriteNullTerminatedString(
                        @"<cross-domain-policy>" +
                        @"<allow-access-from domain=""localhost"" to-ports=""*"" />" +
                        @"<allow-access-from domain=""127.0.0.1"" to-ports=""*"" />" +
                        @"<allow-access-from domain=""dangergun.github.io"" to-ports=""*"" secure=""true"" />" +
                        @"<allow-access-from domain=""162.248.94.239"" to-ports=""*"" secure=""false"" />" +
                        @"</cross-domain-policy>");
                    wtr.Write((byte)'\r');
                    wtr.Write((byte)'\n');
                }
                cli.Close();
            }
            catch { }
        }

        public void Start()
        {
            try
            {
                _listener.Start();
                _listener.BeginAcceptTcpClient(ServePolicyFile, _listener);
                _started = true;
            }
            catch
            {
                Log.Warn("Could not start Socket Policy Server, is port 843 occupied?");
                _started = false;
            }
        }

        public void Stop()
        {
            if (!_started)
                return;

            Log.Warn("Stopping policy server...");
            _listener.Stop();
        }
    }
}
