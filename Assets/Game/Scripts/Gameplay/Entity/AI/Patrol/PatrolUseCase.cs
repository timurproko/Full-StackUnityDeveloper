using System;
using System.Collections.Generic;
using System.Linq;
using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public static class PatrolUseCase
    {
        public static bool PatrolWaypoints(in IEntity entity, float stoppingDistance)
        {
            return PatrolWaypoints(entity, stoppingDistance, out _);
        }

        public static bool PatrolWaypoints(in IEntity entity, in float stoppingDistance, out bool reached)
        {
            Vector3[] waypoints = entity.GetWaypoints();
            if (waypoints == null || waypoints.Length == 0)
            {
                reached = false;
                return false;
            }

            IReactiveVariable<int> waypointIndex = entity.GetWaypointIndex();

            int index = waypointIndex.Value;
            Vector3 currentWaypoint = waypoints[index];

            Vector3 characterPosition = entity.GetTransform().position;
            Vector3 distance = currentWaypoint - characterPosition;
            distance.y = 0;

            reached = distance.sqrMagnitude <= stoppingDistance * stoppingDistance;

            if (reached)
            {
                index = (index + 1) % waypoints.Length;
                waypointIndex.Value = index;
            }
            else
            {
                entity.GetMoveRequest().Invoke(distance.normalized);
            }

            return true;
        }

        public static void AddWaypoints(in IEntity entity, in Vector3 point)
        {
            List<Vector3> waypoints = entity.GetWaypoints().ToList();

            if (waypoints.Count == 0)
                waypoints.Add(entity.GetTransform().position);

            waypoints.Add(point);
            entity.SetWaypoints(waypoints.ToArray());
        }


        public static void ClearWaypoints(in IEntity entity)
        {
            entity.SetWaypoints(Array.Empty<Vector3>());
            entity.GetWaypointIndex().Value = 0;
        }
    }
}