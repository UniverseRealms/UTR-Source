using common.resources;
using System;
using wServer.networking.packets.outgoing;
using wServer.realm.cores;

namespace wServer.realm.entities
{
    internal class Trap : StaticObject
    {
        private const int LIFETIME = 20;

        private Player player;
        private float radius;
        private int dmg;
        private ConditionEffectIndex effect;
        private int Duration;
        private int EffDuration;
        private uint color;
        private int rateOfFire;

        private int coolDown = 0;
        private int coolDown2 = 0;

        private int lifeTime = 0;

        public Trap(Player player, float radius, int dmg, int duration, int rof, ConditionEffectIndex eff, float effDuration, uint color)
            : base(player.Manager, 0x0711, duration, true, true, false)
        {
            this.player = player;
            this.radius = radius;
            this.dmg = dmg;
            this.color = color;
            rateOfFire = rof;
            effect = eff;
            Duration = duration;//(int)(effDuration * 1000);
            lifeTime = duration;
            EffDuration = (int)(effDuration * 1000);
        }
        public override void Tick()
        {
            if(lifeTime <= 0)
            {
                Explode();
            }
            else
                lifeTime -= (int)CoreConstant.worldLogicTickMs;

            DoDamage();

            DoParticles();

            base.Tick();
        }

        private void DoDamage()
        {
            if (coolDown <= 0) //Every ROF do dmg
            {
                coolDown = rateOfFire;
                DamageNearbyEnemies();
            }
            else
            {
                coolDown -= (int)CoreConstant.worldLogicTickMs;
            }
        }

        private void DoParticles()
        {
            if (coolDown2 <= 0)// Every 0.5s show trap effect. Stop trap form spamming packets
            {
                coolDown2 = 500;//0.5s

                Owner.BroadcastPacketNearby(new ShowEffect()
                {
                    EffectType = EffectType.Trap,
                    Color = new ARGB(color),
                    TargetObjectId = Id,
                    Pos1 = new Position() { X = radius * .75f } //radius is only 75% of it size
                }, this, null);
            }
            else
            {
                coolDown2 -= (int)CoreConstant.worldLogicTickMs;
            }
        }

        private void DamageNearbyEnemies()
        {
            var enemies = this.GetNearestEntities(radius);

            if (enemies == null) //Might not be any enemies
                return; 

            foreach (var enemy in enemies)
            {
                if (enemy == null || enemy.ObjectDesc.ObjectType == 0x0711 || enemy.ObjectDesc.Class != "Character") //Enemy might die before we damage it
                    continue;

                

                (enemy as Enemy).Damage(player, dmg, true, new ConditionEffect()
                {
                    Effect = effect,
                    DurationMS = EffDuration
                });
            }    
        }

        private void Explode()
        {
            Owner.BroadcastPacketNearby(new ShowEffect()
            {
                EffectType = EffectType.AreaBlast,
                Color = new ARGB(color),
                TargetObjectId = Id,
                Pos1 = new Position() { X = radius }
            }, this, null);

            this.AOE(radius, false, enemy =>
            {
                (enemy as Enemy).Damage(player, dmg, false, new ConditionEffect()
                {
                    Effect = effect,
                    DurationMS = EffDuration
                });
            });

            Owner.LeaveWorld(this);
        }
    }
}
