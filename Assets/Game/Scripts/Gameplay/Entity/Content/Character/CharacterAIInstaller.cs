using Atomic.Elements;
using Atomic.Entities;
using Modules.FSM;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class CharacterAIInstaller : SceneEntityInstaller
    {
        [SerializeField] private float _stoppingDistance;
        [SerializeField] private Transform[] _waypoints;

        public override void Install(IEntity entity)
        {
            entity.AddTarget(new ReactiveVariable<IEntity>());
            entity.AddWaypoints(_waypoints);
            entity.AddWaypointIndex(new ReactiveInt());
            
            var stateMachine = CreateStateMachine(entity);
            entity.AddStateMachine(stateMachine);
            entity.WhenEnable(stateMachine.OnEnter);
            entity.WhenDisable(stateMachine.OnExit);
            entity.WhenFixedUpdate(stateMachine.OnUpdate);
        }

        private IAutoStateMachine<StateName> CreateStateMachine(IEntity entity)
        {
            return new AutoStateMachine<StateName>(StateName.Patrol,
                new[]
                {
                    (StateName.Patrol, StateFactory.CreatePatrolState(entity, _stoppingDistance)),
                    (StateName.Attack, StateFactory.CreateAttackState(entity, _stoppingDistance))
                },
                new StateTransition<StateName>[]
                {
                    new(StateName.Patrol, StateName.Attack, priority: 3, () => entity.GetTarget().Value != null, null),
                    new(StateName.Attack, StateName.Patrol, () => entity.GetTarget().Value == null),
                }
            );
        }
    }
}