using Atomic.Entities;
using Modules.BehaviourTree;

namespace Game.Gameplay
{
    public class MoveToPointNode : BehaviourNode
    {
        private readonly IEntity _entity;
        private readonly float _stoppingDistance;

        public MoveToPointNode(IEntity entity, float stoppingDistance)
        {
            _entity = entity;
            _stoppingDistance = stoppingDistance;
        }

        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            return !MoveToUseCase.Move(_entity, _stoppingDistance, out bool reached)
                ? BehaviourResult.FAILURE
                : reached
                    ? BehaviourResult.SUCCESS
                    : BehaviourResult.RUNNING;
        }
    }
}