﻿using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Puppet = () => Behav()
            .Init("Puppet Knight",
                new State(
                    new Prioritize(
                        new Follow(0.58, 8, 1),
                        new Wander(0.2)
                        ),
                    new Shoot(8.4, count: 1, projectileIndex: 0, coolDown: 1750),
                    new Shoot(8, count: 1, projectileIndex: 1, coolDown: 2000)
                    ),
                new ItemLoot("Magic Potion", 0.1),
                new Threshold(0.1,
                    new ItemLoot("Obsidian Dagger", 0.02),
                    new ItemLoot("Steel Helm", 0.02)
                    )
            )
        .Init("Archer Puppet",
                new State(
                    new Wander(0.42),
                    new Shoot(8.4, count: 3, projectileIndex: 0, coolDown: 1550),
                    new Shoot(8, count: 1, projectileIndex: 1, coolDown: 2700)
                    ),
                new ItemLoot("Magic Potion", 0.1),
                new Threshold(0.1,
                    new ItemLoot("Obsidian Dagger", 0.02),
                    new ItemLoot("Steel Helm", 0.02)
                    )
            )
         .Init("Paladin Puppet",
                new State(
                    new Prioritize(
                        new Follow(0.35, 8, 1),
                        new Wander(0.2)
                        ),
                    new HealSelf(coolDown: 5000),
                    new Shoot(8.4, count: 1, projectileIndex: 0, coolDown: 1000)
                    ),
                new ItemLoot("Health Potion", 0.1),
                new Threshold(0.1,
                    new ItemLoot("Mithril Dagger", 0.02),
                    new ItemLoot("Mithril Chainmail", 0.02)
                    )
            )
        .Init("Assassin Puppet",
                new State(
                    new Prioritize(
                        new Follow(0.35, 8, 1),
                        new Wander(0.2)
                        ),
                    new Grenade(3, 100, 4, coolDown: 3000),
                    new Shoot(8.4, count: 1, projectileIndex: 0, coolDown: 1500)
                    ),
                new ItemLoot("Health Potion", 0.1),
                new Threshold(0.1,
                    new ItemLoot("Mithril Dagger", 0.02),
                    new ItemLoot("Mithril Chainmail", 0.02)
                    )
            )
                 .Init("Warrior Puppet",
            new State(
                new State("jugg",
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Wander(0.6),
                    new TimedTransition(4700, "berserk")
                    ),
                new State("berserk",
                    new Shoot(8.4, count: 1, projectileIndex: 0, coolDown: 400),
                    new TimedTransition(4700, "jugg")
                    )
                )
            )
         .Init("Rogue Puppet",
            new State(
                new State("jugg",
                    new SetAltTexture(1),
                    new Wander(0.6),
                    new TimedTransition(3000, "berserk")
                    ),
                new State("berserk",
                     new Wander(0.4),
                     new SetAltTexture(0),
                    new Shoot(8.4, count: 1, projectileIndex: 0, coolDown: 1500),
                    new TimedTransition(3000, "jugg")
                    )
                )
            )
        .Init("Trickster Puppet",
            new State(
                new State("wait",
                    new Wander(0.3),
                    new PlayerWithinTransition(6, "start")
                    ),
                new State("start",
                    new Reproduce("Trickster Decoys", densityMax: 1, densityRadius: 1, coolDown: 5250),
                    new Prioritize(
                    new Wander(0.6),
                    new StayBack(0.3, 3)
                        ),
                    new Shoot(8.4, count: 1, projectileIndex: 0, coolDown: 1300)
                    )

                )
            )
        .Init("Trickster Decoys",
                new State(
                    new Wander(0.31),
                    new Shoot(8.4, count: 1, projectileIndex: 0, coolDown: 1400)
                    ),
                new ItemLoot("Health Potion", 0.1),
                new Threshold(0.1,
                    new ItemLoot("Mithril Dagger", 0.02),
                    new ItemLoot("Mithril Chainmail", 0.02)
                    )
            )
         .Init("Mystic Puppet",
                new State(
                    new Wander(0.27),
                    new Shoot(8.4, count: 1, projectileIndex: 0, coolDown: 1350),
                    new Shoot(8.4, count: 1, projectileIndex: 1, coolDown: 1950)
                    ),
                new ItemLoot("Health Potion", 0.1),
                new Threshold(0.1,
                    new ItemLoot("Mithril Dagger", 0.02),
                    new ItemLoot("Mithril Chainmail", 0.02)
                    )
            )
        .Init("Puppet Wizard",
                new State(
                    new Prioritize(
                           new Orbit(0.37, 4, 20, "The Puppet Master"),
                           new Wander(0.4)
                        ),
                    new Shoot(8.4, count: 10, projectileIndex: 0, coolDown: 2650)
                    ),
                new ItemLoot("Health Potion", 0.1),
                new Threshold(0.1,
                    new ItemLoot("Golden Bow", 0.02),
                    new ItemLoot("Demon Edge", 0.02)
                    )
            )

        .Init("Puppet Priest",
                new State(
                    new Orbit(0.37, 4, 20, "The Puppet Master"),
                    new HealGroup(8, "Master", coolDown: 4500, healAmount: 75)
                    ),
                new ItemLoot("Health Potion", 0.1),
                new Threshold(0.1,
                    new ItemLoot("Golden Bow", 0.02),
                    new ItemLoot("Demon Edge", 0.02)
                    )
            )

        .Init("Healer Puppet",
                new State(
                    new Wander(0.4),
                    new Shoot(8.4, count: 1, projectileIndex: 0, coolDown: 1250),
                    new HealGroup(10, "Healers", coolDown: 3000)
                    ),
                new ItemLoot("Health Potion", 0.1),
                new Threshold(0.1,
                    new ItemLoot("Golden Bow", 0.02),
                    new ItemLoot("Demon Edge", 0.02)
                    )
            )

        .Init("Sorcerer Puppet",
                new State(
                     new Prioritize(
                           new Follow(0.2, 8, 1),
                           new Wander(0.6)
                        ),
                     new TossObject("Puppet Bomb", 5, coolDown: 7500),
                    new Shoot(8.4, count: 1, projectileIndex: 0, coolDown: 2000)
                    ),
                new ItemLoot("Magic Potion", 0.1)
            )
              .Init("Puppet Bomb",
                        new State(
                            new TransformOnDeath("Sorc Bomb Thrower", 1, 1, 1),
                    new State("BOUTTOEXPLODE",
                    new TimedTransition(2750, "boom")
                        ),
                    new State("boom",
                        new Shoot(8.4, count: 1, fixedAngle: 0, projectileIndex: 0, coolDown: 1000),
                        new Shoot(8.4, count: 1, fixedAngle: 90, projectileIndex: 0, coolDown: 1000),
                        new Shoot(8.4, count: 1, fixedAngle: 180, projectileIndex: 0, coolDown: 1000),
                        new Shoot(8.4, count: 1, fixedAngle: 270, projectileIndex: 0, coolDown: 1000),
                        new Shoot(8.4, count: 1, fixedAngle: 45, projectileIndex: 0, coolDown: 1000),
                        new Shoot(8.4, count: 1, fixedAngle: 135, projectileIndex: 0, coolDown: 1000),
                        new Shoot(8.4, count: 1, fixedAngle: 235, projectileIndex: 0, coolDown: 1000),
                        new Shoot(8.4, count: 1, fixedAngle: 315, projectileIndex: 0, coolDown: 1000),
                       new Suicide()
                    )
            )
    )
         .Init("Puppet Bomb 2",
                        new State(
                    new State("BOUTTOEXPLODE",
                    new TimedTransition(2750, "boom")
                        ),
                    new State("boom",
                        new Shoot(8.4, count: 1, fixedAngle: 0, projectileIndex: 0, coolDown: 1000),
                        new Shoot(8.4, count: 1, fixedAngle: 90, projectileIndex: 0, coolDown: 1000),
                        new Shoot(8.4, count: 1, fixedAngle: 180, projectileIndex: 0, coolDown: 1000),
                        new Shoot(8.4, count: 1, fixedAngle: 270, projectileIndex: 0, coolDown: 1000),
                        new Shoot(8.4, count: 1, fixedAngle: 45, projectileIndex: 0, coolDown: 1000),
                        new Shoot(8.4, count: 1, fixedAngle: 135, projectileIndex: 0, coolDown: 1000),
                        new Shoot(8.4, count: 1, fixedAngle: 235, projectileIndex: 0, coolDown: 1000),
                        new Shoot(8.4, count: 1, fixedAngle: 315, projectileIndex: 0, coolDown: 1000),
                       new Suicide()
                    )
            )
    )
        .Init("Sorc Bomb Thrower",
                new State(
                     new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("throw",
                        new TossObject("Puppet Bomb 2", 5, angle: 225, coolDown: 1500),
                        new TossObject("Puppet Bomb 2", 5, angle: 300, coolDown: 1500),
                    new TimedTransition(1000, "die")
                      ),
                      new State("die",
                       new Suicide()
                      )
            )
    )

        .Init("False Puppet Master",
            new State(
                new TransformOnDeath("Rogue Puppet", 1, 1, 1),
                new TransformOnDeath("Assassin Puppet", 1, 1, 1),
                new State(
                    new Swirl(0.45, 6, 6),
                    new Shoot(8.4, count: 7, projectileIndex: 0, coolDown: 2850),
                    new Shoot(8.4, count: 3, shootAngle: 30, projectileIndex: 2, coolDown: 3500),
                    new HpLessTransition(0.11, "dead")
                    ),
                 new State("dead",
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt(1.00, "You may have killed me, but I am only a pretender. Get ready for the plot twist!"),
                    new TimedTransition(2500, "die")
                    ),
                 new State("die",
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new Shoot(8.4, count: 8, projectileIndex: 1, coolDown: 2850),
                    new Suicide()
                    )
                 )
            )

