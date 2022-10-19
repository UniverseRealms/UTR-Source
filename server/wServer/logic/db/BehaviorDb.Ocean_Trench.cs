using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Ocean_Trench = () => Behav()
          .Init("Coral Gift",
           new State(
            new ScaleHP(0.3),
            new State("Texture1",
             new SetAltTexture(1),
             new TimedTransition(500, "Texture2")
             ),
            new State("Texture2",
             new SetAltTexture(2),
             new TimedTransition(500, "Texture0")
             ),
            new State("Texture0",
             new SetAltTexture(0),
             new TimedTransition(500, "Texture1")
             )
             ),
                new Threshold(0.01,
                    new ItemLoot("Banner of the Lost City", 0.005),
                    new ItemLoot("Trap of Atlantis", 0.0025),
                    new ItemLoot("Sunken Treasure", 0.001),
                    new ItemLoot("Greater Potion of Wisdom", 1),
                    new ItemLoot("Potion of Restoration", 0.06),
                    new ItemLoot("Potion of Luck", 0.2),
                    new TierLoot(3, ItemType.Weapon, 0.05),
                    new TierLoot(3, ItemType.Ability, 0.05),
                    new TierLoot(3, ItemType.Armor, 0.05),
                    new TierLoot(3, ItemType.Ring, 0.05),
                    new TierLoot(4, ItemType.Weapon, 0.025),
                    new TierLoot(4, ItemType.Ability, 0.025),
                    new TierLoot(4, ItemType.Armor, 0.025),
                    new TierLoot(4, ItemType.Ring, 0.025),
                    new ItemLoot("Rusted Robe", 0.000715),
                    new ItemLoot("Eternal Essence", 0.0005),
                    new ItemLoot("Oceanic Pet Stone", 0.0005)
                          )
           )

          .Init("Coral Bomb Big",
           new State(
            new State("Spawning",
             new TossObject("Coral Bomb Small", 1, angle: 30, coolDown: 500),
             new TossObject("Coral Bomb Small", 1, angle: 90, coolDown: 500),
             new TossObject("Coral Bomb Small", 1, angle: 150, coolDown: 500),
             new TossObject("Coral Bomb Small", 1, angle: 210, coolDown: 500),
             new TossObject("Coral Bomb Small", 1, angle: 270, coolDown: 500),
             new TossObject("Coral Bomb Small", 1, angle: 330, coolDown: 500),
             new TimedTransition(500, "Attack")
             ),
            new State("Attack",
             new Shoot(4.4, count: 5, fixedAngle: 0, shootAngle: 70),
             new Suicide()
             )
             )
             )
          .Init("Coral Bomb Small",
           new State(
            new State("Attack",
             new Shoot(3.8, count: 5, fixedAngle: 0, shootAngle: 70),
             new Suicide()
             )
             )
             )
          .Init("Deep Sea Beast",
           new State(
            new ChangeSize(11, 100),
            new Prioritize(
             new StayCloseToSpawn(0.2, 2),
             new Follow(0.2, acquireRange: 4, range: 1)
              ),
             new Shoot(1.8, count: 1, projectileIndex: 0, coolDown: 1000),
             new Shoot(2.5, count: 1, projectileIndex: 1, coolDown: 1000),
             new Shoot(3.3, count: 1, projectileIndex: 2, coolDown: 1000),
             new Shoot(4.2, count: 1, projectileIndex: 3, coolDown: 1000)
               )
               )
        .Init("Thessal the Mermaid Goddess",
           new State(
                    new ScaleHP(0.3),
                    new TransformOnDeath("Thessal the Mermaid Goddess Wounded", probability: 0.1),
                    new TransformOnDeath("Thessal Dropper"),
                    new State("Start",
                        new Prioritize(
                            new Wander(0.3),
                            new Follow(0.3, acquireRange: 10, range: 2)
                        ),
                        new EntityNotExistsTransition("Deep Sea Beast", 20, "Spawning Deep"),
                        new HpLessTransition(1, "Attack1")
                        ),
                 new State("Main",
                        new Prioritize(
                            new Wander(0.3),
                            new Follow(0.3, acquireRange: 10, range: 2)
                        ),
                        new TimedTransition(200, "Attack1")
                        ),
                new State("Main 2",
                        new Prioritize(
                            new Wander(0.3),
                            new Follow(0.3, acquireRange: 10, range: 2)
                        ),
                        new TimedTransition(200, "Attack2")
                        ),
                    new State("Spawning Bomb",
                        new TossObject("Coral Bomb Big", angle: 45),
                        new TossObject("Coral Bomb Big", angle: 135),
                        new TossObject("Coral Bomb Big", angle: 225),
                        new TossObject("Coral Bomb Big", angle: 315),
                        new TimedTransition(1000, "Main")
                        ),
                   new State("Spawning Bomb Attack2",
                        new TossObject("Coral Bomb Big", angle: 45),
                        new TossObject("Coral Bomb Big", angle: 135),
                        new TossObject("Coral Bomb Big", angle: 225),
                        new TossObject("Coral Bomb Big", angle: 315),
                        new TimedTransition(1000, "Attack2")
                        ),
                    new State("Spawning Deep",
                        new TossObject("Deep Sea Beast", 14, angle: 0, coolDownOffset: 0),
                        new TossObject("Deep Sea Beast", 14, angle: 90, coolDownOffset: 0),
                        new TossObject("Deep Sea Beast", 14, angle: 180, coolDownOffset: 0),
                        new TossObject("Deep Sea Beast", 14, angle: 270, coolDownOffset: 0),
                        new TimedTransition(1000, "Start")
                        ),
                    new State("Attack1",
                        new HpLessTransition(0.5, "Attack2"),
                        //new TimedTransition(3000, "Trident", randomized: true),
                        new TimedTransition(3000, "Yellow Wall", randomized: true),
                        new TimedTransition(3000, "Super Trident", randomized: true),
                        new TimedTransition(3000, "Thunder Swirl", randomized: true),
                        new TimedTransition(3000, "Spawning Bomb", randomized: true)
                    ),
                    new State("Thunder Swirl",
                        new Shoot(8.8, count: 8, shootAngle: 360 / 8, projectileIndex: 0),
                        new TimedTransition(500, "Thunder Swirl 2")
                    ),
                    new State("Thunder Swirl 2",
                        new Shoot(8.8, count: 8, shootAngle: 360 / 8, projectileIndex: 0),
                        new TossObject("Coral Bomb Big"),
                        new TimedTransition(500, "Thunder Swirl 3")
                    ),
                    new State("Thunder Swirl 3",
                        new Shoot(8.8, count: 8, shootAngle: 360 / 8, projectileIndex: 0),
                        new TimedTransition(200, "Main")
                    ),
                    new State("Thunder Swirl Attack2",
                        new Shoot(8.8, count: 16, shootAngle: 360 / 16, projectileIndex: 0),
                        new TimedTransition(500, "Thunder Swirl 2 Attack2")
                    ),
                    new State("Thunder Swirl 2 Attack2",
                        new Shoot(8.8, count: 16, shootAngle: 360 / 16, projectileIndex: 0),
                        new TossObject("Coral Bomb Big"),
                        new TimedTransition(500, "Thunder Swirl 3 Attack2")
                    ),
                    new State("Thunder Swirl 3 Attack2",
                        new Shoot(8.8, count: 16, shootAngle: 360 / 16, projectileIndex: 0),
                        new TimedTransition(200, "Main 2")
                    ),
            new State("Trident",
              new Prioritize(
               new StayCloseToSpawn(0.3, 1),
              new Wander(0.3)
             ),
             new Shoot(21, count: 2, fixedAngle: 0, shootAngle: 10, projectileIndex: 1),
             new Shoot(21, count: 2, fixedAngle: 90, shootAngle: 10, projectileIndex: 1),
             new Shoot(21, count: 2, fixedAngle: 180, shootAngle: 10, projectileIndex: 1),
             new Shoot(21, count: 2, fixedAngle: 270, shootAngle: 10, projectileIndex: 1),
             new Shoot(21, count: 2, fixedAngle: 45, shootAngle: 10, projectileIndex: 1, coolDownOffset: 500),
             new Shoot(21, count: 2, fixedAngle: 135, shootAngle: 10, projectileIndex: 1, coolDownOffset: 500),
             new Shoot(21, count: 2, fixedAngle: 225, shootAngle: 10, projectileIndex: 1, coolDownOffset: 500),
             new Shoot(21, count: 2, fixedAngle: 315, shootAngle: 10, projectileIndex: 1, coolDownOffset: 500),
             new TimedTransition(501, "Start")
            ),
            new State("Super Trident",
                        new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 0),
                        new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 90),
                        new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 180),
                        new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 270),
                        new TossObject("Coral Bomb Big"),
                        new TimedTransition(250, "Super Trident 2")
                    ),
                    new State("Super Trident 2",
                        new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 45),
                        new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 135),
                        new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 225),
                        new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 315),
                        new TossObject("Coral Bomb Big"),
                        new TimedTransition(200, "Main")
                    ),
                    new State("Super Trident Attack2",
                        new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 0),
                        new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 90),
                        new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 180),
                        new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 270),
                        new TossObject("Coral Bomb Big"),
                        new TimedTransition(250, "Super Trident 2 Attack2")
                    ),
                    new State("Super Trident 2 Attack2",
                        new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 45),
                        new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 135),
                        new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 225),
                        new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 315),
                        new TimedTransition(250, "Super Trident 3 Attack2")
                    ),
                    new State("Super Trident 3 Attack2",
                        new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 0),
                        new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 90),
                        new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 180),
                        new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 270),
                        new TossObject("Coral Bomb Big"),
                        new TimedTransition(250, "Super Trident 4 Attack2")
                    ),
                    new State("Super Trident 4 Attack2",
                        new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 45),
                        new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 135),
                        new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 225),
                        new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 315),
                        new TimedTransition(200, "Main 2")
                    ),
                    new State("Yellow Wall",
                        new Flash(0xFFFF00, .1, 15),
                        new Prioritize(
                            new StayCloseToSpawn(0.3, 1)
                        ),
                        new Shoot(18, count: 30, fixedAngle: 6, projectileIndex: 3),
                        new TimedTransition(500, "Yellow Wall 2")
                    ),
                    new State("Yellow Wall 2",
                        new Flash(0xFFFF00, .1, 15),
                        new Shoot(18, count: 30, fixedAngle: 6, projectileIndex: 3),
                        new TimedTransition(500, "Yellow Wall 3")
                    ),
                    new State("Yellow Wall 3",
                        new Flash(0xFFFF00, .1, 15),
                        new Shoot(18, count: 30, fixedAngle: 6, projectileIndex: 3),
                        new TimedTransition(200, "Main")
                    ),
                    new State("Yellow Wall Attack2",
                        new Flash(0xFFFF00, .1, 15),
                        new Prioritize(
                            new StayCloseToSpawn(0.3, 1)
                        ),
                        new Shoot(18, count: 30, fixedAngle: 6, projectileIndex: 3),
                        new TimedTransition(500, "Yellow Wall 2 Attack2")
                    ),
                    new State("Yellow Wall 2 Attack2",
                        new Flash(0xFFFF00, .1, 15),
                        new Shoot(18, count: 30, fixedAngle: 6, projectileIndex: 3),
                        new TimedTransition(500, "Yellow Wall 3 Attack2")
                    ),
                    new State("Yellow Wall 3 Attack2",
                        new Flash(0xFFFF00, .1, 15),
                        new Shoot(18, count: 30, fixedAngle: 6, projectileIndex: 3),
                        new TimedTransition(200, "Main 2")
                    ),
                    new State("Attack2",
                        //new TimedTransition(500, "Trident", randomized: true),
                        new TimedTransition(500, "Yellow Wall Attack2", randomized: true),
                        new TimedTransition(500, "Super Trident Attack2", randomized: true),
                        new TimedTransition(500, "Thunder Swirl Attack2", randomized: true),
                        new TimedTransition(500, "Spawning Bomb", randomized: true)
                    )
                    ),
                new Threshold(0.01,
                    new ItemLoot("Banner of the Lost City", 0.02),
                    new ItemLoot("Trap of Atlantis", 0.0067),
                    new ItemLoot("Sunken Treasure", 0.002),
                    new ItemLoot("Greater Potion of Wisdom", 1),
                    new ItemLoot("Potion of Restoration", 0.06),
                    new ItemLoot("Potion of Luck", 0.2),
                    new TierLoot(3, ItemType.Weapon, 0.1),
                    new TierLoot(3, ItemType.Ability, 0.1),
                    new TierLoot(3, ItemType.Armor, 0.1),
                    new TierLoot(3, ItemType.Ring, 0.1),
                    new TierLoot(4, ItemType.Weapon, 0.05),
                    new TierLoot(4, ItemType.Ability, 0.05),
                    new TierLoot(4, ItemType.Armor, 0.05),
                    new TierLoot(4, ItemType.Ring, 0.05),
                    new ItemLoot("Rusted Robe", 0.00143),
                    new ItemLoot("Eternal Essence", 0.001),
                    new ItemLoot("Oceanic Pet Stone", 0.001)
              )
            )


          .Init("Thessal the Mermaid Goddess Wounded",
           new State(
               new State("Say",
                   new ConditionalEffect(ConditionEffectIndex.Invincible, false, 2000),
                   new Taunt("Please spare me, that I may find my lost Alexander."),
                   new TimedTransition(6000, "Throw Gifts"),
                   new HpLessTransition(0.8, "Doom")
                   ),
               new State("Throw Gifts",
                   new Taunt("Thank you, kind sailor"),
                   new TossObject("Coral Gift", 4, 60, coolDown: 999999),
                   new TossObject("Coral Gift", 4, 180, coolDown: 999999),
                   new TossObject("Coral Gift", 4, 300, coolDown: 999999),
                   new TimedTransition(4000, "Suicide")
                   ),
               new State("Doom",
                   new Taunt("Murderers! I shall avenge him!"),
                   new SetAltTexture(1),
                   new Shoot(18, count: 30, fixedAngle: 6, projectileIndex: 3, coolDown: 200),
                   new Shoot(18, count: 30, fixedAngle: 6, projectileIndex: 1, coolDown: 150),
                   new Shoot(10, count: 8, shootAngle: 360 / 8, projectileIndex: 0, coolDown: 125),
                   new TimedTransition(4000, "Suicide")
                   ),
               new State("Suicide",
                   new Suicide()
                   )
               )
             )
           .Init("Sea Horse",
                 new State(
                     new State(
                         new Prioritize(
                             new Protect(1, "Sea Mare"),
                             new Wander(1)
                         ),
                         new Shoot(7, count: 2, shootAngle: 10, coolDown: 660)

                     )
                 )
             )
          .Init("Sea Mare",
                 new State(
                     new Prioritize(
                         new Follow(.8, 10, 1),
                         new Wander(1)
                     ),
                     new Shoot(5, count: 3, shootAngle: 120, coolDown: 500),
                     new Shoot(3, count: 1, projectileIndex: 1, shootAngle: 0, coolDown: 1500)
             ),
                              new Threshold(0.2,
              new ItemLoot("Wavebreaker", 0.0001)
              )
            )
            .Init("Giant Squid",
                 new State(
                     new Prioritize(
                         new Follow(.8, 10, 1),
                          new Wander(1)
                     ),
                     new Shoot(10, count: 1, shootAngle: 0, coolDown: 400),
                     new TossObject("Ink Bubble", 7, angle: null, coolDown: 1000)

             ))
          .Init("Ink Bubble",
                 new State(
                     new SetNoXP(),
                     new Shoot(1, count: 1, shootAngle: 0, coolDown: 400)
                     ))
          .Init("Sea Slurp Home",
                 new State(
                     new Shoot(4, count: 8, shootAngle: 45, coolDown: 500),
                     new Shoot(2, count: 8, shootAngle: 45, projectileIndex: 1, coolDown: 500),
                     new Spawn("Grey Sea Slurp", 8, coolDown: 1000)
                     ))
         .Init("Grey Sea Slurp",
                 new State(
                     new State(
                         new Prioritize(
                             new Protect(1, "Sea Slurp Home"),
                             new Wander(1)
                         ),
                         new Shoot(8, count: 1, shootAngle: 0, coolDown: 500),
                         new Shoot(4, count: 8, shootAngle: 45, projectileIndex: 1, coolDown: 500)

                     )
                 )
                 
             );
    }
}