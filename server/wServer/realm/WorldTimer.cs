using System;
using wServer.realm.cores;
using wServer.realm.worlds;

namespace wServer.realm
{
    public class WorldTimer
    {
        private readonly Action<World> _cb;
        private readonly Func<World, bool> _rcb;
        private readonly int _total;
        private int _remain;

        public WorldTimer(int tickMs, Action<World> callback)
        {
            _remain = _total = tickMs;
            _cb = callback;
        }

        public WorldTimer(int tickMs, Func<World, bool> callback)
        {
            _remain = _total = tickMs;
            _rcb = callback;
        }

        public void Reset()
        {
            _remain = _total;
        }

        public bool? Tick(World world)
        {
            _remain -= (int)CoreConstant.worldTickMs;

            if (world == null) 
                return null;

            if (_remain >= 0) 
                return false;

            if (_cb != null)
            {
                _cb(world);
                return true;
            }

            return _rcb(world);
        }
    }
}
