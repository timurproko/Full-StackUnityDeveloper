using Atomic.Entities;
using Modules.BehaviourTree;

namespace Game.Gameplay
{
    public static class SequenceFactory
    {
        public static BehaviourNodeSequence CreateAttackSequence(IEntity entity, float stoppingDistance, float attackingDistance)
        {
            return new BehaviourNodeSequence(
                new MoveToNode(entity, stoppingDistance),
                new LookAtNode(entity),
                new AttackNode(entity, attackingDistance)
            );
        }
        
        public static BehaviourNodeSequence CreateFollowSequence(IEntity entity, float stoppingDistance)
        {
            return new BehaviourNodeSequence(
                new MoveToNode(entity, stoppingDistance),
                new LookAtNode(entity),
                new FollowNode(entity, stoppingDistance)
            );
        }
    }
}