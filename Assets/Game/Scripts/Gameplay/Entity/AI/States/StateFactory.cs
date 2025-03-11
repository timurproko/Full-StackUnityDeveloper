using Atomic.Entities;
using Modules.FSM;

namespace Game.Gameplay
{
    public static class StateFactory
    {
        public static IState CreatePatrolState(IEntity entity, float stoppingDistance)
        {
            return new BaseState(
                onUpdate: _ =>
                {
                    PatrolUseCase.PatrolWaypoints(entity, stoppingDistance);
                });
        }

        public static IState CreateAttackState(IEntity entity, float stoppingDistance, float attackingDistance)
        {
            return new BaseState(
                onUpdate: _ =>
                {
                    IEntity target = entity.GetTarget().Value;
                    LookTargetUseCase.LookAt(entity, target);
                    AttackTargetUseCase.Attack(entity, target, attackingDistance);
                    FollowTargetUseCase.Follow(entity, target, stoppingDistance);
                });
        }
        
        public static IState CreateHoldState(IEntity entity, float attackingDistance)
        {
            return new BaseState(
                onUpdate: _ =>
                {
                    IEntity target = entity.GetTarget().Value;
                    LookTargetUseCase.LookAt(entity, target);
                    AttackTargetUseCase.Attack(entity, target, attackingDistance);
                });
        }
        
        public static IState CreateChaseState(IEntity entity, float stoppingDistance)
        {
            return new BaseState(
                onUpdate: _ =>
                {
                    IEntity target = entity.GetTarget().Value;
                    FollowTargetUseCase.Follow(entity, target, stoppingDistance);
                });
        }
    }
}