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

            float4 _Color;
            float _Thickness;
            float _Dash;
            float _Speed;
            float _Scale;
            float3 _Position;

            struct MeshData
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };


            Varyings Vert(uint vertexID : SV_VertexID)
            {
                Varyings o;

                float2 quadVerts[6] = {
                    float2(-1, -1), // bottom-left
                    float2(1, -1), // bottom-right
                    float2(1, 1), // top-right
                    float2(-1, -1), // bottom-left
                    float2(1, 1), // top-right
                    float2(-1, 1) // top-left
                };

                float2 localPos = quadVerts[vertexID] * _Scale;
                float3 worldPos = _Position + float3(localPos.x, 0, localPos.y);
                float4 positionWS = float4(worldPos, 1);
                
                o.positionCS = TransformWorldToHClip(positionWS.xyz);
                o.uv = quadVerts[vertexID] * 0.5 + 0.5;

                return o;
            }
            
            float4 Frag(Varyings i) : SV_Target
            {
                float2 localPos = (i.uv * 2.0 - 1.0) * _Scale;
                float3 worldPos = _Position + float3(localPos.x, 0, localPos.y);

                float radius = max(_Scale - _Thickness, 0.0);
                float worldDashLength = _Dash;
                float circumference = PI * radius;
                float dash = circumference / worldDashLength;
                float angleSpeed = (_Speed / dash);

                return DrawHoverCircle(
                    worldPos,
                    _Position,
                    radius,
                    _Thickness,
                    _Color,
                    dash,
                    angleSpeed
                );
            }
            ENDHLSL
        }
    }
}