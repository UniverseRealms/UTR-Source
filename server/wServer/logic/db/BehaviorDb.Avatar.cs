#region

using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

#endregion
//asdfasdfasdf
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Avatar = () => Behav()

#region shtrs defense system (avatar)
            .Init("shtrs Defense System",
                new State(
                    new ScaleHP(0.3),
                    new DropPortalOnDeath("The Shatters", probability: 1, timeout: 70),
                    new ChangeGroundOnDeath(new[] { "Pure Evil" }, new[] { "shtrs Disaster Floor", "shtrs Shattered Floor" }, 30),
                    new State("attack 1",
                        new HpLessTransition(.8, "shadowmans"),
                        new State("spirals 1",
                            new Taunt(0.7, "BURN!!", "HAHAHAHAHAHA!", "LEAVE THIS PLACE!"),
                            new SetAltTexture(0),
                            new Sequence(
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
                                new SpiralShoot(9, 8, 6, 60, 1, fixedAngle: 9, range: 20, coolDown: 200),
                                new TransitionOnTick("idle 1")
                                )
                            ),
                        new State("idle 1",
                            new Taunt(0.5, "Foolish whelps... leave me be..."),
                            new ConditionalEffect(ConditionEffectIndex.Armored),
                            new SetAltTexture(0),
                            new Flash(0xfFF0000, 0.5, 7),
                            new TimedTransition(4000, "spirals 1")
                            )
                        ),
                    new State("shadowmans",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new State("make some shadowmans",
                            new Taunt("BE CONSUMED BY SHADOW!"),
                            new TossObject("shtrs shadowmans", 5, 0, coolDown: 9999999, coolDownOffset: 1900, count: 8, angleOffset: 45),
                            new TimedTransition(4000, "shadowmans wait")
                            ),
                        new State("shadowmans wait",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new EntityNotExistsTransition("shtrs shadowmans", 100, "attack 2")
                            )
                        ),
                    new State("attack 2",
                        new HpLessTransition(.6, "eyes"),
                        new State("spirals 2",
                            new Taunt(0.7, "BURN!!", "HAHAHAHAHAHA!", "LEAVE THIS PLACE!"),
                            new SetAltTexture(0),
                            new Sequence(
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
                                new SpiralShoot(9, 8, 6, 60, 1, fixedAngle: 9, range: 20, coolDown: 200),
                                new TransitionOnTick("idle 2")
                                )
                            ),
                        new State("idle 2",
                            new Taunt(0.5, "Foolish whelps... leave me be..."),
                            new ConditionalEffect(ConditionEffectIndex.Armored),
                            new SetAltTexture(0),
                            new Flash(0xfFF0000, 0.5, 7),
                            new TimedTransition(4000, "spirals 2")
                            )
                        ),
                    new State("eyes",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("EYE see you!"),
                        new Spawn("shtrs eyeswarmer", 7, 1, 999999999),
                        new EntityNotExistsTransition("shtrs eyeswarmer", 10, "attack 3")
                        ),
                    new State("attack 3",
                        new HpLessTransition(.4, "blobombs"),
                        new Taunt("DO NOT TEST MY PATIENCE!"),
                        new State("spirals 3",
                            new Taunt(0.7, "BURN!!", "HAHAHAHAHAHA!", "LEAVE THIS PLACE!"),
                            new SetAltTexture(0),
                            new Sequence(
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
                                new SpiralShoot(9, 8, 6, 60, 1, fixedAngle: 9, range: 20, coolDown: 200),
                                new TransitionOnTick("idle 3")
                                )
                            ),
                        new State("idle 3",
                            new Taunt(0.5, "Foolish whelps... leave me be..."),
                            new ConditionalEffect(ConditionEffectIndex.Armored),
                            new SetAltTexture(0),
                            new Flash(0xfFF0000, 0.5, 7),
                            new TimedTransition(4000, "spirals 3")
                            )
                        ),
                    new State("blobombs",
                        new State(
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Taunt("YOU SHALL BE FOOD FOR THE ETHER. BLOBS, ATTACK!"),
                            new InvisiToss("shtrs Blobomb", 4, 0, count: 8, angleOffset: 45, coolDown: 9999999),
                            new InvisiToss("shtrs Blobomb", 5, 175, 3000, count: 2, angleOffset: 150),
                            new InvisiToss("shtrs Blobomb", 5, 300, coolDown: 6000),
                            new TimedTransition(12000, "attack 4")
                            ),
                    new State("attack 4",
                        new HpLessTransition(.2, "pillars"),
                        new Taunt("I WILL NOT FALL!"),
                        new State("spirals 4",
                            new Taunt(0.7, "BURN!!", "HAHAHAHAHAHA!", "LEAVE THIS PLACE!"),
                            new SetAltTexture(0),
                            new Sequence(
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
                                new SpiralShoot(9, 8, 6, 60, 1, fixedAngle: 9, range: 20, coolDown: 200),
                                new TransitionOnTick("idle 4")
                                )
                            ),
                        new State("idle 4",
                            new Taunt(0.5, "Foolish whelps... leave me be..."),
                            new ConditionalEffect(ConditionEffectIndex.Armored),
                            new SetAltTexture(0),
                            new Flash(0xfFF0000, 0.5, 7),
                            new TimedTransition(4000, "spirals 4")
                            )
                        ),
                    new State("pillars",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("ACTIVATING PILLAR DEFENSES."),
                        new Order(30, "shtrs Pillar 1", "PROTECT"),
                        new Order(30, "shtrs Pillar 2", "PROTECT"),
                        new Order(30, "shtrs Pillar 3", "PROTECT"),
                        new Order(30, "shtrs Pillar 4", "PROTECT"),
                        new EntitiesNotExistsTransition(30, "attack 5", "shtrs Pillar 1", "shtrs Pillar 2", "shtrs Pillar 3", "shtrs Pillar 4")
                        ),
                    new State("attack 5",
                        new HpLessTransition(.05, "die"),
                        new Taunt("DESTROYING ME WILL ONLY BRING YOU CLOSER TO DEATH!"),
                        new State("spirals 5",
                            new Taunt(0.7, "BURN!!", "LEAVE THIS PLACE!"),
                            new SetAltTexture(0),
                            new Sequence(
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
                                new SpiralShoot(9, 8, 6, 60, 1, fixedAngle: 9, range: 20, coolDown: 200),
                                new TransitionOnTick("idle 5")
                                )
                            ),
                        new State("idle 5",
                            new Taunt(0.5, "Foolish whelps... leave me be..."),
                            new ConditionalEffect(ConditionEffectIndex.Armored),
                            new SetAltTexture(0),
                            new Flash(0xfFF0000, 0.5, 10),
                            new TimedTransition(4000, "spirals 5")
                            )
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
                ),
                new Threshold(0.01,
                    new ItemLoot("Blood Drenched Gown", 0.0084),
                    new ItemLoot("Sacrificial Flask", 0.0046),
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
#endregion shtrs defense system (avatar)

#region minions
            .Init("shtrs shadowmans",
                new State(
                    new HealSelf(coolDown: 3000, amount: 500),
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Sequence(
                        new Shoot(20, 1, projectileIndex: 1),
                        new SpiralShoot(45, 2, 4, coolDown: 2000)
                        )
                    )
                )

            .Init("shtrs eyeswarmer",
                new State(
                    new State("shoot",
                        new Orbit(1.0, 2, 5, "shtrs Defense System"),
                        new Shoot(20, 1, projectileIndex: 0, coolDown: new Cooldown(5000, 500))
                        )
                    )
                )

            .Init("shtrs Pillar 1",
                new State(
                    new State("idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true)
                        ),
                    new State("PROTECT",
                        new Taunt("PROTECT THE AVATAR!"),
                        new State("wait for it...",
                            new TimedTransition(1000, "PROTECT ATTACK")
                            ),
                        new State("PROTECT ATTACK",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new SpiralShoot(18, 2, 10, coolDown: 1500, coolDownOffset: 1000),
                            new Shoot(10, projectileIndex: 1, coolDown: 2500, coolDownOffset: 1000),
                            new TimedTransition(15000, "preparing")
                            ),
                        new State("preparing",
                            new ConditionalEffect(ConditionEffectIndex.Armored),
                            new Flash(0x0D00FF, 0.4, 10),
                            new TimedTransition(25000, "PROTECT ATTACK")
                            )
                        )
                    )
                )

            .Init("shtrs Pillar 2",
                new State(
                    new State("idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true)
                        ),
                    new State("PROTECT",
                        new Taunt("PROTECT THE AVATAR!"),
                        new State("wait for it...",
                            new TimedTransition(11000, "PROTECT ATTACK")
                            ),
                        new State("PROTECT ATTACK",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new SpiralShoot(18, 2, 10, coolDown: 1500, coolDownOffset: 1000),
                            new Shoot(10, projectileIndex: 1, coolDown: 2500, coolDownOffset: 1000),
                            new TimedTransition(15000, "preparing")
                            ),
                        new State("preparing",
                            new ConditionalEffect(ConditionEffectIndex.Armored),
                            new Flash(0x0D00FF, 0.4, 10),
                            new TimedTransition(25000, "PROTECT ATTACK")
                            )
                        )
                    )
                )

            .Init("shtrs Pillar 3",
                new State(
                    new State("idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true)
                        ),
                    new State("PROTECT",
                        new Taunt("PROTECT THE AVATAR!"),
                        new State("wait for it...",
                            new TimedTransition(6000, "PROTECT ATTACK")
                            ),
                        new State("PROTECT ATTACK",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new SpiralShoot(18, 2, 10, coolDown: 1500, coolDownOffset: 1000),
                            new Shoot(10, projectileIndex: 1, coolDown: 2500, coolDownOffset: 1000),
                            new TimedTransition(15000, "preparing")
                            ),
                        new State("preparing",
                            new ConditionalEffect(ConditionEffectIndex.Armored),
                            new Flash(0x0D00FF, 0.4, 10),
                            new TimedTransition(25000, "PROTECT ATTACK")
                            )
                        )
                    )
                )

            .Init("shtrs Pillar 4",
                new State(
                    new State("idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true)
                        ),
                    new State("PROTECT",
                        new Taunt("PROTECT THE AVATAR!"),
                        new State("wait for it...",
                            new TimedTransition(16000, "PROTECT ATTACK")
                            ),
                        new State("PROTECT ATTACK",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new SpiralShoot(18, 2, 10, coolDown: 1500, coolDownOffset: 1000),
                            new Shoot(10, projectileIndex: 1, coolDown: 2500, coolDownOffset: 1000),
                            new TimedTransition(15000, "preparing")
                            ),
                        new State("preparing",
                            new ConditionalEffect(ConditionEffectIndex.Armored),
                            new Flash(0x0D00FF, 0.4, 10),
                            new TimedTransition(25000, "PROTECT ATTACK")
                            )
                        )
                    )
                );
#endregion minions
    }
}