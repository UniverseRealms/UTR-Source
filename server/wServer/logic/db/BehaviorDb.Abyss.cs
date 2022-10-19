using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Abyss = () => Behav()
            .Init("Archdemon Malphas",
                new State(
                    new ScaleHP(0.3),
                    new RealmPortalDrop(),
                    new State("default",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new PlayerWithinTransition(8, "taunt")
                        ),
                    new State("taunt",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Taunt("You dare to wander my depths?"),
                        new TimedTransition(3000, "attack")
                        ),
                    new State("attack",
                        new Prioritize(
                            new Follow(0.6),
                            new Wander(0.2)
                            ),
                        new Taunt("Fall."),
                        new TossObject("Malphas Missile", range: 5, angleOffset: 90, coolDown: 4000),
                        new Shoot(10, count: 3, shootAngle: 20, coolDown: 1000),
                        new Shoot(10, count: 2, shootAngle: 90, projectileIndex: 3, coolDownOffset: 2000, coolDown: 900),
                        new HpLessTransition(0.8, "attack2")
                        ),
                    new State("attack2",
                        new Prioritize(
                            new Follow(0.3),
                            new Wander(0.2)
                            ),
                        new Taunt("Pathetic."),
                        new TossObject("Malphas Missile", range: 5, angleOffset: 90, coolDown: 2000),
                        new Shoot(10, count: 3, shootAngle: 10, coolDown: 750),
                        new Shoot(10, count: 2, shootAngle: 90, projectileIndex: 3, coolDownOffset: 1000, coolDown: 600),
                        new HpLessTransition(0.6, "big")
                        ),
                    new State("big",
                        new Wander(0.4),
                        new Taunt("Your attacks are pointless."),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ChangeSize(40, 200),
                        new TimedTransition(2000, "bear")
                        ),
                    new State("bear",
                        new Prioritize(
                            new Follow(1, acquireRange: 15, range: 8)
                            ),
                        new Taunt("Bear witness to my incredible power!"),
                        new Grenade(2, 210, 10, coolDown: 850),
                        new Shoot(10, 6, angleOffset: 30, projectileIndex: 1, coolDown: 800),
                        new Shoot(10, 6, projectileIndex: 1, coolDownOffset: 400, coolDown: 1000),
                        new HpLessTransition(0.3, "anger")
                        ),
                    new State("anger",
                        new Prioritize(
                            new Wander(0.4)
                            ),
                        new Taunt("YOU WILL BURN."),
                        new Shoot(10, 5, shootAngle: 7, projectileIndex: 4, coolDown: 200),
                        new SpiralShoot(13, 20, 8, 45, 3, coolDown: 1200)
                            )
                        ),
            	new Threshold(0.01,
                    new ItemLoot("Blade of Burning Souls", 0.01),
                    new ItemLoot("Magmatic Gem", 0.01),
                    new ItemLoot("Skeletal Mail", 0.005),
                    new ItemLoot("Potion of Vitality", 1),
                    new TierLoot(3, ItemType.Weapon, 0.05),
                    new TierLoot(3, ItemType.Ability, 0.05),
                    new TierLoot(3, ItemType.Armor, 0.05),
                    new TierLoot(3, ItemType.Ring, 0.05),
                    new TierLoot(4, ItemType.Weapon, 0.025),
                    new TierLoot(4, ItemType.Ability, 0.025),
                    new TierLoot(4, ItemType.Armor, 0.025),
                    new TierLoot(4, ItemType.Ring, 0.025),
                    new ItemLoot("Rusted Sword", 0.00112),
                    new ItemLoot("Eternal Essence", 0.0005),
                    new ItemLoot("Alluring Pet Stone", 0.001)
                )
            )
            .Init("Malphas Missile",
                new State(
                    new State(
                        new Prioritize(
                            new Follow(0.5, acquireRange: 8, range: 1)
                        ),
                        new Shoot(10, 8, coolDown: 700),
                        new HpLessTransition(0.3, "explode"),
                        new TimedTransition(2000, "explode")
                    ),
                    new State("explode",
                        new Shoot(10, 8),
                        new Decay(100)
                        )
                    )
            )
            .Init("Imp of the Abyss",
                new State(
                    new Wander(0.2),
                    new Shoot(8, 5, 10, coolDown: 3200)
                    ),
                new ItemLoot("Magic Potion", 0.1),
                new ItemLoot("Health Potion", 0.1),
                new ItemLoot("Whip", 0.01),
                new Threshold(0.5,
                    new ItemLoot("Cloak of the Red Agent", 0.01),
                    new ItemLoot("Felwasp Toxin", 0.01)
                    )
            )
            .Init("Demon of the Abyss",
                new State(
                    new Prioritize(
                        new Follow(0.4, 8, 5),
                        new Wander(0.25)
                        ),
                    new Shoot(8, 3, shootAngle: 10, coolDown: 5000)
                    ),
                new ItemLoot("Whip", 0.01),
                new ItemLoot("Fire Bow", 0.05),
                new Threshold(0.5,
                    new ItemLoot("Mithril Armor", 0.01)
                    )
            )
            .Init("Demon Warrior of the Abyss",
                new State(
                    new Prioritize(
                        new Follow(0.5, 8, 5),
                        new Wander(0.25)
                        ),
                    new Shoot(8, 3, shootAngle: 10, coolDown: 3000)
                    ),
                new ItemLoot("Whip", 0.01),
                new ItemLoot("Fire Sword", 0.025),
                new ItemLoot("Steel Shield", 0.025)
            )
            .Init("Demon Mage of the Abyss",
                new State(
                    new Prioritize(
                        new Follow(0.4, 8, 5),
                        new Wander(0.25)
                        ),
                    new Shoot(8, 3, shootAngle: 10, coolDown: 3400)
                    ),
                new ItemLoot("Fire Nova Spell", 0.02),
                new ItemLoot("Whip", 0.01),
                new Threshold(0.1,
                    new ItemLoot("Wand of Dark Magic", 0.01),
                    new ItemLoot("Avenger Staff", 0.01),
                    new ItemLoot("Robe of the Invoker", 0.01),
                    new ItemLoot("Essence Tap Skull", 0.01),
                    new ItemLoot("Demonhunter Trap", 0.01)
                    )
            )
            .Init("Brute of the Abyss",
                new State(
                    new Prioritize(
                        new Follow(0.7, 8, 1),
                        new Wander(0.25)
                        ),
                    new Shoot(8, 3, shootAngle: 10, coolDown: 800)
                    ),
                new ItemLoot("Magic Potion", 0.1),
                new ItemLoot("Whip", 0.01),
                new Threshold(0.1,
                    new ItemLoot("Obsidian Dagger", 0.02),
                    new ItemLoot("Steel Helm", 0.02)
                    )
            )
            .Init("Brute Warrior of the Abyss",
                new State(
                    new Prioritize(
                        new Follow(0.4, 8, 1),
                        new Wander(0.25)
                        ),
                    new Shoot(8, 3, shootAngle: 10, coolDown: 800)
                    ),
                new ItemLoot("Whip", 0.01),
                new ItemLoot("Spirit Salve Tome", 0.02),
                new Threshold(0.5,
                    new ItemLoot("Glass Sword", 0.01),
                    new ItemLoot("Ring of Greater Dexterity", 0.01),
                    new ItemLoot("Magesteel Quiver", 0.01)
                    )
            )

            .Init("Abyss Idol",
                new State(
                    new Shoot(10, 9, 40, projectileIndex: 0, fixedAngle: 0, coolDown: 300),
                    new Shoot(10, 4, 90, projectileIndex: 1, fixedAngle: 2, coolDown: 800),
                    new Shoot(10, 4, 90, projectileIndex: 1, fixedAngle: -2, coolDown: 800),
                    new Shoot(10, 3, 4, projectileIndex: 2, coolDown: 700, predictive: 1)
                    ),
                new Threshold(0.25,
                    new ItemLoot("Blade of Burning Souls", 0.01),
                    new ItemLoot("Magmatic Gem", 0.01),
                    new ItemLoot("Skeletal Mail", 0.005),
                    new ItemLoot("Potion of Vitality", 1),
                    new TierLoot(3, ItemType.Weapon, 0.05),
                    new TierLoot(3, ItemType.Ability, 0.05),
                    new TierLoot(3, ItemType.Armor, 0.05),
                    new TierLoot(3, ItemType.Ring, 0.05),
                    new TierLoot(4, ItemType.Weapon, 0.025),
                    new TierLoot(4, ItemType.Ability, 0.025),
                    new TierLoot(4, ItemType.Armor, 0.025),
                    new TierLoot(4, ItemType.Ring, 0.025),
                    new ItemLoot("Rusted Sword", 0.00112),
                    new ItemLoot("Eternal Essence", 0.0005)
                    )
                );
    }
}