namespace Modules.BehaviourTree
{
    public sealed class BehaviourNodeParallel : BehaviourNodeComposite
    {
        public BehaviourNodeParallel(params IBehaviourNode[] nodes) : base(nodes)
        {
        }

        protected override void OnAbort()
        {
            foreach (var node in _nodes)
            {
                node.Abort();
            }
        }

        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            bool allSuccess = true;

            foreach (var node in _nodes)
            {
                BehaviourResult result = node.Run(deltaTime);

                if (result == BehaviourResult.FAILURE)
                {
                    return BehaviourResult.FAILURE;
                }

                if (result != BehaviourResult.SUCCESS)
                {
                    allSuccess = false;
                }
            }

            return allSuccess ? BehaviourResult.SUCCESS : BehaviourResult.RUNNING;
        }
    }
}