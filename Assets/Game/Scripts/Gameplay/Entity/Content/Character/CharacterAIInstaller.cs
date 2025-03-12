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
                (StateName.Idle, StateMachineFactory.CreateIdleState(entity)),
                (StateName.Move, StateMachineFactory.CreateMoveState(entity, 0.1f)),
                (StateName.Patrol, StateMachineFactory.CreateBehaviourTreeState(
                    BehaviourTreeFactory.CreatePatrolSequence(entity, 0.1f, _attackingDistance))),
                (StateName.Attack, StateMachineFactory.CreateBehaviourTreeState(
                    BehaviourTreeFactory.CreateAttackSequence(entity, 0.1f, _attackingDistance))),
                (StateName.Hold, StateMachineFactory.CreateHoldState(entity, _attackingDistance)),
                (StateName.Follow, StateMachineFactory.CreateBehaviourTreeState(
                    BehaviourTreeFactory.CreateFollowSequence(entity, _stoppingDistance)))
            );
        }
    }
}