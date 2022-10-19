using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Infernal = () => Behav()

        #region Bosses
            .Init("Izual",
                new State(
                    new TransferDamageOnDeath("Izual Loot Ctrl"),
                    new State("1",
                        new State(
                            new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new PlayerWithinTransition(10, "2")
                            ),
                    new State("2",
                            new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new Taunt("You really think you can defeat me?", "I am finally free after all these years...", "You mustn't be here now. They are watching."),
                            new TimedTransition(5000, "3")
                            ),
                    new State("3",
                            new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new Taunt("Let's see how well you fair against my unmatched power.", "Step forward, brave challenger.", "Watch where you stand."),
                            new TimedTransition(5000, "4")
                            ),
                        new State("4",
                            new Taunt("You will succumb to the flames."),
                            new OrderOnce(50, "Izual Ground Crtl 4", "shrink"),
                            new Shoot(99, 4, 3, 0, fixedAngle: 90),
                            new Flash(0xfFF0000, 2, 2),
                            new TimedTransition(4000, "5")
                            )
                        ),
                    new State("5",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("Let's make this more interesting."),
                        new MoveTo2(0, 10, once: true),
                        new OrderOnce(50, "Izual Ground Crtl 3", "shrink"),
                        new TimedTransition(4000, "6")
                        ),
                    new State("6",
                        new Taunt("DESTROYING ME WILL ONLY BRING YOU CLOSER TO DEATH!"),
                        new State("spirals 5",
                            new TimedTransition(10000, "fake"),
                            new Taunt(0.7, "BURN!!", "LEAVE THIS PLACE!"),
                                new SpiralShoot(20, 4, 3, 10, fixedAngle: 160, range: 20, coolDown: 200),
                                new Simultaneous(
                                    new SpiralShoot(10, 4, 3, 10, fixedAngle: 100, range: 20, coolDown: 200),
                                    new SpiralShoot(10, 4, 3, 10, fixedAngle: 220, range: 20, coolDown: 200),
                                    new SpiralShoot(10, 4, 3, 10, fixedAngle: 340, range: 20, coolDown: 200)
                                    ),
                                new SpiralShoot(9, 11, 6, 60, 1, fixedAngle: 0, range: 20, coolDown: 200),
                                new SpiralShoot(-9, 6, 6, 60, 1, fixedAngle: 27, range: 20, coolDown: 200),
                                new SpiralShoot(9, 7, 6, 60, fixedAngle: 45, range: 20, coolDown: 200),
                                new SpiralShoot(-9, 11, 6, 60, fixedAngle: 99, range: 20, coolDown: 200),
                                new SpiralShoot(9, 8, 6, 60, 1, fixedAngle: 9, range: 20, coolDown: 200)
                                )
                            ),
                        new State("fake",
                            new Taunt(". . . ."),
                            new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new Flash(0xfFF0000, 0.5, 10),
                            new TimedTransition(5000, "die")
                        ),
                    new State("die",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new State("die die",
                            new Taunt("YOU KNOW NOT WHAT YOU HAVE DONE!"),
                            new TimedTransition(4000, "suicide")
                            ),
                        new State("suicide",
                            new Suicide()
                         )
                      )
                    )
                    )
        .Init("Izual, The Nefarious",
            new State(
                new TransferDamageOnDeath("Izual Loot Ctrl"),
                new TransformOnDeath("Izual Ground Crtl 1"),
                new ScaleHP(0.1),
                new State("waiting",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new EntityNotExistsTransition("Izual", 50, "ready")
                    ),
                new State("ready",
                    new PlayerWithinTransition(8, "boo")
                    ),
                new State("boo",
                    new ChangeSize(50, 100),
                    new ConditionEffectRegion(ConditionEffectIndex.Paused, range: 20, duration: 6000),
                    new Taunt("YOU FELL FOR MY TRAP!", "IT'S OVER FOR YOU.", "HAHAHAHA!!"),
                    new TimedTransition(4000, "think")
                    ),
                new State("think",
                    new Taunt("You really thought you would get away with this mockery?"),
                    new TimedTransition(4000, "prep")
                    ),
                new State("prep",
                    new Taunt("I'll show you how truly powerless you are in my grasp!"),
                    new MoveTo2(0, 20, once: true),
                    new Flash(0xff0000, 1, 3),
                    new TimedTransition(5000, "kill")
                    ),
                new State("kill",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                        new Spawn("Izual Anchor", maxChildren: 1, initialSpawn: 0, coolDown: 99999),
                        new Shoot(10, projectileIndex: 0, count: 10, shootAngle: 36, fixedAngle: 0, coolDown: 2500),
                        new Shoot(10, projectileIndex: 1, count: 3, shootAngle: 36, coolDown: 1200),
                        new Shoot(10, projectileIndex: 3, count: 10, shootAngle: 10, coolDown: 2000),
                        new HpLessTransition(0.5, "final phase")
                        ),
                    new State("final phase",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("You think you can get away with this? I'LL BURN YOU TO ASHES"),
                        new MoveTo2(0, -15, once: true),
                        new TimedTransition(4000, "thelast")
                        ),
                    new State("thelast",
                        new Order(20, "Izual Anchor", "shoot"),
                        new Orbit(5, radius: 15),
                        new Shoot(10, projectileIndex: 2, count: 10, shootAngle: 15, coolDown: 1500, coolDownOffset: 0),
                        new Shoot(10, projectileIndex: 3, count: 5, shootAngle: 20, coolDown: 1500, coolDownOffset: 200),
                        new HpLessTransition(0.25, "opposite rotate")
                        ),
                    new State("opposite rotate",
                        new Orbit(10, radius: 15, orbitClockwise: true),
                        new Taunt("I'LL DROWN YOU IN A SEA OF FLAMES! ALL WILL BURN!"),
                        new Shoot(10, projectileIndex: 0, count: 8, shootAngle: 20, coolDown: 2000, coolDownOffset: 0),
                        new Shoot(10, projectileIndex: 4, count: 3, shootAngle: 36, coolDown: 2000, coolDownOffset: 400),
                        new HpLessTransition(0.1, "dying")
                        ),
                    new State("dying",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Taunt("..."),
                        new Order(20, "Izual Anchor", "ded"),
                        new TimedTransition(3000, "suicide")
                        ),
                    new State("suicide",
                        new Suicide()
                        )
                    )
                )
        .Init("Ignatius, The Vengeful Spirit",
            new State(
                new ScaleHP(0.3),
                new State("1",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new PlayerWithinTransition(8, "2", true)
                    ),
                new State("2",
                    new TimedTransition(3000, "3")
                    ),
                new State("3",
                    new Taunt("Fools. You will learn not to play with fire."),
                    new TimedTransition(4000, "4")
                    ),
                new State("4",
                    new SoundPlay(),
                    new Taunt("A fate sealed in blood."),
                    new TimedTransition(4000, "5")
                    ),
                new State("5",
                    new SoundPlay(1),
                    new Flash(0xFF0000, 3, 6),
                    new Taunt("BY FIRE BE PURGED."),
                    new TimedTransition(4000, "6")
                    ),
                new State("6",
                    new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                    new Grenade(6, 50, 5, null, coolDown: 800, effect: ConditionEffectIndex.Sick),
                    new Shoot(99, 9, 90, 2, coolDown: 950),
                    new Shoot(99, 9, 74, 0, coolDown: 225),
                    new HpLessTransition(0.8, "7 heal")
                    ),
                new State("7",
                    new Prioritize(
                        new Follow(1.5, 10, 4),
                        new Charge(3, 10, coolDown: 2000)
                        ),
                    new Grenade(6, 140, 6, coolDown: 1900, effect: ConditionEffectIndex.Weak, effectDuration: 2500),
                    new Grenade(6, 140, 6, coolDown: 2000, effect: ConditionEffectIndex.Sick, effectDuration: 2500),
                    new Shoot(99, 4, 24, 1, coolDown: 1500),
                    new Shoot(99, 6, 60, 2, coolDown: 1700),
                    new HpLessTransition(0.5, "9")
                    ),
                new State("7 heal",
                    new Prioritize(
                        new Follow(1.5, 10, 4),
                        new Charge(3, 10, coolDown: 2000)
                        ),
                    new HealSelf(amount: 200000, coolDown: 999999),
                    new Grenade(6, 140, 6, coolDown: 1900, effect: ConditionEffectIndex.Weak, effectDuration: 2500),
                    new Grenade(6, 140, 6, coolDown: 2000, effect: ConditionEffectIndex.Sick, effectDuration: 2500),
                    new Shoot(99, 4, 24, 1, coolDown: 1500),
                    new Shoot(99, 6, 60, 2, coolDown: 1700),
                    new HpLessTransition(0.75, "8")
                    ),
                new State("8",
                    new Taunt("PAIN OF A THOUSAND DAGGERS PIERCING INTO YOUR SKIN."),
                    new SpiralShoot(13, 20, 6, 60, 3, coolDown: 333),
                    new Shoot(99, 4, 32, 2, coolDown: 1500, coolDownOffset: 3000),
                    new HpLessTransition(0.5, "9"),
                    new TimedTransition(10000, "7")
                    ),
                new State("9",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Taunt("Rise, my children."),
                    new InvisiToss("Fiery Deity", 3, angle: 40, coolDown: 500000, count: 1),
                    new InvisiToss("Fiery Deity", 3, angle: 80, coolDown: 500000, count: 1),
                    new InvisiToss("Fiery Deity", 3, angle: 120, coolDown: 500000, count: 1),
                    new InvisiToss("Fiery Deity", 3, angle: 160, coolDown: 500000, count: 1),
                    new Shoot(99, 4, 32, 2, coolDown: 1500, coolDownOffset: 500),
                    new TimedTransition(8000, "10 wait")
                    ),
                new State("10 wait",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Taunt("THE SPIRITS SHALL RETURN!"),
                    new Flash(0xADD8E6, 5, 2),
                    new TimedTransition(5000, "10 cd")
                    ),
                new State("10 cd",
                    new Shoot(99, 9, 40, 4, coolDown: 333),
                    new HpLessTransition(0.1, "11"),
                    new TimedTransition(8000, "10 alt")
                    ),
                new State("10 alt",
                    new Taunt("..."),
                    new HpLessTransition(0.1, "11"),
                    new TimedTransition(5000, "10 cd")
                    ),
                new State("11",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Taunt("How dare you proceed into our domain. . ."),
                    new TimedTransition(3000, "12")
                    ),
                new State("12",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Suicide()
                    )
                ),
                new Threshold(0.05,
                    new ItemLoot("Gold Cache", 1),
                    new ItemLoot("Gold Cache", 1)
                )
            )
        #endregion Bosses

        #region Misc
        .Init("Fiery Deity",
                new State(
                    new State("1",
                    new Prioritize(
                        new Swirl(0.4, 7),
                        new Wander(0.4)
                        ),
                        new Taunt("THE SPIRITS WILL RETURN."),
                        new Shoot(99, 4, 4, coolDown: 1000),
                        new Shoot(99, 8, 40, 1, coolDown: 1200, coolDownOffset: 5000)
                        ),
                new State("kill",
                    new Suicide()
                    )
                ),
                new Threshold(0.05,
                    new ItemLoot("Gold Cache", 1)
                    )
                )
        .Init("Izual Loot Ctrl",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new State("Idle",
                        new EntityNotExistsTransition("Izual, the Nefarious", 50, "kill")
                        ),
                new State("kill",
                    new Suicide()
                    )
                ),
                new Threshold(0.05,
                    new ItemLoot("Gold Cache", 1),
                    new ItemLoot("10000 Gold", 0.05),
                    new ItemLoot("Onrane Cache", 0.33),
                    new ItemLoot("Onrane", 1),
                    new ItemLoot("Change of Heart", 0.000333)
                    )
                )
        .Init("Izual Ground Crtl 1",
            new State(
                new TransformOnDeath("Izual Ground Crtl 1"),
                new State("waiting",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("shrink",
                    new ChangeGroundOnDeath(new[] { "Corrupt Floor 1", "Corrupt Floor 2" }, new[] { "Corrupt Lava" }, 1),
                    new Suicide()
                    )
                )
            )
        .Init("Izual Ground Crtl 2",
            new State(
                new TransformOnDeath("Izual Ground Crtl 2"),
                new State("waiting",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("shrink",
                    new ChangeGroundOnDeath(new[] { "Corrupt Floor 1", "Corrupt Floor 2" }, new[] { "Corrupt Lava" }, 1),
                    new Suicide()
                    )
                )
            )
        .Init("Izual Ground Crtl 3",
            new State(
                new TransformOnDeath("Izual Ground Crtl 3"),
                new State("waiting",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("shrink",
                    new ChangeGroundOnDeath(new[] { "Corrupt Lava" }, new[] { "Corrupt Floor 1" }, 1),
                    new Suicide()
                    ),
                new State("shrink2",
                    new ChangeGroundOnDeath(new[] { "Corrupt Floor 1" }, new[] { "Corrupt Lava" }, 1),
                    new Suicide()
                    )
                )
            )
        .Init("Izual Ground Crtl 4",
            new State(
                new TransformOnDeath("Izual Ground Crtl 4"),
                new State("waiting",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("shrink",
                    new ChangeGroundOnDeath(new[] { "Corrupt Lava" }, new[] { "Corrupt Lava 2", "Corrupt Lava 3", "Corrupt Lava 4" }, 1),
                    new Suicide()
                    )
                )
            )
        .Init("Izual Anchor",
            new State(
                new State("waiting",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("shoot",
                    new Shoot(10, projectileIndex: 0, count: 10, shootAngle: 36, fixedAngle: 0, coolDown: 3000)
                    ),
                new State("ded",
                    new Suicide()
                    )
                )
            )
        .Init("Izual Ground Crtl 5",
            new State(
                new TransformOnDeath("Izual Ground Crtl 5"),
                new State("waiting",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("shrink",
                    new ChangeGroundOnDeath(new[] { "Corrupt Floor 1", "Corrupt Floor 2" }, new[] { "Corrupt Lava" }, 1),
                    new Suicide()
                    )
                )
            );
        #endregion Misc


    }
}