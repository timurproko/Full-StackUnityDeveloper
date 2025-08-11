Shader "Custom/HoverSelection"
{
    Properties
    {
        _Thickness ("Thickness", Range(0, 0.25)) = 0.1
        _Dash ("Dash", Range(0.1, 1)) = 1
        _Speed ("Speed", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
            "Queue" = "Overlay"
            "RenderPipeline" = "UniversalPipeline"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        ZTest Always
        Cull Off
        Pass
        {
            Name "HoverSelection"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "UnitSelection.hlsl"

            float _Thickness;
            float _Dash;
            float _Speed;

            struct UnitData
            {
                float3 Position;
                float Scale;
                float4 Color;
            };

            StructuredBuffer<UnitData> _UnitSelectionBuffer;

            struct MeshData
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 position : TEXCOORD1;
                float scale : TEXCOORD2;
                float4 color : COLOR0;
            };

            static const float2 quadVerts[6] = {
                float2(-1, -1),
                float2(1, -1),
                float2(1, 1),
                float2(-1, -1),
                float2(1, 1),
                float2(-1, 1)
            };

            Varyings Vert(uint vertexID : SV_VertexID, uint instanceID : SV_InstanceID)
            {
                Varyings o;

                UnitData data = _UnitSelectionBuffer[instanceID];
                float2 localPos = quadVerts[vertexID] * data.Scale;
                float3 worldPos = data.Position + float3(localPos.x, 0, localPos.y);
                float4 positionWS = float4(worldPos, 1);

                o.positionCS = TransformWorldToHClip(positionWS.xyz);
                o.uv = quadVerts[vertexID] * 0.5 + 0.5;

                o.position = data.Position;
                o.scale = data.Scale;
                o.color = data.Color;
                
                return o;
            }

            float4 Frag(Varyings i) : SV_Target
            {
                float2 localPos = (i.uv * 2.0 - 1.0) * i.scale;
                float3 worldPos = i.position + float3(localPos.x, 0, localPos.y);

                float radius = max(i.scale - _Thickness, 0.0);
                float circumference = PI * radius;
                float uniformDash = circumference / _Dash;
                float uniformSpeed = (_Speed / uniformDash);

                return DrawHoverCircle(
                    worldPos,
                    i.position,
                    radius,
                    _Thickness,
                    i.color,
                    uniformDash,
                    uniformSpeed
                );
            }
            ENDHLSL
        }
    }
}