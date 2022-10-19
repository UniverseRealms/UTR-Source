using common.resources;
using System;
using System.Linq;
using wServer.networking.packets.outgoing;
using wServer.realm;
using wServer.realm.cores;
using wServer.realm.entities;
using Player = wServer.realm.entities.Player;

namespace wServer.logic.behaviors
{
    internal class SpiralShoot : CycleBehavior
    {
        private class SpiralShootStorage
        {
            public int cool;
            public int shots;
        }

        private readonly float _range;
        private readonly int _count;
        private readonly int? _shotsToRestart;
        private readonly float _shootAngle;
        private readonly float _fixedAngle;
        private readonly float _rotateAngle;
        private readonly int _projectileIndex;
        private readonly int _coolDownOffset;
        private Cooldown _coolDown;
        private Cooldown _delayAfterComplete;

        public SpiralShoot(
            double rotateAngle,
            int shotsToRestart,
            int numShots = 1,
            double? shootAngle = null,
            int projectileIndex = 0,
            double fixedAngle = 0,
            int coolDownOffset = 0,
            double range = 0,
            Cooldown coolDown = new Cooldown(),
            int delayAfterComplete = 0)
        {
            _range = (float)range;
            _count = numShots;
            _shootAngle = (float)((shootAngle ?? 360.0 / numShots) * Math.PI / 180);
            _projectileIndex = projectileIndex;
            _fixedAngle = (float)(fixedAngle * Math.PI / 180);
            _rotateAngle = (float)(rotateAngle * Math.PI / 180);
            _shotsToRestart = shotsToRestart;
            _coolDownOffset = coolDownOffset;
            _coolDown = coolDown.Normalize();
            _delayAfterComplete = delayAfterComplete != 0 ? delayAfterComplete : _coolDown.Normalize();
        }

        protected override void OnStateEntry(Entity host, ref object state)
        {
            state = new SpiralShootStorage() { cool = _coolDownOffset, shots = 0 };
        }

        protected override void TickCore(Entity host, ref object state)
        {
            SpiralShootStorage storage;
            if (state == null)
                storage = new SpiralShootStorage() { cool = _coolDownOffset, shots = 0 };
            else
            {
                storage = (SpiralShootStorage)state;
                int track = (int)CoreConstant.worldLogicTickMs;
                storage.cool -= track;
                if (host.HasConditionEffect(ConditionEffects.Dazed))
                    storage.cool += track / 2;
            }

            Status = CycleStatus.InProgress;

            if (storage.cool <= 0)
            {
                if (host.HasConditionEffect(ConditionEffects.Stunned))
                    return;

                var count = _count;

                if (_range == 0 || host.GetNearestEntity(_range, null) != null)
                {
                    var desc = host.ObjectDesc.Projectiles[_projectileIndex];

                    float a = _fixedAngle;

                    a += _rotateAngle * storage.shots;
                    storage.shots++;

                    int dmg = Random.Next(desc.MinDamage, desc.MaxDamage);
                    if (host.HasConditionEffect(ConditionEffects.Weak))
                    {
                        dmg = dmg * 3 / 4;
                    }
                    var startAngle = a - _shootAngle * (count - 1) / 2;
                    byte prjId = 0;
                    Position prjPos = new Position() { X = host.X, Y = host.Y };
                    var prjs = new Projectile[count];
                    for (int i = 0; i < count; i++)
                    {
                        var prj = host.CreateProjectile(
                            desc, host.ObjectType, dmg, host.Manager.Core.getTotalTickCount(),
                            prjPos, (startAngle + _shootAngle * i));
                        host.Owner.EnterWorld(prj);

                        if (i == 0)
                            prjId = prj.ProjectileId;

                        prjs[i] = prj;
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

                if (storage.shots >= _shotsToRestart)
                {
                    storage.shots = 0;
                    storage.cool = _delayAfterComplete.Next(Random);
                    Status = CycleStatus.Completed;
                }
                else
                {
                    storage.cool = _coolDown.Next(Random);
                }
            }

            state = storage;
        }
    }
}
