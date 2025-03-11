using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public static class AttackTargetUseCase
    {
        public static bool Attack(in IEntity entity, in IEntity target, in float attackDistance)
        {
            if (entity == null || target == null)
                return false;

            Vector3 targetVector = VectorUseCase.DistanceVector(entity, target);
            if (targetVector.sqrMagnitude > attackDistance * attackDistance)
                return false;

            entity.GetFireRequest().Invoke();
            return true;
        }
    }
}