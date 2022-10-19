using common.resources;
using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ SummerBreeze = () => Behav()

        .Init("Bluey the Slime",
            new State(
                new ScaleHP(0.1),
                    new Reproduce(densityMax: 5, densityRadius: 10),
                    new TransformOnDeath("Medium Bluey the Slime"),
                    new TransformOnDeath("Medium Bluey the Slime"),
                new State("1",
                    new ConditionEffectRegion(ConditionEffectIndex.Slowed, 2, 2),
                    new Follow(0.8, 8, 1),
                    new Wander(0.8),
                    new Shoot(10, count: 8, fixedAngle: 0, shootAngle: 45, projectileIndex: 0, coolDown: 3000, coolDownOffset: 0),
                    new Shoot(10, count: 6, shootAngle: 20, projectileIndex: 1, coolDown: 2000, coolDownOffset: 500)
                    )
                )
            )
           .Init("Medium Bluey the Slime",
            new State(
                new ScaleHP(0.1),
                new Follow(1, 8, 1),
                new Wander(1),
                new Reproduce(densityMax: 5, densityRadius: 10),
                new TransformOnDeath("Little Bluey the Slime"),
                new TransformOnDeath("Little Bluey the Slime"),
                new TransformOnDeath("Little Bluey the Slime"),
                new TransformOnDeath("Little Bluey the Slime"),
                new TransformOnDeath("Little Bluey the Slime"),
                new State("1",
                    new SpiralShoot(rotateAngle: 30, shotsToRestart: 60, numShots: 6, shootAngle: 60, projectileIndex: 0, coolDown: 1500)
                    )
                )
            )
           .Init("Little Bluey the Slime",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new Follow(1.2, 8, 1),
                    new Wander(1.2),
                    new Shoot(20, projectileIndex: 0, count: 2, shootAngle: 11, coolDown: 1000)
                    )
                )
            )
            .Init("Crimson Disciple",
                new State(
                    new ScaleHP(0.1),
                    new State("1",
                        new Follow(0.6, 8, 1),
                        new Wander(0.6),
                        new Shoot(10, count: 3, projectileIndex: 0, shootAngle: 20, coolDown: 2000),
                        new Grenade(7, damage: 70, range: 5, coolDown: 2000, coolDownOffset: 0),
                        new Grenade(5, damage: 90, range: 5, coolDown: 2000, coolDownOffset: 250),
                        new Grenade(3, damage: 110, range: 5, coolDown: 2000, coolDownOffset: 500),
                        new Grenade(1, damage: 130, range: 5, coolDown: 2000, coolDownOffset: 750)
                    )
                )
            )
            .Init("Disciple of Frost",
                new State(
                    new ScaleHP(0.1),
                    new State("1",
                        new Follow(0.6, 8, 1),
                        new Wander(0.6),
                        new Shoot(10, count: 4, projectileIndex: 0, shootAngle: 3, coolDown: 1000, predictive: 0.5)
                    )
                )
            )
            .Init("Verdant Disciple",
                new State(
                    new ScaleHP(0.1),
                    new State("1",
                        new Follow(0.6, 8, 1),
                        new Wander(0.6),
                        new HealGroup(range: 10, group: "SummerBreeze", coolDown: 5000, healAmount: 1000),
                        new Shoot(10, count: 8, projectileIndex: 0, shootAngle: 20, coolDown: 1500, predictive: 0.5)
                    )
                )
            )
            .Init("Triune of the Elements",
                new State(
                    new ScaleHP(0.1),
                    new State("1",
                        new Follow(0.6, 8, 1),
                        new Wander(0.6),
                        new Shoot(10, count: 10, projectileIndex: 0, shootAngle: 36, coolDown: 1500, fixedAngle: 0, coolDownOffset: 0),
                        new Shoot(10, count: 3, projectileIndex: 1, shootAngle: 20, coolDown: 1500, coolDownOffset: 1000),
                        new TimedTransition(5000, "colorpicker")
                        ),
                    new State("colorpicker",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new TimedTransition(500, "redprep", true),
                        new TimedTransition(500, "blueprep", true),
                        new TimedTransition(500, "greenprep", true)
                        ),
                    new State("redprep",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Flash(0xff0000, 0.5, 10),
                        new TimedTransition(2000, "red")
                        ),
                    new State("blueprep",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Flash(0x00ffff, 0.5, 10),
                        new TimedTransition(2000, "blue")
                        ),
                    new State("greenprep",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Flash(0x00ff00, 0.5, 10),
                        new TimedTransition(2000, "green")
                        ),
                    new State("red",
                        new Follow(0.8, 8, 1),
                        new Wander(0.8),
                        new Shoot(10, count: 3, projectileIndex: 2, shootAngle: 20, coolDown: 2000),
                        new Grenade(7, damage: 70, range: 5, coolDown: 2000, coolDownOffset: 0),
                        new Grenade(5, damage: 90, range: 5, coolDown: 2000, coolDownOffset: 250),
                        new Grenade(3, damage: 110, range: 5, coolDown: 2000, coolDownOffset: 500),
                        new Grenade(1, damage: 130, range: 5, coolDown: 2000, coolDownOffset: 750),
                        new TimedTransition(7000, "1")
                        ),
                    new State("blue",
                        new Follow(0.8, 8, 1),
                        new Wander(0.8),
                        new Shoot(10, count: 4, projectileIndex: 3, shootAngle: 3, coolDown: 1000, predictive: 0.5)
                        ),
                    new State("green",
                        new Follow(0.8, 8, 1),
                        new Wander(0.8),
                        new HealGroup(range: 10, group: "SummerBreeze", coolDown: 3000, healAmount: 1000),
                        new Shoot(10, count: 8, projectileIndex: 4, shootAngle: 20, coolDown: 1500, predictive: 0.5),
                        new TimedTransition(7000, "1")
                    )
                )
            )
            .Init("Vangor, the Ancient",
                new State(
                    new ScaleHP(0.3),
                    new State("idle",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new PlayerWithinTransition(5, "1")
                        ),
                    new State("1",
                        new Taunt("You think you can march in to steal my treasures? Wrong! I will stop you myself!"),
                        new Flash(0x01C7FC, 1, 10),
                        new TimedTransition(5000, "2")
                        ),
                    new State("2",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                        new Shoot(10, count: 20, shootAngle: 18, projectileIndex: 0, coolDown: 1500),
                        new Shoot(10, count: 5, shootAngle: 13, projectileIndex: 1, coolDown: 2500),
                        new HpLessTransition(0.7, "3")
                        ),
                    new State("3",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Taunt("Feel the waves as they carry you away!"),
                        new Shoot(10, count: 6, projectileIndex: 2, shootAngle: 18, coolDown: 2500),
                        new SpiralShoot(40, projectileIndex: 3, shotsToRestart: 45, numShots: 5, shootAngle: 72, fixedAngle: 0, coolDown: 1000, delayAfterComplete: 500),
                        new Grenade(5, damage: 60, range: 10, coolDown: 3000),
                        new HpLessTransition(0.5, "4")
                        ),
                    new State("4",
                        new Taunt("I cannot lose my precious treasures to the likes of you!"),
                        new Follow(0.6, 8, 1),
                        new Wander(0.6),
                        new SpiralShoot(40, projectileIndex: 0, shotsToRestart: 360, numShots: 10, shootAngle: 36, fixedAngle: 0, coolDown: 1500, delayAfterComplete: 500),
                        new Shoot(10, projectileIndex: 4, shootAngle: 18, count: 10, coolDown: 2000),
                        new HpLessTransition(0.2, "5")
                        ),
                    new State("5",
                        new ReturnToSpawn(2),
                        new Taunt("You will fall!"),
                        new Shoot(10, projectileIndex: 4, shootAngle: 13, count: 5, coolDown: 1000),
                        new Shoot(10, projectileIndex: 5, shootAngle: 9, count: 40, coolDown: 3000, fixedAngle: 0)
                    )
                ),
                new Threshold(0.01,
                    new ItemLoot("Ocean's Blessing", 0.01),
                    new ItemLoot("Mineral Stave", 0.01),
                    new ItemLoot("Tiki Skull", 0.005),
                    new ItemLoot("Potion of Dexterity", 1),
                    new TierLoot(3, ItemType.Weapon, 0.05),
                    new TierLoot(3, ItemType.Ability, 0.05),
                    new TierLoot(3, ItemType.Armor, 0.05),
                    new TierLoot(3, ItemType.Ring, 0.05),
                    new TierLoot(4, ItemType.Weapon, 0.025),
                    new TierLoot(4, ItemType.Ability, 0.025),
                    new TierLoot(4, ItemType.Armor, 0.025),
                    new TierLoot(4, ItemType.Ring, 0.025),
                    new ItemLoot("Summer's Breeze Key", 0.005),
                    new ItemLoot("Rusted Staff", 0.00112),
                    new ItemLoot("Eternal Essence", 0.0005)
                )
        );
    }
}