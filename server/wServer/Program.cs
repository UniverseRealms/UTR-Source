using common;
using common.resources;
using log4net;
using log4net.Config;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using wServer.networking;
using wServer.networking.server;
using wServer.realm;

namespace wServer
{
    internal static class Program
    {
        internal static Stopwatch Uptime;
        internal static ServerConfig Config;
        internal static Resources Resources;
        internal static DiscordLogging DL;
        internal static ClientProtection CP;
        internal static readonly ILog Log = LogManager.GetLogger("wServer");
        internal static int NewItems = 0;

        private static readonly ManualResetEvent Shutdown = new ManualResetEvent(false);

        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += LogUnhandledException;

            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.Name = "Entry";

            Config = args.Length > 0 ?
                ServerConfig.ReadFile(args[0]) :
                ServerConfig.ReadFile("wServer.json");

            DL = new DiscordLogging(Config.serverSettings.log2Discord);
            CP = new ClientProtection(Config.serverSettings.checkClient, Config.serverSettings.checkClientAir, Config.serverSettings.tokens);

            Environment.SetEnvironmentVariable("ServerLogFolder", Config.serverSettings.logFolder);
            GlobalContext.Properties["ServerName"] = "GameServer";
            GlobalContext.Properties["ServerType"] = Config.serverInfo.type.ToString();

            XmlConfigurator.ConfigureAndWatch(new FileInfo(Config.serverSettings.log4netConfig));


            using (Resources = new Resources(Config.serverSettings.resourceFolder, true))
            using (var db = new Database(
                Config.dbInfo.host,
                Config.dbInfo.port,
                Config.dbInfo.auth,
                Config.dbInfo.index,
                Resources))
            {
                Uptime = Stopwatch.StartNew();

                var manager = new RealmManager(Resources, db, Config);
                manager.Run();

                var policy = new PolicyServer();
                policy.Start();

                var server = new Server(manager,
                    Config.serverInfo.port,
                    Config.serverSettings.maxConnections,
                    StringUtils.StringToByteArray(Config.serverSettings.key));
                server.Start();

                Console.CancelKeyPress += delegate { Shutdown.Set(); };

                Shutdown.WaitOne();

                manager.Stop();
                server.Stop();
                policy.Stop();
            }
        }

        public static void Stop()
        {
            Shutdown.Set();
        }

        private static void LogUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            try
            {
                Exception ex = (Exception)args.ExceptionObject;
                Debug(typeof(Program), ex.ToString(), true);
            }
            catch
            {
 
            }
        }

        public static void Debug(Type t, string message, bool error = false, bool fatal = false, bool warn = false)
        {
            if (ServerConfig.EnableDebug)
            {
                ILog log = LogManager.GetLogger(t);
                if (error)
                    log.Error(message);
                else if (fatal)
                    log.Fatal(message);
                else if (warn)
                    log.Warn(message);
                else
                    log.Info(message);
            }
        }
    }
}
