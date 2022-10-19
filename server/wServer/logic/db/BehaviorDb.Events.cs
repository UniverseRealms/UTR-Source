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
        private _ Events = () => Behav()
        #region Skull Shrine

            .Init("Skull Shrine",
            new State(
                    new ScaleHP(0.3),
                    new Shoot(25, 9, 10, predictive: 1),
                    new Spawn("Red Flaming Skull", 8, coolDown: 5000),
                    new Spawn("Blue Flaming Skull", 10, coolDown: 1000),
                    new Reproduce("Red Flaming Skull", 10, 8, coolDown: 5000),
                    new Reproduce("Blue Flaming Skull", 10, 10, coolDown: 1000),
                new State("basic",
                    new HpLessTransition(0.4, "immune")
                   ),
                new State("immune",
                    new Flash(0xFF0000, 1, 2),
                    new TimedTransition(3000, "immune2")
                   ),
                new State("immune2",
                    new Flash(0x0000FF, 1, 2),
                    new ConditionalEffect(ConditionEffectIndex.StunImmune)
                   )
                ),
                new MostDamagers(3,
                        LootTemplates.Sor1Perc()
                    ),
              new MostDamagers(3,
                     LootTemplates.StatPots()
                     ),
                new Threshold(0.01,
                    new TierLoot(10, ItemType.Weapon, 0.2),
                    new TierLoot(11, ItemType.Weapon, 0.1),
                    new TierLoot(12, ItemType.Armor, 0.05),
                    new TierLoot(12, ItemType.Weapon, 0.05),
                    new ItemLoot("Flaming Boomerang", 0.02),
                    new ItemLoot("Sacred Essence", 0.0005),
                    new ItemLoot("Legrundy Essence", 0.0005),
                    new ItemLoot("Dagger of Brimstone", 0.02),
                    new ItemLoot("Orb of Conflict", 0.01)
                    )
            )
            .Init("Red Flaming Skull",
                new State(
                    new Prioritize(
                        new Wander(.6),
                        new Follow(.6, 20, 3)
                        ),
                    new Shoot(15, 2, 5, 0, predictive: 1, coolDown: 750)
                    )
            )
            .Init("Blue Flaming Skull",
                new State(
                    new Prioritize(
                        new Orbit(1, 20, target: "Skull Shrine", radiusVariance: 0.5),
                        new Wander(.6)
                        ),
                    new Shoot(15, 2, 5, 0, predictive: 1, coolDown: 750)
                    )
            )
        #endregion

        #region Hermit God

            .Init("Hermit God",
                new State(
                    new ScaleHP(0.3),
                    new DropPortalOnDeath("Ocean Trench Portal", 1, null, 5, 5),
                    new InvisiToss("Hermit God Drop", 6, 0, 90000001, coolDownOffset: 0),
                    new CopyDamageOnDeath("Hermit God Drop"),
                    //new DropPortalOnDeath("Ocean Trench Portal", 100, XAdjustment: 5, YAdjustment: 5),
                    new State("invis",
                        new SetAltTexture(3),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new InvisiToss("Hermit Minion", 9, 0, 90000001, coolDownOffset: 0),
                        new InvisiToss("Hermit Minion", 9, 45, 90000001, coolDownOffset: 0),
                        new InvisiToss("Hermit Minion", 9, 90, 90000001, coolDownOffset: 0),
                        new InvisiToss("Hermit Minion", 9, 135, 90000001, coolDownOffset: 0),
                        new InvisiToss("Hermit Minion", 9, 180, 90000001, coolDownOffset: 0),
                        new InvisiToss("Hermit Minion", 9, 225, 90000001, coolDownOffset: 0),
                        new InvisiToss("Hermit Minion", 9, 270, 90000001, coolDownOffset: 0),
                          new InvisiToss("Hermit Minion", 9, 315, 90000001, coolDownOffset: 0),
                        new InvisiToss("Hermit Minion", 9, 15, 90000001, coolDownOffset: 0),
                        new InvisiToss("Hermit Minion", 9, 30, 90000001, coolDownOffset: 0),
                        new InvisiToss("Hermit Minion", 9, 90, 90000001, coolDownOffset: 0),
                        new InvisiToss("Hermit Minion", 9, 120, 90000001, coolDownOffset: 0),
                        new InvisiToss("Hermit Minion", 9, 150, 90000001, coolDownOffset: 0),
                        new InvisiToss("Hermit Minion", 9, 180, 90000001, coolDownOffset: 0),
                        new InvisiToss("Hermit Minion", 9, 210, 90000001, coolDownOffset: 0),
                        new InvisiToss("Hermit Minion", 9, 240, 90000001, coolDownOffset: 0),
                        new InvisiToss("Hermit Minion", 9, 50, 90000001, coolDownOffset: 0),
                        new InvisiToss("Hermit Minion", 9, 100, 90000001, coolDownOffset: 0),
                        new InvisiToss("Hermit Minion", 9, 150, 90000001, coolDownOffset: 0),
                        new InvisiToss("Hermit Minion", 9, 200, 90000001, coolDownOffset: 0),
                        new InvisiToss("Hermit Minion", 9, 250, 90000001, coolDownOffset: 0),
                        new InvisiToss("Hermit Minion", 9, 300, 90000001, coolDownOffset: 0),
                        new InvisiToss("Hermit God Tentacle", 5, 45, 90000001, coolDownOffset: 0),
                        new InvisiToss("Hermit God Tentacle", 5, 90, 90000001, coolDownOffset: 0),
                        new InvisiToss("Hermit God Tentacle", 5, 135, 90000001, coolDownOffset: 0),
                        new InvisiToss("Hermit God Tentacle", 5, 180, 90000001, coolDownOffset: 0),
                        new InvisiToss("Hermit God Tentacle", 5, 225, 90000001, coolDownOffset: 0),
                        new InvisiToss("Hermit God Tentacle", 5, 270, 90000001, coolDownOffset: 0),
                        new InvisiToss("Hermit God Tentacle", 5, 315, 90000001, coolDownOffset: 0),
                        new TimedTransition(1000, "check")
                        ),
                    new State("check",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new EntityNotExistsTransition("Hermit God Tentacle", 20, "active")
                        ),
                    new State("active",
                        new SetAltTexture(2),
                        new TimedTransition(500, "active2")
                        ),
                    new State("active2",
                        new SetAltTexture(0),
                        new Shoot(25, 3, 10, 0, coolDown: 400),
                        new Wander(.2),
                        new TossObject("Whirlpool", 6, 0, 90000001, 100),
                        new TossObject("Whirlpool", 6, 45, 90000001, 100),
                        new TossObject("Whirlpool", 6, 90, 90000001, 100),
                        new TossObject("Whirlpool", 6, 135, 90000001, 100),
                        new TossObject("Whirlpool", 6, 180, 90000001, 100),
                        new TossObject("Whirlpool", 6, 225, 90000001, 100),
                        new TossObject("Whirlpool", 6, 270, 90000001, 100),
                        new TossObject("Whirlpool", 6, 315, 90000001, 100),
                        new TimedTransition(10000, "rage")
                        ),
                    new State("rage",
                        new SetAltTexture(4),
                        new Order(20, "Whirlpool", "despawn"),
                        new Flash(0xfFF0000, .8, 9000001),
                        new Shoot(25, 8, projectileIndex: 1, coolDown: 2000),
                        new Shoot(25, 20, projectileIndex: 2, coolDown: 3000, coolDownOffset: 5000),
                        new TimedTransition(17000, "invis")
                        )
                    )
            )
            .Init("Whirlpool",
                new State(
                    new ScaleHP(0.1),
                    new State("active",
                        new Shoot(25, 8, projectileIndex: 0, coolDown: 1000),
                        new Orbit(.5, 4, target: "Hermit God", radiusVariance: 0),
                        new EntityNotExistsTransition("Hermit God", 50, "despawn")
                        ),
                    new State("despawn",
                        new Suicide()
                        )
                    )
            )
            .Init("Hermit God Tentacle",
                new State(
                    new ScaleHP(0.1),
                    new Prioritize(
                        new Orbit(.5, 5, target: "Hermit God", radiusVariance: 0.5),
                        new Follow(0.85, range: 1, duration: 2000, coolDown: 0)
                        ),
                    new Shoot(4, 8, projectileIndex: 0, coolDown: 1000)
                    )
            )
            .Init("Hermit Minion",
                new State(
                    new Prioritize(
                        new Wander(.5),
                        new Follow(0.85, 3, 1, 2000, 0)
                        ),
                    new Shoot(5, 1, 1, 1, coolDown: 2300),
                    new Shoot(5, 3, 1, 0, coolDown: 1000)
                    )
            )
            .Init("Hermit God Drop",
                new State(
                    new State("idle",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new EntityNotExistsTransition("Hermit God", 10, "despawn")
                        ),
                    new State("despawn",
                        new Suicide()
                        )
                    ),
                new Threshold(0.01,
                    new ItemLoot("Decapod Helm", 0.0084),
                    new ItemLoot("Marine Tome", 0.0046),
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
         .Init("Peepo",
            new State(
                new ScaleHP(0.3),
                new State("Idle",
                    new StayCloseToSpawn(0.1, 6),
                    new Wander(0.7),
                    new HpLessTransition(0.99999, "Uh oh")
                    ),
                new State("Uh oh",
                    new Flash(0x00ff00, 0.1, 50),
                    new Taunt("Feels cool man"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new TimedTransition(5000, "Go away")
                    ),
                new State("Go away",
                    new Shoot(10, projectileIndex: 0, count: 15, shootAngle: 24, coolDown: 1500),
                    new HpLessTransition(0.80, "Reee")
                    ),
                new State("Reee",
                    new Taunt("REEEEEE"),
                    new Prioritize(
                            new Follow(0.8, range: 7),
                            new Wander(0.5)
                            ),
                    new Shoot(20, projectileIndex: 3, count: 15, shootAngle: 24, coolDown: 1000),
                    new Shoot(15, projectileIndex: 1, count: 3, shootAngle: 15, coolDown: 400),
                    new HpLessTransition(0.65, "Kys1")
                    ),
                new State("Kys1",
                    new Flash(0x00ff00, 1, 2),
                    new Taunt("You are too weak to face me noobs! :lmao:"),
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new TimedTransition(500, "Pause"),
                    new HpLessTransition(0.30, "Rage")
                    ),
                new State("Kys2",
                    new Flash(0x00ff00, 1, 2),
                    new Shoot(20, projectileIndex: 3, count: 15, shootAngle: 24, coolDown: 400),
                    new Shoot(15, projectileIndex: 1, count: 3, shootAngle: 15, coolDown: 500),
                    new TimedTransition(2000, "Pause"),
                    new HpLessTransition(0.30, "Rage")
                    ),
                new State("Pause",
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Shoot(15, projectileIndex: 2, count: 40, shootAngle: 9, coolDown: 1200),
                    new TimedTransition(1000, "Kys2"),
                    new HpLessTransition(0.30, "Rage")
                    ),
                new State("Rage",
                    new Flash(0xff0000, 5, 999999),
                    new Taunt("DIE!"),
                    new Prioritize(
                            new Follow(0.8, range: 7),
                            new Wander(0.5)
                            ),
                    new Shoot(10, projectileIndex: 0, count: 10, shootAngle: 36, coolDown: 1000),
                    new Shoot(20, projectileIndex: 3, count: 3, shootAngle: 24, coolDown: 1000),
                    new Shoot(15, projectileIndex: 1, count: 3, shootAngle: 15, coolDown: 400),
                    new HpLessTransition(0.005, "Die")
                    ),
                new State("Die",
                    new Taunt("Feels bad, man..."),
                    new Flash(0x0000ff, 0.3, 9999999)
                    )
                ),
                new Threshold(0.06,
                    new ItemLoot("Potion Spud", 0.001)
                    )
            )
      .Init("Hellfire Lord",
          new State(
              new ScaleHP(0.3),
              new State("Idle",
                  new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                  new PlayerWithinTransition(12, "Uh oh")
                  ),
              new State("Uh oh",
                  new Taunt("Who arrives to challenge me?"),
                  new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                  new TimedTransition(2000, "Uh oh 2")
                  ),
              new State("Uh oh 2",
                  new Grenade(5, 250, 15, coolDown: 1250),
                  new HpLessTransition(0.97, "Uh oh 3")
                  ),
              new State("Uh oh 3",
                  new Taunt("I see, you seek adventure. But on the behalf of Oryx, I shall destroy you."),
                  new TossObject("Hellfire Turret", range: 9.89949, angle: 45, coolDown: 5000),
                  new TossObject("Hellfire Turret", range: 9.89949, angle: 135, coolDown: 5000),
                  new TossObject("Hellfire Turret", range: 9.89949, angle: 225, coolDown: 5000),
                  new TossObject("Hellfire Turret", range: 9.89949, angle: 315, coolDown: 5000),
                  new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                  new TimedTransition(2000, "Fire Stream")
                  ),
              new State("Fire Stream",
                  new Shoot(30, projectileIndex: 0, count: 3, coolDown: 400, shootAngle: 16),
                  new HpLessTransition(0.80, "Fire Rings")
                  ),
              new State("Fire Rings",
                  new Taunt("Be engulfed in flames!"),
                  new Shoot(30, projectileIndex: 2, count: 7, coolDown: 1000, shootAngle: 360 / 7),
                  new Shoot(30, projectileIndex: 3, count: 7, coolDown: 1000, shootAngle: 360 / 7),
                  new HpLessTransition(0.60, "Fire Tentacles")
                  ),
              new State("Fire Tentacles",
                  new Taunt("Turrets, obliterate them!"),
                  new Order(100, "Hellfire Turret", "Shooting2"),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 400 * 20, coolDownOffset: 0, fixedAngle: 0, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 400 * 20, coolDownOffset: 200 * 1, fixedAngle: 5 * 1, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 400 * 20, coolDownOffset: 200 * 2, fixedAngle: 5 * 2, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 400 * 20, coolDownOffset: 200 * 3, fixedAngle: 5 * 3, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 400 * 20, coolDownOffset: 200 * 4, fixedAngle: 5 * 4, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 400 * 20, coolDownOffset: 200 * 5, fixedAngle: 5 * 5, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 400 * 20, coolDownOffset: 200 * 6, fixedAngle: 5 * 6, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 400 * 20, coolDownOffset: 200 * 7, fixedAngle: 5 * 7, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 400 * 20, coolDownOffset: 200 * 8, fixedAngle: 5 * 8, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 400 * 20, coolDownOffset: 200 * 9, fixedAngle: 5 * 9, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 400 * 20, coolDownOffset: 200 * 10, fixedAngle: 5 * 10, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 400 * 20, coolDownOffset: 200 * 11, fixedAngle: 5 * 11, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 400 * 20, coolDownOffset: 200 * 12, fixedAngle: 5 * 12, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 400 * 20, coolDownOffset: 200 * 13, fixedAngle: 5 * 13, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 400 * 20, coolDownOffset: 200 * 14, fixedAngle: 5 * 14, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 400 * 20, coolDownOffset: 200 * 15, fixedAngle: 5 * 15, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 400 * 20, coolDownOffset: 200 * 16, fixedAngle: 5 * 16, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 400 * 20, coolDownOffset: 200 * 17, fixedAngle: 5 * 17, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 400 * 20, coolDownOffset: 200 * 18, fixedAngle: 5 * 18, shootAngle: 60),
                  new HpLessTransition(0.40, "Fire Waves")
                  ),
              new State("Fire Waves",
                  new Taunt("Gah! I have had enough!"),
                  new Order(100, "Hellfire Turret", "Shooting3"),
                  new Shoot(30, projectileIndex: 1, count: 1, coolDown: 1000),
                  new Shoot(50, projectileIndex: 4, count: 5, coolDown: 4000, coolDownOffset: 0, fixedAngle: 0, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 7, coolDown: 4000, coolDownOffset: 400, fixedAngle: 0, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 800, fixedAngle: 0, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 1200, fixedAngle: 0, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 1600, fixedAngle: 0, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 5, coolDown: 4000, coolDownOffset: 0, fixedAngle: 180, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 7, coolDown: 4000, coolDownOffset: 400, fixedAngle: 180, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 800, fixedAngle: 180, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 1200, fixedAngle: 180, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 1600, fixedAngle: 180, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 5, coolDown: 4000, coolDownOffset: 2000, fixedAngle: 90, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 7, coolDown: 4000, coolDownOffset: 2400, fixedAngle: 90, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 2800, fixedAngle: 90, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 3200, fixedAngle: 90, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 3600, fixedAngle: 90, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 5, coolDown: 4000, coolDownOffset: 2000, fixedAngle: 270, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 7, coolDown: 4000, coolDownOffset: 2400, fixedAngle: 270, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 2800, fixedAngle: 270, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 3200, fixedAngle: 270, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 3600, fixedAngle: 270, shootAngle: 9),
                  new TossObject("Hellfire Bomb", range: 10, angle: 45, coolDown: 4000, coolDownOffset: 0),
                  new TossObject("Hellfire Bomb", range: 10, angle: 135, coolDown: 4000, coolDownOffset: 0),
                  new TossObject("Hellfire Bomb", range: 10, angle: 225, coolDown: 4000, coolDownOffset: 0),
                  new TossObject("Hellfire Bomb", range: 10, angle: 315, coolDown: 4000, coolDownOffset: 0),
                  new TossObject("Hellfire Bomb", range: 10, angle: 0, coolDown: 4000, coolDownOffset: 2000),
                  new TossObject("Hellfire Bomb", range: 10, angle: 90, coolDown: 4000, coolDownOffset: 2000),
                  new TossObject("Hellfire Bomb", range: 10, angle: 180, coolDown: 4000, coolDownOffset: 2000),
                  new TossObject("Hellfire Bomb", range: 10, angle: 270, coolDown: 4000, coolDownOffset: 2000),
                  new HpLessTransition(0.20, "Pre Rage")
                  ),
              new State("Pre Rage",
                  new Taunt("..."),
                  new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                  new TimedTransition(6000, "Rage")
                  ),
              new State("Rage",
                  new Taunt("YOU WILL DIE!!!"),
                  new Flash(0xff0000, 2, 999999),
                  new TossObject("Hellfire Bomb", range: 9, angle: 45, coolDown: 4000, coolDownOffset: 0),
                  new TossObject("Hellfire Bomb", range: 9, angle: 135, coolDown: 4000, coolDownOffset: 0),
                  new TossObject("Hellfire Bomb", range: 9, angle: 225, coolDown: 4000, coolDownOffset: 0),
                  new TossObject("Hellfire Bomb", range: 9, angle: 315, coolDown: 4000, coolDownOffset: 0),
                  new TossObject("Hellfire Bomb", range: 9, angle: 0, coolDown: 4000, coolDownOffset: 2000),
                  new TossObject("Hellfire Bomb", range: 9, angle: 90, coolDown: 4000, coolDownOffset: 2000),
                  new TossObject("Hellfire Bomb", range: 9, angle: 180, coolDown: 4000, coolDownOffset: 2000),
                  new TossObject("Hellfire Bomb", range: 9, angle: 270, coolDown: 4000, coolDownOffset: 2000),
                  new TossObject("Hellfire Bomb", range: 2, angle: 45, coolDown: 4000, coolDownOffset: 0),
                  new TossObject("Hellfire Bomb", range: 2, angle: 135, coolDown: 4000, coolDownOffset: 0),
                  new TossObject("Hellfire Bomb", range: 2, angle: 225, coolDown: 4000, coolDownOffset: 0),
                  new TossObject("Hellfire Bomb", range: 2, angle: 315, coolDown: 4000, coolDownOffset: 0),
                  new TossObject("Hellfire Bomb", range: 2, angle: 0, coolDown: 4000, coolDownOffset: 2000),
                  new TossObject("Hellfire Bomb", range: 2, angle: 90, coolDown: 4000, coolDownOffset: 2000),
                  new TossObject("Hellfire Bomb", range: 2, angle: 180, coolDown: 4000, coolDownOffset: 2000),
                  new TossObject("Hellfire Bomb", range: 2, angle: 270, coolDown: 4000, coolDownOffset: 2000),
                  new Shoot(30, projectileIndex: 0, count: 3, coolDown: 400, shootAngle: 16),
                  new Shoot(30, projectileIndex: 4, count: 1, coolDown: 400),
                  new HpLessTransition(0.05, "Ded")
                  ),
              new State("Ded",
                  new Taunt("Oryx, I have failed you..."),
                  new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                  new Flash(0x000000, 4, 1),
                  new TimedTransition(2000, "Oof")
                  ),
              new State("Oof",
                  new Shoot(30, projectileIndex: 2, count: 36, fixedAngle: 0, shootAngle: 10),
                  new Shoot(30, projectileIndex: 3, count: 36, fixedAngle: 5, shootAngle: 10),
                  new Suicide()
                  )
              ),
                new Threshold(0.01,
                    new ItemLoot("Infernal Spell", 0.0084),
                    new ItemLoot("Ifrit's Charm", 0.0046),
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
          .Init("Hellfire Turret",
          new State(
              new ConditionalEffect(ConditionEffectIndex.Invulnerable),
              new EntityNotExistsTransition("Hellfire Lord", 50, "Kaboom"),
              new State("Shooting1",
                  new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                  new Shoot(30, projectileIndex: 0, count: 5, coolDown: 3000, coolDownOffset: 0, shootAngle: 72),
                  new Shoot(30, projectileIndex: 1, count: 5, coolDown: 3000, coolDownOffset: 1500, shootAngle: 72)
                  ),
              new State("Shooting2",
                  new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                  new Shoot(30, projectileIndex: 1, count: 8, coolDown: 2000, coolDownOffset: 0, fixedAngle: 0, shootAngle: 45),
                  new Shoot(30, projectileIndex: 1, count: 8, coolDown: 2000, coolDownOffset: 1000, fixedAngle: 22.5, shootAngle: 45)
                  ),
              new State("Shooting3",
                  new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                  new Shoot(30, projectileIndex: 0, count: 1, coolDown: 1000)
                  ),
              new State("Kaboom",
                  new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                  new Shoot(30, projectileIndex: 1, count: 8, fixedAngle: 0, shootAngle: 45),
                  new Shoot(30, projectileIndex: 0, count: 8, fixedAngle: 22.5, shootAngle: 45),
                  new Decay(0)
                  )
              )
          )
          .Init("Hellfire Bomb",
          new State(
              new State("Idle",
                  new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                  new Flash(0xff0000, 2, 1),
                  new ChangeSize(100, 80),
                  new TimedTransition(800, "Kaboom2")
                  ),
              new State("Kaboom2",
                  new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                  new Shoot(30, projectileIndex: 0, count: 8, fixedAngle: 0, shootAngle: 45),
                  new Decay(0)
                  )
              )
            )

         .Init("Sanic",
          new State(
              new ScaleHP(0.3),
              new State("Idle",
                  new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                  new PlayerWithinTransition(5, "1")
                  ),
              new State("1",
                  new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                  new Taunt("No one can match my speed, haha!"),
                  new TimedTransition(4000, "2")
                  ),
              new State("2",
                  new Follow(speed: 2.5, range: 10),
                  new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 0, predictive: 0.2, coolDown: 400),
                  new HpLessTransition(0.2, "YoUrEtOoSlOw")
                  ),
              new State("YoUrEtOoSlOw",
                  new Taunt("YoU'rE tOo SlOw!"),
                  new StayBack(speed: 3, distance: 30),
                  new TimedTransition(8000, "rest")
                  ),
              new State("rest",
                  new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                  new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 0, predictive: 0.2, coolDown: 400),
                  new TimedTransition(5000, "YoUrEtOoSlOw")
                  )
              ),
              new Threshold(0.06,
                  new ItemLoot("Sacred Essence", 0.0005)
            )
            )

        #endregion
        #region Transcendent Burrower

        .Init("Transcendent Burrower",
            new State(
                new ScaleHP(0.3),
                new State("idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new PlayerWithinTransition(5, "1")
                    ),
                new State("1",
                    new Flash(0xff0000, 1, 10),
                    new TimedTransition(5000, "2")
                    ),
                new State("2",
                    new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                    new Wander(0.8),
                    new Shoot(20, count: 5, shootAngle: 20, projectileIndex: 1, coolDown: 500),
                    new Shoot(20, projectileIndex: 4, count: 3, shootAngle: 25, coolDown: 2000),
                    new InvisiToss("Sandworms", range: 4.5, angle: 0, coolDown: 10000),
                    new InvisiToss("Sandworms", range: 4.5, angle: 72, coolDown: 11000),
                    new InvisiToss("Sandworms", range: 4.5, angle: 144, coolDown: 12000),
                    new InvisiToss("Sandworms", range: 4.5, angle: 216, coolDown: 13000),
                    new InvisiToss("Sandworms", range: 4.5, angle: 288, coolDown: 14000),
                    new HpLessTransition(0.75, "prep3")
                    ),
                new State("prep3",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new Flash(0xff0000, 1, 10),
                    new Taunt("HISS*"),
                    new TimedTransition(3000, "a1")
                    ),
                new State("a1",
                    new SetAltTexture(2),
                    new TimedTransition(100, "a2")
                    ),
                new State("a2",
                    new SetAltTexture(3),
                    new TimedTransition(100, "a3")
                    ),
                new State("a3",
                    new SetAltTexture(4),
                    new TimedTransition(100, "a4")
                    ),
                new State("a4",
                    new SetAltTexture(5),
                    new TimedTransition(100, "a5")
                    ),
                new State("a5",
                    new SetAltTexture(6),
                    new TimedTransition(100, "a6")
                    ),
                new State("a6",
                    new SetAltTexture(7),
                    new TimedTransition(100, "a7")
                    ),
                new State("a7",
                    new SetAltTexture(8),
                    new TimedTransition(100, "a8")
                    ),
                new State("a8",
                    new SetAltTexture(9),
                    new TimedTransition(100, "3")
                    ),
                new State("3",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new SetAltTexture(1),
                    new Charge(speed: 1, range: 15),
                    new Wander(speed: 0.8),
                    new PlayerWithinTransition(2, "4")
                    ),
                new State("4",
                    new SetAltTexture(0),
                    new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                    new Shoot(20, count: 20, shootAngle: 18, projectileIndex: 0, coolDown: 500),
                    new Shoot(20, count: 8, shootAngle: 45, projectileIndex: 5, coolDown: 1800),
                    new HpLessTransition(0.4, "6prep"),
                    new TimedTransition(3600, "5")
                    ),
                new State("5",
                    new Wander(0.8),
                    new Shoot(20, count: 5, shootAngle: 20, projectileIndex: 1, coolDown: 500),
                    new Shoot(20, projectileIndex: 4, count: 3, shootAngle: 25, coolDown: 2000),
                    new InvisiToss("Sandworms", range: 4.5, angle: 0, coolDown: 10000),
                    new InvisiToss("Sandworms", range: 4.5, angle: 72, coolDown: 11000),
                    new InvisiToss("Sandworms", range: 4.5, angle: 144, coolDown: 12000),
                    new InvisiToss("Sandworms", range: 4.5, angle: 216, coolDown: 13000),
                    new InvisiToss("Sandworms", range: 4.5, angle: 288, coolDown: 14000),
                    new HpLessTransition(0.4, "6prep"),
                    new TimedTransition(3000, "3")
                    ),
                new State("6prep",
                    new ChangeSize(13, 250),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Flash(0xff0000, 1, 10),
                    new Taunt("HISS*"),
                    new TimedTransition(3000, "6")
                    ),
                new State("6",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new SetAltTexture(1),
                    new Charge(speed: 1, range: 15),
                    new Wander(speed: 0.8),
                    new PlayerWithinTransition(2, "7")
                    ),
                new State("7",
                    new SetAltTexture(0),
                    new InvisiToss("Sandworms", range: 6, angle: 0, coolDown: 10000),
                    new InvisiToss("Sandworms", range: 6, angle: 180, coolDown: 11000),
                    new InvisiToss("Sandworms", range: 4.5, angle: 0, coolDown: 12000),
                    new InvisiToss("Sandworms", range: 4.5, angle: 72, coolDown: 13000),
                    new InvisiToss("Sandworms", range: 4.5, angle: 144, coolDown: 14000),
                    new InvisiToss("Sandworms", range: 4.5, angle: 216, coolDown: 15000),
                    new InvisiToss("Sandworms", range: 4.5, angle: 288, coolDown: 16000),
                    new Grenade(radius: 3.5, range: 0, fixedAngle: 0, color: 0x01FCF6, coolDown: 2000, damage: 150, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 2000),
                    new Shoot(20, count: 7, shootAngle: 14, projectileIndex: 3, coolDown: 1500),
                    new Shoot(20, count: 4, shootAngle: 90, fixedAngle: 0, projectileIndex: 6, coolDown: 3000),
                    new Shoot(20, count: 1, projectileIndex: 7, coolDown: 5000),
                    new TimedTransition(10000, "6")
                        )
                    ),
                new Threshold(0.01,
                    new ItemLoot("Sand Dune Cloak", 0.0084),
                    new ItemLoot("Gleaming Mineral", 0.0046),
                    new ItemLoot("Stat Potion Crate", 1),
                    new TierLoot(2, ItemType.Weapon, 0.05),
                    new TierLoot(2, ItemType.Ability, 0.05),
                    new TierLoot(2, ItemType.Armor, 0.05),
                    new TierLoot(2, ItemType.Ring, 0.05),
                    new TierLoot(3, ItemType.Weapon, 0.025),
                    new TierLoot(3, ItemType.Ability, 0.025),
                    new TierLoot(3, ItemType.Armor, 0.025),
                    new TierLoot(3, ItemType.Ring, 0.025),
                    new ItemLoot("Rusted Medium Abilities Chest", 0.00143),
                    new ItemLoot("Eternal Essence", 0.0005)
          )
            )
        .Init("Sandworms",
            new State(
                new ScaleHP(0.1),
                new State("boom",
                    new Charge(speed: 0.8, range: 15),
                    new Wander(speed: 0.6),
                    new Shoot(20, projectileIndex: 0, count: 3, shootAngle: 25, coolDown: 2000)
                    )
                )
            )
        #endregion
        #region Blaze-Born
        .Init("Blaze-Born",
            new State(
                new ScaleHP(0.3),
                new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new PlayerWithinTransition(5, "1")
                    ),
                new State("1",
                    new InvisiToss("Blaze-Born Anchor", range: 0, angle: 0, coolDown: 99999),
                    new Taunt("You made a mistake coming here. Now, you will perish."),
                    new Flash(0xff0000, 1, 3),
                    new TimedTransition(3000, "2")
                    ),
                new State("2",
                    new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new StayCloseToSpawn(speed: 0.6, range: 5),
                    new Wander(0.6),
                    new Shoot(15, projectileIndex: 0, count: 3, shootAngle: 6, coolDown: 1250),
                    new Shoot(15, projectileIndex: 1, count: 5, shootAngle: 9, coolDown: 2000),
                    new Shoot(15, projectileIndex: 2, count: 3, shootAngle: 30, coolDown: 1000),
                    new HpLessTransition(0.75, "3")
                    ),
                new State("3",
                    new Taunt("Minions, protect me!"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new InvisiToss("Blazed Warrior", range: 4.5, angle: 0, coolDown: 7000),
                    new InvisiToss("Blazed Warrior", range: 4.5, angle: 36, coolDown: 7000),
                    new InvisiToss("Blazed Warrior", range: 4.5, angle: 72, coolDown: 7000),
                    new InvisiToss("Blazed Warrior", range: 4.5, angle: 108, coolDown: 7000),
                    new InvisiToss("Blazed Warrior", range: 4.5, angle: 144, coolDown: 7000),
                    new InvisiToss("Blazed Warrior", range: 4.5, angle: 180, coolDown: 7000),
                    new InvisiToss("Blazed Warrior", range: 4.5, angle: 216, coolDown: 7000),
                    new InvisiToss("Blazed Warrior", range: 4.5, angle: 252, coolDown: 7000),
                    new InvisiToss("Blazed Warrior", range: 4.5, angle: 288, coolDown: 7000),
                    new InvisiToss("Blazed Warrior", range: 4.5, angle: 324, coolDown: 7000),
                    new TimedTransition(2000, "4")
                    ),
                new State("4",
                    new Shoot(15, projectileIndex: 3, count: 4, shootAngle: 90, coolDown: 3000),
                    new Shoot(15, projectileIndex: 4, count: 9, shootAngle: 45, fixedAngle: 0, coolDown: 2000, predictive: 0.25),
                    new EntitiesNotExistsTransition(15, "5", "Blazed Warrior")
                    ),
                new State("5",
                    new Flash(0xff0000, 1, 6),
                    new ChangeSize(13, 250),
                    new TimedTransition(3000, "6")
                    ),
                new State("6",
                    new Wander(0.75),
                    new Charge(speed: 0.6, range: 7),
                    new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Shoot(15, projectileIndex: 4, count: 9, shootAngle: 45, fixedAngle: 0, coolDown: 2000),
                    new Shoot(15, projectileIndex: 5, count: 5, shootAngle: 12, coolDown: 3000),
                    new TimedTransition(3500, "7")
                    ),
                new State("7",
                    new Flash(0xff0000, 0.5, 3),
                    new Taunt("Burn!"),
                    new TimedTransition(1000, "8")
                    ),
                new State("8",
                    new Shoot(15, projectileIndex: 6, count: 3, shootAngle: 5, coolDown: 5000),
                    new TimedTransition(2000, "6")
                    )
                ),
            new Threshold(0.01,
                    new ItemLoot("Sheath of the Molten Core", 0.0084),
                    new ItemLoot("Volcanic Battle Axe", 0.0046),
                    new ItemLoot("Stat Potion Crate", 1),
                    new TierLoot(2, ItemType.Weapon, 0.05),
                    new TierLoot(2, ItemType.Ability, 0.05),
                    new TierLoot(2, ItemType.Armor, 0.05),
                    new TierLoot(2, ItemType.Ring, 0.05),
                    new TierLoot(3, ItemType.Weapon, 0.025),
                    new TierLoot(3, ItemType.Ability, 0.025),
                    new TierLoot(3, ItemType.Armor, 0.025),
                    new TierLoot(3, ItemType.Ring, 0.025),
                    new ItemLoot("Rusted Heavy Abilities Chest", 0.00143),
                    new ItemLoot("Eternal Essence", 0.0005)
                )
            )
           .Init("Blaze-Born Anchor",
            new State(
                new State("1",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    )
                )
            )
           .Init("Blazed Warrior",
            new State(
                new ScaleHP(0.1),
                new State("boom",
                    new Orbit(speed: 0.6, radius: 5, target: "Blaze-Born Anchor", speedVariance: 0.2),
                    new Shoot(20, projectileIndex: 0, count: 3, shootAngle: 6, coolDown: 1000, predictive: 0.5)
                    )
                )
            )
        #endregion
        #region Grotesque Serpent
        .Init("Grotesque Serpent",
            new State(
                new ScaleHP(0.3),
                new State("idle",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new PlayerWithinTransition(5, "1")
                    ),
                new State("1",
                    new Taunt("Hiss!"),
                    new Flash(0xff0000, 2, 3),
                    new TimedTransition(3000, "2")
                    ),
                new State("2",
                    new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Wander(0.3),
                    new Charge(0.4, range: 10),
                    new Shoot(10, projectileIndex: 0, shootAngle: 10, count: 3, coolDown: 500),
                    new Shoot(10, projectileIndex: 1, shootAngle: 10, count: 10, coolDown: 3000),
                    new HpLessTransition(0.66, "3")
                    ),
                new State("3",
                    new Shoot(10, projectileIndex: 2, shootAngle: 45, fixedAngle: 0, count: 8, coolDown: 5000),
                    new Shoot(10, projectileIndex: 3, shootAngle: 10, count: 4, coolDown: 1250),
                    new HpLessTransition(0.33, "4")
                    ),
                new State("4",
                    new Taunt("AAAAAAAAAA"),
                    new ChangeSize(13, 125),
                    new Wander(0.4),
                    new Charge(0.5, range: 10),
                    new Shoot(10, projectileIndex: 1, shootAngle: 10, count: 10, coolDown: 3000),
                    new Shoot(10, projectileIndex: 4, shootAngle: 18, count: 20, coolDown: 400),
                    new Shoot(10, projectileIndex: 5, shootAngle: 5, count: 5, coolDown: 750)
                            )
                        ),
                new Threshold(0.01,
                    new ItemLoot("A Serpent's Remains", 0.0084),
                    new ItemLoot("Bone Marrow Katana", 0.0046),
                    new ItemLoot("Stat Potion Crate", 1),
                    new TierLoot(2, ItemType.Weapon, 0.05),
                    new TierLoot(2, ItemType.Ability, 0.05),
                    new TierLoot(2, ItemType.Armor, 0.05),
                    new TierLoot(2, ItemType.Ring, 0.05),
                    new TierLoot(3, ItemType.Weapon, 0.025),
                    new TierLoot(3, ItemType.Ability, 0.025),
                    new TierLoot(3, ItemType.Armor, 0.025),
                    new TierLoot(3, ItemType.Ring, 0.025),
                    new ItemLoot("Rusted Medium Abilities Chest", 0.00143),
                    new ItemLoot("Eternal Essence", 0.0005)
                    )
                  );
        #endregion
        #region Grand Totem of Deception

        #endregion
        #region Fiery Fiend
        #endregion
        #region Kinetic Occultist
        #endregion

    }
}