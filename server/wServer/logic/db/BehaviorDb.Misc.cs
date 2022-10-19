#region

using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;
#endregion

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Misc = () => Behav()
		   .Init("Purple Drake",
				new State(
					new CosmeticFollow()

				)
			)
        #region pets   
        .Init("The Lost Guardian Pet",
            new State(
                new FamiliarFollow()
                )
            )
        .Init("Shadow of the Void Pet",
		    new State(
				new FamiliarFollow()
				)
			)
        .Init("Girl Pet",
			new State(
                new FamiliarFollow()
				)
			)
        .Init("Forgotten Priest Pet",
		    new State(
				new FamiliarFollow()
				)
			)
        .Init("Twilight Entity Pet",
			new State(
                new FamiliarFollow()
				)
			)
        .Init("Sorceress of the Bridge Pet",
		    new State(
				new FamiliarFollow()
				)
			)
        .Init("The Guardian Angel Pet",
			new State(
                new FamiliarFollow()
				)
			)
        .Init("The Time Keeper Pet",
		    new State(
				new FamiliarFollow()
				)
			)
        .Init("Blood Strained Eye Pet",
			new State(
                new FamiliarFollow()
				)
			)
        .Init("Overseer of the Spirits Pet",
		    new State(
				new FamiliarFollow()
				)
			)
        .Init("Parrot King Pet",
			new State(
                new FamiliarFollow()
				)
			)
        .Init("Bat King Pet",
		    new State(
				new FamiliarFollow()
				)
            )
        .Init("Winged Snake Pet",
            new State(
                new FamiliarFollow()
                )
            )
        .Init("Succubus Pet",
            new State(
                new FamiliarFollow()
                )
            )
        .Init("Undead Wanderer Pet",
            new State(
                new FamiliarFollow()
                )
            )
        .Init("Djin Pet",
            new State(
                new FamiliarFollow()
                )
            )
        .Init("The Ancient Slayer Pet",
            new State(
                new FamiliarFollow()
                )
            )
        .Init("Showdown Torturer Pet",
            new State(
                new FamiliarFollow()
                )
            )
        .Init("Triton Pet",
            new State(
                new FamiliarFollow()
                )
            )
        .Init("Ghost Bride Pet",
            new State(
                new FamiliarFollow()
                )
            )
        .Init("Fool of the Court Pet",
            new State(
                new FamiliarFollow()
                )
            )
        .Init("The Berserker Pet",
            new State(
                new FamiliarFollow()
                )
            )
        .Init("Alien Predator Pet",
            new State(
                new FamiliarFollow()
                )
            )
        .Init("The Dark Maiden Pet",
            new State(
                new FamiliarFollow()
                )
            )
        .Init("Dread Moth Pet",
            new State(
                new FamiliarFollow()
                )
            )
        .Init("Black Widow Pet",
            new State(
                new FamiliarFollow()
                )
            )
        .Init("Acollyte of the Darkness Pet",
            new State(
                new FamiliarFollow()
                )
            )
        .Init("Carnivore Pet",
            new State(
                new FamiliarFollow()
                )
            )
        .Init("Granite Debris Pet",
            new State(
                new FamiliarFollow()
                )
            )
        .Init("The Lifeless Harlequin Pet",
            new State(
                new FamiliarFollow()
                )
            )
        .Init("Corrupted Summoner Pet",
            new State(
                new FamiliarFollow()
                )
            )
        .Init("Twin Hydra Pet",
            new State(
                new FamiliarFollow()
                )
            )
        .Init("The Mad Templar Pet",
            new State(
                new FamiliarFollow()
                )
            )
        .Init("Fanatic Consort Pet",
            new State(
                new FamiliarFollow()
                )
            )
        .Init("Head of the Wicked Pet",
            new State(
                new FamiliarFollow()
                )
            )
        .Init("Mistress of Insanity Pet",
            new State(
                new FamiliarFollow()
                )
            )
        .Init("Mr.Moustache Pet",
            new State(
                new FamiliarFollow()
                )
            )
        .Init("Amphitrite Pet",
            new State(
                new FamiliarFollow()
                )
            )
        .Init("Woodland Warrior Pet",
            new State(
                new FamiliarFollow()
                )
            )
        .Init("Archangel Pet",
            new State(
                new FamiliarFollow()
                )
            )
        .Init("Goddess Pet",
            new State(
                new FamiliarFollow()
                )
            )
        .Init("Thunderbird Pet",
            new State(
                new FamiliarFollow()
                )
            )
        .Init("Ice Sorceress Pet",
            new State(
                new FamiliarFollow()
                )
            )
        .Init("Ancient Troll Pet",
            new State(
                new FamiliarFollow()
                )
            )
        #endregion
           .Init("Test Pillar",
                new State(
                    new State("1",
                    new HealGroup(8, "Players", coolDown: 5000, healAmount: 500)
                   )
                )
            )
           .Init("Nexus UFO Small",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("1",
                    new Wander(speed: 0.4),
                    new StayCloseToSpawn(speed: 0.4, range: 4),
                    new TimedTransition(8000, "2")
                        ),
                    new State("2",
                        new TimedTransition(4000, "1")
                   )
                )
            )
            .Init("Nexus UFO Big",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("1",
                    new Wander(speed: 0.4),
                    new StayCloseToSpawn(speed: 0.4, range: 4),
                    new TimedTransition(8000, "2")
                        ),
                    new State("2",
                        new TimedTransition(5000, "1")
                   )
                )
            )
            .Init("White Fountain",
                new State(
                    new HealPlayer(5, 450, 100),
                    new HealPlayerMP(5, 450, 100)
                )
            )
            .Init("Winter Fountain Frozen",
                new State(
                    new HealPlayer(5, 450, 100),
                    new HealPlayerMP(5, 450, 100)
                )
            )
            .Init("Nexus Crier",
                new State("Active",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new BackAndForth(.2, 3)
                )
            )
            .Init("Event Chest",
            new State(
                new ScaleHP(0.5)
                ),
            new Threshold(0.025,
                    new TierLoot(10, ItemType.Weapon, 0.07),
                    new TierLoot(11, ItemType.Weapon, 0.06),
                    new TierLoot(12, ItemType.Weapon, 0.05),
                    new TierLoot(5, ItemType.Ability, 0.07),
                    new TierLoot(6, ItemType.Ability, 0.05),
                    new TierLoot(11, ItemType.Armor, 0.07),
                    new TierLoot(12, ItemType.Armor, 0.06),
                    new TierLoot(13, ItemType.Armor, 0.05),
                    new TierLoot(6, ItemType.Ring, 0.06)
                )
            )
            .Init("Elite Skeleton",
                new State(
                    new State("Default",
                        new Prioritize(
                            new Follow(1.2, 8, 1),
                            new Wander(0.4)
                        ),
                        new Shoot(8, 6, 8, 2, coolDown: 800),
                        new TimedTransition(3000, "Default1")
                    ),
                    new State("Default1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Prioritize(
                            new Charge(2, 10, 1000),
                            new Wander(0.4)
                        ),
                        new Shoot(8, 12, projectileIndex: 1, coolDown: 400),
                        new TimedTransition(2000, "Default2")
                    ),
                    new State("Default2",
                        new Prioritize(
                            new StayBack(1, 4),
                            new Wander(0.4)
                        ),
                        new Shoot(8, 4, 8, 0, coolDown: 800),
                        new TimedTransition(3000, "Default3")
                    ),
                    new State("Default3",
                        new Taunt(0.05, "You don't belong here.."),
                        new HealSelf(1000),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(3000, "Default")
                    )
                )
            )
            .Init("TF Sector",
                new State(
                    new EntitiesNotExistsTransition(99, "die", "TF The Fallen"),
                    new Orbit(0.35, 8, 20, "TF The Fallen"),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("recker",
                        new TimedTransition(12000, "GoDumb"),
                        new State("Quadforce1",
                            new Shoot(0, projectileIndex: 0, count: 5, shootAngle: 60, fixedAngle: 0, coolDown: 1400),
                            new TimedTransition(1400, "Quadforce2")
                        ),
                        new State("Quadforce2",
                            new Shoot(0, projectileIndex: 0, count: 5, shootAngle: 60, fixedAngle: 15, coolDown: 1400),
                            new TimedTransition(1400, "Quadforce3")
                        ),
                        new State("Quadforce3",
                            new Shoot(0, projectileIndex: 0, count: 5, shootAngle: 60, fixedAngle: 30, coolDown: 1400),
                            new TimedTransition(1400, "Quadforce4")
                        ),
                        new State("Quadforce4",
                            new Shoot(0, projectileIndex: 0, count: 5, shootAngle: 60, fixedAngle: 45, coolDown: 1400),
                            new TimedTransition(1400, "Quadforce5")
                        ),
                        new State("Quadforce5",
                            new Shoot(0, projectileIndex: 0, count: 5, shootAngle: 60, fixedAngle: 45, coolDown: 1400),
                            new TimedTransition(1400, "Quadforce6")
                        ),
                        new State("Quadforce6",
                            new Shoot(0, projectileIndex: 0, count: 5, shootAngle: 60, fixedAngle: 30, coolDown: 1400),
                            new TimedTransition(1400, "Quadforce7")
                        ),
                        new State("Quadforce7",
                            new Shoot(0, projectileIndex: 0, count: 5, shootAngle: 60, fixedAngle: 15, coolDown: 1400),
                            new TimedTransition(1400, "Quadforce8")
                        ),
                        new State("Quadforce8",
                            new Shoot(0, projectileIndex: 0, count: 5, shootAngle: 60, fixedAngle: 0, coolDown: 1400),
                            new TimedTransition(1400, "Quadforce1")
                        )
                    ),
                    new State("GoDumb",
                        new Shoot(8.4, 8, 24, 0, coolDown: 2500),
                        new Grenade(3, 160, 8, coolDown: 1000),
                        new TimedTransition(8000, "recker")
                    ),
                    new State("die",
                        new Suicide()
                    )
                )
            )
            .Init("TF The Fallen",
                new State(
                     new ScaleHP(0.3),
                    new State(
                        new HpLessTransition(0.13, "rip1"),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new State("idle",
                            new EntitiesNotExistsTransition(9999, "taunt", "TF Creature Wizard", "TF Warrior",
                                "TF Knight 1", "TF KNight 2")
                        ),
                        new State("taunt",
                            new Taunt(true, "Come. You shall be destroyed by the hands of me.",
                                "We do not have mercy on your souls nor does our leader. Die.",
                                "Our time of awakening has come. Join us or be slayed."),
                            new TimedTransition(10000, "waitforplayers")
                        ),
                        new State("waitforplayers",
                            new PlayerWithinTransition(8, "plantspookstart")
                        ),
                        new State("plantspookstart",
                            new Taunt(
                                "Ah. The time has come. The time for the end of the human race. Consuming your spirits will make me more powerful than ever!"),
                            new InvisiToss("TF Sector", 8, 0, 9999999),
                            new InvisiToss("TF Sector", 8, 180, 9999999),
                            new TimedTransition(5000, "fight1")
                        )
                    ),
                    new State("fight1",
                        new Shoot(8.4, 1, projectileIndex: 3, coolDown: 400),
                        new Shoot(8.4, 5, projectileIndex: 0, coolDown: 2000),
                        new TimedTransition(8000, "fight2")
                    ),
                    new State("fight2",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(8.4, 8, 16, 2, coolDown: 2000),
                        new Shoot(8.4, 4, 16, predictive: 4, projectileIndex: 0, coolDown: 2000),
                        new TimedTransition(8000, "fight3")
                    ),
                    new State("fight3",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Shoot(8.4, 5, projectileIndex: 3, coolDown: 2000),
                        new Shoot(8.4, 8, 12, predictive: 2, projectileIndex: 2, coolDown: 400),
                        new TimedTransition(10000, "fight1"),
                        new State("Quadforce1",
                            new Shoot(0, projectileIndex: 4, count: 5, shootAngle: 60, fixedAngle: 0, coolDown: 400),
                            new TimedTransition(200, "Quadforce2")
                        ),
                        new State("Quadforce2",
                            new Shoot(0, projectileIndex: 4, count: 5, shootAngle: 60, fixedAngle: 15, coolDown: 400),
                            new TimedTransition(200, "Quadforce3")
                        ),
                        new State("Quadforce3",
                            new Shoot(0, projectileIndex: 4, count: 5, shootAngle: 60, fixedAngle: 30, coolDown: 400),
                            new TimedTransition(200, "Quadforce4")
                        ),
                        new State("Quadforce4",
                            new Shoot(0, projectileIndex: 4, count: 5, shootAngle: 60, fixedAngle: 45, coolDown: 400),
                            new TimedTransition(200, "Quadforce5")
                        ),
                        new State("Quadforce5",
                            new Shoot(0, projectileIndex: 4, count: 5, shootAngle: 60, fixedAngle: 45, coolDown: 400),
                            new TimedTransition(200, "Quadforce6")
                        ),
                        new State("Quadforce6",
                            new Shoot(0, projectileIndex: 4, count: 5, shootAngle: 60, fixedAngle: 30, coolDown: 400),
                            new TimedTransition(200, "Quadforce7")
                        ),
                        new State("Quadforce7",
                            new Shoot(0, projectileIndex: 4, count: 5, shootAngle: 60, fixedAngle: 15, coolDown: 400),
                            new TimedTransition(200, "Quadforce8")
                        ),
                        new State("Quadforce8",
                            new Shoot(0, projectileIndex: 4, count: 5, shootAngle: 60, fixedAngle: 0, coolDown: 400),
                            new TimedTransition(200, "Quadforce1")
                        )
                    ),
                    new State("rip1",
                        new RemoveEntity(9999, "TF Sector"),
                        new Taunt("I NEVER THOUGHT I WOULD SEE THE END OF ME!", "THE REST OF MY POWER...IT FADES AWAY!",
                            "GAAAAAAR AAAAGH!"),
                        new Flash(0xFFFF00, 2, 4),
                        new TimedTransition(4000, "rip2")
                    ),
                    new State("rip2",
                        new Suicide()
                    )
                ),
                new Threshold(0.025,
                    new TierLoot(10, ItemType.Weapon, 0.07),
                    new TierLoot(11, ItemType.Weapon, 0.06),
                    new TierLoot(12, ItemType.Weapon, 0.05),
                    new TierLoot(5, ItemType.Ability, 0.07),
                    new TierLoot(6, ItemType.Ability, 0.05),
                    new TierLoot(11, ItemType.Armor, 0.07),
                    new TierLoot(12, ItemType.Armor, 0.06),
                    new TierLoot(13, ItemType.Armor, 0.05),
                    new TierLoot(6, ItemType.Ring, 0.06),
                    new ItemLoot("Spirit of the Past", 0.000333),
                    new ItemLoot("Change of Heart", 0.000333),
                    new ItemLoot("Menacing Sword of No Escape", 0.01),
                    new ItemLoot("Battleplate of Sacred Warlords", 0.01),
                    new ItemLoot("Onrane", 0.05)
                )
            )
            .Init("Sheep",
                new State(
                    new PlayerWithinTransition(15, "player_nearby"),
                    new State("player_nearby",
                        new Prioritize(
                            new StayCloseToSpawn(0.1, 2),
                            new Wander(0.1)
                        ),
                        new Taunt(0.001, 1000, "baa", "baa baa")
                    )
                )
            )
            .Init("Target Spawner",
                new State(
                    new State("Default",
                        new EntityNotExistsTransition("Target Strong", 1, "Spawn")
                    ),
                    new State("Spawn",
                        new Spawn("Target Strong", 1, 1),
                        new TimedTransition(0, "Default")
                    )
                )
            )
            .Init("Dummy Spawner",
                new State(
                    new State("Default",
                        new EntityNotExistsTransition("Dummy Strong", 1, "Spawn")
                    ),
                    new State("Spawn",
                        new Spawn("Dummy Strong", 1, 1),
                        new TimedTransition(0, "Default")
                    )
                )
            )

            .Init("Perma Quiet",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new ConditionEffectRegion(ConditionEffectIndex.Quiet, 9999999)
                    )
                )

            .Init("Perma Sick",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new ConditionEffectRegion(ConditionEffectIndex.Sick, 9999999)
                    )
                )

            .Init("Perma Weak",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new ConditionEffectRegion(ConditionEffectIndex.Weak, 9999999)
                    )
                )

            .Init("Perma OP",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new ConditionEffectRegion(ConditionEffectIndex.Healing, 9999999),
                    new ConditionEffectRegion(ConditionEffectIndex.Empowered, 9999999),
                    new ConditionEffectRegion(ConditionEffectIndex.Armored, 9999999),
                    new ConditionEffectRegion(ConditionEffectIndex.Damaging, 9999999),
                    new ConditionEffectRegion(ConditionEffectIndex.Berserk, 9999999),
                    new ConditionEffectRegion(ConditionEffectIndex.Bravery, 9999999)
                    )
                )

            .Init("GhostShip Nexus",
                new State(
                    new PlayerWithinTransition(15, "player_nearby"),
                    new State("player_nearby",
                        new Prioritize(
                            new StayCloseToSpawn(0.1, 2),
                            new Wander(0.1)
                        )
                    )
                )
            )

           .Init("T0 Talisman",
                new State(
                    new Prioritize(
                        new FamiliarFollow(),
                        new Wander(0.15)
                        ),
                    new TalismanAttack(280, ConditionEffectIndex.Dazed, 1000)
                )
            )
            .Init("T1 Talisman",
                new State(
                    new Prioritize(
                        new FamiliarFollow(),
                        new Wander(0.15)
                        ),
                    new TalismanAttack(320, ConditionEffectIndex.Dazed, 1000)
                )
            )
            .Init("T2 Talisman",
                new State(
                    new Prioritize(
                        new FamiliarFollow(),
                        new Wander(0.15)
                        ),
                    new TalismanAttack(400, ConditionEffectIndex.Dazed, 1000)
                )
            )
            .Init("T3 Talisman",
                new State(
                    new Prioritize(
                        new FamiliarFollow(),
                        new Wander(0.15)
                        ),
                    new TalismanAttack(520, ConditionEffectIndex.Dazed, 1000)
                )
            )
            .Init("T4 Talisman",
                new State(
                    new Prioritize(
                        new FamiliarFollow(),
                        new Wander(0.15)
                        ),
                    new TalismanAttack(600, ConditionEffectIndex.Dazed, 2000)
                )
            )
            .Init("T5 Talisman",
                new State(
                    new Prioritize(
                        new FamiliarFollow(),
                        new Wander(0.15)
                        ),
                    new TalismanAttack(640, ConditionEffectIndex.Dazed, 2000)
                )
            )
            .Init("T6 Talisman",
                new State(
                    new Prioritize(
                        new FamiliarFollow(),
                        new Wander(0.15)
                        ),
                    new TalismanAttack(680, ConditionEffectIndex.Dazed, 3000)
                )
            )
           .Init("UDL Talisman",
                new State(
                    new Prioritize(
                        new FamiliarFollow(),
                        new Wander(0.15)
                        ),
                    new TalismanAttack(680, ConditionEffectIndex.Curse, 4000)
                )
            )
           .Init("Demon Talisman",
                new State(
                    new Prioritize(
                        new FamiliarFollow(),
                        new Wander(0.15)
                        ),
                    new TalismanAttack(800, ConditionEffectIndex.Dazed, 4000)
                )
            )

           .Init("Haunted Talisman1",
                new State(
                    new Prioritize(
                        new FamiliarFollow(),
                        new Wander(0.15)
                        ),
                    new TalismanAttack(1300, ConditionEffectIndex.Curse, 2000)
                )
            )

           .Init("Angelic Talisman1",
                new State(
                    //new SetAttackTarget("Angelic Talisman2", 30, true), // i'll add this when raid reworks get merged into master
                    new Orbit(1.9, 2, 30), // orbit nearest player for now
                    new TalismanAttack(200, ConditionEffectIndex.Weak, 1500)
                )
            )
                   
            .Init("Angelic Talisman2",
                new State(
                    new Prioritize(
                        new FamiliarFollow(),
                        new Wander(0.15)
                        ),
                    new HealPlayer(3, 1000, 60)
                )
            )

        .Init("Event Master",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Active",
                    new Taunt(true, "Hello, I will be the event master for today, in a few seconds I will ask for a number from 1-5."),
                    new TimedTransition(2000, "Info")
                    ),
                    new State("Info",
                        new Taunt(true, "The number you select will change the preset of Spawns, I will only ask once, no return."),
                        new TimedTransition(3000, "Question")
                        ),
                    new State("Question",
                        new Taunt(true, "Pick a number from 1-5"),
                        new PlayerTextTransition("Choice1", "1"),
                        new PlayerTextTransition("Choice2", "2"),
                        new PlayerTextTransition("Choice3", "3"),
                        new PlayerTextTransition("Choice4", "4"),
                        new PlayerTextTransition("Choice5", "5")
                        ),
                    new State("Choice1",
                        new Taunt(true, "Spawning Preset 1 in 10 seconds"),
                        new TimedTransition(10000, "1p1") // 1 Part 1
                        ),
                    new State("1p1",
                        new Spawn("Lucky Ent God", 1, 1, 100000),
                        new TimedTransition(1000, "1p1w") // 1 Part 1 Wait
                        ),
                    new State("1p1w",
                        new EntityNotExistsTransition("Lucky Ent God", 999, "1p1n")
                        ),
                    new State("1p1n",
                        new Taunt(true, "Round 2 in 10 seconds"),
                        new TimedTransition(10000, "1p2")
                        ),
                    new State("1p2",
                        new Spawn("Tomb Defender", 1, 1, 100000),
                        new TimedTransition(1000, "1p2w") // 1 Part 2 Wait
                        ),
                    new State("1p2w",
                        new EntityNotExistsTransition("Tomb Defender", 999, "1p2n")
                        ),
                    new State("1p2n",
                        new Taunt(true, "Round 3 in 10 seconds"),
                        new TimedTransition(10000, "1p3")
                        ),
                    new State("1p3",
                        new Spawn("Doomages", 1, 1, 100000),
                        new TimedTransition(1000, "1p3w") // 1 Part 3 Wait
                        ),
                    new State("1p3w",
                        new EntityNotExistsTransition("Doomages", 999, "1p3n")
                        ),
                    new State("1p3n",
                        new Taunt(true, "Round 4 in 10 seconds"),
                        new TimedTransition(10000, "1p4")
                        ),
                    new State("1p4",
                        new Spawn("Swoll Fairy", 1, 1, 100000),
                        new TimedTransition(1000, "1p4w") // 1 Part 4 Wait
                        ),
                    new State("1p4w",
                        new EntityNotExistsTransition("Swoll Fairy", 999, "1p4n")
                        ),
                    new State("1p4n",
                        new Taunt(true, "Round 5 in 10 seconds"),
                        new TimedTransition(10000, "1p5")
                        ),
                    new State("1p5",
                        new Spawn("Lenin", 1, 1, 100000),
                        new TimedTransition(1000, "1p5w") // 1 Part 5 Wait
                        ),
                    new State("1p5w",
                        new EntityNotExistsTransition("Lenin", 999, "1p5n")
                        ),
                    new State("1p5n",
                        new Taunt(true, "Oh? There's a reward for you!"),
                        new TimedTransition(10000, "eventReward")
                        ),
                    new State("Choice2",
                        new Taunt(true, "Spawning Preset 2 in 10 seconds"),
                        new TimedTransition(10000, "2p1") // 2 Part 1
                        ),
                    new State("2p1",
                        new Spawn("Lucky Djinn", 1, 1, 100000),
                        new TimedTransition(1000, "2p1w") // 2 Part 1 Wait
                        ),
                    new State("2p1w",
                        new EntityNotExistsTransition("Lucky Djinn", 999, "2p1n")
                        ),
                    new State("2p1n",
                        new Taunt(true, "Round 2 in 10 seconds"),
                        new TimedTransition(10000, "2p2")
                        ),
                    new State("2p2",
                        new Spawn("Tomb Support", 1, 1, 100000),
                        new TimedTransition(1000, "2p2w") // 2 Part 2 Wait
                        ),
                    new State("2p2w",
                        new EntityNotExistsTransition("Tomb Support", 999, "2p2n")
                        ),
                    new State("2p2n",
                        new Taunt(true, "Round 3 in 10 seconds"),
                        new TimedTransition(10000, "2p3")
                        ),
                    new State("2p3",
                        new Spawn("Desire Troll", 1, 1, 100000),
                        new TimedTransition(1000, "2p3w") // 2 Part 3 Wait
                        ),
                    new State("2p3w",
                        new EntityNotExistsTransition("Desire Troll", 999, "2p3n")
                        ),
                    new State("2p3n",
                        new Taunt(true, "Round 4 in 10 seconds"),
                        new TimedTransition(10000, "2p4")
                        ),
                    new State("2p4",
                        new Spawn("Lord Stone Gargoyle", 1, 1, 100000),
                        new TimedTransition(1000, "2p4w") // 2 Part 4 Wait
                        ),
                    new State("2p4w",
                        new EntityNotExistsTransition("Lord Stone Gargoyle", 999, "2p4n")
                        ),
                    new State("2p4n",
                        new Taunt(true, "Round 5 in 10 seconds"),
                        new TimedTransition(10000, "2p5")
                        ),
                    new State("2p5",
                        new Spawn("Tunnel Varghus The Eye", 1, 1, 100000),
                        new TimedTransition(1000, "2p5w") // 2 Part 5 Wait
                        ),
                    new State("2p5w",
                        new EntityNotExistsTransition("Tunnel Varghus The Eye", 999, "2p5n")
                        ),
                    new State("2p5n",
                        new Taunt(true, "Oh? There's a reward for you!"),
                        new TimedTransition(10000, "eventReward")
                        ),
                    new State("Choice3",
                        new Taunt(true, "Spawning Preset 3 in 10 seconds"),
                        new TimedTransition(10000, "3p1") // 3 Part 1
                        ),
                    new State("3p1",
                        new Spawn("Limon the Sprite God", 1, 1, 100000),
                        new TimedTransition(1000, "3p1w") // 3 Part 1 Wait
                        ),
                    new State("3p1w",
                        new EntityNotExistsTransition("Limon the Sprite God", 999, "3p1n")
                        ),
                    new State("3p1n",
                        new Taunt(true, "Round 2 in 10 seconds"),
                        new TimedTransition(10000, "3p2")
                        ),
                    new State("3p2",
                        new Spawn("Construct of the Storm", 1, 1, 100000),
                        new TimedTransition(1000, "3p2w") // 3 Part 2 Wait
                        ),
                    new State("3p2w",
                        new EntityNotExistsTransition("Construct of the Storm", 999, "3p2n")
                        ),
                    new State("3p2n",
                        new Taunt(true, "Round 3 in 10 seconds"),
                        new TimedTransition(10000, "3p3")
                        ),
                    new State("3p3",
                        new Spawn("Corrupted Flames of Fate", 1, 1, 100000),
                        new TimedTransition(1000, "3p3w") // 3 Part 3 Wait
                        ),
                    new State("3p3w",
                        new EntityNotExistsTransition("Corrupted Flames of Fate", 999, "3p3n")
                        ),
                    new State("3p3n",
                        new Taunt(true, "Round 4 in 10 seconds"),
                        new TimedTransition(10000, "3p4")
                        ),
                    new State("3p4",
                        new Spawn("Puppet Master V2", 1, 1, 100000),
                        new TimedTransition(1000, "3p4w") // 3 Part 4 Wait
                        ),
                    new State("3p4w",
                        new EntityNotExistsTransition("Puppet Master V2", 999, "3p4n")
                        ),
                    new State("3p4n",
                        new Taunt(true, "Round 5 in 10 seconds"),
                        new TimedTransition(10000, "3p5")
                        ),
                    new State("3p5",
                        new Spawn("UNP Servant of the Dark Knight", 1, 1, 100000),
                        new TimedTransition(1000, "3p5w") // 3 Part 5 Wait
                        ),
                    new State("3p5w",
                        new EntityNotExistsTransition("UNP Servant of the Dark Knight", 999, "3p5n")
                        ),
                    new State("3p5n",
                        new Taunt(true, "Oh? There's a reward for you!"),
                        new TimedTransition(10000, "eventReward")
                        ),
                    new State("Choice4",
                        new Taunt(true, "Spawning Preset 4 in 10 seconds"),
                        new TimedTransition(10000, "4p1") // 4 Part 1
                        ),
                    new State("4p1",
                        new Spawn("Son of Arachna", 1, 1, 100000),
                        new TimedTransition(1000, "4p1w") // 4 Part 1 Wait
                        ),
                    new State("4p1w",
                        new EntityNotExistsTransition("Son of Arachna", 999, "4p1n")
                        ),
                    new State("4p1n",
                        new Taunt(true, "Round 2 in 10 seconds"),
                        new TimedTransition(10000, "4p2")
                        ),
                    new State("4p2",
                        new Spawn("The Overseer", 1, 1, 100000),
                        new TimedTransition(1000, "4p2w") // 4 Part 2 Wait
                        ),
                    new State("4p2w",
                        new EntityNotExistsTransition("The Overseer", 999, "4p2n")
                        ),
                    new State("4p2n",
                        new Taunt(true, "Round 3 in 10 seconds"),
                        new TimedTransition(10000, "4p3")
                        ),
                    new State("4p3",
                        new Spawn("Tomb Attacker", 1, 1, 100000),
                        new TimedTransition(1000, "4p3w") // 4 Part 3 Wait
                        ),
                    new State("4p3w",
                        new EntityNotExistsTransition("Tomb Attacker", 999, "4p3n")
                        ),
                    new State("4p3n",
                        new Taunt(true, "Round 4 in 10 seconds"),
                        new TimedTransition(10000, "4p4")
                        ),
                    new State("4p4",
                        new Spawn("Yazanahar", 1, 1, 100000),
                        new TimedTransition(1000, "4p4w") // 4 Part 4 Wait
                        ),
                    new State("4p4w",
                        new EntityNotExistsTransition("Yazanahar", 999, "4p4n")
                        ),
                    new State("4p4n",
                        new Taunt(true, "Round 5 in 10 seconds"),
                        new TimedTransition(10000, "4p5")
                        ),
                    new State("4p5",
                        new Spawn("Sorgigas, the sor giant", 1, 1, 100000),
                        new TimedTransition(1000, "4p5w") // 4 Part 5 Wait
                        ),
                    new State("4p5w",
                        new EntityNotExistsTransition("Sorgigas, the sor giant", 999, "4p5n")
                        ),
                    new State("4p5n",
                        new Taunt(true, "Oh? There's a reward for you!"),
                        new TimedTransition(10000, "eventReward")
                        ),
                    new State("Choice5",
                        new Taunt(true, "Spawning Preset 5 in 10 seconds"),
                        new TimedTransition(10000, "5p1") // 5 Part 1
                        ),
                    new State("5p1",
                        new Spawn("Truvix, The Lord Wanderer", 1, 1, 100000),
                        new TimedTransition(1000, "5p1w") // 5 Part 1 Wait
                        ),
                    new State("5p1w",
                        new EntityNotExistsTransition("Truvix, The Lord Wanderer", 999, "5p1n")
                        ),
                    new State("5p1n",
                        new Taunt(true, "Round 2 in 10 seconds"),
                        new TimedTransition(10000, "5p2")
                        ),
                    new State("5p2",
                        new Spawn("Zaragon, the Blood Mage", 1, 1, 100000),
                        new TimedTransition(1000, "5p2w") // 5 Part 2 Wait
                        ),
                    new State("5p2w",
                        new EntityNotExistsTransition("Zaragon, the Blood Mage", 999, "5p2n")
                        ),
                    new State("5p2n",
                        new Taunt(true, "Round 3 in 10 seconds"),
                        new TimedTransition(10000, "5p3")
                        ),
                    new State("5p3",
                        new Spawn("The Illusionist", 1, 1, 100000),
                        new TimedTransition(1000, "5p3w") // 5 Part 3 Wait
                        ),
                    new State("5p3w",
                        new EntityNotExistsTransition("The Illusionist", 999, "5p3n")
                        ),
                    new State("5p3n",
                        new Taunt(true, "Round 4 in 10 seconds"),
                        new TimedTransition(10000, "5p4")
                        ),
                    new State("5p4",
                        new Spawn("Hades", 1, 1, 100000),
                        new TimedTransition(1000, "5p4w") // 5 Part 4 Wait
                        ),
                    new State("5p4w",
                        new EntityNotExistsTransition("Hades", 999, "5p4n")
                        ),
                    new State("5p4n",
                        new Taunt(true, "Round 5 in 10 seconds"),
                        new TimedTransition(10000, "5p5")
                        ),
                    new State("5p5",
                        new Spawn("Tridorno", 1, 1, 100000),
                        new TimedTransition(1000, "5p5w") // 5 Part 5 Wait
                        ),
                    new State("5p5w",
                        new EntityNotExistsTransition("Tridorno", 999, "5p5n")
                        ),
                    new State("5p5n",
                        new Taunt(true, "Oh? There's a reward for you!"),
                        new TimedTransition(10000, "eventReward")
                        ),
                    new State("eventReward",
                        new Spawn("Event Chest", 1, 1, 100000),
                        new TimedTransition(1000, "Suicide")
                        ),
                    new State("Suicide",
                        new Suicide()
                        )
                )
            )
        .Init("Marketplace Teleport U",
            new State(
                new State("Teleport",
                    new TeleportPlayerTo(44, 44, 1)
                    )
                )
            )

        .Init("Marketplace Teleport U2",
            new State(
                new State("Teleport",
                new TeleportPlayerTo(85, 44, 1)
                    )
                )
            )

        .Init("Marketplace Teleport UR",
            new State(
                new State("Teleport",
                new TeleportPlayerTo(65, 34, 1)
                    )
                )
            )

        .Init("Marketplace Teleport UR2",
            new State(
                new State("Teleport",
                new TeleportPlayerTo(94, 65, 1)
                    )
                )
            )

        .Init("Marketplace Teleport R",
            new State(
                new State("Teleport",
                new TeleportPlayerTo(85, 43, 1)
                    )
                )
            )

        .Init("Marketplace Teleport R2",
            new State(
                new State("Teleport",
                new TeleportPlayerTo(65, 85, 1)
                    )
                )
            )

        .Init("Marketplace Teleport D",
            new State(
                new State("Teleport",
                new TeleportPlayerTo(94, 65, 1)
                    )
                )
            )

        .Init("Marketplace Teleport D2",
            new State(
                new State("Teleport",
                new TeleportPlayerTo(35, 64, 1)
                    )
                )
            )

        .Init("Marketplace Teleport L",
            new State(
                new State("Teleport",
                new TeleportPlayerTo(65, 85, 1)
                    )
                )
            )

        .Init("Marketplace Teleport L2",
            new State(
                new State("Teleport",
                    new TeleportPlayerTo(44, 44, 1)
                    )
                )
            )

        .Init("Marketplace Teleport UL",
            new State(
                new State("Teleport",
                new TeleportPlayerTo(35, 66, 1)
                    )
                )
            )

        .Init("Marketplace Teleport UL2",
            new State(
                new State("Teleport",
                new TeleportPlayerTo(64, 34, 1)
                    )
                )
            )
            
        .Init("Spectral Decoy",
            new State(             
                new DecoyMove(3000),
                new State("shoot",
                    new AllyShoot(1, projectileIndex: 0, count: 10, shootAngle: 36, fixedAngle: 0, coolDown: 800),
                    new TimedTransition(4000, "die")
                    ),
                new State("die",
                    new Decay(0)
                    )
                )
            )
        .Init("Disco",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("1",
                    new GroundTransform("CGarden Floor Red", 30, persist:true),
                    new TimedTransition(100, "2")
                    ),
                new State("2",
                    new GroundTransform("CGarden Floor Blue", 30, persist: true),
                    new TimedTransition(100, "3")
                    ),
                new State("3",
                    new GroundTransform("CGarden Floor Purple", 30, persist: true),
                    new TimedTransition(100, "4")
                    ),
                new State("4",
                    new GroundTransform("CGarden Floor Yellow", 30, persist: true),
                    new TimedTransition(100, "5")
                    ),
                new State("5",
                    new GroundTransform("CGarden Floor", 30, persist: true),
                    new TimedTransition(100, "1")
                    )
                )
            )
        .Init("Cheems",
            new State(  
                new ChangeMusicOnDeath("cheemsdied"),
                new State("cheemer",
                    new ChangeMusic("cheems"),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new TimedTransition(4000, "dorime")
                    ),
                new State("dorime",
                    new Taunt("Dorime"),
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                    )
                )
            )
        .Init("Casino Manager",
            new State(  
                new State("casino",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new TimedRandomTransition(45000, true, "scared", "chill")
                    ),
                new State("scared",
                    new Taunt("Make sure your host is somebody you can trust!"),
                    new TimedTransition(1000, "casino")
                    ),
                new State("chill",
                    new Taunt("Only gamble what you can afford to lose :trollface:"),
                    new TimedTransition(1000, "casino")
                    )
                )
            )
          .Init("Anubis Decoy",
            new State(
                new State("shoot",
                        new AllyShoot(20, 3, 10, 0, 160),
                        new AllyShoot(20, 4, 10, 0, 180, coolDownOffset: 200),
                        new AllyShoot(20, 3, 10, 0, 200, coolDownOffset: 400),
                        new AllyShoot(20, 3, 10, 0, 260, coolDownOffset: 600),
                        new AllyShoot(20, 5, 10, 0, 280, coolDownOffset: 800),
                        new AllyShoot(20, 3, 10, 0, 300, coolDownOffset: 1000),
                        new AllyShoot(20, 3, 10, 0, 0, coolDownOffset: 1200),
                        new AllyShoot(20, 4, 10, 0, 20, coolDownOffset: 1400),
                        new AllyShoot(20, 3, 10, 0, 40, coolDownOffset: 1600),
                    new TimedTransition(1800, "die")
                    ),
                new State("die",
                    new Decay(0)
                    )
                )
            )
        .Init("Aanaraki Decoy",
            new State(
                new State("idle",
                    new TimedTransition(7000, "die")
                    ),
                new State("die",
                    new Decay(0)
                    )
                )
            )
            
        .Init("Brainwashed Gray Blob",
            new State(
                new State("uh oh",
                    new AllyCharge(2.5),
                    new Wander(0.3),
                    new AnyEntityWithinTransition(2, "boom", true)
                    ),
                new State("boom",
                    new AllyShoot(9999, 18, fixedAngle: 0),
                    new Decay(0)
                    )
                )
            );
    }
}