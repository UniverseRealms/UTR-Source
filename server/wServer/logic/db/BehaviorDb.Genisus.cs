using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Genisus = () => Behav()
            .Init("Codename G-24, Nitrostorm",
                new State(
                    new ScaleHP(0.3),
                    new RealmPortalDrop(),
                    new State(
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("default",
                        new ConditionalEffect(ConditionEffectIndex.Paused),
                        new EntityNotExistsTransition("Galleom of Time", 999, "wait")
                        ),
                   new State("wait",
                        new Taunt(true, "Activated..ready to deploy."),
                        new PlayerWithinTransition(8, "taunt")
                        ),
                   new State("taunt",
                        new Taunt("Let us begin routine..shall we?"),
                        new TimedTransition(8000, "begin")
                        )
                    ),
                    new State(
                        new HpLessTransition(0.2, "rage"),
                    new State("begin",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TossObject("Genisus Core", 3, 0, count: 4, angleOffset: 90, coolDown: 9999999),
                        new TimedTransition(3000, "check1")
                        ),
                    new State("check1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Grenade(2, 60, range: 12, coolDown: 2000, effect: ConditionEffectIndex.Quiet, effectDuration: 2000),
                        new EntityNotExistsTransition("Genisus Core", 999, "fight1")
                        ),
                    new State("fight1",
                        new Grenade(4, 80, range: 12, coolDown: 2000, effect: ConditionEffectIndex.Blind, effectDuration: 2000, color: 0x00FF00),
                        new Shoot(10, 12, projectileIndex: 2, coolDown: 2000),
                        new Sequence(
                            new SpiralShoot(15, 6, 3, projectileIndex: 1, coolDown: 400),
                            new SpiralShoot(-15, 6, 3, projectileIndex: 1, fixedAngle: 90, coolDown: 400)
                            ),
                        new TimedTransition(8000, "charging")
                    ),
                   new State("charging",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0x0000FF, 0.25, 4),
                        new TimedTransition(2000, "fight2")
                        ),
                    new State("fight2",
                        new TossObject("G-10 Testing", 4, 0, coolDown: 9999999),
                        new TossObject("G-10 Testing", 4, 180, coolDown: 9999999),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Shoot(10, 3, shootAngle: 8, projectileIndex: 1, predictive: 0.5, coolDown: 800),
                        new Shoot(10, 4, projectileIndex: 3, predictive: 1, coolDown: 2000),
                        new Shoot(10, 8, projectileIndex: 2, coolDown: 2000),
                        new TimedTransition(6000, "spawnguardians")
                        ),
                    new State("spawnguardians",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TossObject("Guardian of the Genisus", 4, 0, count: 4, angleOffset: 90, coolDown: 9999999),
                        new TimedTransition(3000, "check2")
                        ),
                    new State("check2",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Grenade(2, 60, range: 12, coolDown: 2000, effect: ConditionEffectIndex.Quiet, effectDuration: 2000),
                        new EntityNotExistsTransition("Guardian of the Genisus", 10, "force1")
                        ),
                    new State(
                        new Prioritize(
                            new Follow(0.75, 8, 1),
                            new Wander(0.25)
                        ),
                        new Shoot(10, 4, projectileIndex: 3, predictive: 1, coolDown: 2000),
                        new Shoot(10, 3, shootAngle: 12, projectileIndex: 2, coolDown: 800),
                        new TossObject("Time Energy Void", 4, coolDown: 2000),
                        new TimedTransition(8000, "return"),
                        new State("force1",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new TimedTransition(600, "force2")
                            ),
                        new State("force2",
                            new TimedTransition(600, "force1")
                            )
                        ),
                    new State("return",
                          new ConditionalEffect(ConditionEffectIndex.Invincible),
                          new ReturnToSpawn(3),
                          new TimedTransition(7000, "begin")
                        )
                     ),
                  new State("rage",
                        new Taunt("Ready to..."),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ReturnToSpawn(2),
                        new Flash(0x0000FF, 0.25, 4),
                        new TimedTransition(4000, "toon")
                        ),
                     new State(
                         new Flash(0xFF0000, 0.25, 4),
                        new Taunt("ELIMINATE!"),
                        new Prioritize(
                            new Charge(1, range: 4, coolDown: 1000),
                            new Follow(1, 8, 1),
                            new Wander(0.25)
                        ),
                        new Shoot(10, 4, projectileIndex: 3, predictive: 1, coolDown: 1000),
                        new Shoot(10, 6, shootAngle: 12, projectileIndex: 2, coolDown: 800),
                        new Shoot(10, 1, predictive: 0.5, projectileIndex: 1, coolDown: 400),
                        new TossObject("Genisus Lifeform", 4, 0, count: 4, angleOffset: 90, coolDown: 9999999),
                    new State("toon",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new TimedTransition(600, "rageb")
                            ),
                    new State("rageb",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                            new Shoot(10, 8, projectileIndex: 0, coolDown: 600),
                            new TimedTransition(600, "toon")
                            )
                        )
                    ),
                new Threshold(0.01,
                    new ItemLoot("True Guidance", 0.02),
                    new ItemLoot("Otherworldly Scripture", 0.0067),
                    new ItemLoot("Mechanical Coat", 0.002),
                    new ItemLoot("Greater Potion of Defense", 1),
                    new TierLoot(3, ItemType.Weapon, 0.05),
                    new TierLoot(3, ItemType.Ability, 0.05),
                    new TierLoot(3, ItemType.Armor, 0.05),
                    new TierLoot(3, ItemType.Ring, 0.05),
                    new TierLoot(4, ItemType.Weapon, 0.025),
                    new TierLoot(4, ItemType.Ability, 0.025),
                    new TierLoot(4, ItemType.Armor, 0.025),
                    new TierLoot(4, ItemType.Ring, 0.025),
                    new ItemLoot("Rusted Robe", 0.00112),
                    new ItemLoot("Eternal Essence", 0.0005),
                    new ItemLoot("Mechanical Pet Stone", 0.001)
                )
            )
            .Init("G-10 Testing",
            new State(
                new State("Main",
                    new Shoot(10, count: 6, projectileIndex: 0, coolDown: 1000, coolDownOffset: 2400),
                    new HpLessTransition(0.15, "heal"),
                    new State("fight2",
                    new Prioritize(
                        new Follow(1, 8, 1),
                        new Wander(0.25)
                        ),
                        new Shoot(10, count: 3, shootAngle: 8, projectileIndex: 0, coolDown: 1000)
                        )
                    ),
                new State(
                        new Taunt(0.25, "Sentry mode activated.."),
                    new State("heal",
                        new Shoot(10, count: 4, shootAngle: 90, projectileIndex: 1, fixedAngle: 0, coolDown: 600),
                        new TimedTransition(2000, "heal1")
                        ),
                   new State("heal1",
                       new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(10, count: 4, shootAngle: 90, projectileIndex: 1, fixedAngle: 0, coolDown: 600),
                        new TimedTransition(2000, "heal")
                        )
                      )
                    )
                  )
            .Init("Genisus Lifeform",
            new State(
                new State("Main",
                    new Shoot(10, count: 2, shootAngle: 10, projectileIndex: 0, coolDown: 1000, predictive: 0.5, coolDownOffset: 1400),
                    new State("fight1",
                        new Prioritize(
                            new Follow(0.5, 8, 1),
                            new Wander(0.25)
                            ),
                        new Shoot(10, count: 5, projectileIndex: 1, coolDown: 1600),
                        new TimedTransition(4000, "charging")
                        )
                    ),
                   new State("charging",
                        new Wander(0.1),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xFF0000, 0.25, 4),
                        new TimedTransition(1050, "fight2")
                        ),
                    new State("fight2",
                        new Prioritize(
                            new Follow(1, 8, 1),
                            new Wander(0.25)
                            ),
                        new Shoot(10, count: 1, projectileIndex: 2, coolDown: 1000),
                        new TimedTransition(4000, "fight1")
                        )
                    )
                )
                    .Init("Genisus Core",
            new State(
                new State(
                    new State("fight1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Flash(0xFFFFFF, 0.25, 6),
                        new TimedTransition(6000, "attack1")
                        )
                    ),
                new State("attack1",
                    new Shoot(10, count: 1, projectileIndex: 1, predictive: 0.5, coolDown: 6000),
                    new SpiralShoot(80, 4, 13, 20, fixedAngle: 220, range: 10, coolDown: 3000)
                    )
                    )
                )
            .Init("Guardian of the Genisus",
            new State(
                new EntityExistsTransition("Codename G-24, Nitrostorm", 10, "Orbit"),
                new State("Main",
                    new State("fight1",
                        new Prioritize(
                            new Follow(0.5, 8, 1),
                            new Wander(0.25)
                            ),
                        new Shoot(10, count: 5, shootAngle: 8, projectileIndex: 1, coolDown: 1600),
                        new TimedTransition(4000, "charging")
                        )
                    ),
                   new State("charging",
                        new Prioritize(
                            new Charge(0.75, 6, 1),
                            new Follow(0.5, 8, 1),
                            new Wander(0.25)
                            ),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xFF0000, 0.25, 4),
                        new Shoot(10, count: 8, projectileIndex: 2, coolDown: 400),
                        new TimedTransition(2000, "fight2")
                        ),
                    new State("fight2",
                        new Wander(0.25),
                        new Shoot(10, count: 5, shootAngle: 8, projectileIndex: 1, coolDown: 1600),
                        new TimedTransition(4000, "fight1")
                        ),
                    new State("Orbit",
                        new Orbit(0.5, 4, 10, target: "Codename G-24, Nitrostorm", speedVariance: 0.5),
                        new Shoot(10, count: 8, projectileIndex: 2, coolDown: 400),
                        new Shoot(10, count: 2, shootAngle: 8, projectileIndex: 1, coolDown: 2000),
                        new Shoot(10, count: 2, shootAngle: 8, projectileIndex: 0, predictive: 1, coolDown: 2000)
                        )
                    )
                )
            ;
    }
}