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

        public static IState CreateAttackState(IEntity entity, float stoppingDistance)
        {
            return new BaseState(
                onUpdate: _ =>
                {
                    IEntity target = entity.GetTarget().Value;
                    LookTargetUseCase.LookAt(entity, target);
                    FollowTargetUseCase.Follow(entity, target, stoppingDistance);
                    AttackTargetUseCase.Attack(entity, target, stoppingDistance);
                });
        }
    }
}