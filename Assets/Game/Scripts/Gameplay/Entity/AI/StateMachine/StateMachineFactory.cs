using Atomic.Entities;
using Modules.BehaviourTree;
using Modules.FSM;
using IState = Modules.FSM.IState;

namespace Game.Gameplay
{
    public static class StateMachineFactory
    {
        public static IState CreateIdleState(IEntity entity)
        {
            return new BaseState(
                onUpdate: _ => { });
        }

        public static IState CreateMoveState(IEntity entity, float stoppingDistance)
        {
            return new BaseState(
                onUpdate: _ => { MoveToUseCase.Move(entity, stoppingDistance); });
        }

        public static IState CreateHoldState(IEntity entity, float attackingDistance)
        {
            return new BaseState(
                onUpdate: _ =>
                {
                    IEntity target = entity.GetTarget().Value;
                    LookAtUseCase.LookAt(entity, target);
                    AttackTargetUseCase.Attack(entity, target, attackingDistance);
                });
        }

        public static IState CreatePatrolState(IEntity entity, float stoppingDistance)
        {
            return new BaseState(
                onUpdate: _ => { PatrolUseCase.PatrolWaypoints(entity, stoppingDistance); });
        }

        public static IState CreateBehaviourTreeState(IBehaviourNode behaviourTree)
        {
            return new BaseState(
                onUpdate: deltaTime => behaviourTree.Run(deltaTime),
                onExit: behaviourTree.Abort
            );
        }
    }
}