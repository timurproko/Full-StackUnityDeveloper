using System;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    [Serializable]
    public sealed class TargetSensorInstaller : IEntityInstaller
    {
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _radius;
        [SerializeField] private Transform _center;
        [SerializeField] private int _bufferSize = 32;

        public void Install(IEntity entity)
        {
            entity.AddBehaviour(new TargetSensor(_layerMask, _center, _radius, _bufferSize, other => other.HasCharacterTag()));
        }
    }
}