Shader "Custom/HealthBar"
{
    Properties
    {
        _Transparency ("Transparency", Range(0, 1)) = 0.5
        _Height ("Height", Range(0.1, 1)) = 0.3
        _Width ("Width", Range(0.5, 1)) = 0.3

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
            Name "HealthBar"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            float _Transparency;
            float _Height;
            float _Width;

            struct UnitData
            {
                float3 Position;
                float Scale;
                float CurrentHealth;
                float MaxHealth;
            };

            StructuredBuffer<UnitData> _HealthBarBuffer;

            struct MeshData
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 center : TEXCOORD1;
                float scale : TEXCOORD2;
                float current_health: TEXCOORD3;
                float max_health: TEXCOORD4;
            };

            Varyings Vert(uint vertexID : SV_VertexID, uint instanceID : SV_InstanceID)
            {
                Varyings o;

                float2 quadVerts[6] = {
                    float2(-1, -1),
                    float2(1, -1),
                    float2(1, 1),
                    float2(-1, -1),
                    float2(1, 1),
                    float2(-1, 1)
                };

                float3 right = normalize(UNITY_MATRIX_V[0].xyz); // camera right
                float3 up = normalize(UNITY_MATRIX_V[1].xyz); // camera up

                UnitData data = _HealthBarBuffer[instanceID];

                float2 barSize = float2(_Width * data.Scale, _Height);
                float2 localPos = quadVerts[vertexID] * barSize;
                float3 worldPos = data.Position + localPos.x * right + localPos.y * up;

                o.positionCS = TransformWorldToHClip(worldPos);
                o.uv = quadVerts[vertexID] * 0.5 + 0.5;
                o.center = data.Position;
                o.scale = data.Scale;
                o.current_health = data.CurrentHealth;
                o.max_health = data.MaxHealth;

                return o;
            }

            float4 Frag(Varyings i) : SV_Target
            {
                float2 uv = i.uv;

                float healthPercent = saturate(i.current_health / max(i.max_health, 0.001));
                float t = step(uv.x, healthPercent);

                float4 backgroundColor = float4(0, 0, 0, _Transparency);
                float4 fillColor = float4(0, 1, 0, _Transparency);

                return lerp(backgroundColor, fillColor, t);
            }
            ENDHLSL
        }
    }
}