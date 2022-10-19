using common.resources;
using System;
using System.Linq;
using wServer.networking.packets.outgoing;
using wServer.realm;
using wServer.realm.cores;
using wServer.realm.entities;
using wServer.realm.worlds;
using Player = wServer.realm.entities.Player;

namespace wServer.logic.behaviors
{
    internal class ExplodingShoot : CycleBehavior
    {
        private readonly float _radius;
        private readonly int _count;
        private readonly float _shootAngle;
        private readonly float? _fixedAngle;
        private readonly float _angleOffset;
        private readonly float _predictive;
        private readonly int _projectileIndex;
        private readonly int _coolDownOffset;
        private Cooldown _coolDown;
        private int _explodeIndex;
        private int _explodeCount;
        private float? _fixedAngle2;
        private float? _shootAngle2;

        public ExplodingShoot(
            double radius,
            int count = 1,
            double? shootAngle = null,
            int projectileIndex = 0,
            double? fixedAngle = null,
            double angleOffset = 0,
            double predictive = 0,
            int coolDownOffset = 0,
            Cooldown coolDown = new Cooldown(),
            int explodeIndex = 0,
            int explodeCount = 1,
            float? explodeDirection = null,
            float? explodeAngle = null)
        {
            _radius = (float)radius;
            _count = count;
            _shootAngle = count == 1 ? 0 : (float)((shootAngle ?? 360.0 / count) * Math.PI / 180);
            _projectileIndex = projectileIndex;
            _fixedAngle = (float?)(fixedAngle * Math.PI / 180);
            _angleOffset = (float)(angleOffset * Math.PI / 180);
            _predictive = (float)predictive;
            _coolDownOffset = coolDownOffset;
            _coolDown = coolDown.Normalize();
            _explodeIndex = explodeIndex;
            _explodeCount = explodeCount;
            _fixedAngle2 = (float?)(explodeDirection * Math.PI / 180);
            _shootAngle2 = (float?)(explodeAngle * Math.PI / 180);
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

            if (history == null) return (float)Math.Atan2(target.Y - host.Y, target.X - host.X);

            var targetX = target.X + PREDICT_NUM_TICKS * (target.X - history.Value.X);
            var targetY = target.Y + PREDICT_NUM_TICKS * (target.Y - history.Value.Y);

            float angle = (float)Math.Atan2(targetY - host.Y, targetX - host.X);

            return angle;
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

                if (host.AttackTarget != null) player = host.AttackTarget;
                else player = host.GetNearestEntity(_radius, null);

                if (player != null || _fixedAngle != null)
                {
                    var desc = host.ObjectDesc.Projectiles[_projectileIndex];

                    float a;

                    if (_fixedAngle != null) a = (float)_fixedAngle;
                    else if (player != null)
                    {
                        if (_predictive != 0 && _predictive > Random.NextDouble()) a = Predict(host, player, desc);
                        else a = (float)Math.Atan2(player.Y - host.Y, player.X - host.X);
                    }
                    else a = 0;

                    a += _angleOffset;

                    int dmg = Random.Next(desc.MinDamage, desc.MaxDamage);

                    if (host.HasConditionEffect(ConditionEffects.Weak)) dmg = dmg * 3 / 4;

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

                        host.Owner.Timers.Add(new WorldTimer(prj.ProjDesc.LifetimeMS, (w) =>
                        {
                            if (w == null || w.Deleted || host == null || prj == null) return;

                            Explode(w, host, prj, _explodeCount, _explodeIndex, _fixedAngle2, _shootAngle2);
                        }));
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

                    foreach (var plr in host.Owner.Players.Values.Where(p => p.DistSqr(host) < Player.RadiusSqr))
                        plr.Client.SendPacket(pkt);
                }
                cool = _coolDown.Next(Random);
                Status = CycleStatus.Completed;
            }

            state = cool;
        }

        private static void Explode(World world, Entity host, Projectile prj, int count, int index, float? fixedAngle, float? _shootAngle)
        {
            var shootAngle = (_shootAngle != null ? _shootAngle.Value : (float)(Math.PI * 2 / count));
            var desc = host.ObjectDesc.Projectiles[index];
            Player player = (Player)host.GetNearestEntity(20, null);
            float a;

            if (fixedAngle != null) a = fixedAngle.Value;
            else if (player != null) a = (float)Math.Atan2(player.Y - prj.Y, player.X - prj.X);
            else a = 0;

            a -= shootAngle * (count - 1) / 2;

            int dmg = Random.Next(desc.MinDamage, desc.MaxDamage);

            if (host.HasConditionEffect(ConditionEffects.Weak)) dmg = dmg * 3 / 4;

            byte prjId = 0;
            Position prjPos = prj.GetPosition(prj.ProjDesc.LifetimeMS);

            for (int i = 0; i < count; i++)
            {
                var prj2 = host.CreateProjectile(
                    desc, host.ObjectType, dmg, host.Manager.Core.getTotalTickCount(),
                    prjPos, a + shootAngle * i);

                world.EnterWorld(prj2);

                if (i == 0) prjId = prj2.ProjectileId;
            }

            var pkt = new EnemyShoot()
            {
                BulletId = prjId,
                OwnerId = host.Id,
                StartingPos = prjPos,
                Angle = a,
                Damage = (short)dmg,
                BulletType = (byte)(desc.BulletType),
                AngleInc = shootAngle,
                NumShots = (byte)count,
            };

            foreach (var plr in world.Players.Values.Where(p => p != null && p.DistSqr(prj) < Player.RadiusSqr))
            {
                if (plr.Client == null) continue;

                Program.Debug(typeof(ExplodingShoot), plr.Name);
                plr.Client.SendPacket(pkt);
            }
        }
    }
}
