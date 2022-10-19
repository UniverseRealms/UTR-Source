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
        private _ Crystal = () => Behav()
            .Init("Mysterious Crystal",
                new State(
                    //new DropPortalOnDeath("Deadwater Docks", 100), temp remove because of the exploit
                    new State("Idle",
                        new Taunt(0.1, "Break the crystal for great rewards..."),
                        new Taunt(0.1, "Help me..."),
                        new HpLessTransition(0.9999, "Instructions"),
                        new TimedTransition(10000, "Idle")
                        ),
                    new State("Instructions",
                        new Flash(0xffffffff, 2, 100),
                        new Taunt(0.8, "Fire upon this crystal with all your might for 5 seconds"),
                        new Taunt(0.8, "If your attacks are weak, the crystal magically heals"),
                        new Taunt(0.8, "Gather a large group to smash it open"),
                        new Taunt(0.8, "Strike this encasement down to let me free!"),
                        new HpLessTransition(0.998, "Evaluation")
                        ),
                    new State("Evaluation",
                        new State("Comment1",
                            new Taunt(true, "Sweet treasure awaits for powerful adventurers!"),
                            new Taunt(0.4, "Yes!  Smash my prison for great rewards!", "I can taste freedom like it was a popsicle"),
                            new TimedTransition(5000, "Comment2")
                            ),
                        new State("Comment2",
                            new Taunt(0.3, "If you are not very strong, this could kill you",
                                "If you are not yet powerful, stay away from the Crystal",
                                "New adventurers should stay away",
                                "That's the spirit. Lay your fire upon me.",
                                "So close...",
                                "I was almost free..."
                                ),
                            new TimedTransition(5000, "Comment3")
                            ),
                        new State("Comment3",
                            new Taunt(0.4, "I think you need more people...",
                                "Call all your friends to help you break the crystal!"
                                ),
                            new TimedTransition(10000, "Comment2")
                            ),
                        new HealSelf(coolDown: 5000),
                        new HpLessTransition(0.95, "StartBreak"),
                        new TimedTransition(60000, "Fail")
                        ),
                    new State("Fail",
                        new Taunt("Perhaps you need a bigger group. Ask others to join you!"),
                        new Flash(0xff000000, 5, 1),
                        new Shoot(10, 16, 22.5, fixedAngle: 0, coolDown: 100000),
                        new HealSelf(coolDown: 1000),
                        new TimedTransition(5000, "Idle")
                        ),
                    new State("StartBreak",
                        new Taunt("You cracked the crystal! Soon we shall emerge!"),
                        new ChangeSize(-2, 80),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xff000000, 2, 10),
                        new TimedTransition(4000, "BreakCrystal")
                        ),
                    new State("BreakCrystal",
                        new Taunt("This your reward! Imagine what evil even Oryx needs to keep locked up!"),
                        new Shoot(0, 16, 22.5, fixedAngle: 0, coolDown: 100000),
                        new Spawn("Crystal Prisoner", 1, 1, 100000),
                        new Decay(0)
                        )
                    )
            )
            .Init("Crystal Prisoner",
                new State(
                    new DropPortalOnDeath("Deadwater Docks", 100),
                    new Spawn("Crystal Prisoner Steed", 5, 0, 200),
                    new State("pause",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(2000, "start_the_fun")
                        ),
                    new State("start_the_fun",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("I'm finally free! Yesss!!!"),
                        new TimedTransition(1500, "Daisy_attack")
                        ),
                    new State("Daisy_attack",
                        new Prioritize(
                            new StayCloseToSpawn(0.3, 7),
                            new Wander(0.3)
                            ),
                        new State("Quadforce1",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(0, projectileIndex: 0, count: 4, shootAngle: 90, fixedAngle: 0, coolDown: 400),
                            new TimedTransition(200, "Quadforce2")
                            ),
                        new State("Quadforce2",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(0, projectileIndex: 0, count: 4, shootAngle: 90, fixedAngle: 15, coolDown: 400),
                            new TimedTransition(200, "Quadforce3")
                            ),
                        new State("Quadforce3",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(0, projectileIndex: 0, count: 4, shootAngle: 90, fixedAngle: 30, coolDown: 400),
                            new TimedTransition(200, "Quadforce4")
                            ),
                        new State("Quadforce4",
                            new Shoot(10, projectileIndex: 3, coolDown: 1000),
                            new Shoot(0, projectileIndex: 0, count: 4, shootAngle: 90, fixedAngle: 45, coolDown: 400),
                            new TimedTransition(200, "Quadforce5")
                            ),
                        new State("Quadforce5",
                            new Shoot(0, projectileIndex: 0, count: 4, shootAngle: 90, fixedAngle: 60, coolDown: 400),
                            new TimedTransition(200, "Quadforce6")
                            ),
                        new State("Quadforce6",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(0, projectileIndex: 0, count: 4, shootAngle: 90, fixedAngle: 75, coolDown: 400),
                            new TimedTransition(200, "Quadforce7")
                            ),
                        new State("Quadforce7",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(0, projectileIndex: 0, count: 4, shootAngle: 90, fixedAngle: 90, coolDown: 400),
                            new TimedTransition(200, "Quadforce8")
                            ),
                        new State("Quadforce8",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(10, projectileIndex: 3, coolDown: 1000),
                            new Shoot(0, projectileIndex: 0, count: 4, shootAngle: 90, fixedAngle: 105, coolDown: 400),
                            new TimedTransition(200, "Quadforce1")
                            ),
                        new HpLessTransition(0.3, "Whoa_nelly"),
                        new TimedTransition(18000, "Warning")
                        ),
                    new State("Warning",
                        new Prioritize(
                            new StayCloseToSpawn(0.5, 7),
                            new Wander(0.5)
                            ),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xff00ff00, 0.2, 15),
                        new Follow(0.4, 9, 2),
                        new TimedTransition(3000, "Summon_the_clones")
                        ),
                    new State("Summon_the_clones",
                        new Prioritize(
                            new StayCloseToSpawn(0.85, 7),
                            new Wander(0.85)
                            ),
                        new Shoot(10, projectileIndex: 0, coolDown: 1000),
                        new Spawn("Crystal Prisoner Clone", 4, 0, 200),
                        new TossObject("Crystal Prisoner Clone", 5, 0, 100000),
                        new TossObject("Crystal Prisoner Clone", 5, 60, 100000),
                        new TossObject("Crystal Prisoner Clone", 7, 120, 100000),
                        new TossObject("Crystal Prisoner Clone", 7, 300, 100000),
                        new State("invulnerable_clone",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new TimedTransition(3000, "vulnerable_clone")
                            ),
                        new State("vulnerable_clone",
                            new TimedTransition(1200, "invulnerable_clone")
                            ),
                        new TimedTransition(16000, "Warning2")
                        ),
                    new State("Warning2",
                        new Prioritize(
                            new StayCloseToSpawn(0.85, 7),
                            new Wander(0.85)
                            ),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xff00ff00, 0.2, 25),
                        new TimedTransition(5000, "Whoa_nelly")
                        ),
                    new State("Whoa_nelly",
                        new Prioritize(
                            new StayCloseToSpawn(0.6, 7),
                            new Wander(0.6)
                            ),
                        new Shoot(10, projectileIndex: 3, count: 3, shootAngle: 120, coolDown: 900),
                        new Shoot(10, projectileIndex: 2, count: 3, shootAngle: 15, fixedAngle: 40, coolDown: 1600,
                            coolDownOffset: 0),
                        new Shoot(10, projectileIndex: 2, count: 3, shootAngle: 15, fixedAngle: 220, coolDown: 1600,
                            coolDownOffset: 0),
                        new Shoot(10, projectileIndex: 2, count: 3, shootAngle: 15, fixedAngle: 130, coolDown: 1600,
                            coolDownOffset: 800),
                        new Shoot(10, projectileIndex: 2, count: 3, shootAngle: 15, fixedAngle: 310, coolDown: 1600,
                            coolDownOffset: 800),
                        new State("invulnerable_whoa",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new TimedTransition(2600, "vulnerable_whoa")
                            ),
                        new State("vulnerable_whoa",
                            new TimedTransition(1200, "invulnerable_whoa")
                            ),
                        new TimedTransition(10000, "Absolutely_Massive")
                        ),
                    new State("Absolutely_Massive",
                        new ChangeSize(13, 260),
                        new Prioritize(
                            new StayCloseToSpawn(0.2, 7),
                            new Wander(0.2)
                            ),
                        new Shoot(10, projectileIndex: 1, count: 9, shootAngle: 40, fixedAngle: 40, coolDown: 2000,
                            coolDownOffset: 400),
                        new Shoot(10, projectileIndex: 1, count: 9, shootAngle: 40, fixedAngle: 60, coolDown: 2000,
                            coolDownOffset: 800),
                        new Shoot(10, projectileIndex: 1, count: 9, shootAngle: 40, fixedAngle: 50, coolDown: 2000,
                            coolDownOffset: 1200),
                        new Shoot(10, projectileIndex: 1, count: 9, shootAngle: 40, fixedAngle: 70, coolDown: 2000,
                            coolDownOffset: 1600),
                        new State("invulnerable_mass",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new TimedTransition(2600, "vulnerable_mass")
                            ),
                        new State("vulnerable_mass",
                            new TimedTransition(1000, "invulnerable_mass")
                            ),
                        new TimedTransition(14000, "Start_over_again")
                        ),
                    new State("Start_over_again",
                        new ChangeSize(-20, 100),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xff00ff00, 0.2, 15),
                        new TimedTransition(3000, "Daisy_attack")
                        )
                    ),
                new MostDamagers(3,
                    LootTemplates.Sor3Perc()
                    ),
                new Threshold(0.01,
                    new ItemLoot("Crystalline Blade", 0.0046),
                    new ItemLoot("Spinel Overcoat", 0.0046),
                    new ItemLoot("Potion of Life", 1),
                    new ItemLoot("Potion of Mana", 1),
                    new ItemLoot("Rusted Ring", 0.00112),
                    new TierLoot(2, ItemType.Weapon, 0.05),
                    new TierLoot(2, ItemType.Ability, 0.05),
                    new TierLoot(2, ItemType.Armor, 0.05),
                    new TierLoot(2, ItemType.Ring, 0.05),
                    new TierLoot(3, ItemType.Weapon, 0.025),
                    new TierLoot(3, ItemType.Ability, 0.025),
                    new TierLoot(3, ItemType.Armor, 0.025),
                    new TierLoot(3, ItemType.Ring, 0.025)
                    )
            )
            .Init("Crystal Prisoner Clone",
                new State(
                    new Prioritize(
                        new StayCloseToSpawn(0.85, range: 5),
                        new Wander(0.85)
                        ),
                    new Shoot(10, coolDown: 1400),
                    new State("taunt",
                        new Taunt(0.09, "I am everywhere and nowhere!"),
                        new TimedTransition(1000, "no_taunt")
                        ),
                    new State("no_taunt",
                        new TimedTransition(1000, "taunt")
                        ),
                    new Decay(17000)
                    )
            )
            .Init("Crystal Prisoner Steed",
                new State(
                    new State("change_position_fast",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Prioritize(
                            new StayCloseToSpawn(3.6, 12),
                            new Wander(3.6)
                            ),
                        new TimedTransition(800, "attack")
                        ),
                    new State("attack",
                        new Shoot(10, predictive: 0.3, coolDown: 500),
                        new State("keep_distance",
                            new Prioritize(
                                new StayCloseToSpawn(1, 12),
                                new Orbit(1, 9, target: "Crystal Prisoner", radiusVariance: 0)
                                ),
                            new TimedTransition(2000, "go_anywhere")
                            ),
                        new State("go_anywhere",
                            new Prioritize(
                                new StayCloseToSpawn(1, 12),
                                new Wander(1)
                                ),
                            new TimedTransition(2000, "keep_distance")
                            )
                        )
                    )
            )

        .Init("Large Sor Crystal",
                        new State(
                    new State("Mush1",
                       new HpLessTransition(0.75, "Mush2")
                        ),
                    new State("Mush2",
                        new ChangeSize(2, 140),
                        new HpLessTransition(0.50, "Mush3")
                        ),
                      new State("Mush3",
                          new ChangeSize(2, 100),
                       new HpLessTransition(0.25, "die")
                        ),
                       new State("die",
                        new DropPortalOnDeath("Abandoned Basement Portal", 20),
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
                ),
                new MostDamagers(3,
                    LootTemplates.StatPots()
                ),
                new MostDamagers(3,
                    LootTemplates.SorUncommon()
                    )
            )
            ;
    }
}