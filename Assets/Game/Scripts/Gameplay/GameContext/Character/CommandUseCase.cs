using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public static class CommandUseCase
    {
        public static void Stop(IEntity entity)
        {
            entity.GetStateMachine().ChangeState(StateName.Stop, () => PatrolUseCase.ClearWaypoints(entity));
            Debug.Log("Stop");
        }

        public static void Hold(IEntity entity, bool additive)
        {
            entity.GetStateMachine().ChangeState(StateName.Hold);
            Debug.Log("Hold");
        }

        public static void Move(IEntity entity, RaycastHit hitPoint, bool additive)
        {
            entity.GetMovePosition().Value = hitPoint.point;
            entity.GetStateMachine().ChangeState(StateName.Move);
            Debug.Log("Move");
        }

        public static void Patrol(IEntity entity, RaycastHit hitPoint, bool additive)
        {
            entity.GetStateMachine()
                .ChangeState(StateName.Patrol, () => PatrolUseCase.AddWaypoints(entity, hitPoint.point));
            Debug.Log("Patrol");
        }

        public static void Attack(IEntity entity, RaycastHit hitPoint, bool additive)
        {
            entity.GetMovePosition().Value = hitPoint.point;
            entity.GetStateMachine().ChangeState(StateName.Attack);
            Debug.Log("Attack");
        }

        public static void Follow(IEntity entity, RaycastHit hitPoint, bool additive)
        {
            Debug.Log("Follow");
            entity.GetMovePosition().Value = hitPoint.point;
            entity.GetStateMachine().ChangeState(StateName.Follow);
        }
    }
}