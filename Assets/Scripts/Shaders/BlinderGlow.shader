Shader "Custom Shaders/BlinderGlow" 
{
	Properties
	{
		_GlowColor ("Glow Color", Color) = (0,0,0,0)
		_RimPower ("Glow Strength", Range(0.5,8.0)) = 3.0
		_RimLevel ("Rim Visibility Level", Range(0,1)) = 1.0
	}

    SubShader 
	{
		Blend One One
		ZTest Always
		ZWrite Off

		Tags 
		{ 
			"RenderType"="Transparent"
			"Queue"="Transparent"
		}
		
		CGPROGRAM
		#pragma surface surf SimpleLambert

		struct Input 
		{
			float4 _Color;
			float3 viewDir;
		};
 
		float _RimLevel;
		float _RimPower;
 
		void surf(Input IN, inout SurfaceOutput o) {
		}

		fixed4 _GlowColor;
	   
		half4 LightingSimpleLambert (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) 
		{
			//Glow setup - Done before LightingSimple
			float rim = 1.0 - saturate(dot(normalize(viewDir), s.Normal));
			half4 c;
			c.rgb = (_GlowColor * pow(rim, _RimPower) * _RimLevel);
            c.a = 1;
            return c;
		}
		ENDCG
	}
    FallBack "Diffuse"
}