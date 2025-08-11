using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SampleGame
{
    public class HealthBarRenderPass : ScriptableRenderPass, IDisposable
    {
        private const int MaxUnits = 128;
        
        private static readonly int HealthBarBuffer = Shader.PropertyToID("_HealthBarBuffer");
        private static readonly int UnitCount = Shader.PropertyToID("_UnitCount");

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
            _unitDataBuffer = new ComputeBuffer(MaxUnits, System.Runtime.InteropServices.Marshal.SizeOf<UnitData>());
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!_selection || _selection.UnitCount == 0 || !_material)
                return;

            int count = 0;

            NativeArray<UnitData> unitDataArray = new(_selection.UnitCount, Allocator.Temp);

            foreach (var unit in _selection)
            {
                if (count >= MaxUnits)
                    break;

                unitDataArray[count] = new UnitData
                {
                    Position = unit.Position,
                    Scale = unit.Scale,
                    CurrentHealth = unit.CurrentHealth,
                    MaxHealth = unit.MaxHealth
                };

                count++;
            }

            _unitDataBuffer.SetData(unitDataArray);
            _material.SetBuffer(HealthBarBuffer, _unitDataBuffer);
            _material.SetInt(UnitCount, count);

            Camera cam = renderingData.cameraData.camera;
            Matrix4x4 view = cam.worldToCameraMatrix;
            Matrix4x4 proj = cam.projectionMatrix;

            CommandBuffer cmd = CommandBufferPool.Get(nameof(UnitSelectionRenderPass));
            cmd.SetViewProjectionMatrices(view, proj);
            cmd.DrawProcedural(Matrix4x4.identity, _material, 0, MeshTopology.Triangles, 6, count);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
            unitDataArray.Dispose();
        }

        public void Dispose()
        {
            _unitDataBuffer?.Dispose();
        }
    }
}