using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.networking.packets.outgoing;
using wServer.realm.cores;

namespace wServer.realm.entities
{
    public partial class Player
    {
        private Dictionary<Entity, Dictionary<StatsType, object>> concurrentDictionary = new Dictionary<Entity, Dictionary<StatsType, object>>();
        private ConcurrentQueue<Tuple<Entity, StatsType, object>> upcomingChanges = new ConcurrentQueue<Tuple<Entity, StatsType, object>>();

        public void HandleStatUpdate(Entity e, StatChangedEventArgs statChange)
        {
            if ((statChange.UpdateSelfOnly && e != this))
                return;
            upcomingChanges.Enqueue(Tuple.Create(e, statChange.Stat, statChange.Value));
        }

        public NewTick PrepareNewTick()
        {

            int count = upcomingChanges.Count;
            for (int i = 0; i < count; i++)
            {
                bool valid = upcomingChanges.TryDequeue(out var tuple);
                if (!valid)
                    throw new InvalidOperationException("Invalid attempt to process NewTick");
                if (!concurrentDictionary.ContainsKey(tuple.Item1))
                {
                    concurrentDictionary.Add(tuple.Item1, new Dictionary<StatsType, object>());
                }
                if (tuple.Item2 != StatsType.None)
                {
                    concurrentDictionary[tuple.Item1][tuple.Item2] = tuple.Item3;
                }
            }


            _updateStatuses = concurrentDictionary.Select(_ => new ObjectStats()
            {
                Id = _.Key.Id,
                Position = new Position() { X = _.Key.RealX, Y = _.Key.RealY },
                Stats = _.Value.ToArray()
            }).ToArray();
            concurrentDictionary.Clear();


            return new NewTick()
            {
                TickId = ++TickId,
                TickTime = (int)CoreConstant.worldTickMs,
                Statuses = _updateStatuses
            };
        }
    }
}
