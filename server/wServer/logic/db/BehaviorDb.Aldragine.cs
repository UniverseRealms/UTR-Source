using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;
using wServer.realm.cores;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Aldragine = () => Behav()

#region normal

    #region bosses

        #region Ganus the Sincryer
            .Init("AH The Sincryer",
                new State(
                    new DropPortalOnDeath("The Nontridus Portal", 100, timeout: 0),
                    new ScaleHP(0.3),
                    new ChangeGroundOnDeath(new[] {"Zol Aura"}, new[] {"Zol Aura Dormant"}, 999),
                    new HpLessTransition(0.14, "dead"),
                    new State("default",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new EntitiesNotExistsTransition(9999, "talk", "Stone of Zol")
                        ),
                    new State("talk",
                        new State("talk1",
                            new Taunt("Revealing the power of Zol can't be undone.", "The Zol is everywhere... You've released evil upon yourself."),
                            new TimedTransition(5000, "talk2")
                            ),
                        new State("talk2",
                            new Taunt("You have made a grave mistake."),
                            new TimedTransition(5000, "talk3")
                            ),
                        new State("talk3",
                            new Taunt("Now, this mistake will be your end."),
                            new TimedTransition(5000, "talk4")
                            ),
                        new State("talk4",
                            new Taunt("Perish."),
                            new TimedTransition(5000, "start")
                            )
                        ),

                    new State("fight",
                        new State("start",
                            new ConditionalEffect(ConditionEffectIndex.Invincible, duration: 0),
                            new Prioritize(
                                new Follow(0.8, 8, 1),
                                new Wander(1)
                                ),
                            new Shoot(8, count: 14, shootAngle: 20, projectileIndex: 4, coolDown: 600),
                            new Shoot(8, count: 3, shootAngle: 10, projectileIndex: 1, predictive: 2, coolDown: 1600),
                            new TimedTransition(9000, "f1")
                            ),
                        new State("f1",
                            new ConditionalEffect(ConditionEffectIndex.Armored),
                            new Prioritize(
                                new StayBack(0.4, 4),
                                new Swirl(1, 10)
                                ),
                            new Shoot(12, count: 6, shootAngle: 4, predictive: 1, projectileIndex: 0, coolDown: 400),
                            new Shoot(8, count: 10, projectileIndex: 2, coolDown: new Cooldown(2000, 1000)),
                            new TimedTransition(9000, "f2")
                            ),
                        new State("f2",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new ReturnToSpawn(1),
                            new TimedTransition(3000, "f3")
                            ),
                        new State("f3",
                            new Taunt("They fight with their lives!"),
                            new TossObject("Zol Bomber", 6, 0, count: 8, angleOffset: 45, coolDown: 9999999),
                            new TossObject("Zol Bomber", 9, 45, count: 4, angleOffset: 90, coolDown: 9999999),
                            new TimedTransition(3200, "f4")
                            ),
                        new State("f4",
                            new Shoot(10, count: 9, shootAngle: 8, projectileIndex: 3, coolDownOffset: 1100, angleOffset: 270, coolDown: 3000),
                            new Shoot(10, count: 9, shootAngle: 8, projectileIndex: 3, coolDownOffset: 1100, angleOffset: 90, coolDown: 3000),
                            new Shoot(12, count: 5, shootAngle: 12, projectileIndex: 4, coolDown: 1000),
                            new EntitiesNotExistsTransition(9999, "f5", "Zol Bomber")
                            ),
                        new State("f5",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 0),
                            new Taunt("Your life now belongs to me."),
                            new TimedTransition(4000, "f6")
                            ),
                        new State("f6",
                            new Flash(0x0F0F0F, 2, 2),
                            new Grenade(4, 300, coolDown: 3000),
                            new Shoot(12, count: 8, shootAngle: 4.5, predictive: 1, projectileIndex: 0, coolDown: 500),
                            new Prioritize(
                                new Charge(2, 10, coolDown: 4000),
                                new Wander(0.2)
                                ),
                            new Shoot(12, count: 12, projectileIndex: 2, coolDown: 4000),
                            new TimedTransition(12000, "f7")
                            ),
                        new State("f7",
                            new ConditionalEffect(ConditionEffectIndex.Armored),
                            new Prioritize(
                                new Follow(0.6),
                                new Wander(0.2)
                                ),
                            new Shoot(8, count: 4, projectileIndex: 4, coolDown: 400),
                            new Shoot(8, count: 6, projectileIndex: 4, coolDown: 1400),
                            new Shoot(12, count: 18, projectileIndex: 0, coolDown: 2500),
                            new TimedTransition(5000, "f8")
                            ),
                        new State("f8",
                            new Wander(0.4),
                            new Shoot(8, count: 10, projectileIndex: 4, coolDown: 1000),
                            new Shoot(12, count: 6, shootAngle: 8, projectileIndex: 2, coolDown: 2200),
                            new TossObject("Zol Slime", range: 4, coolDown: 2450, coolDownOffset: 100),
                            new TimedTransition(5000, "f9")
                            ),
                        new State("f9",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Taunt(true, "THE TIME IS NOW!", "A SERENADE FOR YOUR DOOM!"),
                            new Flash(0xFF0000, 1, 2),
                            new ReturnToSpawn(1),
                            new TimedTransition(4400, "f10")
                            ),
                        new State("f10",
                            new Taunt("Al lar kall zanus du era!", "Rul ah ka tera nol zan!"),
                            new Shoot(30, count: 34, projectileIndex: 0, coolDown: 1000),
                            new ReplaceTile("Zol Aura Dormant", "Zol Aura", 250),
                            new TimedTransition(8000, "f11")
                            ),
                        new State("f11",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 0),
                            new ReplaceTile("Zol Aura", "Zol Aura Dormant", 250),
                            new ConditionalEffect(ConditionEffectIndex.Armored),
                            new Prioritize(
                                new Follow(1.2),
                                new Wander(0.2)
                                ),
                            new TossObject("AH Sincryer Orb", range: 10, coolDown: 2500),
                            new Shoot(8, count: 4, projectileIndex: 4, coolDown: 400),
                            new Shoot(8, count: 6, projectileIndex: 4, coolDown: 1400),
                            new Shoot(12, count: 18, projectileIndex: 0, coolDown: 2500),
                            new TimedTransition(5000, "start")
                            )
                        ),

                    new State("dead",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new Taunt("YOU WILL NOT SURVIVE THE ONSLAUGHT OF THE ZOL!"),
                        new Flash(0x00FF00, 1, 3),
                        new ReturnToSpawn(1),
                        new TimedTransition(6000, "ded")
                        ),
                    new State("ded",
                        new Suicide()
                        )
                    ),
                new MostDamagers(3,
                    LootTemplates.Sor4Perc()
                    ),
                new Threshold(0.05,
                   new ItemLoot("fake", 0.010)
                   )
                )
        #endregion Ganus the Sincryer

        #region Nirux the Vision
            .Init("AH The Vision",
                new State(
                    new ScaleHP(0.3),
                    new DropPortalOnDeath("Core of the Hideout Portal", 100, timeout: 0),
                    new HpLessTransition(0.15, "dead"),
                    new State(
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new State("callout",
                            new Taunt(true, "He will fulfill the destiny in store. Come. Let me show you his vision.", "Even the ancients, the controllers, and Oryx himself fear us. Share that fear with them.", "Courage can only take you so far."),
                            new PlayerWithinTransition(5, "start")
                            ),
                        new State("start",
                            new Flash(0xFF0000, 2, 2),
                            new TimedTransition(4000, "talk")
                            ),
                        new State("talk",
                            new Taunt("They say the Zol was only a Myth...what fools....."),
                            new TimedTransition(6500, "spawn constructs")
                            )
                        ),
                    new State("spawn constructs",
                        new Taunt("You can't withstand the Zol."),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new TossObject("Construct of Zol", 6, 0, count: 2, angleOffset: 180, coolDown: 9999999),
                        new TimedTransition(2000, "wait for constructs")
                        ),
                    new State("wait for constructs",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(12, count: 3, shootAngle: 18, projectileIndex: 0, coolDown: 800),
                        new Shoot(11, count: 2, projectileIndex: 3, coolDown: 1600),
                        new SpiralShoot(90, 4, numShots: 5, shootAngle: 10, projectileIndex: 1, coolDown: 2400),
                        new SpiralShoot(90, 4, numShots: 5, shootAngle: 10, projectileIndex: 1, coolDown: 2400, coolDownOffset: 1200),
                        new EntitiesNotExistsTransition(999, "fight", "Construct of Zol")
                        ),
                    new State("fight",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Flash(0xFFFFFF, 1, 2),
                        new Shoot(8.4, count: 6, projectileIndex: 0, coolDown: 1200),
                        new TimedTransition(5000, "fight 2")
                        ),
                    new State("fight 2",
                        new Shoot(12, count: 1, predictive: 1.5, projectileIndex: 2, coolDown: 1000),
                        new Sequence(
                            new SpiralShoot(15, 4, numShots: 4, fixedAngle: 45, projectileIndex: 1, coolDown: 200),
                            new SpiralShoot(-15, 4, numShots: 4, fixedAngle: 90, projectileIndex: 1, coolDown: 200)
                            ),
                        new TimedTransition(8000, "fight 3")
                        ),
                    new State("fight 3",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("Your essence is mine!"),
                        new TossObject("Demon of the Dark", 9, 45, count: 4, angleOffset: 90, coolDown: 9999999),
                        new Flash(0xFFFFFF, 1, 2),
                        new TimedTransition(5000, "fight 4")
                        ),
                    new State("fight 4",
                        new Prioritize(
                            new StayCloseToSpawn(0.5, 3),
                            new Wander(0.05)
                            ),
                        new Shoot(11, count: 5, projectileIndex: 3, coolDown: 400),
                        new Shoot(11, count: 4, shootAngle: 12, projectileIndex: 4, coolDown: 1500),
                        new TimedTransition(8000, "fight 5")
                        ),
                    new State("fight 5",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Flash(0xFFFFFF, 1, 2),
                        new Shoot(8.4, count: 12, projectileIndex: 0, coolDown: 1200),
                        new TimedTransition(5000, "fight 6")
                        ),
                    new State("fight 6",
                        new Shoot(8.4, count: 6, projectileIndex: 0, coolDown: 1200),
                        new Shoot(8.4, count: 5, projectileIndex: 2, coolDown: 600),
                        new Shoot(8.4, count: 7, shootAngle: 10, projectileIndex: 1, coolDown: 1600),
                        new Shoot(8.4, count: 3, shootAngle: 2, projectileIndex: 2, coolDown: 1600, coolDownOffset: 500),
                        new TimedTransition(6400, "spawn constructs")
                        ),
                    new State("dead",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new Flash(0xF0FFF0, 1, 3),
                        new Taunt("....Have I not succeeded this time?", "Aaah..so you don't see it?", "Will the vision finally be broken?"),
                        new ReturnToSpawn(1),
                        new TimedTransition(6000, "ded")
                        ),
                    new State("ded",
                        new OrderOnce(20, "Nontridus Teleporter 1", "activate"),
                        new Suicide()
                        )
                    ),
                new MostDamagers(3,
                    LootTemplates.Sor5Perc()
                    ),
                new Threshold(0.05,
                    new TierLoot(7, ItemType.Ring, 0.003)
                    )
                )
        #endregion Nirux the Vision

        #region The Heart
            .Init("AH The Heart",
                new State(
                    new State("before bursts",
                        new State("idle",
                            new PlayerWithinTransition(6, "default")
                            ),
                        new State("default",
                            new Taunt("Only the worthy will survive...stand on the plates to survive the challenge!"),
                            new TimedTransition(5000, "begin")
                            ),
                        new Flash(0xFF00FF, 1, 2),
                        new State("begin",
                            new Taunt(true, "GANUS AND NIRUX WILL BE FORGIVEN. WILL YOU?"),
                            new TimedTransition(5000, "plate check")
                            ),
                        new State("plate check",
                            new AllyNotExistsTransition("Beacon of Zol B", 999, "ReadyBurst1", true)
                            )
                        ),

                    new State("bursts",
                        new State("ReadyBurst1",
                            new Taunt("DO YOU ACCEPT MY CHALLENGE?", "THE ECHOES OF ZOL HAVE SEEN YOU FIGHT. ENGAGE IN THE TEST?"),
                            new PlayerTextTransition("CommenceBurst1", "Unleash the Power of the Zol!", 99, false, true)
                            ),
                        new State("CommenceBurst1",
                            new Taunt("Such charisma would get you killed here. Let it begin.", "Have never seen anyone so excited to be crushed...", "It would be unlikely you'll live to tell the tale.", "You seem brave now..."),
                            new TimedTransition(6000, "Burst1")
                            ),
                        new State("Burst1",
                            new Spawn("AH The Heart Logic", 1, 1, 9999999),
                            new Shoot(20, count: 8, projectileIndex: 0, coolDown: 8000),
                            new InvisiToss("Zol Sorcerer", 5, 45, count: 4, angleOffset: 90, coolDown: 9999999),
                            new InvisiToss("Giant Cube of Zol", 10, 20, count: 4, angleOffset: 90, coolDown: 9999999, coolDownOffset: 4000),
                            new InvisiToss("Corrupted Golem B", 8, 0, count: 4, angleOffset: 90, coolDown: 9999999, coolDownOffset: 1400),
                            new InvisiToss("Demon of the Dark", 6.5, 0, count: 4, angleOffset: 90, coolDown: 9999999, coolDownOffset: 4000),
                            new TimedTransition(6000, "AfterBurst1")
                            ),
                        new State("AfterBurst1",
                            new Shoot(20, count: 8, projectileIndex: 0, coolDown: 8000),
                            new GroupNotExistTransition(999, "ReadyBurst2", "Zol Minions")
                            ),

                        new State("ReadyBurst2",
                            new Flash(0x0000FF, 1, 1),
                            new Taunt(true, "Burst 1 has been defeated. Stay on your plates warriors... there is more to come."),
                            new TimedTransition(1500, "SayReadyBurst2")
                            ),
                        new State("SayReadyBurst2",
                            new Taunt(true, "Say 'READY' when you wish to begin the next Burst."),
                            new PlayerTextTransition("CommenceBurst2", "ready")
                            ),
                        new State("CommenceBurst2",
                            new TimedTransition(2000, "Burst2")
                            ),
                        new State("Burst2",
                            new Shoot(20, count: 16, projectileIndex: 0, coolDown: 8000),
                            new InvisiToss("Zol Sorcerer", 5, 45, count: 4, angleOffset: 90, coolDown: 9999999),
                            new InvisiToss("Corrupted Golem B", 8, 0, count: 4, angleOffset: 90, coolDown: 9000, coolDownOffset: 2600),
                            new InvisiToss("Cleric of Zol", 7, 10, count: 4, angleOffset: 90, coolDown: 9999999, coolDownOffset: 2600),
                            new InvisiToss("Cleric of Zol", 7, 80, count: 4, angleOffset: 90, coolDown: 9999999, coolDownOffset: 12600),
                            new InvisiToss("Zol Slime", 10, 0, count: 4, angleOffset: 90, coolDown: 9999999, coolDownOffset: 1000),
                            new InvisiToss("Zol Slime", 11, 15, count: 4, angleOffset: 90, coolDown: 9999999, coolDownOffset: 13000),
                            new InvisiToss("Zol Slime", 11, 75, count: 4, angleOffset: 90, coolDown: 9999999, coolDownOffset: 13000),
                            new TimedTransition(13500, "AfterBurst2")
                            ),
                        new State("AfterBurst2",
                            new Shoot(20, count: 16, projectileIndex: 0, coolDown: 8000),
                            new GroupNotExistTransition(999, "ReadyBurst3", "Zol Minions")
                            ),

                        new State("ReadyBurst3",
                            new Flash(0x0000FF, 1, 1),
                            new Taunt(true, "Burst 2 has been defeated. Stay on your plates warriors... there is more to come."),
                            new PlayerTextTransition("CommenceBurst3", "ready")
                            ),
                        new State("CommenceBurst3",
                            new TimedTransition(2000, "Burst3")
                            ),
                        new State("Burst3",
                            new Shoot(20, count: 22, projectileIndex: 0, coolDown: 8000),
                            new InvisiToss("Zol Sorcerer", 5, 45, count: 4, angleOffset: 90, coolDown: 9999999),
                            new InvisiToss("Corrupted Golem B", 8, 0, count: 4, angleOffset: 90, coolDown: 9000, coolDownOffset: 2600),
                            new InvisiToss("Demon of the Dark", 7, 0, count: 4, angleOffset: 90, coolDown: 16000, coolDownOffset: 2600),
                            new InvisiToss("Niolru", 8, 0, count: 4, angleOffset: 90, coolDown: 9999999, coolDownOffset: 11000),
                            new TimedTransition(22500, "AfterBurst3")
                            ),
                        new State("AfterBurst3",
                            new Shoot(20, count: 22, projectileIndex: 0, coolDown: 8000),
                            new GroupNotExistTransition(999, "ReadyBurst4", "Zol Minions")
                            ),

                        new State("ReadyBurst4",
                            new Flash(0x0000FF, 1, 1),
                            new Taunt(true, "Burst 3 has been defeated. Final Burst is near... you must be prepared!"),
                            new PlayerTextTransition("CommenceBurst4", "ready")
                            ),
                        new State("CommenceBurst4",
                            new TimedTransition(2000, "Burst4")
                            ),
                        new State("Burst4",
                            new Shoot(20, count: 22, projectileIndex: 0, coolDown: 8000),
                            new InvisiToss("Zol Sorcerer", 5, 45, count: 4, angleOffset: 90, coolDown: 9999999),
                            new InvisiToss("Corrupted Golem B", 8, 0, count: 4, angleOffset: 90, coolDown: 9000, coolDownOffset: 2600),
                            new InvisiToss("Demon of the Dark", 8, 0, count: 4, angleOffset: 90, coolDown: 10000, coolDownOffset: 7600),
                            new InvisiToss("Giant Cube of Zol", 6, 15, count: 4, angleOffset: 90, coolDown: 20000, coolDownOffset: 7600),
                            new InvisiToss("Giant Cube of Zol", 6, 75, count: 4, angleOffset: 90, coolDown: 9999999, coolDownOffset: 17600),
                            new InvisiToss("Niolru", 11, 15, coolDown: 9999999, coolDownOffset: 25000),
                            new TimedTransition(31500, "AfterBurst4")
                            ),
                        new State("AfterBurst4",
                            new Shoot(20, count: 22, projectileIndex: 0, coolDown: 8000),
                            new GroupNotExistTransition(999, "Done", "Zol Minions")
                            )
                        ),

                    new State("Done",
                        new State(
                            new Flash(0xFFFFFF, 1, 3),
                            new Order(999, "AH The Heart Logic", "success"),
                            new Taunt("You have shown that you can withstand us... but can you resist the FULL power of the Zol?", "It must have taken preparation to defeat us. I applaud you.", "Congratulations! You have passed the final test to get to Aldragine."),
                            new TimedTransition(6000, "Success")
                            ),
                        new State("Success",
                            new DropPortalOnDeath("Keeping of Aldragine Portal", 1, timeout: 0),
                            new Suicide()
                            )
                        ),
                    
                    new State("Failed",
                        new State(
                            new Taunt(true, "THERE IS NO REMORSE. YOU ARE BANISHED.", "YOU HAVE NOT BEEN FORGIVEN. REIGN.", "WE DO NOT FORGIVE. YOU DO NOT ADVANCE."),
                            new Flash(0xFF0000, 0.25, 3),
                            new TimedTransition(1500, "Failure")
                            ),
                        new State("Failure",
                            new ClearLoot(),
                            new State("fail g1",
                                new ReplaceTile("Zol Aura Dormant", "Zol Aura", 999),
                                new ChangeGround( new[] {"Algadrine Tile 3", "Algadrine Tile 4"}, new[] {"Zol Aura Dormant", "Algadrine Tile 3", "Algadrine Tile 4"}, 999),
                                new ReplaceTile("Algadrine Tile Slimey", "Zol Aura Dormant", 999),
                                new TimedTransition(1100, "fail g2")
                                ),
                            new State("fail g2",
                                new ReplaceTile("Zol Aura Dormant", "Zol Aura", 999),
                                new ChangeGround( new[] {"Algadrine Tile 3", "Algadrine Tile 4"}, new[] {"Zol Aura Dormant"}, 999),
                                new ChangeGround( new[] {"Algadrine Tile 1", "Algadrine Tile 2"}, new[] {"Zol Aura Dormant", "Algadrine Tile 1", "Algadrine Tile 2"}, 999),
                                new ReplaceTile("Algadrine Tile Heart", "Zol Aura Dormant", 999),
                                new TimedTransition(1400, "fail g3")
                                ),
                            new State("fail g3",
                                new ReplaceTile("Zol Aura Dormant", "Zol Aura", 999),
                                new ChangeGround( new[] {"Algadrine Tile 1", "Algadrine Tile 2"}, new[] {"Zol Aura Dormant"}, 999),
                                new TimedTransition(1100, "fail g4")
                                ),
                            new State("fail g4",
                                new ReplaceTile("Zol Aura Dormant", "Zol Aura", 999),
                                new TimedTransition(1000, "fail death")
                                ),
                            new State("fail death",
                                new Suicide()
                                )
                            )
                        )
                    ),
                new MostDamagers(3,
                    LootTemplates.Sor5Perc()
                    ),
                new Threshold(0.05,
                    new TierLoot(7, ItemType.Ring, 0.003)
                    )
                )
        #endregion The Heart
        
        #region Aldragine
            .Init("AH Aldragine",
                new State(
                    new ScaleHP(0.3),
                    new State("pre-fight",
                        new State("pf default",
                            new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                            new PlayerWithinTransition(4, "pf intimidation")
                            ),
                        new State("pf intimidation",
                            new ChangeSize(20, 220),
                            new TimedTransition(4000, "pf talk 1")
                            ),
                        new State("pf talk 1",
                            new Taunt("I'd assume it took you quite long to get here. Only to die."),
                            new TimedTransition(4000, "pf talk 2")
                            ),
                        new State("pf talk 2",
                            new Taunt("You've been given multiple warnings, but you fail to see that you WON'T MAKE ME FALL LIKE THE OTHERS.", "Your poor comprehension of true power leads me to think you are genuinely stupid."),
                            new TimedTransition(4000, "pf talk 3")
                            ),
                        new State("pf talk 3",
                            new Flash(0x00FF00, 1, 1),
                            new Taunt("Now, I'll show you the true meaning of power. Come closer. Let me show you."),
                            new TimedTransition(1000, "pf final")
                            ),
                        new State("pf final",
                            new TimedTransition(20000, "pf talk 3"),
                            new PlayerWithinTransition(2, "fight 1")
                            )
                        ),

                    new State("fight",
                        new State("fight 1",
                            new State("f1 flash",
                                new Flash(0xFF0000, 1, 2),
                                new TimedTransition(3000, "f1 fight")
                                ),
                            new State("f1 fight",
                                new ConditionalEffect(ConditionEffectIndex.Invincible, duration: 0),
                                new Prioritize(
                                     new StayCloseToSpawn(0.5, 3),
                                     new Wander(0.05)
                                     ),
                                new Shoot(8, count: 7, projectileIndex: 2, coolDown: 3000),
                                new Shoot(8, count: 4, projectileIndex: 3, shootAngle: 90, fixedAngle: 0, coolDown: 400, coolDownOffset: 2000),
                                new Shoot(8, count: 1, projectileIndex: 4, coolDown: new Cooldown(2200, 500)),
                                new Shoot(8, count: 4, shootAngle: 10, projectileIndex: 2, predictive: 1, coolDown: new Cooldown(1300, 400)),
                                new HpLessTransition(0.8, "fight 2")
                                )
                            ),
                        
                        new State("fight 2",
                            new State("f2 return",
                                new ReturnToSpawn(1),
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                                new TimedTransition(1500, "f2 talk")
                                ),
                            new State("f2 talk",
                                new Taunt("Not even a scratch. Pitiful."),
                                new TimedTransition(2500, "f2 fight")
                                ),
                            new State("f2 fight",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 0),
                                new Prioritize(
                                     new StayCloseToSpawn(0.5, 3),
                                     new Wander(0.05)
                                     ),
                                new Shoot(8, count: 11, projectileIndex: 2, coolDown: 3000),
                                new Shoot(8, count: 4, projectileIndex: 3, shootAngle: 90, fixedAngle: 0, coolDown: 400, coolDownOffset: 2000),
                                new Shoot(8, count: 1, projectileIndex: 4, coolDown: new Cooldown(2200, 500)),
                                new Shoot(8, count: 5, shootAngle: 8, projectileIndex: 0, predictive: 1, coolDown: new Cooldown(1200, 200)),
                                new HpLessTransition(0.6, "wall")
                                )
                            ),

                        new State("wall",
                            new State("w delay",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                                new ReturnToSpawn(1),
                                new TimedTransition(1000, "w talk")
                                ),
                            new State("w talk",
                                new Taunt("You are nothing to me but another small issue to fix."),
                                new ReturnToSpawn(1),
                                new Flash(0x0000FF, 1, 3),
                                new TimedTransition(2000, "w go left")
                                ),
                            new State("w go left",
                                new MoveTo(speed: 1, x: 2, y: 14),
                                new TimedTransition(3000, "w spawn minions")
                                ),
                            new State("w spawn minions",
                                new InvisiToss("Zol Sorcerer", 13, 10, count: 2, angleOffset: 20, coolDown: 9999999),
                                new InvisiToss("Zol Bomber", 11, 10, count: 2, angleOffset: 20, coolDown: 9999999),
                                new InvisiToss("Niolru", 9, 10, count: 2, angleOffset: 30, coolDown: 9999999),
                                new InvisiToss("Demon of the Dark", 7, 10, count: 2, angleOffset: 20, coolDown: 9999999),
                                new InvisiToss("Giant Cube of Zol", 4, 20, count: 2, angleOffset: 40, coolDown: 9999999),
                                new InvisiToss("Haunting Knight", range: 14, angle: 0, count: 5, angleOffset: 15, coolDown: 99999, coolDownOffset: 1800),
                                new TimedTransition(1900, "w wait")
                                ),
                            new State("w wait",
                                new Shoot(8, count: 16, projectileIndex: 4, fixedAngle: 0, coolDown: 5000),
                                new GroupNotExistTransition(50, "w center", "Zol Minions")
                                ),
                            new State("w center",
                                new ReturnToSpawn(1, 0.5),
                                new TimedTransition(3000, "fight 3")
                                )
                            ),

                        new State("fight 3",
                            new State("f3 fight",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 0),
                                new Shoot(15, count: 20, projectileIndex: 5, coolDown: 2500),
                                new Shoot(10, count: 4, shootAngle: 6, projectileIndex: 6, coolDown: 800),
                                new Shoot(10, count: 5, shootAngle: 6, projectileIndex: 2, coolDown: 700),
                                new Shoot(10, count: 2, shootAngle: 15, projectileIndex: 3, coolDown: 2500),
                                new HpLessTransition(0.4, "survival")
                                )
                            ),

                        new State("survival",
                            new State("su delay",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                                new TimedTransition(1000, "su talk")
                                ),
                            new State("su talk",
                                new Taunt("Our heart's bursts were only a Trial to your true challenge. Let's see if you can handle this!"),
                                new TimedTransition(3000, "su throw stones")
                                ),
                            new State("su throw stones",
                                new TossObject("Stone of Zol", 12.53, count: 2, angleOffset: 57.22, angle: 90, coolDown: 9999999, coolDownOffset: 1000),
                                new TossObject("Stone of Zol", 11.66, count: 2, angleOffset: 61.92, angle: 270, coolDown: 9999999, coolDownOffset: 1000),
                                new EntityExistsTransition("Stone of Zol", 20, "su golems")
                                ),
                            new State("su golems",
                                new SpiralShoot(1.5, 120, 2, projectileIndex: 4, coolDown: 200),
                                new OrderOnce(30, "Stone of Zol", "spawn random normal"),
                                new State("su g spawn1",
                                    new TossObject("Corrupted Golem C", 3.5, angle: 270, coolDown: 999999, coolDownOffset: 1000),
                                    new EntityExistsTransition("Corrupted Golem C", 20, "su g wait1")
                                    ),
                                new State("su g wait1",
                                    new EntitiesNotExistsTransition(30, "su g spawn2", "Corrupted Golem C", "Headless Corrupted Golem C")
                                    ),
                                new State("su g spawn2",
                                    new TossObject("Corrupted Golem C", 3.5, angle: 90, count: 2, angleOffset: 180, coolDown: 999999, coolDownOffset: 1000),
                                    new EntityExistsTransition("Corrupted Golem C", 20, "su g wait2")
                                    ),
                                new State("su g wait2",
                                    new EntitiesNotExistsTransition(30, "su g spawn3", "Corrupted Golem C", "Headless Corrupted Golem C")
                                    ),
                                new State("su g spawn3",
                                    new TossObject("Corrupted Golem C", 3.5, angle: 90, count: 3, angleOffset: 120, coolDown: 999999, coolDownOffset: 1000),
                                    new EntityExistsTransition("Corrupted Golem C", 20, "su g wait3")
                                    ),
                                new State("su g wait3",
                                    new EntitiesNotExistsTransition(30, "su g spawn4", "Corrupted Golem C", "Headless Corrupted Golem C")
                                    ),
                                new State("su g spawn4",
                                    new TossObject("Corrupted Golem C", 3.5, angle: 0, count: 4, angleOffset: 90, coolDown: 999999, coolDownOffset: 1000),
                                    new EntityExistsTransition("Corrupted Golem C", 20, "su g wait4")
                                    ),
                                new State("su g wait4",
                                    new EntitiesNotExistsTransition(30, "fight 4", "Corrupted Golem C", "Headless Corrupted Golem C")
                                    )
                                )
                            ),

                        new State("fight 4",
                            new State("f4 delay",
                                new OrderOnce(30, "Stone of Zol", "invulnerable"),
                                new RemoveGroup(30, "Zol Minions"),
                                new TimedTransition(800, "f4 flash")
                                ),
                            new State("f4 flash",
                                new Flash(0xFF0000, 1, 3),
                                new TimedTransition(3000, "f4 fight")
                                ),
                            new State("f4 fight",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 0),
                                new Prioritize(
                                     new StayCloseToSpawn(0.5, 3),
                                     new Wander(0.05)
                                     ),
                                new SpiralShoot(30, 2, numShots: 6, projectileIndex: 4, coolDown: 4000),
                                new Shoot(10, count: 3, shootAngle: 10, projectileIndex: 8, coolDown: 1000),
                                new HpLessTransition(0.2, "stones")
                                )
                            ),

                        new State("stones",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new State("st talk",
                                new OrderOnce(30, "Stone of Zol", "invulnerable"),
                                new ReturnToSpawn(1, 0.5),
                                new Taunt("Is that all you've got?", "Hahaha!"),
                                new TimedTransition(2500, "st start")
                                ),
                            new State("st start",
                                new Flash(0x0F00F0, 1, 3),
                                new TossObject("Zol Bomber", 6, 0, count: 8, angleOffset: 45, coolDown: 9999999, coolDownOffset: 2000),
                                new TimedTransition(3000, "st wait")
                                ),
                            new State("st wait",
                                new OrderOnce(30, "Stone of Zol", "vulnerable"),
                                new Taunt("Your time is ticking, warriors!", "Unstoppable."),
                                new SpiralShoot(36, 2, numShots: 5, projectileIndex: 3, coolDown: 1500, delayAfterComplete: 3500),
                                new Shoot(20, count: 3, shootAngle: 6, projectileIndex: 6, coolDown: 500),
                                new Shoot(20, count: 6, shootAngle: 6, projectileIndex: 7, predictive: 1, coolDown: 500, coolDownOffset: 1000),
                                new EntitiesNotExistsTransition(30, "final", "Stone of Zol"),
                                new TimedTransition(75000, "Failed")
                                )
                            ),

                        new State("final",
                            new State("fin talk 1",
                                new State("fin t1 1",
                                    new Taunt("Your lacking skills have somehow managed to survive me. Perhaps it's luck."),
                                    new TimedTransition(2000, "fin t1 2")
                                    ),
                                new State("fin t1 2",
                                    new Grenade(0, 0, range: 4, fixedAngle: 270, coolDown: 999999, color: 0x00FF00),
                                    new TimedTransition(1200, "fin t1 3")
                                    ),
                                new State("fin t1 3",
                                    new OrderOnce(20, "Hideout Destruction Mechanism Spawner", "activate"),
                                    new TimedTransition(1800, "fin talk 2")
                                    )
                                ),
                            new State("fin shoot",
                                new Shoot(20, count: 1, projectileIndex: 4, fixedAngle: 90, coolDown: 9999999, coolDownOffset: 1500),
                                new Shoot(20, count: 3, shootAngle: 10, projectileIndex: 4, fixedAngle: 90, coolDown: 9999999, coolDownOffset: 2000),
                                new Shoot(20, count: 6, shootAngle: 10, projectileIndex: 4, fixedAngle: 90, coolDown: 9999999, coolDownOffset: 2500),
                                new Shoot(20, count: 10, shootAngle: 10, projectileIndex: 4, fixedAngle: 90, coolDown: 9999999, coolDownOffset: 3000),
                                new Shoot(20, count: 16, shootAngle: 10, projectileIndex: 4, fixedAngle: 90, coolDown: 9999999, coolDownOffset: 3500),
                                new Shoot(20, count: 23, shootAngle: 10, projectileIndex: 4, fixedAngle: 90, coolDown: 9999999, coolDownOffset: 4000),
                                new Shoot(20, count: 31, shootAngle: 10, projectileIndex: 4, fixedAngle: 90, coolDown: 9999999, coolDownOffset: 4500),
                                new State("fin talk 2",
                                    new Taunt("However, nothing is capable of surviving the pure energy of the Zol."),
                                    new TimedTransition(4000, "fin talk 3")
                                    ),
                                new State("fin talk 3",
                                    new Flash(0x00ff00, 1, 2),
                                    new Taunt("PERISH!"),
                                    new TimedTransition(2000, "fin survival")
                                    )
                                ),
                            new State("fin survival",
                                new OrderOnce(50, "Hideout Destruction Mechanism Active", "spawn normal"),
                                new Shoot(20, count: 32, shootAngle: 10, projectileIndex: 4, fixedAngle: 90, coolDown: 2800),
                                new EntityNotExistsTransition("Hideout Destruction Mechanism Active", 50, "Failed")
                                )
                            )
                        ),
                        
                    new State("Done",
                        new Flash(0xFFFFFF, 1, 3),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new State("done delay",
                            new TimedTransition(2000, "done taunt1")
                            ),
                        new State("done taunt1",
                            new Taunt(true, "Awaited the day for me to be defeated..."),
                            new TimedTransition(3000, "done taunt2")
                            ),
                        new State("done taunt2",
                            new Taunt(true, "We'll meet again, warriors..."),
                            new TimedTransition(3000, "done success")
                            ),
                        new State("done success",
                            new AnnounceOnDeath("The Zol, a dark burden, seems to fade away slowly..."),
                            new Shoot(8, count: 10, projectileIndex: 2, coolDown: 9999),
                            new Suicide()
                            )
                        ),

                    new State("Failed",
                        new Flash(0xFF0000, 1, 3),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new Taunt(true, "I HAVE GAINED ENOUGH ZOL. THE WORLDS WILL SOON BE OURS..."),
                        new Order(50, "Hideout Destruction Mechanism Active", "suicide"),
                        new Order(50, "Hideout Destruction Mechanism Inactive", "suicide"),
                        new ReturnToSpawn(1),
                        new TimedTransition(2000, "Failure")
                        ),
                    new State("Failure",
                        new ClearLoot(),
                        new OrderOnce(30, "Stone of Zol", "invulnerable"),
                        new State(
                            new SpiralShoot(6, 2, numShots: 30, projectileIndex: 4, coolDownOffset: 2000, coolDown: 3000),
                            new State("fail g1",
                                new ReplaceTile("Zol Aura Dormant", "Zol Aura", 999),
                                new ChangeGround( new[] {"Algadrine Tile 1", "Algadrine Tile 2"}, new[] {"Zol Aura Dormant", "Algadrine Tile 1", "Algadrine Tile 2"}, 999),
                                new ReplaceTile("Algadrine Tile Slimey", "Zol Aura Dormant", 999),
                                new TimedTransition(1100, "fail g2")
                                ),
                            new State("fail g2",
                                new ReplaceTile("Zol Aura Dormant", "Zol Aura", 999),
                                new ChangeGround( new[] {"Algadrine Tile 1", "Algadrine Tile 2"}, new[] {"Zol Aura Dormant"}, 999),
                                new ChangeGround( new[] {"Algadrine Tile 3", "Algadrine Tile 4"}, new[] {"Zol Aura Dormant", "Algadrine Tile 3", "Algadrine Tile 4"}, 999),
                                new TimedTransition(1400, "fail g3")
                                ),
                            new State("fail g3",
                                new ReplaceTile("Zol Aura Dormant", "Zol Aura", 999),
                                new ChangeGround( new[] {"Algadrine Tile 3", "Algadrine Tile 4"}, new[] {"Zol Aura Dormant"}, 999),
                                new TimedTransition(1100, "fail g4")
                                ),
                            new State("fail g4",
                                new ReplaceTile("Zol Aura Dormant", "Zol Aura", 999),
                                new TimedTransition(1500, "fail death")
                                )
                            ),
                        new State("fail death",
                            new Flash(0x00FF00, 4.8, 1),
                            new ChangeSize(-15, 0),
                            new TimedTransition(3000, "fail suicide")
                            ),
                        new State("fail suicide",
                            new ClearLoot(),
                            new Suicide()
                            )
                        )
                    ),
                new MostDamagers(3,
                    LootTemplates.Sor5Perc()
                    )
                )
        #endregion Aldragine

    #endregion bosses

    #region other
            .Init("AH Sincryers Gate Opener",
                new State(
                    new DropPortalOnDeath("Sincryer's Gate Portal", 100, timeout: 0),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("idle",
                        new EntitiesNotExistsTransition(9999, "activate", "Portal to Eternity")
                        ),
                    new State("activate",
                        new Suicide()
                        )
                    )
                )
    #endregion other

