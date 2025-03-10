using Leopotam.EcsLite;
using Unity.Mathematics;
using UnityEngine;

namespace SampleGame
{
    [CreateAssetMenu(fileName = "Archer", menuName = "SampleGame/Entities/New Archer")]
    public class ArcherPrototype : EcsPrototype
    {
        [Header("General")]
        [SerializeField] private float _moveSpeed = 3f;
        [SerializeField] private float _stoppingDistance = 10f;
        [SerializeField] float _rotationSpeed = 10f;
        [SerializeField] private int _health = 5;
        [Header("Weapons")]
        [SerializeField] private float _fireCooldown = 1f;
        [SerializeField] private ProjectilePrototype _projectile;

        protected override void Install(in EcsWorld world, in int entity)
        {
            world.GetPool<UnitTag>().Add(entity);
            world.GetPool<DeathTag>().Add(entity);
            world.GetPool<AttackableTag>().Add(entity);

            world.GetPool<Radius>().Add(entity).value = 1f;
            world.GetPool<StoppingDistance>().Add(entity).value = _stoppingDistance;

            world.GetPool<UnitDirection>().Add(entity);
            world.GetPool<FireEnabled>().Add(entity);
            world.GetPool<Target>().Add(entity);

            world.GetPool<MoveableTag>().Add(entity);
            world.GetPool<MoveSpeed>().Add(entity).value = _moveSpeed;
            world.GetPool<MoveDirection>().Add(entity).value = new float3(0f, 0f, 1f);

            world.GetPool<RotatableTag>().Add(entity);
            world.GetPool<RotateDirection>().Add(entity).value = new float3(0f, 0f, -1f);
            world.GetPool<RotationSpeed>().Add(entity).value = _rotationSpeed;

            world.GetPool<Health>().Add(entity) = new Health
            {
                current = _health,
                max = _health
            };

            world.GetPool<FireOffset>().Add(entity).value = new float3(0f, 1.7f, 2f);
            world.GetPool<FireCooldown>().Add(entity) = new FireCooldown
            {
                current = 0,
                duration = _fireCooldown
            };

            world.GetPool<RangeWeapon>().Add(entity) = new RangeWeapon
            {
                Projectile = _projectile
            };
        }
    }
}