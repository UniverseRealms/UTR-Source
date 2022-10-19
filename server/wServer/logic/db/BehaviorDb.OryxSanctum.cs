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
        private _ OryxSanctum = () => Behav()

#region akeldama
        .Init("Akeldama, the Disciple of Oryx",
            new State(
                new ScaleHP(0.3),
                new ConditionalEffect(ConditionEffectIndex.StunImmune, true),
                new DropPortalOnDeath("Wine Cellar Portal"),
                new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new PlayerWithinTransition(10, "1")
                    ),
                new State("1",
                    new Taunt("You've made a grave mistake charging into this sanctum heroes."),
                    new TimedTransition(2000, "2")
                    ),
                new State("2",
                    new Taunt("You think you've defeated Oryx, yet you stand no chance against his true form..."),
                    new TimedTransition(2000, "3")
                    ),
                new State("3",
                    new Taunt("Oryx rewards his followers with powers unfathomable."),
                    new TimedTransition(2000, "4")
                    ),
                new State("4",         
                    new Taunt("By my hand you shall fall."),
                    new Flash(0xbf0000, 4, 1),
                    new TimedTransition(3000, "5")
                    ),
                new State("5",
                    new Shoot(25, 45, fixedAngle: 0, projectileIndex: 3, coolDown: 4000),
                    new TimedTransition(3000, "6")
                    ),
                new State("6",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 0),
                    new Order(60, "OS Ground Changer2", "boom"),
                    new Shoot(15, projectileIndex: 0, count: 15, shootAngle: 24),
                    new HpLessTransition(0.8, "7")
                    ),
                new State("7",
                    new Taunt("Impressive, but you will not make it out of here alive!"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true, duration: 6000),
                    new TimedTransition(6000, "8")
                    ),
                new State("8",
                    new ChangeGround(new[] { "osground5" }, new[] { "oslava" }, 100),
                    new Shoot(10, projectileIndex: 2, count: 5, shootAngle: 10, coolDown: 750),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 45, coolDown: 3800, coolDownOffset: 1000),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 225, coolDown: 3800, coolDownOffset: 1000),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 95, coolDown: 3800, coolDownOffset: 1200),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 275, coolDown: 3800, coolDownOffset: 1200),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 22.5, coolDown: 3800, coolDownOffset: 1400),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 200.5, coolDown: 3800, coolDownOffset: 1400),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 135, coolDown: 3800, coolDownOffset: 1600),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 315, coolDown: 3800, coolDownOffset: 1600),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 90, coolDown: 3800, coolDownOffset: 1800),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 270, coolDown: 3800, coolDownOffset: 1800),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 157.2, coolDown: 3800, coolDownOffset: 2000),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 357.2, coolDown: 3800, coolDownOffset: 2000),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 90, coolDown: 3800, coolDownOffset: 2200),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 270, coolDown: 3800, coolDownOffset: 2200),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 30, coolDown: 3800, coolDownOffset: 2400),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 210, coolDown: 3800, coolDownOffset: 2400),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 150, coolDown: 3800, coolDownOffset: 2600),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 350, coolDown: 3800, coolDownOffset: 2600),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 22.5, coolDown: 3800, coolDownOffset: 2800),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 220.5, coolDown: 3800, coolDownOffset: 2800),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 90, coolDown: 3800, coolDownOffset: 3000),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 270, coolDown: 3800, coolDownOffset: 3000),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 135, coolDown: 3800, coolDownOffset: 3200),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 315, coolDown: 3800, coolDownOffset: 3200),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 180, coolDown: 3800, coolDownOffset: 3400),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 0, coolDown: 3800, coolDownOffset: 3400),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 90, coolDown: 3800, coolDownOffset: 3600),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 270, coolDown: 3800, coolDownOffset: 3600),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 0, coolDown: 3800, coolDownOffset: 3800),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 180, coolDown: 3800, coolDownOffset: 3800),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 30, coolDown: 3800, coolDownOffset: 4000),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 210, coolDown: 3800, coolDownOffset: 4000),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 135, coolDown: 3800, coolDownOffset: 4200),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 315, coolDown: 3800, coolDownOffset: 4200),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 22.5, coolDown: 3800, coolDownOffset: 4400),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 200.5, coolDown: 3800, coolDownOffset: 4400),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 150, coolDown: 3800, coolDownOffset: 4600),
                    new Shoot(10, count: 3, shootAngle: 45, projectileIndex: 12, fixedAngle: 330, coolDown: 3800, coolDownOffset: 4600),
                    new TimedTransition(10000, "9")
                    ),
                new State("9",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new Taunt("Fear my power!"),
                    new Flash(0xFF0000, 1, 1),
                    new TimedTransition(5000, "10")
                    ),
                new State("10",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 0),
                    new Shoot(15, projectileIndex: 2, count: 3, shootAngle: 15, coolDown: 500),
                    new Shoot(15, projectileIndex: 4, count: 4, shootAngle: 15, fixedAngle: 0, coolDown: 1600, coolDownOffset: 800),
                    new Shoot(15, projectileIndex: 4, count: 4, shootAngle: 15, fixedAngle: 90, coolDown: 1600, coolDownOffset: 800),
                    new Shoot(15, projectileIndex: 4, count: 4, shootAngle: 15, fixedAngle: 180, coolDown: 1600, coolDownOffset: 800),
                    new Shoot(15, projectileIndex: 4, count: 4, shootAngle: 15, fixedAngle: 270, coolDown: 1600, coolDownOffset: 800),
                    new Shoot(15, projectileIndex: 4, count: 4, shootAngle: 15, fixedAngle: 45, coolDown: 1600, coolDownOffset: 1600),
                    new Shoot(15, projectileIndex: 4, count: 4, shootAngle: 15, fixedAngle: 135, coolDown: 1600, coolDownOffset: 1600),
                    new Shoot(15, projectileIndex: 4, count: 4, shootAngle: 15, fixedAngle: 225, coolDown: 1600, coolDownOffset: 1600),
                    new Shoot(15, projectileIndex: 4, count: 4, shootAngle: 15, fixedAngle: 315, coolDown: 1600, coolDownOffset: 1600),
                    new HpLessTransition(0.5, "11")
                    ),
                new State("11",
                    new Taunt("I will not fall!!!"),
                    new Shoot(40, count: 10, shootAngle: 15, fixedAngle: 45, projectileIndex: 5, coolDown: 1500, coolDownOffset: 1000),
                    new Shoot(40, count: 5, shootAngle: 10, fixedAngle: 135, projectileIndex: 6, coolDown: 1500, coolDownOffset: 1500),
                    new Shoot(40, count: 10, shootAngle: 15, fixedAngle: 225, projectileIndex: 7, coolDown: 1500, coolDownOffset: 2000),
                    new Shoot(40, count: 5, shootAngle: 10, fixedAngle: 315, projectileIndex: 8, coolDown: 1500, coolDownOffset: 2500),
                    new HpLessTransition(0.4, "12")
                    ),
                new State("12",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new Flash(0x000000, 5, 1),
                    new Taunt("Argh, you heroes are much tougher than I thought..."),
                    new TimedTransition(2000, "13")
                    ),
                new State("13",
                    new Flash(0xff0000, 5, 1),
                    new Taunt("However, it's not over yet!"),
                    new TimedTransition(2000, "14")
                    ),
                new State("14",
                    new Flash(0xff0000, 4, 1),
                    new ChangeSize(13, 200),
                    new TimedTransition(5000, "15")
                    ),
                new State("15",
                    new Taunt("Minions, get rid of them!"),
                    new TossObject("Minion of Akeldama", 6, 90, count: 2, angleOffset: 180, coolDown: 100000),
                    new TossObject("Minion of Akeldama2", 6, 0, count: 2, angleOffset: 180, coolDown: 100000),
                    new TimedTransition(2100, "checkforminions")
                    ),
                new State("checkforminions",
                    new EntitiesNotExistsTransition(99, "16", "Minion of Akeldama", "Minion of Akeldama2")
                    ),
                new State("16",
                    new Taunt("I will take care of you personally!"),
                    new TimedTransition(5000, "17")
                    ),
                new State("17",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 0),
                    new Wander(0.5),
                    new Shoot(15, projectileIndex: 9, count: 8, shootAngle: 48.5, coolDown: 1600),
                    new Shoot(10, projectileIndex: 10, count: 4, shootAngle: 5, coolDown: 500),
                    new HpLessTransition(0.2, "18")
                    ),
                new State("18",
                    new Taunt("You... YOU WILL NOT GET PAST ME!!"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new ReturnToSpawn(0.5),
                    new TimedTransition(4000, "19")
                    ),
                new State("19",
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Shoot(40, count: 10, shootAngle: 15, fixedAngle: 45, projectileIndex: 5, coolDown: 1500, coolDownOffset: 1000),
                    new Shoot(40, count: 5, shootAngle: 10, fixedAngle: 135, projectileIndex: 6, coolDown: 1500, coolDownOffset: 1500),
                    new Shoot(40, count: 10, shootAngle: 15, fixedAngle: 225, projectileIndex: 7, coolDown: 1500, coolDownOffset: 2000),
                    new Shoot(40, count: 5, shootAngle: 10, fixedAngle: 315, projectileIndex: 8, coolDown: 1500, coolDownOffset: 2500),
                    new HpLessTransition(0.05, "20")
                    ),
                new State("20",
                    new ChangeGround(new[] { "oslava" }, new[] { "osground2" }, 3),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new Taunt("No... NO, NO!!! YOU! I WILL DRAG YOU DOWN WITH ME!"),
                    new TossObject("Sentry1", 14, 225, 100000),
                    new TossObject("Sentry2", 14, 135, 100000),
                    new TossObject("Sentry3", 14, 45, 100000),
                    new TossObject("Sentry4", 14, 315, 100000),
                    new Order(60, "Sentry5", "idle"),
                    new TimedTransition(8000, "21")
                    ),
                new State("21",
                    new Taunt("PERISH!!!"),
                    new Order(60, "Sentry1", "2"),
                    new Order(60, "Sentry2", "2"),
                    new Order(60, "Sentry3", "2"),
                    new Order(60, "Sentry4", "2"),            
                    new Order(60, "OS Ground Changer4", "boom"),
                    new TossObject("Akel Bomb", range: 2, angle: 45, count: 4, angleOffset: 90, coolDown: 4000, coolDownOffset: 0),
                    new TossObject("Akel Bomb", range: 2, angle: 0, count: 4, angleOffset: 90, coolDown: 4000, coolDownOffset: 2000),
                    new Shoot(25, 60, fixedAngle: 0, projectileIndex: 13, coolDown: 4000),
                    new SpiralShoot(3, 20, 6, projectileIndex: 11, coolDown: 300),
                    new SpiralShoot(3, 20, 6, projectileIndex: 11, fixedAngle: 1, coolDown: 300),
                    new TimedTransition(20000, "22")
                    ),
                new State("22",
                    new Flash(0x9400D3, 10, 1),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Order(60, "Sentry1", "boom"),
                    new Order(60, "Sentry2", "boom"),
                    new Order(60, "Sentry3", "boom"),
                    new Order(60, "Sentry4", "boom"),
                    new Order(60, "Sentry5", "boom"),
                    new Taunt("You... You ruined everything. Oryx will not stand for this... Your acts will have released the wrath upon the realms..."),
                    new TimedTransition(8000, "dead")
                    ),
                new State("dead",
                    new Suicide()
                    )
                ),
                new Threshold(0.01,
                    new ItemLoot("Broadsword of Bloodshed", 0.01),
                    new ItemLoot("Helmet of Madness", 0.005),
                    new ItemLoot("Belt of Insanity", 0.002),
                    new ItemLoot("Potion of Life", 1),
                    new ItemLoot("Potion of Restoration", 0.06),
                    new ItemLoot("Potion of Luck", 0.2),
                    new TierLoot(3, ItemType.Weapon, 0.1),
                    new TierLoot(3, ItemType.Ability, 0.1),
                    new TierLoot(3, ItemType.Armor, 0.1),
                    new TierLoot(3, ItemType.Ring, 0.1),
                    new TierLoot(4, ItemType.Weapon, 0.05),
                    new TierLoot(4, ItemType.Ability, 0.05),
                    new TierLoot(4, ItemType.Armor, 0.05),
                    new TierLoot(4, ItemType.Ring, 0.05),
                    new ItemLoot("Sanctum Key", 0.005),
                    new ItemLoot("Rusted Sword", 0.00143),
                    new ItemLoot("Eternal Essence", 0.001),
                    new ItemLoot("Demonic Pet Stone", 0.001)
                )
            )
