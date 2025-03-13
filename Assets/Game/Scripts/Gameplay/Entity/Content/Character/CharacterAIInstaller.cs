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
            entity.AddMovePosition(new ReactiveVariable<Vector3>(entity.GetTransform().position));
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
            return new StateMachine<StateName>(StateName.Stop,
                (StateName.Stop, StateMachineFactory.CreateBehaviourTreeState(
                    BehaviourTreeFactory.CreateDefaultSequence(entity, _stoppingDistance, _attackingDistance))),
                (StateName.Move, StateMachineFactory.CreateMoveState(entity, 0.1f)),
                (StateName.Patrol, StateMachineFactory.CreatePatrolState(entity, _stoppingDistance)),
                (StateName.Attack, StateMachineFactory.CreateBehaviourTreeState(
                    BehaviourTreeFactory.CreateAttackSequence(entity, _stoppingDistance, _attackingDistance))),
                (StateName.Hold, StateMachineFactory.CreateHoldState(entity, _attackingDistance)),
                (StateName.Follow, StateMachineFactory.CreateBehaviourTreeState(
                    BehaviourTreeFactory.CreateFollowSequence(entity, _stoppingDistance)))
            );
        }
    }
}