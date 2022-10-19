﻿#region

using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

#endregion

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ CandyLand = () => Behav()
        .Init("Candy Gnome",
            new State(
                new DropPortalOnDeath("Candyland Portal", probability: 0.50, timeout: 120),
                new Prioritize(
                    new StayBack(1.0, 55),
                    new Wander(1.4)
                    )
                ),
            new Threshold(0.01,
                new ItemLoot("Red Gumball", 0.5),
                new ItemLoot("Purple Gumball", 0.5),
                new ItemLoot("Blue Gumball", 0.5),
                new ItemLoot("Green Gumball", 0.5),
                new ItemLoot("Yellow Gumball", 0.5),
                new ItemLoot("Rock Candy", 0.5)
                )
            )
        .Init("MegaRototo",
            new State(
                new ScaleHP(0.3),
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Idle",
                    new PlayerWithinTransition(4, "1"),
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                    ),
                new State("1",
                    new Spawn("Tiny Rototo", maxChildren: 6),
                    new Wander(0.5),
                    new Shoot(10, 4, shootAngle: 90, projectileIndex: 0, coolDownOffset: 500),
                    new Shoot(10, 4, shootAngle: 90, projectileIndex: 1, angleOffset: 45, coolDownOffset: 1000),
                    new TimedTransition(4000, "2")
                    ),
                new State("2",
                    new Shoot(10, 2, shootAngle: 10, projectileIndex: 2, coolDown: 400),
                    new Shoot(10, 1, projectileIndex: 3, coolDown: 400),
                    new TimedTransition(4000, "1")
                    )
                ),
                new Threshold(0.01,
                    new TierLoot(6, ItemType.Weapon, 0.2),
                    new TierLoot(7, ItemType.Weapon, 0.1),
                    new TierLoot(7, ItemType.Armor, 0.2),
                    new TierLoot(8, ItemType.Armor, 0.1),
                    new TierLoot(3, ItemType.Ability, 0.2),
                    new TierLoot(4, ItemType.Ability, 0.1),
                    new TierLoot(3, ItemType.Ring, 0.2),
                    new TierLoot(4, ItemType.Ring, 0.1),
                    new ItemLoot("Potion of Life", 0.08),
                    new ItemLoot("Potion of Mana", 0.08)
                )
            )
            .Init("Spoiled Creampuff",
                new State(
                    new ScaleHP(0.3),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Idle",
                    new PlayerWithinTransition(4, "1"),
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                    ),
                new State("1",
                    new Wander(0.5),
                    new Shoot(20, 2, 40, angleOffset: 60 / 3, projectileIndex: 0, coolDown: 1500),
                    new Shoot(20, 4, 15, angleOffset: 40 / 3, projectileIndex: 1, coolDown: 1000),
                    new Spawn("Big Creampuff", maxChildren: 2, initialSpawn: 2, coolDown: 5000)
                    )
                ),
                new Threshold(0.01,
                    new TierLoot(6, ItemType.Weapon, 0.2),
                    new TierLoot(7, ItemType.Weapon, 0.1),
                    new TierLoot(7, ItemType.Armor, 0.2),
                    new TierLoot(8, ItemType.Armor, 0.1),
                    new TierLoot(3, ItemType.Ability, 0.2),
                    new TierLoot(4, ItemType.Ability, 0.1),
                    new TierLoot(3, ItemType.Ring, 0.2),
                    new TierLoot(4, ItemType.Ring, 0.1),
                    new ItemLoot("Potion of Life", 0.08),
                    new ItemLoot("Potion of Mana", 0.08)
                )
            )
            .Init("Desire Troll",
                new State(
                    new ScaleHP(0.3),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Idle",
                    new PlayerWithinTransition(4, "1"),
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                    ),
                new State("1",
                    new Wander(0.5),
                    new Grenade(6, 200, range: 8, coolDown: 3000),
                    new Shoot(15, 3, 5, angleOffset: 30 / 3, projectileIndex: 0, coolDown: 2100),
                    new Shoot(15, 5, 10, angleOffset: 60 / 3, projectileIndex: 2, coolDown: 1950),
                    new Shoot(15, 1, 0, angleOffset: 30 / 3, projectileIndex: 1, coolDown: 1950)
                    )
                ),
                new Threshold(0.01,
                    new TierLoot(6, ItemType.Weapon, 0.2),
                    new TierLoot(7, ItemType.Weapon, 0.1),
                    new TierLoot(7, ItemType.Armor, 0.2),
                    new TierLoot(8, ItemType.Armor, 0.1),
                    new TierLoot(3, ItemType.Ability, 0.2),
                    new TierLoot(4, ItemType.Ability, 0.1),
                    new TierLoot(3, ItemType.Ring, 0.2),
                    new TierLoot(4, ItemType.Ring, 0.1),
                    new ItemLoot("Potion of Life", 0.08),
                    new ItemLoot("Potion of Mana", 0.08)
                )
            )
        .Init("Swoll Fairy",
            new State(
                new ScaleHP(0.3),
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Idle",
                    new PlayerWithinTransition(4, "1"),
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                    ),
                new State("1",
                    new Spawn("Fairy", maxChildren: 3),
                    new Wander(0.3),
                    new Shoot(10, 2, shootAngle: 30, projectileIndex: 0, coolDown: 400),
                    new TimedTransition(4000, "2")
                    ),
                new State("2",
                    new Wander(0.3),
                    new Shoot(10, 8, shootAngle: 45, projectileIndex: 1, fixedAngle: 0, coolDown: 400),
                    new TimedTransition(4000, "1")
                    )
                ),
                new Threshold(0.01,
                    new TierLoot(6, ItemType.Weapon, 0.2),
                    new TierLoot(7, ItemType.Weapon, 0.1),
                    new TierLoot(7, ItemType.Armor, 0.2),
                    new TierLoot(8, ItemType.Armor, 0.1),
                    new TierLoot(3, ItemType.Ability, 0.2),
                    new TierLoot(4, ItemType.Ability, 0.1),
                    new TierLoot(3, ItemType.Ring, 0.2),
                    new TierLoot(4, ItemType.Ring, 0.1),
                    new ItemLoot("Potion of Life", 0.08),
                    new ItemLoot("Potion of Mana", 0.08)
                    )
            )
            .Init("Gigacorn",
                new State(
                    new ScaleHP(0.3),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Idle",
                    new PlayerWithinTransition(4, "1"),
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                    ),
                new State("1",
                    new Wander(0.5),
                    new Charge(2.0, 10f, 4000),
                    new Shoot(20, 1, 40, angleOffset: 60 / 3, projectileIndex: 0, coolDown: 2100),
                    new Shoot(20, 1, 40, angleOffset: 60 / 3, projectileIndex: 0, coolDown: 2200),
                    new Shoot(20, 1, 40, angleOffset: 60 / 3, projectileIndex: 0, coolDown: 2300),
                    new Shoot(20, 1, 40, angleOffset: 60 / 3, projectileIndex: 0, coolDown: 2400),
                    new Shoot(20, 3, 15, angleOffset: 40 / 3, projectileIndex: 1, coolDown: 4000),
                    new Shoot(20, 3, 15, angleOffset: 20 / 3, projectileIndex: 1, coolDown: 2000)
                    )
                ),
                new Threshold(0.01,
                    new TierLoot(6, ItemType.Weapon, 0.2),
                    new TierLoot(7, ItemType.Weapon, 0.1),
                    new TierLoot(7, ItemType.Armor, 0.2),
                    new TierLoot(8, ItemType.Armor, 0.1),
                    new TierLoot(3, ItemType.Ability, 0.2),
                    new TierLoot(4, ItemType.Ability, 0.1),
                    new TierLoot(3, ItemType.Ring, 0.2),
                    new TierLoot(4, ItemType.Ring, 0.1),
                    new ItemLoot("Potion of Life", 0.08),
                    new ItemLoot("Potion of Mana", 0.08)
                )
            )
        .Init("Candyland Boss Spawner",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Waiting",
                    new TimedTransition(10000, "Check")
                    ),
                new State("Check",
                    new NoPlayerWithinTransition(20, "Main")
                    ),
                new State("Main",
                    new TimedTransition(1000, "Pony Check 1", randomized: true),
                    new TimedTransition(1000, "Desire Troll Check 1", randomized: true),
                    new TimedTransition(1000, "Spoiled Creampuff Check 1", randomized: true),
                    new TimedTransition(1000, "MegaRototo Check 1", randomized: true),
                    new TimedTransition(1000, "Swoll Fairy Check 1", randomized: true)
                    ),
                new State("Pony Check 1",
                    new EntitiesNotExistsTransition(20, "Pony Check 2", "Gigacorn", "MegaRototo", "Spoiled Creampuff", "Desire Troll", "Swoll Fairy")
                    ),
                new State("Pony Check 2",
                    new NoPlayerWithinTransition(20, "Pony")
                    ),
                new State("Pony",
                    new Spawn("Gigacorn", maxChildren: 1),
                    new EntitiesNotExistsTransition(20, "Waiting", "Gigacorn", "MegaRototo", "Spoiled Creampuff", "Desire Troll", "Swoll Fairy")
                    ),
                new State("Desire Troll Check 1",
                    new EntitiesNotExistsTransition(20, "Desire Troll Check 2", "Gigacorn", "MegaRototo", "Spoiled Creampuff", "Desire Troll", "Swoll Fairy")
                    ),
                new State("Desire Troll Check 2",
                    new NoPlayerWithinTransition(20, "Desire Troll")
                    ),
                new State("Desire Troll",
                    new Spawn("Desire Troll", maxChildren: 1),
                    new EntitiesNotExistsTransition(20, "Waiting", "Gigacorn", "MegaRototo", "Spoiled Creampuff", "Desire Troll", "Swoll Fairy")
                    ),
                new State("Spoiled Creampuff Check 1",
                    new EntitiesNotExistsTransition(20, "Spoiled Creampuff Check 2", "Gigacorn", "MegaRototo", "Spoiled Creampuff", "Desire Troll", "Swoll Fairy")
                    ),
                new State("Spoiled Creampuff Check 2",
                    new NoPlayerWithinTransition(20, "Spoiled Creampuff")
                    ),
                new State("Spoiled Creampuff",
                    new Spawn("Spoiled Creampuff", maxChildren: 1),
                    new EntitiesNotExistsTransition(20, "Waiting", "Gigacorn", "MegaRototo", "Spoiled Creampuff", "Desire Troll", "Swoll Fairy")
                    ),
                new State("MegaRototo Check 1",
                    new EntitiesNotExistsTransition(20, "MegaRototo Check 2", "Gigacorn", "MegaRototo", "Spoiled Creampuff", "Desire Troll", "Swoll Fairy")
                    ),
                new State("MegaRototo Check 2",
                    new NoPlayerWithinTransition(20, "MegaRototo")
                    ),
                new State("MegaRototo",
                    new Spawn("MegaRototo", maxChildren: 1),
                    new EntitiesNotExistsTransition(20, "Waiting", "Gigacorn", "MegaRototo", "Spoiled Creampuff", "Desire Troll", "Swoll Fairy")
                    ),
                new State("Swoll Fairy Check 1",
                    new EntitiesNotExistsTransition(20, "Swoll Fairy Check 2", "Gigacorn", "MegaRototo", "Spoiled Creampuff", "Desire Troll", "Swoll Fairy")
                    ),
                new State("Swoll Fairy Check 2",
                    new NoPlayerWithinTransition(20, "Swoll Fairy")
                    ),
                new State("Swoll Fairy",
                    new Spawn("Swoll Fairy", maxChildren: 1),
                    new EntitiesNotExistsTransition(20, "Waiting", "Gigacorn", "MegaRototo", "Spoiled Creampuff", "Desire Troll", "Swoll Fairy")
                    )
                )
            )
        .Init("Gumball Machine",
            new State(),
            new Threshold(0.01,
                new ItemLoot("Yellow Gumball", 0.1),
                new ItemLoot("Green Gumball", 0.1),
                new ItemLoot("Blue Gumball", 0.1),
                new ItemLoot("Red Gumball", 0.1),
                new ItemLoot("Purple Gumball", 0.1)
                )
            )
        .Init("Cupcake",
            new State(
                new Shoot(10, 8, shootAngle: 45, projectileIndex: 0, fixedAngle: 0, coolDown: 1000)
                )
            )
        .Init("Unicorn",
            new State(
                new Wander(0.5),
                new Charge(2.0, 10f, 4000),
                new Shoot(20, 1, 40, angleOffset: 60 / 3, projectileIndex: 0, coolDown: 2100),
                new Shoot(20, 1, 40, angleOffset: 60 / 3, projectileIndex: 0, coolDown: 2200),
                new Shoot(20, 1, 40, angleOffset: 60 / 3, projectileIndex: 0, coolDown: 2300),
                new Shoot(20, 1, 40, angleOffset: 60 / 3, projectileIndex: 0, coolDown: 2400),
                new Shoot(20, 3, 15, angleOffset: 40 / 3, projectileIndex: 1, coolDown: 4000),
                new Shoot(20, 3, 15, angleOffset: 20 / 3, projectileIndex: 1, coolDown: 2000)
                )
            )
        .Init("Hard Candy",
            new State(
                new Wander(0.7),
                new Shoot(10, 3, 5, 0, coolDown: 600)
                )
            )
        .Init("Beefy Fairy",
            new State(
                new Spawn("Fairy", maxChildren: 3),
                new State("1",
                    new Wander(0.3),
                    new Shoot(10, 2, shootAngle: 30, projectileIndex: 0, coolDown: 400)
                    )
                )
            )
        .Init("Fairy",
            new State(
                new Wander(0.3),
                new Shoot(10, 2, shootAngle: 30, projectileIndex: 0, coolDown: 500)
                )
            )
        .Init("Wishing Troll",
            new State(
                new Wander(0.5),
                new Grenade(6, 70, range: 8, coolDown: 3000),
                new Shoot(15, 3, 5, angleOffset: 30 / 3, projectileIndex: 0, coolDown: 2100)
                )
            )
        .Init("Spilled Icecream",
            new State(
                new Wander(0.7),
                new Shoot(10, 3, 10, 0, coolDown: 600)
                )
            )
        .Init("Big Creampuff",
            new State(
                new Wander(0.5),
                new Shoot(20, 1, 0, angleOffset: 40 / 3, projectileIndex: 0, coolDown: 1000),
                new Spawn("Small Creampuff", maxChildren: 2, initialSpawn: 0.5, coolDown: 5000)
                )
            )
        .Init("Small Creampuff",
            new State(
                new Wander(.5),
                new Shoot(20, 3, 30, angleOffset: 40 / 3, projectileIndex: 1, coolDown: 1400)
                )
            )
        .Init("Tiny Rototo",
            new State(
                new Wander(0.5),
                new Shoot(10, 4, shootAngle: 90, projectileIndex: 0, coolDownOffset: 500),
                new Shoot(10, 4, shootAngle: 90, projectileIndex: 1, angleOffset: 45, coolDownOffset: 1000)
                )
            )
        .Init("Rototo",
            new State(
                new Spawn("Tiny Rototo", maxChildren: 5),
                new Wander(0.5),
                new Shoot(10, 4, shootAngle: 90, projectileIndex: 0, coolDownOffset: 500),
                new Shoot(10, 4, shootAngle: 90, projectileIndex: 1, angleOffset: 45, coolDownOffset: 1000)
                )
            )
        .Init("Candyland Spawner",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("1",
                    new NoPlayerWithinTransition(20, "2")
                    ),
                new State("2",
                    new TimedTransition(1000, "Unicorn", randomized: true),
                    new TimedTransition(1000, "Big Creampuff", randomized: true),
                    new TimedTransition(1000, "Rototo", randomized: true),
                    new TimedTransition(1000, "Gumball Machine", randomized: true),
                    new TimedTransition(1000, "Spilled Icecream", randomized: true),
                    new TimedTransition(1000, "Wishing Troll", randomized: true),
                    new TimedTransition(1000, "Beefy Fairy", randomized: true),
                    new TimedTransition(1000, "Hard Candy", randomized: true),
                    new TimedTransition(1000, "Tiny Rototo", randomized: true),
                    new TimedTransition(1000, "Small Creampuff", randomized: true),
                    new TimedTransition(1000, "Fairy", randomized: true),
                    new TimedTransition(1000, "Cupcake", randomized: true)
                    ),
                new State("Unicorn",
                    new Spawn("Unicorn", maxChildren: 1),
                    new TimedTransition(100, "Monster Check")
                    ),
                new State("Cupcake",
                    new Spawn("Cupcake", maxChildren: 1),
                    new TimedTransition(100, "Monster Check")
                    ),
                new State("Big Creampuff",
                    new Spawn("Big Creampuff", maxChildren: 1),
                    new TimedTransition(100, "Monster Check")
                    ),
                new State("Rototo",
                    new Spawn("Rototo", maxChildren: 1),
                    new TimedTransition(100, "Monster Check")
                    ),
                new State("Gumball Machine",
                    new Spawn("Gumball Machine", maxChildren: 1),
                    new TimedTransition(100, "Monster Check")
                    ),
                new State("Spilled Icecream",
                    new Spawn("Spilled Icecream", maxChildren: 4),
                    new TimedTransition(100, "Monster Check")
                    ),
                new State("Wishing Troll",
                    new Spawn("Wishing Troll", maxChildren: 1),
                    new TimedTransition(100, "Monster Check")
                    ),
                new State("Beefy Fairy",
                    new Spawn("Beefy Fairy", maxChildren: 1),
                    new TimedTransition(100, "Monster Check")
                    ),
                new State("Hard Candy",
                    new Spawn("Hard Candy", maxChildren: 4),
                    new TimedTransition(100, "Monster Check")
                    ),
                new State("Tiny Rototo",
                    new Spawn("Tiny Rototo", maxChildren: 4),
                    new TimedTransition(100, "Monster Check")
                    ),
                new State("Small Creampuff",
                    new Spawn("Small Creampuff", maxChildren: 4),
                    new TimedTransition(100, "Monster Check")
                    ),
                new State("Fairy",
                    new Spawn("Fairy", maxChildren: 4),
                    new TimedTransition(100, "Monster Check")
                    ),
                new State("Monster Check",
                    new EntitiesNotExistsTransition(20, "Check", "Rototo", "Tiny Rototo", "Small Creampuff", "Big Creampuff", "Spilled Icecream", "Wishing Troll", "Fairy", "Beefy Fairy", "Hard Candy", "Unicorn", "Gumball Machine", "Cupcake")
                    ),
                new State("Check",
                    new NoPlayerWithinTransition(20, "2")
                    )
                )
            )
        .Init("Butterfly",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new Wander(0.3),
                new StayCloseToSpawn(0.5, 2)
                )
            )

        .Init("MegaRototo Statue",
            new State(
            new ConditionalEffect(ConditionEffectIndex.Invincible),
            new ConditionalEffect(ConditionEffectIndex.Petrify),
            new TransformOnDeath("MegaRototo"),
            new State("Idle"
                ),
            new State("Die",
                new Suicide()
                )
                )
            )

        .Init("Spoiled Creampuff Statue",
            new State(
            new ConditionalEffect(ConditionEffectIndex.Invincible),
            new ConditionalEffect(ConditionEffectIndex.Petrify),
            new TransformOnDeath("Spoiled Creampuff"),
            new State("Idle"
                ),
            new State("Die",
                new Suicide()
                )
                )
            )

        .Init("C.A.N.D.Y Defense System",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("ALARM TRIPPED",
                        new PlayerWithinTransition(10, "Inform Players")
                        ),
                    new State("Inform Players",
                        new Taunt("Enemies have arrived through our Caverns, Ready yourselves."),
                        new TimedTransition(500, "Contestant Ready?")
                        ),
                    new State("Contestant Ready?",
                        new Taunt("Ready?"),
                        new PlayerTextTransition("Before Statue Activation", "Ready")
                        ),
                    new State("Before Statue Activation",
                        new OrderOnce(999, "Spoiled Creampuff Statue", "Die"),
                        new OrderOnce(999, "MegaRototo Statue", "Die"),
                        new TimedTransition(3000, "Check Death")
                        ),
                    new State("Check Death",
                        new EntitiesNotExistsTransition(5, "Waiting", "Spoiled Creampuff", "MegaRototo")
                        ),
                    new State("Waiting",
                        new Taunt("Ready?"),
                        new PlayerTextTransition("Rip", "Ready")
                    ),
                    new State("Rip",
                    new OrderOnce(999, "cland Check 1", "Suicide"),
                    new TimedTransition(2000, "Check Death2")
                    ),
                    new State("Check Death2",
                        new EntitiesNotExistsTransition(10, "Idle2", "Spoiled Creampuff", "MegaRototo", "Desire Troll", "Gigacorn")
                        ),
                    new State("Idle2",
                        new Taunt("This is your last warning, Are you Ready?"),
                        new PlayerTextTransition("Rip2", "Ready")
                    ),
                    new State("Rip2",
                    new OrderOnce(999, "cland Check 2", "Suicide")
                    )
                )
            )

        .Init("cland Check 1",
            new State(
            new ConditionalEffect(ConditionEffectIndex.Invincible),
            new RemoveObjectOnDeath("Candy Choc Column Whole", 999),
            new State("Idle"
                ),
            new State("Suicide",
                new TimedTransition(500, "Death")
                ),
            new State("Death",
                new Suicide()
                )
                )
            )

        .Init("cland Check 2",
            new State(
            new ConditionalEffect(ConditionEffectIndex.Invincible),
            new RemoveObjectOnDeath("Candy Straw Column Whole", 999),
            new State("Idle"
                ),
            new State("Suicide",
                new TimedTransition(500, "Death")
                ),
            new State("Death",
                new Suicide()
                )
                )
            );
    }
}