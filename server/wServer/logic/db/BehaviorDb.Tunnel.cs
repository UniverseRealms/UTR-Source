using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Tunnel = () => Behav()
#region Tunnel Minions
              .Init("Tunnel Fearless Ranger",
                  new State(
                    new State("gitgud",
                        new Follow(0.35, 8, 1),
                        new Shoot(10, count: 1, projectileIndex: 0, coolDown: 2550),
                        new Shoot(10, count: 3, shootAngle: 18, projectileIndex: 1, coolDown: 1250),
                        new HpLessTransition(0.35, "gitgud2")
                        ),
                    new State("gitgud2",
                    new Orbit(0.55, 2, target: null),
                     new Shoot(10, count: 2, shootAngle: 18, projectileIndex: 1, coolDown: 650),
                    new Shoot(10, count: 5, shootAngle: 18, projectileIndex: 0, coolDown: 1900)
                       )
                    ),
                new ItemLoot("Magic Potion", 0.25),
                new ItemLoot("Health Potion", 0.25)
              )
        .Init("Tunnel Fearless Archer",
                  new State(
                    new State("gitgud",
                        new Wander(0.3),
                        new Shoot(10, count: 1, projectileIndex: 0, coolDown: 920),
                        new HpLessTransition(0.30, "gitgud2")
                        ),
                    new State("gitgud2",
                    new Follow(0.5, 8, 1),
                    new Shoot(10, count: 3, shootAngle: 20, projectileIndex: 0, coolDown: 1900)
                       )
                    ),
                  new TierLoot(6, ItemType.Weapon, 0.2),
                  new TierLoot(7, ItemType.Weapon, 0.1)
                 )
        .Init("Tunnel Faceless Evil",
                  new State(
                    new State("gitgud",
                        new Follow(0.6, 8, 1),
                        new Shoot(10, count: 1, projectileIndex: 0, coolDown: 400)
               )))
        .Init("Tunnel Armored Mage",
                  new State(
                    new State("ShootStaff",
                        new Wander(0.42),
                        new Shoot(10, count: 2, shootAngle: 10, projectileIndex: 0, coolDown: 550),
                        new TimedTransition(5000, "Ep")
                        ),
                    new State("Ep",
                    new Follow(0.32, 8, 1),
                    new HealSelf(coolDown: 4000, amount: 600),
                    new Shoot(10, count: 8, projectileIndex: 1, coolDown: 1750),
                    new TimedTransition(5000, "ShootStaff2")
                       ),
                   new State("ShootStaff2",
                        new BackAndForth(0.35, 6),
                        new Shoot(10, count: 2, shootAngle: 10, projectileIndex: 0, coolDown: 400),
                        new TimedTransition(5000, "ShootStaff")
                        )
                    ),
                new ItemLoot("Magic Potion", 0.25),
                new ItemLoot("Health Potion", 0.25)
                 )
           .Init("Tunnel Spearman of Pain",
                new State(
                  new State("gitgud",
                      new Wander(0.42),
                      new Shoot(10, count: 1, projectileIndex: 0, coolDown: 700, coolDownOffset: 900),
                      new TimedTransition(4250, "gitgud2")
                      ),
                  new State("gitgud2",
                  new Follow(0.38, 8, 1),
                  new Shoot(10, count: 2, shootAngle: 1, projectileIndex: 0, coolDown: 400),
                  new TimedTransition(1000, "gitgud")
                     )
                  )
               )
      .Init("Tunnel Mini Eye",
                new State(
                  new State("swag",
                      new Follow(1.25, 8, 1),
                      new Shoot(10, count: 1, projectileIndex: 0, coolDown: 1500),
                       new Shoot(10, count: 1, projectileIndex: 0, predictive: 2.5, coolDown: 525)
             )))
      .Init("Tunnel N",
                new State(
                     new State("waitforaperson",
                      new Wander(0.6),
                      new ConditionalEffect(ConditionEffectIndex.Invincible),
                      new PlayerWithinTransition(7, "RingShotgun")
                      ),
                new State(
                  new Spawn("Tunnel Mini Eye", 1, 2, coolDown: 3000),
                  new State("RingShotgun",
                      new Taunt(0.50, "...."),
                      new Wander(0.4),
                      new Shoot(10, count: 10, projectileIndex: 0, coolDown: 1250),
                      new Shoot(10, count: 3, shootAngle: 30, projectileIndex: 1, coolDown: 1600),
                      new TimedTransition(6000, "Charge&Insta")
                      ),
                 new State("Charge&Insta",
                     new Prioritize(
                       new Charge(0.5, 7, coolDown: 1000),
                       new Follow(0.45, 8, 1)
                     ),
                      new Shoot(10, count: 8, projectileIndex: 0, coolDown: 1250, coolDownOffset: 1300),
                      new Shoot(10, count: 6, shootAngle: 30, projectileIndex: 1, coolDown: 1600),
                      new TimedTransition(3000, "RingShotgun")
                        )
                      )
                    )
                  )
        #endregion Tunnel Minions
