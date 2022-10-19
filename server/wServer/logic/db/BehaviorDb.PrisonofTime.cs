﻿using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ PrisonofTime = () => Behav()
        .Init("Timelord's Mage",
            new State(
                new State("fight1",
                new Prioritize(
                        new Follow(0.6, 8, 1),
                        new Wander(1)
                        ),
                     new Shoot(10, count: 6, projectileIndex: 0, coolDown: 2000),
                     new TimedTransition(6000, "fight2")
                    ),
                new State("fight2",
                     new ConditionalEffect(ConditionEffectIndex.Armored),
                     new Shoot(10, count: 4, shootAngle: 32, projectileIndex: 1, coolDown: 400),
                     new TimedTransition(3000, "fight1")
                    )
                )
            )
        .Init("Time Sentinel",
            new State(
                new DamageTakenTransition(1600, "Rage"),
                new State("fight1",
                new Prioritize(
                        new Follow(0.4, 8, 1),
                        new Wander(1)
                        ),
                     new Shoot(10, count: 1, projectileIndex: 0, coolDown: 1000),
                     new TimedTransition(4000, "fight2")
                    ),
                new State("fight2",
                    new Prioritize(
                        new Follow(0.8, 8, 1),
                        new Wander(1)
                        ),
                     new Shoot(10, count: 4, projectileIndex: 0, coolDown: 1000),
                     new TimedTransition(3000, "fight1")
                    ),
                new State("Rage",
                     new Prioritize(
                        new Follow(1, 8, 1),
                        new Wander(0.5)
                        ),
                     new Shoot(10, count: 6, projectileIndex: 0/*index was 1, fix later when you have a spriter*/, coolDown: 3600)
                    )
                )
            )
        .Init("Time Warrior 1",
                new State(
                    new Prioritize(
                        new Follow(1.5, 8, 1),
                        new Wander(0.25)
                        ),
                    new Shoot(8, 1, shootAngle: 10, coolDown: 500)
                    ),
                new ItemLoot("Magic Potion", 0.1),
                new Threshold(0.1,
                    new ItemLoot("Golden Shield", 0.02),
                    new ItemLoot("Steel Helm", 0.02)
                    )
            )
        .Init("Time Warrior 2",
                new State(
                    new Prioritize(
                        new Follow(1.5, 8, 1),
                        new Wander(0.25)
                        ),
                    new Shoot(8, 1, shootAngle: 10, coolDown: 500)
                    ),
                new ItemLoot("Health Potion", 0.1),
                new Threshold(0.1,
                    new ItemLoot("Cloak of the Red Agent", 0.02),
                    new ItemLoot("Timelock Orb", 0.02)
                    )
            )
        .Init("Time Energy Void",
            new State(
                new State("fight1",
                     new Wander(0.6),
                     new Shoot(10, count: 4, shootAngle: 14, projectileIndex: 0, coolDown: 400),
                     new TimedTransition(3000, "fight2")
                    ),
                new State("fight2",
                     new Swirl(0.5, 4),
                     new ConditionalEffect(ConditionEffectIndex.ArmorBroken),
                     new Shoot(10, count: 10, projectileIndex: 1, coolDown: 400),
                     new TimedTransition(2600, "fight1")
                    )
                )
            )
        .Init("Time Turret 1",
            new State(
                new State("shootgrenade",
                     new ConditionalEffect(ConditionEffectIndex.Invincible),
                     new Grenade(4, 75, range: 4, coolDown: 750)
                    )
                )
            ) 
         .Init("Time Turret 2",
            new State(
                new State("shootgrenade",
                     new ConditionalEffect(ConditionEffectIndex.Invincible),
                     new Shoot(10, count: 8, projectileIndex: 0, coolDown: new Cooldown(4000, 2000)),
                     new EntitiesNotExistsTransition(50, "ded", "Galleom of Time")
                    ),
                new State("ded",
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                    )
                )
            )
        .Init("Timelord Blocker",
            new State(
                new State("fight1",
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new Orbit(0.6, 3, target: "Galleom of Time")
                    ),
                new State("Flash",
                     new Flash(0x00FF00, 1, 1),
                     new TimedTransition(2200, "Rush")
                    ),
                new State("Rush",
                     new Charge(2, 6, coolDown: 9999),
                     new TimedTransition(1200, "Explode")
                    ),
                new State("Explode",
                     new Shoot(10, count: 4, projectileIndex: 0, coolDown: 9999),
                     new Suicide()
                    )
                )
            )
        .Init("Galleom of Time",
                new State(
                    new HpLessTransition(0.12, "Rage"),
                    new State("default",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new PlayerWithinTransition(7, "taunt")
                        ),
                    new State(
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("taunt",
                        new Taunt("Time stops for no one. I'm quite sure you can't stop me.", "Soon time will bind to my will!"),
                        new TimedTransition(5000, "taunt2")
                        ),
                    new State("taunt2",
                        new Taunt("I shall banish you from existence!"),
                        new TimedTransition(1000, "Fight1")
                        )
                      ),
                    new State("Fight1",
                        new Prioritize(
                            new Follow(0.4, 10, 1),
                            new Wander(1)
                            ),
                        new Shoot(10, count: 2, shootAngle: 28, projectileIndex: 1, coolDown: 2000),
                        new Shoot(10, count: 3, shootAngle: 12, projectileIndex: 0, coolDown: 2000),
                        new Shoot(10, count: 6, projectileIndex: 3, coolDown: 4000),
                        new TimedTransition(8400, "Fight2")
                        ),
                    new State("Fight2",
                        new ReturnToSpawn(speed: 1),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Shoot(10, count: 12, projectileIndex: 0, coolDown: 4000),
                        new TimedTransition(4000, "NadeBarrage")
                        ),
                    new State("NadeBarrage",
                        new Taunt(0.60, "You won't live beyond this point!"),
                        new Shoot(10, count: 4, projectileIndex: 0, coolDown: 4000),
                        new Shoot(10, count: 7, shootAngle: 18, projectileIndex: 1, coolDown: 1000),
                        new Grenade(2, 160, range: 8, coolDown: 600),
                        new TimedTransition(6500, "LaserBeamSH")
                        ),
                    new State("LaserBeamSH",
                        new Prioritize(
                            new Follow(0.6),
                            new Wander(0.1)
                            ),
                        new Shoot(10, count: 1, projectileIndex: 3, coolDown: 400),
                        new Shoot(10, count: 6, projectileIndex: 0, coolDown: 2000),
                        new TimedTransition(9200, "Fight3"),
                        new State("Beam1",
                            new Shoot(8.4, count: 1, fixedAngle: 0, projectileIndex: 2, coolDown: 400),
                            new Shoot(8.4, count: 1, fixedAngle: 180, projectileIndex: 2, coolDown: 400),
                            new TimedTransition(2000, "Beam2")
                        ),
                        new State("Beam2",
                            new Shoot(8.4, count: 1, fixedAngle: 90, projectileIndex: 2, coolDown: 400),
                            new Shoot(8.4, count: 1, fixedAngle: 270, projectileIndex: 2, coolDown: 400),
                            new TimedTransition(2000, "Beam1")
                        )
                      ),
                    new State("Fight3",
                        new ReturnToSpawn(speed: 1),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(10, count: 6, projectileIndex: 2, coolDown: 1000),
                        new TimedTransition(3000, "Blocker")
                        ),
                    new State("Blocker",
                        new Taunt("Onslaught! Be fearful!"),
                        new TossObject("Timelord Blocker", 3, 0, coolDown: 9999999),
                        new TossObject("Timelord Blocker", 3, 45, coolDown: 9999999),
                        new TossObject("Timelord Blocker", 3, 90, coolDown: 9999999),
                        new TossObject("Timelord Blocker", 3, 135, coolDown: 9999999),
                        new TossObject("Timelord Blocker", 3, 180, coolDown: 9999999),
                        new TossObject("Timelord Blocker", 3, 225, coolDown: 9999999),
                        new TossObject("Timelord Blocker", 3, 270, coolDown: 9999999),
                        new TossObject("Timelord Blocker", 3, 315, coolDown: 9999999),
                        new Shoot(10, count: 5, shootAngle: 10, projectileIndex: 0, coolDown: 4000, coolDownOffset: 2000),
                        new TimedTransition(5400, "NextUp")
                        ),
                    new State("NextUp",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Order(999, "Timelord Blocker", "Flash"),
                        new Shoot(10, count: 4, shootAngle: 24, projectileIndex: 3, predictive: 2, coolDown: 2000, coolDownOffset: 2000),
                        new TimedTransition(6500, "Fight1")
                        ),
                    new State("Rage",
                        new Flash(0xFF0000, 2, 2),
                        new Prioritize(
                            new Follow(0.8, 10, 1),
                            new Wander(1)
                            ),
                        new Shoot(10, count: 6, shootAngle: 18, projectileIndex: 0, predictive: 1, coolDown: 3600, coolDownOffset: 2000),
                        new Shoot(10, count: 3, projectileIndex: 3, coolDown: 500)
                        )
                    ),
                new Threshold(0.01,
                    new ItemLoot("Outland Medallion", 0.01),
                    new ItemLoot("Potion of Defense", 1),
                    new TierLoot(3, ItemType.Weapon, 0.05),
                    new TierLoot(3, ItemType.Ability, 0.05),
                    new TierLoot(3, ItemType.Armor, 0.05),
                    new TierLoot(3, ItemType.Ring, 0.05),
                    new TierLoot(4, ItemType.Weapon, 0.025),
                    new TierLoot(4, ItemType.Ability, 0.025),
                    new TierLoot(4, ItemType.Armor, 0.025),
                    new TierLoot(4, ItemType.Ring, 0.025),
                    new ItemLoot("Rusted Medium Abilities Chest", 0.00125),
                    new ItemLoot("Eternal Essence", 0.0005)
                )
            )
            ;
    }
}