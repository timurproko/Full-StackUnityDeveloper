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
            if (entity == null)
            {
                reached = false;
                return false;
            }

            var targetPosition = entity.GetMovePoint().Value;
            targetPosition.y = 0;

            Vector3 currentPosition = entity.GetTransform().position;
            currentPosition.y = 0;

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