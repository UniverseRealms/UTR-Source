using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;
//asdfasdfasdf
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ UndeadLair = () => Behav()
#region boss

    #region septavius
            .Init("Septavius the Ghost God",
                new State(
                    new ScaleHP(0.3),
                    new RealmPortalDrop(),
                    new State(
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new PlayerWithinTransition(8, "transition1")
                        ),
                    new State(
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0x00FF00, 0.25, 12),
                        new Wander(0.1),
                        new State("transition1",
                            new TimedTransition(3000, "spiral")
                            ),
                        new State("transition2",
                            new TimedTransition(3000, "ring")
                            ),
                        new State("transition3",
                            new TimedTransition(3000, "quiet")
                            ),
                        new State("transition4",
                            new TimedTransition(3000, "spawn")
                            )
                        ),
                    new State("spiral",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 0),
                        new Spawn("Lair Ghost Archer", 1, 1),
                        new Spawn("Lair Ghost Knight", 2, 1),
                        new Spawn("Lair Ghost Mage", 1, 1),
                        new Spawn("Lair Ghost Rogue", 2, 1),
                        new Spawn("Lair Ghost Paladin", 1, 1),
                        new Spawn("Lair Ghost Warrior", 2, 1),
                        new SpiralShoot(24, 5, 3, coolDown: 200),
                        new TimedTransition(10000, "transition2")
                        ),
                    new State("ring",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 0),
                        new Wander(0.1),
                        new Shoot(10, 12, projectileIndex: 4, coolDown: 2000),
                        new TimedTransition(10000, "transition3")
                        ),
                    new State("quiet",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 0),
                        new Wander(0.1),
                        new Shoot(10, 8, projectileIndex: 1, coolDown: 1000),
                        new Shoot(10, 8, projectileIndex: 1, coolDownOffset: 500, angleOffset: 22.5, coolDown: 1000),
                        new Shoot(8, 3, shootAngle: 20, projectileIndex: 2, coolDown: 2000),
                        new TimedTransition(10000, "transition4")
                        ),
                    new State("spawn",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 0),
                        new Wander(0.1),
                        new Spawn("Ghost Mage of Septavius", 2, 1),
                        new Spawn("Ghost Rogue of Septavius", 2, 1),
                        new Spawn("Ghost Warrior of Septavius", 2, 1),
                        new Reproduce("Ghost Mage of Septavius", densityMax: 2, coolDown: 1000),
                        new Reproduce("Ghost Rogue of Septavius", densityMax: 2, coolDown: 1000),
                        new Reproduce("Ghost Warrior of Septavius", densityMax: 2, coolDown: 1000),
                        new Shoot(8, 3, shootAngle: 10, projectileIndex: 1, coolDown: 1000),
                        new TimedTransition(10000, "transition1")
                        )
                    ),
                 new MostDamagers(3,
                    LootTemplates.Sor1Perc()
                    ),
                new Threshold(0.01,
                    new ItemLoot("Ethereal Bow", 0.01),
                    new ItemLoot("Hallowed Crucifix", 0.01),
                    new ItemLoot("Ectoplasma", 0.005),
                    new ItemLoot("Potion of Wisdom", 1),
                    new TierLoot(3, ItemType.Weapon, 0.05),
                    new TierLoot(3, ItemType.Ability, 0.05),
                    new TierLoot(3, ItemType.Armor, 0.05),
                    new TierLoot(3, ItemType.Ring, 0.05),
                    new TierLoot(4, ItemType.Weapon, 0.025),
                    new TierLoot(4, ItemType.Ability, 0.025),
                    new TierLoot(4, ItemType.Armor, 0.025),
                    new TierLoot(4, ItemType.Ring, 0.025),
                    new ItemLoot("Rusted Robe", 0.000926),
                    new ItemLoot("Eternal Essence", 0.000417),
                    new ItemLoot("Ghostly Pet Stone", 0.001)
                    )
                )
    #endregion septavius

    #region minions
            .Init("Ghost Mage of Septavius",
                new State(
                    new Prioritize(
                        new Protect(0.625, "Septavius the Ghost God", protectionRange: 6),
                        new Follow(0.75, range: 7)
                        ),
                    new Wander(0.25),
                    new Shoot(8, 1, coolDown: 1000)
                    )
                )

            .Init("Ghost Rogue of Septavius",
                new State(
                    new Follow(0.75, range: 1),
                    new Wander(0.25),
                    new Shoot(8, 1, coolDown: 1000)
                    )
                )

            .Init("Ghost Warrior of Septavius",
                new State(
                    new Follow(0.75, range: 1),
                    new Wander(0.25),
                    new Shoot(8, 1, coolDown: 1000)
                    )
                )

            .Init("Lair Ghost Archer",
                new State(
                    new Prioritize(
                        new Protect(0.625, "Septavius the Ghost God", protectionRange: 6),
                        new Follow(0.75, range: 7)
                        ),
                    new Wander(0.25),
                    new Shoot(8, 1, coolDown: 1000)
                    )
                )

            .Init("Lair Ghost Knight",
                new State(
                    new Follow(0.75, range: 1),
                    new Wander(0.25),
                    new Shoot(8, 1, coolDown: 1000)
                    )
                )

            .Init("Lair Ghost Mage",
                new State(
                    new Prioritize(
                        new Protect(0.625, "Septavius the Ghost God", protectionRange: 6),
                        new Follow(0.75, range: 7)
                        ),
                    new Wander(0.25),
                    new Shoot(8, 1, coolDown: 1000)
                    )
                )

            .Init("Lair Ghost Paladin",
                new State(
                    new Follow(0.75, range: 1),
                    new Wander(0.25),
                    new Shoot(8, 1, coolDown: 1000),
                    new HealSelf(coolDown: 5000)
                    )
                )

            .Init("Lair Ghost Rogue",
                new State(
                    new Follow(0.75, range: 1),
                    new Wander(0.25),
                    new Shoot(8, 1, coolDown: 1000)
                    ),
                new ItemLoot("Magic Potion", 0.25),
                new ItemLoot("Health Potion", 0.25)
                )

            .Init("Lair Ghost Warrior",
                new State(
                    new Follow(0.75, range: 1),
                    new Wander(0.25),
                    new Shoot(8, 1, coolDown: 1000)
                    ),
                new ItemLoot("Magic Potion", 0.25),
                new ItemLoot("Health Potion", 0.25)
                )
    #endregion minions

