using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SampleGame
{
    public class HoverSelectionRenderPass : ScriptableRenderPass
    {
        private readonly Material _material;
        private readonly UnitHover _unit;

        public HoverSelectionRenderPass(Material material, UnitHover unit)
        {
            _material = material;
            _unit = unit;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!_unit || !_unit.Target || !_material)
                return;
            
            Vector3 position = _unit.Target.Position;
            float scale = _unit.Target.Scale;
            Color color = _unit.Target.TeamType switch
            {
                TeamType.Blue => Color.blue,
                TeamType.Red => Color.red,
                TeamType.Green => Color.green,
                TeamType.Yellow => Color.yellow,
                TeamType.Gray => Color.gray,
                _ => Color.white
            };

            _material.SetVector("_Position", position);
            _material.SetFloat("_Scale", scale);
            _material.SetColor("_Color", color);
            
            Camera cam = renderingData.cameraData.camera;
            Matrix4x4 view = cam.worldToCameraMatrix;
            Matrix4x4 proj = cam.projectionMatrix;
            
            CommandBuffer cmd = CommandBufferPool.Get("HoverSelectionPass");
            cmd.SetViewProjectionMatrices(view, proj);
            cmd.DrawProcedural(Matrix4x4.identity, _material, 0, MeshTopology.Triangles, 6, 1);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }
}