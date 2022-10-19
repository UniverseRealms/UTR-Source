using common.resources;
using System;
using System.Linq;
using wServer.networking.packets;
using wServer.networking.packets.outgoing;
using wServer.realm;
using wServer.realm.cores;
using Player = wServer.realm.entities.Player;

namespace wServer.logic.behaviors
{
    internal class AllyShoot : CycleBehavior
    {
        //State storage: cooldown timer

        private readonly float _radius;
        private readonly int _count;
        private readonly float _shootAngle;
        private readonly float? _fixedAngle;
        private readonly float? _rotateAngle;
        private readonly float _angleOffset;
        private readonly float? _defaultAngle;
        private readonly int _projectileIndex;
        private readonly int _coolDownOffset;
        private Cooldown _coolDown;
        private readonly bool _shootLowHp;

        private int _rotateCount;

        public AllyShoot(
            double radius,
            int count = 1,
            double? shootAngle = null,
            int projectileIndex = 0,
            double? fixedAngle = null,
            double? rotateAngle = null,
            double angleOffset = 0,
            double? defaultAngle = null,
            int coolDownOffset = 0,
            Cooldown coolDown = new Cooldown(),
            bool shootLowHp = false)
        {
            _radius = (float)radius;
            _count = count;
            _shootAngle = count == 1 ? 0 : (float)((shootAngle ?? 360.0 / count) * Math.PI / 180);
            _projectileIndex = projectileIndex;
            _fixedAngle = (float?)(fixedAngle * Math.PI / 180);
            _rotateAngle = (float?)(rotateAngle * Math.PI / 180);
            _angleOffset = (float)(angleOffset * Math.PI / 180);
            _defaultAngle = (float?)(defaultAngle * Math.PI / 180);
            _coolDownOffset = coolDownOffset;
            _coolDown = coolDown.Normalize();
            _shootLowHp = shootLowHp;
        }

        protected override void OnStateEntry(Entity host, ref object state)
        {
            state = _coolDownOffset;
        }

        protected override void TickCore(Entity host, ref object state)
        {
            int cool = (int?)state ?? -1; // <-- crashes server due to state being null... patched now but should be looked at.

            Status = CycleStatus.NotStarted;

            if (cool <= 0)
            {
                if (host.HasConditionEffect(ConditionEffects.Stunned))
                {
                    cool = _coolDown.Next(Random);
                    Status = CycleStatus.Completed;
                    return;
                }

                var count = _count;

                Player playerOwner = host.GetPlayerOwner();

                if (playerOwner == null)
                {
                    Program.Debug(typeof(AllyShoot), "Allies cannot shoot if their playerOwner is null.", warn: true);
                    return;
                }

                Entity en;
                en = host.GetNearestEntity(_radius, players: false);

                if (en != null || _defaultAngle != null || _fixedAngle != null)
                {
                    var desc = host.ObjectDesc.Projectiles[_projectileIndex];

                    float a;

                    if (_fixedAngle != null) a = (float)_fixedAngle;
                    else if (en != null) a = (float)Math.Atan2(en.Y - host.Y, en.X - host.X);
                    else if (_defaultAngle != null) a = (float)_defaultAngle;
                    else a = 0;

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
                    var batch = new Packet[count];

                    for (int i = 0; i < count; i++)
                    {
                        var prj = playerOwner.CreateProjectile(desc, host.ObjectType, dmg, host.Manager.Core.getTotalTickCount(), prjPos, (float)(startAngle + _shootAngle * i));
                        host.Owner.EnterWorld(prj);

                        if (i == 0)
                            prjId = prj.ProjectileId;

                        batch[i] = new ServerPlayerShoot()
                        {
                            BulletId = prj.ProjectileId,
                            OwnerId = playerOwner.Id,
                            ContainerType = host.ObjectType,
                            StartingPos = prjPos,
                            Angle = prj.Angle,
                            Damage = (short)dmg
                        };
                    }

                    foreach (var plr in host.Owner.Players.Values
                        .Where(p => p.DistSqr(host) < Player.RadiusSqr))
                    {
                        plr.Client.SendPackets(batch);
                    }
                }
                cool = _coolDown.Next(Random);
                Status = CycleStatus.Completed;
            }
            else
            {
                int track = (int)CoreConstant.worldLogicTickMs;
                cool -= track;
                if (host.HasConditionEffect(ConditionEffects.Dazed))
                    cool += track / 2;
                Status = CycleStatus.InProgress;
            }

            state = cool;
        }
    }
}