#endregion boss

#region enemies
            .Init("Lair Skeleton",
                new State(
                    new Shoot(6),
                    new Prioritize(
                        new Follow(1, range: 1),
                        new Wander(0.4)
                        )
                    ),
                new ItemLoot("Magic Potion", 0.05),
                new ItemLoot("Health Potion", 0.05)
                )

            .Init("Lair Skeleton King",
                new State(
                    new Shoot(10, 3, shootAngle: 10),
                    new Prioritize(
                        new Follow(1, range: 7),
                        new Wander(0.4)
                        )
                    ),
                new TierLoot(5, ItemType.Armor, 0.2),
                new Threshold(0.5,
                    new TierLoot(6, ItemType.Weapon, 0.2),
                    new TierLoot(7, ItemType.Weapon, 0.1),
                    new TierLoot(8, ItemType.Weapon, 0.05),
                    new TierLoot(6, ItemType.Armor, 0.1),
                    new TierLoot(7, ItemType.Armor, 0.05),
                    new TierLoot(3, ItemType.Ring, 0.1),
                    new TierLoot(3, ItemType.Ability, 0.1)
                    )
                )

            .Init("Lair Skeleton Mage",
                new State(
                    new Shoot(10),
                    new Prioritize(
                        new Follow(1, range: 7),
                        new Wander(0.4)
                        )
                    ),
                new ItemLoot("Magic Potion", 0.05),
                new ItemLoot("Health Potion", 0.05)
                )

            .Init("Lair Skeleton Swordsman",
                new State(
                    new Shoot(5),
                    new Prioritize(
                        new Follow(1, range: 1),
                        new Wander(0.4)
                        )
                    ),
                new ItemLoot("Magic Potion", 0.05),
                new ItemLoot("Health Potion", 0.05)
                )

            .Init("Lair Skeleton Veteran",
                new State(
                    new Shoot(5),
                    new Prioritize(
                        new Follow(1, range: 1),
                        new Wander(0.4)
                        )
                    ),
                new ItemLoot("Magic Potion", 0.05),
                new ItemLoot("Health Potion", 0.05)
                )

            .Init("Lair Mummy",
                new State(
                    new Shoot(10),
                    new Prioritize(
                        new Follow(0.9, range: 7),
                        new Wander(0.4)
                        )
                    ),
                new ItemLoot("Magic Potion", 0.05),
                new ItemLoot("Health Potion", 0.05)
                )

            .Init("Lair Mummy King",
                new State(
                    new Shoot(10),
                    new Prioritize(
                        new Follow(0.9, range: 7),
                        new Wander(0.4)
                        )
                    ),
                new ItemLoot("Magic Potion", 0.05),
                new ItemLoot("Health Potion", 0.05)
                )

            .Init("Lair Mummy Pharaoh",
                new State(
                    new Shoot(10),
                    new Prioritize(
                        new Follow(0.9, range: 7),
                        new Wander(0.4)
                        )
                    ),
                new TierLoot(5, ItemType.Armor, 0.2),
                new Threshold(0.5,
                    new TierLoot(6, ItemType.Weapon, 0.2),
                    new TierLoot(7, ItemType.Weapon, 0.1),
                    new TierLoot(8, ItemType.Weapon, 0.05),
                    new TierLoot(6, ItemType.Armor, 0.1),
                    new TierLoot(7, ItemType.Armor, 0.05),
                    new TierLoot(3, ItemType.Ring, 0.1),
                    new TierLoot(3, ItemType.Ability, 0.1)
                    )
                )

            .Init("Lair Big Brown Slime",
                new State(
                    new Shoot(10, 3, shootAngle: 10, coolDown: 500),
                    new Wander(0.1),
                    new TransformOnDeath("Lair Little Brown Slime", 1, 6, 1)
                    )
                )

            .Init("Lair Little Brown Slime",
                new State(
                    new Shoot(10, 3, shootAngle: 10, coolDown: 500),
                    new Protect(0.1, "Lair Big Brown Slime", acquireRange: 5, protectionRange: 2),
                    new Wander(0.1)
                    ),
                new ItemLoot("Magic Potion", 0.05),
                new ItemLoot("Health Potion", 0.05)
                )

            .Init("Lair Big Black Slime",
                new State(
                    new Shoot(10, coolDown: 1000),
                    new Wander(0.1),
                    new TransformOnDeath("Lair Little Black Slime", 1, 4, 1)
                    )
                )

            .Init("Lair Medium Black Slime",
                new State(
                    new Shoot(10, coolDown: 1000),
                    new Wander(0.1),
                    new TransformOnDeath("Lair Little Black Slime", 1, 4, 1)
                    )
                )

            .Init("Lair Little Black Slime",
                new State(
                    new Shoot(10, coolDown: 1000),
                    new Wander(0.1)
                    ),
                new ItemLoot("Magic Potion", 0.05),
                new ItemLoot("Health Potion", 0.05)
                )

            .Init("Lair Construct Giant",
                new State(
                    new Prioritize(
                        new Follow(0.8, range: 7),
                        new Wander(0.4)
                        ),
                    new Shoot(10, 3, shootAngle: 20, coolDown: 1000),
                    new Shoot(10, projectileIndex: 1, coolDown: 1000)
                    ),
                new TierLoot(5, ItemType.Armor, 0.2),
                new Threshold(0.5,
                    new TierLoot(6, ItemType.Weapon, 0.2),
                    new TierLoot(7, ItemType.Weapon, 0.1),
                    new TierLoot(8, ItemType.Weapon, 0.05),
                    new TierLoot(6, ItemType.Armor, 0.1),
                    new TierLoot(7, ItemType.Armor, 0.05),
                    new TierLoot(3, ItemType.Ring, 0.1),
                    new TierLoot(3, ItemType.Ability, 0.1)
                    )
                )

            .Init("Lair Construct Titan",
                new State(
                    new Prioritize(
                        new Follow(0.8, range: 7),
                        new Wander(0.4)
                        ),
                    new Shoot(10, 3, shootAngle: 20, coolDown: 1000),
                    new Shoot(10, 3, shootAngle: 20, projectileIndex: 1, coolDownOffset: 100, coolDown: 2000)
                    ),
                new TierLoot(5, ItemType.Armor, 0.2),
                new Threshold(0.5,
                    new TierLoot(6, ItemType.Weapon, 0.2),
                    new TierLoot(7, ItemType.Weapon, 0.1),
                    new TierLoot(8, ItemType.Weapon, 0.05),
                    new TierLoot(6, ItemType.Armor, 0.1),
                    new TierLoot(7, ItemType.Armor, 0.05),
                    new TierLoot(3, ItemType.Ring, 0.1),
                    new TierLoot(3, ItemType.Ability, 0.1)
                    )
                )

            .Init("Lair Brown Bat",
                new State(
                    new Wander(0.1),
                    new Charge(3, 8, 2000),
                    new Shoot(3, coolDown: 1000)
                    ),
                new ItemLoot("Magic Potion", 0.05),
                new ItemLoot("Health Potion", 0.05)
                )

            .Init("Lair Ghost Bat",
                new State(
                    new Wander(0.1),
                    new Charge(3, 8, 2000),
                    new Shoot(3, coolDown: 1000)
                    ),
                new ItemLoot("Magic Potion", 0.05),
                new ItemLoot("Health Potion", 0.05)
                )

            .Init("Lair Reaper",
                new State(
                    new Shoot(3, coolDown: 400),
                    new Follow(1.3, range: 1),
                    new Wander(0.1)
                    ),
                new TierLoot(5, ItemType.Armor, 0.2),
                new Threshold(0.5,
                    new TierLoot(6, ItemType.Weapon, 0.2),
                    new TierLoot(7, ItemType.Weapon, 0.1),
                    new TierLoot(8, ItemType.Weapon, 0.05),
                    new TierLoot(6, ItemType.Armor, 0.1),
                    new TierLoot(7, ItemType.Armor, 0.05),
                    new TierLoot(3, ItemType.Ring, 0.1),
                    new TierLoot(3, ItemType.Ability, 0.1)
                    )
                )

            .Init("Lair Vampire",
                new State(
                    new Shoot(10, coolDown: 700),
                    new Shoot(10, coolDown: 1000, projectileIndex: 1),
                    new Follow(1.3, range: 1),
                    new Wander(0.1)
                    ),
                new ItemLoot("Magic Potion", 0.05),
                new ItemLoot("Health Potion", 0.05)
                )

            .Init("Lair Vampire King",
                new State(
                    new Shoot(10, coolDown: 700),
                    new Shoot(10, coolDown: 1000, projectileIndex: 1),
                    new Follow(1.3, range: 1),
                    new Wander(0.1)
                    ),
                new TierLoot(5, ItemType.Armor, 0.2),
                new Threshold(0.5,
                    new TierLoot(6, ItemType.Weapon, 0.2),
                    new TierLoot(7, ItemType.Weapon, 0.1),
                    new TierLoot(8, ItemType.Weapon, 0.05),
                    new TierLoot(6, ItemType.Armor, 0.1),
                    new TierLoot(7, ItemType.Armor, 0.05),
                    new TierLoot(3, ItemType.Ring, 0.1),
                    new TierLoot(3, ItemType.Ability, 0.1)
                    )
                )

            .Init("Lair Grey Spectre",
                new State(
                    new Wander(0.1),
                    new Shoot(10, coolDown: 1000),
                    new Grenade(2.5, 50, 8, coolDown: 1000)
                    )
                )

            .Init("Lair Blue Spectre",
                new State(
                    new Wander(0.1),
                    new Shoot(10, coolDown: 1000),
                    new Grenade(2.5, 70, 8, coolDown: 1000)
                    )
                )

            .Init("Lair White Spectre",
                new State(
                    new Wander(0.1),
                    new Shoot(10, coolDown: 1000),
                    new Grenade(2.5, 90, 8, coolDown: 1000)
                    ),
                new Threshold(0.5,
                    new TierLoot(4, ItemType.Ability, 0.15)
                    )
                )
#endregion enemies

#region traps
           .Init("Lair Burst Trap",
                new State(
                    new State("FinnaBustANut",
                        new PlayerWithinTransition(3, "Aaa")
                        ),
                    new State("Aaa",
                        new Shoot(8.4, count: 12, coolDown: 999999),
                        new Suicide()
                        )
                    ),
                new ItemLoot("Potion of Wisdom", 0.0001)
                )

           .Init("Lair Blast Trap",
                new State(
                    new State("FinnaBustANut",
                        new PlayerWithinTransition(3, "Aaa")
                        ),
                    new State("Aaa",
                        new Shoot(25, count: 12, coolDown: 999999),
                        new Suicide()
                        )
                    ),
                new ItemLoot("Potion of Wisdom", 0.0001)
                );
#endregion traps
    }
}