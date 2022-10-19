using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Ally = () => Behav()
             .Init("Zombie T6",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true, -1),
                    new Decay(10000),
                    new NoEntityWithinTransition(10, "FollowParent", true),
                    new State("FollowParent",
                        new PetFollow(2, 10),
                        new AnyEntityWithinTransition(7, "Charge", true)),
                    new State("Charge", 
                        new Zap(500, 100, 2f, true)),
                        new AllyCharge(5, 8),
                        new NoEntityWithinTransition(7, "FollowParent", true)
                    )
            )
        
        
        
            .Init("Splosive Decoy",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true, -1),
                    new AllyShoot(45, 8, null, 0, null, null, 0, null, 0, 200, false)                    


            ))
            
            
        ;

            
    }
}