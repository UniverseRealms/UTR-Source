using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Phantom = () => Behav()
            .Init("Elemental Phantom",
                new State(
                    new ScaleHP(0.3),
                    new ConditionalEffect(ConditionEffectIndex.StasisImmune),
                    new State("default",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new PlayerWithinTransition(8, "taunt1")
                        ),
                    new State("taunt1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Taunt("You think you are going to survive? I do not think this is true."),
                        new TimedTransition(5000, "taunt2")
                        ),
                    new State("taunt2",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Taunt("I will show you my power."),
                        new TimedTransition(5000, "taunt3")
                        ),
                    new State("taunt3",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Flash(0xFFFFFF, 2, 2),
                        new Taunt("Haha, so what will it be? Freeze or Burn to death?"),
                        new TimedTransition(5000, "CircleShot")
                        ),
                    new State("tauntcool",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Flash(0xFFFFFF, 2, 2),
                        new Taunt("Better watch out!", "Let Urios live forever!", "Your souls will freeze then burn!"),
                        new TimedTransition(3000, "CircleShot")
                        ),
                    new State("CircleShot",
                       new Prioritize(
                            new StayCloseToSpawn(0.6, 5),
                            new Wander(0.2)
                            ),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Shoot(10, count: 10, projectileIndex: 5, coolDown: 500),
                        new Shoot(10, count: 3, shootAngle: 16, projectileIndex: 4, coolDown: 3000, coolDownOffset: 200),
                        new Shoot(10, count: 4, shootAngle: 16, projectileIndex: 4, coolDown: 3000, coolDownOffset: 500),
                        new Shoot(10, count: 5, shootAngle: 16, projectileIndex: 4, coolDown: 3000, coolDownOffset: 900),
                        new Shoot(10, count: 6, shootAngle: 16, projectileIndex: 4, coolDown: 3000, coolDownOffset: 1300),
                        new Shoot(10, count: 7, shootAngle: 16, projectileIndex: 4, coolDown: 3000, coolDownOffset: 1700),
                        new TimedTransition(9000, "LinePhase")
                        ),
                    new State("LinePhase",
                        new Prioritize(
                            new Follow(0.4, 8, 1),
                            new StayCloseToSpawn(0.4, 6)
                            ),
                        new Taunt("Fire and Ice, neither is nice!"),
                        new Shoot(10, count: 1, projectileIndex: 0, coolDown: 750),
                        new Shoot(10, count: 1, projectileIndex: 0, coolDown: 750, coolDownOffset: 250),
                        new Shoot(10, count: 1, projectileIndex: 1, coolDown: 750),
                        new Shoot(10, count: 1, projectileIndex: 1, coolDown: 750, coolDownOffset: 250),
                        new TimedTransition(8000, "ViciousPhase")
                    ),
                   new State("ViciousPhase",
                        new Prioritize(
                            new StayCloseToSpawn(0.4, 4),
                            new Wander(0.2)
                            ),
                        new Taunt("I don't think you'll enjoy this!", "You will be destroyed!"),
                        new Shoot(10, count: 7, projectileIndex: 2, coolDown: 1500),
                        new Grenade(3, 360, 6, coolDown: 3500),
                        new TimedTransition(7000, "ShotgunPhase")
                       ),
                   new State("ShotgunPhase",
                        new Prioritize(
                            new StayCloseToSpawn(1, 5),
                            new Wander(0.2)
                            ),
                        new ConditionalEffect(ConditionEffectIndex.ParalyzeImmune),
                        new ConditionalEffect(ConditionEffectIndex.StunImmune),
                        new Shoot(10, count: 12, shootAngle: 18, projectileIndex: 3, coolDown: 2750),
                        new Shoot(10, count: 6, shootAngle: 20, predictive: 3, projectileIndex: 4, coolDown: 2250),
                        new Shoot(10, count: 6, shootAngle: 20, predictive: 3, projectileIndex: 5, coolDown: 2250),
                        new TimedTransition(7000, "tauntcool")
                       )
                    ),
                new Threshold(0.01,
                    new ItemLoot("Amulet of the Fallen Star", 0.0084),
                    new ItemLoot("Elemental Tapestry", 0.0046),
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
            );
    }
}