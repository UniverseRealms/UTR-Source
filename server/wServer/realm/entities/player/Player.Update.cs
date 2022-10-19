using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wServer.networking.packets.outgoing;
using wServer.realm.terrain;

namespace wServer.realm.entities
{
    public class UpdatedSet// : HashSet<Entity>
    {
        private readonly Player _player;
        private readonly string _changeLock = "UpdatedSet.ChangeLock";
        private ConcurrentDictionary<Entity, object> storage = new ConcurrentDictionary<Entity, object>();
        private readonly ConcurrentQueue<Entity> RemoveQueue = new ConcurrentQueue<Entity>();

        public UpdatedSet(Player player)
        {
            _player = player;
        }

        public bool Add(Entity e)
        {
            if (storage.TryGetValue(e, out object _))
                return false;
            //using (TimedLock.Lock(_changeLock))
            //{
            //if(!storage.Contains(e))
            //{
            e.StatChanged += _player.HandleStatChanges;
            storage.TryAdd(e, new object());
            return true;
            //}
            //return false;

            //}
        }

        public IEnumerable<Entity> GetEntities()
        {
            int count = RemoveQueue.Count;
            //using (TimedLock.Lock(_changeLock))
            //{
            for (int i = 0; i < count; i++)
            {
                RemoveQueue.TryDequeue(out var e);
                if (e == null)
                    continue;
                e.StatChanged -= _player.HandleStatChanges;
                storage.TryRemove(e, out object _);
            }
            //}
            return storage.Keys.ToArray();
        }

        public bool Remove(Entity e)
        {
            RemoveQueue.Enqueue(e);
            //using (TimedLock.Lock(_changeLock))
            //{
            //    e.StatChanged -= _player.HandleStatChanges;
            //    return storage.Remove(e);
            //}
            return true;
        }

        public void RemoveByID(int id)
        {
            //Remove(_player.Owner.GetEntity(id));
            Entity e = _player.Owner.GetEntity(id);
            if (e == null)
                return;
            Remove(e);
            //using (TimedLock.Lock(_changeLock))
            //{
            //    Entity e = _player.Owner.GetEntity(id);
            //    if (e == null)
            //        return;
            //    e.StatChanged -= _player.HandleStatChanges;
            //    storage.Remove(e);
            //}
        }

        //public void RemoveWhere(Predicate<Entity> match)
        //{
        //    using (TimedLock.Lock(_changeLock))
        //    {
        //        foreach (var e in storage.Where(match.Invoke))
        //        {
        //            e.StatChanged -= _player.HandleStatChanges;
        //            storage.Remove(e);
        //        }
        //    }
        //}

        public bool Contains(Entity e)
        {
            return storage.TryGetValue(e, out object _);
        }

        public void Dispose()
        {
            //RemoveWhere(e => true);
            foreach (var val in GetEntities())
            {
                val.StatChanged -= _player.HandleStatChanges;
                storage.TryRemove(val, out object _);
            }
        }
    }

    public partial class Player
    {
        public UpdatedSet clientEntities => _clientEntities;

        public readonly ConcurrentQueue<Entity> ClientKilledEntity = new ConcurrentQueue<Entity>();

        public const int Radius = 20;
        public const int RadiusSqr = Radius * Radius;
        private const int StaticBoundingBox = Radius * 2;
        private const int AppoxAreaOfSight = (int)(Math.PI * Radius * Radius + 1);

        private readonly HashSet<IntPoint> _clientStatic = new HashSet<IntPoint>();
        private readonly UpdatedSet _clientEntities;
        private ObjectStats[] _updateStatuses;
        private Update.TileData[] _tiles;
        private ObjectDef[] _newObjects;
        private int[] _removedObjects;

        private readonly object _statUpdateLock = new object();
        private readonly Dictionary<Entity, Dictionary<StatsType, object>>[] _statUpdates =
            { new Dictionary<Entity, Dictionary<StatsType, object>>(), new Dictionary<Entity, Dictionary<StatsType, object>>() };
        private int currentStatUpdate;
        public Task UpdateTask { get; set; }

        public Sight Sight { get; private set; }

        public int TickId;

