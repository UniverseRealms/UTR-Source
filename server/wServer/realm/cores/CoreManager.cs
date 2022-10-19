using log4net;
using System;
using System.Linq;

namespace wServer.realm.cores
{
    /// <summary>
    /// This algorithm is responsible to manage all logic actions for
    /// worlds and connected servers of game based on <see cref="CoreType"/>
    /// for each <see cref="cores"/>.
    /// </summary>
    public sealed class CoreManager
    {
        private readonly RealmManager manager;

        private CoreThread[] cores;
        private bool initialized = false;
        private ILog log;

        public CoreManager(RealmManager manager) => this.manager = manager;

        private event EventHandler<Action> packetHandler;

        /// <summary>
        /// Add a pending packet to being executed.
        /// </summary>
        /// <param name="action"></param>
        public void addPendingAction(Action action) => packetHandler.Invoke(this, action);

        /// <summary>
        /// Gets the number of milliseconds elapsed since the game started, but normalized.
        /// </summary>
        /// <returns></returns>
        public int getTickCount() => (int)Program.Uptime.ElapsedTicks;

        /// <summary>
        /// Gets the number of milliseconds elapsed since the game started.
        /// </summary>
        /// <returns></returns>
        public int getTotalTickCount() => (int)Program.Uptime.ElapsedMilliseconds;

        /// <summary>
        /// Initialize <see cref="CoreManager"/>.
        /// </summary>
        public void init()
        {
            log = LogManager.GetLogger(typeof(CoreManager));
            cores = new[] {
                new CoreThread(log, CoreType.Flush, new CoreAction(flushAction), manager, CoreConstant.flushTickMs, false),
                new CoreThread(log, CoreType.Monitor, new CoreAction(monitorAction), manager, CoreConstant.monitorTickMs, false)
            };

            initialized = true;
        }

        /// <summary>
        /// Start all <see cref="cores"/>.
        /// </summary>
        public void start()
        {
            if (!isInitialized()) return;

            log.InfoFormat("Starting {0} core{1}...", cores.Length, cores.Length > 1 ? "s" : "");

            packetHandler += onAddPendingPacket;

            for (var i = 0; i < cores.Length; i++)
                cores[i].start();

            log.Info("All cores have been started!");
        }

        /// <summary>
        /// Stop all <see cref="cores"/>.
        /// </summary>
        /// <param name="disconnect"></param>
        public void stop(bool disconnect = false)
        {
            if (!isInitialized()) return;

            log.InfoFormat("Stopping {0} core{1}...", cores.Length, cores.Length > 1 ? "s" : "");

            for (var i = 0; i < cores.Length; i++)
                cores[i].stop();

            packetHandler -= onAddPendingPacket;

            log.Info("All cores have been stopped!");

            if (disconnect)
                foreach (var client in manager.Clients.Keys)
                    client.Disconnect("Server is restarting.");
        }

        private void flushAction()
        {
            var clients = manager.Clients.Keys.ToArray();

            for (var k = 0; k < clients.Length; k++)
                if (clients[k] != null && clients[k].Player != null && clients[k].Player.Owner != null)
                    clients[k].Player.Flush();
        }

        private bool isInitialized()
        {
            if (!initialized)
            {
                log.Error("RealmCore isn't initialized!");
                return false;
            }

            return true;
        }

        private void monitorAction()
        {
            manager.ConMan.Tick();
            manager.Monitor.Tick();
            manager.InterServer.Tick(CoreConstant.monitorTickMs);
        }

        private void onAddPendingPacket(object sender, Action action)
        {
            try { action.Invoke(); }
            catch { }
        }
    }
}
