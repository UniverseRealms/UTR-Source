using common.resources;
using System;
using System.Collections.Generic;
using System.Linq;
using wServer.networking.packets.outgoing;
using wServer.realm;
using wServer.realm.cores;
using wServer.realm.entities;
using wServer.realm.terrain;

namespace wServer.logic.behaviors
{
    internal class TossObject : Behavior
    {
        //State storage: cooldown timer

        private readonly double _range;
        private readonly double? _angle;
        private Cooldown _coolDown;
        private readonly int _coolDownOffset;
        private readonly int _count;
        private readonly double _angleOffset;
        private readonly double _probability;
        private readonly ushort[] _children;
        private readonly double? _minRange;
        private readonly double? _maxRange;
        private readonly double? _minAngle;
        private readonly double? _maxAngle;
        private readonly double? _densityRange;
        private readonly int? _maxDensity;
        private readonly string _group;
        private readonly TileRegion _region;
        private readonly double _regionRange;
        private List<IntPoint> _reproduceRegions;

        public TossObject(string child, double range = 5, double? angle = null,
            Cooldown coolDown = new Cooldown(), int coolDownOffset = 0,
            double probability = 1, string group = null,
            double? minAngle = null, double? maxAngle = null,
            double? minRange = null, double? maxRange = null,
            double? densityRange = null, int? maxDensity = null,
            TileRegion region = TileRegion.None, double regionRange = 10,
            int count = 1, double angleOffset = 0
            )
        {
            if (group == null)
                _children = new ushort[] { GetObjType(child) };
            else
                _children = BehaviorDb.InitGameData.ObjectDescs.Values
                .Where(x => x.Group == group)
                .Select(x => x.ObjectType).ToArray();

            _range = range;
            _angle = angle * Math.PI / 180;
            _coolDown = coolDown.Normalize();
            _coolDownOffset = coolDownOffset;
            _count = count;
            _angleOffset = angleOffset * Math.PI / 180;
            _probability = probability;
            _minRange = minRange;
            _maxRange = maxRange;
            _minAngle = minAngle * Math.PI / 180;
            _maxAngle = maxAngle * Math.PI / 180;
            _densityRange = densityRange;
            _maxDensity = maxDensity;
            _group = group;
            _region = region;
            _regionRange = regionRange;
        }

        protected override void OnStateEntry(Entity host, ref object state)
        {
            state = _coolDownOffset;

            if (_region == TileRegion.None)
                return;

            var map = host.Owner.Map;

            var w = map.Width;
            var h = map.Height;

            _reproduceRegions = new List<IntPoint>();

            for (var y = 0; y < h; y++)
                for (var x = 0; x < w; x++)
                {
                    if (map[x, y].Region != _region)
                        continue;

                    _reproduceRegions.Add(new IntPoint(x, y));
                }
        }

        protected override void TickCore(Entity host, ref object state)
        {
            int cool = (int)state - (int)CoreConstant.worldLogicTickMs;

            if (cool <= 0)
            {
                if (host.HasConditionEffect(ConditionEffects.Stunned))
                    return;

                if (Random.NextDouble() > _probability)
                {
                    state = _coolDown.Next(Random);
                    return;
                }

                Entity player = null;
                if (_angle == null)
                    player = host.GetNearestEntity(_range, null);

                if (player != null || _angle != null)
                {
                    if (_densityRange != null && _maxDensity != null)
                    {
                        var cnt = 0;
                        if (_children.Length > 1)
                            cnt = host.CountEntity((double)_densityRange, _group);
                        else
                        {
                            cnt = host.CountEntity((double)_densityRange, _children[0]);
                        }

                        if (cnt >= _maxDensity)
                        {
                            state = _coolDown.Next(Random);
                            return;
                        }
                    }

                    double r;
                    if (_minRange != null && _maxRange != null)
                        r = _minRange.Value + Random.NextDouble() * (_maxRange.Value - _minRange.Value);
                    else
                        r = (player != null ? host.Dist(player) : _range);

                    double? a;
                    if (_angle == null && _minAngle != null && _maxAngle != null)
                        a = _minAngle.Value + Random.NextDouble() * (_maxAngle.Value - _minAngle.Value);
                    else
                        a = _angle ?? Math.Atan2(host.Y - player.Y, player.X - host.X);

                    a -= _angleOffset * (_count - 1) / 2;
                    for (int i = 0; i < _count; i++)
                    {
                        Position target;
                        if (a != null)
                        {
                            target = new Position()
                            {
                                X = host.X + (float)(r * Math.Cos(a.Value)),
                                Y = host.Y - (float)(r * Math.Sin(a.Value)),
                            };
                            a += _angleOffset;
                        }
                        else
                            target = new Position()
                            {
                                X = player.X,
                                Y = player.Y,
                            };

                        if (_reproduceRegions != null && _reproduceRegions.Count > 0)
                        {
                            var sx = (int)host.X;
                            var sy = (int)host.Y;
                            var regions = _reproduceRegions
                                .Where(p => Math.Abs(sx - p.X) <= _regionRange &&
                                            Math.Abs(sy - p.Y) <= _regionRange).ToList();
                            var tile = regions[Random.Next(regions.Count)];
                            target = new Position()
                            {
                                X = tile.X,
                                Y = tile.Y
                            };
                        }

                        host.Owner.BroadcastPacketNearby(new ShowEffect()
                        {
                            EffectType = EffectType.Throw,
                            Color = new ARGB(0xffffbf00),
                            TargetObjectId = host.Id,
                            Pos1 = target
                        }, target);
                        host.Owner.Timers.Add(new WorldTimer(1500, (w) =>
                        {
                            if (w == null || w.Deleted || host == null) return;
                            if (!w.IsPassable(target.X, target.Y, true)) return;

                            var entity = Entity.Resolve(host.Manager, _children[Random.Next(_children.Length)]);
                            entity.Move(target.X, target.Y);
                            entity.GivesNoXp = true;

                            if (host.Spawned) entity.Spawned = true;

                            var enemyEntity = entity as Enemy;

                            if (host is Enemy enemyHost && enemyEntity != null)
                            {
                                enemyEntity.Terrain = enemyHost.Terrain;

                            }

                            w.EnterWorld(entity);
                        }));
                    }
                    cool = _coolDown.Next(Random);
                }
            }

            state = cool;
        }
    }
}
