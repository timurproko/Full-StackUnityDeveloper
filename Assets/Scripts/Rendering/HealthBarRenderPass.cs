using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SampleGame
{
    public class HealthBarRenderPass : ScriptableRenderPass
    {
        private const int _maxUnits = 128;

        private readonly Material _material;
        private readonly UnitSelectionManager _selection;
        private readonly ComputeBuffer _unitDataBuffer;

        private struct UnitData
        {
            public Vector3 Position;
            public float Scale;
            public float CurrentHealth;
            public float MaxHealth;
        }

        public HealthBarRenderPass(Material material, UnitSelectionManager selection)
        {
            _material = material;
            _selection = selection;
            _unitDataBuffer = new ComputeBuffer(_maxUnits, System.Runtime.InteropServices.Marshal.SizeOf<UnitData>());
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!_selection || _selection.UnitCount == 0 || !_material)
                return;

            int count = 0;
            
            UnitData[] unitDataArray = new UnitData[_maxUnits];

            foreach (var unit in _selection)
            {
                if (count >= _maxUnits)
                    break;

                unitDataArray[count].Position = unit.Position;
                unitDataArray[count].Scale = unit.Scale;
                unitDataArray[count].CurrentHealth = unit.CurrentHealth;
                unitDataArray[count].MaxHealth = unit.MaxHealth;
                
                count++;
            }

            _unitDataBuffer.SetData(unitDataArray);
            _material.SetBuffer("_HealthBarBuffer", _unitDataBuffer);
            _material.SetInt("_UnitCount", count);

            Camera cam = renderingData.cameraData.camera;
            Matrix4x4 view = cam.worldToCameraMatrix;
            Matrix4x4 proj = cam.projectionMatrix;
            
            CommandBuffer cmd = CommandBufferPool.Get("HealthBarPass");
            cmd.SetViewProjectionMatrices(view, proj);
            cmd.DrawProcedural(Matrix4x4.identity, _material, 0, MeshTopology.Triangles, 6, count);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
        
        public void Dispose()
        {
            _unitDataBuffer?.Dispose();
        }
    }
}