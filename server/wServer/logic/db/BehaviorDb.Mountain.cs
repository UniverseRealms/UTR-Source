using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Mountain = () => Behav()
            //.Init("Arena Horseman Anchor",
            //    new State(
            //        new ConditionalEffect(ConditionEffectIndex.Invincible)
            //        )
            //)
            //.Init("Arena Headless Horseman",
            //    new State(
            //        new DropPortalOnDeath("Haunted Cemetery Portal", .40),
            //        new Spawn("Arena Horseman Anchor", 1, 1),
            //        new State("Default",
            //            new HpLessTransition(0.1, "End"),
            //            new State("Circle",
            //                new Shoot(15, 3, shootAngle: 25, projectileIndex: 0, coolDown: 1000),
            //                new Shoot(15, projectileIndex: 1, coolDown: 1000),
            //                new Orbit(1, 5, 10, "Arena Horseman Anchor"),
            //                new TimedTransition(8000, "Shoot")
            //                ),
            //            new State("Shoot",
            //                new ReturnToSpawn(1.5),
            //                new ConditionalEffect(ConditionEffectIndex.Invincible),
            //                new Flash(0xF0E68C, 1, 6),
            //                new Shoot(15, 8, projectileIndex: 2, coolDown: 1500),
            //                new Shoot(15, projectileIndex: 1, coolDown: 2500),
            //                new TimedTransition(6000, "Circle")
            //                )
            //            ),
            //        new State("End",
            //            new Prioritize(
            //                new Follow(1.5, 20, 1),
            //                new Wander(1.5)
            //                ),
            //            new Flash(0xF0E68C, 1, 1000),
            //            new Shoot(15, 3, shootAngle: 25, projectileIndex: 0, coolDown: 1000),
            //            new Shoot(15, projectileIndex: 1, coolDown: 1000)
            //            )
            //        ),
            //    new Threshold(0.05,
            //        new ItemLoot("Potion of Defense", 0.06666),
            //        new ItemLoot("Tiny Eternal Fragment", 0.0001)
            //        )
            //)
            .Init("White Demon",
                new State(
                    new DropPortalOnDeath("Abyss of Demons Portal", .27),
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Follow(1, range: 7),
                        new Wander(0.4)
                        ),
                    new Shoot(10, count: 3, shootAngle: 20, predictive: 1, coolDown: 500),
                    new Reproduce(densityMax: 3)
                    ),
                new Threshold(0.01,
                    new ItemLoot("Potion of Vitality", 0.067),
                    new TierLoot(2, ItemType.Weapon, 0.025),
                    new TierLoot(2, ItemType.Ability, 0.025),
                    new TierLoot(2, ItemType.Armor, 0.025),
                    new TierLoot(2, ItemType.Ring, 0.025)
                    )
            )
            .Init("White Demon of the Abyss",
                new State(
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Follow(1, range: 7),
                        new Wander(0.4)
                        ),
                    new Shoot(10, count: 3, shootAngle: 20, predictive: 1, coolDown: 500),
                    new Reproduce(densityMax: 3)
                    ),
                new Threshold(0.01,
                    new ItemLoot("Potion of Vitality", 0.067),
                    new TierLoot(2, ItemType.Weapon, 0.025),
                    new TierLoot(2, ItemType.Ability, 0.025),
                    new TierLoot(2, ItemType.Armor, 0.025),
                    new TierLoot(2, ItemType.Ring, 0.025)
                    )
             )
            .Init("Sprite God",
                new State(
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Wander(0.4)
                        ),
                    new Shoot(12, projectileIndex: 0, count: 4, shootAngle: 10),
                    new Shoot(10, projectileIndex: 1, predictive: 1),
                    new Reproduce(densityMax: 3),
                    new ReproduceChildren(5, .5, 5000, "Sprite Child")
                    ),
                new Threshold(0.01,
                    new ItemLoot("Potion of Dexterity", 0.067),
                    new TierLoot(2, ItemType.Weapon, 0.025),
                    new TierLoot(2, ItemType.Ability, 0.025),
                    new TierLoot(2, ItemType.Armor, 0.025),
                    new TierLoot(2, ItemType.Ring, 0.025)
                    )
            )
            .Init("Angelic Commander",
            new State(
                new DropPortalOnDeath("Heavenly Rift Portal", .75),
                new Reproduce(densityMax: 3),
                new State("First",
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Follow(1, range: 7),
                        new Wander(0.4)
                        ),
                    new Shoot(12, projectileIndex: 0, count: 3, shootAngle: 6, coolDown: 800),
                    new TimedTransition(4000, "Battle")
                         ),
                 new State("Battle",
                    new Follow(1.2, 8, 6),
                    new Shoot(12, projectileIndex: 1, count: 6, shootAngle: 12, coolDown: 1000),
                    new Shoot(12, projectileIndex: 2, count: 8, coolDown: 2000),
                    new TimedTransition(4000, "First")
                         )
                    ),
                            new MostDamagers(3,
                    LootTemplates.SorVeryRare()
                    )
                    )
            .Init("Sprite Child",
                new State(
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Protect(0.4, "Sprite God", protectionRange: 1),
                        new Wander(0.4)
                        ),
                    new DropPortalOnDeath("Glowing Portal", .33)
                    )
            )
            .Init("Medusa",
                new State(
                    new DropPortalOnDeath("Snake Pit Portal", .27),
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Follow(1, range: 7),
                        new Wander(0.4)
                        ),
                    new Shoot(12, count: 5, shootAngle: 10, coolDown: 1000),
                    new Grenade(4, 150, range: 8, coolDown: 3000),
                    new Reproduce(densityMax: 3)
                    ),
                new Threshold(0.01,
                    new ItemLoot("Potion of Speed", 0.067),
                    new TierLoot(2, ItemType.Weapon, 0.025),
                    new TierLoot(2, ItemType.Ability, 0.025),
                    new TierLoot(2, ItemType.Armor, 0.025),
                    new TierLoot(2, ItemType.Ring, 0.025)
                    )
            )
           .Init("Thunder God",
                new State(
                    new DropPortalOnDeath("Storm Palace Portal", 1),
                    new Prioritize(
                        new Swirl(0.4, 7),
                        new Wander(0.4)
                        ),
                    new Shoot(10, 4, 20, coolDown: 3000),
                    new Shoot(10, 8, coolDown: 2500, predictive: 1, coolDownOffset: 2690, projectileIndex: 1)
                    ),
                new Threshold(0.01,
                    new ItemLoot("Potion of Dexterity", 0.067),
                    new TierLoot(2, ItemType.Weapon, 0.025),
                    new TierLoot(2, ItemType.Ability, 0.025),
                    new TierLoot(2, ItemType.Armor, 0.025),
                    new TierLoot(2, ItemType.Ring, 0.025)
                        )
                )
            .Init("Ent God",
                new State(
                    new DropPortalOnDeath("Garden of Chaos Portal", .05),
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Follow(1, range: 7),
                        new Wander(0.4)
                        ),
                    new Shoot(12, count: 5, shootAngle: 10, predictive: 1, coolDown: 1250),
                    new Reproduce(densityMax: 3)
                    ),
            	new Threshold(0.01,
                    new ItemLoot("Potion of Speed", 0.067),
                    new TierLoot(2, ItemType.Weapon, 0.025),
                	new TierLoot(2, ItemType.Ability, 0.025),
                	new TierLoot(2, ItemType.Armor, 0.025),
                	new TierLoot(2, ItemType.Ring, 0.025)
                    )
            )
            .Init("Beholder",
                new State(
                    new DropPortalOnDeath("Tunnel of Pain Portal", .21),
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Follow(1, range: 7),
                        new Wander(0.4)
                        ),
                    new Shoot(12, projectileIndex: 0, count: 5, shootAngle: 72, predictive: 0.5, coolDown: 750),
                    new Shoot(10, projectileIndex: 1, predictive: 1),
                    new Reproduce(densityMax: 3)
                    ),
            	new Threshold(0.01,
                    new ItemLoot("Potion of Attack", 0.067),
                    new TierLoot(2, ItemType.Weapon, 0.025),
                    new TierLoot(2, ItemType.Ability, 0.025),
                    new TierLoot(2, ItemType.Armor, 0.025),
                    new TierLoot(2, ItemType.Ring, 0.025)
                    )
            )
            .Init("Flying Brain",
                new State(
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Follow(1, range: 7),
                        new Wander(0.4)
                        ),
                    new Shoot(12, count: 5, shootAngle: 72, coolDown: 500),
                    new Reproduce(densityMax: 3),
                    new DropPortalOnDeath("Mad Lab Portal", .17)
                    ),
            	new Threshold(0.01,
                    new ItemLoot("Potion of Wisdom", 0.067),
                    new TierLoot(2, ItemType.Weapon, 0.025),
                    new TierLoot(2, ItemType.Ability, 0.025),
                    new TierLoot(2, ItemType.Armor, 0.025),
                    new TierLoot(2, ItemType.Ring, 0.025)
                    )
            )
            .Init("Slime God",
                new State(
                    new DropPortalOnDeath("Summer's Breeze Portal", .17),
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Follow(1, range: 7),
                        new Wander(0.4)
                        ),
                    new Shoot(12, projectileIndex: 0, count: 5, shootAngle: 10, predictive: 1, coolDown: 1000),
                    new Shoot(10, projectileIndex: 1, predictive: 1, coolDown: 650),
                    new Reproduce(densityMax: 2)
                    ),
            	new Threshold(0.01,
                    new ItemLoot("Potion of Defense", 0.067),
                    new TierLoot(2, ItemType.Weapon, 0.025),
                    new TierLoot(2, ItemType.Ability, 0.025),
                    new TierLoot(2, ItemType.Armor, 0.025),
                    new TierLoot(2, ItemType.Ring, 0.025)
                    )
            )
            .Init("Ghost God",
                new State(
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Follow(1, range: 7),
                        new Wander(0.4)
                        ),
                    new Shoot(12, count: 7, shootAngle: 25, predictive: 0.5, coolDown: 900),
                    new Reproduce(densityMax: 3),
                    new DropPortalOnDeath("Undead Lair Portal", 0.27)
                    ),
            	new Threshold(0.01,
                    new ItemLoot("Potion of Wisdom", 0.067),
                    new TierLoot(2, ItemType.Weapon, 0.025),
                    new TierLoot(2, ItemType.Ability, 0.025),
                    new TierLoot(2, ItemType.Armor, 0.025),
                    new TierLoot(2, ItemType.Ring, 0.025)
                    )
            )
           .Init("Construct of the Concealment",
                new State(
                    new Taunt("STRONGER!", "AGAIN!"),
                    new Wander(0.6),
                    new Shoot(12, 2, 1, coolDown: 400),
                    new Shoot(12, 10, 1, projectileIndex: 1, coolDown: 2000),
                    new DropPortalOnDeath("Concealment of the Dreadnought Portal", 1)
                    ),
             	new Threshold(0.01,
                    new ItemLoot("Potion of Defense", 0.067),
                    new TierLoot(2, ItemType.Weapon, 0.025),
                    new TierLoot(2, ItemType.Ability, 0.025),
                    new TierLoot(2, ItemType.Armor, 0.025),
                    new TierLoot(2, ItemType.Ring, 0.025)
                        )
                    )
            .Init("Rock Bot",
                new State(
                    new TransformOnDeath("Construct of the Concealment", 1, 1, probability: 0.05),
                    new Spawn("Paper Bot", maxChildren: 1, initialSpawn: 1, coolDown: 10000, givesNoXp: false),
                    new Spawn("Steel Bot", maxChildren: 1, initialSpawn: 1, coolDown: 10000, givesNoXp: false),
                    new Swirl(speed: 0.6, radius: 3, targeted: false),
                    new State("Waiting",
                        new PlayerWithinTransition(15, "Attacking")
                        ),
                    new State("Attacking",
                        new Shoot(8, coolDown: 2000),
                        new HealGroup(8, "Papers", coolDown: 1000),
                        new Taunt(0.5, "We are impervious to non-mystic attacks!"),
                        new TimedTransition(10000, "Waiting")
                        )
                    ),
                new Threshold(.01,
                    new TierLoot(2, ItemType.Weapon, 0.025),
                    new TierLoot(2, ItemType.Ability, 0.025),
                    new TierLoot(2, ItemType.Armor, 0.025),
                    new TierLoot(2, ItemType.Ring, 0.025)
                    )
            )
            .Init("Paper Bot",
                new State(
                    new TransformOnDeath("Construct of the Concealment", 1, 1, probability: 0.05),
                    new Prioritize(
                        new Orbit(0.4, 3, target: "Rock Bot"),
                        new Wander(0.8)
                        ),
                    new State("Idle",
                        new PlayerWithinTransition(15, "Attack")
                        ),
                    new State("Attack",
                        new Shoot(8, count: 3, shootAngle: 20, coolDown: 800),
                        new HealGroup(8, "Steels", coolDown: 1000),
                        new NoPlayerWithinTransition(30, "Idle"),
                        new HpLessTransition(0.2, "Explode")
                        ),
                    new State("Explode",
                        new Shoot(0, count: 10, shootAngle: 36, fixedAngle: 0),
                        new Decay(0)
                        )
                    ),
                new Threshold(.01,
                    new TierLoot(2, ItemType.Weapon, 0.025),
                    new TierLoot(2, ItemType.Ability, 0.025),
                    new TierLoot(2, ItemType.Armor, 0.025),
                    new TierLoot(2, ItemType.Ring, 0.025)
                    )
            )
            .Init("Steel Bot",
                new State(
                    new TransformOnDeath("Construct of the Concealment", 1, 1, probability: 0.05),
                    new Prioritize(
                        new Orbit(0.4, 3, target: "Rock Bot"),
                        new Wander(0.8)
                        ),
                    new State("Idle",
                        new PlayerWithinTransition(15, "Attack")
                        ),
                    new State("Attack",
                        new Shoot(8, count: 3, shootAngle: 20, coolDown: 800),
                        new HealGroup(8, "Rocks", coolDown: 1000),
                        new Taunt(0.5, "Silly squishy. We heal our brothers in a circle."),
                        new NoPlayerWithinTransition(30, "Idle"),
                        new HpLessTransition(0.2, "Explode")
                        ),
                    new State("Explode",
                        new Shoot(0, count: 10, shootAngle: 36, fixedAngle: 0),
                        new Decay(0)
                        )
                    ),
                new Threshold(.01,
                    new TierLoot(2, ItemType.Weapon, 0.10)
                    ),
                new Threshold(0.05,
                    new TierLoot(2, ItemType.Weapon, 0.025),
                    new TierLoot(2, ItemType.Ability, 0.025),
                    new TierLoot(2, ItemType.Armor, 0.025),
                    new TierLoot(2, ItemType.Ring, 0.025)
                    )
            )
            .Init("Djinn",
                new State(
                    new DropPortalOnDeath("Trial of the Illusionist Portal", .15),
                    new State("Idle",
                        new Prioritize(
                            new StayAbove(1, 200),
                            new Wander(0.8)
                            ),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Reproduce(densityMax: 3, densityRadius: 20),
                        new PlayerWithinTransition(8, "Attacking")
                        ),
                    new State("Attacking",
                        new State("Bullet",
                            new Shoot(1, count: 4, coolDown: 400, fixedAngle: 90, rotateAngle: 10, coolDownOffset: 0, shootAngle: 90),
                            new Shoot(1, count: 8, coolDown: 10000, fixedAngle: 180, coolDownOffset: 2000, shootAngle: 45),
                            new Shoot(1, count: 4, coolDown: 400, fixedAngle: 180, rotateAngle: -10, coolDownOffset: 0, shootAngle: 90),
                            new Shoot(1, count: 4, coolDown: 10000, fixedAngle: 90, coolDownOffset: 2000, shootAngle: 22.5),
                            new TimedTransition(2000, "Wait")
                            ),
                        new State("Wait",
                            new Follow(0.7, range: 0.5),
                            new Flash(0xff00ff00, 0.1, 20),
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new TimedTransition(2000, "Bullet")
                            ),
                        new NoPlayerWithinTransition(13, "Idle"),
                        new HpLessTransition(0.5, "FlashBeforeExplode")
                        ),
                    new State("FlashBeforeExplode",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xff0000, 0.3, 3),
                        new TimedTransition(1000, "Explode")
                        ),
                    new State("Explode",
                        new Shoot(0, count: 10, shootAngle: 36, fixedAngle: 0),
                        new Suicide()
                        )
                    ),
            	new Threshold(0.01,
                    new ItemLoot("Potion of Vitality", 0.067),
                    new TierLoot(2, ItemType.Weapon, 0.025),
                    new TierLoot(2, ItemType.Ability, 0.025),
                    new TierLoot(2, ItemType.Armor, 0.025),
                    new TierLoot(2, ItemType.Ring, 0.025)
                    )
            )
            .Init("Leviathan",
                new State(
                    new DropPortalOnDeath("Puppet Theatre Portal", .33),
                    new State("Wander",
                        new Swirl(),
                        new Shoot(10, 2, 10, 1, coolDown: 500),
                        new TimedTransition(5000, "Triangle")
                        ),
                    new State("Triangle",
                        new State("1",
                            new MoveLine(.7, 40),
                            new Shoot(1, 3, 120, fixedAngle: 34, coolDown: 400),
                            new Shoot(1, 3, 120, fixedAngle: 38, coolDown: 400),
                            new Shoot(1, 3, 120, fixedAngle: 42, coolDown: 400),
                            new Shoot(1, 3, 120, fixedAngle: 46, coolDown: 400),
                            new TimedTransition(1500, "2")
                            ),
                        new State("2",
                            new MoveLine(.7, 160),
                            new Shoot(1, 3, 120, fixedAngle: 94, coolDown: 400),
                            new Shoot(1, 3, 120, fixedAngle: 98, coolDown: 400),
                            new Shoot(1, 3, 120, fixedAngle: 102, coolDown: 400),
                            new Shoot(1, 3, 120, fixedAngle: 106, coolDown: 400),
                            new TimedTransition(1500, "3")
                            ),
                        new State("3",
                            new MoveLine(.7, 280),
                            new Shoot(1, 3, 120, fixedAngle: 274, coolDown: 400),
                            new Shoot(1, 3, 120, fixedAngle: 278, coolDown: 400),
                            new Shoot(1, 3, 120, fixedAngle: 282, coolDown: 400),
                            new Shoot(1, 3, 120, fixedAngle: 286, coolDown: 400),
                            new TimedTransition(1500, "Wander"))
                        )
                    ),
            	new Threshold(0.01,
                    new ItemLoot("Potion of Attack", 0.067),
                    new TierLoot(2, ItemType.Weapon, 0.025),
                    new TierLoot(2, ItemType.Ability, 0.025),
                    new TierLoot(2, ItemType.Armor, 0.025),
                    new TierLoot(2, ItemType.Ring, 0.025)
                    )
            )
            .Init("Lucky Djinn",
                new State(
                    new ScaleHP(0.3),
                    new State("Idle",
                        new Prioritize(
                            new StayAbove(1, 200),
                            new Wander(0.8)
                            ),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new PlayerWithinTransition(8, "Attacking")
                        ),
                    new State("Attacking",
                        new State("Bullet",
                            new Shoot(1, count: 4, coolDown: 400, fixedAngle: 90, rotateAngle: 10, coolDownOffset: 0, shootAngle: 90),
                            new Shoot(1, count: 8, coolDown: 10000, fixedAngle: 180, coolDownOffset: 2000, shootAngle: 45),
                            new Shoot(1, count: 4, coolDown: 400, fixedAngle: 180, rotateAngle: -10, coolDownOffset: 0, shootAngle: 90),
                            new Shoot(1, count: 4, coolDown: 10000, fixedAngle: 90, coolDownOffset: 2000, shootAngle: 22.5),
                            new TimedTransition(2000, "Wait")
                            ),
                        new State("Wait",
                            new Follow(0.7, range: 0.5),
                            new Flash(0xff00ff00, 0.1, 20),
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new TimedTransition(2000, "Bullet")
                            ),
                        new NoPlayerWithinTransition(13, "Idle"),
                        new HpLessTransition(0.5, "FlashBeforeExplode")
                        ),
                    new State("FlashBeforeExplode",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xff0000, 0.3, 3),
                        new TimedTransition(1000, "Explode")
                        ),
                    new State("Explode",
                        new Shoot(0, count: 10, shootAngle: 36, fixedAngle: 0),
                        new Suicide()
                        ),
                    new DropPortalOnDeath("The Crawling Depths", 1)
                    ),
              	new Threshold(0.01,
                	new ItemLoot("Potion of Mana", 1),
                    new ItemLoot("Potion of Life", 1),
                    new ItemLoot("Armor of Desire", 0.0034),
                    new TierLoot(2, ItemType.Weapon, 0.05),
                    new TierLoot(2, ItemType.Ability, 0.05),
                    new TierLoot(2, ItemType.Armor, 0.05),
                    new TierLoot(2, ItemType.Ring, 0.05),
                    new TierLoot(3, ItemType.Weapon, 0.025),
                    new TierLoot(3, ItemType.Ability, 0.025),
                    new TierLoot(3, ItemType.Armor, 0.025),
                    new TierLoot(3, ItemType.Ring, 0.025),
                    new ItemLoot("Rusted Leather Hide", 0.00112),
                    new ItemLoot("Eternal Essence", 0.0005)
                    )
            )
            .Init("Lucky Ent God",
                new State(
                    new ScaleHP(0.3),
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Follow(1, range: 7),
                        new Wander(0.4)
                        ),
                    new Shoot(12, count: 5, shootAngle: 10, predictive: 1, coolDown: 1250),
                    new DropPortalOnDeath("Woodland Labyrinth", 1)
                    ),
              	new Threshold(0.01,
                    new ItemLoot("Potion of Mana", 1),
                    new ItemLoot("Potion of Life", 1),
                    new ItemLoot("Battleplate of Fate", 0.0034),
                    new TierLoot(2, ItemType.Weapon, 0.05),
                    new TierLoot(2, ItemType.Ability, 0.05),
                    new TierLoot(2, ItemType.Armor, 0.05),
                    new TierLoot(2, ItemType.Ring, 0.05),
                    new TierLoot(3, ItemType.Weapon, 0.025),
                    new TierLoot(3, ItemType.Ability, 0.025),
                    new TierLoot(3, ItemType.Armor, 0.025),
                    new TierLoot(3, ItemType.Ring, 0.025),
                    new ItemLoot("Rusted Platemail", 0.00112),
                    new ItemLoot("Eternal Essence", 0.0005)
                    )
            )
            .Init("Frost Knight",
                new State(
                   new DropPortalOnDeath("Inner Sanctum Portal", 0.04),
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Follow(1, range: 7),
                        new Wander(0.4)
                        ),
                    new Shoot(12, projectileIndex: 0, count: 6, shootAngle: 30, predictive: 0.5, coolDown: 1000),
                    new Shoot(3, projectileIndex: 1, count: 10, shootAngle: 72, predictive: 1, coolDown: 1000),
                    //new Shoot(6, projectileIndex: 2, count: 4, shootAngle: 60, predictive: 1, coolDown: 1000),
                    new Reproduce(densityMax: 3)
                    ),
                new MostDamagers(3,
                    LootTemplates.SorVeryRare()
                    ),
            	new Threshold(0.01,
                    new ItemLoot("Potion of Life", 0.067),
                    new TierLoot(2, ItemType.Weapon, 0.025),
                    new TierLoot(2, ItemType.Ability, 0.025),
                    new TierLoot(2, ItemType.Armor, 0.025),
                    new TierLoot(2, ItemType.Ring, 0.025)
                        )
                    )
            .Init("Elemental Flame",
                new State(
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Follow(1, range: 7),
                        new Wander(0.4)
                        ),
                    new Shoot(12, projectileIndex: 0, count: 12, shootAngle: 45, predictive: 1, coolDown: 1000),
                    new Shoot(9, projectileIndex: 1, count: 9, shootAngle: 80, predictive: 1, coolDown: 1000),
                    new Reproduce(densityMax: 3)
                    ),
                                new MostDamagers(3,
                    LootTemplates.SorVeryRare()
                    ),
                new Threshold(.01,
                    new TierLoot(6, ItemType.Weapon, 0.1),
                    new TierLoot(7, ItemType.Weapon, 0.07),
                    new TierLoot(8, ItemType.Weapon, 0.05),
                    new TierLoot(7, ItemType.Armor, 0.1),
                    new TierLoot(8, ItemType.Armor, 0.07),
                    new TierLoot(9, ItemType.Armor, 0.05),
                    new TierLoot(4, ItemType.Ring, 0.04),
                    new TierLoot(4, ItemType.Ability, 0.04)
                    ),
                new Threshold(0.07,
                    new ItemLoot("Blazed Bow", 0.000004),
                    new OnlyOne(
                        new ItemLoot("Potion of Defense", 0.2),
                        new ItemLoot("Potion of Attack", 0.2),
                        new ItemLoot("Potion of Speed", 0.2),
                        new ItemLoot("Potion of Vitality", 0.2),
                        new ItemLoot("Potion of Wisdom", 0.2),
                        new ItemLoot("Potion of Dexterity", 0.2)
                        )
                    )
            )
            ;
    }
}