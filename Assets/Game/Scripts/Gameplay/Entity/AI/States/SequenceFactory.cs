using Atomic.Entities;
using Modules.BehaviourTree;

namespace Game.Gameplay
{
    public static class SequenceFactory
    {
        public static BehaviourNodeSequence CreateAttackSequence(IEntity entity, float stoppingDistance, float attackingDistance)
        {
            return new BehaviourNodeSequence(
                new MoveNode(entity, 0.1f),
                new AttackNode(entity, attackingDistance)
            );
        }
    }
}