#endregion normal

#region ultra

    #region bosses

        #region Ultra Ganus the Sincryer
            .Init("AHU The Sincryer",
                new State(
                    new DropPortalOnDeath("Ultra The Nontridus Portal", 100, timeout: 0),
                    new ScaleHP(0.3),
                    new ChangeGroundOnDeath(new[] {"Zol Aura"}, new[] {"Zol Aura Dormant"}, 999),
                    new HpLessTransition(0.14, "dead"),
                    new State("default",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new EntitiesNotExistsTransition(9999, "talk1", "Stone of Zol")
                        ),
                    new State("talk",
                        new State("talk1",
                            new Taunt("The Zol grows stronger night by night... Can you not see your failure already?"),
                            new TimedTransition(5000, "talk2")
                            ),
                        new State("talk2",
                            new Taunt("You have once again made a grave mistake."),
                            new TimedTransition(5000, "talk3")
                            ),
                        new State("talk3",
                            new Taunt("Now, this mistake will be your end."),
                            new TimedTransition(5000, "talk4")
                            ),
                        new State("talk4",
                            new Taunt("PERISH!"),
                            new TimedTransition(5000, "fight")
                            )
                        ),

                    new State("fight",
                        new Shoot(12, count: 1, projectileIndex: 5, coolDown: 6000),
                        new Shoot(12, count: 2, shootAngle: 8, projectileIndex: 5, coolDown: 6000, coolDownOffset: 1000),
                        new Shoot(12, count: 3, shootAngle: 10, projectileIndex: 5, coolDown: 6000, coolDownOffset: 2000),
                        new State("f1",
                            new ConditionalEffect(ConditionEffectIndex.Invincible, duration: 0),
                            new Prioritize(
                                new Follow(0.8, 8, 1),
                                new Wander(1)
                                ),
                            new Shoot(8, count: 14, shootAngle: 20, projectileIndex: 4, coolDown: 600),
                            new Shoot(8, count: 3, shootAngle: 10, projectileIndex: 1, predictive: 2, coolDown: 1600),
                            new TimedTransition(9000, "f2")
                            ),
                        new State("f2",
                            new ConditionalEffect(ConditionEffectIndex.Armored),
                            new Prioritize(
                                new StayBack(0.4, 4),
                                new Swirl(1, 10)
                                ),
                            new Shoot(12, count: 6, shootAngle: 4, predictive: 1, projectileIndex: 0, coolDown: 500),
                            new Shoot(8, count: 10, projectileIndex: 2, coolDown: new Cooldown(2000, 1000)),
                            new TimedTransition(9000, "f3")
                            ),
                        new State("f3",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new ReturnToSpawn(1),
                            new TimedTransition(3000, "f4")
                            ),
                        new State("f4",
                            new Taunt("They fight with their lives!"),
                            new TossObject("Zol Bomber", 6, 22.5, count: 8, angleOffset: 45, coolDown: 9999999),
                            new TossObject("Zol Bomber", 9, 45, count: 4, angleOffset: 90, coolDown: 9999999),
                            new TossObject("AH Zol Incarnation", 6, 45, count: 4, angleOffset: 90, coolDown: 9999999),
                            new TimedTransition(3200, "f5")
                            ),
                        new State("f5",
                            new Shoot(10, count: 9, shootAngle: 8, projectileIndex: 3, coolDownOffset: 1100, angleOffset: 270, coolDown: 3000),
                            new Shoot(10, count: 9, shootAngle: 8, projectileIndex: 3, coolDownOffset: 1100, angleOffset: 90, coolDown: 3000),
                            new Shoot(12, count: 5, shootAngle: 12, projectileIndex: 4, coolDown: 1000),
                            new EntitiesNotExistsTransition(9999, "f6", "Zol Bomber", "AH Zol Incarnation")
                            ),
                        new State("f6",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 0),
                            new Taunt("Your life now belongs to me and me only!"),
                            new TimedTransition(4000, "f7")
                            ),
                        new State("f7",
                            new Flash(0x0F0F0F, 2, 2),
                            new Grenade(4, 300, coolDown: 3000),
                            new Prioritize(
                                new Charge(2, 10, coolDown: 4000),
                                new Wander(0.2)
                                ),
                            new Shoot(12, count: 7, shootAngle: 4, predictive: 1, projectileIndex: 0, coolDown: 500),
                            new Shoot(12, count: 18, projectileIndex: 2, coolDown: 4000),
                            new TimedTransition(12000, "f8")
                            ),
                        new State("f8",
                            new ConditionalEffect(ConditionEffectIndex.Armored),
                            new Prioritize(
                                new Follow(0.6),
                                new Wander(0.2)
                                ),
                            new Shoot(8, count: 4, projectileIndex: 4, coolDown: 400),
                            new Shoot(8, count: 6, projectileIndex: 4, coolDown: 1400),
                            new Shoot(12, count: 18, projectileIndex: 0, coolDown: 2500),
                            new TimedTransition(5000, "f9")
                            ),
                        new State("f9",
                            new Wander(0.4),
                            new Shoot(8, count: 12, projectileIndex: 4, coolDown: 1000),
                            new Shoot(12, count: 6, shootAngle: 8, projectileIndex: 2, coolDown: 2200),
                            new TossObject("Zol Slime", range: 4, coolDown: 2500),
                            new TimedTransition(5000, "f10")
                            ),
                        new State("f10",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Taunt(true, "THE TIME IS NOW!", "A SERENADE FOR YOUR DOOM!"),
                            new Flash(0xFF0000, 1, 2),
                            new ReturnToSpawn(1),
                            new TimedTransition(4400, "f11")
                            ),
                        new State("f11",
                            new Taunt("Al lar kall zanus du era!", "Rul ah ka tera nol zan!"),
                            new ReplaceTile("Zol Aura Dormant", "Zol Aura", 250),
                            new Shoot(30, count: 34, projectileIndex: 0, coolDown: 1000),
                            new TimedTransition(8000, "f12")
                            ),
                        new State("f12",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 0),
                            new ConditionalEffect(ConditionEffectIndex.Armored),
                            new ReplaceTile("Zol Aura", "Zol Aura Dormant", 250),
                            new Prioritize(
                                new Follow(1.2),
                                new Wander(0.2)
                                ),
                            new TossObject("AH Sincryer Orb", range: 10, coolDown: 2400),
                            new Shoot(8, count: 4, projectileIndex: 4, coolDown: 400),
                            new Shoot(8, count: 6, projectileIndex: 4, coolDown: 1400),
                            new Shoot(12, count: 18, projectileIndex: 0, coolDown: 2500),
                            new TimedTransition(5000, "f1")
                            )
                        ),

                    new State("dead",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new Taunt("YOU WILL NOT SURVIVE THE ONSLAUGHT OF THE ZOL!"),
                        new Flash(0x00FF00, 1, 3),
                        new ReturnToSpawn(1),
                        new TimedTransition(6000, "ded")
                        ),
                    new State("ded",
                        new Suicide()
                        )
                    ),
                new Threshold(0.01,
                    new ItemLoot("Vial Crate", 1),
                    new ItemLoot("Star of Corruption", 0.002),
                    new ItemLoot("Death Essence Trap", 0.002),
                    new ItemLoot("Rusted Medium Abilities Chest", 0.002),
                    new ItemLoot("Potion of Life", 1),
                    new ItemLoot("Potion of Mana", 1),
                    new ItemLoot("Vial of Life", 0.02),
                    new ItemLoot("Vial of Mana", 0.02),
                    new ItemLoot("Vial of Attack", 0.02),
                    new ItemLoot("Vial of Speed", 0.02),
                    new ItemLoot("Vial of Defense", 0.02),
                    new ItemLoot("Vial of Dexterity", 0.02),
                    new ItemLoot("Vial of Vitality", 0.02),
                    new ItemLoot("Vial of Wisdom", 0.02),
                    new ItemLoot("Vial of Luck", 0.0125),
                    new ItemLoot("Vial of Restoration", 0.0125)
                    )
                )
        #endregion Ultra Ganus the Sincryer

        #region Ultra Nirux the Vision
            .Init("AHU The Vision",
                new State(
                    new ScaleHP(0.3),
                    new DropPortalOnDeath("Ultra Core of the Hideout Portal", 100, timeout: 0),
                    new HpLessTransition(0.15, "dead"),
                    new State(
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new State("callout",
                            new Taunt(true, "He will fulfill the destiny in store. Come. Let me show you his vision.", "Even the ancients, the controllers, and Oryx himself fear us. Share that fear with them.", "Courage can only take you so far."),
                            new PlayerWithinTransition(5, "start")
                            ),
                        new State("start",
                            new Flash(0xFF0000, 2, 2),
                            new TimedTransition(4000, "talk")
                            ),
                        new State("talk",
                            new Taunt("They say the Zol was only a Myth... what fools... it's always been no mystery to you though..."),
                            new TimedTransition(6500, "spawn constructs")
                            )
                        ),
                    new State("fight",
                        new Shoot(14, count: 2, projectileIndex: 5, predictive: 1, coolDown: new Cooldown(4000, 2000)),
                        new Grenade(4, 200, range: 8, coolDown: 8000, effect: ConditionEffectIndex.Quiet, effectDuration: 6, color: 8388736),
                        new State("spawn constructs",
                            new ConditionalEffect(ConditionEffectIndex.Armored),
                            new Taunt("You can't withstand the Zol."),
                            new TossObject("Construct of Zol", 6, 0, count: 2, angleOffset: 180, coolDown: 9999999),
                            new TimedTransition(2000, "wait for constructs")
                            ),
                        new State("wait for constructs",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(12, count: 3, shootAngle: 18, projectileIndex: 0, coolDown: 800),
                            new Shoot(11, count: 2, projectileIndex: 3, coolDown: 1600),
                            new SpiralShoot(90, 4, numShots: 5, shootAngle: 10, projectileIndex: 1, coolDown: 2400),
                            new SpiralShoot(90, 4, numShots: 5, shootAngle: 10, projectileIndex: 1, coolDown: 2400, coolDownOffset: 1200),
                            new EntitiesNotExistsTransition(999, "fight 1", "Construct of Zol")
                            ),
                        new State("fight 1",
                            new ConditionalEffect(ConditionEffectIndex.Armored),
                            new Flash(0xFFFFFF, 1, 2),
                            new Shoot(8.4, count: 6, projectileIndex: 0, coolDown: 1200),
                            new TimedTransition(5000, "fight 2")
                            ),
                        new State("fight 2",
                            new Shoot(12, count: 1, predictive: 1.5, projectileIndex: 2, coolDown: 1000),
                            new Sequence(
                                new SpiralShoot(15, 4, numShots: 5, projectileIndex: 1, coolDown: 200),
                                new SpiralShoot(-15, 4, numShots: 5, fixedAngle: 45, projectileIndex: 1, coolDown: 200)
                                ),
                            new TimedTransition(8000, "fight 3")
                            ),
                        new State("fight 3",
                            new Taunt("Your essence is mine!"),
                            new TossObject("Demon of the Dark", 9, 45, count: 4, angleOffset: 90, coolDown: 9999999),
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Flash(0xFFFFFF, 1, 2),
                            new TimedTransition(5000, "fight 4")
                            ),
                        new State("fight 4",
                            new ConditionalEffect(ConditionEffectIndex.Armored),
                            new Shoot(14, count: 6, projectileIndex: 5, coolDown: 2000),
                            new TossObject("AH Feral of the Zol", 4, 315, count: 2, angleOffset: 90, coolDown: 9999999),
                            new TimedTransition(4000, "fight 5")
                            ),
                        new State("fight 5",
                            new Prioritize(
                                 new StayCloseToSpawn(0.5, 3),
                                 new Wander(0.05)
                                 ),
                            new Shoot(11, count: 5, projectileIndex: 3, coolDown: 400),
                            new Shoot(11, count: 4, shootAngle: 12, projectileIndex: 4, coolDown: 1500),
                            new TimedTransition(8000, "fight 6")
                            ),
                        new State("fight 6",
                            new ConditionalEffect(ConditionEffectIndex.Armored),
                            new Flash(0xFFFFFF, 1, 2),
                            new Shoot(10, count: 12, projectileIndex: 0, coolDown: 1200),
                            new TimedTransition(5000, "fight 7")
                            ),
                        new State("fight 7",
                            new Shoot(10, count: 6, projectileIndex: 0, coolDown: 1200),
                            new Shoot(10, count: 5, projectileIndex: 2, coolDown: 600),
                            new Shoot(10, count: 7, shootAngle: 10, projectileIndex: 1, coolDown: 1600),
                            new Shoot(10, count: 3, shootAngle: 2, projectileIndex: 2, coolDown: 1600, coolDownOffset: 500),
                            new TimedTransition(6400, "spawn constructs")
                            )
                        ),
                    new State("dead",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new Taunt("...Have I not succeeded this time?", "Aaah... so you don't see it?", "Will the vision finally be broken?"),
                        new Flash(0xF0FFF0, 1, 3),
                        new ReturnToSpawn(1),
                        new TimedTransition(6000, "ded")
                        ),
                    new State("ded",
                        new OrderOnce(20, "Nontridus Teleporter 1", "activate"),
                        new Suicide()
                        )
                    ),
                new Threshold(0.01,
                    new ItemLoot("Vial Crate", 1),
                    new ItemLoot("Staff of Consumption", 0.002),
                    new ItemLoot("Zol Striker", 0.002),
                    new ItemLoot("Rusted Heavy Abilities Chest", 0.002),
                    new ItemLoot("Potion of Life", 1),
                    new ItemLoot("Potion of Mana", 1),
                    new ItemLoot("Vial of Life", 0.02),
                    new ItemLoot("Vial of Mana", 0.02),
                    new ItemLoot("Vial of Attack", 0.02),
                    new ItemLoot("Vial of Speed", 0.02),
                    new ItemLoot("Vial of Defense", 0.02),
                    new ItemLoot("Vial of Dexterity", 0.02),
                    new ItemLoot("Vial of Vitality", 0.02),
                    new ItemLoot("Vial of Wisdom", 0.02),
                    new ItemLoot("Vial of Luck", 0.0125),
                    new ItemLoot("Vial of Restoration", 0.0125)
                    )
                )
        #endregion Ultra Nirux the Vision

        #region Ultra The Heart
            .Init("AHU The Heart",
                new State(
                    new State("before bursts",
                        new State("idle",
                            new PlayerWithinTransition(6, "default")
                            ),
                        new State("default",
                            new Taunt("Only the worthy will survive... stand on the plates to begin the challenge!"),
                            new TimedTransition(5000, "begin")
                            ),
                        new State("begin",
                            new Flash(0xFF00FF, 1, 2),
                            new Taunt(true, "GANUS AND NIRUX WILL BE FORGIVEN. WILL YOU?"),
                            new TimedTransition(5000, "plate check")
                            ),
                        new State("plate check",
                            new AllyNotExistsTransition("Beacon of Zol B", 999, "ReadyBurst1", true)
                            )
                        ),

                    new State("bursts",
                        new State("ReadyBurst1",
                            new Taunt("DO YOU ACCEPT MY CHALLENGE?", "THE ECHOES OF ZOL HAVE SEEN YOU FIGHT. ENGAGE IN THE TEST?"),
                            new PlayerTextTransition("CommenceBurst1", "Unleash the Power of the Zol!")
                            ),
                        new State("CommenceBurst1",
                            new Taunt("Such charisma would get you killed here. Let it begin.", "Have never seen anyone so excited to be crushed...", "It is unlikely you'll live to tell the tale.", "You seem brave now..."),
                            new TimedTransition(6000, "Burst1")
                            ),
                        new State("Burst1",
                            new Spawn("AH The Heart Logic", 1, 1, 9999999),
                            new Shoot(10, count: 8, projectileIndex: 0, coolDown: 8000),
                            new InvisiToss("Zol Sorcerer", 5, 45, count: 4, angleOffset: 90, coolDown: 9999999),
                            new InvisiToss("AH Zol Incarnation", 11.5, 15, count: 4, angleOffset: 90, coolDown: 9999999, coolDownOffset: 2600),
                            new InvisiToss("AH Feral of the Zol", 11.5, 75, count: 4, angleOffset: 90, coolDown: 9999999, coolDownOffset: 17500),
                            new InvisiToss("Corrupted Golem B", 8, 0, count: 4, angleOffset: 90, coolDown: 7000, coolDownOffset: 2600),
                            new TimedTransition(19000, "AfterBurst1")
                            ),
                        new State("AfterBurst1",
                            new GroupNotExistTransition(999, "ReadyBurst2", "Zol Minions")
                            ),

                        new State("ReadyBurst2",
                            new Flash(0x0000FF, 1, 1),
                            new Taunt(true, "Burst 1 has been defeated. Stay on your plates warriors... there is more to come."),
                            new TimedTransition(1500, "SayReadyBurst2")
                            ),
                        new State("SayReadyBurst2",
                            new Taunt(true, "Say 'READY' when you wish to begin the next Burst."),
                            new PlayerTextTransition("CommenceBurst2", "ready")
                            ),
                        new State("CommenceBurst2",
                            new TimedTransition(2000, "Burst2")
                            ),
                        new State("Burst2",
                            new Shoot(10, count: 16, projectileIndex: 0, coolDown: 8000),
                            new InvisiToss("Zol Sorcerer", 5, 45, count: 4, angleOffset: 90, coolDown: 9999999),
                            new InvisiToss("Corrupted Golem B", 8, 0, count: 4, angleOffset: 90, coolDown: 14000, coolDownOffset: 2600),
                            new InvisiToss("Corrupted Golem B", 11.5, 15, count: 4, angleOffset: 90, coolDown: 9999999, coolDownOffset: 9600),
                            new InvisiToss("Corrupted Golem B", 11.5, 75, count: 4, angleOffset: 90, coolDown: 9999999, coolDownOffset: 23600),
                            new InvisiToss("AH Zol Incarnation", 6, 80, count: 4, angleOffset: 90, coolDown: 24000, coolDownOffset: 2600),
                            new InvisiToss("AH Feral of the Zol", 14, 0, count: 4, angleOffset: 90, coolDown: 9999999, coolDownOffset: 2600),
                            new InvisiToss("AH Feral of the Zol", 14, 0, count: 4, angleOffset: 90, coolDown: 9999999, coolDownOffset: 26600),
                            new InvisiToss("Demon of the Dark", 7, 10, count: 4, angleOffset: 90, coolDown: 18000, coolDownOffset: 6600),
                            new TimedTransition(32000, "AfterBurst2")
                            ),
                        new State("AfterBurst2",
                            new GroupNotExistTransition(999, "Done", "Zol Minions")
                            )
                        ),

                    new State("Done",
                        new State(
                            new Flash(0xFFFFFF, 1, 3),
                            new Order(999, "AH The Heart Logic", "success"),
                            new Taunt("You have shown that you can withstand us... but can you resist the FULL power of the Zol?", "It must have taken preparation to defeat us. I applaud you.", "Congratulations! You have passed the final test to get to Aldragine."),
                            new TimedTransition(6000, "Success")
                            ),
                        new State("Success",
                            new DropPortalOnDeath("Ultra Keeping of Aldragine Portal", 1, timeout: 0),
                            new Suicide()
                            )
                        ),
                    
                    new State("Failed",
                        new State(
                            new Taunt(true, "THERE IS NO REMORSE. YOU ARE BANISHED.", "YOU HAVE NOT BEEN FORGIVEN. REIGN.", "WE DO NOT FORGIVE. YOU DO NOT ADVANCE."),
                            new Flash(0xFF0000, 0.25, 3),
                            new TimedTransition(1500, "Failure")
                            ),
                        new State("Failure",
                            new ClearLoot(),
                            new State("fail g1",
                                new ReplaceTile("Zol Aura Dormant", "Zol Aura", 999),
                                new ChangeGround( new[] {"Algadrine Tile 3", "Algadrine Tile 4"}, new[] {"Zol Aura Dormant", "Algadrine Tile 3", "Algadrine Tile 4"}, 999),
                                new ReplaceTile("Algadrine Tile Slimey", "Zol Aura Dormant", 999),
                                new TimedTransition(1100, "fail g2")
                                ),
                            new State("fail g2",
                                new ReplaceTile("Zol Aura Dormant", "Zol Aura", 999),
                                new ChangeGround( new[] {"Algadrine Tile 3", "Algadrine Tile 4"}, new[] {"Zol Aura Dormant"}, 999),
                                new ChangeGround( new[] {"Algadrine Tile 1", "Algadrine Tile 2"}, new[] {"Zol Aura Dormant", "Algadrine Tile 1", "Algadrine Tile 2"}, 999),
                                new ReplaceTile("Algadrine Tile Heart", "Zol Aura Dormant", 999),
                                new TimedTransition(1400, "fail g3")
                                ),
                            new State("fail g3",
                                new ReplaceTile("Zol Aura Dormant", "Zol Aura", 999),
                                new ChangeGround( new[] {"Algadrine Tile 1", "Algadrine Tile 2"}, new[] {"Zol Aura Dormant"}, 999),
                                new TimedTransition(1100, "fail g4")
                                ),
                            new State("fail g4",
                                new ReplaceTile("Zol Aura Dormant", "Zol Aura", 999),
                                new TimedTransition(1000, "fail death")
                                ),
                            new State("fail death",
                                new Suicide()
                                )
                            )
                        )
                    ),
                new Threshold(0.01,
                new ItemLoot("Vial Crate", 1),
                new ItemLoot("Crossbow of the Legion", 0.002),
                new ItemLoot("Chaotic Vanguard", 0.002),
                new ItemLoot("Rusted Light Abilities Chest", 0.002),
                new ItemLoot("Potion of Life", 1),
                new ItemLoot("Potion of Mana", 1),
                new ItemLoot("Vial of Life", 0.02),
                new ItemLoot("Vial of Mana", 0.02),
                new ItemLoot("Vial of Attack", 0.02),
                new ItemLoot("Vial of Speed", 0.02),
                new ItemLoot("Vial of Defense", 0.02),
                new ItemLoot("Vial of Dexterity", 0.02),
                new ItemLoot("Vial of Vitality", 0.02),
                new ItemLoot("Vial of Wisdom", 0.02),
                new ItemLoot("Vial of Luck", 0.0125),
                new ItemLoot("Vial of Restoration", 0.0125)
                    )
                )
        #endregion Ultra The Heart

        #region Ultra Aldragine
            .Init("AHU Aldragine",
                new State(
                    new ScaleHP(0.3),
                    new State("pre-fight",
                        new State("pf default",
                            new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                            new PlayerWithinTransition(4, "pf intimidation")
                            ),
                        new State("pf intimidation",
                            new ChangeSize(20, 220),
                            new TimedTransition(4000, "pf talk 1")
                            ),
                        new State("pf talk 1",
                            new Taunt("I'd assume it took you quite long to get here. Only to die."),
                            new TimedTransition(4000, "pf talk 2")
                            ),
                        new State("pf talk 2",
                            new Taunt("You've been given multiple warnings, but you fail to see that you WON'T MAKE ME FALL LIKE THE OTHERS.", "Your poor comprehension of true power leads me to think you are genuinely stupid."),
                            new TimedTransition(4000, "pf talk 3")
                            ),
                        new State("pf talk 3",
                            new Flash(0x00FF00, 1, 1),
                            new Taunt("Now, I'll show you the true meaning of power. Come closer. Let me show you."),
                            new TimedTransition(1000, "pf final")
                            ),
                        new State("pf final",
                            new TimedTransition(20000, "pf talk 3"),
                            new PlayerWithinTransition(2, "fight 1")
                            )
                        ),

                    new State("fight",
                        new State("fight 1",
                            new State("f1 flash",
                                new Flash(0xFF0000, 1, 2),
                                new TimedTransition(3000, "f1 fight")
                                ),
                            new State("f1 fight",
                                new ConditionalEffect(ConditionEffectIndex.Invincible, duration: 0),
                                new Prioritize(
                                     new StayCloseToSpawn(0.5, 3),
                                     new Wander(0.05)
                                     ),
                                new Shoot(8, count: 7, projectileIndex: 2, coolDown: 3000),
                                new Shoot(8, count: 4, projectileIndex: 3, shootAngle: 90, fixedAngle: 0, coolDown: 400, coolDownOffset: 2000),
                                new Shoot(8, count: 1, projectileIndex: 4, coolDown: new Cooldown(1500, 500)),
                                new Shoot(8, count: 4, shootAngle: 10, projectileIndex: 2, predictive: 1, coolDown: new Cooldown(1000, 600)),
                                new HpLessTransition(0.8, "fight 2")
                                )
                            ),
                        
                        new State("fight 2",
                            new State("f2 return",
                                new ReturnToSpawn(1),
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                                new TimedTransition(1500, "f2 talk")
                                ),
                            new State("f2 talk",
                                new Taunt("Not even a scratch. Pitiful."),
                                new TimedTransition(2500, "f2 fight")
                                ),
                            new State("f2 fight",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 0),
                                new Prioritize(
                                     new StayCloseToSpawn(0.5, 3),
                                     new Wander(0.05)
                                     ),
                                new Shoot(8, count: 11, projectileIndex: 2, coolDown: 2600),
                                new Shoot(8, count: 4, projectileIndex: 3, shootAngle: 90, fixedAngle: 0, coolDown: 200, coolDownOffset: 1600),
                                new Shoot(8, count: 1, projectileIndex: 4, coolDown: new Cooldown(1500, 500)),
                                new Shoot(8, count: 5, shootAngle: 8, projectileIndex: 0, predictive: 1, coolDown: new Cooldown(1000, 400)),
                                new HpLessTransition(0.6, "wall")
                                )
                            ),

                        new State("wall",
                            new State("w delay",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                                new ReturnToSpawn(1),
                                new TimedTransition(1000, "w talk")
                                ),
                            new State("w talk",
                                new Taunt("You are nothing to me but another small issue to fix."),
                                new ReturnToSpawn(1),
                                new Flash(0x0000FF, 1, 3),
                                new TimedTransition(2000, "w go left")
                                ),
                            new State("w go left",
                                new MoveTo(speed: 1, x: 2, y: 14),
                                new TimedTransition(3000, "w spawn minions")
                                ),
                            new State("w spawn minions",
                                new InvisiToss("Zol Sorcerer", 13, 10, count: 2, angleOffset: 20, coolDown: 9999999),
                                new InvisiToss("Zol Bomber", 11, 10, count: 2, angleOffset: 20, coolDown: 9999999),
                                new InvisiToss("Niolru", 9, 10, count: 2, angleOffset: 30, coolDown: 9999999),
                                new InvisiToss("Demon of the Dark", 7, 10, count: 2, angleOffset: 20, coolDown: 9999999),
                                new InvisiToss("Giant Cube of Zol", 4, 20, count: 2, angleOffset: 40, coolDown: 9999999),
                                new TimedTransition(1800, "w spawn knights")
                                ),
                            new State("w spawn knights",
                                new InvisiToss("Haunting Knight", range: 14, angle: 0, count: 5, angleOffset: 15, coolDown: 99999, coolDownOffset: 1800),
                                new InvisiToss("AH Feral of the Zol", 11, 0, count: 2, angleOffset: 30, coolDown: 9999999),
                                new InvisiToss("AH Zol Incarnation", 9, 0, count: 2, angleOffset: 30, coolDown: 9999999),
                                new InvisiToss("AH Battlemage of the Zol", 7, 0, count: 2, angleOffset: 30, coolDown: 9999999),
                                new EntityExistsTransition("Haunting Knight", 30, "w wait")
                                ),
                            new State("w wait",
                                new Shoot(8, count: 18, projectileIndex: 4, fixedAngle: 0, coolDown: 5000),
                                new GroupNotExistTransition(50, "w center", "Zol Minions")
                                ),
                            new State("w center",
                                new ReturnToSpawn(1, 0.5),
                                new TimedTransition(3000, "clones")
                                )
                            ),

                        new State("clones",
                            new State("c talk",
                                new Taunt("Fervent attacks are a waste if they can't compete with an unstoppable defense."),
                                new TimedTransition(2500, "c spawn")
                                ),
                            new State("c spawn",
                                new InvisiToss("AHU Aldragine Clone", 5.5, 90, count: 2, angleOffset: 180, coolDown: 9999999),
                                new EntityExistsTransition("AHU Aldragine Clone", 20, "c wait")
                                ),
                            new State("c wait",
                                new Shoot(20, count: 3, shootAngle: 30, projectileIndex: 10, coolDown: new Cooldown(1000, 500)),
                                new Shoot(20, count: 8, shootAngle: 45, fixedAngle: 0, projectileIndex: 2, coolDown: 2500),
                                new EntitiesNotExistsTransition(50, "fight 3", "AHU Aldragine Clone")
                                )
                            ),

                        new State("fight 3",
                            new State("f3 delay",
                                new RemoveEntity(30, "Demon of the Dark"),
                                new TimedTransition(500, "f3 fight")
                                ),
                            new State("f3 fight",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 0),
                                new Shoot(15, count: 20, projectileIndex: 5, coolDown: 2500),
                                new Shoot(10, count: 4, shootAngle: 6, projectileIndex: 6, coolDown: 800),
                                new Shoot(10, count: 5, shootAngle: 6, projectileIndex: 2, coolDown: 400),
                                new Shoot(10, count: 2, shootAngle: 15, projectileIndex: 3, coolDown: 1400),
                                new HpLessTransition(0.4, "survival")
                                )
                            ),

                        new State("survival",
                            new State("su delay",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                                new TimedTransition(1000, "su talk")
                                ),
                            new State("su talk",
                                new Taunt("Our heart's bursts were only a Trial to your true challenge. Let's see if you can handle this!"),
                                new TimedTransition(3000, "su throw stones")
                                ),
                            new State("su throw stones",
                                new TossObject("Stone of Zol", 12.53, count: 2, angleOffset: 57.22, angle: 90, coolDown: 9999999),
                                new TossObject("Stone of Zol", 11.66, count: 2, angleOffset: 61.92, angle: 270, coolDown: 9999999),
                                new EntityExistsTransition("Stone of Zol", 20, "su golems")
                                ),
                            new State(
                                new SpiralShoot(2, 90, 2, projectileIndex: 4, coolDown: 200),
                                /* alternative survival phase, where golems spawn in waves like normal aldragine's survival phase
                                new State("su golems",
                                    new OrderOnce(30, "Stone of Zol", "spawn random ultra"),
                                    new TossObject("AH Zol Incarnation", range: 5, angle: 0, count: 2, angleOffset: 180, coolDown: 30000),
                                    new TossObject("AH Feral of the Zol", range: 5, angle: 90, count: 2, angleOffset: 180, coolDown: 30000),
                                    new TossObject("AH Zol Incarnation", range: 5, angle: 90, count: 2, angleOffset: 180, coolDown: 30000, coolDownOffset: 15000),
                                    new TossObject("AH Feral of the Zol", range: 5, angle: 0, count: 2, angleOffset: 180, coolDown: 30000, coolDownOffset: 15000),
                                    new State("su g spawn1",
                                        new TossObject("Corrupted Golem C", 3.5, angle: 90, count: 2, angleOffset: 180, coolDown: 999999),
                                        new EntityExistsTransition("Corrupted Golem C", 20, "su g wait1")
                                        ),
                                    new State("su g wait1",
                                        new EntitiesNotExistsTransition(30, "su g spawn2", "Corrupted Golem C", "Headless Corrupted Golem C")
                                        ),
                                    new State("su g spawn2",
                                        new TossObject("Corrupted Golem C", 3.5, angle: 0, count: 4, angleOffset: 90, coolDown: 999999),
                                        new EntityExistsTransition("Corrupted Golem C", 20, "su g wait2")
                                        ),
                                    new State("su g wait2",
                                        new EntitiesNotExistsTransition(30, "fight 4", "Corrupted Golem C", "Headless Corrupted Golem C")
                                        )
                                    )
                                */
                                new State("su golems",
                                    new OrderOnce(30, "Stone of Zol", "spawn random ultra"),
                                    new TossObject("AH Zol Incarnation", range: 5, angle: 0, count: 2, angleOffset: 180, coolDown: 26000),
                                    new TossObject("AH Feral of the Zol", range: 5, angle: 90, count: 2, angleOffset: 180, coolDown: 26000),
                                    new TossObject("AH Zol Incarnation", range: 5, angle: 90, count: 2, angleOffset: 180, coolDown: 26000, coolDownOffset: 13000),
                                    new TossObject("AH Feral of the Zol", range: 5, angle: 0, count: 2, angleOffset: 180, coolDown: 26000, coolDownOffset: 13000),
                                    new TossObject("Corrupted Golem C", 3.5, angle: 0, count: 4, angleOffset: 90, coolDown: 999999, coolDownOffset: 62000),
                                    new TimedTransition(65000, "su after")
                                    ),
                                new State("su after",
                                    new EntitiesNotExistsTransition(30, "fight 4", "Corrupted Golem C", "Headless Corrupted Golem C")
                                    )
                                )
                            ),

                        new State("fight 4",
                            new State("f4 delay",
                                new OrderOnce(30, "Stone of Zol", "invulnerable"),
                                new RemoveGroup(30, "Zol Minions"),
                                new TimedTransition(800, "f4 flash")
                                ),
                            new State("f4 flash",
                                new Flash(0xFF0000, 1, 3),
                                new TimedTransition(3000, "f4 fight")
                                ),
                            new State("f4 fight",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 0),
                                new Prioritize(
                                     new StayCloseToSpawn(0.5, 3),
                                     new Wander(0.05)
                                     ),
                                new SpiralShoot(30, 2, numShots: 6, projectileIndex: 4, coolDown: 3000),
                                new Shoot(10, count: 4, shootAngle: 10, projectileIndex: 8, coolDown: 600),
                                new HpLessTransition(0.2, "stones")
                                )
                            ),

                        new State("stones",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new State("st talk",
                                new ReturnToSpawn(1, 0.5),
                                new Taunt("Is that all you've got?", "Hahaha!"),
                                new TimedTransition(2500, "st start")
                                ),
                            new State("st start",
                                new Flash(0x0F00F0, 1, 3),
                                new TossObject("AH Battlemage of the Zol", 6, 0, count: 8, angleOffset: 45, coolDown: 9999999, coolDownOffset: 2000),
                                new TimedTransition(3000, "st wait")
                                ),
                            new State("st wait",
                                new Order(30, "Stone of Zol", "vulnerable"),
                                new Taunt("Your time is ticking, warriors!", "Unstoppable."),
                                new SpiralShoot(36, 2, numShots: 5, projectileIndex: 3, coolDown: 500, delayAfterComplete: 1500),
                                new Shoot(20, count: 3, shootAngle: 6, projectileIndex: 6, coolDown: 400),
                                new Shoot(20, count: 6, shootAngle: 6, projectileIndex: 7, predictive: 1, coolDown: 400, coolDownOffset: 800),
                                new EntitiesNotExistsTransition(30, "final", "Stone of Zol"),
                                new TimedTransition(75000, "Failed")
                                )
                            ),

                        new State("final",
                            new State("fin talk 1",
                                new State("fin t1 1",
                                    new Taunt("Your lacking skills have somehow managed to survive me. Perhaps it's luck."),
                                    new TimedTransition(2000, "fin t1 2")
                                    ),
                                new State("fin t1 2",
                                    new Grenade(0, 0, range: 4, fixedAngle: 270, coolDown: 999999, color: 0x00FF00),
                                    new TimedTransition(1200, "fin t1 3")
                                    ),
                                new State("fin t1 3",
                                    new OrderOnce(20, "Hideout Destruction Mechanism Spawner", "activate"),
                                    new TimedTransition(1800, "fin talk 2")
                                    )
                                ),
                            new State("fin shoot",
                                new Shoot(20, count: 1, projectileIndex: 4, fixedAngle: 90, coolDown: 9999999, coolDownOffset: 1500),
                                new Shoot(20, count: 3, shootAngle: 10, projectileIndex: 4, fixedAngle: 90, coolDown: 9999999, coolDownOffset: 2000),
                                new Shoot(20, count: 6, shootAngle: 10, projectileIndex: 4, fixedAngle: 90, coolDown: 9999999, coolDownOffset: 2500),
                                new Shoot(20, count: 10, shootAngle: 10, projectileIndex: 4, fixedAngle: 90, coolDown: 9999999, coolDownOffset: 3000),
                                new Shoot(20, count: 16, shootAngle: 10, projectileIndex: 4, fixedAngle: 90, coolDown: 9999999, coolDownOffset: 3500),
                                new Shoot(20, count: 23, shootAngle: 10, projectileIndex: 4, fixedAngle: 90, coolDown: 9999999, coolDownOffset: 4000),
                                new Shoot(20, count: 31, shootAngle: 10, projectileIndex: 4, fixedAngle: 90, coolDown: 9999999, coolDownOffset: 4500),
                                new State("fin talk 2",
                                    new Taunt("However, nothing is capable of surviving the pure energy of the Zol."),
                                    new TimedTransition(4000, "fin talk 3")
                                    ),
                                new State("fin talk 3",
                                    new Flash(0x00ff00, 1, 2),
                                    new Taunt("PERISH!"),
                                    new TimedTransition(2000, "fin survival")
                                    )
                                ),
                            new State("fin survival",
                                new OrderOnce(50, "Hideout Destruction Mechanism Active", "spawn ultra"),
                                new Shoot(20, count: 32, shootAngle: 10, projectileIndex: 4, fixedAngle: 90, coolDown: 2200),
                                new EntityNotExistsTransition("Hideout Destruction Mechanism Active", 50, "Failed")
                                )
                            )
                        ),

                    new State("Done",
                        new Flash(0xFFFFFF, 1, 3),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new State("done delay",
                            new TimedTransition(2000, "done taunt1")
                            ),
                        new State("done taunt1",
                            new Taunt(true, "Awaited the day for me to be defeated..."),
                            new TimedTransition(3000, "done taunt2")
                            ),
                        new State("done taunt2",
                            new Taunt(true, "We'll meet again, warriors..."),
                            new TimedTransition(3000, "done success")
                            ),
                        new State("done success",
                            new AnnounceOnDeath("The Zol, a dark burden, seems to fade away slowly, once again..."),
                            new Shoot(8, count: 10, projectileIndex: 2, coolDown: 9999),
                            new Suicide()
                            )
                        ),

                    new State("Failed",
                        new Flash(0xFF0000, 1, 3),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new Taunt(true, "I HAVE GAINED ENOUGH ZOL. THE WORLDS WILL SOON BE OURS..."),
                        new Order(50, "Hideout Destruction Mechanism Active", "suicide"),
                        new Order(50, "Hideout Destruction Mechanism Inactive", "suicide"),
                        new ReturnToSpawn(1),
                        new TimedTransition(2000, "Failure")
                        ),
                    new State("Failure",
                        new ClearLoot(),
                        new OrderOnce(30, "Stone of Zol", "invulnerable"),
                        new State(
                            new SpiralShoot(6, 2, numShots: 30, projectileIndex: 4, coolDownOffset: 2000, coolDown: 3000),
                            new State("fail g1",
                                new ReplaceTile("Zol Aura Dormant", "Zol Aura", 999),
                                new ChangeGround( new[] {"Algadrine Tile 1", "Algadrine Tile 2"}, new[] {"Zol Aura Dormant", "Algadrine Tile 1", "Algadrine Tile 2"}, 999),
                                new ReplaceTile("Algadrine Tile Slimey", "Zol Aura Dormant", 999),
                                new TimedTransition(1100, "fail g2")
                                ),
                            new State("fail g2",
                                new ReplaceTile("Zol Aura Dormant", "Zol Aura", 999),
                                new ChangeGround( new[] {"Algadrine Tile 1", "Algadrine Tile 2"}, new[] {"Zol Aura Dormant"}, 999),
                                new ChangeGround( new[] {"Algadrine Tile 3", "Algadrine Tile 4"}, new[] {"Zol Aura Dormant", "Algadrine Tile 3", "Algadrine Tile 4"}, 999),
                                new TimedTransition(1400, "fail g3")
                                ),
                            new State("fail g3",
                                new ReplaceTile("Zol Aura Dormant", "Zol Aura", 999),
                                new ChangeGround( new[] {"Algadrine Tile 3", "Algadrine Tile 4"}, new[] {"Zol Aura Dormant"}, 999),
                                new TimedTransition(1100, "fail g4")
                                ),
                            new State("fail g4",
                                new ReplaceTile("Zol Aura Dormant", "Zol Aura", 999),
                                new TimedTransition(1500, "fail death")
                                )
                            ),
                        new State("fail death",
                            new Flash(0x00FF00, 4.8, 1),
                            new ChangeSize(-15, 0),
                            new TimedTransition(3000, "fail suicide")
                            ),
                        new State("fail suicide",
                            new ClearLoot(),
                            new Suicide()
                            )
                        )
                    ),
                new Threshold(0.01,
                new ItemLoot("Vial Crate", 1),
                new ItemLoot("The Final Stand", 0.002),
                new ItemLoot("Seal of Endless Regret", 0.002),
                new ItemLoot("Armor of the Zol", 0.002),
                new ItemLoot("Zol Medallion", 0.002),
                new ItemLoot("Rusted Ring", 0.00143),
                new ItemLoot("Master Eon", 0.00334),
                new ItemLoot("Potion of Life", 1),
                new ItemLoot("Potion of Mana", 1),
                new ItemLoot("Vial of Life", 0.04),
                new ItemLoot("Vial of Mana", 0.04),
                new ItemLoot("Vial of Attack", 0.04),
                new ItemLoot("Vial of Speed", 0.04),
                new ItemLoot("Vial of Defense", 0.04),
                new ItemLoot("Vial of Dexterity", 0.04),
                new ItemLoot("Vial of Vitality", 0.04),
                new ItemLoot("Vial of Wisdom", 0.04),
                new ItemLoot("Vial of Luck", 0.025),
                new ItemLoot("Vial of Restoration", 0.025)
                    )
                )
        #endregion Ultra Aldragine
                
    #endregion bosses

    #region minions

        #region Sincryer's Gate Spawner
            .Init("AHU Sincryers Gate Opener",
                new State(
                    new DropPortalOnDeath("Ultra Sincryer's Gate Portal", 100, timeout: 0),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("idle",
                        new EntitiesNotExistsTransition(9999, "activate", "Portal to Eternity")
                        ),
                    new State("activate",
                        new Suicide()
                        )
                    )
                )
        #endregion Sincryer's Gate Spawner

        #region Corrupted Entity
            .Init("Corrupted Entity",
                new State(
                    new State("start",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt(0.25, "Yesss....YESSSS......", "I FEEL SO..POWERFUL!", "MY VEINS...THE ZOL COURSES WITHIN THEM!", "Old companions...I AM YOUR NEW MASTER!", "...."),
                        new ChangeSize(60, 150),
                        new TimedTransition(6000, "talk")
                        ),
                    new State("talk",
                        new Flash(0x00FF00, 0.2, 8),
                        new Taunt(0.25, "Time to demonstrate my new powers..", "Come here you fool...", "I've been burdened by you fools for too long..DIE!"),
                        new TimedTransition(2000, "invuln")
                        ),
                    new State(
                        new Prioritize(
                            new Follow(1, 8, 1),
                            new Wander(0.4)
                            ),
                        new Shoot(10, count: 8, fixedAngle: 0, projectileIndex: 1, coolDown: 4000, coolDownOffset: 4000),
                        new Shoot(10, count: 5, shootAngle: 12, projectileIndex: 0, coolDown: 2000, coolDownOffset: 4000),
                        new State("invuln",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new TimedTransition(4000, "heal")
                            ),
                        new State("heal",
                            new TimedTransition(4000, "invuln")
                            )
                        )
                    )
                )
        #endregion Corrupted Entity

        #region Zol Turret
            .Init("AHU Zol Turret",
                new State(
                    new SetNoXP(),
                    new State("switch1",
                        new SetAltTexture(0),
                        new ChangeSize(20, 130),
                        new Shoot(8, count: 2, shootAngle: 90, projectileIndex: 0, coolDown: 4000),
                        new Shoot(8, count: 2, shootAngle: 0, projectileIndex: 0, coolDown: 4000, coolDownOffset: 2000),
                        new DamageTakenTransition(25000, "switch2")
                        ),
                    new State("switch2",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new SetAltTexture(1),
                        new ChangeSize(-20, 110),
                        new Flash(0xFF00FF, 1, 1),
                        new TimedTransition(6000, "switch1")
                        )
                    )
                )
        #endregion Zol Turret

        #region Aldragine Clone
            .Init("AHU Aldragine Clone",
                new State(
                    new ScaleHP(0.3),
                    new SetNoXP(),
                    new SetAltTexture(1),
                    new ReproduceChildren(maxChildren: 2, initialSpawn: 0, coolDown: 2000, children: new[] {"Demon of the Dark"}),
                    new Orbit(0.3, 5.5, 10, target: "AHU Aldragine", speedVariance: 0, radiusVariance: 0),
                    new SpiralShoot(45, 2, numShots: 4, shootAngle: 90, coolDown: 1000)
                    )
                )
        #endregion Aldragine Clone

    #endregion minions

