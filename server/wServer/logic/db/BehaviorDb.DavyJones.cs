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
        private _ DavyJones = () => Behav()
        .Init("Davy Jones",
                new State(
                    new ScaleHP(0.3),
                    new State("Floating",
                        new ChangeSize(100, 100),
                        new SetAltTexture(1),
                        new SetAltTexture(3),
                        new Wander(.2),
                        new Shoot(10, 5, 10, 0, coolDown: 2000),
                        new Shoot(10, 1, 10, 1, coolDown: 4000),
                        new EntityNotExistsTransition("Ghost Lanturn Off", 10, "Vunerable"),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable)
                        ),
                    new State("CheckOffLanterns",
                        new SetAltTexture(2),
                        new StayCloseToSpawn(.1, 3),
                        new EntityNotExistsTransition("Ghost Lanturn Off", 10, "Vunerable")
                        ),
                    new State("Vunerable",
                        new SetAltTexture(2),
                        new StayCloseToSpawn(.1, 0),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(2500, "deactivate")
                        ),
                    new State("deactivate",
                        new SetAltTexture(2),
                        new StayCloseToSpawn(.1, 0),
                        new EntityNotExistsTransition("Ghost Lanturn On", 10, "Floating")
                            )
                        ),
                new Threshold(0.01,
                    new ItemLoot("Draped Hide", 0.01),
                    new ItemLoot("Ghastly Shank", 0.01),
                    new ItemLoot("Phantasm Katana", 0.005),
                    new ItemLoot("Greater Potion of Wisdom", 1),
                    new TierLoot(3, ItemType.Weapon, 0.05),
                    new TierLoot(3, ItemType.Ability, 0.05),
                    new TierLoot(3, ItemType.Armor, 0.05),
                    new TierLoot(3, ItemType.Ring, 0.05),
                    new TierLoot(4, ItemType.Weapon, 0.025),
                    new TierLoot(4, ItemType.Ability, 0.025),
                    new TierLoot(4, ItemType.Armor, 0.025),
                    new TierLoot(4, ItemType.Ring, 0.025),
                    new ItemLoot("Davy's Key", 0.005),
                    new ItemLoot("Rusted Leather Hide", 0.00112),
                    new ItemLoot("Eternal Essence", 0.0005),
                    new ItemLoot("Spectral Pet Stone", 0.001)
                    )
                )
           .Init("Ghost Lanturn Off",
            new State(
                new State("default",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new EntityNotExistsTransition("Yellow Key", 9999, "gogogo")
                        ),
                new State("gogogo",
                    new TransformOnDeath("Ghost Lanturn On")
                    )
                    )
            )

            .Init("Lost Soul",
                new State(
                    new State("Default",
                        new Prioritize(
                            new Orbit(0.3, 3, 20, "Ghost of Roger"),
                            new Wander(0.1)
                            ),
                        new PlayerWithinTransition(4, "Default1")
                        ),
                    new State("Default1",
                       new Charge(0.5, 8, coolDown: 2000),
                       new TimedTransition(2200, "Blammo")
                        ),
                     new State("Blammo",
                       new Shoot(10, count: 6, projectileIndex: 0, coolDown: 2000),
                       new Suicide()
                    )
                )
            ).Init("Ghost of Roger",
                new State(
                    new State("spawn",
                        new Spawn("Lost Soul", 3, 1, 5000),
                        new TimedTransition(200, "Attack")
                    ),
                    new State("Attack",
                        new Shoot(13, 1, 0, 0, coolDown: 400),
                        new TimedTransition(200, "Attack2")
                    ),
                    new State("Attack2",
                        new Shoot(13, 1, 0, 0, coolDown: 400),
                        new TimedTransition(200, "Attack3")
                    ),
                    new State("Attack3",
                        new Shoot(13, 1, 0, 0, coolDown: 400),
                        new TimedTransition(200, "Wait")
                    ),
                    new State("Wait",
                        new TimedTransition(1000, "Attack")
                    )
                )
            )

            .Init("Purple Key",
                new State(
                        new RemoveObjectOnDeath("GhostShip PurpleDoor Lf", 99),
                        new RemoveObjectOnDeath("GhostShip PurpleDoor Rt", 99),
                    new State("Idle",
                        new PlayerWithinTransition(1, "Cycle")

                    ),
                    new State("Cycle",
                        new Taunt(true, "Purple Key has been found!"),
                        new Suicide()

                    )
                )
            )
            .Init("Red Key",
                new State(
                        new RemoveObjectOnDeath("GhostShip RedDoor Lf", 999),
                        new RemoveObjectOnDeath("GhostShip RedDoor Rt", 999),
                    new State("Idle",
                        new PlayerWithinTransition(1, "Cycle")

                    ),
                    new State("Cycle",
                        new Taunt(true, "Red Key has been found!"),
                        new Suicide()

                    )
                )
            )
            .Init("Green Key",
                new State(
                                            new RemoveObjectOnDeath("GhostShip GreenDoor Lf", 99),
                        new RemoveObjectOnDeath("GhostShip GreenDoor Rt", 99),
                    new State("Idle",
                        new PlayerWithinTransition(1, "Cycle")

                    ),
                    new State("Cycle",
                        new Taunt(true, "Green Key has been found!"),
                        new Suicide()

                    )
                )
            )
            .Init("Yellow Key",
                new State(
                        new RemoveObjectOnDeath("GhostShip YellowDoor Lf", 99),
                        new RemoveObjectOnDeath("GhostShip YellowDoor Rt", 99),
                    new State("Idle",
                        new PlayerWithinTransition(1, "Cycle")

                    ),
                    new State("Cycle",
                        new Taunt(true, "Yellow Key has been found!"),
                        new Suicide()

                    )
                )
            )
            .Init("Ghost Lanturn On",
                new State(
                    new State("idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(5000, "deactivate")
                        ),
                    new State("deactivate",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Ghost Lanturn Off", 10, "shoot"),
                        new TimedTransition(10000, "gone")
                        ),
                    new State("shoot",
                        new Shoot(10, 6, coolDown: 9000001, coolDownOffset: 100),
                        new TimedTransition(1000, "gone")
                        ),
                    new State("gone",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Transform("Ghost Lanturn Off")
                        )
                    )
            )
            .Init("Lil' Ghost Pirate",
                new State(
                    new ChangeSize(30, 120),
                    new Shoot(10, count: 1, projectileIndex: 0, coolDown: 2000),
                    new State("Default",
                        new Prioritize(
                            new Follow(0.6, 8, 1),
                            new Wander(0.1)
                            ),
                        new TimedTransition(2850, "Default1")
                        ),
                    new State("Default1",
                       new StayBack(0.2, 3),
                       new TimedTransition(1850, "Default")
                    )
                )
            )
                 .Init("Zombie Pirate Sr",
                new State(
                    new Shoot(10, count: 1, projectileIndex: 0, coolDown: 2000),
                    new State("Default",
                        new Prioritize(
                            new Follow(0.3, 8, 1),
                            new Wander(0.1)
                            ),
                        new TimedTransition(2850, "Default1")
                        ),
                    new State("Default1",
                       new ConditionalEffect(ConditionEffectIndex.Armored),
                       new Prioritize(
                            new Follow(0.3, 8, 1),
                            new Wander(0.1)
                            ),
                        new TimedTransition(2850, "Default")
                    )
                )
            )
           .Init("Zombie Pirate Jr",
                new State(
                    new Shoot(10, count: 1, projectileIndex: 0, coolDown: 2500),
                    new State("Default",
                        new Prioritize(
                            new Follow(0.4, 8, 1),
                            new Wander(0.1)
                            ),
                        new TimedTransition(2850, "Default1")
                        ),
                    new State("Default1",
                       new Swirl(0.2, 3),
                       new TimedTransition(1850, "Default")
                    )
                )
            )
        .Init("Captain Summoner",
                new State(
                    new State("Default",
                        new ConditionalEffect(ConditionEffectIndex.Invincible)
                        )
                )
            )
           .Init("GhostShip Rat",
                new State(
                    new State("Default",
                        new Shoot(10, count: 1, projectileIndex: 0, coolDown: 1750),
                        new Prioritize(
                            new Follow(0.55, 8, 1),
                            new Wander(0.1)
                            )
                        )
                )
            )
        .Init("Violent Spirit",
                new State(
                    new State("Default",
                        new ChangeSize(35, 120),
                        new Shoot(10, count: 3, projectileIndex: 0, coolDown: 1750),
                        new Prioritize(
                            new Follow(0.25, 8, 1),
                            new Wander(0.1)
                            )
                        )
                )
            )
           .Init("School of Ghostfish",
                new State(
                    new State("Default",
                        new Shoot(10, count: 3, shootAngle: 18, projectileIndex: 0, coolDown: 4000),
                        new Wander(0.35)
                        )
                )
            );
    }
}