using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace common
{
    public class ISManager : InterServerChannel, IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ISManager));

        private const int PingPeriod = 2000;
        private const int ServerTimeout = 30000;
        private long _lastPing;

        private readonly ServerConfig _settings;

        private readonly object _dicLock = new object();

        private readonly Dictionary<string, int> _lastUpdateTime = new Dictionary<string, int>();

        private readonly Dictionary<string, ServerInfo> _servers = new Dictionary<string, ServerInfo>();

        private readonly System.Timers.Timer _tmr = new System.Timers.Timer(PingPeriod);

        public EventHandler NewServer;
        public EventHandler ServerQuit;
        public EventHandler ServerPing;

        public ISManager(Database db, ServerConfig settings) : base(db, settings.serverInfo.instanceId)
        {
            _settings = settings;

            // kind of fucked up to do this, but can't really think of another way
            db.SetISManager(this);

            // listen to "network" communications
            AddHandler<NetworkMsg>(Channel.Network, HandleNetwork);

            // tell other servers listening that we've join the network
            Publish(Channel.Network, new NetworkMsg
            {
                Code = NetworkCode.Join,
                Info = _settings.serverInfo
            });
        }

        public void Run()
        {
            _tmr.Elapsed += (sender, e) => Tick(PingPeriod);
            _tmr.Start();
        }

        public void Tick(int elapsedMs)
        {
            if (Monitor.TryEnter(_dicLock, new TimeSpan(0, 0, 1)))
                try
                {
                    // update running time
                    _lastPing += elapsedMs;
                    foreach (var s in _lastUpdateTime.Keys.ToArray())
                        _lastUpdateTime[s] += elapsedMs;

                    if (_lastPing < PingPeriod)
                        return;
                    _lastPing = 0;

                    // notify other servers we're still alive. Update info in the process.
                    Publish(Channel.Network, new NetworkMsg
                    {
                        Code = NetworkCode.Ping,
                        Info = _settings.serverInfo
                    });

                    // check for server timeouts
                    foreach (var s in _lastUpdateTime.Where(s => s.Value > ServerTimeout).ToArray())
                    {
                        var sInfo = _servers[s.Key];
                        RemoveServer(s.Key);

                        // invoke server quit event
                        var networkMsg = new NetworkMsg
                        {
                            Code = NetworkCode.Timeout,
                            Info = sInfo
                        };
                        ServerQuit?.Invoke(this,
                            new InterServerEventArgs<NetworkMsg>(s.Key, networkMsg));
                    }
                }
                finally { Monitor.Exit(_dicLock); }
        }

        public void Dispose()
        {
            Publish(Channel.Network, new NetworkMsg
            {
                Code = NetworkCode.Quit,
                Info = _settings.serverInfo
            });
        }

        private void HandleNetwork(object sender, InterServerEventArgs<NetworkMsg> e)
        {
            if (Monitor.TryEnter(_dicLock, new TimeSpan(0, 0, 1)))
                try
                {
                    switch (e.Content.Code)
                    {
                        case NetworkCode.Join:
                            if (AddServer(e.InstanceId, e.Content.Info))
                            {
                                // make new server aware of this server
                                Publish(Channel.Network, new NetworkMsg
                                {
                                    Code = NetworkCode.Join,
                                    Info = _settings.serverInfo
                                });

                                NewServer?.Invoke(this, e);
                            }
                            else
                            {
                                UpdateServer(e.InstanceId, e.Content.Info);
                            }
                            break;

                        case NetworkCode.Ping:
                            UpdateServer(e.InstanceId, e.Content.Info);
                            ServerPing?.Invoke(this, e);
                            break;

                        case NetworkCode.Quit:
                            RemoveServer(e.InstanceId);
                            ServerQuit?.Invoke(this, e);
                            break;
                    }
                }
                finally { Monitor.Exit(_dicLock); }
        }

        private bool AddServer(string instanceId, ServerInfo info)
        {
            if (_servers.ContainsKey(instanceId))
                return false;

            UpdateServer(instanceId, info);
            return true;
        }

        private void UpdateServer(string instanceId, ServerInfo info)
        {
            _servers[instanceId] = info;
            _lastUpdateTime[instanceId] = 0;
        }

        private void RemoveServer(string instanceId)
        {
            _servers.Remove(instanceId);
            _lastUpdateTime.Remove(instanceId);
        }

        public ServerInfo[] GetServerList()
        {
            if (Monitor.TryEnter(_dicLock, new TimeSpan(0, 0, 1)))
                try { return _servers.Values.ToArray(); }
                finally { Monitor.Exit(_dicLock); }

            return default;
        }

        public string[] GetServerGuids()
        {
            if (Monitor.TryEnter(_dicLock, new TimeSpan(0, 0, 1)))
                try { return _servers.Keys.ToArray(); }
                finally { Monitor.Exit(_dicLock); }

            return default;
        }

        public ServerInfo GetServerInfo(string instanceId)
        {
            if (Monitor.TryEnter(_dicLock, new TimeSpan(0, 0, 1)))
                try { return _servers.ContainsKey(instanceId) ? _servers[instanceId] : null; }
                finally { Monitor.Exit(_dicLock); }

            return default;
        }
    }
}
