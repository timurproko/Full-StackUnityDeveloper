using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace SampleGame
{
    public sealed class FireRangeWeaponSystem : IEcsRunSystem
    {
        private readonly EcsEventInject<AnimationEvent> _events;
        private readonly EcsWorldInject _world;
        private readonly EcsPoolInject<RangeWeapon> _rangeWeapons;
        private readonly EcsUseCaseInject<FireProjectileUseCase> _fireProjectileUseCase;

        public void Run(IEcsSystems systems)
        {
            foreach (AnimationEvent animationEvent in _events.Value)
            {
                if (!animationEvent.entity.Unpack(_world.Value, out int entity))
                    continue;

                if (!_rangeWeapons.Value.Has(entity))
                    continue;

                ref var bowWeapon = ref _rangeWeapons.Value.Get(entity);
                _fireProjectileUseCase.Value.Fire(entity, bowWeapon.Projectile);
            }
        }
    }
}