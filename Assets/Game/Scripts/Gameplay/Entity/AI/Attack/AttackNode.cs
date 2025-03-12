using Atomic.Entities;
using Modules.BehaviourTree;

namespace Game.Gameplay
{
    public class AttackNode : BehaviourNode
    {
        private readonly IEntity _entity;
        private readonly float _attackingDistance;

        public AttackNode(IEntity entity, float attackingDistance)
        {
            _entity = entity;
            _attackingDistance = attackingDistance;
        }

        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            var target = _entity.GetTarget().Value;
            if (target == null)
                return BehaviourResult.FAILURE;

            bool isAttacking = AttackTargetUseCase.Attack(_entity, target, _attackingDistance);
            return isAttacking ? BehaviourResult.RUNNING : BehaviourResult.FAILURE;
        }
    }
}