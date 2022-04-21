Shader "Unlit/Fire"
{
    Properties
    {
        _NoiseTex("Noise Texture", 2D) = "white" {}
		_MainTex ("Texture", 2D) = "white" {}
        _AlphaTex("Alpha Texture", 2D) = "white" {}
        _CullingMult("Culling Multiplier", float) = 1.2
		_Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType" = "Transparent" "IgnoreProjector"="True"}
        ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
			#pragma multi_compile_instancing

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
			sampler2D _NoiseTex;
            sampler2D _AlphaTex;
			fixed4 _Color;
            float4 _MainTex_ST;
            float _CullingMult;

            v2f vert (appdata v)
            {
                v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				//UNITY_TRANSFER_INSTANCE_ID(v, o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                ///remove if preformance hit is real bad <
                o.uv = v.uv.xy;

                // billboard mesh towards camera
                float3 vpos = mul((float3x3)unity_ObjectToWorld, v.vertex.xyz);
                float4 worldCoord = float4(unity_ObjectToWorld._m03, unity_ObjectToWorld._m13, unity_ObjectToWorld._m23, 1);
                float4 viewPos = mul(UNITY_MATRIX_V, worldCoord) + float4(vpos, 0);
                float4 outPos = mul(UNITY_MATRIX_P, viewPos);

                o.vertex = outPos;
                /// >
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float4 alphaTex = tex2D(_AlphaTex, i.uv);
                float4 noiseValue = tex2D(_NoiseTex, i.uv - float2(0, (_Time.x) * 10)) - 0.3;
                float d = floor(length((float2(i.uv.x, 1 - (pow(1 - i.uv.y, 2) + noiseValue.y)) * 2 - 1) * _CullingMult / float2(0.5, 0.7)));
				float4 noiseTwo = (1 - (noiseValue * i.uv.y * 3));//* _Color;
                fixed4 col = (floor(noiseTwo * (1 - i.uv.y) + 0.2) + floor(noiseTwo * (1 - i.uv.y) + 0.4) + floor(noiseTwo * (1 - i.uv.y) + 1)) * _Color;
                /// :)
				///lerp(noiseTwo, -3, 0.1);
                ///col.w = floor(noiseTwo * (1 - i.uv.y) + 0.6 * d);//clamp((noiseTwo * (1 - i.uv.y) + 0.6), 0, 1);
                col.w = clamp(1-d, 0, 1);
                ///col.z = noiseTwo;
                /// apply fog
                ///UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
