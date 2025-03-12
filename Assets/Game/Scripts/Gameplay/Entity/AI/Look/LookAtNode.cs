using Atomic.Entities;
using Modules.BehaviourTree;

namespace Game.Gameplay
{
    public class LookAtNode : BehaviourNode
    {
        private readonly IEntity _entity;

        public LookAtNode(IEntity entity)
        {
            _entity = entity;
        }

        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            var target = _entity.GetTarget().Value;
            if (target == null)
                return BehaviourResult.FAILURE;

            bool isLooking = LookAtUseCase.LookAt(_entity, target);

            if (isLooking)
                return BehaviourResult.SUCCESS;

            return BehaviourResult.RUNNING;
        }
    }
}