using Atomic.Entities;
using Modules.BehaviourTree;
using UnityEngine;

namespace Game.Gameplay
{
    public class HoldNode : BehaviourNode
    {
        private readonly IEntity _entity;
        private readonly float _stoppingDistance;
        private readonly float _attackingDistance;
        private readonly Vector3 initialPoint;

        public HoldNode(IEntity entity, float stoppingDistance, float attackingDistance)
        {
            _entity = entity;
            _stoppingDistance = stoppingDistance;
            _attackingDistance = attackingDistance;
        }

        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            var target = _entity.GetTarget().Value;

            if (target == null)
                return BehaviourResult.FAILURE;


            if (target != null)
            {
                LookAtUseCase.LookAt(_entity, target);
                FollowUseCase.Follow(_entity, target, _stoppingDistance);
                AttackTargetUseCase.Attack(_entity, target, _attackingDistance);
                return BehaviourResult.RUNNING;
            }

            return BehaviourResult.SUCCESS;
        }
    }
}