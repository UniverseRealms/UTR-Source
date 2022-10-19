using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;
//asdfasdfasdf
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Woodland = () => Behav()
#region turrets
            .Init("Woodland Weakness Turret",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Shoot(15, projectileIndex: 0, count: 8, coolDown: 3000, coolDownOffset: 4000)
                    )
                )

            .Init("Woodland Silence Turret",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Shoot(15, projectileIndex: 0, count: 8, coolDown: 3000, coolDownOffset: 4000)
                    )
                )

            .Init("Woodland Paralyze Turret",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Shoot(15, projectileIndex: 0, count: 8, coolDown: 3000, coolDownOffset: 4000)
                    )
                )
#endregion turrets

#region enemies
            .Init("Wooland Armor Squirrel",
                new State(
                    new Prioritize(
                        new Follow(0.52, 8, 2, coolDown: 500),
                        new StayBack(0.7, 4)
                        ),
                    new Shoot(12, projectileIndex: 0, count: 3, shootAngle: 30, coolDown: 700, coolDownOffset: 1000)
                    )
                )

            .Init("Woodland Ultimate Squirrel",
                new State(
                    new Prioritize(
                        new Follow(0.3, 8, 1),
                        new Wander(0.3)
                        ),
                    new Shoot(12, projectileIndex: 0, count: 3, shootAngle: 10, coolDown: 900, coolDownOffset: 1000),
                    new Shoot(12, projectileIndex: 0, count: 1, shootAngle: 35, coolDown: 1100, coolDownOffset: 21000)
                    )
                )

            .Init("Woodland Goblin Mage",
                new State(
                    new Prioritize(
                        new Follow(0.3, 8, 2, coolDown: 500),
                        new StayBack(0.7, 4)
                        ),
                    new Shoot(12, projectileIndex: 0, count: 2, shootAngle: 10, coolDown: 900, coolDownOffset: 1000)
                    )
                )

            .Init("Woodland Goblin",
                new State(
                    new Follow(0.46, 8, 1),
                    new Shoot(12, projectileIndex: 0, count: 1, shootAngle: 20, coolDown: 900, coolDownOffset: 1000)
                    )
                )

            .Init("Woodland Mini Megamoth",
                new State(
                    new Prioritize(
                        new Protect(0.5, "Epic Mama Megamoth", protectionRange: 15, reprotectRange: 3),
                        new Wander(0.35)
                        ),
                    new Shoot(25, projectileIndex: 0, count: 3, shootAngle: 20, coolDown: 700, coolDownOffset: 1000)
                    ),
                new Threshold(0.5,
                    new ItemLoot("Magic Potion", 0.1),
                    new ItemLoot("Magic Potion", 0.1)
                    )
                )

            .Init("Mini Larva",
                new State(
                    new Prioritize(
                        new Protect(0.5, "Murderous Megamoth", protectionRange: 7, reprotectRange: 3),
                        new Wander(0.4)
                        ),
                    new Shoot(25, projectileIndex: 0, count: 6, coolDown: 3500, coolDownOffset: 1200)
                    ),
                new Threshold(0.5,
                    new ItemLoot("Health Potion", 0.01),
                    new ItemLoot("Magic Potion", 0.01)
                    )
                )
#endregion enemies

#region boss
            .Init("Blood Ground Effector",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new ApplySetpiece("Puke"),
                    new Suicide()
                    )
                )

            .Init("Epic Larva",
                new State(
                    new TransformOnDeath("Epic Mama Megamoth", 1, 1),
                    new State(
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Follow(0.08, 8, 1),
                        new Shoot(8.4, count: 4, shootAngle: 90, fixedAngle: 45, projectileIndex: 0, coolDown: 1750),
                        new TossObject("Blood Ground Effector", angle: null, coolDown: 3000),
                        new HpLessTransition(0.75, "tran"),
                        new TimedTransition(24000, "stop puking")
                        ),
                    new State("stop puking",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Follow(0.08, 8, 1),
                        new Shoot(8.4, count: 4, shootAngle: 90, fixedAngle: 45, projectileIndex: 0, coolDown: 1750),
                        new HpLessTransition(0.75, "tran")
                        ),
                    new State("tran",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xFF0000, 2, 2),
                        new TimedTransition(6000, "home")
                        ),
                    new State("home",
                        new Suicide()
                        )
                    )
                )

            .Init("Epic Mama Megamoth",
                new State(
                    new TransformOnDeath("Murderous Megamoth"),
                    new State(
                        new Spawn("Woodland Mini Megamoth", 7, 1, 999999),
                        new Reproduce("Woodland Mini Megamoth", 10, 8, coolDown: 5500),
                        new Prioritize(
                            new Charge(1, range: 4, coolDown: 2000),
                            new Wander(0.4)
                            ),
                        new Shoot(8.4, count: 4, shootAngle: 90, fixedAngle: 35, projectileIndex: 0, coolDown: 2000),
                        new Shoot(8.4, count: 4, shootAngle: 90, fixedAngle: 45, projectileIndex: 0, coolDown: 2000),
                        new Shoot(8.4, count: 4, shootAngle: 90, fixedAngle: 55, projectileIndex: 0, coolDown: 2000),
                        new HpLessTransition(0.70, "tran")
                        ),
                    new State("tran",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xFF0000, 2, 2),
                        new TimedTransition(6000, "home")
                        ),
                    new State("home",
                        new Suicide()
                        )
                    )
                )

            .Init("Murderous Megamoth",
                new State(
                    new ScaleHP(0.3),
                    new RealmPortalDrop(),
                    new Spawn("Mini Larva", 7, 1, 999999),
                    new Reproduce("Mini Larva", 10, 7, coolDown: 4500),
                    new Prioritize(
                        new Charge(1.25, range: 4, coolDown: 2000),
                        new Follow(0.4, 8, 1)
                        ),
                    new Shoot(25, projectileIndex: 1, count: 2, coolDown: 700, coolDownOffset: 1000),
                    new Shoot(8.4, count: 4, shootAngle: 90, angleOffset: 45, projectileIndex: 0, coolDown: 2000, coolDownOffset: 3000)
                    ),
                new Threshold(0.01,
                    new ItemLoot("Fey Quiver", 0.01),
                    new ItemLoot("Terrablossom", 0.01),
                    new ItemLoot("Woodland Crossbow", 0.005),
                    new ItemLoot("Greater Potion of Vitality", 1),
                    new TierLoot(3, ItemType.Weapon, 0.05),
                    new TierLoot(3, ItemType.Ability, 0.05),
                    new TierLoot(3, ItemType.Armor, 0.05),
                    new TierLoot(3, ItemType.Ring, 0.05),
                    new TierLoot(4, ItemType.Weapon, 0.025),
                    new TierLoot(4, ItemType.Ability, 0.025),
                    new TierLoot(4, ItemType.Armor, 0.025),
                    new TierLoot(4, ItemType.Ring, 0.025),
                    new ItemLoot("Woodland Labyrinth Key", 0.005),
                    new ItemLoot("Rusted Bow", 0.00112),
                    new ItemLoot("Eternal Essence", 0.0005),
                    new ItemLoot("Insect Pet Stone", 0.001)
                    )
                );
#endregion boss
    }
}