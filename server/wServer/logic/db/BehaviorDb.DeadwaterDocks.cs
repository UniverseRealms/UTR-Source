using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ DeadwaterDocks = () => Behav()
              .Init("Deadwater Docks Parrot",
                  new State(
                    new ScaleHP(0.1),
                    new EntityNotExistsTransition("Jon Bilgewater the Pirate King", 90000, "rip"),
                    new State("CircleOrWander",
                        new Prioritize(
                            new Orbit(0.55, 2, 99, "Parrot Cage"),
                            new Wander(0.12)
                            )
                        ),
                    new State("Orbit&HealJon",
                    new Orbit(0.55, 2, 20, "Jon Bilgewater the Pirate King"),
                    new HealSelf(coolDown: 2000, amount: 100)

                    ),
                    new State("rip",
                    new Suicide()
                    )
                 )
              )
              .Init("Parrot Cage",
                  new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new EntityNotExistsTransition("Jon Bilgewater the Pirate King", 90000, "NoSpawn"),
                    new State("NoSpawn"
                        ),
                    new State("SpawnParrots",
                    new Reproduce("Deadwater Docks Parrot", densityRadius: 5, densityMax: 5, coolDown: 2500)
                    ),
                    new State("Shoot",
                    new Reproduce("Deadwater Docks Parrot", densityRadius: 5, densityMax: 5, coolDown: 2500),
                    new Shoot(99, 2, null, 0, 90, coolDown: 200, coolDownOffset: 1500)
                    )
                 )
              )
                .Init("DDocks Helper",
                  new State(
                    new EntityNotExistsTransition("Jon Bilgewater the Pirate King", 90000, "die"),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("Shoot",
                    new Shoot(99, 4, null, 0, 45, coolDown: 200, coolDownOffset: 1500)
                        ),
                    new State("die",
                        new Suicide()
                        )
                    )
                )
                .Init("DDocks Helper 2",
                  new State(
                    new EntityNotExistsTransition("Jon Bilgewater the Pirate King", 90000, "die"),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("Shoot",
                    new Shoot(99, 4, null, 0, -45, coolDown: 200, coolDownOffset: 1500)
                        ),
                    new State("die",
                        new Suicide()
                    )
                )
            )
                .Init("DDocks Helper 3",
                  new State(
                    new EntityNotExistsTransition("Jon Bilgewater the Pirate King", 90000, "die"),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("Shoot",
                    new Shoot(99, 2, null, 0, 90, coolDown: 200, coolDownOffset: 1500)
                        ),
                    new State("die",
                        new Suicide()
                    )
                )
            )
             .Init("Bottled Evil Water",
                 new State(
                    new State("water",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new TimedTransition(2000, "drop")
                        ),
                    new State("drop",
                       new ApplySetpiece("BottledEvil"),
                       new Suicide()
                    )))
          .Init("Deadwater Docks Lieutenant",
                new State(
                    new ScaleHP(0.1),
                    new Follow(1, 8, 1),
                    new Shoot(8, 1, 10, coolDown: 1000),
                    new TossObject("Bottled Evil Water", angle: null, coolDown: 6750)
                    ),
                new ItemLoot("Magic Potion", 0.1),
                new ItemLoot("Health Potion", 0.1)
            )
          .Init("Deadwater Docks Veteran",
                new State(
                    new ScaleHP(0.1),
                    new Follow(0.8, 8, 1),
                    new Shoot(8, 1, 10, coolDown: 500)
                    ),
                new ItemLoot("Magic Potion", 0.1),
                new ItemLoot("Health Potion", 0.1)
            )
          .Init("Deadwater Docks Admiral",
                new State(
                    new ScaleHP(0.1),
                    new Follow(0.6, 8, 1),
                    new Shoot(8, 3, 10, coolDown: 1325)
                    ),
                new ItemLoot("Magic Potion", 0.1),
                new ItemLoot("Health Potion", 0.1)
            )
          .Init("Deadwater Docks Brawler",
                new State(
                    new ScaleHP(0.1),
                    new Follow(1.12, 8, 1),
                    new Shoot(8, 1, 10, coolDown: 400)
                    ),
                new ItemLoot("Magic Potion", 0.1),
                new ItemLoot("Health Potion", 0.1)
            )
          .Init("Deadwater Docks Sailor",
                new State(
                    new ScaleHP(0.1),
                    new Follow(0.9, 8, 1),
                    new Shoot(8, 1, 10, coolDown: 525)
                    ),
                new ItemLoot("Magic Potion", 0.1),
                new ItemLoot("Health Potion", 0.1)
            )
          .Init("Deadwater Docks Commander",
                new State(
                    new ScaleHP(0.1),
                    new Follow(0.90, 8, 1),
                    new Shoot(8, 1, 10, coolDown: 900),
                    new TossObject("Bottled Evil Water", angle: null, coolDown: 8750)
                    ),
                new ItemLoot("Magic Potion", 0.1),
                new ItemLoot("Health Potion", 0.1)
            )
          .Init("Deadwater Docks Captain",
                new State(
                    new ScaleHP(0.1),
                    new Follow(0.47, 8, 1),
                    new Shoot(8, 1, 10, coolDown: 3500)
                    ),
                new ItemLoot("Magic Potion", 0.1),
                new ItemLoot("Health Potion", 0.1)
            )

          .Init("Jon Bilgewater the Pirate King",
                new State(
                    new ScaleHP(0.3),
                    new RealmPortalDrop(),
                    new State("default",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Taunt(true, "Dreadstump has fallen, and somebody's gotta pay the price!"),
                        new TimedTransition(3000, "2")
                        ),
                  new State(
                    new Order(90, "Parrot Cage", "SpawnParrots"),
                    new HpLessTransition(0.6, "4"),
                    new State("2",
                        new Wander(0.21),
                        new StayCloseToSpawn(0.2, 6),
                        new Taunt(true, "Dodge this!"),
                        new Shoot(99, count: 5, shootAngle: 5, projectileIndex: 0, coolDown: 600),
                        new TimedTransition(2500, "3")
                        ),
                    new State("3",
                        new Wander(0.4),
                        new StayCloseToSpawn(0.2, 6),
                        new Taunt(true, "Check out my AWESOME CANNON CLUSTER!"),
                        new Shoot(99, count: 11, shootAngle: 20, projectileIndex: 1, coolDown: 1000),
                        new TimedTransition(3000, "2")
                        )
                      ),

                    new State("4",
                        new ReturnToSpawn(speed: 0.52),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Taunt(true, "You aren't prepared, BILGE RATS!"),
                        new TimedTransition(3000, "5")
                        ),
                    new State("5",
                        new Taunt(true, "CANNON BARRAGE!"),
                        new Order(90, "Deadwater Docks Parrot", "CircleOrWander"),
                        new Shoot(99, count: 18, shootAngle: 20, projectileIndex: 1, coolDown: 800),
                        new HpLessTransition(0.4, "6")
                        ),
                    new State("6",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Taunt(true, "Group up, my parrots!"),
                        new Order(90, "Deadwater Docks Parrot", "Orbit&HealJon"),
                        new TimedTransition(6000, "7"),
                        new HpLessTransition(0.2, "7")
                        ),
                    new State("7",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Taunt("Pirates! Light it up."),
                        new InvisiToss("DDocks Helper", 6, 180, coolDown: 999999),
                        new InvisiToss("DDocks Helper 2", 6, 360, coolDown: 999999),
                        new InvisiToss("DDocks Helper 3", 6, 90, coolDown: 999999),
                        new InvisiToss("DDocks Helper 3", 6, 270, coolDown: 999999),
                        new TimedTransition(4000, "8")
                        ),
                    new State("8",
                        new Taunt("My time approaches... BUT SO DOES YOURS!"),
                        new Flash(0xFF0000, 0.5, 3),
                        new Shoot(99, 1, null, 0 , coolDown: 200),
                        new HpLessTransition(0.1, "9")
                        ),
                    new State("9",
                        new Taunt("Farewell."),
                        new ChangeSize(3, 0),
                        new TimedTransition(2000, "10")
                        ),
                    new State("10",
                        new Suicide()
                            )
                        ),
                new Threshold(0.01,
                    new ItemLoot("Blackbeard’s Battleplate", 0.01),
                    new ItemLoot("Cutlass of Western Winds", 0.005),
                    new ItemLoot("A Pirate's Hat", 0.005),
                    new ItemLoot("Greater Potion of Speed", 1),
                    new TierLoot(3, ItemType.Weapon, 0.05),
                    new TierLoot(3, ItemType.Ability, 0.05),
                    new TierLoot(3, ItemType.Armor, 0.05),
                    new TierLoot(3, ItemType.Ring, 0.05),
                    new TierLoot(4, ItemType.Weapon, 0.025),
                    new TierLoot(4, ItemType.Ability, 0.025),
                    new TierLoot(4, ItemType.Armor, 0.025),
                    new TierLoot(4, ItemType.Ring, 0.025),
                    new ItemLoot("Deadwater Docks Key", 0.005),
                    new ItemLoot("Rusted Platemail", 0.00112),
                    new ItemLoot("Eternal Essence", 0.0005),
                    new ItemLoot("Exotic Pet Stone", 0.001)
                )
            )
            ;
    }
}