#region Misc
        .Init("Tunnel Spike",
               new State(
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("spike",
                    new Shoot(100, 1, fixedAngle: 0, projectileIndex: 0, coolDown: 500)
                    )
                )
            )
         .Init("Tunnel Arrow Turret1",
                    new State(
                       new SetNoXP(),
                       new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("wait",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new TimedRandomTransition(500, true, "pew pew")
                        ),
                     new State("pew pew",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Shoot(10, 1, fixedAngle: 90, coolDown: 2000)
                        )
            )
            )

            .Init("Tunnel Arrow Turret2",
             new State(
                       new SetNoXP(),
                       new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("wait",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new TimedRandomTransition(600, true, "pew pew")
                        ),
                     new State("pew pew",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Shoot(10, 1, fixedAngle: 270, coolDown: 2000)
                        )
            )
            )
#endregion misc
#region Test Chest
        /*.Init("Varghus Test Chest",
              new State(
                  new State("Idle",
                      new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                      new TimedTransition(5000, "UnsetEffect")
                  ),
                  new State("UnsetEffect")
              ),
              new Threshold(0.15,
              new ItemLoot("Soulreaper Armor", 0.035),
              new ItemLoot("Nether Blade", 0.035),
              new ItemLoot("Shadow Beacon", 0.035),
              new ItemLoot("Staff of Dark Malediction", 0.035)
              )
          )*/
        #endregion Test Chest
