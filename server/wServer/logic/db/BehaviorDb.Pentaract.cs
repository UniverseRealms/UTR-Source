using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Pentaract = () => Behav()
            .Init("Pentaract Eye",
                new State(
                    new Prioritize(
                        new Swirl(2, 8, 20, true),
                        new Protect(2, "Pentaract Tower", 20, 6, 4)
                        ),
                    new Shoot(9, 1, coolDown: 1000)
                    )
            )
            .Init("Pentaract Tower",
                new State(
                    new Spawn("Pentaract Eye", 5, coolDown: 5000, givesNoXp: false),
                    new Grenade(4, 100, 8, coolDown: 5000),
                    new TransformOnDeath("Pentaract Tower Corpse"),
                    new TransferDamageOnDeath("Pentaract"),
                    // needed to avoid crash, Oryx.cs needs player name otherwise hangs server (will patch that later)
                    new TransferDamageOnDeath("Pentaract Tower Corpse")
                    )
            )
            .Init("Pentaract",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("Waiting",
                        new EntityNotExistsTransition("Pentaract Tower", 50, "Die")
                        ),
                    new State("Die",
                        new Suicide()
                        )
                    )
            )
            .Init("Pentaract Tower Corpse",
                new State(
                    new DropPortalOnDeath("The Unspeakable Portal", 0.33),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("Waiting",
                        new TimedTransition(15000, "Spawn"),
                        new EntityNotExistsTransition("Pentaract Tower", 50, "Die")
                        ),
                    new State("Spawn",
                        new Transform("Pentaract Tower")
                        ),
                    new State("Die",
                        new Suicide()
                        )
                    ),
                new Threshold(0.01,
                    new ItemLoot("Possessed Ring", 0.005),
                    new ItemLoot("Ritual Seal", 0.0034),
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
            ;
    }
}