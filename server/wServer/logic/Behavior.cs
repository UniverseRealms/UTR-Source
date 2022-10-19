using common;
using System;
using wServer.realm;

namespace wServer.logic
{
    public abstract class Behavior : IStateChildren
    {
        public void Tick(Entity host)
        {
            if (!host.StateStorage.TryGetValue(this, out var state)) state = null;

            TickCore(host, ref state);

            if (state == null) host.StateStorage.Remove(this);
            else host.StateStorage[this] = state;
        }

        protected abstract void TickCore(Entity host, ref object state);

        public void OnStateEntry(Entity host)
        {
            if (!host.StateStorage.TryGetValue(this, out var state)) state = null;

            OnStateEntry(host, ref state);

            if (state == null) host.StateStorage.Remove(this);
            else host.StateStorage[this] = state;
        }

        protected virtual void OnStateEntry(Entity host, ref object state)
        {
        }

        public void OnStateExit(Entity host)
        {
            if (!host.StateStorage.TryGetValue(this, out var state)) state = null;

            OnStateExit(host, ref state);

            if (state == null) host.StateStorage.Remove(this);
            else host.StateStorage[this] = state;
        }

        protected virtual void OnStateExit(Entity host, ref object state)
        {
        }

        protected internal virtual void Resolve(State parent)
        {
        }

        public static ushort GetObjType(string id)
        {
            if (BehaviorDb.InitGameData.IdToObjectType.TryGetValue(id, out var ret)) return ret;

            ret = BehaviorDb.InitGameData.IdToObjectType["Pirate"];

            Program.Debug(typeof(Behavior), $"Object type '{id}' not found. Using Pirate ({ret.To4Hex()}).", warn: true);

            return ret;
        }

        [ThreadStatic]
        private static Random _rand;

        protected static Random Random => _rand ?? (_rand = new Random());
    }
}
