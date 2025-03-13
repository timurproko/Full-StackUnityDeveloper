using Atomic.Entities;
using Modules.BehaviourTree;

namespace Game.Gameplay
{
    public static class BehaviourTreeFactory
    {
        public static BehaviourNodeSequence CreateAttackSequence(IEntity entity, float stoppingDistance,
            float attackingDistance)
        {
            return new BehaviourNodeSequence(
                new MoveToPointNode(entity, stoppingDistance),
                new LookAtNode(entity),
                new AttackNode(entity, attackingDistance)
            );
        }

        public static IBehaviourNode CreateFollowSequence(IEntity entity, float stoppingDistance)
        {
            return new BehaviourNodeSequence(
                new MoveToPointNode(entity, stoppingDistance),
                new LookAtNode(entity),
                new FollowNode(entity, stoppingDistance)
            );
        }
        
        public static IBehaviourNode CreateDefaultSequence(IEntity entity, float stoppingDistance, float attackingDistance)
        {
            return new BehaviourNodeSelector(
                new BehaviourNodeAborter(
                    new BehaviourNodeSequence(
                        new BehaviourNodeCondition(() => entity.GetTarget().Value == null),
                        new MoveToBaseNode(entity, stoppingDistance)
                    ),
                    () => entity.GetTarget().Value != null 
                ),
                new HoldNode(entity, stoppingDistance, attackingDistance)
            );
        }
    }
}