#endregion ultra

#region both

    #region misc. minions

        #region Zol Sorcerer
            .Init("Zol Sorcerer",
                new State(
                    new ScaleHP(0.1),
                    new Prioritize(
                        new StayBack(0.4, 8),
                        new Wander(0.35)
                        ),
                    new Grenade(3, 75, 12, coolDown: 1200, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 3500)
                    )
                )
        #endregion Zol Sorcerer

        #region Demon of the Dark
            .Init("Demon of the Dark",
                new State(
                    new ScaleHP(0.1),
                    new Prioritize(
                        new Follow(0.8, 8, 1),
                        new Wander(0.5)
                        ),
                    new State("fight1",
                        new Shoot(10, count: 3, shootAngle: 12, projectileIndex: 0, coolDown: 2000),
                        new Shoot(10, count: 7, projectileIndex: 1, coolDown: 6000),
                        new TimedTransition(6800, "fight2")
                        ),
                    new State("fight2",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Shoot(10, count: 5, projectileIndex: 2, coolDown: 2000),
                        new TimedTransition(4000, "fight1")
                        )
                    )
                )
        #endregion Demon of the Dark

        #region Niolru
            .Init("Niolru",
                new State(
                    new ScaleHP(0.1),
                    new State("fight1",
                        new Prioritize(
                            new Follow(1.1, 10, 1),
                            new Wander(0.2)
                            ),
                        new Shoot(10, count: 5, shootAngle: 5, projectileIndex: 1, coolDown: 700),
                        new TimedTransition(1500, "fight2")
                        ),
                    new State("fight2",
                        new Prioritize(
                            new StayBack(0.7, 6.5),
                            new Wander(0.3)
                            ),
                        new Shoot(10, count: 5, shootAngle: 5, projectileIndex: 0, coolDown: 700, coolDownOffset: 500),
                        new TimedTransition(3000, "fight1")
                        )
                    )
                )
        #endregion Niolru

        #region Brute of the Hideout
            .Init("Brute of the Hideout",
                new State(
                    new ScaleHP(0.1),
                    new Prioritize(
                        new Follow(1.2, 8, 1),
                        new Wander(0.4)
                        ),
                    new Shoot(8, 3, shootAngle: 10, coolDown: 400)
                    ),
                new Threshold(0.5,
                    new ItemLoot("Ring of Exalted Vitality", 0.01)
                    )
                )
        #endregion Brute of the Hideout

        #region Zol Cleric
            .Init("Cleric of Zol",
                new State(
                    new ScaleHP(0.1),
                    new Wander(0.6),
                    new Shoot(12, count: 1, projectileIndex: 0, coolDown: 1000)
                    ),
                new Threshold(0.5,
                    new ItemLoot("Tome of Divine Favor", 0.15)
                    )
                )
        #endregion Zol Cleric

        #region Zol Bomber
            .Init("Zol Bomber",
                new State(
                    new ScaleHP(0.1),
                    new State("default",
                        new Prioritize(
                            new Follow(0.4, 8, 1),
                            new Wander(0.7)
                            ),
                        new Shoot(10, count: 4, projectileIndex: 1, coolDown: 2000),
                        new TossObject("Zol Mine", range: 7, coolDown: new Cooldown(6200, 4000), coolDownOffset: 2400),
                        new HpLessTransition(0.25, "fight2")
                        ),
                    new State(
                        new Follow(1, 8, 1),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new State("fight2",
                            new Flash(0xFF0000, 1, 3),
                            new PlayerWithinTransition(2, "ded")
                            ),
                        new State("ded",
                            new Shoot(10, count: 12, projectileIndex: 0, coolDown: 9999),
                            new Suicide()
                            )
                        )
                    )
                )
        #endregion Zol Bomber

        #region Zol Mine
            .Init("Zol Mine",
                new State(
                    new State("1",
                        new Shoot(10, count: 8, angleOffset: 45, fixedAngle: 0, projectileIndex: 0, coolDown: 4000, coolDownOffset: 4000),
                        new DamageTakenTransition(2500, "2")
                        ),
                    new State("2",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new SetAltTexture(1),
                        new Flash(0xFFFFFF, 1, 1),
                        new TimedTransition(2000, "3")
                        ),
                    new State("3",
                        new SetAltTexture(1),
                        new Shoot(10, count: 8, projectileIndex: 2, coolDown: 2000),
                        new Suicide()
                        )
                    )
                )
        #endregion Zol Mine

        #region Slimes
            .Init("Zol Slime",
                new State(
                    new ScaleHP(0.1),
                    new Shoot(10, 3, shootAngle: 10, predictive: 0.5, coolDown: 1400),
                    new Wander(0.1),
                    new TransformOnDeath("Small Zol Slime", 1, 6, 1)
                    )
                )

            .Init("Small Zol Slime",
                new State(
                    new ScaleHP(0.1),
                    new Shoot(10, 1, predictive: 0.2, coolDown: 700),
                    new Wander(0.1)
                    )
                )
        #endregion Slimes

        #region Cubes
            .Init("Giant Cube of Zol",
                new State(
                    new ScaleHP(0.1),
                    new State("swag1",
                        new Shoot(10, count: 5, shootAngle: 14, projectileIndex: 0, coolDown: new Cooldown(3000, 1000)),
                        new Shoot(10, count: 1, projectileIndex: 1, coolDown: 500),
                        new TimedTransition(5000, "swag2")
                        ),
                    new State("swag2",
                        new TossObject("Cube of Zol", coolDown: 2500),
                        new TimedTransition(5000, "swag1")
                        )
                    )
                )


            .Init("Cube of Zol",
                new State(
                    new ScaleHP(0.1),
                    new Wander(0.8),
                    new Shoot(8, 1, coolDown: 750)
                    ),
                new Threshold(0.5,
                    new ItemLoot("Mithril Shield", 0.01),
                    new ItemLoot("Agateclaw Dagger", 0.01)
                    )
                )
        #endregion Cubes

        #region Servant of Darkness
            .Init("Servant of Darkness",
                new State(
                    new ScaleHP(0.1),
                    new Wander(1),
                    new State("fight1",
                        new Shoot(10, count: 2, shootAngle: 8, projectileIndex: 0, coolDown: 400),
                        new TimedTransition(4000, "fight2")
                        ),
                    new State("fight2",
                        new Shoot(10, count: 5, projectileIndex: 1, coolDown: 1200),
                        new TimedTransition(4000, "fight1")
                        )
                    )
                )
        #endregion Servant of Darkness
                
        #region Feral of the Zol
            .Init("AH Feral of the Zol",
                new State(
                    new ScaleHP(0.1),
                    new State("Main",
                        new Prioritize(
                            new StayAbove(1, 200),
                            new Follow(0.4, range: 7),
                            new Wander(0.4)
                            ),
                        new Shoot(10, count: 5, shootAngle: 14, projectileIndex: 0, predictive: 1, coolDown: 3000, coolDownOffset: 250),
                        new TimedTransition(3000, "charging")
                        ),
                     new State("charging",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 1000),
                        new Taunt("YOU SHALL BE DEVOURED."),
                        new Flash(0xFF0000, 0.25, 4),
                        new Shoot(10, count: 18, projectileIndex: 0, coolDown: 2000, coolDownOffset: 250),
                        new Charge(speed: 1.5, range: 10, coolDown: 4000),
                        new TimedTransition(4000, "grr")
                        ),
                    new State("grr",
                        new StayBack(0.7, distance: 3),
                        new Shoot(10, count: 8, shootAngle: 20, projectileIndex: 1, coolDown: 400),
                        new Shoot(10, count: 6, projectileIndex: 0, coolDown: 2000),
                        new TimedTransition(4000, "fight2")
                        ),
                    new State("fight2",
                        new Wander(0.1),
                        new Shoot(10, count: 3, shootAngle: 8, projectileIndex: 2, coolDown: 400),
                        new TimedTransition(4000, "heal")
                        ),
                    new State("heal",
                        new Grenade(7, 125, range: 8, coolDown: 1000),
                        new Shoot(10, count: 4, shootAngle: 40, projectileIndex: 0, coolDown: new Cooldown(1000, 500)),
                        new TimedTransition(4000, "Main")
                        )
                    )
                )
        #endregion Feral of the Zol

        #region Zol Incarnation
            .Init("AH Zol Incarnation",
                new State(
                    new ScaleHP(0.1),
                    new State("fight",
                        new TimedTransition(8000, "sneaky"),
                        new State("fight1",
                            new Prioritize(
                                new Follow(0.6, range: 7),
                                new Wander(0.4)
                                ),
                            new Shoot(4, count: 1, projectileIndex: 1, coolDown: 400),
                            new Shoot(10, count: 4, shootAngle: 18, fixedAngle: 0, projectileIndex: 0, coolDown: 1000),
                            new Shoot(10, count: 4, shootAngle: 18, fixedAngle: 180, projectileIndex: 0, coolDown: 1000),
                            new TimedTransition(2000, "fight2")
                            ),
                        new State("fight2",
                            new Shoot(4, count: 1, projectileIndex: 1, coolDown: 400),
                            new Prioritize(
                                new Follow(0.6, range: 7),
                                new Wander(0.4)
                            ),
                            new Shoot(10, count: 4, shootAngle: 18, fixedAngle: 90, projectileIndex: 0, coolDown: 1000),
                            new Shoot(10, count: 4, shootAngle: 18, fixedAngle: 270, projectileIndex: 0, coolDown: 1000),
                            new TimedTransition(2000, "fight1")
                            )
                        ),
                    new State("sneaky",
                        new ChangeSize(10, 0),
                        new Follow(1, range: 7),
                        new TimedTransition(4000, "reveal")
                        ),
                    new State("reveal",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Flash(0xFF00FF, 1, 1),
                        new ChangeSize(10, 130),
                        new Shoot(10, count: 6, projectileIndex: 1, coolDown: 2000),
                        new Orbit(1, 3, target: null, orbitClockwise: true),
                        new TimedTransition(4000, "fight1")
                        )
                    )
                )
        #endregion Zol Incarnation

        #region Battlemage of the Zol
            .Init("AH Battlemage of the Zol",
                new State(
                    new ScaleHP(0.1),
                    new Shoot(8, count: 1, projectileIndex: 2, coolDown: new Cooldown(2000, 1000)),
                    new Shoot(8, count: 6, projectileIndex: 3, coolDown: 3000),
                    new State("default",
                        new Wander(0.1),
                        new Shoot(8, count: 2, shootAngle: 6, projectileIndex: 0, coolDown: new Cooldown(600, 400)),
                        new PlayerWithinTransition(3, "shotgun")
                        ),
                    new State("shotgun",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ConditionalEffect(ConditionEffectIndex.StunImmune),
                        new Prioritize(
                            new Follow(1.5, 8, 1),
                            new Wander(0.25)
                            ),
                        new Taunt(0.50, "I call upon the great Zol to EMPOWER ME!", "You'll be eradicated quickly, fiend."),
                        new Shoot(8, count: 3, shootAngle: 8, projectileIndex: 1, predictive: 1, coolDown: 1000),
                        new Shoot(8, count: 4, projectileIndex: 3, coolDown: 600),
                        new TimedTransition(3000, "shotgun2")
                        ),
                    new State("shotgun2",
                        new Shoot(8, count: 6, shootAngle: 18, projectileIndex: 0, coolDown: new Cooldown(1200, 800)),
                        new Shoot(8, count: 8, projectileIndex: 1, coolDown: 400),
                        new TimedTransition(4600, "default")
                        )
                    ),
                new Threshold(0.5,
                    new ItemLoot("Bow of Innocent Blood", 0.02),
                    new ItemLoot("Abyssal Armor", 0.02)
                    )
                )
        #endregion Battlemage of the Zol
                
        #region Haunting Knight
            .Init("Haunting Knight",
                new State(
                    new ScaleHP(0.1),
                    new ConditionalEffect(ConditionEffectIndex.StunImmune),
                    new ConditionalEffect(ConditionEffectIndex.ArmorBreakImmune),
                    new ConditionalEffect(ConditionEffectIndex.StasisImmune),
                    new State("fight1",
                        new Prioritize(
                            new Follow(1.5, 8, 1),
                            new Wander(0.2)
                            ),
                        new Shoot(9, count: 7, shootAngle: 15, projectileIndex: 1, coolDown: new Cooldown(2500, 500), coolDownOffset: 1000),
                        new Shoot(7, count: 8, projectileIndex: 0, coolDown: new Cooldown(5300, 1000), coolDownOffset: 2000),
                        new TimedTransition(7000, "fight2")
                        ),
                    new State("fight2",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Wander(0.4),
                        new TimedTransition(2000, "fight1")
                        )
                    )
                )
        #endregion Haunting Knight

        #region Sincryer Orb
            .Init("AH Sincryer Orb",
                new State(
                    new ScaleHP(0.1),
                    new Flash(0x00FF00, 0.25, 6),
                    new Shoot(10, count: 6, projectileIndex: 0, coolDown: 1800, coolDownOffset: 3000)
                    )
                )
        #endregion Sincryer Orb

    #endregion misc. minions

    #region Aura Controller
            .Init("AH Aura Controller",
                new State(
                    new ScaleHP(0.1),
                    new ChangeGroundOnDeath(new[] {"Zol Aura"}, new[] {"Zol Aura Dormant"}, 9999),

                    new State("normal speed",
                        new State("Deactivate",
                            new Shoot(20, fixedAngle: 180, coolDown: 99999), // to trigger the attack animation
                            new ReplaceTile("Zol Aura", "Zol Aura Dormant", 9999),
                            new TimedTransition(5000, "Activate")
                            ),
                        new State("Activate",
                            new Shoot(20, fixedAngle: 180, coolDown: 99999),
                            new ReplaceTile("Zol Aura Dormant", "Zol Aura", 9999),
                            new TimedTransition(5000, "Deactivate")
                            )
                        ),
                        
                    new State("fast speed", // this is necessary because using Entity.TickStateManually
                                // and ticking the entity from the World's Tick() function results in
                                // the entity ticking less often, so it needs to do stuff faster
                        new State("DeactivateFast",
                            new Shoot(20, fixedAngle: 180, coolDown: 99999), // to trigger the attack animation
                            new ReplaceTile("Zol Aura", "Zol Aura Dormant", 9999),
                            new TimedTransition((int)(5000 * CoreConstant.worldLogicTickMs / CoreConstant.worldTickMs), "ActivateFast")
                            ),
                        new State("ActivateFast",
                            new Shoot(20, fixedAngle: 180, coolDown: 99999),
                            new ReplaceTile("Zol Aura Dormant", "Zol Aura", 9999),
                            new TimedTransition((int)(5000 * CoreConstant.worldLogicTickMs / CoreConstant.worldTickMs), "DeactivateFast")
                            )
                        )
                    )
                )
    #endregion Aura Controller

    #region Nontridus Teleporters

            .Init("Nontridus Teleporter 1",
                new State(
                    new State("inactive"),
                    new State("activate",
                        new TimedTransition(1000, "activate2")
                        ),
                    new State("activate2",
                        new Grenade(3, 0, range: 0, fixedAngle: 0, coolDown: 99999, color: 0xd20b79),
                        new TimedTransition(1400, "active")
                        ),
                    new State("active",
                        new ChangeGround(null, new[] {"Algadrine Tile Heart"}, 1),
                        new TeleportPlayerTo(51.5f, 40.5f, 0.6)
                        )
                    )
                )
                
            .Init("Nontridus Teleporter 2",
                new State(
                    new ChangeGround(null, new[] {"Algadrine Tile Heart"}, 1),
                    new TeleportPlayerTo(124.5f, 123.5f, 0.6)
                    )
                )

    #endregion Nontridus Teleporters

    #region Portal to Eternity
            .Init("Portal to Eternity",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("swag1",
                        new EntitiesNotExistsTransition(10, "dying", "Corrupted Golem A")
                        ),
                    new State("dying",
                        new Flash(0xFF00FF, 1, 2),
                        new TimedTransition(2500, "swag2")
                        ),
                    new State("swag2",
                        new Shoot(10, count: 10, projectileIndex: 0, coolDown: 9999),
                        new Suicide()
                        )
                    )
                )
    #endregion Portal to Eternity

    #region golems
        
        #region Corrupted Golem A
            .Init("Corrupted Golem A",
                new State(
                    new ScaleHP(0.1),
                    new HpLessTransition(0.15, "suicide1"),

                    new State("fight",
                        new Orbit(0.5, 3, 20, "Portal to Eternity"),
                        new State("fight1",
                            new Shoot(15, count: 1, coolDown: 400),
                            new Shoot(15, count: 4, shootAngle: 30, projectileIndex: 1, coolDown: 1000),
                            new TimedTransition(5000, "fight2")
                            ),
                        new State("fight2",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 6000),
                            new Sequence(
                                new SpiralShoot(90, 12, 6, 10, coolDown: 500),
                                new TransitionOnTick("fight3")
                                )
                            ),
                        new State("fight3",
                            new ConditionalEffect(ConditionEffectIndex.Armored),
                            new Shoot(10, count: 3, shootAngle: 15, projectileIndex: 0, coolDown: 1000),
                            new Shoot(10, count: 2, shootAngle: 15, projectileIndex: 0, coolDown: 1000, coolDownOffset: 500),
                            new TimedTransition(5000, "fight1")
                            )
                        ),

                    new State("suicide",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new State("suicide1",
                            new Flash(0x00ff00, 0.7, 3),
                            new TimedTransition(2000, "suicide2")
                            ),
                        new State("suicide2",
                            new Shoot(50, 5, projectileIndex: 2, coolDown: 9999999),
                            new Grenade(4, 150, 0, 0, coolDown: 9999999, color: 0x00ff00),
                            new Suicide()
                            )
                        )
                    )
                )
        #endregion Corrupted Golem A

        #region Corrupted Golem B
            .Init("Corrupted Golem B",
                new State(
                    new ScaleHP(0.1),
                    new TransformOnDeath("Headless Corrupted Golem B", 3, 3),
                    new TransferDamageOnDeath("AH The Heart", 50),
                    new TransferDamageOnDeath("AHU The Heart", 50),
                    new State("Main",
                        new SetAttackTarget("Beacon of Zol A", seeInvis: true),
                        new State("fight1",
                            new Orbit(0.15, 4.13, 20, seeInvis: true),
                            new Shoot(8, count: 5, shootAngle: 20, projectileIndex: 1, coolDown: 1300, coolDownOffset: 1000, seeInvis: true),
                            new TimedTransition(9000, "fight2")
                            ),
                        new State("fight2",
                            new ConditionalEffect(ConditionEffectIndex.Armored),
                            new Prioritize(
                                new Follow(0.2, acquireRange: 15, range: 1),
                                new Wander(0.1)
                                ),
                            new Shoot(7, count: 6, shootAngle: 10, projectileIndex: 0, coolDown: 700, coolDownOffset: 500, seeInvis: true),
                            new TimedTransition(5000, "fight1"),
                            new State("machine gun cooldown",
                                new TimedTransition(2000, "machine gun")
                                ),
                            new State("machine gun",
                                new Shoot(4, 1, projectileIndex: 2, coolDown: 200, seeInvis: true),
                                new TimedTransition(1400, "machine gun cooldown")
                                )
                            )
                        )
                    )
                )
            
            .Init("Headless Corrupted Golem B",
                new State(
                    new ScaleHP(0.1),
                    new ConditionalEffect(ConditionEffectIndex.ArmorBroken),
                    new SetAttackTarget("Beacon of Zol A", seeInvis: true),
                    new Prioritize(
                        new Follow(0.2, range: 6),
                        new StayBack(0.2, 3),
                        new Wander(0.2)
                        ),
                    new Shoot(10, 1, coolDown: new Cooldown(4000, 400)),
                    new Shoot(10, 2, shootAngle: 20, coolDown: new Cooldown(4000, 400), coolDownOffset: 2000)
                    )
                )
        #endregion Corrupted Golem B

        #region Corrupted Golem C
            .Init("Corrupted Golem C",
                new State(
                    new ScaleHP(0.1),
                    new TransformOnDeath("Headless Corrupted Golem C"),
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new State("normal or ultra",
                        new EntityExistsTransition("AHU Aldragine", 30, "ultra"),
                        new EntityExistsTransition("AH Aldragine", 30, "normal"),
                        new TimedTransition(200, "normal")
                        ),
                    new State("normal",
                        new State("n fight",
                            new Orbit(0.25, 3.5, target: "AH Aldragine", speedVariance: 0.1, radiusVariance: 0.5, orbitClockwise: null),
                            new Grenade(2, 25, 6, coolDown: new Cooldown(1000, 400), effect: ConditionEffectIndex.ArmorBroken, effectDuration: 2000, color: 0x00ff00),
                            new Shoot(10, count: 1, coolDown: new Cooldown(800, 100)),
                            new HpLessTransition(0.15, "n death")
                            ),
                        new State("n death",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Flash(0x00ff00, 0.7, 3),
                            new TimedTransition(2000, "n suicide")
                            ),
                        new State("n suicide",
                            new Shoot(20, count: 5, projectileIndex: 1, coolDown: 99999),
                            new Suicide()
                            )
                        ),
                    new State("ultra",
                        new State("u fight",
                            new Orbit(0.25, 2.5, target: "AHU Aldragine", speedVariance: 0.1, radiusVariance: 0.5, orbitClockwise: null),
                            new Grenade(2, 50, 8, coolDown: new Cooldown(2000, 500), effect: ConditionEffectIndex.ArmorBroken, effectDuration: 1500, color: 0x00ff00),
                            new Shoot(10, count: 2, shootAngle: 12, coolDown: new Cooldown(1200, 300)),
                            new HpLessTransition(0.15, "u death")
                            ),
                        new State("u death",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Flash(0x00ff00, 0.7, 3),
                            new Shoot(20, count: 5, shootAngle: 15, coolDown: 550, coolDownOffset: 1000),
                            new TimedTransition(2000, "u suicide")
                            ),
                        new State("u suicide",
                            new Shoot(20, count: 7, projectileIndex: 1, coolDown: 99999),
                            new Suicide()
                            )
                        )
                    )
                )

            .Init("Headless Corrupted Golem C",
                new State(
                    new ScaleHP(0.1),
                    new ConditionalEffect(ConditionEffectIndex.ArmorBroken),
                    new State("fight",
                        new Shoot(7, shootAngle: 5, count: 6, projectileIndex: 0, coolDown: 1200),
                        new State("charge",
                            new Prioritize(
                                new Follow(1, range: 7),
                                new Wander(0.15)
                                ),
                            new TimedTransition(2900, "orbit")
                            ),
                        new State("orbit",
                            new Orbit(0.7, 3.8, orbitClockwise: null),
                            new TimedTransition(3400, "charge")
                            )
                        )
                    )
                )
        #endregion Corrupted Golem C

    #endregion golems

    #region Construct of Zol
            .Init("Construct of Zol",
                new State(
                    new ScaleHP(0.1),
                    new OnDeathBehavior(new Spawn("Niolru", 2, 1, 99999)),
                    new TransformOnDeath("Demon of the Dark"),
                    new State("1",
                        new SetAttackTarget("AH The Vision", 20),
                        new SetAttackTarget("AHU The Vision", 20),
                        new Orbit(0.2, 6, 20),
                        new Shoot(12, count: 2, angleOffset: 90, projectileIndex: 0, coolDown: 3800, coolDownOffset: 2000),
                        new EntitiesNotExistsTransition(20, "boss died", "AHU The Vision", "AH The Vision")
                        ),
                    new State("boss died",
                        new SpiralShoot(15, 2, numShots: 12, projectileIndex: 1, coolDown: 6000)
                        )
                    )
                )
    #endregion Construct of Zol

    #region Beacons and Heart Logic
            .Init("Beacon of Zol A",
                new State(
                    new SetNoXP(),
                    new TransformOnDeath("Beacon of Zol C"),
                    new State("active",
                        new NoPlayerWithinTransition(3.4, "transform")
                        ),
                    new State("transform",
                        new Transform("Beacon of Zol B")
                        ),
                    new State("transform2",
                        new Transform("Beacon of Zol C")
                        ),
                    new State("no transform")
                    )
                )

            .Init("Beacon of Zol B",
                new State(
                    new SetNoXP(),
                    new ConditionalEffect(ConditionEffectIndex.Invisible),
                    new State("idle",
                        new PlayerWithinTransition(3.4, "transform")
                        ),
                    new State("transform",
                        new Transform("Beacon of Zol A")
                        ),
                    new State("transform2",
                        new Transform("Beacon of Zol C")
                        ),
                    new State("no transform")
                    )
                )

            .Init("Beacon of Zol C",
                new State(
                    new SetNoXP(),
                    new ConditionalEffect(ConditionEffectIndex.Invisible)
                    )
                )

            .Init("AH The Heart Logic",
                new State(
                    new State("first beacon",
                        new AllyCountLessTransition("Beacon of Zol A", 999, "that was close", 4)
                        ),
                    new State("that was close",
                        new Taunt("STAY ON YOUR PLATES, OR BE DOOMED! THIS IS YOUR FINAL WARNING!"),
                        new ReplaceTile("Zol Aura Dormant", "Zol Aura", 999),
                        new OrderAlly(999, "Beacon of Zol B", "transform2"),
                        new TransitionOnTick("second beacon")
                        ),
                    new State("second beacon",
                        new AllyCountLessTransition("Beacon of Zol A", 999, "failed", 3)
                        ),
                    new State("failed",
                        new OrderOnce(999, "AH The Heart", "Failed"),
                        new OrderOnce(999, "AHU The Heart", "Failed"),
                        new OrderAlly(999, "Beacon of Zol A", "transform2"),
                        new OrderAlly(999, "Beacon of Zol B", "transform2"),
                        new Suicide()
                        ),
                    new State("success",
                        new ReplaceTile("Zol Aura", "Zol Aura Dormant", 999),
                        new OrderAlly(999, "Beacon of Zol A", "no transform"),
                        new OrderAlly(999, "Beacon of Zol B", "no transform"),
                        new Suicide()
                        )
                    )
                )
    #endregion Beacons and Heart Logic

    #region Stone of Zol
            .Init("Stone of Zol",
                new State(
                    new ScaleHP(0.1),
                    new State("vulnerable",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 0),
                        new Shoot(10, count: 8, projectileIndex: 0, coolDown: 2500, coolDownOffset: 1000),
                        new HpLessTransition(0.15, "death")
                        ),
                    new State("invulnerable",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(10, count: 8, projectileIndex: 0, coolDown: 2500, coolDownOffset: 1000)
                        ),
                    new State("spawn random normal",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(15, count: 3, coolDown: 5600),
                        new Shoot(15, count: 4, coolDown: 5600, coolDownOffset: 2800),
                        new SpawnRandom(3, new[] { "Demon of the Dark", "Cleric of Zol", "Zol Bomber", "Brute of the Hideout", "Small Zol Slime", "Cube of Zol", "Servant of Darkness" }, maxChildren: 5, coolDown: 15000, coolDownOffset: 3500)
                        ),
                    new State("spawn random ultra",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(15, count: 5, coolDown: 6400),
                        new Shoot(15, count: 4, coolDown: 6400, coolDownOffset: 3200),
                        new SpawnRandom(3, new[] { "Niolru", "Demon of the Dark", "Cleric of Zol", "Zol Bomber", "Brute of the Hideout", "Small Zol Slime", "Cube of Zol", "Servant of Darkness", "Servant of Darkness" }, maxChildren: 6, coolDown: 15000, coolDownOffset: 3500)
                        ),
                    new State(
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new State("death",
                            new Flash(0xFF00FF, 1, 1),
                            new TimedTransition(1600, "suicide")
                            ),
                        new State("suicide",
                            new Shoot(10, count: 12, projectileIndex: 0, coolDown: 9999),
                            new Suicide()
                            )
                        )
                    )
                )
    #endregion Stone of Zol

    #region Hideout Destruction Mechanism
            .Init("Hideout Destruction Mechanism Spawner",
                new State(
                    new State("wait"),
                    new State("activate",
                        new Transform("Hideout Destruction Mechanism Inactive")
                        )
                    )
                )

            .Init("Hideout Destruction Mechanism Inactive",
                new State(
                    new State("first",
                        new PlayerWithinTransition(1, "transform")
                        ),
                    new State("transform",
                        new Transform("Hideout Destruction Mechanism Active")
                        ),
                    new State("suicide",
                        new Suicide()
                        )
                    )
                )

            .Init("Hideout Destruction Mechanism Active",
                new State(
                    new State("active",
                        new NoPlayerWithinTransition(1, "transform"),
                        new State("default"),
                        new State("spawn normal",
                            new State("s n s1",
                                new InvisiToss("Headless Corrupted Golem C", 5, 90, count: 2, angleOffset: 180, coolDown: 9999999, coolDownOffset: 1500),
                                new EntityExistsTransition("Headless Corrupted Golem C", 20, "s n w1")
                                ),
                            new State("s n w1",
                                new InvisiToss("Demon of the Dark", 6, angle: 45, count: 2, angleOffset: 180, coolDown: 8000, coolDownOffset: 0),
                                new InvisiToss("Demon of the Dark", 6, angle: 135, count: 2, angleOffset: 180, coolDown: 8000, coolDownOffset: 4000),
                                new EntityNotExistsTransition("Headless Corrupted Golem C", 40, "s n s2")
                                ),
                            new State("s n s2",
                                new InvisiToss("Headless Corrupted Golem C", 5, 0, count: 2, angleOffset: 180, coolDown: 9999999, coolDownOffset: 1500),
                                new EntityExistsTransition("Headless Corrupted Golem C", 20, "s n w2")
                                ),
                            new State("s n w2",
                                new InvisiToss("Niolru", 6, angle: 135, count: 2, angleOffset: 180, coolDown: 8000, coolDownOffset: 0),
                                new InvisiToss("Niolru", 6, angle: 45, count: 2, angleOffset: 180, coolDown: 8000, coolDownOffset: 4000),
                                new EntityNotExistsTransition("Headless Corrupted Golem C", 40, "s n s3")
                                ),
                            new State("s n s3",
                                new InvisiToss("Headless Corrupted Golem C", 5, 90, count: 2, angleOffset: 180, coolDown: 9999999, coolDownOffset: 1500),
                                new EntityExistsTransition("Headless Corrupted Golem C", 20, "s n w3")
                                ),
                            new State("s n w3",
                                new InvisiToss("Demon of the Dark", 6, angle: 45, count: 2, angleOffset: 180, coolDown: 8000, coolDownOffset: 0),
                                new InvisiToss("Demon of the Dark", 6, angle: 135, count: 2, angleOffset: 180, coolDown: 8000, coolDownOffset: 4000),
                                new EntityNotExistsTransition("Headless Corrupted Golem C", 40, "s n s4")
                                ),
                            new State("s n s4",
                                new InvisiToss("Headless Corrupted Golem C", 5, 0, count: 2, angleOffset: 180, coolDown: 9999999, coolDownOffset: 1500),
                                new EntityExistsTransition("Headless Corrupted Golem C", 20, "s n w4")
                                ),
                            new State("s n w4",
                                new InvisiToss("Niolru", 6, angle: 135, count: 2, angleOffset: 180, coolDown: 8000, coolDownOffset: 0),
                                new InvisiToss("Niolru", 6, angle: 45, count: 2, angleOffset: 180, coolDown: 8000, coolDownOffset: 4000),
                                new EntityNotExistsTransition("Headless Corrupted Golem C", 40, "s n s5")
                                ),
                            new State("s n s5",
                                new InvisiToss("Headless Corrupted Golem C", 5, 90, count: 2, angleOffset: 180, coolDown: 9999999, coolDownOffset: 1500),
                                new EntityExistsTransition("Headless Corrupted Golem C", 20, "s n w5")
                                ),
                            new State("s n w5",
                                new InvisiToss("Demon of the Dark", 6, angle: 45, count: 2, angleOffset: 180, coolDown: 8000, coolDownOffset: 0),
                                new InvisiToss("Demon of the Dark", 6, angle: 135, count: 2, angleOffset: 180, coolDown: 8000, coolDownOffset: 4000),
                                new EntityNotExistsTransition("Headless Corrupted Golem C", 40, "after spawn")
                                )
                            ),
                        new State("spawn ultra",
                            new State("s u s1",
                                new InvisiToss("Headless Corrupted Golem C", 5, 180, count: 1, angleOffset: 120, coolDown: 9999999),
                                new EntityExistsTransition("Headless Corrupted Golem C", 20, "s u w1")
                                ),
                            new State("s u w1",
                                new InvisiToss("Demon of the Dark", 6, angle: 45, count: 1, angleOffset: 180, coolDown: 8000, coolDownOffset: 0),
                                new InvisiToss("Demon of the Dark", 6, angle: 135, count: 1, angleOffset: 180, coolDown: 8000, coolDownOffset: 4000),
                                new EntityNotExistsTransition("Headless Corrupted Golem C", 40, "s u s2")
                                ),
                            new State("s u s2",
                                new InvisiToss("Headless Corrupted Golem C", 5, 0, count: 1, angleOffset: 1200, coolDown: 9999999),
                                new EntityExistsTransition("Headless Corrupted Golem C", 20, "s u w2")
                                ),
                            new State("s u w2",
                                new InvisiToss("AH Zol Incarnation", 6, angle: 0, count: 1, angleOffset: 180, coolDown: 8000, coolDownOffset: 0),
                                new InvisiToss("AH Zol Incarnation", 6, angle: 90, count: 1, angleOffset: 180, coolDown: 8000, coolDownOffset: 4000),
                                new EntityNotExistsTransition("Headless Corrupted Golem C", 40, "s u s3")
                                ),
                            new State("s u s3",
                                new InvisiToss("Headless Corrupted Golem C", 5, 180, count: 1, angleOffset: 120, coolDown: 9999999),
                                new EntityExistsTransition("Headless Corrupted Golem C", 20, "s u w3")
                                ),
                            new State("s u w3",
                                new InvisiToss("Niolru", 6, angle: 45, count: 1, angleOffset: 180, coolDown: 8000, coolDownOffset: 0),
                                new InvisiToss("Niolru", 6, angle: 135, count: 1, angleOffset: 180, coolDown: 8000, coolDownOffset: 4000),
                                new EntityNotExistsTransition("Headless Corrupted Golem C", 40, "after spawn")
                                )
                            ),
                        new State("after spawn",
                            new GroupNotExistTransition(50, "success", "Zol Minions")
                            )
                        ),
                    new State("success",
                        new OrderOnce(40, "AH Aldragine", "Done"),
                        new OrderOnce(40, "AHU Aldragine", "Done"),
                        new ReplaceTile("Zol Aura", "Zol Aura Dormant", 40),
                        new TimedTransition(100, "done")
                        ),
                    new State("done"),
                    new State("transform",
                        new Transform("Hideout Destruction Mechanism Inactive")
                        ),
                    new State("suicide",
                        new Suicide()
                        )
                    )
                )
    #endregion Hideout Destruction Mechanism

    #region Crystal of Corruption
            .Init("AH Crystal of Corruption",
                new State(
                    new ScaleHP(0.3),
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new PlayerWithinTransition(5, "Activate")
                        ),
                    new State("Activate",
                        new TimedTransition(3000, "Vulnerable")
                        ),
                    new State("Vulnerable",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 0)
                        )
                    ),
                new Threshold(0.001,
                    new ItemLoot("Star of Corruption", 0.0005),
                    new ItemLoot("Death Essence Trap", 0.0005),
                    new ItemLoot("Staff of Consumption", 0.0005),
                    new ItemLoot("Zol Striker", 0.0005),
                    new ItemLoot("Crossbow of the Legion", 0.0005),
                    new ItemLoot("Chaotic Vanguard", 0.0005),
                    new ItemLoot("Rusted Light Abilities Chest", 0.0005),
                    new ItemLoot("Rusted Medium Abilities Chest", 0.0005),
                    new ItemLoot("Rusted Heavy Abilities Chest", 0.0005),
                    new ItemLoot("Master Eon", 0.001),
                    new ItemLoot("Vial of Life", 0.01),
                    new ItemLoot("Vial of Mana", 0.01),
                    new ItemLoot("Vial of Attack", 0.01),
                    new ItemLoot("Vial of Speed", 0.01),
                    new ItemLoot("Vial of Defense", 0.01),
                    new ItemLoot("Vial of Dexterity", 0.01),
                    new ItemLoot("Vial of Vitality", 0.01),
                    new ItemLoot("Vial of Wisdom", 0.01),
                    new ItemLoot("Vial of Luck", 0.00625),
                    new ItemLoot("Vial of Restoration", 0.00625)
                    )
                );
    #endregion Crystal of Corruption

#endregion both
    }
}
