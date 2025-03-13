using Atomic.Entities;
using Modules.BehaviourTree;
using UnityEngine;

namespace Game.Gameplay
{
    public class MoveToBaseNode : BehaviourNode
    {
        private readonly IEntity _entity;
        private readonly float _stoppingDistance;
        private readonly Vector3 initialPoint;

        public MoveToBaseNode(IEntity entity, float stoppingDistance)
        {
            _entity = entity;
            _stoppingDistance = stoppingDistance;
            initialPoint = _entity.GetTransform().position;
        }

        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            var target = _entity.GetTarget().Value;

            if (target != null)
                return BehaviourResult.FAILURE;

            if (target == null)
            {
                MoveToUseCase.MoveToBase(_entity, _stoppingDistance, initialPoint);
                return BehaviourResult.RUNNING;
            }

            return BehaviourResult.SUCCESS;
        }
    }
}