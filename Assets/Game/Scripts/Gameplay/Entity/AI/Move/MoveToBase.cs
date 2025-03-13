using Atomic.Entities;
using Modules.BehaviourTree;
using UnityEngine;

namespace Game.Gameplay
{
    public class MoveToBaseNode : BehaviourNode
    {
        private readonly IEntity _entity;
        private readonly float _stoppingDistance;

        public MoveToBaseNode(IEntity entity, float stoppingDistance)
        {
            _entity = entity;
            _stoppingDistance = stoppingDistance;
        }

        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            var target = _entity.GetTarget().Value;

            if (target != null)
                return BehaviourResult.FAILURE;

            if (target == null)
            {
                var initialPoint = _entity.GetMovePosition().Value;
                MoveToUseCase.MoveToBase(_entity, _stoppingDistance, initialPoint);
                return BehaviourResult.RUNNING;
            }

            return BehaviourResult.SUCCESS;
        }
    }
}