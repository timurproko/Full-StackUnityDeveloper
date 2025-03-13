using Atomic.Entities;
using Modules.BehaviourTree;

namespace Game.Gameplay
{
    public static class BehaviourTreeFactory
    {
        public static IBehaviourNode CreateDefaultSequence(IEntity entity, float stoppingDistance,
            float attackingDistance)
        {
            return new BehaviourNodeSelector(
                new BehaviourNodeAborter(
                    new BehaviourNodeSequence(
                        new BehaviourNodeCondition(() => entity.GetTarget().Value == null),
                        new MoveToBaseNode(entity, 0.1f)
                    ),
                    () => entity.GetTarget().Value != null
                ),
                new DefaultBehaviourNode(entity, stoppingDistance, attackingDistance)
            );
        }

        public static IBehaviourNode CreateAttackSequence(IEntity entity, float stoppingDistance,
            float attackingDistance)
        {
            return new BehaviourNodeSequence(
                new MoveToPointNode(entity, stoppingDistance),
                new BehaviourNodeParallel(
                    new DefaultBehaviourNode(entity, stoppingDistance, attackingDistance)
                )
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
    }
}