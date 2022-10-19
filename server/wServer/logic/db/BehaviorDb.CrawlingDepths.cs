using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ CrawlingDepths = () => Behav()
                        /*  .Init("Son of Arachna",
                              new State(
                                  new RealmPortalDrop(),
                                  new State("default",
                                      new PlayerWithinTransition(7.2, "fight")
                                      ),
                                  new State("fight",
                                  new If(
                                       new EntityCountGreaterThan("Yellow Son of Arachna Giant Egg Sac", 9999, 0),
                                       new Shoot(25, projectileIndex: 0, count: 8, coolDown: 3000, coolDownOffset: 4000)
                                      )
                                      ),
                                  new State("shrink",
                                      new Wander(0.4),
                                      new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                      new ChangeSize(-15, 25),
                                      new TimedTransition(1000, "smallAttack")
                                      ),
                                  new State("smallAttack",
                                      new Prioritize(
                                          new Follow(1, acquireRange: 15, range: 8),
                                          new Wander(1)
                                      ),
                                      new Shoot(10, predictive: 1, coolDown: 750),
                                      new Shoot(10, 6, projectileIndex: 1, predictive: 1, coolDown: 1000),
                                      new TimedTransition(10000, "grow")
                                      ),
                                  new State("grow",
                                      new Wander(0.1),
                                      new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                      new ChangeSize(35, 200),
                                      new TimedTransition(1050, "bigAttack")
                                      ),
                                  new State("bigAttack",
                                      new Prioritize(
                                          new Follow(0.2),
                                          new Wander(0.1)
                                      ),
                                      new Shoot(10, projectileIndex: 2, predictive: 1, coolDown: 2000),
                                      new Shoot(10, projectileIndex: 2, predictive: 1, coolDownOffset: 300, coolDown: 2000),
                                      new Shoot(10, 3, projectileIndex: 3, predictive: 1, coolDownOffset: 100, coolDown: 2000),
                                      new Shoot(10, 3, projectileIndex: 3, predictive: 1, coolDownOffset: 400, coolDown: 2000),
                                      new TimedTransition(10000, "normalize")
                                      ),
                                  new State("normalize",
                                      new Wander(0.3),
                                      new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                      new ChangeSize(-20, 100),
                                      new TimedTransition(1000, "basic")
                                      )
                                  ),
                              new Threshold(0.03,
                                  new TierLoot(10, ItemType.Weapon, 0.06),
                                  new TierLoot(11, ItemType.Weapon, 0.05),
                                  new TierLoot(12, ItemType.Weapon, 0.04),
                                  new TierLoot(5, ItemType.Ability, 0.06),
                                  new TierLoot(6, ItemType.Ability, 0.04),
                                  new TierLoot(11, ItemType.Armor, 0.06),
                                  new TierLoot(12, ItemType.Armor, 0.05),
                                  new TierLoot(13, ItemType.Armor, 0.04),
                                  new TierLoot(5, ItemType.Ring, 0.05),
                                  new ItemLoot("Potion of Mana", 1),
                                  new ItemLoot("Doku No Ken", 0.01)
                                  )
                          )*/
                .Init("Son of Arachna",
                new State(
                    new ScaleHP(0.3),
                    new ConditionEffectRegion(ConditionEffectIndex.Slowed, 3, 3),
                    new RealmPortalDrop(),
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new PlayerWithinTransition(12, "2")
                    ),
                    new State("2",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("RAAH!!"),
                        new Flash(0xFF0000, 1, 2),
                        new TimedTransition(2000, "3")
                        ),
                    new State("3",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("RISE MY CHILDREN!"),
                        new ReturnToSpawn(1),
                        new InvisiToss("Blue Son of Arachna Giant Egg Sac", 10, 90, coolDown: 900000),
                        new InvisiToss("Yellow Son of Arachna Giant Egg Sac", 10, 180, coolDown: 900000),
                        new InvisiToss("Red Son of Arachna Giant Egg Sac", 10, 270, coolDown: 900000),
                        new InvisiToss("Silver Son of Arachna Giant Egg Sac", 10, 360, coolDown: 900000),
                        new TimedTransition(1000, "3a")
                        ),
                    new State("3a",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ReturnToSpawn(1),
                        new EntitiesNotExistsTransition(99, "4", "Blue Son of Arachna Giant Egg Sac", "Yellow Son of Arachna Giant Egg Sac", "Red Son of Arachna Giant Egg Sac", "Silver Son of Arachna Giant Egg Sac")
                        ),
                    new State("4",
                        new Reproduce("Crawling Grey Spotted Spider", 3, 5, coolDown: 4000),
                        new Wander(0.6),
                        new Follow(0.6, 10, 3, coolDown: 100),
                        new Shoot(99, 5, 52, 3, coolDown: 900),
                        new Shoot(99, 5, 16, 4, coolDown: 900, coolDownOffset: 300),
                        new HpLessTransition(0.5, "5")
                        ),
                    new State("5",
                        new Wander(0.5),
                        new Taunt("I WILL STRING YOU UP!"),
                        new Shoot(99, 3, 17, 1, coolDown: 4000),
                        new Shoot(99, 1, null, 0, coolDown: 400),
                        new HpLessTransition(0.1, "6")
                        ),
                    new State("6",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("BROTHERS AND SISTERS, TO ARMS. WE WILL RETURN."),
                        new Flash(0xFFFFFF, 1, 4),
                        new TimedTransition(4000, "7")
                        ),
                    new State("7",
                        new Suicide()
                            )
                        ),
                new Threshold(0.01,
                    new ItemLoot("Greater Potion of Speed", 1),
                    new ItemLoot("Garments of the Underground", 0.01),
                    new ItemLoot("Sheath of the Vengeful Son", 0.01),
                    new ItemLoot("Stinger of the Sinful Brother", 0.005),
                    new TierLoot(3, ItemType.Weapon, 0.05),
                    new TierLoot(3, ItemType.Ability, 0.05),
                    new TierLoot(3, ItemType.Armor, 0.05),
                    new TierLoot(3, ItemType.Ring, 0.05),
                    new TierLoot(4, ItemType.Weapon, 0.025),
                    new TierLoot(4, ItemType.Ability, 0.025),
                    new TierLoot(4, ItemType.Armor, 0.025),
                    new TierLoot(4, ItemType.Ring, 0.025),
                    new ItemLoot("The Crawling Depths Key", 0.005),
                    new ItemLoot("Rusted Katana", 0.00112),
                    new ItemLoot("Eternal Essence", 0.0005),
                    new ItemLoot("Arachnid Pet Stone", 0.001)
                    )
                )
           .Init("Crawling Depths Egg Sac",
                new State(
                    new ScaleHP(0.1),
                    new State("CheckOrDeath",
                    new PlayerWithinTransition(4, "Urclose"),
                    new TransformOnDeath("Crawling Spider Hatchling", 5, 7)
                    ),
                new State("Urclose",
                    new Spawn("Crawling Spider Hatchling", 6),
                    new Suicide()
            )))
         .Init("Crawling Spider Hatchling",
                new State(
                    new ScaleHP(0.1),
                    new SetNoXP(),
                    new Prioritize(
                        new Wander(.4)
                    ),
                    new Shoot(7, count: 1, shootAngle: 0, coolDown: 650),
                    new Shoot(7, count: 1, shootAngle: 0, projectileIndex: 1, predictive: 1, coolDown: 850)
                )
            )
                 .Init("Crawling Grey Spotted Spider",
                new State(
                    new ScaleHP(0.1),
                    new Prioritize(
                        new Charge(2, 8, 1050),
                        new Wander(.4)
                    ),
                    new Shoot(10, count: 1, shootAngle: 0, coolDown: 500)
                ),
                new ItemLoot("Healing Ichor", 0.2),
                new ItemLoot("Health Potion", 0.3)
            )
          .Init("Crawling Grey Spider",
                new State(
                    new ScaleHP(0.1),
                    new Prioritize(
                        new Charge(2, 8, 1050),
                        new Wander(.4)
                    ),
                    new Shoot(9, count: 1, shootAngle: 0, coolDown: 850)
                ),
                new ItemLoot("Healing Ichor", 0.2),
                new ItemLoot("Health Potion", 0.3)
            )
        .Init("Crawling Red Spotted Spider",
                new State(
                    new ScaleHP(0.1),
                    new Prioritize(
                        new Wander(.4)
                    ),
                    new Shoot(8, count: 1, shootAngle: 0, coolDown: 750)
                ),
                new ItemLoot("Healing Ichor", 0.2),
                new ItemLoot("Health Potion", 0.3)
            )
         .Init("Crawling Green Spider",
                new State(
                    new ScaleHP(0.1),
                    new Prioritize(
                        new Follow(.6, 11, 1),
                        new Wander(.4)
                    ),
                    new Shoot(8, count: 3, shootAngle: 10, coolDown: 400)
                ),
                new ItemLoot("Healing Ichor", 0.2),
                new ItemLoot("Health Potion", 0.3)
            )
         .Init("Yellow Son of Arachna Giant Egg Sac",
                new State(
                    new ScaleHP(0.1),
                    new TransformOnDeath("Yellow Egg Summoner"),
                new State("Spawn",
                    new Spawn("Crawling Green Spider", 2),
                    new EntityNotExistsTransition("Crawling Green Spider", 20, "Spawn2")
                    ),
                new State("Spawn2",
                    new Spawn("Crawling Grey Spider", 2),
                    new EntityNotExistsTransition("Crawling Grey Spider", 20, "Spawn3")
                    ),
                new State("Spawn3",
                    new Spawn("Crawling Red Spotted Spider", 2),
                    new EntityNotExistsTransition("Crawling Red Spotted Spider", 20, "Spawn4")
                    ),
                 new State("Spawn4",
                    new Spawn("Crawling Spider Hatchling", 2),
                    new EntityNotExistsTransition("Crawling Spider Hatchling", 20, "Spawn5")
                     ),
                 new State("Spawn5",
                    new Spawn("Crawling Grey Spotted Spider", 2),
                    new EntityNotExistsTransition("Crawling Grey Spotted Spider", 20, "Spawn")
            )),
                new Threshold(0.15,
                    new ItemLoot("Potion of Speed", 0.25),
                    new ItemLoot("Garments of the Underground", 0.0025),
                    new ItemLoot("Sheath of the Vengeful Son", 0.0025),
                    new ItemLoot("Stinger of the Sinful Brother", 0.00125),
                    new TierLoot(3, ItemType.Weapon, 0.0125),
                    new TierLoot(3, ItemType.Ability, 0.0125),
                    new TierLoot(3, ItemType.Armor, 0.0125),
                    new TierLoot(3, ItemType.Ring, 0.0125),
                    new TierLoot(4, ItemType.Weapon, 0.00625),
                    new TierLoot(4, ItemType.Ability, 0.00625),
                    new TierLoot(4, ItemType.Armor, 0.00625),
                    new TierLoot(4, ItemType.Ring, 0.00625),
                    new ItemLoot("Rusted Katana", 0.00028),
                    new ItemLoot("Eternal Essence", 0.000125)
                    )
            )
         .Init("Blue Son of Arachna Giant Egg Sac",
                new State(
                    new ScaleHP(0.1),
                    new State("DeathSpawn",
                    new TransformOnDeath("Crawling Spider Hatchling", 5, 7)

            )),
                new Threshold(0.15,
                    new ItemLoot("Potion of Speed", 0.25),
                    new ItemLoot("Garments of the Underground", 0.0025),
                    new ItemLoot("Sheath of the Vengeful Son", 0.0025),
                    new ItemLoot("Stinger of the Sinful Brother", 0.00125),
                    new TierLoot(3, ItemType.Weapon, 0.0125),
                    new TierLoot(3, ItemType.Ability, 0.0125),
                    new TierLoot(3, ItemType.Armor, 0.0125),
                    new TierLoot(3, ItemType.Ring, 0.0125),
                    new TierLoot(4, ItemType.Weapon, 0.00625),
                    new TierLoot(4, ItemType.Ability, 0.00625),
                    new TierLoot(4, ItemType.Armor, 0.00625),
                    new TierLoot(4, ItemType.Ring, 0.00625),
                    new ItemLoot("Rusted Katana", 0.00028),
                    new ItemLoot("Eternal Essence", 0.000125)
                    ))
         .Init("Red Son of Arachna Giant Egg Sac",
                new State(
                    new ScaleHP(0.1),
                    new State("DeathSpawn",
                    new TransformOnDeath("Crawling Red Spotted Spider", 3, 3)

            )),
                new Threshold(0.15,
                    new ItemLoot("Potion of Speed", 0.25),
                    new ItemLoot("Garments of the Underground", 0.0025),
                    new ItemLoot("Sheath of the Vengeful Son", 0.0025),
                    new ItemLoot("Stinger of the Sinful Brother", 0.00125),
                    new TierLoot(3, ItemType.Weapon, 0.0125),
                    new TierLoot(3, ItemType.Ability, 0.0125),
                    new TierLoot(3, ItemType.Armor, 0.0125),
                    new TierLoot(3, ItemType.Ring, 0.0125),
                    new TierLoot(4, ItemType.Weapon, 0.00625),
                    new TierLoot(4, ItemType.Ability, 0.00625),
                    new TierLoot(4, ItemType.Armor, 0.00625),
                    new TierLoot(4, ItemType.Ring, 0.00625),
                    new ItemLoot("Rusted Katana", 0.00028),
                    new ItemLoot("Eternal Essence", 0.000125)
                    ))
         .Init("Silver Son of Arachna Giant Egg Sac",
                new State(
                    new ScaleHP(0.1),
                    new State("DeathSpawn",
                    new TransformOnDeath("Crawling Grey Spider", 3, 3)

            )),
                new Threshold(0.15,
                    new ItemLoot("Potion of Speed", 0.25),
                    new ItemLoot("Garments of the Underground", 0.0025),
                    new ItemLoot("Sheath of the Vengeful Son", 0.0025),
                    new ItemLoot("Stinger of the Sinful Brother", 0.00125),
                    new TierLoot(3, ItemType.Weapon, 0.0125),
                    new TierLoot(3, ItemType.Ability, 0.0125),
                    new TierLoot(3, ItemType.Armor, 0.0125),
                    new TierLoot(3, ItemType.Ring, 0.0125),
                    new TierLoot(4, ItemType.Weapon, 0.00625),
                    new TierLoot(4, ItemType.Ability, 0.00625),
                    new TierLoot(4, ItemType.Armor, 0.00625),
                    new TierLoot(4, ItemType.Ring, 0.00625),
                    new ItemLoot("Rusted Katana", 0.00028),
                    new ItemLoot("Eternal Essence", 0.000125)
                    )
            )
         .Init("Silver Egg Summoner",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                    )
                )
         .Init("Yellow Egg Summoner",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                    )
                )
         .Init("Red Egg Summoner",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                    )
                )
         .Init("Blue Egg Summoner",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                    )
                )
         .Init("Epic Arachna Web Spoke 1",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new Shoot(200, count: 1, fixedAngle: 180, coolDown: 400),
                new Shoot(200, count: 1, fixedAngle: 120, coolDown: 400),
                new Shoot(200, count: 1, fixedAngle: 240, coolDown: 400)
                    )
            )
           .Init("Epic Arachna Web Spoke 2",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new Shoot(200, count: 1, fixedAngle: 240, coolDown: 400),
                new Shoot(200, count: 1, fixedAngle: 180, coolDown: 400),
                new Shoot(200, count: 1, fixedAngle: 300, coolDown: 400)
                    )
            )
           .Init("Epic Arachna Web Spoke 3",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new Shoot(200, count: 1, fixedAngle: 300, coolDown: 400),
                new Shoot(200, count: 1, fixedAngle: 240, coolDown: 400),
                new Shoot(200, count: 1, fixedAngle: 0, coolDown: 400)
                    )
            )
           .Init("Epic Arachna Web Spoke 4",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new Shoot(200, count: 1, fixedAngle: 0, coolDown: 400),
                new Shoot(200, count: 1, fixedAngle: 60, coolDown: 400),
                new Shoot(200, count: 1, fixedAngle: 300, coolDown: 400)
                    )
            )
           .Init("Epic Arachna Web Spoke 5",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new Shoot(200, count: 1, fixedAngle: 60, coolDown: 400),
                new Shoot(200, count: 1, fixedAngle: 0, coolDown: 400),
                new Shoot(200, count: 1, fixedAngle: 120, coolDown: 400)
     )
            )
           .Init("Epic Arachna Web Spoke 6",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new Shoot(200, count: 1, fixedAngle: 120, coolDown: 400),
                new Shoot(200, count: 1, fixedAngle: 60, coolDown: 400),
                new Shoot(200, count: 1, fixedAngle: 180, coolDown: 400)
                    )
            )
           .Init("Epic Arachna Web Spoke 7",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new Shoot(200, count: 1, fixedAngle: 180, coolDown: 400),
                new Shoot(200, count: 1, fixedAngle: 120, coolDown: 400),
                new Shoot(200, count: 1, fixedAngle: 240, coolDown: 400)
                    )
            )
           .Init("Epic Arachna Web Spoke 8",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new Shoot(200, count: 1, fixedAngle: 360, coolDown: 400),
                new Shoot(200, count: 1, fixedAngle: 240, coolDown: 400),
                new Shoot(200, count: 1, fixedAngle: 300, coolDown: 400)
                    )
            )
           .Init("Epic Arachna Web Spoke 9",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new Shoot(200, count: 1, fixedAngle: 0, coolDown: 400),
                new Shoot(200, count: 1, fixedAngle: 60, coolDown: 400),
                new Shoot(200, count: 1, fixedAngle: 120, coolDown: 400)
                    )
            );
    }
}