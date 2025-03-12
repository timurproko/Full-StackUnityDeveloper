using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public static class CommandUseCase
    {
        public static void Stop(IEntity entity)
        {
            PatrolUseCase.ClearWaypoints(entity);
            entity.GetStateMachine().ChangeState(StateName.Idle);
        }

        public static void Hold(IEntity entity, bool additive)
        {
            entity.GetStateMachine().ChangeState(StateName.Hold);
        }

        public static void Move(IEntity entity, RaycastHit hitPoint, bool additive)
        {
            entity.GetMovePoint().Value = hitPoint.point;
            entity.GetStateMachine().ChangeState(StateName.Move);
        }

        public static void Patrol(IEntity entity, RaycastHit hitPoint, bool additive)
        {
            PatrolUseCase.AddWaypoints(entity, hitPoint.point);
            entity.GetStateMachine().ChangeState(StateName.Patrol);
        }

        public static void Attack(IEntity entity, RaycastHit hitPoint, bool additive)
        {
            entity.GetMovePoint().Value = hitPoint.point;
            entity.GetStateMachine().ChangeState(StateName.Attack);
        }

        public static void Follow(IEntity entity, RaycastHit hitPoint, bool additive)
        {
            entity.GetMovePoint().Value = hitPoint.point;
            entity.GetStateMachine().ChangeState(StateName.Follow);
        }
    }
}