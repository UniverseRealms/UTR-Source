using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace wServer.networking.server
{
    internal sealed class SocketAsyncEventArgsPool
    {
        // Pool of reusable SocketAsyncEventArgs objects.
        private Stack<SocketAsyncEventArgs> pool;

        // initializes the object pool to the specified size.
        // "capacity" = Maximum number of SocketAsyncEventArgs objects
        internal SocketAsyncEventArgsPool(Int32 capacity)
        {
            pool = new Stack<SocketAsyncEventArgs>(capacity);
        }

        // The number of SocketAsyncEventArgs instances in the pool.
        internal Int32 Count
        {
            get { return pool.Count; }
        }

        // Removes a SocketAsyncEventArgs instance from the pool.
        // returns SocketAsyncEventArgs removed from the pool.
        internal SocketAsyncEventArgs Pop()
        {
            if (Monitor.TryEnter(pool, new TimeSpan(0, 0, 1)))
                try { return pool.Pop(); }
                finally { Monitor.Exit(pool); }

            return default;
        }

        // Add a SocketAsyncEventArg instance to the pool.
        // "item" = SocketAsyncEventArgs instance to add to the pool.
        internal void Push(SocketAsyncEventArgs item)
        {
            if (item == null) throw new ArgumentNullException("Items added to a SocketAsyncEventArgsPool cannot be null");
            if (Monitor.TryEnter(pool, new TimeSpan(0, 0, 1)))
                try { if (!pool.Contains(item)) pool.Push(item); }
                finally { Monitor.Exit(pool); }
        }
    }
}