        public void HandleStatChanges(object entity, StatChangedEventArgs statChange)
        {
            if (!(entity is Entity e) || (e != this && statChange.UpdateSelfOnly))
                return;

            HandleStatUpdate(e, statChange);

            //using (TimedLock.Lock(_statUpdateLock))
            //{
            //    if (e == this && statChange.Stat == StatsType.None)
            //        return;

            //    if (!_statUpdates[currentStatUpdate].ContainsKey(e))
            //        _statUpdates[currentStatUpdate][e] = new Dictionary<StatsType, object>();

            //    if (statChange.Stat != StatsType.None)
            //        _statUpdates[currentStatUpdate][e][statChange.Stat] = statChange.Value;

            //    //Log.Info($"{entity} {statChange.Stat} {statChange.Value}");
            //}
        }

        private void SendNewTick()
        {
            //int oldUpdate = -1;
            //using (TimedLock.Lock(_statUpdateLock))
            //{
            //    oldUpdate = currentStatUpdate;
            //    currentStatUpdate = currentStatUpdate == 1 ? 0 : 1;
            //}

            //_updateStatuses = _statUpdates[oldUpdate].Select(_ => new ObjectStats()
            //{
            //    Id = _.Key.Id,
            //    Position = new Position() { X = _.Key.RealX, Y = _.Key.RealY },
            //    Stats = _.Value.ToArray()
            //}).ToArray();
            //System.Console.WriteLine("Sending tick [{TickID}]");
            //_statUpdates[oldUpdate].Clear();

            //_client.SendPacket(new NewTick
            //{
            //    TickId = ++TickId,
            //    TickTime = time.ElapsedMsDelta,
            //    Statuses = _updateStatuses
            //});
            _client.SendPacket(PrepareNewTick());
            AwaitMove(TickId);
        }

        private void SendUpdate()
        {
            
            if(Owner == null || Owner.Deleted || this == null)// If the Players owner isnt a current world it is safe to leave the method
                return;

            // init sight circle
            var sCircle = Sight.GetSightCircle(Owner.Blocking);

            // get list of tiles for update
            var tilesUpdate = new List<Update.TileData>(AppoxAreaOfSight);

            foreach (var point in sCircle)
            {
                var x = point.X;
                var y = point.Y;
                if (this == null || Owner == null || Owner.Deleted)
                {
                    return;
                }
                var tile = Owner.Map[x, y];

                if (tile.TileId == 255 ||
                    tiles[x, y] == tile.UpdateCount)
                    continue;

                tilesUpdate.Add(new Update.TileData()
                {
                    X = (short)x,
                    Y = (short)y,
                    Tile = (Tile)tile.TileId
                });
                tiles[x, y] = tile.UpdateCount;
            }
            FameCounter.TileSent(tilesUpdate.Count);

            // get list of new static objects to add
            var staticsUpdate = GetNewStatics(sCircle).ToArray();

            // get dropped entities list
            var entitiesRemove = new HashSet<int>(GetRemovedEntities(sCircle));

            // removed stale entities
            //_clientEntities.RemoveWhere(e => entitiesRemove.Contains(e.Id));
            var killedEntities = GetKilledEntities().ToArray();

            foreach (var entityID in entitiesRemove)
            {
                _clientEntities.RemoveByID(entityID);
            }


            // get list of added entities
            var entitiesAdd = GetNewEntities(sCircle).ToArray();

            // get dropped statics list
            
            var staticsRemove = new HashSet<IntPoint>(GetRemovedStatics(sCircle));
            _clientStatic.ExceptWith(staticsRemove);

            if (tilesUpdate.Count > 0 || entitiesRemove.Count > 0 || staticsRemove.Count > 0 ||
                entitiesAdd.Length > 0 || staticsUpdate.Length > 0)
            {
                entitiesRemove.UnionWith(
                    staticsRemove.Select(s => Owner.Map[s.X, s.Y].ObjId));

                _tiles = tilesUpdate.ToArray();
                _newObjects = entitiesAdd.Select(_ => _.ToDefinition()).Concat(staticsUpdate).ToArray();
                _removedObjects = entitiesRemove.ToArray();
                _client.SendPacket(new Update
                {
                    Tiles = _tiles,
                    NewObjs = _newObjects,
                    Drops = _removedObjects,
                });
                AwaitUpdateAck(Manager.Core.getTotalTickCount());
            }
        }

