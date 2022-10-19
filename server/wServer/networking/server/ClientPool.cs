using System;
using System.Collections.Generic;
using System.Threading;

namespace wServer.networking.server
{
    internal class ClientPool
    {
        private readonly Queue<Client> _pool;

        internal ClientPool(Int32 capacity)
        {
            _pool = new Queue<Client>(capacity);
        }

        internal Int32 Count
        {
            get { return _pool.Count; }
        }

        internal Client Pop()
        {
            if (Monitor.TryEnter(_pool, new TimeSpan(0, 0, 1)))
                try { return _pool.Dequeue(); }
                finally { Monitor.Exit(_pool); }

            return default;
        }

        internal void Push(Client client)
        {
            if (client == null) throw new ArgumentNullException("Clients added to a ClientPool cannot be null");

            if (Monitor.TryEnter(_pool, new TimeSpan(0, 0, 1)))
                try { if (!_pool.Contains(client)) _pool.Enqueue(client); }
                finally { Monitor.Exit(_pool); }
        }
    }
}
