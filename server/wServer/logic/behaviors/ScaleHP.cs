using System;
using wServer.realm;
using wServer.realm.cores;
using wServer.realm.entities;
using System.Linq;
using common.resources;

namespace wServer.logic.behaviors
{
    internal class ScaleHP : Behavior
    {
        private readonly double _amount;
        private readonly bool _checkMax;
        private readonly int _radius;
        private int _cachedMaxHp = -1;

        public ScaleHP(double amount, bool checkMax = true, int radius = 50)
        {
            _amount = amount;
            _checkMax = checkMax;
            _radius = radius;
        }

        protected override void TickCore(Entity host, ref object state)
        {
            var cool = (int?)state ?? -1;

            if (cool <= 0)
            {
                var enemy = host as Enemy;
                int playerNum = enemy.Owner.Players.Values.Count(p => enemy.Dist(p) < _radius && !p.HasConditionEffect(ConditionEffects.Hidden));

                if (enemy.SpawnedHp == 0)
                {
                    if (enemy.lastPlayerNum == playerNum) return;

                    float hpPerc = enemy.HP / (float)enemy.MaximumHP;

                    if (_cachedMaxHp == -1)
                        _cachedMaxHp = enemy.MaximumHP;

                    long newHp = (long)(_cachedMaxHp + _amount * enemy.ObjectDesc.MaxHP * Math.Max(playerNum - 1, 0));

                    if (newHp > Int32.MaxValue)
                        newHp = Int32.MaxValue;

                    enemy.MaximumHP = (int)newHp;
                    enemy.HP = (int)(enemy.MaximumHP * hpPerc);
                }

                cool = 3000;
                enemy.lastPlayerNum = playerNum;
            }
            else cool -= (int)CoreConstant.worldLogicTickMs;

            state = cool;
        }
    }
}
