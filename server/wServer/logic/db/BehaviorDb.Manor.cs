﻿#region

using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

#endregion

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Manor = () => Behav()
        //lord ruthven is waaay unfinished
                    .Init("Lord Ruthven",
                new State(
                    new ScaleHP(0.3),
                    new RealmPortalDrop(),
                    new State("default",
                        new PlayerWithinTransition(8, "spooksters")
                        ),
                    new State("spooksters",
                        new Wander(0.2),
                        new Shoot(10, count: 5, shootAngle: 2, projectileIndex: 0, coolDown: 900),
                        new TimedTransition(6000, "spooksters2")
                        ),
                    new State("spooksters2",
                        new Wander(0.15),
                        new Shoot(8.4, count: 40, projectileIndex: 1, coolDown: 2750),
                        new Shoot(10, count: 5, shootAngle: 2, projectileIndex: 0, coolDown: 900),
                        new TimedTransition(4000, "spooksters3")
                        ),
                    new State("spooksters3",
                        new Shoot(8.4, count: 40, projectileIndex: 1, coolDown: 2750),
                        new TimedTransition(4000, "spooksters")
                            )
                        ),
                new Threshold(0.01,
                    new ItemLoot("Bloodmage's Wand", 0.01),
                    new ItemLoot("Exorcism Skull", 0.01),
                    new ItemLoot("The Forth Guiding Flag", 0.005),
                    new ItemLoot("Potion of Vitality", 1),
                    new TierLoot(3, ItemType.Weapon, 0.05),
                    new TierLoot(3, ItemType.Ability, 0.05),
                    new TierLoot(3, ItemType.Armor, 0.05),
                    new TierLoot(3, ItemType.Ring, 0.05),
                    new TierLoot(4, ItemType.Weapon, 0.025),
                    new TierLoot(4, ItemType.Ability, 0.025),
                    new TierLoot(4, ItemType.Armor, 0.025),
                    new TierLoot(4, ItemType.Ring, 0.025),
                    new ItemLoot("Manor Key", 0.005),
                    new ItemLoot("Rusted Wand", 0.00112),
                    new ItemLoot("Eternal Essence", 0.0005),
                    new ItemLoot("Vampiric Pet Stone", 0.001)
                    )
            )
            .Init("Hellhound",
                new State(
                    new Follow(1.25, 8, 1, coolDown: 400),
                    new Shoot(10, count: 5, shootAngle: 7, coolDown: 2000)
                    ),
                new ItemLoot("Magic Potion", 0.05),
                new Threshold(0.5,
                    new ItemLoot("Timelock Orb", 0.01)
                    )
            )
                    .Init("Vampire Bat Swarmer",
                new State(
                    new Follow(1.5, 8, 1),
                    new Shoot(10, count: 1, coolDown: 400)
                    )
            )
                    .Init("Lil Feratu",
                new State(
                    new Follow(0.35, 8, 1),
                    new Shoot(10, count: 6, shootAngle: 2, coolDown: 900)
                    ),
                new ItemLoot("Health Potion", 0.05),
                new Threshold(0.5,
                    new ItemLoot("Steel Helm", 0.01)
                    )
            )
                            .Init("Lesser Bald Vampire",
                new State(
                    new Follow(0.35, 8, 1),
                    new Shoot(10, count: 5, shootAngle: 6, coolDown: 1000)
                    ),
                new ItemLoot("Health Potion", 0.05),
                new Threshold(0.5,
                    new ItemLoot("Steel Helm", 0.01)
                    )
            )
                  .Init("Nosferatu",
                new State(
                    new Wander(0.25),
                    new Shoot(10, count: 5, shootAngle: 2, projectileIndex: 1, coolDown: 1000),
                    new Shoot(10, count: 6, shootAngle: 90, projectileIndex: 0, coolDown: 1500)
                    ),
                new ItemLoot("Health Potion", 0.05),
                new Threshold(0.5,
                    new ItemLoot("Bone Dagger", 0.01),
                    new ItemLoot("Wand of Death", 0.05),
                    new ItemLoot("Golden Bow", 0.04),
                    new ItemLoot("Steel Helm", 0.05),
                    new ItemLoot("Ring of Paramount Defense", 0.09)
                    )
            )
                          .Init("Armor Guard",
                new State(
                    new Wander(0.2),
                    new TossObject("RockBomb", 7, coolDown: 3000),
                    new Shoot(10, count: 1, projectileIndex: 0, predictive: 7, coolDown: 1000),
                    new Shoot(10, count: 1, projectileIndex: 1, coolDown: 750)
                    ),
                new ItemLoot("Magic Potion", 0.05),
                new Threshold(0.5,
                    new ItemLoot("Glass Sword", 0.01),
                    new ItemLoot("Staff of Destruction", 0.01),
                    new ItemLoot("Golden Shield", 0.01),
                    new ItemLoot("Ring of Paramount Speed", 0.01)
                    )
            )
                                  .Init("Coffin Creature",
                new State(
                    new Spawn("Lil Feratu", initialSpawn: 1, maxChildren: 2, coolDown: 2250),
                    new Shoot(10, count: 1, projectileIndex: 0, coolDown: 700)
                    ),
                new ItemLoot("Magic Potion", 0.05)
            )
                              .Init("RockBomb",
                        new State(
                    new State("BOUTTOEXPLODE",
                    new TimedTransition(1111, "boom")
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
           .Init("Coffin",
                        new State(
                    new State("Coffin1",
                        new HpLessTransition(0.75, "Coffin2")
                        ),
                    new State("Coffin2",
                        new Spawn("Vampire Bat Swarmer", initialSpawn: 1, maxChildren: 15, coolDown: 99999),
                         new HpLessTransition(0.40, "Coffin3")
                        ),
                       new State("Coffin3",
                           new Spawn("Vampire Bat Swarmer", initialSpawn: 1, maxChildren: 8, coolDown: 99999),
                            new Spawn("Nosferatu", initialSpawn: 1, maxChildren: 2, coolDown: 99999)
                        )
                ),
                new Threshold(0.5,
                    new ItemLoot("Holy Water", 1.00)
                    )
            );
    }
}