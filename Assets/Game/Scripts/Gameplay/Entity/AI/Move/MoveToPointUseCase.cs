using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public static class MoveToPointUseCase
    {
        public static bool Move(in IEntity entity, float stoppingDistance)
        {
            if (entity == null)
                return false;

            var targetPosition = entity.GetMovePoint().Value;
            targetPosition.y = 0;
            
            Vector3 currentPosition = entity.GetTransform().position;
            currentPosition.y = 0;

            Vector3 distanceVector = targetPosition - currentPosition;

            Vector3 moveDirection = distanceVector.sqrMagnitude <= stoppingDistance * stoppingDistance
                ? Vector3.zero
                : distanceVector.normalized;

            entity.GetMoveRequest().Invoke(moveDirection);
            return true;
        }
    }
}