        private IEnumerable<int> GetKilledEntities()
        {
            foreach (var e in ClientKilledEntity)
            {
                if (e is Enemy test && test.HP < 0 && MathsUtils.DistSqr(test.X, test.Y, this.X, this.Y) < RadiusSqr && DamagedEnemy(test))
                    yield return e.Id;
                else if (e is Player p && p.HP < 0 && MathsUtils.DistSqr(p.X, p.Y, this.X, this.Y) < RadiusSqr)
                    yield return p.Id;
            }
        }

        private bool DamagedEnemy(Enemy test)
        {
            return (from damageValue
                    in test.DamageCounter.GetPlayerData()
                    where damageValue.Item1 == this
                    select damageValue).Count() != 0;
        }

        private IEnumerable<int> GetRemovedEntities(HashSet<IntPoint> visibleTiles)
        {
            foreach (var e in ClientKilledEntity)
                yield return e.Id;
            var temp = _clientEntities.GetEntities();
            //System.Console.WriteLine(temp.Count());
            foreach (var i in temp)
            {
                if (i.Owner == null)
                    yield return i.Id;

                if (i != this && !i.CanBeSeenBy(this))
                    yield return i.Id;

                var so = i as StaticObject;
                if (so != null && so.Static)
                {
                    if (Math.Abs(StaticBoundingBox - ((int)X - i.X)) > 0 &&
                        Math.Abs(StaticBoundingBox - ((int)Y - i.Y)) > 0)
                        continue;
                }

                if (i is Player ||
                    i == Quest || i == SpectateTarget || /*(i is StaticObject && (i as StaticObject).Static) ||*/
                    visibleTiles.Contains(new IntPoint((int)i.X, (int)i.Y)))
                    continue;

                yield return i.Id;
            }
        }

        private IEnumerable<Entity> GetNewEntities(HashSet<IntPoint> visibleTiles)
        {
            Entity entity;
            while (ClientKilledEntity.TryDequeue(out entity))
                _clientEntities.Remove(entity);

            if (Owner != null)
            {
                foreach (var i in Owner.Players)
                    if ((i.Value == this || (i.Value.Client.Account != null && i.Value.Client.Player.CanBeSeenBy(this))) && _clientEntities.Add(i.Value))
                        yield return i.Value;
                        
                var p = new IntPoint(0, 0);
                foreach (Entity i in Owner.PlayersCollision.HitTest(X, Y, Radius))
                {
                    if (i is Ally)
                    {
                        p.X = (int)i.X;
                        p.Y = (int)i.Y;
                        if (visibleTiles.Contains(p) && _clientEntities.Add(i))
                            yield return i;
                    }
                }

                foreach (var i in Owner.EnemiesCollision.HitTest(X, Y, Radius))
                {
                    if (i is Container)
                    {
                        int[] owners = (i as Container).BagOwners;
                        if (owners.Length > 0 && Array.IndexOf(owners, AccountId) == -1)
                            continue;
                    }

                    p.X = (int)i.X;
                    p.Y = (int)i.Y;
                    if (visibleTiles.Contains(p) && _clientEntities.Add(i))
                        yield return i;
                }
            }

            if (Quest?.Owner != null && _clientEntities.Add(Quest))
                yield return Quest;

            if (SpectateTarget?.Owner != null && _clientEntities.Add(SpectateTarget))
                yield return SpectateTarget;
        }

        private IEnumerable<IntPoint> GetRemovedStatics(HashSet<IntPoint> visibleTiles)
        {

            foreach (var i in _clientStatic)
            {
                var tile = Owner.Map[i.X, i.Y];

                if (/*visibleTiles.Contains(i)*/
                    StaticBoundingBox - ((int)X - i.X) > 0 &&
                    StaticBoundingBox - ((int)Y - i.Y) > 0 &&
                    tile.ObjType != 0 &&
                    tile.ObjId != 0)
                    continue;

                yield return i;
            }
        }

        private readonly List<ObjectDef> _newStatics = new List<ObjectDef>(AppoxAreaOfSight);
        private IEnumerable<ObjectDef> GetNewStatics(HashSet<IntPoint> visibleTiles)
        {
            _newStatics.Clear();

            foreach (var i in visibleTiles)
            {
                var x = i.X;
                var y = i.Y;
                var tile = Owner.Map[x, y];

                if (tile.ObjId != 0 && tile.ObjType != 0 && _clientStatic.Add(i))
                    _newStatics.Add(tile.ToDef(x, y));
            }

            return _newStatics;
        }
    }
}
