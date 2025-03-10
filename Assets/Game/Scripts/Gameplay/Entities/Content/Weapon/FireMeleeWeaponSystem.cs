using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace SampleGame
{
    public sealed class FireMeleeWeaponSystem : IEcsRunSystem
    {
        private readonly EcsEventInject<AnimationEvent> _events;
        private readonly EcsWorldInject _world;
        private readonly EcsPoolInject<MeleeWeapon> _meleeWeapons;
        private readonly EcsUseCaseInject<FireMeleeUseCase> _fireMeleeUseCase;

        public void Run(IEcsSystems systems)
        {
            foreach (AnimationEvent animationEvent in _events.Value)
            {
                if (!animationEvent.entity.Unpack(_world.Value, out int entity))
                    continue;

                if (!_meleeWeapons.Value.Has(entity))
                    continue;

                int damage = _meleeWeapons.Value.Get(entity).Damage;
                _fireMeleeUseCase.Value.Fire(entity, damage);
            }
        }
    }
}