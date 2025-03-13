// using Atomic.Entities;
// using Modules.BehaviourTree;
// using UnityEngine;
//
// namespace Game.Gameplay
// {
//     public class MoveToBaseNode : BehaviourNode
//     {
//         private readonly IEntity _entity;
//         private readonly float _stoppingDistance;
//         private readonly Vector3 _basePosition;
//
//         public MoveToBaseNode(IEntity entity, Vector3 basePosition)
//         {
//             _entity = entity;
//             _basePosition = basePosition;
//         }
//
//         protected override BehaviourResult OnUpdate(float deltaTime)
//         {
//             return !MoveToUseCase.MoveToBase(_entity, _stoppingDistance, _basePosition,  out bool reached)
//                 ? BehaviourResult.FAILURE
//                 : reached
//                     ? BehaviourResult.SUCCESS
//                     : BehaviourResult.RUNNING;
//         }
//     }
// }