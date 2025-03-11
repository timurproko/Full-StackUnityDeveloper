using Atomic.Elements;
using Atomic.Entities;
using Modules.FSM;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class CharacterAIInstaller : SceneEntityInstaller
    {
        [SerializeField] private float _stoppingDistance = 1.5f;
        [SerializeField] private float _attackingDistance = 5f;
        [SerializeField] private Transform[] _waypoints;
        [SerializeField] private TargetSensorInstaller _sensorInstaller;

        private IStateMachine<StateName> _stateMachine;

        public override void Install(IEntity entity)
        {
            _sensorInstaller.Install(entity);
            _stateMachine = CreateStateMachine(entity);

            entity.AddTarget(new ReactiveVariable<IEntity>());
            entity.AddWaypoints(_waypoints);
            entity.AddWaypointIndex(new ReactiveInt());
            entity.AddStateMachine(_stateMachine);
            entity.AddMovePoint(new ReactiveVariable<Vector3>());

            entity.WhenEnable(_stateMachine.OnEnter);
            entity.WhenDisable(_stateMachine.OnExit);
            entity.WhenFixedUpdate(_stateMachine.OnUpdate);
        }

        private IStateMachine<StateName> CreateStateMachine(IEntity entity)
        {
            return new StateMachine<StateName>(StateName.Idle,
                (StateName.Idle, StateFactory.CreateIdleState(entity)),
                (StateName.Move, StateFactory.CreateMoveState(entity)),
                (StateName.Patrol, StateFactory.CreatePatrolState(entity, _stoppingDistance)),
                (StateName.Attack, StateFactory.CreateAttackState(entity, _attackingDistance, _stoppingDistance)),
                (StateName.Hold, StateFactory.CreateHoldState(entity, _attackingDistance)),
                (StateName.Follow, StateFactory.CreateChaseState(entity, _stoppingDistance))
            );
        }
    }
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