// using Modules.BehaviourTree;
// using Modules.FSM;
//
// public class SequenceState : IState
// {
//     private readonly IBehaviourNode _behaviourTree;
//
//     public SequenceState(IBehaviourNode behaviourTree)
//     {
//         _behaviourTree = behaviourTree;
//     }
//
//     public void OnEnter()
//     {
//     }
//
//     public void OnUpdate(float deltaTime)
//     {
//         _behaviourTree.Run(deltaTime);
//     }
//
//     public void OnExit()
//     {
//     }
// }