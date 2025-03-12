using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public static class LookAtUseCase
    {
        public static bool LookAt(in IEntity entity, in IEntity target)
        {
            if (entity == null || target == null)
                return false;

            Vector3 distanceVector = VectorUseCase.DistanceVector(entity, target);
            distanceVector.y = 0;

            entity.GetLookRequest().Invoke(distanceVector);

            Vector3 directionToTarget = distanceVector.normalized;
            float dotProduct = Vector3.Dot(entity.GetTransform().forward, directionToTarget);

            return dotProduct > 0.99f;
        }
    }
}