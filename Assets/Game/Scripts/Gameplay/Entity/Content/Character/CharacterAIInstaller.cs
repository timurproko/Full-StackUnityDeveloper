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
        [SerializeField] private TargetSensorInstaller _sensorInstaller;

        public override void Install(IEntity entity)
        {
            _sensorInstaller.Install(entity);

            entity.AddTarget(new ReactiveVariable<IEntity>());
            entity.AddMovePoint(new ReactiveVariable<Vector3>());
            entity.AddWaypoints(new Vector3[] { });
            entity.AddWaypointIndex(new ReactiveInt());

            var stateMachine = CreateStateMachine(entity);
            entity.AddStateMachine(stateMachine);
            entity.WhenEnable(stateMachine.OnEnter);
            entity.WhenDisable(stateMachine.OnExit);
            entity.WhenFixedUpdate(stateMachine.OnUpdate);
        }

        private IStateMachine<StateName> CreateStateMachine(IEntity entity)
        {
            return new StateMachine<StateName>(StateName.Idle,
                (StateName.Idle, StateFactory.CreateIdleState(entity)),
                (StateName.Move, StateFactory.CreateMoveState(entity, 0.1f)),
                (StateName.Patrol, StateFactory.CreatePatrolState(entity, 0.1f)),
                (StateName.Attack, StateFactory.CreateBehaviourTreeState(SequenceFactory.CreateAttackSequence(entity, _stoppingDistance, _attackingDistance))),
                (StateName.Hold, StateFactory.CreateHoldState(entity, _attackingDistance)),
                (StateName.Follow, StateFactory.CreateChaseState(entity, _stoppingDistance))
            );
        }
    }
}