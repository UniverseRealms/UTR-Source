using System;
using wServer.realm;
using wServer.realm.cores;
using wServer.realm.entities;
using System.Linq;
using common.resources;

namespace wServer.logic.behaviors
{
    internal class ItemScaleHP : Behavior
    {
        private readonly double _amount;
        private readonly bool _checkMax;
        private readonly int _radius;
        private int _cachedMaxHp = -1;

        public ItemScaleHP(double amount, bool checkMax = true, int radius = 30)
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

                if (enemy.lastPlayerNum == playerNum) return;

                foreach (var player in enemy.Owner.Players.Values)
                {
                    if (enemy.Dist(player) > _radius)
                        continue;

                    var Eternals = 0;
                    var Sacreds = 0;
                    var Divines = 0;
                    if (player.WearingDivine) Divines++;
                    if (player.WearingEternal) Eternals++;
                    if (player.WearingSacred) Sacreds++;

                    if(enemy.SpawnedHp == 0)
                    {
                        float hpPerc = enemy.HP / (float)enemy.MaximumHP;

                        if(_cachedMaxHp == -1)
                        {
                            _cachedMaxHp = enemy.MaximumHP;
                        }
                        //Eternals boost slightly, sacreds boost 2x as much and divines boost 3times as much
                        float boost = 1 + Eternals * ((1 + (Sacreds * 2)) * (1 + (Divines * 3)));
                        long newHp = (long)(_cachedMaxHp + _amount * enemy.ObjectDesc.MaxHP * boost * Math.Max(playerNum - 1 , 0));

                        if (newHp > Int32.MaxValue)
                            newHp = Int32.MaxValue;

                        enemy.MaximumHP = (int)newHp;
                        enemy.HP = (int)(enemy.MaximumHP * hpPerc);


                    }

                    cool = 10000;
                    enemy.lastPlayerNum = playerNum;

                }
            }
            else cool -= (int)CoreConstant.worldLogicTickMs;

            state = cool;
        }
    }
}
