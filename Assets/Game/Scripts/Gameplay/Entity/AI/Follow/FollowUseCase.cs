using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class FollowUseCase
    {
        public static bool Follow(in IEntity entity, in IEntity target, in float stoppingDistance)
        {
            if (entity == null || target == null)
                return false;
            
            Vector3 distanceVector = VectorUseCase.DistanceVector(entity, target);
            distanceVector.y = 0;
             
            Vector3 moveDirection = distanceVector.sqrMagnitude <= stoppingDistance * stoppingDistance
                ? Vector3.zero
                : distanceVector.normalized;

            entity.GetMoveRequest().Invoke(moveDirection);
            return true;
        }
    }
}