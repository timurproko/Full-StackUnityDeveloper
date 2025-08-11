using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SampleGame
{
    public class HoverSelectionRenderPass : ScriptableRenderPass, IDisposable
    {
        private const int MaxUnits = 1;

        private static readonly int UnitSelectionBuffer = Shader.PropertyToID("_UnitSelectionBuffer");

        private readonly Material _material;
        private readonly UnitHover _unit;
        private readonly UnitSelectionManager _selection;
        private readonly TeamsConfig _teams;
        private readonly ComputeBuffer _unitDataBuffer;

        private struct UnitData
        {
            public Vector3 Position;
            public float Scale;
            public Vector4 Color;
        }

        public HoverSelectionRenderPass(Material material, UnitHover unit, UnitSelectionManager selection,
            TeamsConfig teams)
        {
            _material = material;
            _unit = unit;
            _selection = selection;
            _teams = teams;
            _unitDataBuffer = new ComputeBuffer(MaxUnits, UnsafeUtility.SizeOf<UnitData>());
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!_unit || !_unit.Target || !_material || !_teams) return;
            if (_selection != null && _selection.Contains(_unit.Target)) return;

            NativeArray<UnitData> unitDataArray = new(MaxUnits, Allocator.Temp);

            unitDataArray[0] = new UnitData
            {
                Position = _unit.Target.Position,
                Scale = _unit.Target.Scale,
                Color = _teams.GetColor(_unit.Target.TeamType)
            };

            _unitDataBuffer.SetData(unitDataArray, 0, 0, 1);
            _material.SetBuffer(UnitSelectionBuffer, _unitDataBuffer);

            var cam = renderingData.cameraData.camera;
            var view = cam.worldToCameraMatrix;
            var proj = cam.projectionMatrix;

            var cmd = CommandBufferPool.Get(nameof(HoverSelectionRenderPass));
            cmd.SetViewProjectionMatrices(view, proj);
            cmd.DrawProcedural(Matrix4x4.identity, _material, 0, MeshTopology.Triangles, 6, 1);
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