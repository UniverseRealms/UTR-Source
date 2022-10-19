using common.resources;
using Mono.Game;
using System;
using System.Collections.Generic;
using wServer.realm;
using wServer.realm.cores;
using wServer.realm.entities;

namespace wServer.logic.behaviors
{
    internal class ZombieBehaviour : CycleBehavior
    {
        //State storage: follow state
        private class ZombieState
        {
            public S State;
            public int RemainingTime;
        }

        private enum S
        {
            ReturnToParent,
            FollowParent,
            Calculate,
            Attack,
        }

        private readonly float _speed;
        private readonly float _range;
        private readonly int _damage;
        private readonly int _coolDown;
        private readonly float _radius;
        private readonly bool _ignoreArmor;
        private readonly bool _targetPlayer;
        private readonly uint _color;
        private readonly ConditionEffectIndex _effect;
        private readonly int _effectDuration;
        private int attackCooldown;
        private bool getOnce = false;





        public ZombieBehaviour(double speed, double range,
            int damage, int cooldown, float radius, bool ignoreArmor = false, bool targetPlayer = false, uint color = 0xff9000ff, ConditionEffectIndex effect = ConditionEffectIndex.Dead, int effectDuration = 2000)
        {
            this._speed = (float)speed;
            this._range = (float)range;

            this._color = color;
            this._damage = damage;
            this._coolDown = cooldown;
            this._targetPlayer = targetPlayer;
            this._radius = radius;
            this._effect = effect;
            this._ignoreArmor = ignoreArmor;
            this._effectDuration = effectDuration;

        }

        protected override void TickCore(Entity host, ref object state)
        {
            var player = host.GetPlayerOwner();
            if (player == null)
                return;

            ZombieState s;

            if (state == null) s = new ZombieState();
            else s = (ZombieState)state;

            Status = CycleStatus.NotStarted;


            //if (player.HasConditionEffect(ConditionEffects.Paralyzed)) return; //if Pet Stasis, turn to chicken kekw
            int cool = (int)(state ?? _coolDown);
            
            if (cool <= 0)
            {
                s.State = S.Calculate;
                cool = _coolDown;
            }
            else
            {
                cool -= (int)CoreConstant.worldLogicTickMs;
            }
            
            IEnumerable<Entity> nearestEnemies = null;
            if(attackCooldown > 0)
            {
                if (!getOnce)
                {
                    nearestEnemies = host.GetNearestEntities(_range);
                    getOnce = true;
                }
                attackCooldown -= (int)CoreConstant.worldLogicTickMs;
            }

            Vector2 vect;
            var pX = player.X;
            var pY = player.Y;

            Entity nearestEnemy = GetEnemy(nearestEnemies);

            switch (s.State)
            {
                case S.Calculate:
                    {
                        Status = CycleStatus.InProgress; //First find enemy to attack
                        var distanceToPlayer = new Vector2(pX - host.X, pY - host.Y); //if cant find enemy, check if player is close enough
                        if (distanceToPlayer.Length() > 10f)
                            goto case S.ReturnToParent;
                        else //if too far than tp or follow
                            goto case S.FollowParent;
                    }
                case S.ReturnToParent:
                    {
                        host.Move(player.X, player.Y); //If further than 10 tiles than just tp
                        goto case S.Calculate;
                    }
                case S.FollowParent:
                    {
                        vect = new Vector2(pX - host.X, pY - host.Y);
                        vect.X -= Random.Next(-2, 2) / 2f;
                        vect.Y -= Random.Next(-2, 2) / 2f;
                        vect.Normalize();
                        var dist = host.GetSpeed(_speed) * (CoreConstant.worldLogicTickMs / 1000f); //Orbit like follow, i think
                        host.ValidateAndMove(host.X + vect.X * dist, host.Y + vect.Y * dist);
                        if (nearestEnemy != null)
                            goto case S.Attack;

                        Status = CycleStatus.Completed;
                        break;
                    }
                case S.Attack:
                    { //attack the fuker
                        ChargeAndAttack(host, nearestEnemy);
                        Status = CycleStatus.Completed;
                        break;
                    }
            }
            state = s;
        }

        private Entity GetEnemy(IEnumerable<Entity> enemies)
        {
            if (enemies == null)
                return null;

            foreach (Entity i in enemies)
            {   //if its an ally just ignore it
                if (i.ObjectDesc.Ally || i.ObjectDesc.Class == "Ally" || i.HasConditionEffect(ConditionEffects.Invincible) || i.HasConditionEffect(ConditionEffects.Invulnerable)) //if its a ally or is invicible ignore it
                {
                    continue;
                }

                if (i.ObjectDesc.Quest || i.ObjectDesc.Encounter) //prioritise quests/events 
                {
                    return i;
                }

                if (i.ObjectDesc.Enemy)//Last target enemies
                {
                    return i;
                }
            }
            return null; // if its not enemy then its null
        }

        private void ChargeAndAttack(Entity host, Entity en)
        {
            //if _targetPlayer, jump to player instead           
            Vector2 vect = new Vector2(en.X - host.X, en.Y - host.Y);
            vect.Normalize();
            var distance = host.GetSpeed(_speed * 1.5f) * (CoreConstant.worldLogicTickMs / 1000f);
            host.ValidateAndMove(host.X + vect.X * distance, host.Y + vect.Y * distance);
            if(attackCooldown <= 0)
                Attack(host, en);
        }

        private void Attack(Entity host, Entity entity)
        {
            attackCooldown = _coolDown;
            getOnce = true;
            if (_targetPlayer)
            {
                System.Console.WriteLine("<ZAP BEHAVIOUR> Targeting players not implemented!");
                //not implemented
                //Maybe add UT zombies which heal nearby players
            }
            
            if(!_targetPlayer)
            {
                var target = entity;
                if (target != null)
                {
                    if (target is Enemy e)
                    {
                        e.Damage((Player)host.GetPlayerOwner(), _damage, _ignoreArmor, new ConditionEffect()
                        {
                            Effect = _effect,
                            DurationMS = 3000
                        });
                    }
                }
            }  
        }
    }
}
