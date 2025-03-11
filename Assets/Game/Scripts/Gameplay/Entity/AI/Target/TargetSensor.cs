using System;
using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class TargetSensor : IEntityInit, IEntityFixedUpdate, IEntityGizmos
    {
        private readonly LayerMask _layerMask;
        private readonly Transform _center;
        private readonly float _radius;
        private readonly int _bufferSize;
        private readonly Predicate<IEntity> _predicate;

        private IReactiveVariable<IEntity> _target;

        public TargetSensor(
            LayerMask layerMask,
            Transform center,
            float radius,
            int bufferSize,
            Predicate<IEntity> predicate
        )
        {
            _layerMask = layerMask;
            _center = center;
            _radius = radius;
            _bufferSize = bufferSize;
            _predicate = predicate;
        }

        public void Init(in IEntity entity)
        {
            _target = entity.GetTarget();
        }

        public void OnFixedUpdate(in IEntity entity, in float deltaTime)
        {
            TargetUseCase.FindClosest(
                _center.position,
                _radius,
                _layerMask,
                _bufferSize,
                out IEntity target,
                _predicate
            );
            _target.Value = target;
        }

        public void OnGizmosDraw(in IEntity entity)
        {
            if (_center == null)
                return;

            Color prevColor = Gizmos.color;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_center.position, _radius);
            Gizmos.color = prevColor;
        }
    }
}