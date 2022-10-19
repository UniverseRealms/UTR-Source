using wServer.logic.behaviors;
using wServer.logic.transitions;
using common.resources;
using wServer.logic.loot;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ LostHalls = () => Behav()
            .Init("LH Void Entity",
                new State(
                    new ScaleHP(0.3),
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new Taunt("Those who enter my realm hold little value of their lives."),
                        new ChangeSize(250, 0),
                        new SetAltTexture(0),
                        new PlayerWithinTransition(10, "2", true)
                        ),
                    new State("2",
                        new ChangeSize(15, 200),
                        new Taunt("Ah, heroes, blessed souls… I shall destroy you with the power of shadow!"),
                        new TimedTransition(5000, "3")
                        ),
                    new State("3",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                        new TimedTransition(100, "4", true),
                        new TimedTransition(100, "5", true),
                        new TimedTransition(100, "6", true),
                        new TimedTransition(100, "7", true),
                        new TimedTransition(100, "8", true),
                        new TimedTransition(100, "9", true),
                        new TimedTransition(100, "10", true)
                        ),
                new State(
                    new Orbit(1, 20, 30, target: "LH Void Controller", orbitClockwise: true),
                    new HpLessTransition(0.8, "11"),
                    new State("4",
                        new Shoot(99, 12, 11, 5, coolDown: 3000),
                        new Shoot(99, 3, 14, 0, coolDown: 600),
                        new TimedTransition(8000, "3")
                        ),
                    new State("5",
                        new Shoot(99, 8, 14, 4, coolDown: 1500),
                        new Shoot(99, 3, 13, 0, coolDown: 750),
                        new TimedTransition(8000, "3")
                        ),
                    new State("6",
                        new Shoot(99, 6, 22, 0, coolDown: 2000),
                        new Shoot(99, 3, 20, 3, coolDown: 800),
                        new TimedTransition(8000, "3")
                        ),
                    new State("7",
                        new Shoot(99, 10, 9, 0, coolDown: 1000),
                        new Shoot(99, 2, 12, 1, coolDown: 400),
                        new TimedTransition(8000, "3")
                        ),
                    new State("8",
                        new Shoot(99, 9, 13, 6, coolDown: 1000),
                        new TimedTransition(8000, "3")
                        ),
                    new State("9",
                        new Shoot(99, 10, 7, 7, coolDown: 1000),
                        new Shoot(99, 3, 8, 6, coolDown: 300),
                        new TimedTransition(8000, "3")
                        ),
                    new State("10",
                        new Shoot(99, 16, null, 4, fixedAngle: 24, coolDown: 1000),
                        new Shoot(99, 2, 12, 5, coolDown: 400),
                        new TimedTransition(8000, "3")
                            )
                        ),
                new State(
                    new TimedRandomTransition(60000, true, "mid"),
                    new Orbit(1.5, 20, 30, target: "LH Void Controller", orbitClockwise: true),
                    new HpLessTransition(0.4, "13"),
                    new State("11",
                        new Taunt("Spirits of transgression, I command you to swarm these blights!"),
                        new Order(99, "LH Void Spawner", "2"),
                        new Order(99, "LH Void Controller", "2"),
                        new Order(99, "LH Void Split", "2"),
                        new TimedTransition(2000, "12")
                        ),
                    new State("12",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                        new TimedTransition(100, "4a", true),
                        new TimedTransition(100, "5a", true),
                        new TimedTransition(100, "6a", true),
                        new TimedTransition(100, "7a", true),
                        new TimedTransition(100, "8a", true),
                        new TimedTransition(100, "9a", true),
                        new TimedTransition(100, "10a", true)
                        ),
                    new State("4a",
                        new Shoot(99, 12, 11, 5, coolDown: 3000),
                        new Shoot(99, 3, 14, 0, coolDown: 600),
                        new TimedTransition(8000, "12")
                        ),
                    new State("5a",
                        new Shoot(99, 8, 14, 4, coolDown: 1500),
                        new Shoot(99, 3, 13, 0, coolDown: 750),
                        new TimedTransition(8000, "12")
                        ),
                    new State("6a",
                        new Shoot(99, 6, 22, 0, coolDown: 2000),
                        new Shoot(99, 3, 20, 3, coolDown: 800),
                        new TimedTransition(8000, "12")
                        ),
                    new State("7a",
                        new Shoot(99, 10, 9, 0, coolDown: 1000),
                        new Shoot(99, 2, 12, 1, coolDown: 400),
                        new TimedTransition(8000, "12")
                        ),
                    new State("8a",
                        new Shoot(99, 9, 13, 6, coolDown: 1000),
                        new TimedTransition(8000, "12")
                        ),
                    new State("9a",
                        new Shoot(99, 10, 7, 7, coolDown: 1000),
                        new Shoot(99, 3, 8, 6, coolDown: 300),
                        new TimedTransition(8000, "12")
                        ),
                    new State("10a",
                        new Shoot(99, 16, null, 4, fixedAngle: 24, coolDown: 1000),
                        new Shoot(99, 2, 12, 5, coolDown: 400),
                        new TimedTransition(8000, "12")
                                )
                            ),
                new State(
                    new HpLessTransition(0.2, "ends"),
                    new Orbit(2, 20, 30, target: "LH Void Controller", orbitClockwise: true),
                    new TimedRandomTransition(60000, true, "mid"),
                    new State("13",
                        new Taunt("I have fed off the sorrow and hatred of the lost souls… AND I WILL FEED AGAIN!", "HA HA HA! You shall be enveloped by darkness!"),
                        new TimedTransition(1000, "14")
                        ),
                    new State("14",
                        new TimedTransition(100, "4b", true),
                        new TimedTransition(100, "5b", true),
                        new TimedTransition(100, "6b", true),
                        new TimedTransition(100, "7b", true),
                        new TimedTransition(100, "8b", true),
                        new TimedTransition(100, "9b", true),
                        new TimedTransition(100, "10b", true)
                        ),
                    new State("4b",
                        new Shoot(99, 12, 11, 5, coolDown: 3000),
                        new Shoot(99, 3, 14, 0, coolDown: 600),
                        new TimedTransition(8000, "14")
                        ),
                    new State("5b",
                        new Shoot(99, 8, 14, 4, coolDown: 1500),
                        new Shoot(99, 3, 13, 0, coolDown: 750),
                        new TimedTransition(8000, "14")
                        ),
                    new State("6b",
                        new Shoot(99, 6, 22, 0, coolDown: 2000),
                        new Shoot(99, 3, 20, 3, coolDown: 800),
                        new TimedTransition(8000, "14")
                        ),
                    new State("7b",
                        new Shoot(99, 10, 9, 0, coolDown: 1000),
                        new Shoot(99, 2, 12, 1, coolDown: 400),
                        new TimedTransition(8000, "14")
                        ),
                    new State("8b",
                        new Shoot(99, 9, 13, 6, coolDown: 1000),
                        new TimedTransition(8000, "14")
                        ),
                    new State("9b",
                        new Shoot(99, 10, 7, 7, coolDown: 1000),
                        new Shoot(99, 3, 8, 6, coolDown: 300),
                        new TimedTransition(8000, "14")
                        ),
                    new State("10b",
                        new Shoot(99, 16, null, 4, fixedAngle: 24, coolDown: 1000),
                        new Shoot(99, 2, 12, 5, coolDown: 400),
                        new TimedTransition(8000, "14")
                                )
                            ),
                new State(
                    new State("mid",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new SetAltTexture(25),
                        new TimedTransition(100, "mid2")
                        ),
                    new State("mid2",
                        new SetAltTexture(26),
                        new TimedTransition(100, "mid3")
                        ),
                    new State("mid3",
                        new SetAltTexture(27),
                        new TimedTransition(100, "mid4")
                        ),
                    new State("mid4",
                        new SetAltTexture(28),
                        new TimedTransition(100, "mid4a")
                        ),
                    new State("mid4a",
                        new SetAltTexture(29),
                        new TimedTransition(100, "mid5")
                        ),
                    new State("mid5",
                        new SetAltTexture(30),
                        new TimedTransition(2000, "mid6")
                        ),
                    new State("mid6",
                        new MoveTo2(62, 42, 2, true, true, true),
                        new TimedTransition(1000, "mid7")
                        ),
                    new State("mid7",
                        new SetAltTexture(18),
                        new TimedTransition(50, "mid8")
                        ),
                    new State("mid8",
                        new SetAltTexture(19),
                        new TimedTransition(50, "mid9")
                        ),
                    new State("mid9",
                        new SetAltTexture(20),
                        new TimedTransition(50, "mid10")
                        ),
                    new State("mid10",
                        new SetAltTexture(21),
                        new TimedTransition(50, "mid11")
                        ),
                    new State("mid11",
                        new SetAltTexture(22),
                        new TimedTransition(50, "mid12")
                        ),
                    new State("mid12",
                        new SetAltTexture(23),
                        new TimedTransition(50, "mid13")
                        ),
                    new State("mid13",
                        new SetAltTexture(24),
                        new TimedTransition(50, "mid14")
                        ),
                    new State("mid14",
                        new SetAltTexture(0),
                        new HpLessTransition(0.2, "ends1"),
                        new TimedTransition(2000, "15", true),
                        new TimedTransition(2000, "16", true),
                        new TimedTransition(2000, "17", true),
                        new TimedTransition(2000, "18")
                        ),
                    new State("15",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                        new SpiralShoot(15, 360, 6, null, 20, 60, coolDown: 150),
                        new TimedTransition(6000, "19")
                        ),
                    new State("16",
                        new SpiralShoot(30, 360, 8, null, 19, 45, coolDown: 300),
                        new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                        new TimedTransition(6000, "19")
                        ),
                    new State("17",
                        new Shoot(99, 18, 20, 5, coolDown: 800),
                        new TossObject("LH Greater Void Shade", 15, 90, 2900),
                        new TossObject("LH Greater Void Shade", 15, 180, 2900),
                        new TossObject("LH Greater Void Shade", 15, 270, 2900),
                        new TossObject("LH Greater Void Shade", 15, 360, 2900),
                        new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                        new TimedTransition(6000, "19")
                        ),
                    new State("18",
                        new SpiralShoot(30, 360, 8, null, 18, 45, coolDown: 400),
                        new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                        new TimedTransition(6000, "19")
                        ),
                    new State("19",
                        new Wander(0.1),
                        new StayCloseToSpawn(0.4, 1),
                        new TimedTransition(2000, "return1")
                            ),
                    new State("return1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new SetAltTexture(25),
                        new TimedTransition(50, "return2")
                        ),
                    new State("return2",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new SetAltTexture(26),
                        new TimedTransition(50, "return3")
                        ),
                    new State("return3",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new SetAltTexture(27),
                        new TimedTransition(50, "return4")
                        ),
                    new State("return4",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new SetAltTexture(28),
                        new TimedTransition(50, "return5")
                        ),
                    new State("return5",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new SetAltTexture(29),
                        new TimedTransition(50, "return6")
                        ),
                    new State("return6",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new SetAltTexture(30),
                        new TimedTransition(1000, "return7")
                        ),
                    new State("return7",
                        new MoveTo2(62, 23, 2, true, true, true),
                        new TimedTransition(1000, "return8")
                        ),
                    new State("return8",
                        new SetAltTexture(18),
                        new TimedTransition(50, "return9")
                        ),
                    new State("return9",
                        new SetAltTexture(19),
                        new TimedTransition(50, "return10")
                        ),
                    new State("return10",
                        new SetAltTexture(20),
                        new TimedTransition(50, "return11")
                        ),
                    new State("return11",
                        new SetAltTexture(21),
                        new TimedTransition(50, "return12")
                        ),
                    new State("return12",
                        new SetAltTexture(22),
                        new TimedTransition(50, "return13")
                        ),
                    new State("return13",
                        new SetAltTexture(23),
                        new TimedTransition(50, "return14")
                        ),
                    new State("return14",
                        new SetAltTexture(24),
                        new TimedTransition(100, "return15")
                        ),
                    new State("return15",
                        new SetAltTexture(0),
                        new TimedTransition(2000, "12")
                            )
                        ),
                    new State("ends",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new Orbit(0.5, 20, 30, target: "LH Void Controller", orbitClockwise: true),
                        new Order(99, "LH Void Spawner", "1"),
                        new Taunt("ALL NOW ENDS!"),
                        new TimedTransition(5000, "mid")
                        ),
                    new State("ends1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new Shoot(99, 18, 20, 5, coolDown: 1000, coolDownOffset: 1500),
                        new TimedTransition(10000, "ends2")
                        ),
                    new State("ends2",
                        new TimedTransition(100, "ends3")
                        ),
                    new State("ends3",
                        new SpiralShoot(15, 360, 6, null, 20, 60, coolDown: 150),
                        new TimedTransition(8000, "ends4")
                        ),
                    new State("ends4",
                        new SpiralShoot(30, 360, 8, null, 19, 45, coolDown: 300),
                        new TimedTransition(8000, "ends5")
                        ),
                    new State("ends5",
                        new SpiralShoot(30, 360, 8, null, 18, 45, coolDown: 400),
                        new TimedTransition(8000, "survival1")
                        ),
                    new State("survival1",
                        new TossObject("LH Void Entity N", range: 20, angle: 0, coolDown: 99999),
                        new TossObject("LH Void Entity W", range: 20, angle: 270, coolDown: 99999),
                        new TossObject("LH Void Entity S", range: 20, angle: 180, coolDown: 99999),
                        new TossObject("LH Void Entity E", range: 20, angle: 90, coolDown: 99999),
                        new Taunt("Brace yourselves, for this shall be the last battle your precious realm will ever see!"),
                        new TimedTransition(5000, "survival2")
                        ),
                    new State("survival2",
                        new TossObject("LH Greater Void Shade", 10, 45, 12000),
                        new TossObject("LH Greater Void Shade", 10, 135, 12000),
                        new TossObject("LH Greater Void Shade", 10, 235, 12000),
                        new TossObject("LH Greater Void Shade", 10, 315, 12000),
                        new EntitiesNotExistsTransition(99, "speech1", "LH Void Entity N", "LH Void Entity S", "LH Void Entity E", "LH Void Entity W")
                        ),
                    new State("speech1",
                        new Taunt(true, "A fallen king, an imprisoned queen, an effigy of gold."),
                        new TimedTransition(1500, "speech2")
                        ),
                    new State("speech2",
                        new Taunt(true, "A glacial elder who knows more than you think, and a necromancer of old."),
                        new TimedTransition(1500, "speech3")
                        ),
                    new State("speech3",
                        new Taunt(true, "An experiment that surpassed its master, a ventroliquist's final show."),
                        new TimedTransition(1500, "speech4")
                        ),
                    new State("speech4",
                        new Taunt(true, "And a white titan to conquer the Mad God…"),
                        new TimedTransition(1500, "speech5")
                        ),
                    new State("speech5",
                        new Taunt(true, "ALL UNDER MY CONTROL!"),
                        new TimedTransition(2000, "shotgun")
                        ),
                    new State("shotgun",
                        new Shoot(99, 18, null, 21, fixedAngle: 20, coolDown: 490),
                        new TimedTransition(2000, "shotgun2")
                        ),
                    new State("shotgun2",
                        new Shoot(99, 10, 10, 21, coolDown: 490),
                        new TimedTransition(4000, "final")
                        ),
                    new State("final",
                        new Order(99, "LH Void Split", "3"),
                        new SetAltTexture(1),
                        new Taunt("No…NO! THIS IS NOT THE END!"),
                        new TimedTransition(3000, "chest1")
                        ),
                    new State("chest1",
                        new SetAltTexture(2),
                        new TimedTransition(100, "chest2")
                        ),
                    new State("chest2",
                        new SetAltTexture(3),
                        new TimedTransition(100, "chest3")
                        ),
                    new State("chest3",
                        new SetAltTexture(4),
                        new TimedTransition(100, "chest4")
                        ),
                    new State("chest4",
                        new SetAltTexture(5),
                        new TimedTransition(1500, "chest5")
                        ),
                    new State("chest5",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                        new Wander(0.8),
                        new HpLessTransition(0.08, "death")
                        ),
                    new State("death",
                        new Taunt("Naivety! You really think you have won? The inevitable has merely been delayed. One day you shall all be slain, and on that day I will claim your souls in vengeance."),
                        new Suicide()
                        )
                    ),
                new Threshold(0.01,
                    new ItemLoot("Vial Crate", 1),
                    new ItemLoot("Vial of Life", 0.04),
                    new ItemLoot("Vial of Mana", 0.04),
                    new ItemLoot("Vial of Attack", 0.04),
                    new ItemLoot("Vial of Speed", 0.04),
                    new ItemLoot("Vial of Defense", 0.04),
                    new ItemLoot("Vial of Dexterity", 0.04),
                    new ItemLoot("Vial of Vitality", 0.04),
                    new ItemLoot("Vial of Wisdom", 0.04),
                    new ItemLoot("Vial of Luck", 0.025),
                    new ItemLoot("Vial of Restoration", 0.025),
                    new ItemLoot("Shiv of the Void", 0.002),
                    new ItemLoot("Void Matter", 0.002),
                    new ItemLoot("Overcoat of Darkness", 0.002),
                    new ItemLoot("Oblivion Keepsake", 0.002),
                    new ItemLoot("Lost Halls Key", 0.005),
                    new ItemLoot("Void Pet Stone", 0.001)
                )
                )
            .Init("LH Void Entity N",
                new State(
                    new ScaleHP(0.3),
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new ChangeSize(250, 0),
                        new TimedTransition(5000, "2")
                        ),
                    new State("2",
                        new HpLessTransition(0.4, "3"),
                        new Orbit(0.7, 20, 30, target: "LH Void Controller", orbitClockwise: true),
                        new ChangeSize(20, 200),
                        new Taunt(true, "Join us, heroes! The Void lusts for your souls!"),
                        new Shoot(99, 5, 30, 2)
                        ),
                    new State("3",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new Orbit(0.7, 20, 30, target: "LH Void Controller", orbitClockwise: true),
                        new Taunt(true, "The end of the world is upon us! Relinquish your spirits to the Void, and watch helplessly as your universe is eradicated!"),
                        new TimedTransition(2000, "4")
                        ),
                    new State("4",
                        new Suicide()
                        )
                    )
                )
            .Init("LH Void Entity S",
                new State(
                    new ScaleHP(0.3),
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new ChangeSize(250, 0),
                        new TimedTransition(5000, "2")
                        ),
                    new State("2",
                        new HpLessTransition(0.4, "3"),
                        new Orbit(0.7, 20, 30, target: "LH Void Controller", orbitClockwise: true),
                        new ChangeSize(20, 200),
                        new Taunt(true, "Join us, heroes! The Void lusts for your souls!"),
                        new Shoot(99, 5, 30, 2)
                        ),
                    new State("3",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new Orbit(0.7, 20, 30, target: "LH Void Controller", orbitClockwise: true),
                        new Taunt(true, "Eons of meticulous planning have lead to this moment. I will not let such a mindless crowd get in my way!"),
                        new TimedTransition(2000, "4")
                        ),
                    new State("4",
                        new Suicide()
                        )
                    )
                )
            .Init("LH Void Entity E",
                new State(
                    new ScaleHP(0.3),
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new ChangeSize(250, 0),
                        new TimedTransition(5000, "2")
                        ),
                    new State("2",
                        new HpLessTransition(0.4, "3"),
                        new Orbit(0.7, 20, 30, target: "LH Void Controller", orbitClockwise: true),
                        new ChangeSize(20, 200),
                        new Taunt(true, "Join us, heroes! The Void lusts for your souls!"),
                        new Shoot(99, 5, 30, 0)
                        ),
                    new State("3",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new Orbit(0.7, 20, 30, target: "LH Void Controller", orbitClockwise: true),
                        new Taunt(true, "Oryx will kneel before me!"),
                        new TimedTransition(2000, "4")
                        ),
                    new State("4",
                        new Suicide()
                        )
                    )
                )
            .Init("LH Void Entity W",
                new State(
                    new ScaleHP(0.3),
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new ChangeSize(250, 0),
                        new TimedTransition(5000, "2")
                        ),
                    new State("2",
                        new HpLessTransition(0.4, "3"),
                        new Orbit(0.7, 20, 30, target: "LH Void Controller", orbitClockwise: true),
                        new ChangeSize(20, 200),
                        new Taunt(true, "Join us, heroes! The Void lusts for your souls!"),
                        new Shoot(99, 5, 30, 1),
                        new HpLessTransition(0.4, "3")
                        ),
                    new State("3",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new Orbit(0.7, 20, 30, target: "LH Void Controller", orbitClockwise: true),
                        new Taunt(true, "You putrid pawns think you can defy fate? Laughable!"),
                        new TimedTransition(2000, "4")
                        ),
                    new State("4",
                        new Suicide()
                        )
                    )
                )
            .Init("LH Void Controller",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("1",
                        new ChangeSize(250, 0)
                        ),
                    new State("2",
                        new ApplySetpiece("VoidLiquid")
                        ),
                    new State("3",
                        new ApplySetpiece("VoidPlatform")
                        )
                    )
                )
            .Init("LH Void Spawner",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                        ),
                    new State("2",
                        new Reproduce("LH Void Fragment", 3, 50, coolDown: 15000)
                        )
                    )
                )
            .Init("LH Void Split",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                        ),
                    new State("2",
                        new ApplySetpiece("VoidLiquid")
                        ),
                    new State("3",
                        new ApplySetpiece("VoidPlatform")
                        )
                    )
                )
        .Init("LH Pot 1",
                new State(
                    new ScaleHP(0.1),
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Armored, true)
                        )
                    ),
                new Threshold(0.0001,
                    new ItemLoot("Onrane", 0.05)
                    )
                )
        .Init("LH Pot 2",
                new State(
                    new ScaleHP(0.1),
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Armored, true)
                        )
                    ),
                new Threshold(0.0001,
                    new ItemLoot("Onrane", 0.05)
                    )
                )
        .Init("LH Pot 3",
                new State(
                    new ScaleHP(0.1),
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Armored, true)
                        )
                    ),
                new Threshold(0.0001,
                    new ItemLoot("Onrane", 0.05)
                    )
                )
        .Init("LH Pot 4",
                new State(
                    new ScaleHP(0.1),
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Armored, true)
                        )
                    ),
                new Threshold(0.0001,
                    new ItemLoot("Onrane", 0.05)
                    )
                )
        .Init("LH Pot 5",
                new State(
                    new ScaleHP(0.1),
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Armored, true)
                        )
                    ),
                new Threshold(0.0001,
                    new ItemLoot("Onrane", 0.05)
                    )
                )
        .Init("LH Pot 6",
                new State(
                    new ScaleHP(0.1),
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Armored, true)
                        )
                    ),
                new Threshold(0.0001,
                    new ItemLoot("Onrane", 0.05)
                    )
                )
        .Init("LH Pot 7",
                new State(
                    new ScaleHP(0.1),
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Armored, true)
                        )
                    ),
                new Threshold(0.0001,
                    new ItemLoot("Onrane", 0.05)
                    )
                )
        .Init("LH Halls Colossus Pillar",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new TimedTransition(100, "2", true),
                        new TimedTransition(100, "3", true),
                        new TimedTransition(100, "4", true),
                        new TimedTransition(100, "5", true),
                        new TimedTransition(100, "6", true)
                        ),
                    new State("2",
                        new Shoot(99, 8, 45, 0, coolDown: 1200, coolDownOffset: 1000),
                        new TimedTransition(12000, "2", true),
                        new TimedTransition(12000, "3", true),
                        new TimedTransition(12000, "4", true),
                        new TimedTransition(12000, "5", true),
                        new TimedTransition(12000, "6", true)
                        ),
                    new State("3",
                        new Shoot(99, 8, 45, 0, coolDown: 1200, coolDownOffset: 1000),
                        new TimedTransition(12000, "2", true),
                        new TimedTransition(12000, "3", true),
                        new TimedTransition(12000, "4", true),
                        new TimedTransition(12000, "5", true),
                        new TimedTransition(12000, "6", true)
                        ),
                    new State("4",
                        new Shoot(99, 8, 45, 2, coolDown: 1200, coolDownOffset: 1000),
                        new TimedTransition(12000, "2", true),
                        new TimedTransition(12000, "3", true),
                        new TimedTransition(12000, "4", true),
                        new TimedTransition(12000, "5", true),
                        new TimedTransition(12000, "6", true)
                        ),
                    new State("5",
                        new Shoot(99, 8, 45, 7, coolDown: 1200, coolDownOffset: 1000),
                        new TimedTransition(12000, "2", true),
                        new TimedTransition(12000, "3", true),
                        new TimedTransition(12000, "4", true),
                        new TimedTransition(12000, "5", true),
                        new TimedTransition(12000, "6", true)
                        ),
                    new State("6",
                        new Shoot(99, 8, 45, 4, coolDown: 1200, coolDownOffset: 1000),
                        new TimedTransition(12000, "2", true),
                        new TimedTransition(12000, "3", true),
                        new TimedTransition(12000, "4", true),
                        new TimedTransition(12000, "5", true),
                        new TimedTransition(12000, "6", true)
                        )
                    )
                )
        .Init("LH Marble Core 1",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Orbit(0.7, 9, 20, "LH Marble Colossus")
                        )
                    )
                )
        .Init("LH Marble Core 2",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Orbit(0.7, 9, 20, "LH Marble Colossus")
                        )
                    )
                )
        .Init("LH Marble Core 3",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Orbit(0.7, 2, 20, "LH Marble Colossus")
                        )
                    )
                )
        .Init("LH Wall Spawner",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                        ),
                    new State("2",
                        new Spawn("LH Boss Wall", coolDown: 100000),
                        new Suicide()
                        )
                    )
                )
        .Init("LH Colossus Rock 1",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new Follow(0.7, 25, 1.5, 10000),
                        new PlayerWithinTransition(2, "2"),
                        new TimedTransition(4000, "2")
                        ),
                    new State("2",
                        new Flash(0xFF0000, 0.25, 2),
                        new SetAltTexture(1),
                        new TimedTransition(1000, "3")
                        ),
                    new State("3",
                        new Shoot(99, 8, null, 0, 45),
                        new Suicide()
                        )
                    )
                )
        .Init("LH Marble Colossus",
                new State(
                    new StayCloseToSpawn(1, 15),
                    new ScaleHP(0.3),
                    new DropPortalOnDeath("LH Void Portal", timeout: 120, YAdjustment: 3),
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new PlayerWithinTransition(10, "2")
                        ),
                    new State("2",
                        new SetAltTexture(1),
                        new TimedTransition(1500, "3")
                        ),
                    new State("3",
                        new SetAltTexture(2),
                        new TimedTransition(1500, "4")
                        ),
                    new State("4",
                        new SetAltTexture(3),
                        new TimedTransition(1500, "5")
                        ),
                    new State("5",
                        new SetAltTexture(4),
                        new TimedTransition(1500, "6")
                        ),
                    new State("6",
                        new SetAltTexture(0),
                        new Flash(0x00FF00, 1, 5),
                        new TimedTransition(5000, "7")
                        ),
                    new State("7",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                        new Taunt("Look upon my mighty bulwark."),
                        new OrderOnce(99, "LH Wall Spawner", "2"),
                        new InvisiToss("LH Halls Colossus Pillar", 12, 225, coolDown: 999999),
                        new InvisiToss("LH Halls Colossus Pillar", 12, 45, coolDown: 999999),
                        new InvisiToss("LH Halls Colossus Pillar", 12, 135, coolDown: 999999),
                        new InvisiToss("LH Halls Colossus Pillar", 12, 315, coolDown: 999999),
                        new Flash(0x00FF00, 1, 5),
                        new SpiralShoot(90, 10, 10, 36, 0, coolDown: 1500),
                        new HpLessTransition(0.95, "8")
                        ),
                    new State("8",
                        new Taunt("You doubt my strength? FATUUS! I will destroy you!"),
                        new Follow(1.3, 99, 2, 5000, 2000),
                        new Shoot(99, 6, 10, 1, coolDown: 1500),
                        new Shoot(99, 6, 60, 2, coolDown:2000),
                        new HpLessTransition(0.9, "9")
                        ),
                    new State("9",
                        new Taunt("I cast you off!"),
                        new ReturnToSpawn(1),
                        new Shoot(99, 3, 50, 5, coolDown: 1700),
                        new Reproduce("LH Colossus Rock 1", 10, 1, coolDown: 1500),
                        new Reproduce("LH Colossus Rock 1", 10, 1, coolDown: 1500),
                        new HpLessTransition(0.85, "10")
                        ),
                    new State("10",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new ReturnToSpawn(1),
                        new Reproduce("LH Marble Colossus Anchor", 10, 1, coolDown: 999999),
                        new Taunt("Your fervent attacks are no match for my strength! BEGONE!"),
                        new SpiralShoot(2, 180, 6, null, 3, 60, coolDown: 200),
                        new Shoot(99, 16, 22.5, 4, coolDown: 2000),
                        new Shoot(99, 3, 60, 5, coolDown: 1500),
                        new TimedTransition(10000, "11")
                        ),
                    new State("11",
                        new Taunt("INSOLENTIA! Darkness will consume you!"),
                        new Orbit(1.2, 10, 20, "LH Marble Colossus Anchor"),
                        new Shoot(99, 3, 40, 5, coolDown: 1500),
                        new Shoot(99, 4, 90, 6, coolDown: 2000),
                        new HpLessTransition(0.7, "12")
                        ),
                    new State("12",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new ReturnToSpawn(0.5),
                        new Taunt("Brace for your demise!"),
                        new Reproduce("LH Colossus Rock 1", 10, 1, coolDown: 1000),
                        new SpiralShoot(5, 72, 4, null, 7, 90, coolDown: 450),
                        new HpLessTransition(0.62, "13")
                        ),
                    new State("13",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("Futility!"),
                        new InvisiToss("LH Marble Core 1", 1, 90, coolDown: 999999),
                        new TimedTransition(2000, "14")
                        ),
                    new State("14",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("LH Marble Core 1", 99, "15"),
                        new Shoot(99, 30, 12, 9, coolDown: 1500),
                        new Shoot(99, 30, 12, 10, coolDown: 2500)
                        ),
                    new State("15",
                        new Taunt("Call of voice, for naught. Plea of mercy, for naught. None may enter this chamber and live!"),
                        new Follow(1, 30, 1, 7000, 2000),
                        new Wander(1),
                        new Shoot(99, 8, null, 11, fixedAngle: 45, coolDown: 2500),
                        new Shoot(99, 3, 50, 5, coolDown: 1000),
                        new HpLessTransition(0.51, "16")
                        ),
                    new State("16",
                        new Taunt("SANGUIS! OSSE! CARO! Feel it rend from your body!"),
                        new Shoot(99, 8, null, 11, fixedAngle: 45, coolDown: 2500),
                        new Shoot(99, 4, 90, 6, coolDown: 1500),
                        new Follow(1, 30, 1, 7000, 2000),
                        new Wander(1),
                        new HpLessTransition(0.42, "17")
                        ),
                    new State("17",
                        new Taunt("PESTIS! The darkness consumes!"),
                        new Shoot(99, 8, null, 11, fixedAngle: 45, coolDown: 2500),
                        new Shoot(99, 3, 50, 5, coolDown: 1500),
                        new Shoot(99, 4, 90, 6, coolDown: 1500),
                        new Follow(1, 30, 1, 7000, 2000),
                        new Wander(1),
                        new HpLessTransition(0.34, "18")
                        ),
                    new State("18",
                        new Taunt("Enough! Pillars, serve your purpose!"),
                        new ReturnToSpawn(1.2),
                        new Orbit(1.5, 10, 30, "LH Marble Colossus Anchor"),
                        new HpLessTransition(0.27, "19")
                        ),
                    new State("19",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt("..."),
                    new ReturnToSpawn(1.2),
                    new TimedTransition(2000, "20")
                        ),
                    new State("20",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("Perish, blights upon this realm!"),
                        new ReturnToSpawn(1.2),
                        new InvisiToss("LH Marble Core 2", 1, 90, coolDown: 999999),
                        new InvisiToss("LH Marble Core 3", 1, 90, coolDown: 999999),
                        new TimedTransition(2000, "21")
                        ),
                    new State("21",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Charge(2, 40, 6000),
                        new StayCloseToSpawn(1, 10),
                        new Shoot(99, 10, null, 14, 36, coolDown: 6000, coolDownOffset: 2400),
                        new Shoot(99, 10, null, 14, 18, coolDown: 6000, coolDownOffset: 2800),
                        new Shoot(99, 10, null, 14, 36, coolDown: 6000, coolDownOffset: 3200),
                        new Shoot(99, 10, null, 14, 18, coolDown: 6000, coolDownOffset: 3600),
                        new Shoot(99, 10, null, 14, 36, coolDown: 6000, coolDownOffset: 4000),
                        new EntitiesNotExistsTransition(99, "22", "LH Marble Core 3", "LH Marble Core 2")
                        ),
                    new State("22",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new ReturnToSpawn(1.2),
                        new Taunt("You have seen your last glimpse of sunlight!"),
                        new SpiralShoot(5, 72, 3, 120, 3, coolDown: 200, coolDownOffset: 300),
                        new Shoot(99, 3, 40, 5, coolDown: 1500, coolDownOffset: 300),
                        new Shoot(99, 16, 22.5, 4, coolDown: 2000, coolDownOffset: 300),
                        new HpLessTransition(0.19, "23")
                        ),
                    new State("23",
                        new Taunt("It is my duty to protect these catacombs! You dare threaten my purpose?"),
                        new SpiralShoot(99, 18, 4, 15, 15, coolDown: 750),
                        new SpiralShoot(99, 18, 4, 15, 15, fixedAngle: 90, coolDown: 750),
                        new SpiralShoot(99, 18, 4, 15, 15, fixedAngle: 180, coolDown: 750),
                        new SpiralShoot(99, 18, 4, 15, 15, fixedAngle: 270, coolDown: 750),
                        new Shoot(99, 3, 50, 5, coolDown: 2000),
                        new HpLessTransition(0.1, "24")
                        ),
                    new State("24",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Taunt("Magia saps from my body… My immense physical strength STILL REMAINS!"),
                        new Shoot(99, 7, 40, 5, coolDown: 1500),
                        new Shoot(99, 4, 90, 6, coolDown: 1300),
                        new HpLessTransition(0.08, "25")
                        ),
                    new State("25",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Taunt("Fear the halls!"),
                        new Shoot(99, 12, null, 10, fixedAngle: 30, coolDown: 1500),
                        new Shoot(99, 3, 50, 5, coolDown: 1500),
                        new HpLessTransition(0.05, "26")
                        ),
                    new State("26",
                        new Taunt("I... I am... Dying..."),
                        new Shoot(99, 12, null, 10, fixedAngle: 30, coolDown: 1200),
                        new Shoot(99, 3, 50, 5, coolDown: 1100),
                        new Reproduce("LH Colossus Rock 1", 10, 1, coolDown: 1000),
                        new HpLessTransition(0.02, "27")
                        ),
                    new State("27",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("You... YOU WILL COME WITH ME!"),
                        new SpiralShoot(2, 180, 6, null, 3, 60, coolDown: 200),
                        new Shoot(99, 16, 22.5, 4, coolDown: 2000),
                        new Shoot(99, 3, 60, 5, coolDown: 1500),
                        new TimedTransition(10000, "28")
                        ),
                    new State("28",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("I feel myself… Slipping… Into the void… It is so… Dark…"),
                        new Flash(0x82dfe5, 0.5, 20),
                        new TimedTransition(5000, "30")
                        ),
                    new State("30",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new RemoveObjectOnDeath("LH Halls Colossus Pillar", 99),
                        new Shoot(99, 36, null, 18, 10, coolDown: 9999),
                        new Shoot(99, 36, null, 17, 10, defaultAngle: 180, angleOffset: 0.3, coolDown: 9999),
                        new Shoot(99, 45, null, 16, 8, angleOffset: 0.6, coolDown: 9999),
                        new TimedTransition(100, "31")
                        ),
                    new State("31",
                        new Suicide()
                        )
                    ),
                new Threshold(0.01,
                    new ItemLoot("Vial Crate", 1),
                    new ItemLoot("Vial of Life", 0.02),
                    new ItemLoot("Vial of Mana", 0.02),
                    new ItemLoot("Vial of Attack", 0.02),
                    new ItemLoot("Vial of Speed", 0.02),
                    new ItemLoot("Vial of Defense", 0.02),
                    new ItemLoot("Vial of Dexterity", 0.02),
                    new ItemLoot("Vial of Vitality", 0.02),
                    new ItemLoot("Vial of Wisdom", 0.02),
                    new ItemLoot("Vial of Luck", 0.0125),
                    new ItemLoot("Vial of Restoration", 0.0125),
                    new ItemLoot("Helm of the Halls", 0.002),
                    new ItemLoot("Banner of Direction", 0.002),
                    new ItemLoot("Plate of the Colossus", 0.002),
                    new ItemLoot("Marble Pet Stone", 0.001)
                    )
                )
        .Init("LH Marble Defender",
                new State(
                    new ScaleHP(0.3),
                    new RemoveObjectOnDeath("LH Boss Wall", 7),
                    new RemoveObjectOnDeath("LH Marble Defender S", 20),
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new PlayerWithinTransition(7, "2", true)
                        ),
                    new State("2",                        
                        new Taunt("I wish only to protect you. Forgive me, but I have no other choice."),
                        new TimedTransition(3000, "3")
                        ),
                    new State("3",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                        //new Shoot(99, 16, 23, 0, coolDown: 200)
                        new SpiralShoot(2, 60, 16, 23, 0, coolDown: 250),
                        new TimedTransition(8000, "4")
                        ),
                    new State("4",
                        new SpiralShoot(-2, 60, 16, 23, 0, coolDown: 250),
                        new TimedTransition(8000, "3"),
                        new HpLessTransition(0.5, "5")
                        ),
                    new State("5",
                        new Taunt("Do not allow this evil to escape!"),
                        new Shoot(99, 23, null, 0, 8, coolDown: 750, coolDownOffset: 2000)
                        )
                    ),
                new Threshold(0.01,
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
                    new ItemLoot("Marble Battalion", 0.002)
                    )
                )
        .Init("LH Tormented Golem",
                new State(
                    new ScaleHP(0.3),
                    new State("1",
                        new PlayerWithinTransition(25, "2", true)
                        ),
                    new State("2",
                        new InvisiToss("LH Golem of Anger", 5, 310, 999999), 
                        new InvisiToss("LH Golem of Fear", 4, 70, 999999),
                        new InvisiToss("LH Golem of Fear", 4, 270, 999999),
                        new InvisiToss("LH Golem of Anger", 6, 10, 999999),
                        new InvisiToss("LH Golem of Sorrow", 3, 310, 999999),
                        new InvisiToss("LH Golem of Sorrow", 3, 10, 999999),
                        new Charge(2, 8, coolDown: 3000),
                        new Shoot(99, 18, 20, 0, coolDown: 1500, coolDownOffset: 500),
                        new StayCloseToSpawn(1, 10)
                        )
                    )
                )
        .Init("LH Grotto Blob",
                new State(
                    new ScaleHP(0.3),
                    new State("1",
                        new PlayerWithinTransition(25, "2", true)
                        ),
                    new State("2",
                        new InvisiToss("LH Grotto Slime", 4, 70, 999999),
                        new InvisiToss("LH Grotto Slime", 4, 270, 999999),
                        new InvisiToss("LH Grotto Rat", 3, 10, 999999),
                        new InvisiToss("LH Grotto Rat", 3, 310, 999999),
                        new InvisiToss("LH Grotto Bat", 3, 10, 999999),
                        new InvisiToss("LH Grotto Bat", 3, 310, 999999),
                        new TimedTransition(50, "3")
                        ),
                    new State("3",
                        new Reproduce("LH Grotto Slime", 20, 5, coolDown: 4500),
                        new Reproduce("LH Grotto Rat", 20, 3, coolDown: 4500),
                        new Reproduce("LH Grotto Bat", 20, 3, coolDown: 4500),
                        new SpiralShoot(10, 36, 3, 7, 0, 120, coolDown: 200),
                        new SpiralShoot(10, 36, 3, 7, 0, 240, coolDown: 200),
                        new SpiralShoot(10, 36, 3, 7, 0, 360, coolDown: 200)
                        )
                    )
                )
        .Init("LH Grotto Slime",
                new State(
                    new ScaleHP(0.1),
                    new State("1",
                        new Wander(0.8),
                        new StayBack(2, 2.5),
                        new Shoot(10, 1, null, 0, coolDown: 200)
                        )
                    )
                )
        .Init("LH Commander of the Crusade",
                new State(
                    new ScaleHP(0.3),
                    new State("1",
                        new InvisiToss("LH Crusade Soldier", 4, 40, 999999),
                        new InvisiToss("LH Crusade Soldier", 4, 80, 999999),
                        new InvisiToss("LH Crusade Soldier", 4, 360, 999999),
                        new InvisiToss("LH Crusade Soldier", 4, 320, 999999),
                        new InvisiToss("LH Crusade Shipwright", 2, 40, 999999),
                        new InvisiToss("LH Crusade Shipwright", 2, 80, 999999),
                        new InvisiToss("LH Crusade Shipwright", 2, 360, 999999),
                        new InvisiToss("LH Crusade Shipwright", 2, 320, 999999),
                        new InvisiToss("LH Crusade Explorer", 2, 360, 999999),
                        new InvisiToss("LH Crusade Explorer", 2, 320, 999999),
                        new InvisiToss("LH Crusade Explorer", 2, 180, 999999),
                        new TimedTransition(100, "2")
                        ),
                    new State("2",
                        new Follow(1, 12, 0, coolDown: 100),
                        new Shoot(12, 8, 12, 0, coolDown: 400),
                        new Reproduce("LH Crusade Soldier", 3, 2, coolDown: 10000),
                        new Reproduce("LH Crusade Soldier", 3, 2, coolDown: 10000),
                        new Reproduce("LH Crusade Soldier", 3, 2, coolDown: 10000),
                        new Reproduce("LH Crusade Shipwright", 3, 2, coolDown: 10000),
                        new Reproduce("LH Crusade Shipwright", 3, 2, coolDown: 10000),
                        new Reproduce("LH Crusade Shipwright", 3, 2, coolDown: 10000),
                        new Reproduce("LH Crusade Explorer", 3, 2, coolDown: 10000),
                        new Reproduce("LH Crusade Explorer", 3, 2, coolDown: 10000),
                        new Reproduce("LH Crusade Explorer", 3, 2, coolDown: 10000)
                        )
                    )
                )        
        .Init("LH Crusade Soldier",
                new State(
                    new ScaleHP(0.1),
                    new State("1",
                        new Wander(0.9),
                        new Follow(0.8, 12, 0, coolDown: 100),
                        new Shoot(12, 1, null, 0, coolDown: 750)
                        )
                    )
                )
        .Init("LH Champion of Oryx",
                new State(
                    new ScaleHP(0.3),
                    new State("1",
                        new Shoot(99, 15, null, 0, fixedAngle: 24, coolDown: 1000),
                        new InvisiToss("LH Oryx Armorbearer", 8, 90, 999999),
                        new InvisiToss("LH Oryx Armorbearer", 8, 360, 999999),
                        new InvisiToss("LH Oryx Armorbearer", 8, 180, 999999),
                        new InvisiToss("LH Oryx Swordsman", 4, 120, 999999),
                        new InvisiToss("LH Oryx Swordsman", 4, 300, 999999),
                        new InvisiToss("LH Oryx Swordsman", 4, 120, 999999),
                        new InvisiToss("LH Oryx Admiral", 4, 90, 999999),
                        new TimedTransition(100, "2")
                        ),
                    new State("2",
                        new Shoot(99, 15, null, 0, fixedAngle: 24, coolDown: 1000),
                        new Reproduce("LH Oryx Armorbearer", 8, 1, coolDown: 5000),
                        new Reproduce("LH Oryx Swordsman", 4, 1, coolDown: 6000)
                        )
                    )
                )
        .Init("LH Oryx Armorbearer",
                new State(
                    new ScaleHP(0.1),
                    new State("1",
                        new Orbit(1, 8, 10, "LH Champion of Oryx"),
                        new Shoot(99, 8, null, 0, fixedAngle: 60, coolDown: 1100),
                        new HpLessTransition(0.5, "2")

                        ),
                    new State("2",
                        new Flash(0xFF0000, 0.5, 2),
                        new Follow(0.8, 12, 0, coolDown: 100),
                        new Shoot(99, 9, 241, 0, coolDown: 1100)
                        )
                    )
                )
        .Init("LH Oryx Swordsman",
                new State(
                    new ScaleHP(0.1),
                    new State("1",
                        new Orbit(1, 4, 10, "LH Champion of Oryx"),
                        new Shoot(99, 4, null, 0, fixedAngle: 90, coolDown: 850),
                        new Shoot(99, 4, null, 0, fixedAngle: 45, coolDown: 1050),
                        new HpLessTransition(0.5, "2")
                        ),
                    new State("2",
                        new Flash(0xFF0000, 0.5, 2),
                        new Follow(0.8, 12, 0, coolDown: 100),
                        new Shoot(99, 4, null, 0, fixedAngle: 90, coolDown: 850),
                        new Shoot(99, 4, null, 0, fixedAngle: 45, coolDown: 1050)
                        )
                    )
                )
        .Init("LH Crusade Shipwright",
                new State(
                    new ScaleHP(0.1),
                    new State("1",
                        new Wander(0.9),
                        new Follow(0.8, 14, 0, coolDown: 100),
                        new Shoot(12, 1, null, 0, coolDown: 750)
                        )
                    )
                )
        .Init("LH Crusade Explorer",
                new State(
                    new ScaleHP(0.1),
                    new State("1",
                        new Wander(0.6),
                        new Shoot(12, 1, null, 0, coolDown: 750)
                        )
                    )
                )
        .Init("LH Agonized Titan",
                new State(
                    new ScaleHP(0.3),
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new PlayerWithinTransition(7, "2", true)
                        ),
                    new State("2",
                        new Taunt(true, "I..."),
                        new TimedTransition(10000, "3")
                        ),
                    new State("3",
                        new Taunt("I will not allow to bring calamity upon this world!"),
                        new TimedTransition(3000, "4")
                        ),
                    new State("4",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                        new SpiralShoot(12, 30, 10, 36, 0, coolDown: 400),
                        new Shoot(99, 12, 30, 2, coolDown:2000)
                        )
                    )
                )
        .Init("LH Golem of Anger",
                new State(
                    new ScaleHP(0.1),
                    new State("1",
                        new Wander(1.2),
                        new Follow(0.4, 2, 0, duration: 500, coolDown: 4000),
                        new Shoot(99, 1, null, 0, coolDown: 350),
                        new Shoot(99, 10, null, 1, 36, coolDown: 900)
                        )
                    )
                )
        .Init("LH Golem of Sorrow",
                new State(
                    new ScaleHP(0.1),
                    new State("1",
                        new Wander(1.4),
                        new StayBack(0.4, 5),
                        new Shoot(99, 12, 24, 1, coolDown: 1200),
                        new Shoot(99, 1, null, 0, coolDown: 300)
                        )
                    )
                )
        .Init("LH Golem of Fear",
                new State(
                    new ScaleHP(0.1),
                    new State("1",
                        new Wander(1.4),
                        new Shoot(99, 4, null, 0, fixedAngle: 90, coolDown: 2000),
                        new Shoot(99, 4, null, 1, fixedAngle: 90, angleOffset: 45, coolDown: 2000, coolDownOffset: 500)
                        )
                    )
                )
        .Init("LH Grotto Bat",
                new State(
                    new ScaleHP(0.1),
                    new State("1",
                        new Wander(0.7),
                        new Follow(1, 15, 0, 1000, coolDown: 200),
                        new Shoot(99, 1, null, 0, 90, coolDown: 2200)
                        )
                    )
                )
        .Init("LH Void Fragment",
                new State(
                    new ScaleHP(0.1),
                    new State("1",
                        new Wander(0.8),
                        new Shoot(8, 1, null, 0, coolDown: 1000),
                        new Shoot(99, 1, null, 5, coolDown: 1000),
                        new TimedTransition(10000, "2")
                        ),
                    new State("2",
                        new Flash(0xFF0000, 0.8, 2),
                        new TimedTransition(2000, "3")
                        ),
                    new State("3",
                        new Transform("LH Void Shade")
                        )
                    )
                )
        .Init("LH Void Shade",
                new State(
                    new ScaleHP(0.1),
                    new State("1",
                        new Follow(0.3, 12, 2, 1000, coolDown: 100),
                        new Shoot(8, 3, 12, 0, coolDown: 800),
                        new Shoot(99, 1, null, 5, coolDown: 1000),
                        new TimedTransition(10000, "2")
                        ),
                    new State("2",
                        new Flash(0xFF0000, 0.8, 2),
                        new TimedTransition(2000, "3")
                        ),
                    new State("3",
                        new Transform("LH Greater Void Shade")
                        )
                    )
                )
        .Init("LH Greater Void Shade",
                new State(
                    new ScaleHP(0.1),
                    new State("1",
                        new Follow(0.8, 12, 0, 1000, coolDown: 100),
                        new Shoot(99, 10, shootAngle: 36, projectileIndex: 1, coolDown: 1000),
                        new Shoot(9, 1, null, 0, coolDown: 500)
                        )
                    )
                )
        .Init("LH Marble Defender S",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new EntityNotExistsTransition("LH Marble Defender", 99, "2")
                        ),
                    new State("2",
                        new Shoot(99, 1, 180, coolDown: 300)
                        )
                    )
                )
        .Init("LH Grotto Rat",
                new State(
                    new ScaleHP(0.1),
                    new State("1",
                        new Wander(2.2),
                        new Follow(0.5, 10, 1, 1000, coolDown: 9999999),
                        new Shoot(99, 1, null, 0, 90, coolDown: 2200)
                        )
                    )
                )
            .Init("LH Spawn Pillar",
                new State(
                    new ScaleHP(0.1),
                    new RemoveObjectOnDeath("LH Boss Wall", 7),
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Armored, true)
                        )
                    )
                );
    }
}