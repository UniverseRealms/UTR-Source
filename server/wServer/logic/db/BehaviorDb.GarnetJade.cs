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
        private _ GarnetandJade = () => Behav()
        /*
         *
         *
         *
         * GARNET AND JADE STATUE BEHAVIORS
         *    ---- Created by Png----
         *   Don't give this out :P
         *
         *
         */
        #region Garnet Statue
        /*.Init("Garnet Statue",
            new State(
                new State("Wait",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                ),
                new State("Activate",
                    new TimedTransition(200, "RemINVINC")
                ),
                new State("RemINVINC",
                    new Flash(0xffffff, 2, 100),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new TimedTransition(2000, "Shotgun")
                ),
                new State("FlashRING",
                    new Flash(0xd40000, 2, 100),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new TimedTransition(2000, "RingCharge"),
                    new EntitiesNotExistsTransition(50, "JadeDied", "Jade Statue")
                ),
                new State("RingCharge",
                    new Follow(1.8, range: 1, duration: 5000, coolDown: 0),
                    new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 5),
                    new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 220),
                    new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 420), //smonk :3
                    new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 620),
                    new TimedTransition(800, "ChooseRandom"),
                    new EntitiesNotExistsTransition(50, "JadeDied", "Jade Statue")
                ),
                new State("Shotgun",
                    new Follow(.7, range: 1, duration: 5000, coolDown: 0),
                    new Shoot(10, count: 5, predictive: 1, shootAngle: 5, coolDown: 500, projectileIndex: 0),
                    //new Shoot(10, count: 1, predictive: 1, shootAngle: 5, coolDown: 1000, projectileIndex: 4),
                    new TimedTransition(3700, "ChooseRandom"),
                    new EntitiesNotExistsTransition(50, "JadeDied", "Jade Statue")
                ),
                new State("Singular",
                    new Follow(.7, range: 1, duration: 5000, coolDown: 0),
                    new Shoot(10, count: 1, predictive: 1, coolDown: 400, projectileIndex: 0),
                    new TimedTransition(1800, "ChooseRandom"),
                    new EntitiesNotExistsTransition(50, "JadeDied", "Jade Statue")
                ),
                new State("PetRing",
                    new Shoot(10, count: 20, shootAngle: 18, coolDown: 400, projectileIndex: 4),
                    new TimedTransition(200, "ChooseRandom"),
                    new EntitiesNotExistsTransition(50, "JadeDied", "Jade Statue")
                ),
                new State("Spawn",
                    new TossObject("Corrupted Spirit", 5, coolDown: 8000),
                    new TossObject("Corrupted Spirit", 5, coolDown: 8000),
                    new TimedTransition(700, "ChooseRandom")
                ),
                new State("ChooseRandom",
                    new Flash(0xffffff, 2, 100),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new TimedTransition(1200, "Singular", true),
                    new TimedTransition(1200, "FlashRING", true),
                    new TimedTransition(1200, "Shotgun", true),
                    new TimedTransition(1200, "PetRing", true),
                    new TimedTransition(1200, "Spawn", true),
                    new EntitiesNotExistsTransition(50, "JadeDied", "Jade Statue")
                ),
                new State("JadeDied",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new HealSelf(coolDown: 999999, amount: 7000),
                    new ChangeSize(20, 130),
                    new Flash(0xffffff, 2, 100),
                    new TimedTransition(1500, "ChooseRandomV2")
                ),
                new State("ChooseRandomV2",
                    new Flash(0xffffff, 2, 100),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new TimedTransition(1200, "SingularV2", true),
                    new TimedTransition(1200, "FlashRINGV2", true),
                    new TimedTransition(1200, "ShotgunV2", true),
                    new TimedTransition(1200, "PetRingV2", true),
                    new TimedTransition(1200, "ShotgunWIDER", true),
                    new TimedTransition(1200, "Swirl", true),
                    new TimedTransition(1200, "SpawnV2", true)
                ),
                new State("SpawnV2",
                    new TossObject("Corrupted Spirit", 5, coolDown: 8000),
                    new TossObject("Corrupted Spirit", 5, coolDown: 8000),
                    new TimedTransition(700, "ChooseRandomV2")
                ),
                new State("FlashRINGV2",
                    new Flash(0xd40000, 2, 100),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new TimedTransition(2000, "RingChargeV2")
                ),
                new State("RingChargeV2",
                    new Follow(1.8, range: 1, duration: 5000, coolDown: 0),
                    new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 5),
                    new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 220),
                    new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 420), //smonk :3
                    new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 620),
                    new TimedTransition(800, "ChooseRandomV2")
                ),
                new State("ShotgunV2",
                    new Follow(.7, range: 1, duration: 5000, coolDown: 0),
                    new Shoot(10, count: 5, predictive: 1, shootAngle: 5, coolDown: 500, projectileIndex: 0),
                    //new Shoot(10, count: 1, predictive: 1, shootAngle: 5, coolDown: 580, projectileIndex: 3),
                    new TimedTransition(2300, "ChooseRandomV2")
                ),
                new State("ShotgunWIDER",
                    new Shoot(10, count: 5, predictive: 1, shootAngle: 5, coolDown: 99999, projectileIndex: 0, coolDownOffset: 50),
                    new Shoot(10, count: 1, predictive: 1, shootAngle: 5, coolDown: 99999, projectileIndex: 3, coolDownOffset: 50),
                    //
                    new Shoot(10, count: 10, predictive: 1, shootAngle: 5, coolDown: 99999, projectileIndex: 0, coolDownOffset: 500),
                    new Shoot(10, count: 1, predictive: 1, shootAngle: 5, coolDown: 99999, projectileIndex: 3, coolDownOffset: 500),
                    //
                    new Shoot(10, count: 15, predictive: 1, shootAngle: 5, coolDown: 99999, projectileIndex: 0, coolDownOffset: 1000),
                    new Shoot(10, count: 1, predictive: 1, shootAngle: 5, coolDown: 99999, projectileIndex: 3, coolDownOffset: 1000),
                    new TimedTransition(1200, "ChooseRandomV2")
                ),
                new State("SingularV2",
                    new Follow(.7, range: 1, duration: 5000, coolDown: 0),
                    new Shoot(10, count: 1, predictive: 1, coolDown: 400, projectileIndex: 0),
                    new TimedTransition(2500, "ChooseRandomV2")
                ),
                new State("PetRingV2",
                    new Shoot(10, count: 20, shootAngle: 18, fixedAngle: 0, coolDown: 9400, projectileIndex: 4),
                    new Shoot(10, count: 20, shootAngle: 18, fixedAngle: 10, coolDown: 9400, projectileIndex: 4, coolDownOffset: 100),
                    new Shoot(10, count: 20, shootAngle: 18, fixedAngle: 20, coolDown: 9400, projectileIndex: 4, coolDownOffset: 300),
                    new TimedTransition(200, "ChooseRandomV2")
                ),
                new State("Swirl",
                    new Shoot(10, count: 2, shootAngle: 20, coolDown: 99999, fixedAngle: 0, projectileIndex: 4, coolDownOffset: 50),
                    new Shoot(10, count: 2, shootAngle: 20, coolDown: 99999, fixedAngle: 90, projectileIndex: 4, coolDownOffset: 200),
                    new Shoot(10, count: 2, shootAngle: 20, coolDown: 99999, fixedAngle: 180, projectileIndex: 4, coolDownOffset: 400),
                    new Shoot(10, count: 2, shootAngle: 20, coolDown: 99999, fixedAngle: 270, projectileIndex: 4, coolDownOffset: 600),
                    new Shoot(10, count: 2, shootAngle: 20, coolDown: 99999, fixedAngle: 45, projectileIndex: 4, coolDownOffset: 800),
                    new Shoot(10, count: 2, shootAngle: 20, coolDown: 99999, fixedAngle: 135, projectileIndex: 4, coolDownOffset: 1000),
                    new Shoot(10, count: 2, shootAngle: 20, coolDown: 99999, fixedAngle: 225, projectileIndex: 4, coolDownOffset: 1200),
                    new Shoot(10, count: 2, shootAngle: 20, coolDown: 99999, fixedAngle: 315, projectileIndex: 4, coolDownOffset: 1400),
                    new TimedTransition(1500, "ChooseRandomV2")
                )
            ),
           new MostDamagers(3,
                    LootTemplates.Sor3Perc()
                ),
           new MostDamagers(5,
                LootTemplates.StatPots()
            ),
            new MostDamagers(1,
                new ItemLoot("Kageboshi", 0.01)
            )
        )*/
        .Init("Garnet Statue",
            new State(
                new ScaleHP(0.3),
                new State("Idol",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("Thinking",
                    new TimedTransition(500, "Thinking2")
                    ),
                new State("Thinking2",
                    new TimedTransition(500, "Thinking3")
                    ),
                new State("Thinking3",
                    new Taunt("What should we do brother?!"),
                    new TimedTransition(1000, "Maybe")
                    ),
                new State("Maybe",
                    new TimedTransition(500, "Maybe2")
                    ),
                new State("Maybe2",
                    new Taunt("Maybe We'll just crush you under our Unearthly mass!"),
                    new TimedTransition(1000, "PreAttack")
                    ),
                new State("PreAttack",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, false),
                    new TimedTransition(250, "BeginAttack")
                    ),
                new State("BeginAttack",
                    new HpLessTransition(0.8, "you'vedoneit"),
                    new Orbit(0.5, 5, target: "Encounter Altar"),
                    new Shoot(10, count: 5, predictive: 0.6, shootAngle: 5, coolDown: 500, projectileIndex: 0),
                    new TimedTransition(2000, "Follow")
                    ),
                new State("Follow",
                    new HpLessTransition(0.8, "you'vedoneit"),
                    new Follow(0.6, range: 1, duration: 5000, coolDown: 0),
                    new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 5),
                    new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 220),
                    new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 420), //smonk :3
                    new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 620),
                    new TimedTransition(4000, "BeginAttack")
                    ),
                new State("you'vedoneit",
                    new HpLessTransition(0.5, "ImBleeding"),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, false),
                    new Follow(1, range: 1, duration: 5000, coolDown: 0),
                    new Shoot(10, count: 1, predictive: 0.6, coolDown: 400, projectileIndex: 0),
                    new TossObject("Corrupted Spirit", 5, coolDown: 8000),
                    new TossObject("Corrupted Spirit", 5, coolDown: 8000),
                    new TimedTransition(2500, "ReturnToSpawn")
                    ),
                new State("ReturnToSpawn",
                    new HpLessTransition(0.5, "ImBleeding"),
                    new StayCloseToSpawn(2, 1),
                    new TimedTransition(2000, "BacktoTheStart")
                    ),
                new State("BacktoTheStart",
                    new HpLessTransition(0.5, "ImBleeding"),
                    new Orbit(0.5, 3, target: "Encounter Altar"),
                    new Shoot(10, count: 1, predictive: 0.6, coolDown: 400, projectileIndex: 0),
                    new TimedTransition(2000, "you'vedoneit")
                    ),
                new State("ImBleeding",
                    new EntityNotExistsTransition("Jade Statue", 200, "Jump?"),
                    new StayCloseToSpawn(2, 1),
                    new TimedTransition(1000, "PreEnrage")
                    ),
                new State("PreEnrage",
                    new EntityNotExistsTransition("Jade Statue", 200, "Jump?"),
                    new ChangeSize(50, 100),
                    new Shoot(10, count: 5, predictive: 0.6, shootAngle: 5, coolDown: 500, projectileIndex: 0),
                    new TimedTransition(5000, "Attack")
                    ),
                new State("GrowBigger",
                    new EntityNotExistsTransition("Jade Statue", 200, "Jump?"),
                    new ChangeSize(50, 200),
                    new Flash(0xff5e00, 2, 1),
                    new TimedTransition(2000, "PreEnrage")
                    ),
                new State("Attack",
                    new EntityNotExistsTransition("Jade Statue", 200, "Jump?"),
                    new Orbit(0.2, 3, target: "Encounter Altar"),
                    new Shoot(10, count: 8, predictive: 0.6, coolDownOffset: 0, projectileIndex: 0, rotateAngle: 10, fixedAngle: 0, coolDown: 400),
                    new TimedTransition(5000, "GrowBigger")
                    ),
                new State("Jump?",
                    new Taunt("Fine... I'll DESTROY YOU"),
                    new TimedTransition(2000, "JUMP")
                    ),
                new State("JUMP",
                    new Follow(1, range: 1, duration: 5000, coolDown: 0),
                    new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 5),
                    new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 220),
                    new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 420), //smonk :3
                    new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 620),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, false),
                    new TimedTransition(2500, "Huff")
                    ),
                new State("Huff",
                    new Taunt(text: "HUFF...", cooldown: 900),
                    new TimedTransition(3000, "JUMP")
                    )
                ),
            new MostDamagers(3,
                    LootTemplates.Sor3Perc()
                ),
           new MostDamagers(5,
                LootTemplates.GreaterPots()
            ),
            new Threshold(0.1,
                new ItemLoot("Legrundy Essence", 0.0005),
                new ItemLoot("Kageboshi", 0.01),
                new ItemLoot("Sacred Essence", 0.0005),
                new ItemLoot("Garnet's Onslaught", 0.001)
            )
            )
        #endregion
        #region Jade Statue

        .Init("Jade Statue",
            new State(
                new ScaleHP(0.3),
                new State("Idol",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("Thinking",
                    new Taunt("Hmmm.."),
                    new TimedTransition(500, "Thinking2")
                    ),
                new State("Thinking2",
                    new Taunt("The Adventurers are here!"),
                    new TimedTransition(500, "Thinking3")
                    ),
                new State("Thinking3",
                    new TimedTransition(500, "Maybe")
                    ),
                new State("Maybe",
                    new Taunt("Maybe we could skin them."),
                    new TimedTransition(1000, "Maybe2")
                    ),
                new State("Maybe2",
                    new TimedTransition(1000, "PreAttack")
                    ),
                new State("PreAttack",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, false),
                    new TimedTransition(250, "BeginAttack")
                    ),
                new State("BeginAttack",
                    new HpLessTransition(0.8, "you'vedoneit"),
                    new Orbit(0.5, 3, target: "Encounter Altar"),
                    new Shoot(10, count: 5, predictive: 0.6, shootAngle: 5, coolDown: 500, projectileIndex: 0),
                    new TimedTransition(2000, "Follow")
                    ),
                new State("Follow",
                    new HpLessTransition(0.8, "you'vedoneit"),
                    new Follow(0.6, range: 1, duration: 5000, coolDown: 0),
                    new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 5),
                    new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 220),
                    new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 420), //smonk :3
                    new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 620),
                    new TimedTransition(4000, "BeginAttack")
                    ),
                new State("you'vedoneit",
                    new HpLessTransition(0.5, "ImBleeding"),
                    new Follow(1, range: 1, duration: 5000, coolDown: 0),
                    new Shoot(10, count: 1, predictive: 0.6, coolDown: 400, projectileIndex: 0),
                    new TossObject("Corrupted Sprite", 5, coolDown: 8000),
                    new TossObject("Corrupted Sprite", 5, coolDown: 8000),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, false),
                    new TimedTransition(2500, "ReturnToSpawn")
                    ),
                new State("ReturnToSpawn",
                    new HpLessTransition(0.5, "ImBleeding"),
                    new StayCloseToSpawn(2, 1),
                    new TimedTransition(2000, "BacktoTheStart")
                    ),
                new State("BacktoTheStart",
                    new HpLessTransition(0.5, "ImBleeding"),
                    new Orbit(0.5, 3, target: "Encounter Altar"),
                    new Shoot(10, count: 1, predictive: 0.6, coolDown: 400, projectileIndex: 0),
                    new TimedTransition(2000, "you'vedoneit")
                    ),
                new State("ImBleeding",
                    new EntityNotExistsTransition("Garnet Statue", 200, "Jump?"),
                    new StayCloseToSpawn(2, 1),
                    new TimedTransition(1000, "GrowBigger")
                    ),
                new State("GrowBigger",
                    new EntityNotExistsTransition("Garnet Statue", 200, "Jump?"),
                    new ChangeSize(50, 200),
                    new Flash(0xff5e00, 2, 1),
                    new TimedTransition(2000, "Attack")
                    ),
                new State("Attack",
                    new EntityNotExistsTransition("Garnet Statue", 200, "Jump?"),
                    new Orbit(0.2, 3, target: "Encounter Altar"),
                    new Shoot(10, count: 8, predictive: 0.6, coolDownOffset: 0, projectileIndex: 0, rotateAngle: 10, fixedAngle: 0, coolDown: 400),
                    new TimedTransition(5000, "PreEnrage")
                    ),
                new State("PreEnrage",
                    new EntityNotExistsTransition("Garnet Statue", 200, "Jump?"),
                    new ChangeSize(50, 100),
                    new Shoot(10, count: 5, predictive: 0.6, shootAngle: 5, coolDown: 500, projectileIndex: 0),
                    new TimedTransition(5000, "Attack")
                    ),
                new State("Jump?",
                    new Taunt("Fine... I'll Jump"),
                    new TimedTransition(2000, "JUMP")
                    ),
                new State("JUMP",
                    new Follow(1, range: 1, duration: 5000, coolDown: 0),
                    new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 5),
                    new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 220),
                    new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 420), //smonk :3
                    new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 620),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, false),
                    new TimedTransition(2500, "Huff")
                    ),
                new State("Huff",
                    new Taunt(text: "HUFF...", cooldown: 900),
                    new TimedTransition(3000, "JUMP")
                    )
                ),
            new MostDamagers(3,
                        LootTemplates.Sor3Perc()
                    ),
                new MostDamagers(5,
                    LootTemplates.GreaterPots()
                ),
                new Threshold(0.1,
                    new ItemLoot("Wand of the Fallen", 0.01),
                    new ItemLoot("Legrundy Essence", 0.0005),
                    new ItemLoot("Sacred Essence", 0.0005),
                    new ItemLoot("Jade's Judgment", 0.001)
                )
            )

            /*.Init("Jade Statue",
				new State(
					new State("Wait",
						new ConditionalEffect(ConditionEffectIndex.Invincible, true)
					),
					new State("Activate",
						new TimedTransition(200, "RemINVINC")
					),
					new State("RemINVINC",
						new Flash(0xffffff, 2, 100),
						new ConditionalEffect(ConditionEffectIndex.Invincible),
						new TimedTransition(2000, "Shotgun")
					),
					new State("FlashRING",
						new Flash(0xd40000, 2, 100),
						new ConditionalEffect(ConditionEffectIndex.Invulnerable),
						new TimedTransition(2000, "RingCharge"),
						new EntitiesNotExistsTransition(50, "GarnetDied", "Garnet Statue")
					),
					new State("RingCharge",
						new Follow(1.8, range: 1, duration: 5000, coolDown: 0),
						new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 5),
						new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 220),
						new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 420), //smonk :3
						new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 620),
						new TimedTransition(800, "ChooseRandom"),
						new EntitiesNotExistsTransition(50, "GarnetDied", "Garnet Statue")
					),
					new State("Shotgun",
						new Follow(.7, range: 1, duration: 5000, coolDown: 0),
						new Shoot(10, count: 5, predictive: 1, shootAngle: 5, coolDown: 500, projectileIndex: 0),
						//new Shoot(10, count: 1, predictive: 1, shootAngle: 5, coolDown: 1000, projectileIndex: 4),
						new TimedTransition(3700, "ChooseRandom"),
						new EntitiesNotExistsTransition(50, "GarnetDied", "Garnet Statue")
					),
					new State("Singular",
						new Follow(.7, range: 1, duration: 5000, coolDown: 0),
						new Shoot(10, count: 1, predictive: 1, coolDown: 400, projectileIndex: 0),
						new TimedTransition(1800, "ChooseRandom"),
						new EntitiesNotExistsTransition(50, "GarnetDied", "Garnet Statue")
					),
					new State("PetRing",
						new Shoot(10, count: 20, shootAngle: 18, coolDown: 400, projectileIndex: 4),
						new TimedTransition(200, "ChooseRandom"),
						new EntitiesNotExistsTransition(50, "GarnetDied", "Garnet Statue")
					),
					new State("Spawn",
						new TossObject("Corrupted Sprite", 5, coolDown: 8000),
						new TossObject("Corrupted Sprite", 5, coolDown: 8000),
						new TimedTransition(700, "ChooseRandom")
					),
					new State("ChooseRandom",
						new Flash(0xffffff, 2, 100),
						new ConditionalEffect(ConditionEffectIndex.Invulnerable),
						new TimedTransition(1200, "Singular", true),
						new TimedTransition(1200, "FlashRING", true),
						new TimedTransition(1200, "Shotgun", true),
						new TimedTransition(1200, "PetRing", true),
						new TimedTransition(1200, "Spawn", true),
						new EntitiesNotExistsTransition(50, "GarnetDied", "Garnet Statue")
					),
					new State("GarnetDied",
						new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new HealSelf(coolDown: 999999, amount: 7000),
                        new ChangeSize(20, 130),
						new Flash(0xffffff, 2, 100),
						new TimedTransition(1500, "ChooseRandomV2")
					),
					new State("ChooseRandomV2",
						new Flash(0xffffff, 2, 100),
						new ConditionalEffect(ConditionEffectIndex.Invulnerable),
						new TimedTransition(1200, "SingularV2", true),
						new TimedTransition(1200, "FlashRINGV2", true),
						new TimedTransition(1200, "ShotgunV2", true),
						new TimedTransition(1200, "PetRingV2", true),
						new TimedTransition(1200, "ShotgunWIDER", true),
						new TimedTransition(1200, "Swirl", true),
						new TimedTransition(1200, "SpawnV2", true)
					),
					new State("SpawnV2",
						new TossObject("Corrupted Sprite", 5, coolDown: 8000),
						new TossObject("Corrupted Sprite", 5, coolDown: 8000),
						new TimedTransition(700, "ChooseRandomV2")
					),
					new State("FlashRINGV2",
						new Flash(0xd40000, 2, 100),
						new ConditionalEffect(ConditionEffectIndex.Invulnerable),
						new TimedTransition(2000, "RingChargeV2")
					),
					new State("RingChargeV2",
						new Follow(1.8, range: 1, duration: 5000, coolDown: 0),
						new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 5),
						new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 220),
						new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 420), //smonk :3
						new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 620),
						new TimedTransition(800, "ChooseRandomV2")
					),
					new State("ShotgunV2",
						new Follow(.7, range: 1, duration: 5000, coolDown: 0),
						new Shoot(10, count: 5, predictive: 1, shootAngle: 5, coolDown: 500, projectileIndex: 0),
						//new Shoot(10, count: 1, predictive: 1, shootAngle: 5, coolDown: 580, projectileIndex: 3),
						new TimedTransition(2300, "ChooseRandomV2")
					),
					new State("ShotgunWIDER",
						new Shoot(10, count: 5, predictive: 1, shootAngle: 5, coolDown: 99999, projectileIndex: 0, coolDownOffset: 50),
						new Shoot(10, count: 1, predictive: 1, shootAngle: 5, coolDown: 99999, projectileIndex: 3, coolDownOffset: 50),
						//
						new Shoot(10, count: 10, predictive: 1, shootAngle: 5, coolDown: 99999, projectileIndex: 0, coolDownOffset: 500),
						new Shoot(10, count: 1, predictive: 1, shootAngle: 5, coolDown: 99999, projectileIndex: 3, coolDownOffset: 500),
						//
						new Shoot(10, count: 15, predictive: 1, shootAngle: 5, coolDown: 99999, projectileIndex: 0, coolDownOffset: 1000),
						new Shoot(10, count: 1, predictive: 1, shootAngle: 5, coolDown: 99999, projectileIndex: 3, coolDownOffset: 1000),
						new TimedTransition(1200, "ChooseRandomV2")
					),
					new State("SingularV2",
						new Follow(.7, range: 1, duration: 5000, coolDown: 0),
						new Shoot(10, count: 1, predictive: 1, coolDown: 400, projectileIndex: 0),
						new TimedTransition(2500, "ChooseRandomV2")
					),
					new State("PetRingV2",
						new Shoot(10, count: 20, shootAngle: 18, fixedAngle: 0, coolDown: 9400, projectileIndex: 4),
						new Shoot(10, count: 20, shootAngle: 18, fixedAngle: 10, coolDown: 9400, projectileIndex: 4, coolDownOffset: 100),
						new Shoot(10, count: 20, shootAngle: 18, fixedAngle: 20, coolDown: 9400, projectileIndex: 4, coolDownOffset: 300),
						new TimedTransition(200, "ChooseRandomV2")
					),
					new State("Swirl",
						new Shoot(10, count: 2, shootAngle: 20, coolDown: 99999, fixedAngle: 0, projectileIndex: 4, coolDownOffset: 50),
						new Shoot(10, count: 2, shootAngle: 20, coolDown: 99999, fixedAngle: 90, projectileIndex: 4, coolDownOffset: 200),
						new Shoot(10, count: 2, shootAngle: 20, coolDown: 99999, fixedAngle: 180, projectileIndex: 4, coolDownOffset: 400),
						new Shoot(10, count: 2, shootAngle: 20, coolDown: 99999, fixedAngle: 270, projectileIndex: 4, coolDownOffset: 600),
						new Shoot(10, count: 2, shootAngle: 20, coolDown: 99999, fixedAngle: 45, projectileIndex: 4, coolDownOffset: 800),
						new Shoot(10, count: 2, shootAngle: 20, coolDown: 99999, fixedAngle: 135, projectileIndex: 4, coolDownOffset: 1000),
						new Shoot(10, count: 2, shootAngle: 20, coolDown: 99999, fixedAngle: 225, projectileIndex: 4, coolDownOffset: 1200),
						new Shoot(10, count: 2, shootAngle: 20, coolDown: 99999, fixedAngle: 315, projectileIndex: 4, coolDownOffset: 1400),
						new TimedTransition(1500, "ChooseRandomV2")
					)
				),
               new MostDamagers(3,
                        LootTemplates.Sor3Perc()
                    ),
                new MostDamagers(5,
                    LootTemplates.StatPots()
                ),
                new MostDamagers(1,
                    new ItemLoot("Wand of the Fallen", 0.01)
                )
            )*/
        #endregion
        #region Misc
            .Init("Corrupted Sprite",
                new State(
                    new State("One",
                        new Follow(.4, range: 1, duration: 5000, coolDown: 0),
                        // 1
                        new Shoot(10, count: 1, predictive: 1, projectileIndex: 0, coolDown: 99999),
                        new Shoot(10, count: 1, predictive: 1, projectileIndex: 0, coolDown: 99999, coolDownOffset: 400),
                        // 2
                        new Shoot(10, count: 2, predictive: 1, shootAngle: 25, projectileIndex: 0, coolDown: 99999, coolDownOffset: 1200),
                        new Shoot(10, count: 2, predictive: 1, shootAngle: 25, projectileIndex: 0, coolDown: 99999, coolDownOffset: 1600),
                        // 8
                        new Shoot(10, count: 8, predictive: 1, shootAngle: 30, projectileIndex: 0, coolDown: 99999, coolDownOffset: 2300),
                        new Shoot(10, count: 8, predictive: 1, shootAngle: 30, projectileIndex: 0, coolDown: 99999, coolDownOffset: 2800),
                        // 2
                        new Shoot(10, count: 2, predictive: 1, shootAngle: 25, projectileIndex: 0, coolDown: 99999, coolDownOffset: 3200),
                        new Shoot(10, count: 2, predictive: 1, shootAngle: 25, projectileIndex: 0, coolDown: 99999, coolDownOffset: 3500),
                        // 1
                        new Shoot(10, count: 1, predictive: 1, projectileIndex: 0, coolDown: 99999, coolDownOffset: 4000),
                        new Shoot(10, count: 1, predictive: 1, projectileIndex: 0, coolDown: 99999, coolDownOffset: 4400),
                        new TimedTransition(10000, "KYS")
                    ),
                    new State("KYS",
                        new Suicide()
                    )
                )
            )
            .Init("Corrupted Spirit",
                new State(
                    new State("One",
                        new Follow(.4, range: 1, duration: 5000, coolDown: 0),
                        // 1
                        new Shoot(10, count: 1, predictive: 1, projectileIndex: 0, coolDown: 99999),
                        new Shoot(10, count: 1, predictive: 1, projectileIndex: 0, coolDown: 99999, coolDownOffset: 400),
                        // 2
                        new Shoot(10, count: 2, predictive: 1, shootAngle: 25, projectileIndex: 0, coolDown: 99999, coolDownOffset: 1200),
                        new Shoot(10, count: 2, predictive: 1, shootAngle: 25, projectileIndex: 0, coolDown: 99999, coolDownOffset: 1600),
                        // 8
                        new Shoot(10, count: 8, predictive: 1, shootAngle: 30, projectileIndex: 0, coolDown: 99999, coolDownOffset: 2300),
                        new Shoot(10, count: 8, predictive: 1, shootAngle: 30, projectileIndex: 0, coolDown: 99999, coolDownOffset: 2800),
                        // 2
                        new Shoot(10, count: 2, predictive: 1, shootAngle: 25, projectileIndex: 0, coolDown: 99999, coolDownOffset: 3200),
                        new Shoot(10, count: 2, predictive: 1, shootAngle: 25, projectileIndex: 0, coolDown: 99999, coolDownOffset: 3500),
                        // 1
                        new Shoot(10, count: 1, predictive: 1, projectileIndex: 0, coolDown: 99999, coolDownOffset: 4000),
                        new Shoot(10, count: 1, predictive: 1, projectileIndex: 0, coolDown: 99999, coolDownOffset: 4400),
                        new TimedTransition(10000, "KYS")
                    ),
                    new State("KYS",
                        new Suicide()
                    )
                )
            )
            .Init("Encounter Altar",
                new State(
                    //new DropPortalOnDeath("Mountain Temple", 40),
                    new State("Wait",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new PlayerWithinTransition(6, "IsPlayerNearG&J")
                    ),
                    new State("IsPlayerNearG&J",
                        new Order(100, "Jade Statue", "Thinking"),
                        new Order(100, "Garnet Statue", "Thinking"),
                        new TimedTransition(200, "IsG&JDead?")
                        ),
                    new State("IsG&JDead?",
                        new EntitiesNotExistsTransition(50, "DropPortal", "Garnet Statue", "Jade Statue")
                    ),
                    new State("DropPortal",
                        new Suicide()
                    )
                )
            );
    }
}

#endregion