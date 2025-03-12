using Atomic.Entities;
using Modules.BehaviourTree;

namespace Game.Gameplay
{
    public class FollowNode : BehaviourNode
    {
        private readonly IEntity _entity;
        private readonly float _stoppingDistance;

        public FollowNode(IEntity entity, float stoppingDistance)
        {
            _entity = entity;
            _stoppingDistance = stoppingDistance;
        }

        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            var target = _entity.GetTarget().Value;
            if (target == null)
                return BehaviourResult.FAILURE;

            bool isFollowing = FollowUseCase.Follow(_entity, target, _stoppingDistance);

            return isFollowing ? BehaviourResult.RUNNING : BehaviourResult.FAILURE;
        }
    }
}