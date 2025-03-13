using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public static class MoveToUseCase
    {
        public static bool Move(in IEntity entity, float stoppingDistance)
        {
            return Move(entity, stoppingDistance, out _);
        }

        public static bool Move(in IEntity entity, float stoppingDistance, out bool reached)
        {
            return MoveToPosition(entity, stoppingDistance, entity.GetMovePoint().Value, out reached);
        }

        public static bool MoveToBase(in IEntity entity, float stoppingDistance, Vector3 targetPosition)
        {
            return MoveToPosition(entity, stoppingDistance, targetPosition, out _);
        }

        public static bool MoveToPosition(in IEntity entity, float stoppingDistance, Vector3 targetPosition, out bool reached)
        {
            if (entity == null)
            {
                reached = false;
                return false;
            }

            Vector3 currentPosition = entity.GetTransform().position;
            currentPosition.y = 0;
            targetPosition.y = 0;

            Vector3 distanceVector = targetPosition - currentPosition;

            reached = distanceVector.sqrMagnitude <= stoppingDistance * stoppingDistance;
            if (!reached)
            {
                Vector3 moveDirection = distanceVector.normalized;
                entity.GetMoveRequest().Invoke(moveDirection);
            }

            return true;
        }
    }
}