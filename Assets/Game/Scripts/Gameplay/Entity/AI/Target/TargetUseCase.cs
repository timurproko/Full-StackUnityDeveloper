using System;
using System.Buffers;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public static class TargetUseCase
    {
        public static bool FindClosest(
            in Vector3 center,
            in float radius,
            in LayerMask layerMask,
            in int bufferSize,
            out IEntity closest,
            in Predicate<IEntity> predicate
        )
        {
            var arrayPool = ArrayPool<Collider>.Shared;
            Collider[] buffer = arrayPool.Rent(bufferSize);

            int count = Physics.OverlapSphereNonAlloc(center, radius, buffer, layerMask);

            float minDistance = float.MaxValue;
            closest = default;
            
            for (int i = 0; i < count; i++)
            {
                Collider collider = buffer[i];
                if (!collider.TryGetEntity(out IEntity other))
                    continue;

                if (!predicate.Invoke(other))
                    continue;

                Vector3 position = other.GetTransform().position;
                Vector3 vector = position - center;
                float distance = vector.sqrMagnitude;
                
                if (distance < minDistance)
                {
                    closest = other;
                    minDistance = distance;
                }
            }

            arrayPool.Return(buffer);
            return closest != null;
        }
    }
}