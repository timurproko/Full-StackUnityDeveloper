using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public static class CommandUseCase
    {
        public static void Stop(IEntity entity)
        {
            entity.GetStateMachine().ChangeState(StateName.Idle);
            PatrolUseCase.ClearWaypoints(entity);
        }

        public static void Hold(IEntity entity, bool additive)
        {
            entity.GetStateMachine().ChangeState(StateName.Hold);
        }

        public static void Move(IEntity entity, RaycastHit hitPoint, bool additive)
        {
            var target = hitPoint.collider.GetComponentInParent<IEntity>();
            if (target != null)
            {
                entity.GetMovePoint().Value = target.GetGameObject().transform.position;
                entity.GetStateMachine().ChangeState(StateName.Move);
            }
            else
            {
                entity.GetStateMachine().ChangeState(StateName.Idle);
            }

            entity.GetMovePoint().Value = hitPoint.point;
            entity.GetStateMachine().ChangeState(StateName.Move);
        }

        public static void Patrol(IEntity entity, RaycastHit hitPoint, bool additive)
        {
            var target = hitPoint.collider.GetComponentInParent<IEntity>();
            if (target != null)
            {
                var entityPos = target.GetGameObject().transform.position;
                PatrolUseCase.AddWaypoints(entity, entityPos);
            }
            else
            {
                entity.GetStateMachine().ChangeState(StateName.Idle);
            }
            
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