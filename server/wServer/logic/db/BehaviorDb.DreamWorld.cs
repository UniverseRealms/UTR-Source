using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ DreamWorld = () => Behav()

        #region Leaders
        //Leader 1
        .Init("Luna, the Mischievous",
            new State(
                new ScaleHP(0.1),
                new State("idle",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new PlayerWithinTransition(5, "moons")
                    ),
                new State("moons",
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new TossObject("Moon of Luna", range: 1.5, angle: 270, coolDown: 99999),
                    new TossObject("Moon of Luna", range: 1.5, angle: 0, coolDown: 99999),
                    new TossObject("Moon of Luna", range: 1.5, angle: 90, coolDown: 99999),
                    new TossObject("Moon of Luna", range: 1.5, angle: 180, coolDown: 99999),
                    new TimedTransition(2100, "checkformoons")
                    ),
                new State("checkformoons",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new EntitiesNotExistsTransition(9999, "minions", "Moon of Luna")
                    ),
                new State("minions",
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new TossObject("Etrigan the Ravager", range: 1, angle: 85, coolDown: 99999),
                    new TossObject("Etrigan the Ravager", range: 1, angle: 315, coolDown: 99999),
                    new TossObject("Etrigan the Ravager", range: 1, angle: 225, coolDown: 99999),
                    new TimedTransition(2100, "1")
                    ),
                new State("1",
                    new Shoot(10, count: 8, fixedAngle: 0, shootAngle: 45, projectileIndex: 2, coolDown: 1000, coolDownOffset: 1000),
                    new Shoot(10, count: 5, shootAngle: 35, projectileIndex: 0, coolDown: 2000, coolDownOffset: 1200),
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Charge(speed: 1, range: 8),
                    new HpLessTransition(0.5, "2")
                    ),
                new State("2",
                    new Follow(0.6, 8, 1),
                    new Wander(0.4),
                    new Shoot(10, count: 6, shootAngle: 60, projectileIndex: 3, coolDown: 1500),
                    new Shoot(10, count: 6, shootAngle: 60, projectileIndex: 4, coolDown: 1500),
                    new Shoot(10, count: 6, shootAngle: 60, projectileIndex: 5, coolDown: 1500),
                    new Shoot(10, count: 6, shootAngle: 60, projectileIndex: 6, coolDown: 1500),
                    new Shoot(10, count: 6, shootAngle: 60, projectileIndex: 7, coolDown: 1500),
                    new HpLessTransition(0.2, "3")
                    ),
                new State("3",
                    new Follow(1.2, 8, 1),
                    new Wander(1.2),
                    new Flash(0xffff00, 1, 1),
                    new Shoot(10, count: 4, projectileIndex: 1, coolDown: 500)
                    )
                ),
            new Threshold(0.01,
                new ItemLoot("Strike of Calamity", 0.005)
                )
            )
          //Leader 2
          .Init("Nox, the Nightmare",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new SetAltTexture(0),
                    new Wander(speed: 0.7),
                    new Shoot(20, projectileIndex: 0, count: 5, shootAngle: 72, coolDown: 2000, coolDownOffset: 1200),
                    new Shoot(15, projectileIndex: 1, count: 6, shootAngle: 20, coolDown: 2000, coolDownOffset: 1600),
                    new HpLessTransition(0.15, "rageprep")
                    ),
                new State("rageprep",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Flash(0xff0000, 1, 1),
                    new SetAltTexture(1),
                    new TimedTransition(3000, "rage")
                    ),
                new State("rage",
                    new TossObject("Farkal the Void", range: 1, angle: 0, coolDown: 99999),
                    new TossObject("Farkal the Void", range: 1, angle: 180, coolDown: 99999),
                    new ConditionalEffect(ConditionEffectIndex.ArmorBreakImmune),
                    new ConditionalEffect(ConditionEffectIndex.StunImmune),
                    new ConditionalEffect(ConditionEffectIndex.WeakImmune),
                    new Charge(speed: 1, range: 12),
                    new Shoot(15, projectileIndex: 3, shootAngle: 13, count: 8, coolDown: 3000),
                    new Shoot(15, projectileIndex: 2, count: 4, shootAngle: 11, coolDown: 500)
                    )
                ),
            new Threshold(0.01,
                new ItemLoot("The Grand Grimoire", 0.005)
                )
            )
          //Leader 3
          .Init("Grod, the Cosmic Parasite",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new PlayerWithinTransition(7, "2")
                    ),
                new State("2",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new TossObject("Wandering Soul", range: 3, angle: 0, coolDown: 99999),
                    new TossObject("Wandering Soul", range: 3, angle: 180, coolDown: 99999),
                    new TimedTransition(2100, "checkforminions")
                    ),
                new State("checkforminions",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new EntitiesNotExistsTransition(9999, "3", "Wandering Soul")
                    ),
                new State("3",
                    new Follow(0.5, 8, 1),
                    new Wander(0.4),
                    new Shoot(10, count: 3, shootAngle: 11, projectileIndex: 1, coolDown: 3000, coolDownOffset: 1000),
                    new Shoot(10, count: 5, shootAngle: 11, projectileIndex: 2, coolDown: 3000, coolDownOffset: 1400),
                    new Shoot(10, count: 7, shootAngle: 11, projectileIndex: 3, coolDown: 3000, coolDownOffset: 1800),
                    new HpLessTransition(0.2, "rageprep")
                    ),
                new State("rageprep",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new ChangeSize(13, 140),
                    new Flash(0xff0000, 5, 10),
                    new TimedTransition(2000, "rage")
                    ),
                new State("rage",
                    new Charge(speed: 1.3, range: 12),
                    new Shoot(10, count: 20, shootAngle: 18, coolDown: 3000, projectileIndex: 0, coolDownOffset: 5),
                    new Shoot(10, count: 20, shootAngle: 18, coolDown: 3000, projectileIndex: 0, coolDownOffset: 220),
                    new Shoot(10, count: 20, shootAngle: 18, coolDown: 3000, projectileIndex: 0, coolDownOffset: 420),
                    new Shoot(10, count: 20, shootAngle: 18, coolDown: 3000, projectileIndex: 0, coolDownOffset: 620)
                    )
                ),
            new Threshold(0.01,
                new ItemLoot("Robe of the Celestial Grace", 0.005)
                )
            )
           //Leader 4
           .Init("Aleph, the Lost Soul",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new PlayerWithinTransition(7, "2")
                    ),
                new State("2",
                    new Shoot(10, count: 7, shootAngle: 13, coolDown: 1000, projectileIndex: 0),
                    new Follow(0.6, 8, 1),
                    new Wander(0.4),
                    new HpLessTransition(0.3, "rageprep")
                    ),
                new State("rageprep",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new ChangeSize(13, 140),
                    new Flash(0xff0000, 1, 10),
                    new TimedTransition(2000, "rage")
                    ),
                new State("rage",
                    new TossObject("Green Parasite", range: 1, angle: 90, coolDown: 99999),
                    new TossObject("Green Parasite", range: 1, angle: 315, coolDown: 99999),
                    new TossObject("Green Parasite", range: 1, angle: 225, coolDown: 99999),
                    new TimedTransition(2000, "rage2")
                    ),
                new State("rage2",
                    new Follow(1, 8, 1),
                    new Wander(1),
                    new Shoot(10, count: 6, shootAngle: 60, projectileIndex: 1, coolDown: 1500),
                    new Shoot(10, count: 6, shootAngle: 60, projectileIndex: 2, coolDown: 1500),
                    new Shoot(10, count: 6, shootAngle: 60, projectileIndex: 3, coolDown: 1500),
                    new Shoot(10, count: 6, shootAngle: 60, projectileIndex: 4, coolDown: 1500),
                    new Shoot(10, count: 6, shootAngle: 60, projectileIndex: 5, coolDown: 1500),
                    new TimedTransition(4000, "rage3")
                    ),
                new State("rage3",
                    new Shoot(10, count: 8, fixedAngle: 0, shootAngle: 45, projectileIndex: 6, coolDown: 1000, coolDownOffset: 1000),
                    new Shoot(10, count: 5, shootAngle: 37, projectileIndex: 1, coolDown: 1000, coolDownOffset: 1200),
                    new Shoot(10, count: 5, shootAngle: 37, projectileIndex: 2, coolDown: 1000, coolDownOffset: 1200),
                    new Shoot(10, count: 5, shootAngle: 37, projectileIndex: 3, coolDown: 1000, coolDownOffset: 1200),
                    new Shoot(10, count: 5, shootAngle: 37, projectileIndex: 4, coolDown: 1000, coolDownOffset: 1200),
                    new Shoot(10, count: 5, shootAngle: 37, projectileIndex: 5, coolDown: 1000, coolDownOffset: 1200),
                    new TimedTransition(6000, "rage2")
                    )
                ),
            new Threshold(0.01,
                new ItemLoot("Ring of Dwelling Nightmares", 0.005)
                )
            )
        #endregion
        #region Dream Crusade
         //Minion #8
         .Init("Commander of Dreams",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new PlayerWithinTransition(5, "activate")
                    ),
                new State("activate",
                    new TossObject("Soldier of Dreams", range: 1, angle: 90, coolDown: 99999, coolDownOffset: 0),
                    new TossObject("Mage of Dreams", range: 1, angle: 155, coolDown: 99999, coolDownOffset: 0),
                    new TossObject("Soldier of Dreams", range: 1, angle: 25, coolDown: 99999, coolDownOffset: 0),
                    new TossObject("Mage of Dreams", range: 1, angle: 335, coolDown: 99999, coolDownOffset: 0),
                    new TossObject("Soldier of Dreams", range: 2, angle: 270, coolDown: 99999, coolDownOffset: 0),
                    new TossObject("Mage of Dreams", range: 2, angle: 205, coolDown: 99999, coolDownOffset: 0),
                    new Follow(0.5, 8, 1),
                    new Wander(0.5),
                    new Shoot(20, projectileIndex: 0, count: 5, shootAngle: 10, coolDown: 3000),
                    new HpLessTransition(0.4, "lastprep")
                    ),
                new State("lastprep",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Flash(0xff0000, 1, 1),
                    new ChangeSize(13, 140),
                    new TimedTransition(3000, "2")
                    ),
                new State("2",
                    new TossObject("Soldier of Dreams", range: 1, angle: 45, coolDown: 99999, coolDownOffset: 0),
                    new TossObject("Mage of Dreams", range: 1, angle: 135, coolDown: 99999, coolDownOffset: 0),
                    new TossObject("Soldier of Dreams", range: 1, angle: 225, coolDown: 99999, coolDownOffset: 0),
                    new TossObject("Mage of Dreams", range: 1, angle: 315, coolDown: 99999, coolDownOffset: 0),
                    new Follow(0.9, 8, 1),
                    new Wander(0.9),
                    new Shoot(20, projectileIndex: 0, count: 5, shootAngle: 10, coolDown: 2000)
                    )
                )
            )
         //Minion Extras (Crusade)
         .Init("Soldier of Dreams",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Follow(0.7, 8, 1),
                    new Wander(0.7),
                    new Shoot(20, projectileIndex: 0, count: 2, shootAngle: 11, fixedAngle: 90, coolDown: 3000),
                    new Shoot(20, projectileIndex: 0, count: 3, shootAngle: 15, fixedAngle: 270, coolDown: 3000)
                    )
                )
            )
          //Minion Extras (Crusade)
          .Init("Mage of Dreams",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new Follow(0.5, 8, 1),
                    new Wander(0.5),
                    new Shoot(10, count: 2, shootAngle: 10, projectileIndex: 0, coolDown: 1200),
                    new Shoot(15, projectileIndex: 1, count: 1, coolDown: 1000)
                    )
                )
            )
        #endregion
        #region Minions
           //minion#1
           .Init("Etrigan the Ravager",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new Charge(speed: 1.4, range: 15),
                    new Wander(speed: 0.3),
                    new Shoot(10, count: 2, projectileIndex: 0, shootAngle: 0, coolDown: 1000, coolDownOffset: 1200),
                    new Shoot(10, count: 2, projectileIndex: 1, shootAngle: 0, coolDown: 1000, coolDownOffset: 1200),
                    new Shoot(10, count: 2, projectileIndex: 2, shootAngle: 0, coolDown: 1000, coolDownOffset: 1200),
                    new Shoot(10, count: 6, projectileIndex: 3, shootAngle: 15, coolDown: 1200, coolDownOffset: 1400),
                    new Grenade(10, damage: 75, range: 6, coolDown: 3000),
                    new TimedTransition(5000, "2")
                    ),
                new State("2",
                    new Shoot(10, count: 2, projectileIndex: 4, shootAngle: 11, coolDown: 1000, coolDownOffset: 1200),
                    new Shoot(10, count: 2, projectileIndex: 5, shootAngle: 11, coolDown: 1000, coolDownOffset: 1200),
                    new Shoot(10, count: 2, projectileIndex: 6, shootAngle: 11, coolDown: 1000, coolDownOffset: 1200),
                    new Shoot(10, count: 6, projectileIndex: 3, shootAngle: 15, coolDown: 1200, coolDownOffset: 1400),
                    new TimedTransition(5000, "1")
                    )
                )
            )
           //minion#2
           .Init("Farkal the Void",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new Follow(0.7, 8, 1),
                    new Wander(0.4),
                    new Shoot(10, count: 1, shootAngle: 11, projectileIndex: 0, coolDown: 1200),
                    new Shoot(10, count: 1, shootAngle: 11, projectileIndex: 1, coolDown: 1200),
                    new Shoot(10, count: 1, shootAngle: 11, projectileIndex: 2, coolDown: 1200),
                    new Shoot(10, count: 1, shootAngle: 11, projectileIndex: 3, coolDown: 1200),
                    new TimedTransition(6000, "2")
                    ),
                new State("2",
                    new Shoot(radius: 30, count: 4, projectileIndex: 4, fixedAngle: 0, coolDown: 10),
                    new TimedTransition(1000, "3")
                    ),
                new State("3",
                    new Shoot(radius: 30, count: 4, projectileIndex: 5, fixedAngle: 0, coolDown: 10),
                    new TimedTransition(2000, "1")
                    )
                )
            )
           //minion#3
           .Init("Aluz'kha the Chaos",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new Shoot(10, count: 3, projectileIndex: 0, coolDown: 600),
                    new HealGroup(8, "DreamWorld", coolDown: 5000, healAmount: 500)
                    )
                )
            )
           //minion#4
           .Init("Lucifer the Distraught",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Charge(speed: 1.4, range: 10),
                    new PlayerWithinTransition(2, "boom")
                    ),
                new State("boom",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Shoot(10, count: 10, projectileIndex: 0, shootAngle: 36),
                    new Suicide()
                    )
                )
            )
           //minion#5
           .Init("Lokee the Sadness",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new Follow(0.7, 8, 1),
                    new Wander(0.4),
                    new Shoot(10, count: 2, shootAngle: 0, projectileIndex: 0, coolDown: 2000, coolDownOffset: 1400),
                    new Shoot(10, count: 5, shootAngle: 17, projectileIndex: 1, coolDown: 2000, coolDownOffset: 1600),
                    new Shoot(10, count: 10, shootAngle: 36, projectileIndex: 2, coolDown: 2000, coolDownOffset: 1800)
                    )
                )
            )
           //minion#6
           .Init("The Sleepwalker",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new Wander(0.5),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new PlayerWithinTransition(5, "2")
                    ),
                new State("2",
                    new Follow(0.5, 8, 1),
                    new Wander(0.4),
                    new ExplodingShoot(10, count: 5, shootAngle: 75, projectileIndex: 1, explodeIndex: 1, explodeCount: 5, coolDown: 2000, coolDownOffset: 1200),
                    new ExplodingShoot(10, count: 3, shootAngle: 40, projectileIndex: 1, explodeIndex: 0, explodeCount: 3, coolDown: 2000, coolDownOffset: 2000, explodeAngle: 60)
                    )
                )
            )
           //minion#7
           .Init("Naiad the Feared One",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Follow(0.5, 8, 1),
                    new Wander(0.4),
                    new TossObject("Naiad Bomb", range: 6, angle: 90, coolDown: 4000),
                    new TossObject("Naiad Bomb", range: 6, angle: 315, coolDown: 4000),
                    new TossObject("Naiad Bomb", range: 6, angle: 225, coolDown: 4000),
                    new HpLessTransition(0.3, "2")
                    ),
                new State("2",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new TossObject("Lucifer the Distraught", angleOffset: 0, coolDown: 99999),
                    new TossObject("Lucifer the Distraught", angleOffset: 180, coolDown: 99999),
                    new TimedTransition(1000, "3")
                    ),
                new State("3",
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Wander(0.4),
                    new Spawn("Lucifer the Distraught", maxChildren: 1, initialSpawn: 1, coolDown: 5000),
                    new TossObject("Naiad Bomb", range: 6, angle: 90, coolDown: 2000),
                    new TossObject("Naiad Bomb", range: 6, angle: 315, coolDown: 2000),
                    new TossObject("Naiad Bomb", range: 6, angle: 225, coolDown: 2000)
                    )
                )
            )
         //minion#7 Bomb
         .Init("Naiad Bomb",
          new State(
              new State("Idle",
                  new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                  new Flash(0xff0000, 2, 1),
                  new ChangeSize(100, 130),
                  new TimedTransition(800, "Kaboom2")
                  ),
              new State("Kaboom2",
                  new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                  new Shoot(30, projectileIndex: 0, count: 8, fixedAngle: 0, shootAngle: 45),
                  new Decay(0)
                  )
              )
            )
           //minion#9
           .Init("Eye of Omniscience",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new SpiralShoot(rotateAngle: 20, shotsToRestart: 20, numShots: 2, shootAngle: 90, projectileIndex: 0, fixedAngle: 0, coolDown: 300),
                    new SpiralShoot(rotateAngle: -20, shotsToRestart: 20, numShots: 2, shootAngle: 90, projectileIndex: 0, fixedAngle: 180, coolDown: 300)
                    )
                )
            )
        //minion#10  
        .Init("Wandering Soul",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new Follow(0.7, 8, 1),
                    new Wander(0.4),
                    new Shoot(10, count: 5, shootAngle: 15, projectileIndex: 0, coolDown: 1200, coolDownOffset: 1200),
                    new Shoot(10, count: 10, shootAngle: 36, projectileIndex: 1, coolDown: 2400),
                    new TimedTransition(4800, "2")
                    ),
                new State("2",
                    new SpiralShoot(rotateAngle: 5, projectileIndex: 0, numShots: 4, shotsToRestart: 30, shootAngle: 90, coolDown: 200, fixedAngle: 0),
                    new TimedTransition(5000, "1")
                    )
                )
            )
           //minion#11
           .Init("Green Parasite",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Follow(0.7, 8, 1),
                    new Wander(0.4),
                    new Shoot(10, count: 3, shootAngle: 10, projectileIndex: 0, coolDown: 2000),
                    new Shoot(10, count: 3, shootAngle: 10, projectileIndex: 1, coolDown: 2000),
                    new Shoot(10, count: 3, shootAngle: 10, projectileIndex: 2, coolDown: 2000),
                    new Shoot(10, count: 3, shootAngle: 10, projectileIndex: 3, coolDown: 2000),
                    new Shoot(10, count: 3, shootAngle: 10, projectileIndex: 4, coolDown: 2000)
                    )
                )
            )
           //Luna's Moons
           .Init("Moon of Luna",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new Orbit(0.5, 4, 10, "Luna, the Mischievous"),
                    new Shoot(10, count: 4, projectileIndex: 0, shootAngle: 90, coolDown: 1000),
                    new Shoot(10, count: 4, projectileIndex: 0, shootAngle: 90, coolDown: 1000)
                    )
                )
             )
        #endregion
        #region Boss + Objects
                    .Init("Somnus, the Dream Entity",
                       new State(
                           new ChangeMusicOnDeath("DreamWorldBoss2"),
                           new ScaleHP(0.3),
                           new State("Idle",
                           new SetAltTexture(0),
                           new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                           new PlayerWithinTransition(10, "start")
                           ),
                       new State("start",
                           new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                           new Taunt("I have waited eons for this day to arrive..."),
                           new TimedTransition(3000, "1")
                           ),
                       new State("1",
                           new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                           new Taunt("Soon the realm will be mine, and you fools will not get in the way!"),
                           new InvisiToss("Somnus Anchor", range: 0, angle: 0, coolDown: 99999),
                           new Flash(0xff0000, 1, 7),
                           new TimedTransition(7000, "2")
                           ),
                       new State("2",
                           new Order(100, "DW Ground Changer 1", "boom"),
                           new ChangeMusic("DreamWorldBoss1"),
                           new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                           new Taunt("Minions, to arms!"),
                           new TossObject("Etrigan the Ravager", range: 8, angle: 90, coolDown: 9000),
                           new TossObject("Farkal the Void", range: 8, angle: 180, coolDown: 9000),
                           new TossObject("Lokee the Sadness", range: 8, angle: 0, coolDown: 9000),
                           new TossObject("Eye of Omniscience", range: 8, angle: 225, coolDown: 9000),
                           new TossObject("Aluz'kha the Chaos", range: 8, angle: 315, coolDown: 9000),
                           new TimedTransition(2100, "checkminions1")
                           ),
                       new State("checkminions1",
                           new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                           new EntitiesNotExistsTransition(9999, "3", "Etrigan the Ravager", "Farkal the Void", "Lokee the Sadness", "Eye of Omniscience", "Aluz'kha the Chaos")
                           ),
                       new State("3",
                           new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                           new Taunt("Enough! You will meet your demise!"),
                           new TossObject("Wandering Soul", range: 8, angle: 90, coolDown: 9000),
                           new TossObject("Green Parasite", range: 8, angle: 180, coolDown: 9000),
                           new TossObject("Naiad the Feared One", range: 8, angle: 0, coolDown: 9000),
                           new TossObject("The Sleepwalker", range: 8, angle: 225, coolDown: 9000),
                           new TossObject("Commander of Dreams", range: 8, angle: 315, coolDown: 9000),
                           new TimedTransition(2100, "checkminions2")
                           ),
                      new State("checkminions2",
                           new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                           new EntitiesNotExistsTransition(9999, "prep", "Wandering Soul", "Green Parasite", "Naiad the Feared One", "The Sleepwalker", "Commander of Dreams")
                           ),
                      new State("prep",
                          new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                          new Taunt("I will take care of you myself!"),
                          new Flash(0xff0000, 1, 7),
                          new TimedTransition(4000, "StateChooser")
                          ),
                      new State("StateChooser",
                          new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                          new TimedTransition(1000, "4prep", true),
                          new TimedTransition(1000, "6prep", true),
                          new TimedTransition(1000, "39", true),
                          new TimedTransition(1000, "41", true),
                          //back to mid
                          new TimedTransition(3000, "5prep", true),
                          new TimedTransition(3000, "7", true),
                          new TimedTransition(3000, "9prep", true)
                          ),
                      new State("4prep",
                          new Taunt("I will shatter you into nothingness!"),
                          new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                          new TimedTransition(3000, "4")
                          ),
                      new State("4",
                          new Follow(1, 8, 1),
                          new Wander(0.5),
                          new Shoot(50, projectileIndex: 0, shootAngle: 11, count: 5, coolDown: 1500, coolDownOffset: 1200),
                          new Shoot(50, projectileIndex: 1, shootAngle: 36, count: 10, coolDown: 1500, coolDownOffset: 1400, fixedAngle: 45),
                          new Shoot(50, projectileIndex: 2, shootAngle: 36, count: 10, coolDown: 1500, coolDownOffset: 1400, fixedAngle: 45),
                          new Shoot(50, projectileIndex: 3, shootAngle: 36, count: 10, coolDown: 1500, coolDownOffset: 1400, fixedAngle: 45),
                          new TimedTransition(10000, "StateChooser"),
                          new HpLessTransition(0.3, "10")
                          ),
                      new State("5prep",
                          new ReturnToSpawn(2.3),
                          new Taunt("The universe resonates within me! You cannot hide!"),
                          new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                          new TimedTransition(4000, "5")
                          ),
                      new State("5",
                          new Orbit(1.3, 10, 150, target: "Somnus Anchor", orbitClockwise: true),
                          new Shoot(50, projectileIndex: 24, shootAngle: 72, count: 5, coolDown: 800, coolDownOffset: 1200, fixedAngle: 0),
                          new Shoot(50, projectileIndex: 25, shootAngle: 36, count: 10, coolDown: 800, coolDownOffset: 1400, fixedAngle: 45),
                          new Shoot(50, projectileIndex: 24, shootAngle: 72, count: 5, coolDown: 800, coolDownOffset: 1600, fixedAngle: 15),
                          new Shoot(50, projectileIndex: 25, shootAngle: 36, count: 10, coolDown: 800, coolDownOffset: 1800, fixedAngle: 35),
                          new Shoot(50, projectileIndex: 24, shootAngle: 72, count: 5, coolDown: 800, coolDownOffset: 2000, fixedAngle: 60),
                          new Shoot(50, projectileIndex: 25, shootAngle: 36, count: 10, coolDown: 800, coolDownOffset: 2200, fixedAngle: 50),
                          new TimedTransition(10000, "StateChooser"),
                          new HpLessTransition(0.3, "10")
                          ),
                      new State("6prep",
                          new ReturnToSpawn(2.3),
                          new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                          new Taunt("I will strike you down!"),
                          new TimedTransition(2000, "6")
                          ),
                      new State("6",
                          new Shoot(50, projectileIndex: 0, shootAngle: 10, count: 3, coolDown: 2000),
                          new Shoot(50, projectileIndex: 5, count: 20, shootAngle: 18, coolDown: 1500, coolDownOffset: 1200),
                          new Shoot(50, projectileIndex: 4, count: 20, shootAngle: 18, coolDown: 1500, coolDownOffset: 1600),
                          new Shoot(50, count: 12, fixedAngle: 0, shootAngle: 45, projectileIndex: 6, coolDown: 3000, coolDownOffset: 1600),
                          new Shoot(50, count: 12, fixedAngle: 0, shootAngle: 45, projectileIndex: 7, coolDown: 3000, coolDownOffset: 1600),
                          new TimedTransition(10000, "StateChooser"),
                          new HpLessTransition(0.3, "10")
                          ),
                      new State("7",
                          new ReturnToSpawn(2.3),
                          new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                          new Taunt("Perish, unsightly blights!"),
                          new TimedTransition(4000, "8")
                          ),
                      new State("8",
                          new Shoot(80, count: 12, fixedAngle: 0, shootAngle: 45, projectileIndex: 17, coolDown: 2500),
                          new Shoot(80, count: 12, fixedAngle: 0, shootAngle: 45, projectileIndex: 6, coolDown: 2500),
                          new Shoot(80, count: 12, fixedAngle: 0, shootAngle: 45, projectileIndex: 7, coolDown: 2500),
                          new Shoot(80, projectileIndex: 8, count: 20, shootAngle: 18, fixedAngle: 0, coolDown: 3000, coolDownOffset: 0),
                          new Shoot(80, projectileIndex: 9, count: 20, shootAngle: 18, fixedAngle: 0, coolDown: 3000, coolDownOffset: 0),
                          new Shoot(80, projectileIndex: 10, count: 20, shootAngle: 18, fixedAngle: 0, coolDown: 3000, coolDownOffset: 0),
                          new Shoot(80, projectileIndex: 8, count: 20, shootAngle: 18, fixedAngle: 45, coolDown: 3000, coolDownOffset: 500),
                          new Shoot(80, projectileIndex: 9, count: 20, shootAngle: 18, fixedAngle: 45, coolDown: 3000, coolDownOffset: 500),
                          new Shoot(80, projectileIndex: 10, count: 20, shootAngle: 18, fixedAngle: 45, coolDown: 3000, coolDownOffset: 500),
                          new TimedTransition(10000, "StateChooser"),
                          new HpLessTransition(0.3, "10")
                           ),
                       new State("9prep",
                          new ReturnToSpawn(2.3),
                          new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                          new Taunt("Feel the cosmic wrath!"),
                          new TimedTransition(4000, "9")
                          ),
                      new State("9",
                          new Shoot(50, projectileIndex: 0, shootAngle: 11, count: 5, coolDown: 1500),
                          new Shoot(50, projectileIndex: 8, count: 10, shootAngle: 36, fixedAngle: 0, coolDown: 2500, coolDownOffset: 0),
                          new Shoot(50, projectileIndex: 9, count: 10, shootAngle: 36, fixedAngle: 0, coolDown: 2500, coolDownOffset: 0),
                          new Shoot(50, projectileIndex: 10, count: 10, shootAngle: 36, fixedAngle: 0, coolDown: 2500, coolDownOffset: 0),
                          new Shoot(50, projectileIndex: 11, count: 2, fixedAngle: 0, shootAngle: 0, coolDown: 200),
                          new Shoot(50, projectileIndex: 11, count: 2, fixedAngle: 90, shootAngle: 0, coolDown: 200),
                          new Shoot(50, projectileIndex: 11, count: 2, fixedAngle: 180, shootAngle: 0, coolDown: 200),
                          new Shoot(50, projectileIndex: 11, count: 2, fixedAngle: 270, shootAngle: 0, coolDown: 200),
                          new Shoot(50, projectileIndex: 12, count: 20, shootAngle: 18, coolDown: 3000),
                          new TimedTransition(10000, "StateChooser"),
                          new HpLessTransition(0.3, "10")
                          ),
                      new State("39",
                          new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                          new Taunt("There's nowhere to run!"),
                          new TimedTransition(2000, "40")
                          ),
                     new State("40",
                          new Orbit(1.4, 15, 20, target: "Somnus Anchor", orbitClockwise: true),
                          new Shoot(50, projectileIndex: 22, count: 10, shootAngle: 36, fixedAngle: 0, coolDown: 2000, coolDownOffset: 0),
                          new Shoot(50, projectileIndex: 21, count: 10, shootAngle: 36, fixedAngle: 0, coolDown: 2000, coolDownOffset: 0),
                          new Shoot(50, projectileIndex: 20, count: 10, shootAngle: 36, fixedAngle: 0, coolDown: 2000, coolDownOffset: 0),
                          new Shoot(50, projectileIndex: 22, count: 10, shootAngle: 48, fixedAngle: 0, coolDown: 2000, coolDownOffset: 400),
                          new Shoot(50, projectileIndex: 21, count: 10, shootAngle: 48, fixedAngle: 0, coolDown: 2000, coolDownOffset: 400),
                          new Shoot(50, projectileIndex: 20, count: 10, shootAngle: 48, fixedAngle: 0, coolDown: 2000, coolDownOffset: 400),
                          new Shoot(50, projectileIndex: 22, count: 10, shootAngle: 60, fixedAngle: 0, coolDown: 2000, coolDownOffset: 600),
                          new Shoot(50, projectileIndex: 21, count: 10, shootAngle: 60, fixedAngle: 0, coolDown: 2000, coolDownOffset: 600),
                          new Shoot(50, projectileIndex: 20, count: 10, shootAngle: 60, fixedAngle: 0, coolDown: 2000, coolDownOffset: 600),
                          new TimedTransition(10000, "StateChooser"),
                          new HpLessTransition(0.3, "10")
                          ),
                    new State("41",
                          new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                          new Taunt("I will chase you down to the ends of the universe!"),
                          new TimedTransition(2000, "42")
                          ),
                     new State("42",
                          new Follow(1.2, 8, 1),
                          new Wander(0.6),
                          new SpiralShoot(rotateAngle: 45, shotsToRestart: 42, numShots: 6, shootAngle: 60, projectileIndex: 23, coolDown: 1300),
                          new Shoot(50, projectileIndex: 22, count: 10, shootAngle: 36, fixedAngle: 24, coolDown: 2000, coolDownOffset: 0),
                          new Shoot(50, projectileIndex: 22, count: 10, shootAngle: 36, fixedAngle: 48, coolDown: 2000, coolDownOffset: 0),
                          new TimedTransition(10000, "StateChooser"),
                          new HpLessTransition(0.3, "10")
                          ),
                      new State("10",
                          new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                          new SetAltTexture(1),
                          new InvisiToss("DW Crystal 4", range: 0, angle: 0, coolDown: 99999),
                          new InvisiToss("DW Crystal 5", range: 0, angle: 0, coolDown: 99999),
                          new InvisiToss("DW Crystal 6", range: 0, angle: 0, coolDown: 99999),
                          new TimedTransition(1000, "11")
                          ),
                      new State("11",
                          new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                          new Taunt("Suffer for eternity!"),
                          new Spawn("DW Rocket", 1000, 0.001, coolDown: 3000),
                          new Shoot(50, projectileIndex: 8, count: 5, shootAngle: 72, fixedAngle: 0, coolDown: 3500),
                          new Shoot(50, projectileIndex: 9, count: 5, shootAngle: 72, fixedAngle: 0, coolDown: 3500),
                          new Shoot(50, projectileIndex: 10, count: 5, shootAngle: 72, fixedAngle: 0, coolDown: 3500),
                          new SpiralShoot(rotateAngle: 3, projectileIndex: 13, shotsToRestart: 100, numShots: 3, shootAngle: 120, coolDown: 100),
                          new Shoot(50, count: 12, fixedAngle: 0, shootAngle: 45, projectileIndex: 6, coolDown: 2500),
                          new Shoot(50, count: 12, fixedAngle: 0, shootAngle: 45, projectileIndex: 7, coolDown: 2500),
                          new Shoot(50, count: 12, fixedAngle: 0, shootAngle: 45, projectileIndex: 17, coolDown: 2500),
                          new Shoot(50, count: 4, fixedAngle: 0, shootAngle: 90, projectileIndex: 14, coolDown: 4000),
                          new EntitiesNotExistsTransition(100, "12", "DW Crystal 4", "DW Crystal 5", "DW Crystal 6")
                          ),
                      new State("12",
                          new SetAltTexture(2),
                          new Taunt("No... You won't prevail..."),
                          new Follow(0.4, 8, 1),
                          new Wander(0.4),
                          new Spawn("DW Rocket", 1000, 0.001, coolDown: 3000),
                          new Shoot(50, count: 1, projectileIndex: 14, coolDown: 4000),
                          new Shoot(50, projectileIndex: 1, shootAngle: 60, count: 6, coolDown: 2000, fixedAngle: 45),
                          new Shoot(50, projectileIndex: 2, shootAngle: 60, count: 6, coolDown: 2000, fixedAngle: 45),
                          new Shoot(50, projectileIndex: 3, shootAngle: 60, count: 6, coolDown: 2000, fixedAngle: 45),
                          new HpLessTransition(0.1, "rage")
                          ),
                      new State("rage",
                          new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                          new ReturnToSpawn(speed: 2),
                          new Taunt("YOU WILL COME WITH ME!!!"),
                          new TimedTransition(3000, "rage2prep")
                          ),
                      new State("rage2prep",
                          new Shoot(50, projectileIndex: 15, shootAngle: 9, count: 15, coolDown: 200, fixedAngle: 270),
                          new TimedTransition(600, "rage2")
                          ),
                      new State("rage2",
                          new Shoot(50, projectileIndex: 16, shootAngle: 9, count: 15, coolDown: 200, fixedAngle: 270),
                          new TimedTransition(3000, "rage3prep")
                          ),
                      new State("rage3prep",
                          new Shoot(50, projectileIndex: 15, shootAngle: 9, count: 15, coolDown: 200, fixedAngle: 0),
                          new TimedTransition(600, "rage3")
                          ),
                      new State("rage3",
                          new Shoot(50, projectileIndex: 16, shootAngle: 9, count: 15, coolDown: 200, fixedAngle: 0),
                          new TimedTransition(3000, "rage4prep")
                          ),
                      new State("rage4prep",
                          new Shoot(50, projectileIndex: 15, shootAngle: 9, count: 15, coolDown: 200, fixedAngle: 90),
                          new TimedTransition(600, "rage4")
                          ),
                      new State("rage4",
                          new Shoot(50, projectileIndex: 16, shootAngle: 9, count: 15, coolDown: 200, fixedAngle: 90),
                          new TimedTransition(3000, "rage5prep")
                          ),
                      new State("rage5prep",
                          new Shoot(50, projectileIndex: 15, shootAngle: 9, count: 15, coolDown: 200, fixedAngle: 180),
                          new TimedTransition(600, "rage5")
                          ),
                      new State("rage5",
                          new Shoot(50, projectileIndex: 16, shootAngle: 9, count: 15, coolDown: 200, fixedAngle: 180),
                          new TimedTransition(3000, "rage6prep")
                          ),
                      new State("rage6prep",
                          new Shoot(50, projectileIndex: 15, shootAngle: 9, count: 15, coolDown: 200, fixedAngle: 0),
                          new Shoot(50, projectileIndex: 15, shootAngle: 9, count: 15, coolDown: 200, fixedAngle: 180),
                          new TimedTransition(600, "rage6")
                          ),
                      new State("rage6",
                          new Shoot(50, projectileIndex: 16, shootAngle: 9, count: 15, coolDown: 200, fixedAngle: 0),
                          new Shoot(50, projectileIndex: 16, shootAngle: 9, count: 15, coolDown: 200, fixedAngle: 180),
                          new TimedTransition(3000, "rage7prep")
                          ),
                      new State("rage7prep",
                          new Shoot(50, projectileIndex: 15, shootAngle: 9, count: 15, coolDown: 200, fixedAngle: 90),
                          new Shoot(50, projectileIndex: 15, shootAngle: 9, count: 15, coolDown: 200, fixedAngle: 270),
                          new TimedTransition(600, "rage7")
                          ),
                       new State("rage7",
                          new Shoot(50, projectileIndex: 16, shootAngle: 9, count: 15, coolDown: 200, fixedAngle: 90),
                          new Shoot(50, projectileIndex: 16, shootAngle: 9, count: 15, coolDown: 200, fixedAngle: 270),
                          new TimedTransition(3000, "13")
                          ),
                       new State("13",
                          new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                          new Taunt("NO!!!"),
                          new Flash(0xff0000, 0.5, 10),
                          new TimedTransition(4000, "14")
                          ),
                      new State("14",
                          new SetAltTexture(3),
                          new TimedTransition(100, "15")
                          ),
                      new State("15",
                          new SetAltTexture(4),
                          new TimedTransition(100, "16")
                          ),
                      new State("16",
                          new SetAltTexture(5),
                          new TimedTransition(100, "17")
                          ),
                      new State("17",
                          new SetAltTexture(6),
                          new TimedTransition(100, "18")
                          ),
                      new State("18",
                          new SetAltTexture(7),
                          new TimedTransition(5000, "19")
                          ),
                      new State("19",
                          new Flash(0x000000, 1, 7),
                          new Taunt("Fools, you won't get away with your insolence!"),
                          new InvisiToss("DW Spawner", range: 0, angle: 0, coolDown: 99999),
                          new InvisiToss("DW Ground Changer 1", range: 0, angle: 0, coolDown: 99999),
                          new TimedTransition(3000, "20")
                          ),
                      new State("20",
                          new SetAltTexture(8),
                          new TimedTransition(100, "21")
                          ),
                      new State("21",
                          new SetAltTexture(9),
                          new TimedTransition(100, "22")
                          ),
                      new State("22",
                          new SetAltTexture(10),
                          new TimedTransition(100, "23")
                          ),
                      new State("23",
                          new SetAltTexture(11),
                          new TimedTransition(100, "24")
                          ),
                      new State("24",
                          new SetAltTexture(12),
                          new TimedTransition(100, "25")
                          ),
                      new State("25",
                          new SetAltTexture(13),
                          new TimedTransition(100, "26")
                          ),
                      new State("26",
                          new SetAltTexture(14),
                          new TimedTransition(100, "27")
                          ),
                      new State("27",
                          new SetAltTexture(15),
                          new TimedTransition(100, "28")
                          ),
                      new State("28",
                          new SetAltTexture(16),
                          new TimedTransition(100, "29")
                          ),
                      new State("29",
                          new SetAltTexture(17),
                          new TimedTransition(100, "30")
                          ),
                      new State("30",
                          new SetAltTexture(18),
                          new TimedTransition(100, "31")
                          ),
                      new State("31",
                          new SetAltTexture(19),
                          new TimedTransition(100, "32")
                          ),
                      new State("32",
                          new SetAltTexture(20),
                          new TimedTransition(100, "33")
                          ),
                      new State("33",
                          new SetAltTexture(19),
                          new TimedTransition(100, "34")
                          ),
                      new State("34",
                          new SetAltTexture(20),
                          new TimedTransition(100, "35")
                          ),
                      new State("35",
                          new SetAltTexture(21),
                          new TimedTransition(100, "36")
                          ),
                      new State("36",
                          new SetAltTexture(22),
                          new TimedTransition(100, "37")
                          ),
                      new State("37",
                          new SetAltTexture(23),
                          new TimedTransition(100, "38")
                          ),
                      new State("38",
                          new Order(50, "DW Spawner", "boom"),
                          new Suicide()
                          )
                        )
                     )
           .Init("True Somnus, the Dream Entity",
              new State(
                  new ScaleHP(0.3),
                  new State("1",
                      new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                      new Taunt("I will not rest until the realms are mine!"),
                      new Flash(0xff0000, 1, 16),
                      new TimedTransition(7000, "2")
                      ),
                  new State("2",
                      new Follow(0.5, 8, 1),
                      new Wander(0.5),
                      new Shoot(50, projectileIndex: 0, count: 11, shootAngle: 11, coolDown: 3000, coolDownOffset: 0),
                      new Shoot(50, projectileIndex: 1, count: 6, shootAngle: 60, coolDown: 3000, coolDownOffset: 1000),
                      new Shoot(50, projectileIndex: 2, count: 6, shootAngle: 60, coolDown: 3000, coolDownOffset: 1000),
                      new Shoot(50, projectileIndex: 3, count: 6, shootAngle: 60, coolDown: 3000, coolDownOffset: 1000),
                      new Shoot(50, projectileIndex: 6, count: 8, shootAngle: 45, coolDown: 2000),
                      new HpLessTransition(0.8, "3")
                      ),
                  new State("3",
                      new Taunt("Hahaha, tremble beneath the cosmic might!"),
                      new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                      new Flash(0xff0000, 1, 10),
                      new ReturnToSpawn(speed: 2),
                      new TimedTransition(3000, "4prep")
                      ),
                  new State("4prep",
                      new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                      new Shoot(50, count: 20, shootAngle: 18, projectileIndex: 4, fixedAngle: 0, coolDown: 100),
                      new TimedTransition(1200, "4")
                      ),
                  new State("4",
                      new Shoot(50, count: 20, shootAngle: 18, projectileIndex: 5, fixedAngle: 0, coolDown: 10000, coolDownOffset: 0),
                      new Shoot(50, count: 20, shootAngle: 18, projectileIndex: 5, fixedAngle: 0, coolDown: 10000, coolDownOffset: 200),
                      new Shoot(50, count: 20, shootAngle: 18, projectileIndex: 5, fixedAngle: 0, coolDown: 10000, coolDownOffset: 400),
                      new Shoot(50, count: 20, shootAngle: 18, projectileIndex: 5, fixedAngle: 0, coolDown: 10000, coolDownOffset: 600),
                      new Shoot(50, count: 20, shootAngle: 18, projectileIndex: 5, fixedAngle: 0, coolDown: 10000, coolDownOffset: 800),
                      new TimedTransition(1000, "5")
                      ),
                  new State("5",
                      new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                      new Follow(0.8, 8, 1),
                      new Wander(0.8),
                      new Taunt("You are nothing but a nuisance!"),
                      new Shoot(50, count: 30, shootAngle: 12, projectileIndex: 7, coolDown: 2000, coolDownOffset: 0),
                      new Shoot(50, count: 30, shootAngle: 12, projectileIndex: 8, coolDown: 2000, coolDownOffset: 600),
                      new Shoot(50, count: 30, shootAngle: 12, projectileIndex: 9, coolDown: 2000, coolDownOffset: 1200),
                      new Shoot(50, count: 4, shootAngle: 11, projectileIndex: 10, coolDown: 2000, coolDownOffset: 1200, fixedAngle: 0),
                      new Shoot(50, count: 4, shootAngle: 11, projectileIndex: 10, coolDown: 2000, coolDownOffset: 1200, fixedAngle: 90),
                      new Shoot(50, count: 4, shootAngle: 11, projectileIndex: 10, coolDown: 2000, coolDownOffset: 1200, fixedAngle: 180),
                      new Shoot(50, count: 4, shootAngle: 11, projectileIndex: 10, coolDown: 2000, coolDownOffset: 1200, fixedAngle: 270),
                      new HpLessTransition(0.6, "6")
                      ),
                  new State("6",
                      new Follow(0.6, 8, 1),
                      new Wander(0.6),
                      new Grenade(radius: 50, range: 5, damage: 100, coolDown: 3000),
                      new Shoot(50, count: 8, shootAngle: 18, projectileIndex: 11, coolDown: 2000, coolDownOffset: 0),
                      new Shoot(80, projectileIndex: 12, count: 15, shootAngle: 24, fixedAngle: 0, coolDown: 3000, coolDownOffset: 1000),
                      new Shoot(80, projectileIndex: 13, count: 15, shootAngle: 24, fixedAngle: 0, coolDown: 3000, coolDownOffset: 1000),
                      new Shoot(80, projectileIndex: 14, count: 15, shootAngle: 24, fixedAngle: 0, coolDown: 3000, coolDownOffset: 1000),
                      new Shoot(80, projectileIndex: 12, count: 15, shootAngle: 24, fixedAngle: 0, coolDown: 3000, coolDownOffset: 2000),
                      new Shoot(80, projectileIndex: 13, count: 15, shootAngle: 24, fixedAngle: 0, coolDown: 3000, coolDownOffset: 2000),
                      new Shoot(80, projectileIndex: 14, count: 15, shootAngle: 24, fixedAngle: 0, coolDown: 3000, coolDownOffset: 2000),
                      new HpLessTransition(0.2, "7")
                      ),
                  new State("7",
                      new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                      new Flash(0xff0000, 1, 20),
                      new ReturnToSpawn(speed: 2),
                      new Taunt("Very well. Let's see how far your determination takes you!"),
                      new TimedTransition(7000, "8")
                      ),
                  new State("8",
                      new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                      new Orbit(1.5, 20, 30, "Somnus Anchor"),
                      new Shoot(50, projectileIndex: 0, count: 11, shootAngle: 11, coolDown: 3000),
                      new Shoot(50, projectileIndex: 1, count: 6, shootAngle: 60, coolDown: 3000),
                      new Shoot(50, projectileIndex: 2, count: 6, shootAngle: 60, coolDown: 3000),
                      new Shoot(50, projectileIndex: 3, count: 6, shootAngle: 60, coolDown: 3000),
                      new Shoot(50, count: 8, shootAngle: 17, projectileIndex: 15, coolDown: 500),
                      new Shoot(50, count: 5, shootAngle: 72, projectileIndex: 16, coolDown: 2000),
                      new HpLessTransition(0.7, "9")
                      ),
                  new State("9",
                      new ReturnToSpawn(2),
                      new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                      new Taunt("You reach your end now!"),
                      new TimedTransition(3000, "10")
                      ),
                  new State("10",
                      new ExplodingShoot(50, count: 5, shootAngle: 72, projectileIndex: 16, explodeIndex: 17, explodeCount: 10, coolDown: 2000, coolDownOffset: 0, explodeAngle: 36),
                      new ExplodingShoot(50, count: 5, shootAngle: 72, projectileIndex: 16, explodeIndex: 17, explodeCount: 10, coolDown: 2000, coolDownOffset: 1400, explodeAngle: 36),
                      new HpLessTransition(0.4, "11")
                      ),
                  new State("11",
                      new ConditionalEffect(ConditionEffectIndex.Invincible),
                      new Taunt("Crystals, do your job!"),
                      new TossObject("DW Crystal 1", range: 0, angle: 0, coolDown: 99999),
                      new TossObject("DW Crystal 2", range: 3, angle: 0, coolDown: 99999),
                      new TossObject("DW Crystal 3", range: 5, angle: 0, coolDown: 99999),                     
                      new TimedTransition(2000, "checkdwcrystals")
                      ),
                  new State("checkdwcrystals",
                      new ConditionalEffect(ConditionEffectIndex.Invincible),
                      new SpiralShoot(rotateAngle: 10, projectileIndex: 18, shotsToRestart: 1000, numShots: 4, shootAngle: 90, coolDown: 500),
                      new EntitiesNotExistsTransition(100, "12", "DW Crystal 1", "DW Crystal 2", "DW Crystal 3", "DW Crystal Mini")
                      ),
                  new State("12",
                      new Taunt("NO! I WON'T ALLOW MY EFFORTS TO GO TO WASTE!"),
                      new Follow(0.8, 8, 1),
                      new Wander(0.8),
                      new TossObject("DW Crystal 1", range: 0, angle: 0, coolDown: 99999),
                      new TossObject("DW Crystal 2", range: 3, angle: 0, coolDown: 99999),
                      new TossObject("DW Crystal 3", range: 5, angle: 0, coolDown: 99999),
                      new Shoot(50, projectileIndex: 0, count: 11, shootAngle: 11, coolDown: 2000, coolDownOffset: 0),
                      new Shoot(50, count: 30, shootAngle: 12, projectileIndex: 7, coolDown: 2000, coolDownOffset: 1600),
                      new Shoot(50, count: 30, shootAngle: 12, projectileIndex: 8, coolDown: 2000, coolDownOffset: 2200),
                      new Shoot(50, count: 30, shootAngle: 12, projectileIndex: 9, coolDown: 2000, coolDownOffset: 2800),
                      new HpLessTransition(0.1, "13")
                      ),
                  new State("13",
                      new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                      new Taunt("FOOLS, I AM IMMORTAL! YOU WILL NOT HAVE ME!"),
                      new ReturnToSpawn(2),
                      new TimedTransition(4000, "14")
                      ),
                  new State("14",
                      new TossObject("DW Somnus Clone 1", range: 5, angle: 0, coolDown: 99999),
                      new TossObject("DW Somnus Clone 2", range: 5, angle: 270, coolDown: 99999),
                      new TossObject("DW Somnus Clone 3", range: 5, angle: 180, coolDown: 99999),
                      new TossObject("DW Somnus Clone 4", range: 5, angle: 90, coolDown: 99999),
                      new TimedTransition(2000, "checkclones")
                      ),
                  new State("checkclones",
                      new EntitiesNotExistsTransition(100, "Finalprep", "DW Somnus Clone 1", "DW Somnus Clone 2", "DW Somnus Clone 3", "DW Somnus Clone 4")
                      ),
                  new State("Finalprep",
                      new Flash(0xff0000, 0.7, 8),
                      new Taunt("No, no, no, no!!!"),
                      new TimedTransition(3000, "Final")
                      ),
                  new State("Final",
                      new Flash(0xff0000, 1, 40),
                      new Taunt("THE REALMS SHALL FALL WITH MY DEMISE!!!"),
                      new Shoot(50, count: 40, shootAngle: 9, projectileIndex: 19, fixedAngle: 0, coolDown: 500),
                      new Shoot(50, count: 40, shootAngle: 9, projectileIndex: 21, fixedAngle: 0, coolDown: 500),
                      new Shoot(50, count: 5, shootAngle: 72, projectileIndex: 20, fixedAngle: 0, coolDown: 2000, coolDownOffset: 0),
                      new Shoot(50, count: 5, shootAngle: 72, projectileIndex: 20, fixedAngle: 45, coolDown: 2000, coolDownOffset: 1400),
                      new TimedTransition(20000, "dying")
                      ),
                  new State("dying",
                      new Flash(0x000000, 1, 10),
                      new Taunt("No... this wasn't how the realms were supposed to be... One day, YOU WILL ALL PERISH."),
                      new TimedTransition(5000, "boom")
                      ),
                  new State("boom",
                      new Suicide() 
                      )
                      ),
                new Threshold(0.01,
                    LootTemplates.VialDropDW()
                    )//,
                )
                 /* new Threshold(0.01,
                      new ItemLoot("Greater Potion of Might", 1),
                      new ItemLoot("Greater Potion of Luck", 1),
                      new ItemLoot("Greater Potion of Restoration", 1),
                      new ItemLoot("Greater Potion of Protection", 1),
                      new ItemLoot("Greater Potion of Life", 1),
                      new ItemLoot("Greater Potion of Mana", 1),
                      new ItemLoot("Greater Potion of Vitality", 1),
                      new ItemLoot("Greater Potion of Dexterity", 1),
                      new ItemLoot("Greater Potion of Speed", 1),
                      new ItemLoot("Greater Potion of Attack", 1),
                      new ItemLoot("Greater Potion of Defense", 1),
                      new ItemLoot("Greater Potion of Wisdom", 1),
                      new ItemLoot("Ultimate Onrane Cache", 0.005),
                      new ItemLoot("Onrane Cache", 1),
                      new ItemLoot("10000 Gold", 1),
                      new ItemLoot("Master Eon", 0.001),
                      new ItemLoot("Life Eon", 0.01),
                      new ItemLoot("Mana Eon", 0.01)
                      )
            )*/
           .Init("DW Ground Changer 1",
            new State(
                new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("boom",
                    new ChangeGroundOnDeath(new[] { "DW Tile9", "DW Carpet3", "DW Carpet2", "DW Carpet5" }, new[] { "DW Unwalk" }, 1),
                    new Suicide()
                    )
                )
            )
           .Init("DW Wall Remover",
            new State(
                new RemoveObjectOnDeath("DW Wall4", 10),
                new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new EntitiesNotExistsTransition(2000, "boom", "Blue Wisp", "Yellow Wisp", "Red Wisp")
                    ),
                new State("boom",
                    new Suicide()
                    )
                )
            )
           .Init("DW Ground Changer 2",
            new State(
                new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("boom",
                    new ChangeGroundOnDeath(new[] { "DW Galaxy Tile1", "DW Galaxy Tile2", "DW Galaxy Tile3", "DW Galaxy Tile4", "DW Galaxy Tile5", "DW Galaxy Tile6", "DW Galaxy Tile7", "DW Galaxy Tile8", "DW Galaxy Tile9", "DW Galaxy Tile10", "DW Galaxy Tile11", "DW Galaxy Tile12", "DW Galaxy Tile13", "DW Galaxy Tile14", "DW Galaxy Tile15", "DW Galaxy Tile16", "DW Galaxy Tile17", "DW Galaxy Tile18", "DW Galaxy Tile19", "DW Galaxy Tile20", "DW Galaxy Tile21", "DW Galaxy Tile22", "DW Galaxy Tile23", "DW Galaxy Tile24", "DW Galaxy Tile25" }, new[] { "DW Lava1" }, 1),
                    new Suicide()
                    )
                )
            )
           .Init("DW Ground Changer 3",
            new State(
                new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("boom",
                    new ChangeGroundOnDeath(new[] { "DW Galaxy Tile1", "DW Galaxy Tile2", "DW Galaxy Tile3", "DW Galaxy Tile4", "DW Galaxy Tile5", "DW Galaxy Tile6", "DW Galaxy Tile7", "DW Galaxy Tile8", "DW Galaxy Tile9", "DW Galaxy Tile10", "DW Galaxy Tile11", "DW Galaxy Tile12", "DW Galaxy Tile13", "DW Galaxy Tile14", "DW Galaxy Tile15", "DW Galaxy Tile16", "DW Galaxy Tile17", "DW Galaxy Tile18", "DW Galaxy Tile19", "DW Galaxy Tile20", "DW Galaxy Tile21", "DW Galaxy Tile22", "DW Galaxy Tile23", "DW Galaxy Tile24", "DW Galaxy Tile25" }, new[] { "DW Lava1" }, 1),
                    new Suicide()
                    )
                )
            )

           .Init("DW Spawner",
            new State(
                new TransformOnDeath("True Somnus, the Dream Entity"),
                new State("idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("boom",
                    new Suicide()
                    )
                )
            )

           .Init("DW Minion Spawner",
            new State(
                new State("idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("spawn",
                    new TimedTransition(4000, "etrigan", true),
                    new TimedTransition(4000, "farkal", true),
                    new TimedTransition(4000, "sleepwalker", true),
                    new TimedTransition(4000, "parasite", true),
                    new TimedTransition(4000, "soul", true),
                    new TimedTransition(4000, "naiad", true),
                    new TimedTransition(4000, "lokee", true)
                    ),
                new State("etrigan",
                    new Spawn("Etrigan the Ravager", initialSpawn: 0.5, coolDown: 99999)
                    ),
                new State("farkal",
                    new Spawn("Farkal the Void", initialSpawn: 0.5, coolDown: 99999)
                    ),
                new State("sleepwalker",
                    new Spawn("The Sleepwalker", initialSpawn: 0.5, coolDown: 99999)
                    ),
                new State("parasite",
                    new Spawn("Green Parasite", initialSpawn: 0.5, coolDown: 99999)
                    ),
                new State("soul",
                    new Spawn("Wandering Soul", initialSpawn: 0.5, coolDown: 99999)
                    ),
                new State("naiad",
                    new Spawn("Naiad the Feared One", initialSpawn: 0.5, coolDown: 99999)
                    ),
                new State("lokee",
                    new Spawn("Lokee the Sadness", initialSpawn: 0.5, coolDown: 99999)

                    )
                )
            )

           .Init("Somnus Anchor",
            new State(
                new State("1",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    )
                )
            )
           .Init("DW Rocket",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                new State("idle",
                    new Flash(0xFFFFFF, 1, 3),
                    new Follow(0.8, 50, 5),
                    new TimedTransition(1000, "boom")
                    ),
                new State("boom",
                    new ExplodingShoot(50, projectileIndex: 0, count: 1, explodeIndex: 1, explodeCount: 15),
                    new Decay(0)
                    )
                )
            )

           .Init("DW Crystal 1",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new Orbit(0.5, 5, 30, "Somnus Anchor"),
                    new Shoot(50, count: 10, shootAngle: 36, projectileIndex: 0, coolDown: 3000),
                    new HpLessTransition(0.1, "mini")
                    ),
                new State("mini",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new TossObject("DW Crystal Mini", range: 3, angle: 0, coolDown: 99999),
                    new TossObject("DW Crystal Mini", range: 3, angle: 90, coolDown: 99999),
                    new TossObject("DW Crystal Mini", range: 3, angle: 180, coolDown: 99999),
                    new TossObject("DW Crystal Mini", range: 3, angle: 270, coolDown: 99999),
                    new TimedTransition(1000, "chase")
                    ),
                new State("chase",
                    new Flash(0xff0000, 0.5, 10),
                    new Charge(speed: 2, range: 50),
                    new TimedTransition(2000, "boom")
                    ),
                new State("boom",
                    new Shoot(50, count: 20, shootAngle: 18, projectileIndex: 0),
                    new Suicide()
                    )
                )
            )

           .Init("DW Crystal 2",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new Orbit(0.8, 10, 30, "Somnus Anchor", orbitClockwise: true),
                    new Shoot(50, count: 10, shootAngle: 36, projectileIndex: 0, coolDown: 3000),
                    new HpLessTransition(0.1, "mini")
                    ),
                new State("mini",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new TossObject("DW Crystal Mini", range: 3, angle: 0, coolDown: 99999),
                    new TossObject("DW Crystal Mini", range: 3, angle: 90, coolDown: 99999),
                    new TossObject("DW Crystal Mini", range: 3, angle: 180, coolDown: 99999),
                    new TossObject("DW Crystal Mini", range: 3, angle: 270, coolDown: 99999),
                    new TimedTransition(1000, "chase")
                    ),
                new State("chase",
                    new Flash(0xff0000, 0.5, 10),
                    new Charge(speed: 2, range: 50),
                    new TimedTransition(2000, "boom")
                    ),
                new State("boom",
                    new Shoot(50, count: 20, shootAngle: 18, projectileIndex: 0),
                    new Suicide()
                    )
                )
            )

           .Init("DW Crystal 3",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new Orbit(1.2, 15, 30, "Somnus Anchor"),
                    new Shoot(50, count: 10, shootAngle: 36, projectileIndex: 0, coolDown: 3000),
                    new HpLessTransition(0.1, "mini")
                    ),
                new State("mini",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new TossObject("DW Crystal Mini", range: 3, angle: 0, coolDown: 99999),
                    new TossObject("DW Crystal Mini", range: 3, angle: 90, coolDown: 99999),
                    new TossObject("DW Crystal Mini", range: 3, angle: 180, coolDown: 99999),
                    new TossObject("DW Crystal Mini", range: 3, angle: 270, coolDown: 99999),
                    new TimedTransition(1000, "chase")
                    ),
                new State("chase",
                    new Flash(0xff0000, 0.5, 10),
                    new Charge(speed: 2, range: 50),
                    new TimedTransition(2000, "boom")
                    ),
                new State("boom",
                    new Shoot(50, count: 20, shootAngle: 18, projectileIndex: 0),
                    new Suicide()
                    )
                )
            )

           .Init("DW Crystal 4",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new Orbit(1.2, 10, 30, "Somnus, the Dream Entity"),
                    new Shoot(50, count: 10, shootAngle: 20, projectileIndex: 0, coolDown: 3000)
                    )
                )
            )
           .Init("DW Crystal 5",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new Orbit(1.2, 15, 30, "Somnus, the Dream Entity"),
                    new Shoot(50, count: 10, shootAngle: 20, projectileIndex: 0, coolDown: 3000)
                    )
                )
            )
           .Init("DW Crystal 6",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new Orbit(1.2, 20, 30, "Somnus, the Dream Entity"),
                    new Shoot(50, count: 10, shootAngle: 20, projectileIndex: 0, coolDown: 3000)
                    )
                )
            )
           .Init("DW Crystal Mini",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new Follow(0.6, 8, 1),
                    new Wander(0.6),
                    new Shoot(50, count: 3, shootAngle: 20, projectileIndex: 0, coolDown: 1500)
                    )
                )
            )

           .Init("DW Somnus Clone 1",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new ConditionalEffect(ConditionEffectIndex.StunImmune, true),
                    new ConditionalEffect(ConditionEffectIndex.PetrifyImmune, true),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new MoveTo2(X: 10, Y: 0, speed: 1),
                    new TimedTransition(5000, "2")
                    ),
                new State("2",
                    new Orbit(0.8, 10, 30, "Somnus Anchor", orbitClockwise: true),
                    new Shoot(50, projectileIndex: 0, count: 8, shootAngle: 25, coolDown: 2000)
                    )
                )
            )

           .Init("DW Somnus Clone 2",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new ConditionalEffect(ConditionEffectIndex.StunImmune, true),
                    new ConditionalEffect(ConditionEffectIndex.PetrifyImmune, true),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new MoveTo2(X: 0, Y: -10, speed: 1),
                    new TimedTransition(5000, "2")
                    ),
                new State("2",
                    new Orbit(0.8, 10, 30, "Somnus Anchor", orbitClockwise: true),
                    new Shoot(50, projectileIndex: 0, count: 6, shootAngle: 13, coolDown: 2000),
                    new Shoot(50, projectileIndex: 1, count: 6, shootAngle: 13, coolDown: 2000),
                    new Shoot(50, projectileIndex: 2, count: 6, shootAngle: 13, coolDown: 2000)
                    )
                )
            )

           .Init("DW Somnus Clone 3",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new ConditionalEffect(ConditionEffectIndex.StunImmune, true),
                    new ConditionalEffect(ConditionEffectIndex.PetrifyImmune, true),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new MoveTo2(X: -10, Y: 0, speed: 1),
                    new TimedTransition(5000, "2")
                    ),
                new State("2",
                    new Orbit(0.8, 10, 30, "Somnus Anchor", orbitClockwise: true),
                    new Shoot(50, projectileIndex: 0, count: 8, shootAngle: 25, coolDown: 3000)
                    )
                )
            )

           .Init("DW Somnus Clone 4",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new ConditionalEffect(ConditionEffectIndex.StunImmune, true),
                    new ConditionalEffect(ConditionEffectIndex.PetrifyImmune, true),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new MoveTo2(X: 0, Y: 10, speed: 1),
                    new TimedTransition(5000, "2")
                    ),
                new State("2",
                    new Orbit(0.8, 10, 30, "Somnus Anchor", orbitClockwise: true),
                    new Shoot(50, projectileIndex: 0, count: 8, shootAngle: 13, coolDown: 2500)
                    )
                )
            )
            .Init("DW Pillar",
                new State(
                    new ScaleHP(0.1),
                    new State("idle",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new ConditionalEffect(ConditionEffectIndex.Stunned),
                        new TimedTransition(20000, "1")
                        ),
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Grenade(5, damage: 60, range: 7, coolDown: 3000),
                        new Shoot(150, count: 15, projectileIndex: 0, shootAngle: 24, coolDown: 2000),
                        new DamageTakenTransition(15000, "idle")
                        )
                        )
                    )

               .Init("Head Spawner",
                new State(
                    new State(
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntitiesNotExistsTransition(2000, "ItsGoTime", "Blue Wisp", "Red Wisp", "Yellow Wisp")
                        ),
                    new State("ItsGoTime",
                        new TossObject("Head Anchor", range: 0, angle: 0, coolDown: 99999),
                        new TossObject("Head of Wisdom", range: 0, angle: 0, coolDown: 99999),
                        new TossObject("Head of Power", range: 0, angle: 0, coolDown: 99999),
                        new TossObject("Head of Knowledge", range: 0, angle: 0, coolDown: 99999),
                        new TimedTransition(3000, "boom")
                        ),
                    new State("boom",
                        new Suicide()
                        )
                    )
                )
        .Init("Head of Wisdom",
            new State(
                new State("beginning",
                    new MoveTo2(X: 5, Y: -5, speed: 1),
                    new Taunt("Halt! You are trespassing on forbidden grounds!"),
                    new TimedTransition(5000, "prep")
                    ),
                new State("prep",
                    new Flash(0x00c9ff, 0.8, 10),
                    new Taunt("All heads, stop them at all costs!"),
                    new Orbit(0.8, 5, 10, "Head Anchor"),
                    new TimedTransition(4000, "1")
                    ),
                new State("1",
                    new Orbit(0.8, 5, 10, "Head Anchor"),
                    new Shoot(50, count: 5, shootAngle: 13, projectileIndex: 0, coolDown: 2000),
                    new HpLessTransition(0.4, "rage")
                    ),
                new State("rage",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new Flash(0xff0000, 1, 10),
                    new ChangeSize(rate: 13, target: 150),
                    new ReturnToSpawn(2),
                    new TimedTransition(5000, "rage1")
                    ),
                new State("rage1",
                    new TossObject("Head of Wisdom Mini", range: 3, angle: 0, coolDown: 99999),
                    new TossObject("Head of Wisdom Mini", range: 3, angle: 90, coolDown: 99999),
                    new TossObject("Head of Wisdom Mini", range: 3, angle: 180, coolDown: 99999),
                    new TossObject("Head of Wisdom Mini", range: 3, angle: 270, coolDown: 99999),
                    new Order(50, "Head Anchor", "blue"),
                    new TimedTransition(2000, "rage2")
                    ),
                new State("rage2",
                    new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Follow(0.6, 8, 1),
                    new Wander(0.6),
                    new Shoot(50, count: 20, shootAngle: 18, fixedAngle: 0, projectileIndex: 0, coolDown: 1500)
                    )
                )
            )
           .Init("Head of Power",
            new State(
                new State("beginning",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new MoveTo2(X: -5, Y: -5, speed: 1),
                    new Taunt("You will be judged by Somnus for passing beyond these points!"),
                    new TimedTransition(5000, "prep")
                    ),
                new State("prep",
                    new Flash(0x00c9ff, 0.8, 10),
                    new Orbit(0.8, 5, 10, "Head Anchor"),
                    new TimedTransition(4000, "pre1")
                    ),
                new State("pre1",
                    new Orbit(0.8, 5, 10, "Head Anchor"),
                    new Shoot(50, count: 3, shootAngle: 11, projectileIndex: 0, coolDown: 2000),
                    new EntitiesNotExistsTransition(50, "1", "Head of Wisdom")
                    ),
                new State("1",
                    new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Orbit(0.8, 5, 10, "Head Anchor"),
                    new Shoot(50, count: 5, shootAngle: 13, projectileIndex: 0, coolDown: 2000),
                    new HpLessTransition(0.4, "rage")
                    ),
                new State("rage",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new Flash(0xff0000, 1, 10),
                    new ChangeSize(rate: 13, target: 150),
                    new ReturnToSpawn(2),
                    new TimedTransition(5000, "rage1")
                    ),
                new State("rage1",
                    new TossObject("Head of Power Mini", range: 3, angle: 0, coolDown: 99999),
                    new TossObject("Head of Power Mini", range: 3, angle: 90, coolDown: 99999),
                    new TossObject("Head of Power Mini", range: 3, angle: 180, coolDown: 99999),
                    new TossObject("Head of Power Mini", range: 3, angle: 270, coolDown: 99999),
                    new Order(50, "Head Anchor", "red"),
                    new TimedTransition(2000, "rage2")
                    ),
                new State("rage2",
                    new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Follow(0.6, 8, 1),
                    new Wander(0.6),
                    new Shoot(50, count: 8, shootAngle: 13, fixedAngle: 0, projectileIndex: 0, coolDown: 1500)
                    )
                )
            )
           .Init("Head of Knowledge",
            new State(
                new RemoveObjectOnDeath("DW Wall5", 100),
                new State("beginning",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new MoveTo2(X: 0, Y: 5, speed: 1),
                    new Taunt("You imbeciles do not know what you are tampering with!"),
                    new TimedTransition(5000, "prep")
                    ),
                new State("prep",
                    new Flash(0x00c9ff, 0.8, 10),
                    new Orbit(0.8, 5, 10, "Head Anchor"),
                    new TimedTransition(4000, "pre1")
                    ),
                new State("pre1",
                    new Orbit(0.8, 5, 10, "Head Anchor"),
                    new Shoot(50, count: 10, shootAngle: 36, projectileIndex: 0, coolDown: 2000),
                    new EntitiesNotExistsTransition(50, "1", "Head of Wisdom", "Head of Power")
                    ),
                new State("1",
                    new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Orbit(0.8, 5, 10, "Head Anchor"),
                    new Shoot(50, count: 10, shootAngle: 36, projectileIndex: 0, coolDown: 2000),
                    new HpLessTransition(0.4, "rage")
                    ),
                new State("rage",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new Flash(0xff0000, 1, 10),
                    new ChangeSize(rate: 13, target: 150),
                    new ReturnToSpawn(2),
                    new TimedTransition(5000, "rage1")
                    ),
                new State("rage1",
                    new TossObject("Head of Knowledge Mini", range: 3, angle: 0, coolDown: 99999),
                    new TossObject("Head of Knowledge Mini", range: 3, angle: 90, coolDown: 99999),
                    new TossObject("Head of Knowledge Mini", range: 3, angle: 180, coolDown: 99999),
                    new TossObject("Head of Knowledge Mini", range: 3, angle: 270, coolDown: 99999),
                    new Order(50, "Head Anchor", "yellow"),
                    new TimedTransition(2000, "rage2")
                    ),
                new State("rage2",
                    new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Follow(0.6, 8, 1),
                    new Wander(0.6),
                    new Shoot(50, count: 8, shootAngle: 13, fixedAngle: 0, projectileIndex: 0, coolDown: 1500),
                    new HpLessTransition(0.1, "dying")
                    ),
                new State("dying",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt("We have warned you... You do not know what great dangers lie beyond this point..."),
                    new TimedTransition(4000, "boom")
                    ),
                new State("boom",
                    new Suicide()
                    )
                )
            )
           .Init("Head of Wisdom Mini",
            new State(
                new State("1",
                    new Follow(0.6, 8, 1),
                    new Wander(0.6),
                    new Shoot(50, count: 5, shootAngle: 13, projectileIndex: 0, coolDown: 1000)
                    )
                )
            )
           .Init("Head of Power Mini",
            new State(
                new State("1",
                    new Follow(0.6, 8, 1),
                    new Wander(0.6),
                    new Shoot(50, count: 3, shootAngle: 11, projectileIndex: 0, coolDown: 1000)
                    )
                )
            )
           .Init("Head of Knowledge Mini",
            new State(
                new State("1",
                    new Follow(0.6, 8, 1),
                    new Wander(0.6),
                    new Shoot(50, count: 10, shootAngle: 36, projectileIndex: 0, coolDown: 1000)
                    )
                )
            )
        .Init("Head Anchor",
            new State(
                new State("idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("blue",
                    new Shoot(50, count: 15, shootAngle: 24, projectileIndex: 0, fixedAngle: 0, coolDown: 2500),
                    new EntitiesNotExistsTransition(50, "idle", "Head of Wisdom")
                    ),
                new State("red",
                    new Shoot(50, count: 4, shootAngle: 13, projectileIndex: 1, fixedAngle: 225, coolDown: 1500),
                    new Shoot(50, count: 4, shootAngle: 13, projectileIndex: 1, fixedAngle: 315, coolDown: 1500),
                    new Shoot(50, count: 4, shootAngle: 13, projectileIndex: 1, fixedAngle: 45, coolDown: 1500),
                    new Shoot(50, count: 4, shootAngle: 13, projectileIndex: 1, fixedAngle: 135, coolDown: 1500),
                    new EntitiesNotExistsTransition(50, "idle", "Head of Power")
                    ),
                new State("yellow",
                    new Shoot(50, count: 5, shootAngle: 13, projectileIndex: 2, fixedAngle: 225, coolDown: 1500),
                    new EntitiesNotExistsTransition(50, "idle", "Head of Knowledge")
                    )
                )
            )
           .Init("Red Wisp",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new Wander(0.6),
                    new StayCloseToSpawn(speed: 0.8, range: 3)
                    )
                )
            )
           .Init("Blue Wisp",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new Wander(0.6),
                    new StayCloseToSpawn(speed: 0.8, range: 3)
                    )
                )
            )
           .Init("Yellow Wisp",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new Wander(0.6),
                    new StayCloseToSpawn(speed: 0.8, range: 3)
                    )
                )                      
        #endregion
                    );
    }

}