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
        private _ Oryx = () => Behav()
#region oryx the mad god 2

    #region o2 wine cellar
            .Init("Oryx the Mad God 2",
                new State(
                    new ScaleHP(1),
                    new State("dontkillme",
                        new Follow(.1, 15, 3),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(25, 30, fixedAngle: 0, projectileIndex: 7, coolDown: 4000, coolDownOffset: 4000),
                        new Shoot(25, 30, fixedAngle: 30, projectileIndex: 8, coolDown: 4000, coolDownOffset: 4000),
                        new TimedTransition(30000, "Attack")
                        ),
                    new State("Attack",
                        new Wander(.05),
                        new Shoot(25, projectileIndex: 0, count: 8, shootAngle: 45, coolDown: 1500, coolDownOffset: 1500),
                        new Shoot(25, projectileIndex: 1, count: 3, shootAngle: 10, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(25, projectileIndex: 2, count: 3, shootAngle: 10, predictive: 0.2, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(25, projectileIndex: 3, count: 2, shootAngle: 10, predictive: 0.4, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(25, projectileIndex: 4, count: 3, shootAngle: 10, predictive: 0.6, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(25, projectileIndex: 5, count: 2, shootAngle: 10, predictive: 0.8, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(25, projectileIndex: 6, count: 3, shootAngle: 10, predictive: 1, coolDown: 1000, coolDownOffset: 1000),
                        new Taunt(1, 6000, "Puny mortals! My {HP} HP will annihilate you!"),
                        new Spawn("Henchman of Oryx", 5, coolDown: 5000),
                        new HpLessTransition(.3, "prepareRage")
                        ),
                    new State("prepareRage",
                        new Follow(.1, 15, 3),
                        new Taunt("Can't... keep... henchmen... alive... anymore! ARGHHH!!!"),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(25, 30, fixedAngle: 0, projectileIndex: 7, coolDown: 4000, coolDownOffset: 4000),
                        new Shoot(25, 30, fixedAngle: 30, projectileIndex: 8, coolDown: 4000, coolDownOffset: 4000),
                        new TimedTransition(10000, "rage")
                        ),
                    new State("rage",
                        new Follow(.1, 15, 3),
                        new Shoot(25, 30, projectileIndex: 7, coolDown: 90000001, coolDownOffset: 8000),
                        new Shoot(25, 30, projectileIndex: 8, coolDown: 90000001, coolDownOffset: 8500),
                        new Shoot(25, projectileIndex: 0, count: 8, shootAngle: 45, coolDown: 1500, coolDownOffset: 1500),
                        new Shoot(25, projectileIndex: 1, count: 3, shootAngle: 10, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(25, projectileIndex: 2, count: 3, shootAngle: 10, predictive: 0.2, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(25, projectileIndex: 3, count: 2, shootAngle: 10, predictive: 0.4, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(25, projectileIndex: 4, count: 3, shootAngle: 10, predictive: 0.6, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(25, projectileIndex: 5, count: 2, shootAngle: 10, predictive: 0.8, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(25, projectileIndex: 6, count: 3, shootAngle: 10, predictive: 1, coolDown: 1000, coolDownOffset: 1000),
                        new TossObject("Monstrosity Scarab", 7, coolDown: 1000),
                        new Taunt(1, 6000, "Puny mortals! My {HP} HP will annihilate you!")
                        )
                    ),
                new Threshold(0.01,
                    new ItemLoot("Furious Slasher", 0.002),
                    new ItemLoot("Prophecy Sheath", 0.002),
                    new ItemLoot("Mad God's Battle-Gear", 0.002),
                    new ItemLoot("Tyrant's Pendant", 0.002),
                    new ItemLoot("Potion of Life", 1),
                    new ItemLoot("Potion of Restoration", 0.06),
                    new ItemLoot("Potion of Luck", 0.2),
                    new ItemLoot("Vial of Life", 0.01),
                    new ItemLoot("Vial of Mana", 0.01),
                    new ItemLoot("Vial of Attack", 0.01),
                    new ItemLoot("Vial of Speed", 0.01),
                    new ItemLoot("Vial of Defense", 0.01),
                    new ItemLoot("Vial of Dexterity", 0.01),
                    new ItemLoot("Vial of Vitality", 0.01),
                    new ItemLoot("Vial of Wisdom", 0.01),
                    new ItemLoot("Vial of Luck", 0.00625),
                    new ItemLoot("Vial of Restoration", 0.00625),
                    new TierLoot(3, ItemType.Weapon, 0.1),
                    new TierLoot(3, ItemType.Ability, 0.1),
                    new TierLoot(3, ItemType.Armor, 0.1),
                    new TierLoot(3, ItemType.Ring, 0.1),
                    new TierLoot(4, ItemType.Weapon, 0.05),
                    new TierLoot(4, ItemType.Ability, 0.05),
                    new TierLoot(4, ItemType.Armor, 0.05),
                    new TierLoot(4, ItemType.Ring, 0.05),
                    new ItemLoot("Rusted Ring", 0.00125),
                    new ItemLoot("Eternal Essence", 0.001),
                    new ItemLoot("Defender Pet Stone", 0.001),
                    new ItemLoot("Necromancer Pet Stone", 0.001),
                    new ItemLoot("The Zol Awakening (Token)", 0.005),
                    new ItemLoot("Calling of the Titan (Token)", 0.005)
                    )
                )
    #endregion o2 wine cellar

    #region o2 oryx's arena
            .Init("Oryx the Mad God 2OA",
                new State(
                    new ScaleHP(1),
                    new State("Attack",
                        new Wander(.05),
                        new Shoot(25, projectileIndex: 0, count: 8, shootAngle: 45, coolDown: 1500, coolDownOffset: 1500),
                        new Shoot(25, projectileIndex: 1, count: 3, shootAngle: 10, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(25, projectileIndex: 2, count: 3, shootAngle: 10, predictive: 0.2, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(25, projectileIndex: 3, count: 2, shootAngle: 10, predictive: 0.4, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(25, projectileIndex: 4, count: 3, shootAngle: 10, predictive: 0.6, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(25, projectileIndex: 5, count: 2, shootAngle: 10, predictive: 0.8, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(25, projectileIndex: 6, count: 3, shootAngle: 10, predictive: 1, coolDown: 1000, coolDownOffset: 1000),
                        new Taunt(1, 6000, "Puny mortals! My {HP} HP will annihilate you!"),
                        new Spawn("Henchman of Oryx", 5, coolDown: 5000),
                        new HpLessTransition(.2, "prepareRage")
                        ),
                    new State("prepareRage",
                        new Follow(.1, 15, 3),
                        new Taunt("Can't... keep... henchmen... alive... anymore! ARGHHH!!!"),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(25, 30, fixedAngle: 0, projectileIndex: 7, coolDown: 4000, coolDownOffset: 4000),
                        new Shoot(25, 30, fixedAngle: 30, projectileIndex: 8, coolDown: 4000, coolDownOffset: 4000),
                        new TimedTransition(10000, "rage")
                        ),
                    new State("rage",
                        new Follow(.1, 15, 3),
                        new Shoot(25, 30, projectileIndex: 7, coolDown: 90000001, coolDownOffset: 8000),
                        new Shoot(25, 30, projectileIndex: 8, coolDown: 90000001, coolDownOffset: 8500),
                        new Shoot(25, projectileIndex: 0, count: 8, shootAngle: 45, coolDown: 1500, coolDownOffset: 1500),
                        new Shoot(25, projectileIndex: 1, count: 3, shootAngle: 10, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(25, projectileIndex: 2, count: 3, shootAngle: 10, predictive: 0.2, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(25, projectileIndex: 3, count: 2, shootAngle: 10, predictive: 0.4, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(25, projectileIndex: 4, count: 3, shootAngle: 10, predictive: 0.6, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(25, projectileIndex: 5, count: 2, shootAngle: 10, predictive: 0.8, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(25, projectileIndex: 6, count: 3, shootAngle: 10, predictive: 1, coolDown: 1000, coolDownOffset: 1000),
                        new TossObject("Monstrosity Scarab", 7, coolDown: 1000),
                        new Taunt(1, 6000, "Puny mortals! My {HP} HP will annihilate you!")
                        )
                    ),
                new MostDamagers(3,
                    LootTemplates.Sor5Perc()
                    ),
                new Threshold(0.29,
                    new ItemLoot("Greater Potion of Vitality", 1)
                    ),
                new Threshold(0.05,
                    new ItemLoot("Greater Potion of Attack", 0.3),
                    new ItemLoot("Greater Potion of Defense", 0.3),
                    new ItemLoot("Greater Potion of Wisdom", 0.3)
                    ),
                new Threshold(0.1,
                    new TierLoot(10, ItemType.Weapon, 0.07),
                    new TierLoot(11, ItemType.Weapon, 0.06),
                    new TierLoot(12, ItemType.Weapon, 0.05),
                    new TierLoot(5, ItemType.Ability, 0.07),
                    new TierLoot(6, ItemType.Ability, 0.05),
                    new TierLoot(11, ItemType.Armor, 0.07),
                    new TierLoot(12, ItemType.Armor, 0.06),
                    new TierLoot(13, ItemType.Armor, 0.05),
                    new TierLoot(5, ItemType.Ring, 0.06)
                    )
                )
    #endregion o2 oryx's arena

#endregion oryx the mad god 2

#region oryx the mad god 1
            
    #region o1
            .Init("Oryx the Mad God 1",
                new State(
                    new ScaleHP(1),
                    new DropPortalOnDeath("Wine Cellar Portal", timeout: 120),
                    new State("before rage",
                        new HpLessTransition(.4, "rage"),
                        new State("Slow",
                            new Taunt("Fools! I still have {HP} hitpoints!"),
                            new Spawn("Minion of Oryx", 5, 0, 350000),
                            new Reproduce("Minion of Oryx", 10, 5, coolDown: 1500),
                            new Shoot(25, 4, 10, 4, coolDown: 1000),
                            new TimedTransition(20000, "Dance 1")
                            ),
                        new State("Dance 1",
                            new Flash(0xf389E13, 0.5, 60),
                            new Taunt("BE SILENT!!!"),
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(50, 8, projectileIndex: 6, coolDown: 700, coolDownOffset: 200),
                            new TossObject("Ring Element", 9, 0, 320000, count: 15, angleOffset: 24),
                            //new Grenade(radius: 4, damage: 150, fixedAngle: new Random().Next(0, 359), range: 5, coolDown: 2000),
                            //new Grenade(radius: 4, damage: 150, fixedAngle: new Random().Next(0, 359), range: 5, coolDown: 2000),
                            //new Grenade(radius: 4, damage: 150, fixedAngle: new Random().Next(0, 359), range: 5, coolDown: 2000),
                            new TimedTransition(15000, "artifacts")
                            ),
                        new State("artifacts",
                            new Taunt("My Artifacts will protect me!"),
                            new Order(40, "Ring Element", "die"),
                            new Flash(0xf389E13, 0.5, 60),
                            new Shoot(50, 3, projectileIndex: 9, coolDown: 1500, coolDownOffset: 200),
                            new Shoot(50, 10, projectileIndex: 8, coolDown: 2000, coolDownOffset: 200),
                            new Shoot(50, 10, projectileIndex: 7, coolDown: 500, coolDownOffset: 200),
                            new TossObject("Guardian Element 1", 1, 0, 90000001, 1000, count: 4, angleOffset: 90),
                            new TossObject("Guardian Element 2", 9, 0, 90000001, 1000, count: 4, angleOffset: 90),
                            new TimedTransition(25000, "gaze")
                            ),
                        new State("gaze",
                            new Taunt("All who looks upon my face shall die."),
                            new Shoot(count: 2, coolDown: 1000, projectileIndex: 1, radius: 7, shootAngle: 10, coolDownOffset: 800),
                            new TimedTransition(10000, "Dance 2")
                            ),
                        new State("Dance 2",
                            new Flash(0xf389E13, 0.5, 60),
                            new Taunt("Time for more dancing!"),
                            new TossObject("Ring Element", 9, 0, 320000, count: 15, angleOffset: 24),
                            new Shoot(50, 8, projectileIndex: 6, coolDown: 700, coolDownOffset: 200),
                            new Sequence(
                                new SpiralShoot(20, 3, 5, projectileIndex: 8, coolDown: 400),
                                new SpiralShoot(40, 2, 6, projectileIndex: 8, coolDown: 400)
                                )
                            )
                        ),
                    new State("rage",
                        new State("rage 1",
                            new ChangeSize(10, 200),
                            new Taunt(.3, "I HAVE HAD ENOUGH OF YOU!!!", "ENOUGH!!!", "DIE!!!"),
                            new Reproduce("Minion of Oryx", 17, 5, coolDown: 1500),
                            new Shoot(count: 2, coolDown: 1500, projectileIndex: 1, radius: 7, shootAngle: 10, coolDownOffset: 2000),
                            new Shoot(count: 5, coolDown: 1500, projectileIndex: 16, radius: 7, shootAngle: 10, coolDownOffset: 2000),
                            new Follow(0.85, range: 1, coolDown: 0),
                            new Flash(0xfFF0000, 0.5, 9000001),
                            new TimedTransition(8000, "rage 2")
                            ),
                        new State("rage 2",
                            new ReturnToSpawn(1),
                            new SpiralShoot(45, 8, 3, projectileIndex: 1, coolDown: 400),
                            new TimedTransition(9000, "rage 1")
                            )
                        )
                    ),
                new MostDamagers(3,
                    LootTemplates.Sor5Perc()
                    ),
                new MostDamagers(3,
                    new ItemLoot("Greater Potion of Life", 0.3),
                    new ItemLoot("Greater Potion of Mana", 0.3)
                    ),
                new Threshold(0.05,
                    new ItemLoot("Amulet of Oryx", 0.001)
                    ),
                new Threshold(0.05,
                    new ItemLoot("Greater Potion of Attack", 0.6),
                    new ItemLoot("Greater Potion of Defense", 0.6),
                    new ItemLoot("Greater Potion of Dexterity", 0.6),
                    new ItemLoot("Greater Potion of Speed", 0.6),
                    new ItemLoot("The Zol Awakening (Token)", 0.005),
                    new ItemLoot("Calling of the Titan (Token)", 0.005)
                    ),
                new Threshold(0.1,
                    new TierLoot(10, ItemType.Weapon, 0.07),
                    new TierLoot(11, ItemType.Weapon, 0.06),
                    new TierLoot(5, ItemType.Ability, 0.07),
                    new TierLoot(11, ItemType.Armor, 0.07),
                    new TierLoot(5, ItemType.Ring, 0.06)
                    )
                )
    #endregion o1


    #region minions
            .Init("Ring Element",
                new State(
                    new State(
                        new Shoot(50, 12, projectileIndex: 0, coolDown: 700, coolDownOffset: 200),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(20000, "die")
                        ),
                    new State("die", //new Decay(time:0)
                        new Suicide()
                        )
                    )
                )

            .Init("Minion of Oryx",
                new State(
                    new Wander(0.4),
                    new Shoot(3, 3, 10, 0, coolDown: 1000),
                    new Shoot(3, 3, projectileIndex: 1, shootAngle: 10, coolDown: 1000)
                    ),
                new TierLoot(7, ItemType.Weapon, 0.2),
                new ItemLoot("Magic Potion", 0.03)
                )

            .Init("Guardian Element 1",
                new State(
                    new State(
                        new Orbit(1, 1, target: "Oryx the Mad God 1", radiusVariance: 0),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(25, 3, 10, 0, coolDown: 1000),
                        new TimedTransition(10000, "Grow"),
                        new EntityNotExistsTransition("Oryx the Mad God 1", 9, "Despawn")
                        ),
                    new State("Grow",
                        new ChangeSize(100, 200),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Orbit(1, 1, target: "Oryx the Mad God 1", radiusVariance: 0),
                        new Shoot(3, 1, 10, 0, coolDown: 700),
                        new TimedTransition(10000, "Despawn"),
                        new EntityNotExistsTransition("Oryx the Mad God 1", 9, "Despawn")
                        ),
                    new State("Despawn",
                        new Suicide()
                        )
                    )
                )

            .Init("Guardian Element 2",
                new State(
                    new State(
                        new Orbit(1.3, 9, target: "Oryx the Mad God 1", radiusVariance: 0),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(25, 3, 10, 0, coolDown: 1000),
                        new TimedTransition(20000, "Despawn"),
                        new EntityNotExistsTransition("Oryx the Mad God 1", 13, "Despawn")
                        ),
                    new State("Despawn",
                        new Suicide()
                        )
                    )
                )
    #endregion minions

#endregion oryx the mad god 1

#region wine cellar enemies
            .Init("Henchman of Oryx",
                new State(
                    new State("Attack",
                        new Prioritize(
                            new Orbit(.2, 2, target: "Oryx the Mad God 2", radiusVariance: 1),
                            new Wander(.3)
                            ),
                        new Shoot(15, predictive: 1, coolDown: 2500),
                        new Shoot(10, count: 3, shootAngle: 10, projectileIndex: 1, coolDown: 2500),
                        new Spawn("Vintner of Oryx", maxChildren: 1, initialSpawn: 1, coolDown: 5000),
                        //new Spawn("Bile of Oryx", maxChildren: 1, initialSpawn: 1, coolDown: 5000),
                        new Spawn("Aberrant of Oryx", maxChildren: 1, initialSpawn: 1, coolDown: 5000),
                        new Spawn("Monstrosity of Oryx", maxChildren: 1, initialSpawn: 1, coolDown: 5000),
                        new Spawn("Abomination of Oryx", maxChildren: 1, initialSpawn: 1, coolDown: 5000)
                        ),
                    new State("Suicide",
                        new Decay(0)
                        )
                    )
                )

            .Init("Monstrosity of Oryx",
                new State(
                    new State("Wait",
                        new PlayerWithinTransition(15, "Attack")
                        ),
                    new State("Attack",
                        new TimedTransition(10000, "Wait"),
                        new Prioritize(
                            new Orbit(.1, 6, target: "Oryx the Mad God 2", radiusVariance: 3),
                            new Follow(.1, acquireRange: 15),
                            new Wander(.2)
                            ),
                        new TossObject("Monstrosity Scarab", coolDown: 10000, range: 1, angle: 0, coolDownOffset: 1000)
                        )
                    )
                )

            .Init("Monstrosity Scarab",
                new State(
                    new State("Attack",
                        new State("Charge",
                            new Prioritize(
                                new Charge(range: 25, coolDown: 1000),
                                new Wander(.3)
                                ),
                            new PlayerWithinTransition(1, "Boom")
                            ),
                        new State("Boom",
                            new Shoot(1, count: 16, fixedAngle: 0),
                            new Decay(0)
                            )
                        )
                    )
                )

            .Init("Vintner of Oryx",
                new State(
                    new State("Attack",
                        new Prioritize(
                            new Protect(1, "Oryx the Mad God 2", protectionRange: 4, reprotectRange: 3),
                            new Charge(speed: 1, range: 15, coolDown: 2000),
                            new Protect(1, "Henchman of Oryx"),
                            new StayBack(1, 15),
                            new Wander(1)
                            ),
                        new Shoot(10, coolDown: 400)
                        )
                    )
                )

            .Init("Aberrant of Oryx",
                new State(
                    new Prioritize(
                        new Protect(.2, "Oryx the Mad God 2"),
                        new Wander(.7)
                        ),
                    new State("Wait",
                        new PlayerWithinTransition(15, "Attack")
                        ),
                    new State("Attack",
                        new TimedTransition(10000, "Wait"),
                        new TossObject("Aberrant Blaster", 5, coolDown: 4900, minAngle: 0, maxAngle: 360, densityRange: 10, maxDensity: 13)
                        )
                    )
                )

            .Init("Aberrant Blaster",
                new State(
                    new State("Wait",
                        new PlayerWithinTransition(3, "Boom")
                        ),
                    new State("Boom",
                        new Shoot(10, count: 5, shootAngle: 7),
                        new Decay(0)
                        )
                    )
                )

            .Init("Bile of Oryx",
                new State(
                    new Prioritize(
                        new Protect(.4, "Oryx the Mad God 2", protectionRange: 5, reprotectRange: 4),
                        new Wander(.5)
                        )//,
                     //new Spawn("Purple Goo", maxChildren: 20, initialSpawn: 0, coolDown: 1000)
                    )
                )

            .Init("Abomination of Oryx",
                new State(
                    new State("Shoot",
                        new Shoot(1, 3, shootAngle: 5, projectileIndex: 0),
                        new Shoot(1, 5, shootAngle: 5, projectileIndex: 1),
                        new Shoot(1, 7, shootAngle: 5, projectileIndex: 2),
                        new Shoot(1, 5, shootAngle: 5, projectileIndex: 3),
                        new Shoot(1, 3, shootAngle: 5, projectileIndex: 4),
                        new TimedTransition(1000, "Wait")
                        ),
                    new State("Wait",
                        new PlayerWithinTransition(2, "Shoot")),
                    new Prioritize(
                        new Charge(3, 10, 3000),
                        new Wander(.5))
                        )
                    );
#endregion wine cellar minions
    }
}