#endregion akeldama

#region oryx defense system
        .Init("Oryx Defense System",
            new State(
                new ScaleHP(0.3),
                new RemoveObjectOnDeath("ALG Wall", 50),
                new State("Initialize",
                    new PlayerWithinTransition(10, "1")
                    ),
                new State("1",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true, duration: 14000),
                    new Taunt("Uninvited entities detected."),
                    new TimedTransition(2000, "2")
                    ),
                new State("2",
                    new Taunt("Activating defense system."),
                    new TimedTransition(2000, "3")
                    ),
                new State("3",
                    new Taunt("Initializing."),
                    new TimedTransition(2000, "4")
                    ),
                new State("4",
                    new Taunt("Leave, or be exterminated."),
                    new Flash(0x00FF00, 4, 1),
                    new TimedTransition(3000, "5")
                    ),
                new State("5",
                    new Shoot(25, 36, fixedAngle: 0, projectileIndex: 1, coolDown: 2000),
                    new Shoot(25, 36, fixedAngle: 5, projectileIndex: 1, coolDown: 2000, coolDownOffset: 1000),
                    new TimedTransition(5000, "6")
                    ),
                new State("6",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 4000),
                    new ConditionalEffect(ConditionEffectIndex.Armored, true),
                    new TossObject("Defense Bomb", range: 8, angle: 135, count: 1, coolDown: 8000),
                    new TossObject("Defense Bomb", range: 2, angle: 315, count: 1, coolDown: 8000, coolDownOffset: 500),
                    new TossObject("Defense Bomb", range: 8, angle: 315, count: 1, coolDown: 8000, coolDownOffset: 1000),
                    new TossObject("Defense Bomb", range: 2, angle: 115, count: 1, coolDown: 8000, coolDownOffset: 1500),
                    new TossObject("Defense Bomb", range: 8, angle: 45, count: 1, coolDown: 8000, coolDownOffset: 2000),
                    new TossObject("Defense Bomb", range: 2, angle: 225, count: 1, coolDown: 8000, coolDownOffset: 2500),
                    new TossObject("Defense Bomb", range: 8, angle: 225, count: 1, coolDown: 8000, coolDownOffset: 3000),
                    new TossObject("Defense Bomb", range: 2, angle: 45, count: 1, coolDown: 8000, coolDownOffset: 3500),
                    new TossObject("Defense Bomb", range: 8, angle: 0, count: 1, coolDown: 8000, coolDownOffset: 4000),
                    new TossObject("Defense Bomb", range: 2, angle: 180, count: 1, coolDown: 8000, coolDownOffset: 4500),
                    new TossObject("Defense Bomb", range: 8, angle: 180, count: 1, coolDown: 8000, coolDownOffset: 5000),
                    new TossObject("Defense Bomb", range: 2, angle: 0, count: 1, coolDown: 8000, coolDownOffset: 5500),
                    new TossObject("Defense Bomb", range: 8, angle: 90, count: 1, coolDown: 8000, coolDownOffset: 6000),
                    new TossObject("Defense Bomb", range: 2, angle: 270, count: 1, coolDown: 8000, coolDownOffset: 6500),
                    new TossObject("Defense Bomb", range: 8, angle: 270, count: 1, coolDown: 8000, coolDownOffset: 7000),
                    new TossObject("Defense Bomb", range: 2, angle: 90, count: 1, coolDown: 8000, coolDownOffset: 7500),
                    new Sequence(
                        new SpiralShoot(5, 18, 6, projectileIndex: 8, coolDown: 200, delayAfterComplete: 500),
                        new SpiralShoot(-5, 18, 6, fixedAngle: 90, projectileIndex: 8, coolDown: 200, delayAfterComplete: 500)
                        ),
                    new HpLessTransition(0.75, "7")
                    ),
                new State("7",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true, duration: 9000),
                    new ReturnToSpawn(1),
                    new Taunt("Significant damage taken...refactoring extermination pattern."),
                    new TimedTransition(5000, "8")
                    ),
                new State("8",
                    new TossObject("Defense Bomb", range: 8, angle: 135, count: 1, coolDown: 8000),
                    new TossObject("Defense Bomb", range: 2, angle: 315, count: 1, coolDown: 8000, coolDownOffset: 500),
                    new TossObject("Defense Bomb", range: 8, angle: 315, count: 1, coolDown: 8000, coolDownOffset: 1000),
                    new TossObject("Defense Bomb", range: 2, angle: 115, count: 1, coolDown: 8000, coolDownOffset: 1500),
                    new TossObject("Defense Bomb", range: 8, angle: 45, count: 1, coolDown: 8000, coolDownOffset: 2000),
                    new TossObject("Defense Bomb", range: 2, angle: 225, count: 1, coolDown: 8000, coolDownOffset: 2500),
                    new TossObject("Defense Bomb", range: 8, angle: 225, count: 1, coolDown: 8000, coolDownOffset: 3000),
                    new TossObject("Defense Bomb", range: 2, angle: 45, count: 1, coolDown: 8000, coolDownOffset: 3500),
                    new TossObject("Defense Bomb", range: 8, angle: 0, count: 1, coolDown: 8000, coolDownOffset: 4000),
                    new TossObject("Defense Bomb", range: 2, angle: 180, count: 1, coolDown: 8000, coolDownOffset: 4500),
                    new TossObject("Defense Bomb", range: 8, angle: 180, count: 1, coolDown: 8000, coolDownOffset: 5000),
                    new TossObject("Defense Bomb", range: 2, angle: 0, count: 1, coolDown: 8000, coolDownOffset: 5500),
                    new TossObject("Defense Bomb", range: 8, angle: 90, count: 1, coolDown: 8000, coolDownOffset: 6000),
                    new TossObject("Defense Bomb", range: 2, angle: 270, count: 1, coolDown: 8000, coolDownOffset: 6500),
                    new TossObject("Defense Bomb", range: 8, angle: 270, count: 1, coolDown: 8000, coolDownOffset: 7000),
                    new TossObject("Defense Bomb", range: 2, angle: 90, count: 1, coolDown: 8000, coolDownOffset: 7500),
                    new Shoot(25, 24, fixedAngle: 0, projectileIndex: 8, coolDown: 2000),
                    new Shoot(25, 24, fixedAngle: 7.5, projectileIndex: 8, coolDown: 2000, coolDownOffset: 1000),
                    new HpLessTransition(0.5, "9")
                    ),
                new State("9",
                    new ChangeGround(new[] { "SB Nexus Tile7" }, new[] { "oslava" }, 100),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new ReturnToSpawn(1),
                    new Flash(0xFF0000, 4, 1),
                    new Taunt("Last stand activated."),
                    new TimedTransition(5000, "10")
                    ),
                new State("10",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new TossObject("Defense System Minion2", 4, 60, 100000),
                    new TossObject("Defense System Minion2", 4, 180, 100000),
                    new TossObject("Defense System Minion2", 4, 300, 100000),
                    new TimedTransition(3000, "11")
                    ),
                new State("11",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new TossObject("Defense System Minion2", 4, 0, 100000, coolDownOffset: 2000),
                    new TossObject("Defense System Minion2", 4, 120, 100000, coolDownOffset: 2000),
                    new TossObject("Defense System Minion2", 4, 240, 100000, coolDownOffset: 2000),
                    new Shoot(10, count: 4, shootAngle: 2, fixedAngle: 90, projectileIndex: 3, coolDown: 300, coolDownOffset: 1200),
                    new Shoot(10, count: 4, shootAngle: 2, fixedAngle: 180, projectileIndex: 3, coolDown: 300, coolDownOffset: 1200),
                    new Shoot(10, count: 4, shootAngle: 2, fixedAngle: 270, projectileIndex: 3, coolDown: 300, coolDownOffset: 1200),
                    new Shoot(10, count: 4, shootAngle: 2, fixedAngle: 0, projectileIndex: 3, coolDown: 300, coolDownOffset: 1200),
                    new Shoot(10, count: 4, shootAngle: 2, fixedAngle: 45, projectileIndex: 3, coolDown: 300, coolDownOffset: 1500),
                    new Shoot(10, count: 4, shootAngle: 2, fixedAngle: 225, projectileIndex: 3, coolDown: 300, coolDownOffset: 1500),
                    new Shoot(10, count: 4, shootAngle: 2, fixedAngle: 315, projectileIndex: 3, coolDown: 300, coolDownOffset: 1500),
                    new Shoot(10, count: 4, shootAngle: 2, fixedAngle: 135, projectileIndex: 3, coolDown: 300, coolDownOffset: 1500),
                    new TimedTransition(5000, "11.5")
                    ),
                new State("11.5",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Shoot(10, count: 4, shootAngle: 2, fixedAngle: 90, projectileIndex: 3, coolDown: 300, coolDownOffset: 1200),
                    new Shoot(10, count: 4, shootAngle: 2, fixedAngle: 180, projectileIndex: 3, coolDown: 300, coolDownOffset: 1200),
                    new Shoot(10, count: 4, shootAngle: 2, fixedAngle: 270, projectileIndex: 3, coolDown: 300, coolDownOffset: 1200),
                    new Shoot(10, count: 4, shootAngle: 2, fixedAngle: 0, projectileIndex: 3, coolDown: 300, coolDownOffset: 1200),
                    new Shoot(10, count: 4, shootAngle: 2, fixedAngle: 45, projectileIndex: 3, coolDown: 300, coolDownOffset: 1500),
                    new Shoot(10, count: 4, shootAngle: 2, fixedAngle: 225, projectileIndex: 3, coolDown: 300, coolDownOffset: 1500),
                    new Shoot(10, count: 4, shootAngle: 2, fixedAngle: 315, projectileIndex: 3, coolDown: 300, coolDownOffset: 1500),
                    new Shoot(10, count: 4, shootAngle: 2, fixedAngle: 135, projectileIndex: 3, coolDown: 300, coolDownOffset: 1500),
                    new EntitiesNotExistsTransition(99, "Finalphase", "Defense System Minion1", "Defense System Minion2")
                    ),
                new State("Finalphase",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Flash(0xFF0000, 8, 1),
                    new ChangeSize(13, 200),
                    new TimedTransition(5000, "12")
                    ),
                new State("12",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 0),
                    new Wander(0.5),
                    new Shoot(15, count: 5, shootAngle: 6, projectileIndex: 4, coolDown: 1500),
                    new Sequence(
                        new SpiralShoot(10, 15, 4, projectileIndex: 5, coolDown: 200),
                        new SpiralShoot(-10, 15, 4, fixedAngle: 140, projectileIndex: 5, coolDown: 200),
                        new SpiralShoot(10, 15, 4, projectileIndex: 5, coolDown: 200),
                        new SpiralShoot(10, 15, 4, projectileIndex: 5, coolDown: 200),
                        new SpiralShoot(-10, 15, 4, fixedAngle: 140, projectileIndex: 5, coolDown: 200)
                        ),
                    new TimedTransition(5000, "13"),
                    new HpLessTransition(0.05, "14")
                    ),
                new State("13",
                    new Shoot(15, count: 20, shootAngle: 20, projectileIndex: 6, coolDown: 500),
                    new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 7, predictive: 0.2, coolDown: 300),
                    new Follow(0.7, range: 4, coolDown: 0),
                    new TimedTransition(5000, "12"),
                    new HpLessTransition(0.05, "14")
                    ),
                new State("14",
                    new ReturnToSpawn(1),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new Taunt("Defense systems failed, unable to maintain protection barrier. Shutting down."),
                    new TimedTransition(8000, "boom")
                    ),
                new State("boom",
                    new Shoot(25, 45, fixedAngle: 0, projectileIndex: 1, coolDown: 4000),
                    new Suicide()
                    )
                ),
                new Threshold(0.01,
                    new ItemLoot("Sanctioned Gem", 0.01),
                    new ItemLoot("Blood-Monk's Apparel", 0.005),
                    new ItemLoot("Wand of Command", 0.002),
                    new ItemLoot("Potion of Life", 1),
                    new ItemLoot("Potion of Restoration", 0.06),
                    new ItemLoot("Potion of Luck", 0.2),
                    new TierLoot(3, ItemType.Weapon, 0.1),
                    new TierLoot(3, ItemType.Ability, 0.1),
                    new TierLoot(3, ItemType.Armor, 0.1),
                    new TierLoot(3, ItemType.Ring, 0.1),
                    new TierLoot(4, ItemType.Weapon, 0.05),
                    new TierLoot(4, ItemType.Ability, 0.05),
                    new TierLoot(4, ItemType.Armor, 0.05),
                    new TierLoot(4, ItemType.Ring, 0.05),
                    new ItemLoot("Rusted Platemail", 0.00143),
                    new ItemLoot("Eternal Essence", 0.001),
                    new ItemLoot("Skull Pet Stone", 0.001)
                )
            )
