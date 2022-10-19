using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Turkey = () => Behav()
        .Init("Big Turkey God",
            new State(
                new ScaleHP(0.3),
                new State(
                new State("default",
                    new ConditionalEffect(ConditionEffectIndex.SlowedImmune, true),
                    new ConditionalEffect(ConditionEffectIndex.PetrifyImmune, true),
                    new ConditionalEffect(ConditionEffectIndex.ParalyzeImmune, true),
                    new ConditionalEffect(ConditionEffectIndex.DazedImmune, true),
                    new ConditionalEffect(ConditionEffectIndex.WeakImmune, true),
                    new ConditionalEffect(ConditionEffectIndex.StunImmune, true),
                    new ConditionalEffect(ConditionEffectIndex.StasisImmune, true),
                    new ConditionalEffect(ConditionEffectIndex.BleedingImmune, true),
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new ChangeSize(30, 145),
                    new TimedTransition(3000, "fight1")
                    ),
                new State("fight1",
                     new Taunt("Gobble*"),
                     new Wander(0.5),
                     new Spawn("Yeti", maxChildren: 3, initialSpawn: 3, coolDown: 100000),
                     new Shoot(10, count: 12, projectileIndex: 0, coolDown: 3000, shootAngle: 30),
                     new Shoot(10, predictive: 0.5, projectileIndex: 1, coolDown: 1500, count: 5, shootAngle: 10),
                     new HpLessTransition(0.7, "fight2")
                    ),
                new State("fight2",
                    new ReturnToSpawn(speed: 2),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Flash(0xff0000, 1, 5),
                    new TimedTransition(4000, "3")
                    ),
                new State("3",
                    new Taunt("Gobble*"),
                    new SpiralShoot(rotateAngle: 10, projectileIndex: 2, shotsToRestart: 1000, numShots: 4, shootAngle: 90, coolDown: 500),
                    new Shoot(10, projectileIndex: 3, count: 3, shootAngle: 10, coolDown: 3500),
                    new HpLessTransition(0.4, "4")
                    ),
                new State("4",
                    new TossObject("Egg Bomb", range: 2, angle: 0, coolDown: 5000),
                    new Follow(0.6, 8, 1),
                    new Wander(0.6),
                    new Shoot(10, count: 12, shootAngle: 24, projectileIndex: 4, coolDown: 1500),
                    new HpLessTransition(0.1, "5")
                    ),
                new State("5",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new ReturnToSpawn(speed: 2),
                    new Taunt("GOBBLE!*"),
                    new Shoot(80, projectileIndex: 5, count: 20, shootAngle: 18, fixedAngle: 0, coolDown: 1000, coolDownOffset: 0),
                    new Shoot(80, projectileIndex: 6, count: 20, shootAngle: 18, fixedAngle: 0, coolDown: 1000, coolDownOffset: 0),
                    new Shoot(80, projectileIndex: 7, count: 20, shootAngle: 18, fixedAngle: 0, coolDown: 1000, coolDownOffset: 0),
                    new Shoot(80, projectileIndex: 5, count: 20, shootAngle: 18, fixedAngle: 45, coolDown: 1000, coolDownOffset: 500),
                    new Shoot(80, projectileIndex: 6, count: 20, shootAngle: 18, fixedAngle: 45, coolDown: 1000, coolDownOffset: 500),
                    new Shoot(80, projectileIndex: 7, count: 20, shootAngle: 18, fixedAngle: 45, coolDown: 1000, coolDownOffset: 500),
                    new TimedTransition(20000, "death")
                    ),
                new State("death",
                    new Shoot(10, projectileIndex: 8, count: 5, shootAngle: 72, fixedAngle: 0, coolDown: 1000),
                    new Suicide()
                    )
                )
            ),
            new Threshold(0.01,
                    new ItemLoot("Onrane", 0.00001)
                )
            )
            .Init("Turkey Overseer",
                 new State(
                     new ScaleHP(0.1),
                     new StayCloseToSpawn(0.3, range: 7),
                              new Wander(1),
                              new Shoot(10, count: 4, predictive: 0.9, projectileIndex: 0, coolDown: 1250)
                 )
             )
             .Init("Turkey Defender",
                 new State(
                     new ScaleHP(0.1),
                     new Wander(0.5),
                              new StayCloseToSpawn(0.03, range: 7),
                              new Follow(0.4, acquireRange: 9, range: 2),
                              new Shoot(10, count: 1, coolDown: 1000, predictive: 0.9, projectileIndex: 0)
                 )
             )
             .Init("Turkey Blaster",
                 new State(
                     new ScaleHP(0.1),
                     new Wander(0.5),
                              new StayCloseToSpawn(0.03, range: 7),
                              new Follow(0.4, acquireRange: 9, range: 2),
                              new Shoot(10, count: 2, predictive: 0.9, projectileIndex: 0, coolDown: 1500),
                              new Shoot(10, count: 1, predictive: 0.9, projectileIndex: 0, coolDown: 1500)
                 )
            )
                     .Init("Yeti",
                 new State(
                    new ScaleHP(0.1),
                    new Follow(0.6, 8, 1),
                    new Wander(0.6),
                    new Shoot(10, count: 5, projectileIndex: 0, fixedAngle: 0, coolDown: 2000, shootAngle: 80),
                    new Shoot(10, count: 5, projectileIndex: 1, fixedAngle: 0, coolDown: 2000, shootAngle: 80),
                    new Shoot(10, count: 5, projectileIndex: 2, fixedAngle: 0, coolDown: 2000, shootAngle: 80),
                    new Shoot(10, count: 5, projectileIndex: 3, fixedAngle: 0, coolDown: 2000, shootAngle: 80),
                    new Shoot(10, count: 5, projectileIndex: 4, fixedAngle: 0, coolDown: 2000, shootAngle: 80)
                 )
            )
         .Init("Egg Bomb",
          new State(
              new State("Idle",
                  new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                  new Flash(0xff0000, 2, 1),
                  new ChangeSize(100, 130),
                  new TimedTransition(800, "Kaboom2")
                  ),
              new State("Kaboom2",
                  new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                  new Shoot(30, projectileIndex: 0, count: 8, fixedAngle: 0, shootAngle: 45),
                  new Decay(0)
                  )
              )
            )
        ;
    }
}