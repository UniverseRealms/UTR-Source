using common;
using System;
using System.Collections.Generic;
using System.Linq;
using wServer.realm.entities;
using wServer.realm.worlds.logic;

namespace wServer.logic
{
    public class DamageCounter
    {
        private Enemy enemy;
        public Enemy Host { get { return enemy; } }
        public Projectile LastProjectile { get; private set; }
        public Player LastHitter { get; private set; }
        public int TotalDamage { get; private set; }

        public DamageCounter Corpse { get; set; }
        public DamageCounter Parent { get; set; }

        private WeakDictionary<Player, int> hitters = new WeakDictionary<Player, int>();

        public DamageCounter(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public void HitBy(Player player, Projectile projectile, int dmg)
        {
            if (!hitters.TryGetValue(player, out int totalDmg)) totalDmg = 0;

            var trueDmg = Math.Min(dmg, Host.HP);

            TotalDamage += trueDmg;
            hitters[player] = totalDmg + trueDmg;

            LastProjectile = projectile;
            LastHitter = player;

            player.FameCounter.Hit(projectile, enemy);
            if (enemy.ObjectDesc.Quest && (enemy.HP + TotalDamage > 0))
                player.Surge = (totalDmg + trueDmg) * 1000 / (enemy.HP + TotalDamage);
        }

        public Tuple<Player, int>[] GetPlayerData()
        {
            if (Parent != null)
                return Parent.GetPlayerData();
            List<Tuple<Player, int>> dat = new List<Tuple<Player, int>>();
            foreach (var i in hitters)
            {
                if (i.Key.Owner == null) continue;
                dat.Add(new Tuple<Player, int>(i.Key, i.Value));
            }
            return dat.ToArray();
        }

        public void UpdateEnemy(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public void Death()
        {
            if (Corpse != null)
            {
                Corpse.Parent = this;
                return;
            }

            var enemy = (Parent ?? this).enemy;

            if (enemy.Owner == null || enemy.Owner.Deleted) 
                return;

            if (enemy.Owner is Realm) 
                (enemy.Owner as Realm).EnemyKilled(enemy, (Parent ?? this).LastHitter);

            if (enemy.Spawned || enemy.Owner is Arena || enemy.Owner is ArenaSolo || enemy.Owner is Nexus) 
                return;

            var lvlUps = 0;

            foreach (var player in enemy.Owner.Players.Values.Where(p => enemy.Dist(p) < 25))
            {
                if (player == null || player.Owner == null) continue;
                if (player.HasConditionEffect(common.resources.ConditionEffects.Paused)) continue;

                var xp = enemy.GivesNoXp ? 0d : 1d;
                xp *= Math.Floor(Math.Min(enemy.ObjectDesc.MaxHP * 0.1f, (player.Level * 100 - 50) * 0.1f) * (enemy.ObjectDesc.ExpMultiplier ?? 1));

                var upperLimit = player.ExperienceGoal * 0.1f;

                if (player.Quest == enemy) upperLimit = player.ExperienceGoal * 0.5f;

                double playerXp;

                if (upperLimit < xp) playerXp = upperLimit;
                else playerXp = xp;

                if (player.Owner is DeathArena || player.Owner.GetDisplayName().Contains("Theatre")) playerXp *= .33f;
                if (player.XPBoostTime != 0 && player.Level < 20) playerXp *= 5;

                var killer = (Parent ?? this).LastHitter == player;

                if (player.EnemyKilled(enemy, (int)playerXp, killer) && !killer) lvlUps++;
            }

            if ((Parent ?? this).LastHitter != null) (Parent ?? this).LastHitter.FameCounter.LevelUpAssist(lvlUps);
        }

        public int DamageFrom(Player p)
        {
            return hitters[p];
        }

        public void TransferData(DamageCounter dc)
        { // transfers data from this to dc
            dc.LastProjectile = LastProjectile;
            dc.LastHitter = LastHitter;
            dc.TotalDamage += TotalDamage;

            foreach (var plr in hitters.Keys)
            {
                if (!hitters.TryGetValue(plr, out int totalDmg))
                    totalDmg = 0;
                if (!dc.hitters.TryGetValue(plr, out int totalExistingDmg))
                    totalExistingDmg = 0;

                dc.hitters[plr] = totalDmg + totalExistingDmg;
            }
        }
    }
}
