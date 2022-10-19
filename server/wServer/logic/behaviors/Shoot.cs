using common.resources;
using Mono.Game;
using System;
using System.Linq;
using wServer.networking.packets.outgoing;
using wServer.realm;
using wServer.realm.cores;
using Player = wServer.realm.entities.Player;

namespace wServer.logic.behaviors
{
    internal class Shoot : CycleBehavior
    {
        //State storage: cooldown timer

        private readonly float _radius;
        private readonly int _count;
        private readonly float _shootAngle;
        private readonly float? _fixedAngle;
        private readonly float? _rotateAngle;
        private readonly float _angleOffset;
        private readonly float? _defaultAngle;
        private readonly float _predictive;
        private readonly int _projectileIndex;
        private readonly int _coolDownOffset;
        private Cooldown _coolDown;
        private readonly bool _seeInvis;

        private int _rotateCount;

        public Shoot(
            double radius,
            int count = 1,
            double? shootAngle = null,
            int projectileIndex = 0,
            double? fixedAngle = null,
            double? rotateAngle = null,
            double angleOffset = 0,
            double? defaultAngle = null,
            double predictive = 0,
            int coolDownOffset = 0,
            Cooldown coolDown = new Cooldown(),
            bool seeInvis = false)
        {
            _radius = (float)radius;
            _count = count;
            _shootAngle = count == 1 ? 0 : (float)((shootAngle ?? 360.0 / count) * Math.PI / 180);
            _projectileIndex = projectileIndex;
            _fixedAngle = (float?)(fixedAngle * Math.PI / 180);
            _rotateAngle = (float?)(rotateAngle * Math.PI / 180);
            _angleOffset = (float)(angleOffset * Math.PI / 180);
            _defaultAngle = (float?)(defaultAngle * Math.PI / 180);
            _predictive = (float)predictive;
            _coolDownOffset = coolDownOffset;
            _coolDown = coolDown.Normalize(true);
        }

        protected override void OnStateEntry(Entity host, ref object state)
        {
            state = _coolDownOffset;
        }

        private static float Predict(Entity host, Entity target, ProjectileDesc desc)
        {
            // trying prod prediction

            const int PREDICT_NUM_TICKS = 4; // magic determined by experiement
            var history = target.TryGetHistory(1);

            if (history == null)
            {
                return (float)Math.Atan2(target.Y - host.Y, target.X - host.X);
            }

            var targetX = target.X + PREDICT_NUM_TICKS *
                (target.X - history.Value.X);
            var targetY = target.Y + PREDICT_NUM_TICKS *
                (target.Y - history.Value.Y);

            float angle = (float)Math.Atan2(targetY - host.Y, targetX - host.X);
            Console.WriteLine("Angle:" + angle);
            return angle;
        }

        private static float CollisionTime(Vector2 position, Vector2 relativeV, float bulletV)
        {
            float a = Vector2.Dot(relativeV, relativeV) - bulletV * bulletV;
            float b = 2f * Vector2.Dot(relativeV, position);
            float c = Vector2.Dot(position, position);
            float det = b * b - 4f * a * c;

            if (det > 0f)
                return 2f * c / ((float)Math.Sqrt(det) - b);

            return -1f;
        }

        protected override void TickCore(Entity host, ref object state)
        {
            int track = (int)CoreConstant.worldLogicTickMs;
            int cool = (int?)state - track ?? -1; // <-- crashes server due to state being null... patched now but should be looked at.
            if (host.HasConditionEffect(ConditionEffects.Dazed))
                cool += track / 2;
            Status = CycleStatus.InProgress;

            if (cool <= 0)
            {
                if (host.HasConditionEffect(ConditionEffects.Stunned))
                {
                    cool = _coolDown.Next(Random);
                    Status = CycleStatus.Completed;
                    return;
                }
                var count = _count;

                Entity player;
                if (host.AttackTarget != null)
                    player = host.AttackTarget;
                else
                    player = host.GetNearestEntity(_radius, null, _seeInvis);

                if (player != null || _defaultAngle != null || _fixedAngle != null)
                {
                    if (_projectileIndex >= host.ObjectDesc.Projectiles.Length)
                    {
                        Program.Debug(typeof(Shoot), $"projectile not found, index: {_projectileIndex}, entity: {host.ObjectDesc.ObjectId}");
                        return;
                    }
                    var desc = host.ObjectDesc.Projectiles[_projectileIndex];

                    float a;

                    if (_fixedAngle != null)
                    {
                        a = (float)_fixedAngle;
                    }
                    else if (player != null)
                    {
                        if (_predictive != 0 && _predictive > Random.NextDouble())
                        {
                            a = Predict(host, player, desc);
                        }
                        else
                        {
                            a = (float)Math.Atan2(player.Y - host.Y, player.X - host.X);
                        }
                    }
                    else if (_defaultAngle != null)
                    {
                        a = (float)_defaultAngle;
                    }
                    else
                    {
                        a = 0;
                    }

                    a += _angleOffset + ((_rotateAngle != null) ? (float)_rotateAngle * _rotateCount : 0);
                    _rotateCount++;

                    int dmg = Random.Next(desc.MinDamage, desc.MaxDamage);
                    if (host.HasConditionEffect(ConditionEffects.Weak))
                    {
                        dmg = dmg * 3 / 4;
                    }
                    var startAngle = a - _shootAngle * (count - 1) / 2;
                    byte prjId = 0;
                    Position prjPos = new Position() { X = host.X, Y = host.Y };
                    for (int i = 0; i < count; i++)
                    {
                        var prj = host.CreateProjectile(
                            desc, host.ObjectType, dmg, host.Manager.Core.getTotalTickCount(),
                            prjPos, (startAngle + _shootAngle * i));
                        host.Owner.EnterWorld(prj);

                        if (i == 0)
                            prjId = prj.ProjectileId;
                    }

                    var pkt = new EnemyShoot()
                    {
                        BulletId = prjId,
                        OwnerId = host.Id,
                        StartingPos = prjPos,
                        Angle = startAngle,
                        Damage = (short)dmg,
                        BulletType = (byte)(desc.BulletType),
                        AngleInc = _shootAngle,
                        NumShots = (byte)count,
                    };
                    foreach (var plr in host.Owner.Players.Values
                        .Where(p => p.DistSqr(host) < Player.RadiusSqr))
                    {
                        plr.Client.SendPacket(pkt);
                    }
                }
                cool = _coolDown.Next(Random);
                Status = CycleStatus.Completed;
            }

            state = cool;
        }
    }
}