.Init("Puppet Loot Chest",
                new State(
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(5000, "UnsetEffect")
                    ),
                    new State("UnsetEffect")
                ),
                new Threshold(0.01,
                    new TierLoot(3, ItemType.Weapon, 0.025),
                    new TierLoot(3, ItemType.Ability, 0.025),
                    new TierLoot(3, ItemType.Armor, 0.025),
                    new TierLoot(3, ItemType.Ring, 0.025)
                )
            )
        .Init("The Puppet Master",
                new State(
                    new ScaleHP(0.3),
                    new TransformOnDeath("Puppet Loot Chest", 1, 1),
                    new State("default",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new PlayerWithinTransition(6, "move")
                        ),
                     new State("move",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new MoveTo(speed: 1, x: 38, y: 65),
                        new Taunt(1.00, "My puppets need life essence to live! Your sad, sad lives will have to do."),
                        new TimedTransition(5250, "middleShots")
                        ),
                    new State("middleShots",
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 160, coolDownOffset: 1400, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 170, coolDownOffset: 1600, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 180, coolDownOffset: 1800, shootAngle: 90),
                            new Shoot(1, 8, projectileIndex: 3, coolDown: 4575, fixedAngle: 180, coolDownOffset: 2000, shootAngle: 45),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 180, coolDownOffset: 0, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 170, coolDownOffset: 200, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 160, coolDownOffset: 400, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 150, coolDownOffset: 600, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 140, coolDownOffset: 800, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 130, coolDownOffset: 1000, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 120, coolDownOffset: 1200, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 110, coolDownOffset: 1400, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 100, coolDownOffset: 1600, shootAngle: 90),
                         new TimedTransition(5200, "sadTime")
                        ),
                    new State("sadTime",
                        new Follow(0.34, 8, 5),
                        new Shoot(10, projectileIndex: 2, predictive: 0.5, coolDown: 1000),
                        new TimedTransition(6000, "customspiral")
                        ),
                    new State("customspiral",
                       new Spawn("Puppet Wizard 2", initialSpawn: 1, maxChildren: 1, coolDown: 6000),
                        new Spawn("Puppet Priest", initialSpawn: 1, maxChildren: 1, coolDown: 6000),
                        new Spawn("Puppet Knight", initialSpawn: 1, maxChildren: 1, coolDown: 6000),
                        new Shoot(1, 4, projectileIndex: 3, coolDown: 10000, fixedAngle: 90, coolDownOffset: 0, shootAngle: 90),
                        new Shoot(1, 4, projectileIndex: 3, coolDown: 10000, fixedAngle: 100, coolDownOffset: 200, shootAngle: 90),
                        new Shoot(1, 4, projectileIndex: 3, coolDown: 10000, fixedAngle: 110, coolDownOffset: 400, shootAngle: 90),
                        new Shoot(1, 4, projectileIndex: 3, coolDown: 10000, fixedAngle: 120, coolDownOffset: 600, shootAngle: 90),
                        new Shoot(8.4, count: 1, fixedAngle: 0, projectileIndex: 0, coolDown: 1000),
                        new Shoot(8.4, count: 1, fixedAngle: 90, projectileIndex: 0, coolDown: 4500),
                        new Shoot(8.4, count: 1, fixedAngle: 180, projectileIndex: 0, coolDown: 4500),
                        new Shoot(8.4, count: 1, fixedAngle: 270, projectileIndex: 0, coolDown: 4500),
                        new Shoot(8.4, count: 1, fixedAngle: 45, projectileIndex: 0, coolDown: 4500),
                        new Shoot(8.4, count: 1, fixedAngle: 135, projectileIndex: 0, coolDown: 2250),
                        new Shoot(8.4, count: 1, fixedAngle: 235, projectileIndex: 0, coolDown: 2250),
                        new Shoot(8.4, count: 1, fixedAngle: 315, projectileIndex: 0, coolDown: 2250),
                                                new Shoot(1, 4, projectileIndex: 3, coolDown: 10000, fixedAngle: 90, coolDownOffset: 0, shootAngle: 90),
                        new Shoot(1, 4, projectileIndex: 3, coolDown: 10000, fixedAngle: 100, coolDownOffset: 200, shootAngle: 90),
                        new Shoot(1, 4, projectileIndex: 3, coolDown: 10000, fixedAngle: 110, coolDownOffset: 400, shootAngle: 90),
                        new Shoot(1, 4, projectileIndex: 3, coolDown: 10000, fixedAngle: 120, coolDownOffset: 600, shootAngle: 90),
                        new Shoot(8.4, count: 1, fixedAngle: 0, projectileIndex: 0, coolDown: 1000),
                        new Shoot(8.4, count: 1, fixedAngle: 90, projectileIndex: 0, coolDown: 4500),
                        new Shoot(8.4, count: 1, fixedAngle: 180, projectileIndex: 0, coolDown: 4500),
                        new Shoot(8.4, count: 1, fixedAngle: 270, projectileIndex: 0, coolDown: 4500),
                        new Shoot(8.4, count: 1, fixedAngle: 45, projectileIndex: 0, coolDown: 4500),
                        new Shoot(8.4, count: 1, fixedAngle: 135, projectileIndex: 0, coolDown: 2250),
                        new Shoot(8.4, count: 1, fixedAngle: 235, projectileIndex: 0, coolDown: 2250),
                        new Shoot(8.4, count: 1, fixedAngle: 315, projectileIndex: 0, coolDown: 2250),
                       new TimedTransition(7950, "spookclone")
                        ),
                    new State("spookclone",
                        new Spawn("False Puppet Master", maxChildren: 5, coolDown: 5500),
                            new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Wander(0.3),
                        new Shoot(7, count: 7, projectileIndex: 0, coolDown: 4500),
                        new Shoot(8, count: 3, shootAngle: 16, projectileIndex: 2, coolDown: 3000),
                        new TimedTransition(11000, "luckyguess")
                        ),
                    new State("luckyguess",
                        new RemoveEntity(9999, "False Puppet Master"),
                        new Flash(0xFFFFFF, 2, 2),
                        new Taunt(1.00, "Lucky guess hero, but I've run out of time to play games with you. It is time that you die!"),
                         new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new MoveTo(speed: 0.7f, x: 38, y: 54),
                        new TimedTransition(3250, "OutofTime")
                        ),
                   new State("OutofTime",
                            new Spawn("Puppet Wizard", initialSpawn: 1, maxChildren: 2, coolDown: 4500),
                            new Spawn("Puppet Priest", initialSpawn: 1, maxChildren: 1, coolDown: 5000),
                            new Spawn("Puppet Knight", initialSpawn: 1, maxChildren: 1, coolDown: 5000),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 90, coolDownOffset: 0, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 100, coolDownOffset: 200, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 110, coolDownOffset: 400, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 120, coolDownOffset: 600, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 130, coolDownOffset: 800, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 140, coolDownOffset: 1000, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 150, coolDownOffset: 1200, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 160, coolDownOffset: 1400, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 170, coolDownOffset: 1600, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 180, coolDownOffset: 1800, shootAngle: 90),
                            new Shoot(1, 8, projectileIndex: 3, coolDown: 4575, fixedAngle: 180, coolDownOffset: 2000, shootAngle: 45),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 180, coolDownOffset: 0, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 170, coolDownOffset: 200, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 160, coolDownOffset: 400, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 150, coolDownOffset: 600, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 140, coolDownOffset: 800, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 130, coolDownOffset: 1000, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 120, coolDownOffset: 1200, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 110, coolDownOffset: 1400, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 100, coolDownOffset: 1600, shootAngle: 90)
                        ),
                    new State("NopeImDead",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xFFFFFF, 2, 2),
                        new MoveTo(speed: 1, x: 38, y: 54),
                        new Taunt(1.00, "NO!! This cannot be how my story ends!! I WILL HAVE MY ENCORE, HERO!"),
                        new TimedTransition(3250, "YepDead")
                        ),
                   new State("YepDead",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TransformOnDeath("Puppet Loot Chest", 1, 1, 1),
                        new Shoot(7, count: 8, projectileIndex: 1, coolDown: 5000),
                        new Suicide()
                        )
                 ),
                                new Threshold(0.01,
                    new ItemLoot("Comedic Clothes", 0.01),
                    new ItemLoot("Laughing Gas", 0.01),
                    new ItemLoot("Loaded Dice", 0.005),
                    new ItemLoot("Potion of Attack", 1),
                    new TierLoot(3, ItemType.Weapon, 0.05),
                    new TierLoot(3, ItemType.Ability, 0.05),
                    new TierLoot(3, ItemType.Armor, 0.05),
                    new TierLoot(3, ItemType.Ring, 0.05),
                    new TierLoot(4, ItemType.Weapon, 0.025),
                    new TierLoot(4, ItemType.Ability, 0.025),
                    new TierLoot(4, ItemType.Armor, 0.025),
                    new TierLoot(4, ItemType.Ring, 0.025),
                    new ItemLoot("Theatre Key", 0.005),
                    new ItemLoot("Rusted Dagger", 0.00112),
                    new ItemLoot("Eternal Essence", 0.0005),
                    new ItemLoot("Jester Pet Stone", 0.001)
                )
            );
    }
}