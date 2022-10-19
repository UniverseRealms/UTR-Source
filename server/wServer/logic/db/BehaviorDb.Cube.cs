using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ CubeGod = () => Behav()
             .Init("Cube God",
                 new State(
                     new ScaleHP(0.3),
                     new StayCloseToSpawn(0.3, range: 7),
                            new Wander(0.5),
                              new Shoot(10, count: 9, predictive: 0.9, shootAngle: 6.5, coolDown: 1000),
                              new Shoot(10, count: 6, predictive: 0.9, shootAngle: 6.5, projectileIndex: 1, coolDown: 1000, coolDownOffset: 200),
                              new Spawn("Cube Overseer", maxChildren: 5, initialSpawn: 3, coolDown: 100000),
                              new Spawn("Cube Defender", maxChildren: 5, initialSpawn: 5, coolDown: 100000),
                              new Spawn("Cube Blaster", maxChildren: 5, initialSpawn: 5, coolDown: 100000)
                 ),
                new Threshold(0.01,
                    new ItemLoot("Shapeshift Hide", 0.0084),
                    new ItemLoot("Gelatinous Dagger", 0.0046),
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
             .Init("Cube Overseer",
                 new State(
                     new StayCloseToSpawn(0.3, range: 7),
                              new Wander(1),
                              new Shoot(10, count: 4, predictive: 0.9, projectileIndex: 0, coolDown: 1250)
                 )
             )
             .Init("Cube Defender",
                 new State(
                     new Wander(0.5),
                              new StayCloseToSpawn(0.03, range: 7),
                              new Follow(0.4, acquireRange: 9, range: 2),
                              new Shoot(10, count: 1, coolDown: 1000, predictive: 0.9, projectileIndex: 0)
                 )
             )
             .Init("Cube Blaster",
                 new State(
                     new Wander(0.5),
                              new StayCloseToSpawn(0.03, range: 7),
                              new Follow(0.4, acquireRange: 9, range: 2),
                              new Shoot(10, count: 2, predictive: 0.9, projectileIndex: 0, coolDown: 1500),
                              new Shoot(10, count: 1, predictive: 0.9, projectileIndex: 0, coolDown: 1500)
                 )
             );
    }
}