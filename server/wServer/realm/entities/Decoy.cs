using common.resources;
using Mono.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using wServer.networking.packets;
using wServer.networking.packets.outgoing;
using wServer.realm.cores;

namespace wServer.realm.entities
{
    internal class Decoy : Ally
    {
        private static Random rand = new Random();

        private Player player;
        private int duration;
        private bool explode;
        private bool customBehav;
        private int numShots;
        private Item item;
        private Vector2 direction;
        private float speed;

        private Vector2 GetRandDirection()
        {
            double angle = rand.NextDouble() * 2 * Math.PI;
            return new Vector2(
                (float)Math.Cos(angle),
                (float)Math.Sin(angle)
            );
        }

        public Decoy(Player player, int duration, float tps, bool explode, int numShots, Item item, ushort decoyObjType)
            : base(player.Manager, (decoyObjType != 0 ? decoyObjType : (ushort)0x0715), true)
        {
            this.customBehav = (decoyObjType != 0 && decoyObjType != 0x0715);
            this.player = player;
            this.duration = customBehav ? 1 : duration;
            this.speed = tps;
            this.explode = explode;
            this.numShots = numShots;
            this.item = item;

            var history = player.TryGetHistory(1);
            if (history == null)
                direction = GetRandDirection();
            else
            {
                direction = new Vector2(player.X - history.Value.X, player.Y - history.Value.Y);
                if (direction.LengthSquared() == 0)
                    direction = GetRandDirection();
                else
                    direction.Normalize();
            }
        }

        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            stats[StatsType.Texture1] = player.Texture1;
            stats[StatsType.Texture2] = player.Texture2;
            base.ExportStats(stats);
        }

        private long JoinedWorld { get; set; }
        private bool IsJoinedWorld { get; set; }

        public override void Tick()
        {
            if (customBehav)
            {
                base.Tick();
            }
            else
            {
                if (!IsJoinedWorld)
                {
                    IsJoinedWorld = true;
                    JoinedWorld = Manager.Core.getTotalTickCount();

                    base.Tick();
                }
                else if (duration - 2000 > Manager.Core.getTotalTickCount() - JoinedWorld)
                {
                    this.ValidateAndMove(
                        X + direction.X * speed * CoreConstant.worldLogicTickMs / 1000,
                        Y + direction.Y * speed * CoreConstant.worldLogicTickMs / 1000
                    );
                }
                else if (duration < Manager.Core.getTotalTickCount() - JoinedWorld && HP >= 0)
                {
                    if (explode)
                    {
                        var batch = new Packet[numShots];

                        ProjectileDesc prjDesc = item.Projectiles[0];
                        Position decoyPos = new Position() { X = X, Y = Y };
                        float _angle = 360 / numShots;
                        wRandom rando = new wRandom();
                        for (var i = 0; i < numShots; i++)
                        {
                            int dmg = (int)rando.NextIntRange((uint)prjDesc.MinDamage, (uint)prjDesc.MaxDamage);
                            Projectile proj = player.CreateProjectile(prjDesc, item.ObjectType, dmg, Manager.Core.getTotalTickCount(), decoyPos, (float)(i * (Math.PI * 2) / numShots));

                            Owner.EnterWorld(proj);
                            player.FameCounter.Shoot(proj);
                            batch[i] = new ServerPlayerShoot()
                            {
                                BulletId = proj.ProjectileId,
                                OwnerId = player.Id,
                                ContainerType = item.ObjectType,
                                StartingPos = decoyPos,
                                Angle = proj.Angle,
                                Damage = (short)proj.Damage
                            };
                        }

                        foreach (Player plr in Owner?.Players.Values.Where(p => p?.DistSqr(this) < Player.RadiusSqr))
                            plr?.Client.SendPackets(batch);
                    }

                    Owner.BroadcastPacketNearby(new ShowEffect()
                    {
                        EffectType = EffectType.AreaBlast,
                        Color = new ARGB(0xffff0000),
                        TargetObjectId = Id,
                        Pos1 = new Position() { X = 1 }
                    }, this, null);

                    base.Tick();
                    HP = -1;
                }
                else
                {
                    base.Tick();
                }
            }
        }
    }
}
