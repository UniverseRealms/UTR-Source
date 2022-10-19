using common.resources;
using StackExchange.Redis;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Lotll = () => Behav()
        .Init("Lord of the Lost Lands",
                new State(
                    new ScaleHP(0.5),
                    new DropPortalOnDeath("Lost Halls Portal", 1, timeout: 60),
                    new HpLessTransition(0.15, "IMDONELIKESOOOODONE!"),
                    new State("timetogeticey",
                        new PlayerWithinTransition(8, "announce")
                        ),
                    new State("announce",
                        new Announce("A Lord of the Lost Lands has been discovered in the Realm!"),
                        new TimedTransition(1000, "startupandfireup")
                        ),
                    new State("startupandfireup",
                        new SetAltTexture(0),
                        new Wander(0.3),
                        new Shoot(10, count: 7, shootAngle: 7, coolDownOffset: 1100, angleOffset: 270, coolDown: 2250),
                        new Shoot(10, count: 7, shootAngle: 7, coolDownOffset: 1100, angleOffset: 90, coolDown: 2250),
                        new Shoot(10, count: 7, shootAngle: 7, coolDown: 2250),
                        new Shoot(10, count: 7, shootAngle: 7, angleOffset: 180, coolDown: 2250),
                        new TimedTransition(10000, "GatherUp")
                        ),
                    new State("GatherUp",
                        new SetAltTexture(3),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Taunt("GATHERING POWER!"),
                        new Shoot(8.4, count: 6, shootAngle: 60, projectileIndex: 1, coolDown: 2000, coolDownOffset: 2000),
                        new Shoot(8.4, count: 6, shootAngle: 60, predictive: 2, projectileIndex: 1, coolDown: 2000, coolDownOffset: 1000),
                        new TimedTransition(5750, "protect")
                        ),
                    new State("protect",
                        //Minions spawn
                        new ConditionalEffect(ConditionEffectIndex.StunImmune),
                        new TossObject("Guardian of the Lost Lands", 5, 0, coolDown: 9999999),
                        new TossObject("Guardian of the Lost Lands", 5, 90, coolDown: 9999999),
                        new TossObject("Guardian of the Lost Lands", 5, 180, coolDown: 9999999),
                        new TossObject("Guardian of the Lost Lands", 5, 270, coolDown: 9999999),
                        new TimedTransition(1000, "crystals")
                        ),
                    new State("crystals",
                        new SetAltTexture(1),
                        new ConditionalEffect(ConditionEffectIndex.StunImmune),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TossObject("Protection Crystal", 4, 0, coolDown: 9999999),
                        new TossObject("Protection Crystal", 4, 45, coolDown: 9999999),
                        new TossObject("Protection Crystal", 4, 90, coolDown: 9999999),
                        new TossObject("Protection Crystal", 4, 135, coolDown: 9999999),
                        new TossObject("Protection Crystal", 4, 180, coolDown: 9999999),
                        new TossObject("Protection Crystal", 4, 225, coolDown: 9999999),
                        new TossObject("Protection Crystal", 4, 270, coolDown: 9999999),
                        new TossObject("Protection Crystal", 4, 315, coolDown: 9999999),
                        new TimedTransition(2100, "checkforcrystals")
                        ),
                    new State("checkforcrystals",
                        new SetAltTexture(1),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntitiesNotExistsTransition(9999, "startupandfireup", "Protection Crystal")
                        ),
                    new State("IMDONELIKESOOOODONE!",
                        new Taunt("NOOOOOOOOOOOOOOO!"),
                        new SetAltTexture(3),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new Flash(0xFF0000, 0.2, 3),
                        new TimedTransition(5250, "dead")
                        ),
                    new State("dead",
                        new Shoot(8.4, count: 6, shootAngle: 60, projectileIndex: 1),
                        new Suicide()
                        )
                    ),
                new Threshold(0.01,
                    new ItemLoot("Prism of Prestige", 0.0084),
                    new ItemLoot("Orc's Battleplate", 0.0046),
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
            .Init("Protection Crystal",
                new State(
                    new Prioritize(
                        new Orbit(0.3, 4, 10, "Lord of the Lost Lands")
                        ),
                    new Shoot(8, count: 4, shootAngle: 7, coolDown: 500)
                    )
            )
            .Init("Guardian of the Lost Lands",
                new State(
                    new State("Full",
                        new Spawn("Knight of the Lost Lands", 2, 1, coolDown: 4000),
                        new Prioritize(
                            new Follow(0.6, 20, 6),
                            new Wander(0.2)
                            ),
                        new Shoot(10, count: 8, fixedAngle: 360 / 8, coolDown: 3000, projectileIndex: 1),
                        new Shoot(10, count: 5, shootAngle: 10, coolDown: 1500),
                        new HpLessTransition(0.25, "Low")
                        ),
                    new State("Low",
                        new Prioritize(
                            new StayBack(0.6, 5),
                            new Wander(0.2)
                            ),
                        new Shoot(10, count: 8, fixedAngle: 360 / 8, coolDown: 3000, projectileIndex: 1),
                        new Shoot(10, count: 5, shootAngle: 10, coolDown: 1500)
                        )
                    ),
                new ItemLoot("Health Potion", 0.1),
                new ItemLoot("Magic Potion", 0.1)
            )
            .Init("Knight of the Lost Lands",
                new State(
                    new Prioritize(
                        new Follow(1, 20, 4),
                        new StayBack(0.5, 2),
                        new Wander(0.3)
                        ),
                    new Shoot(13, 1, coolDown: 700)
                    ),
                new ItemLoot("Health Potion", 0.1),
                new ItemLoot("Magic Potion", 0.1)
            )
            ;
    }
}