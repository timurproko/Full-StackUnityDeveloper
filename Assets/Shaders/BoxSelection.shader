Shader "Custom/BoxSelection"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _BorderWidth ("Border Width", Float) = 1.0

        [HideInInspector] _StartPoint ("Start Point", Vector) = (0, 0, 0, 0)
        [HideInInspector] _EndPoint ("End Point", Vector) = (1, 1, 0, 0)
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
            Name "BoxSelection"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            float4 _Color;
            float _BorderWidth;
            float2 _StartPoint;
            float2 _EndPoint;

            struct MeshData
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Interpolators Vert(uint vertexID : SV_VertexID)
            {
                Interpolators o;

                float2 positions[3] = {
                    float2(-1, -1),
                    float2(3, -1),
                    float2(-1, 3)
                };

                float2 uvs[3] = {
                    float2(0, 0),
                    float2(2, 0),
                    float2(0, 2)
                };

                o.positionCS = float4(positions[vertexID], 0, 1);
                o.uv = uvs[vertexID];
                return o;
            }

            float4 Frag(Interpolators i) : SV_Target
            {
                float2 screenPos = i.uv * _ScreenParams.xy;
                screenPos.y = _ScreenParams.y - screenPos.y;

                float2 startPt = _StartPoint.xy;
                float2 endPt = _EndPoint.xy;

                float2 minScreen = float2(0.0, 0.0);
                float2 maxScreen = _ScreenParams.xy;


                float2 minRect = clamp(min(startPt, endPt), minScreen, maxScreen);
                float2 maxRect = clamp(max(startPt, endPt), minScreen, maxScreen);

                // Inside selection rectangle?
                bool insideRect = all(screenPos >= minRect) && all(screenPos <= maxRect);

                // Inside inner rect (excluding border)?
                bool insideInner = all(screenPos >= (minRect + _BorderWidth)) &&
                    all(screenPos <= (maxRect - _BorderWidth));

                // Border is inside, but not insideInner
                bool isBorder = insideRect && !insideInner;

                float4 borderColor = float4(_Color.rgb, 1.0);
                float4 fillColor = float4(_Color);

                return isBorder ? borderColor : insideRect ? fillColor : float4(0, 0, 0, 0);
            }
            ENDHLSL
        }
    }
}