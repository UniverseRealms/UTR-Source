using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ QueenofHearts = () => Behav()
        .Init("Queen of Hearts",
                new State(
                    new ScaleHP(0.3),
                    new State("StartQueen",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(1000, "Taunt")
                        ),
                    new State("Taunt",
                        new Taunt("Guards, kill these troublemakers!"),
                        new TimedTransition(200, "Phase One")
                        ),
                    new State("Phase One",
                        new Reproduce("Card Knight Red", 20, 5, coolDown: 4000),
                        new Reproduce("Card Knight Black", 20, 5, coolDown: 4000),
                        new Shoot(25, projectileIndex: 3, count: 8, shootAngle: 45, coolDown: 2000),
                        new HpLessTransition(.800, "Taunt2")
                        ),
                    new State("Taunt2",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("My guards that is enough, I'll do it alone!"),
                        new TimedTransition(200, "Phase Two")
                        ),
                    new State("Phase Two",
                        new Shoot(22, count: 5, projectileIndex: 1, shootAngle: 10, coolDown: 2000),
                        new Shoot(22, count: 3, projectileIndex: 0, shootAngle: 15, coolDown: 1000),
                        new StayAbove(.6, 1),
                        new Wander(.2),
                        new StayBack(.1, distance: 4),
                        new StayCloseToSpawn(0.4, 8),
                        new TimedTransition(2000, "Phase Two"),
                        new HpLessTransition(.500, "Phase Three")
                        ),
                    new State("Phase Three",
                        new Shoot(22, count: 3, projectileIndex: 2, shootAngle: 15, coolDown: 750),
                        new Shoot(25, projectileIndex: 0, count: 8, shootAngle: 45, coolDown: 2000),
                        new Wander(.2),
                        new StayCloseToSpawn(0.4, 3),
                        new TimedTransition(750, "Phase Three"),
                        new HpLessTransition(.150, "Taunt3")
                        ),
                    new State("Taunt3",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("Guard protect me, if I die, Wonderland will die with me!"),
                        new Flash(0xC90015, 4, 4),
                        new TimedTransition(5000, "Phase Four")
                        ),
                        new State("Phase Four",
                            new StayBack(.1, distance: 4),
                            new Wander(0.2),
                            new Reproduce("Card Knight Red", 20, 5, coolDown: 4000),
                            new OrderOnce(99, "Card Knight Red", "Rage"),
                            new Reproduce("Card Knight Black", 20, 5, coolDown: 4000),
                            new OrderOnce(99, "Card Knight Black", "Rage"),
                            new Shoot(22, count: 2, projectileIndex: 3, shootAngle: 10, coolDown: 1000),
                            new Shoot(25, projectileIndex: 1, count: 8, shootAngle: 45, coolDown: 2000),
                            new HpLessTransition(.005, "Last Taunt")
                                ),
                            new State("Last Taunt",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                new Taunt("How could i be this weak i was supposed to rule the world :("),
                                new TimedTransition(3000, "Suicide")
                                ),
                    new State("Suicide",
                        new Suicide()
                        )
                    ),
                new Threshold(0.01,
                    new ItemLoot("Royal Dice", 0.0084),
                    new ItemLoot("Queen's Robes", 0.0046),
                    new ItemLoot("Stat Potion Crate", 1),
                    new TierLoot(2, ItemType.Weapon, 0.05),
                    new TierLoot(2, ItemType.Ability, 0.05),
                    new TierLoot(2, ItemType.Armor, 0.05),
                    new TierLoot(2, ItemType.Ring, 0.05),
                    new TierLoot(3, ItemType.Weapon, 0.025),
                    new TierLoot(3, ItemType.Ability, 0.025),
                    new TierLoot(3, ItemType.Armor, 0.025),
                    new TierLoot(3, ItemType.Ring, 0.025),
                    new ItemLoot("Rusted Light Abilities Chest", 0.00143),
                    new ItemLoot("Eternal Essence", 0.0005)
            )
            )
       .Init("Card Knight Black",
            new State(
              new State("Start2",
                new Protect(0.4, "Queen of Hearts", 4, 4, 1),
                new Wander(0.2),
                new Shoot(22, count: 2, projectileIndex: 0, shootAngle: 10, coolDown: 2000)
                ),
              new State("Rage",
                  new Shoot(22, count: 2, projectileIndex: 0, shootAngle: 10, coolDown: 1000),
                  new Wander(0.5),
                  new Follow(0.7, range: 2, coolDown: 0)
                )
            )
            )
         .Init("Card Knight Red",
                new State(
            new State("Start",
                new Protect(0.4, "Queen of Hearts", 3, 3, 1),
                new Wander(0.2),
                new Shoot(22, count: 2, projectileIndex: 0, shootAngle: 10, coolDown: 1000)
                ),
            new State("Rage",
                  new Shoot(22, count: 2, projectileIndex: 0, shootAngle: 10, coolDown: 1000),
                  new Wander(0.5),
                  new Follow(0.7, range: 2, coolDown: 0)
            )
                    )
                    )
        ;
    }
}