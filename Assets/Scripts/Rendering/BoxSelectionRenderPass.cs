using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SampleGame
{
    public class BoxSelectionRenderPass : ScriptableRenderPass
    {
        private readonly Material _material;
        private readonly RectInput _rectInput;
        
        public BoxSelectionRenderPass(Material material, RectInput rectInput)
        {
            _material = material;
            _rectInput = rectInput;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!_rectInput || !_rectInput.IsSelecting || !_material)
                return;

            var startPoint = new Vector4(_rectInput.StartPoint.x, _rectInput.StartPoint.y, 0, 0);
            var endPoint = new Vector4(_rectInput.EndPoint.x, _rectInput.EndPoint.y, 0, 0);
            
            _material.SetVector("_StartPoint", startPoint);
            _material.SetVector("_EndPoint", endPoint);
            
            CommandBuffer cmd = CommandBufferPool.Get("BoxSelectionPass");
            cmd.SetViewProjectionMatrices(Matrix4x4.identity, Matrix4x4.identity);
            cmd.DrawProcedural(Matrix4x4.identity, _material, 0, MeshTopology.Triangles, 3);
            
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }
}