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
                new MoveToNode(entity, stoppingDistance),
                new LookAtNode(entity),
                new AttackNode(entity, attackingDistance)
            );
        }

        public static IBehaviourNode CreateFollowSequence(IEntity entity, float stoppingDistance)
        {
            return new BehaviourNodeSequence(
                new MoveToNode(entity, stoppingDistance),
                new LookAtNode(entity),
                new FollowNode(entity, stoppingDistance)
            );
        }

        public static IBehaviourNode CreatePatrolSequence(IEntity entity, float stoppingDistance, float attackingDistance)
        {
            return new BehaviourNodeSelector(
                new BehaviourNodeAborter( // Abort patrol if enemy exists
                    new BehaviourNodeSequence(
                        new BehaviourNodeCondition(() => entity.GetTarget().Value == null),
                        new PatrolNode(entity, stoppingDistance)
                    ),
                    () => entity.GetTarget().Value != null
                ),
                new BehaviourNodeAborter( // Abort attack if not looking at target
                    new BehaviourNodeSequence(
                        new LookAtNode(entity),
                        new BehaviourNodeCondition(() => LookAtUseCase.LookAt(entity, entity.GetTarget().Value)),
                        new AttackNode(entity, attackingDistance)
                    ),
                    () => !LookAtUseCase.LookAt(entity, entity.GetTarget().Value)
                )
            );
        }
    }
}