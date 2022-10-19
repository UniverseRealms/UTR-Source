using System;
using wServer.realm.cores;

namespace wServer.logic
{
    internal struct Cooldown
    {
        public int cooldown;
        public int variance;

        public Cooldown(int cooldown, int variance)
        {
            this.cooldown = cooldown;
            this.variance = variance;
        }

        public Cooldown Normalize(bool isShoot = false)
        {
            if (cooldown == 0) return (int)(1000 / (isShoot ? CoreConstant.enemyRateOfFire : 1f));
            else
            {
                if (isShoot)
                {
                    cooldown = (int)(cooldown / CoreConstant.enemyRateOfFire);
                    variance = (int)(variance / CoreConstant.enemyRateOfFire);
                }

                return this;
            }
        }

        public Cooldown Normalize(int def, bool isShoot = false)
        {
            if (cooldown == 0) return (int)(def / (isShoot ? CoreConstant.enemyRateOfFire : 1f));
            else
            {
                if (isShoot)
                {
                    cooldown = (int)(cooldown / CoreConstant.enemyRateOfFire);
                    variance = (int)(variance / CoreConstant.enemyRateOfFire);
                }

                return this;
            }
        }

        public int Next(Random rand)
        {
            if (variance == 0) return cooldown;

            return cooldown + rand.Next(-variance, variance + 1);
        }

        public static implicit operator Cooldown(int cooldown)
        {
            return new Cooldown(cooldown, 0);
        }
    }
}
