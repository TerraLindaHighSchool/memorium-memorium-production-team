Shader "Custom/RimLight"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_RimColor ("Rim Color", Color) = (1,1,1,1)
		_RimPower ("Rim Power", float) = 1.0
    }
    SubShader
    {
		Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard 

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
			float3 viewDir;
        };
		
        fixed4 _Color;
		fixed4 _RimColor;
		float _RimPower;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Alpha = c.a;
			half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
			o.Emission = _Color - ((1 - _RimColor) * pow(rim, _RimPower));
        }
        ENDCG
    }
    FallBack "Diffuse"
}