#endregion oryx defense system

#region minions/sentries/ground
        .Init("Minion of Akeldama",
            new State(
                new State("1",
                    new ScaleHP(0.1),
                    new Wander(0.6),
                    new Shoot(10, projectileIndex: 0, count: 8, shootAngle: 10, coolDown: 2500),
                    new HpLessTransition(0.5, "2")
                    ),
                new State("2",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Flash(0xFF0000, 1, 1),
                    new TimedTransition(3000, "3")
                    ),
                new State("3",
                    new Wander(0.6),
                    new Shoot(10, projectileIndex: 1, count: 3, shootAngle: 5, coolDown: 500)
                    )
                )
            )

        .Init("Minion of Akeldama2",
            new State(
                new State("1",
                    new ScaleHP(0.1),
                    new Wander(1),
                    new Shoot(10, projectileIndex: 0, count: 3, shootAngle: 5, coolDown: 1000, coolDownOffset: 1200),
                    new Shoot(10, projectileIndex: 1, count: 6, shootAngle: 10, coolDown: 1000, coolDownOffset: 2000)
                    )
                )
            )

        .Init("Defense System Minion1",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new Follow(0.5),
                    new Shoot(10, projectileIndex: 0, count: 3, shootAngle: 15, coolDown: 400),
                    new Shoot(10, projectileIndex: 1, count: 6, shootAngle: 60, coolDown: 2000)
                    )
                )
            )

        .Init("Defense System Minion2",
            new State(
                new ScaleHP(0.1),
                new State("1",
                    new Follow(0.5),
                    new Shoot(10, projectileIndex: 0, count: 2, shootAngle: 20, coolDown: 1000),
                    new Shoot(10, projectileIndex: 1, count: 1, coolDown: 2500)
                    )
                )
            )

        .Init("OS Ground Changer",
            new State(
                new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("boom",
                    new ChangeGroundOnDeath(new[] { "osground1", "osground2", "osground4", "osground5" }, new[] { "oslava" }, 1),
                    new Suicide()
                    )
                )
            )

        .Init("OS Ground Changer2",
            new State(
                new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("boom",
                    new ChangeGroundOnDeath(new[] { "osground1", "osground2", "osground4", "osground5", "osground3" }, new[] { "osunwalk" }, 1),
                    new Suicide()
                    )
                )
            )

        .Init("OS Ground Changer3",
            new State(
                new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("boom",
                    new ChangeGroundOnDeath(new[] { "osground1", "osground2", "osground4", "osground5", "osground3" }, new[] { "oslava" }, 1),
                    new Suicide()
                    )
                )
            )

        .Init("OS Ground Changer4",
            new State(
                new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("boom",
                    new ChangeGroundOnDeath(new[] { "osground1", "osground2", "osground4", "osground5", "osground3" }, new[] { "oslava" }, 1),
                    new Suicide()
                    )
                )
            )

        .Init("Sentry1",
            new State(
                new State("1",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true)
                    ),
                new State("2",
                    new Shoot(10, count: 4, shootAngle: 20, fixedAngle: 315, coolDown: 1000),
                    new Shoot(10, count: 4, shootAngle: 20, fixedAngle: 135, coolDown: 1000)
                    ),
                new State("boom",
                    new Suicide()
                    )
                )
            )

        .Init("Sentry2",
            new State(
                new State("1",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true)
                    ),
                new State("2",
                    new Shoot(10, count: 4, shootAngle: 20, fixedAngle: 45, coolDown: 1000),
                    new Shoot(10, count: 4, shootAngle: 20, fixedAngle: 225, coolDown: 1000)
                    ),
                new State("boom",
                    new Suicide()
                    )
                )
            )

        .Init("Sentry3",
            new State(
                new State("1",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true)
                    ),
                new State("2",
                    new Shoot(10, count: 4, shootAngle: 20, fixedAngle: 135, coolDown: 1000),
                    new Shoot(10, count: 4, shootAngle: 20, fixedAngle: 315, coolDown: 1000)
                    ),
                new State("boom",
                    new Suicide()
                    )
                )
            )

        .Init("Sentry4",
            new State(
                new State("1",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true)
                    ),
                new State("2",
                    new Shoot(10, count: 4, shootAngle: 20, fixedAngle: 225, coolDown: 1000),
                    new Shoot(10, count: 4, shootAngle: 20, fixedAngle: 45, coolDown: 1000)
                    ),
                new State("boom",
                    new Suicide()
                    )
                )
            )

        .Init("Akel Bomb",
            new State(
                new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new Flash(0xff0000, 2, 1),
                    new ChangeSize(100, 80),
                    new TimedTransition(800, "Kaboom2")
                    ),
                new State("Kaboom2",
                    new Shoot(30, projectileIndex: 0, count: 8, fixedAngle: 0, shootAngle: 45),
                    new Decay(0)
                    )
                )
            )

        .Init("Defense Bomb",
            new State(
                new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new Flash(0xff0000, 2, 1),
                    new ChangeSize(100, 80),
                    new TimedTransition(800, "Kaboom2")
                    ),
                new State("Kaboom2",
                    new Shoot(30, projectileIndex: 0, count: 8, fixedAngle: 0, shootAngle: 45),
                    new Decay(0)
                    )
                )      
            )

        .Init("Sentry5",
            new State(
                new State("1",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("Main",
                    new State("idle",
                        new PlayerWithinTransition(6, "charging")
                        ),
                    new State("charging",
                        new ChangeSize(60, 150),
                        new TimedTransition(2000, "shoot")
                        ),
                    new State("shoot",
                        new Shoot(10, count: 6, shootAngle: 20, projectileIndex: 0, coolDown: 900)
                        ),
                    new State("boom",
                        new Suicide()
                        )
                    )
                )
            );
#endregion minions/sentries/ground

    }
}
