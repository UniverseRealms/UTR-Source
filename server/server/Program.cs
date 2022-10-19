﻿using Anna;
using Anna.Request;
using common;
using common.resources;
using log4net;
using log4net.Config;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Web;
using wServer;

namespace server
{
    internal class Program
    {
        internal static readonly ILog Log = LogManager.GetLogger("Server");
        internal static readonly ManualResetEvent Shutdown = new ManualResetEvent(false);
        internal static ServerConfig Config;
        internal static Resources Resources;
        internal static Database Database;
        internal static ISManager ISManager;
        internal static ChatManager ChatManager;
        internal static ISControl ISControl;
        internal static LegendSweeper LegendSweeper;
        internal static MarketSweeper MarketSweeper;

        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += LogUnhandledException;

            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.Name = "Entry";

            Config = args.Length > 0 ?
                ServerConfig.ReadFile(args[0]) :
                ServerConfig.ReadFile("server.json");

            Environment.SetEnvironmentVariable("ServerLogFolder", Config.serverSettings.logFolder);
            GlobalContext.Properties["ServerName"] = Config.serverInfo.name;
            GlobalContext.Properties["ServerType"] = Config.serverInfo.type.ToString();
            XmlConfigurator.ConfigureAndWatch(new FileInfo(Config.serverSettings.log4netConfig));

            Resources = new Resources(Config.serverSettings.resourceFolder);
            Database = new Database(Config.dbInfo.host, Config.dbInfo.port, Config.dbInfo.auth, Config.dbInfo.index, Resources);

            RequestHandlers.Initialize(Resources);

            Config.serverInfo.instanceId = Guid.NewGuid().ToString();
            ISManager = new ISManager(Database, Config);
            ISManager.Run();
            ChatManager = new ChatManager(ISManager);
            ISControl = new ISControl(ISManager);
            LegendSweeper = new LegendSweeper(Database);
            LegendSweeper.Run();
            var marketSweeper = new MarketSweeper(Database);
            marketSweeper.Run();

            Console.CancelKeyPress += delegate { Shutdown.Set(); };

            var port = Config.serverInfo.port;
            using (var server = new HttpServer($"http://{Config.serverInfo.bindAddress}:{port}/"))
            {
                foreach (var uri in RequestHandlers.Get.Keys)
                    server.GET(uri).Subscribe(Response);

                foreach (var uri in RequestHandlers.Post.Keys)
                    server.POST(uri).Subscribe(Response);

                Log.Info("Listening at port " + port + "...");

                Shutdown.WaitOne();
            }

            Log.Info("Terminating...");

            ISManager.Dispose();
            Database.Dispose();
            Resources.Dispose();
        }

        public static void Stop()
        {
            Shutdown.Set();
        }

        private static void Response(RequestContext rContext)
        {
            try
            {
                if (rContext.Request.HttpMethod.Equals("GET"))
                {
                    var query = HttpUtility.ParseQueryString(rContext.Request.Url.Query);
                    RequestHandlers.Get[rContext.Request.Url.LocalPath].HandleRequest(rContext, query);
                    return;
                }

                GetBody(rContext.Request, 4096).Subscribe(body =>
                {
                    try
                    {
                        var query = HttpUtility.ParseQueryString(body);
                        RequestHandlers.Post[rContext.Request.Url.LocalPath]
                            .HandleRequest(rContext, query);
                    }
                    catch (Exception e)
                    {
                        OnError(e, rContext);
                    }
                });
            }
            catch (Exception e)
            {
                OnError(e, rContext);
            }
        }

        private static void OnError(Exception e, RequestContext rContext)
        {
            Debug(typeof(Program), e.ToString(), true);

            try
            {
                rContext.Respond("<Error>Internal server error</Error>", 500);
            }
            catch
            {
            }
        }

        private static IObservable<string> GetBody(Request r, int maxContentLength = 50000)
        {
            int bufferSize = maxContentLength;
            if (r.Headers.ContainsKey("Content-Length"))
                bufferSize = Math.Min(maxContentLength, int.Parse(r.Headers["Content-Length"].First()));

            var buffer = new byte[bufferSize];
            return Observable.FromAsyncPattern<byte[], int, int, int>(r.InputStream.BeginRead, r.InputStream.EndRead)(buffer, 0, bufferSize)
                .Select(bytesRead => r.ContentEncoding.GetString(buffer, 0, bytesRead));
        }

        private static void LogUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Debug(typeof(Program), args.ExceptionObject.ToString(), fatal: true);
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