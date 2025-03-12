using Atomic.Entities;
using Modules.BehaviourTree;

namespace Game.Gameplay
{
    public class PatrolNode : BehaviourNode
    {
        private readonly IEntity _entity;
        private readonly float _stoppingDistance;

        public PatrolNode(IEntity entity, float stoppingDistance)
        {
            _entity = entity;
            _stoppingDistance = stoppingDistance;
        }

        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            return !PatrolUseCase.PatrolWaypoints(_entity, _stoppingDistance, out bool reached)
                ? BehaviourResult.FAILURE
                : reached
                    ? BehaviourResult.SUCCESS
                    : BehaviourResult.RUNNING;
        }
    }
}