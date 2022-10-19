using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ ShadowLand = () => Behav()

        .Init("SL Test1",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new Follow(0.5, 8, 1),
                    new Wander(0.5),
                    new Shoot(10, count: 3, shootAngle: 37, projectileIndex: 0, fixedAngle: 0, coolDown: 500, coolDownOffset: 0),
                    new Shoot(10, count: 3, shootAngle: 37, projectileIndex: 0, fixedAngle: 180, coolDown: 500, coolDownOffset: 0),
                    new Shoot(10, count: 3, shootAngle: 37, projectileIndex: 0, fixedAngle: 90, coolDown: 500, coolDownOffset: 400),
                    new Shoot(10, count: 3, shootAngle: 37, projectileIndex: 0, fixedAngle: 270, coolDown: 500, coolDownOffset: 400),
                    new Shoot(10, count: 3, shootAngle: 30, projectileIndex: 1, coolDown: 1000)
                    )
                )
            );
    }
}