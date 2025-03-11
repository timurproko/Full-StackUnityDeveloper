using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public static class PatrolUseCase
    {
        public static void PatrolWaypoints(in IEntity entity, in float stoppingDistance)
        {
            Transform[] waypoints = entity.GetWaypoints();
            IReactiveVariable<int> waypointIndex = entity.GetWaypointIndex();
            
            int index = waypointIndex.Value;
            Vector3 currentWaypoint = waypoints[index].position;

            Vector3 characterPosition = entity.GetTransform().position;
            Vector3 distance = currentWaypoint - characterPosition;
            distance.y = 0;

            if (distance.sqrMagnitude <= stoppingDistance * stoppingDistance)
            {
                index = (index + 1) % waypoints.Length;
                waypointIndex.Value = index;
            }
            else
            {
                entity.GetMoveRequest().Invoke(distance.normalized);
            }
        }
    }
}