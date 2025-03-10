using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedSystems;
using UnityEngine;

namespace SampleGame
{
    [CreateAssetMenu(
        fileName = "EcsSystemsFactory",
        menuName = "SampleGame/Ecs/New EcsSystemsFactory"
    )]
    public sealed class EcsSystemsFactory : ScriptableObject
    {
        [SerializeField] private InputMap _inputMap;
        [SerializeField] private TeamViewConfig _teamViewConfig;

        [Header("Animation Keys")]
        [SerializeField] private string _attack = "Attack";
        [SerializeField] private string _takeDamage = "Take Damage";
        [SerializeField] private string _move = "IsWalking";

        [Header("Sound Clips")]
        [SerializeField] private AudioClip _unitTakeDamage;
        [SerializeField] private AudioClip _baseTakeDamage;
        [SerializeField] private AudioClip _archerAttack;
        [SerializeField] private AudioClip _swordmanAttack;

        public IEcsSystems Create()
        {
            EcsWorld world = new EcsWorld();
            EcsSystems systems = new EcsSystems(world, new GameData());

            systems.AddWorld(new EcsWorld(), EcsConsts.EventWorld);
            systems

                //INPUT:
                // .Add(new InputSystem(_inputMap))
                // .Add(new PlayerMoveController())
                // .Add(new PlayerFireController())

                //GAME_LOGIC:
                .Add(new SpawnSystem())
                .Add(new LifetimeSystem())
                .Add(new FireCooldownSystem())
                .Add(new DeathSystem())
                .Add(new DespawnSystem())
                .Add(new MoveSystem())
                .Add(new RotationSystem())
                
                //Unit:
                .Add(new UnitTargetSystem())
                .Add(new UnitDirectionSystem())
                .Add(new UnitMoveSystem())
                .Add(new UnitRotateSystem())
                .Add(new UnitFireRequestSystem())
                .Add(new UnitFireAISystem())
                
                //Weapon:
                .Add(new FireMeleeWeaponSystem())
                .Add(new ProjectileInitializer())
                .Add(new FireRangeWeaponSystem())
                .Add(new ProjectileCollisionSystem())
                
                //Other:
                .Add(new HideBannersSystem())
                .Add(new TeamWinSystem())

                //RENDERING:
                .Add(new TransformViewSystem())
                .Add(new TeamViewSystem(_teamViewConfig))
                .Add(new MoveAnimSystem(_move))
                .Add(new TakeDamageAnimSystem(_takeDamage))
                .Add(new FireAnimSystem(_attack))
                .Add(new TakeDamageParticleSystem())

                //AUDIO:
                .Add(new UnitTakeDamageAudioSystem(_unitTakeDamage))
                .Add(new BaseTakeDamageAudioSystem(_baseTakeDamage))

                //CLEAR:
                .ClearEvents<AnimationEvent>()
                .ClearEvents<FireEvent>()
                .ClearEvents<TakeDamageEvent>()

                //DEBUG:
#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem(EcsConsts.EventWorld));
#endif
            return systems;
        }
    }
}