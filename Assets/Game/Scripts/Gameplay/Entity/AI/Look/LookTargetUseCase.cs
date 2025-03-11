using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public static class LookTargetUseCase
    {
        public static bool LookAt(in IEntity entity, in IEntity target)
        {
            if (entity == null || target == null)
                return false;
            
            Vector2 distanceVector = VectorUseCase.DistanceVector(entity, target);
            distanceVector.y = 0;
            entity.GetLookRequest().Invoke(distanceVector);
            return true;
        }
    }
}