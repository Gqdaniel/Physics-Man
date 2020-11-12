Shader "Unlit/TetrahedronShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 0)
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_Outline ("Outline width", Range (0.0, 0.03)) = .005
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldNormal : NORMAL;
                float3 viewDir : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.viewDir = WorldSpaceViewDir(v.vertex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 _Color;
            float4 _OutlineColor;
            float4 _Outline;

            fixed4 frag (v2f i) : SV_Target
            {

                float3 normal = normalize(i.worldNormal);
				float3 viewDir = normalize(i.viewDir);
				float NdotL = dot(_WorldSpaceLightPos0, normal);

				float4 outlineDot = dot(viewDir, normal);
                float4 outlineThickness = outlineDot * pow(NdotL, _Outline);
				float4 outline = outlineThickness * _OutlineColor;


                // sample the texture

                float4 color = _Color;
                return color + outline;
            }
            ENDCG
        }
    }
}