#region Vargus the Eye & Spawns
        .Init("Tunnel Ground Changer",
            new State(
                new State("waiting",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("sleep",
                    new ChangeGroundOnDeath(new[] { "Purple Rug", "TOP Ground" }, new[] { "Black" }, 100),
                    new Suicide()
                    )
                )
            )
        .Init("Tunnel Ground Changer 2", 
            new State(
                new TransformOnDeath("Realm Portal"),
                new State("check",
                    new EntityNotExistsTransition("Tunnel Varghus the Eye", 50, "die")
                    ),
                new State("die",
                    new Suicide()
                    )
                )
            )
        .Init("Tunnel Hazv Power Thing",
        new State(
            new ConditionalEffect(ConditionEffectIndex.Invincible),
            new State("Idle",
                new EntityExistsTransition("FateT", 9999, "Active")
                ),
            new State("Active",
            new Shoot(10, count: 5, projectileIndex: 0, coolDown: 400),
            new EntitiesNotExistsTransition(9999, "Idle", "FateT")
            )
            )
        )
        .Init("Tunnel Varghus the Eye",
            new State(
                new ScaleHP(0.3),
                //new TransformOnDeath("Varghus Test Chest", 1, 1, 1),
                new State("default",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new PlayerWithinTransition(8, "setthethingies")
                    ),
                    new State("setthethingies",
                    new Taunt("Is somebody there? EYE SEE YOU!"),
                    new TimedTransition(3000, "dark")
                    ),
                new State("dark",
                    new Taunt("Is it just me?"),
                    new TimedTransition(1500, "dark2")
                    ),
                new State("dark2",
                    new Taunt("Or did everything just get a little bit..."),
                    new TimedTransition(1500, "dark3")
                    ),
                new State("dark3",
                    new Taunt("DARKER."),
                    new OrderOnce(100, "Tunnel Ground Changer", "sleep"),
                    new TimedTransition(1000, "corrupt")
                    ),
                new State("corrupt",
                    new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                    new HpLessTransition(0.4, "minion"),
                    new ConditionEffectRegion(ConditionEffectIndex.Curse, 50, -1),
                    new SpiralShoot(13, 45, 8, 45, 2, coolDown: 500),
                    new TimedTransition(5000, "tp1")
                    ),
                new State("tp1",
                    new HpLessTransition(0.4, "minion"),
                    new ConditionEffectRegion(ConditionEffectIndex.Curse, 50, 0),
                    new MoveTo2(0, -4, isMapPosition: false, instant: true),
                    new Taunt("YOU CANNOT STOP THE OMNISCIENT VARGHUS."),
                    new Flash(0xff0000, 1, 3),
                    new Shoot(30, count: 8, shootAngle: 20, projectileIndex: 0, predictive: 0.2, coolDown: 900, coolDownOffset: 250),
                    new GroundTransform("Oryx Castle Rug", 1, persist: false),
                    new TimedTransition(2000, "tp2")
                    ),
                new State("tp2",
                    new HpLessTransition(0.4, "minion"),
                    new MoveTo2(10, 4, isMapPosition: false, instant: true),
                    new Shoot(30, count: 8, shootAngle: 20, projectileIndex: 0, predictive: 0.2, coolDown: 900, coolDownOffset: 250),
                    new GroundTransform("Oryx Castle Rug", 1, persist: false),
                    new TimedTransition(2000, "tp3")
                    ),
                new State("tp3",
                    new HpLessTransition(0.4, "minion"),
                    new MoveTo2(-20, 0, isMapPosition: false, instant: true),
                    new Shoot(30, count: 8, shootAngle: 20, projectileIndex: 0, predictive: 0.2, coolDown: 900, coolDownOffset: 250),
                    new GroundTransform("Oryx Castle Rug", 1, persist: false),
                    new TimedTransition(2000, "corrupt2")
                    ),
                new State("corrupt2",
                    new HpLessTransition(0.4, "minion"),
                    new MoveTo2(10, 0, isMapPosition: false, instant: true),
                    new ConditionEffectRegion(ConditionEffectIndex.Curse, 50, -1),
                    new SpiralShoot(13, 45, 8, 45, 2, coolDown: 500, coolDownOffset: 1500),
                    new TimedTransition(5000, "tp1")
                    ),
                new State("minion",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new ConditionEffectRegion(ConditionEffectIndex.Curse, 50, 0),
                    new Taunt("I CAN'T SEE. . .!"),
                    new SetAltTexture(1, 2, 800, true),
                    new TimedTransition(3200, "minion2")
                    ),
                new State("minion2",
                    new SetAltTexture(1, 2, 800, true),
                    new Taunt("You may have taken my sight. . ."),
                    new Flash(0xffffff, 2, 4),
                    new TimedTransition(2500, "minion3")
                    ),
                new State("minion3",
                    new SetAltTexture(1, 2, 800, true),
                    new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                    new Taunt("BUT YOU CANNOT TAKE MY POWER!!"),
                    new Flash(0xff0000, 1, 3),
                    new SpiralShoot(13, 20, 8, 45, 3, coolDown: 200)
                    )
                ),
                new Threshold(0.01,
                    new ItemLoot("Spectral Battleplate", 0.01),
                    new ItemLoot("Helm of the Consumed", 0.01),
                    new ItemLoot("Staff of Endless Shadows", 0.005),
                    new ItemLoot("Potion of Attack", 1),
                    new TierLoot(3, ItemType.Weapon, 0.05),
                    new TierLoot(3, ItemType.Ability, 0.05),
                    new TierLoot(3, ItemType.Armor, 0.05),
                    new TierLoot(3, ItemType.Ring, 0.05),
                    new TierLoot(4, ItemType.Weapon, 0.025),
                    new TierLoot(4, ItemType.Ability, 0.025),
                    new TierLoot(4, ItemType.Armor, 0.025),
                    new TierLoot(4, ItemType.Ring, 0.025),
                    new ItemLoot("Tunnel of Pain Key", 0.005),
                    new ItemLoot("Rusted Platemail", 0.00112),
                    new ItemLoot("Eternal Essence", 0.0005),
                    new ItemLoot("Vision Pet Stone", 0.001)
                )                         
        );
#endregion varghus the eye
    }
}