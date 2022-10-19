using log4net;
using System.Threading;

namespace wServer.realm.cores
{
    /// <summary>
    /// Represents a core thread according <see cref="CoreType"/>.
    /// </summary>
    public sealed class CoreThread
    {
        private readonly CoreAction action;
        private readonly CoreType coreType;
        private readonly int delay;
        private readonly ILog log;
        private readonly RealmManager manager;
        private readonly ManualResetEvent resetEvent;
        private readonly Thread thread;

        public CoreThread(CoreType coreType, CoreAction action, RealmManager manager, int delay, bool isBackground)
            : this(null, coreType, action, manager, delay, isBackground)
        {
        }

        public CoreThread(ILog log, CoreType coreType, CoreAction action, RealmManager manager, int delay, bool isBackground)
            : this(log, coreType, action, manager, delay, isBackground, isBackground ? ThreadPriority.Normal : ThreadPriority.AboveNormal)
        {
        }

        public CoreThread(ILog log, CoreType coreType, CoreAction action, RealmManager manager, int delay, bool isBackground, ThreadPriority priority)
        {
            this.coreType = coreType;
            this.delay = delay;
            this.log = log;
            this.manager = manager;
            this.action = action;

            resetEvent = new ManualResetEvent(false);
            thread = new Thread(new ThreadStart(() =>
            {
                do
                {
                    action.runTask();
                    resetEvent.WaitOne(delay);
                } while (!manager.Terminating || isAlive());
            }))
            {
                IsBackground = isBackground,
                Priority = priority
            };
        }

        public CoreType getCoreType() => coreType;

        public bool isAlive() => thread.IsAlive;

        public void start(bool notifyOnLog = true)
        {
            if (log != null && notifyOnLog) log.InfoFormat("Starting {0} core thread...", coreType);

            thread.Start();
        }

        public void stop(bool notifyOnLog = true)
        {
            if (!thread.IsAlive) return;
            if (log != null && notifyOnLog) log.InfoFormat("Stopping {0} core thread...", coreType);

            thread.Abort();
        }
    }
}
