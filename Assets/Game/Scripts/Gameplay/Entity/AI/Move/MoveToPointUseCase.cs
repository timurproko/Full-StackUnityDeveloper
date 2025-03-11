using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public static class MoveToPointUseCase
    {
        private const float STOPPING_DISTANCE = 0.1f;

        public static bool Move(in IEntity entity)
        {
            if (entity == null)
                return false;

            var targetPosition = entity.GetMovePoint().Value;
            targetPosition.y = 0;
            
            Vector3 currentPosition = entity.GetTransform().position;
            currentPosition.y = 0;

            Vector3 distanceVector = targetPosition - currentPosition;

            Vector3 moveDirection = distanceVector.sqrMagnitude <= STOPPING_DISTANCE
                ? Vector3.zero
                : distanceVector.normalized;

            entity.GetMoveRequest().Invoke(moveDirection);
            return true;
        }
    }
}