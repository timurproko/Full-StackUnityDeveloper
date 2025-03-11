using Atomic.Elements;
using Atomic.Entities;
using Modules.FSM;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class CharacterAIInstaller : SceneEntityInstaller
    {
        private IState _patrolState;
        private IState _attackState;
        private IState _holdState;
        private IState _chaseState;

        [SerializeField] private float _stoppingDistance = 1.5f;
        [SerializeField] private float _attackingDistance = 5f;
        [SerializeField] private Transform[] _waypoints;
        [SerializeField] private TargetSensorInstaller _sensorInstaller;

        public override void Install(IEntity entity)
        {
            _sensorInstaller.Install(entity);

            entity.AddTarget(new ReactiveVariable<IEntity>());
            entity.AddWaypoints(_waypoints);
            entity.AddWaypointIndex(new ReactiveInt());

            _patrolState = StateFactory.CreatePatrolState(entity, _stoppingDistance);
            _attackState = StateFactory.CreateAttackState(entity, _stoppingDistance, _attackingDistance);
            _holdState = StateFactory.CreateHoldState(entity, _attackingDistance);
            _chaseState = StateFactory.CreateChaseState(entity, _stoppingDistance);

            entity.WhenEnable(_attackState.OnEnter);
            entity.WhenDisable(_attackState.OnExit);
            entity.WhenFixedUpdate(_attackState.OnUpdate);

            // var stateMachine = CreateStateMachine(entity);
            // entity.AddStateMachine(stateMachine);
            // entity.WhenEnable(stateMachine.OnEnter);
            // entity.WhenDisable(stateMachine.OnExit);
            // entity.WhenFixedUpdate(stateMachine.OnUpdate);
        }

        // private IAutoStateMachine<StateName> CreateStateMachine(IEntity entity)
        // {
        //     return new AutoStateMachine<StateName>(StateName.Patrol,
        //         new[]
        //         {
        //             (StateName.Patrol, StateFactory.CreatePatrolState(entity, _stoppingDistance)),
        //             (StateName.Attack, StateFactory.CreateAttackState(entity, _stoppingDistance))
        //         },
        //         new StateTransition<StateName>[]
        //         {
        //             new(StateName.Patrol, StateName.Attack, priority: 3, () => entity.GetTarget().Value != null, null),
        //             new(StateName.Attack, StateName.Patrol, () => entity.GetTarget().Value == null),
        //         }
        //     );
        // }
    }
}