Shader "Custom/LD2018WorldspaceDissolve" {
	Properties{
		_Color("Primary Color", Color) = (1,1,1,1)
		_MainTex("Primary (RGB)", 2D) = "white" {}
		_NormalMap("Primary Normal map", 2D) = "bump" {}
		_Metallic("Metallic value", Range(0, 1)) = 0
		_Smoothness("Smoothness value", Range(0, 1)) = 0
	_Color2("Secondary Color", Color) = (1,1,1,1)
		_SecondTex("Secondary (RGB)", 2D) = "white" {}
	_Ramp("Toon Ramp (RGB)", 2D) = "gray" {}
	_NoiseTex("Dissolve Noise", 2D) = "white"{}
	_NScale("Noise Scale", Range(0, 10)) = 1
		_DisAmount("Noise Texture Opacity", Range(0.01, 1)) = 0.01
		_Radius("Radius", Range(0, 10)) = 0
		_DisLineWidth("Line Width", Range(0, 2)) = 0
		_DisLineColor("Line Tint", Color) = (1,1,1,1)

		_RimPower("Glow Strength", Range(0.5,8.0)) = 3.0
		_RimLevel("Rim Visibility Level", Range(0,1)) = 1.0

		[Toggle] _useLayer2("Swap Primary and Secondary Texture slots", Float) = 0
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows alpha:fade vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		float3 _Position0; // from script
		float3 _Position1; // from script
		float3 _Position2; // from script
		float3 _Position3; // from script
		float3 _Position4; // from script
		float3 _Position5; // from script
		float3 _Position6; // from script
		float3 _Position7; // from script
		float3 _Position8; // from script
		float3 _Position9; // from script

		float1 _Scale0; // from script
		float1 _Scale1; // from script
		float1 _Scale2; // from script
		float1 _Scale3; // from script
		float1 _Scale4; // from script
		float1 _Scale5; // from script
		float1 _Scale6; // from script
		float1 _Scale7; // from script
		float1 _Scale8; // from script
		float1 _Scale9; // from script

		sampler2D _MainTex, _NormalMap, _SecondTex;
		float4 _Color, _Color2;
		sampler2D _NoiseTex;
		float _Smoothness, _Metallic;
		float _DisAmount, _NScale;
		float _DisLineWidth;
		float4 _DisLineColor;
		float _Radius;
		float _RimLevel;
		float _RimPower;
		float _useLayer2;

		struct Input {
			float2 uv_MainTex : TEXCOORD0;
			float3 worldPos;// built in value to use the world space position
			float3 worldNormal; // built in value for world normal
			float3 viewDir; //Built in value for active camera view direction.
			float3 localNormal; //Meant to capture local normal from vert
			INTERNAL_DATA
		};

		float3 min10(float3 dis0, float3 dis1, float3 dis2, float3 dis3, float3 dis4, float3 dis5, float3 dis6, float3 dis7, float3 dis8, float3 dis9)
		{
			return min(min(min(min(min(min(min(min(min(dis0, dis1), dis2), dis3), dis4), dis5), dis6), dis7), dis8), dis9);
		}

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void vert(inout appdata_full v, out Input data)
		{
			UNITY_INITIALIZE_OUTPUT(Input, data);
			data.localNormal = v.normal.xyz;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			half4 c;
			half4 c2;
			if (_useLayer2) 
			{
				c = tex2D(_SecondTex, IN.uv_MainTex) * _Color;
				c2 = tex2D(_MainTex, IN.uv_MainTex) * _Color2;
			}
			else 
			{
				c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				c2 = tex2D(_SecondTex, IN.uv_MainTex) * _Color2;
			}
			

			// triplanar noise
			float3 blendNormal = saturate(pow(WorldNormalVector (IN, o.Normal) * 1.4, 4));
			half4 nSide1 = tex2D(_NoiseTex, (IN.worldPos.xy + _Time.x) * _NScale);
			half4 nSide2 = tex2D(_NoiseTex, (IN.worldPos.xz + _Time.x) * _NScale);
			half4 nTop = tex2D(_NoiseTex, (IN.worldPos.yz + _Time.x) * _NScale);

			float3 noisetexture = nSide1;
			noisetexture = lerp(noisetexture, nTop, blendNormal.x);
			noisetexture = lerp(noisetexture, nSide2, blendNormal.y);

			// distance influencer position to world position
			float3 dis0 = distance(_Position0, IN.worldPos) - _Scale0 / 2;
			float3 dis1 = distance(_Position1, IN.worldPos) - _Scale1 / 2;
			float3 dis2 = distance(_Position2, IN.worldPos) - _Scale2 / 2;
			float3 dis3 = distance(_Position3, IN.worldPos) - _Scale3 / 2;
			float3 dis4 = distance(_Position4, IN.worldPos) - _Scale4 / 2;
			float3 dis5 = distance(_Position5, IN.worldPos) - _Scale5 / 2;
			float3 dis6 = distance(_Position6, IN.worldPos) - _Scale6 / 2;
			float3 dis7 = distance(_Position7, IN.worldPos) - _Scale7 / 2;
			float3 dis8 = distance(_Position8, IN.worldPos) - _Scale8 / 2;
			float3 dis9 = distance(_Position9, IN.worldPos) - _Scale9 / 2;

			float3 minimum = min10(dis0, dis1, dis2, dis3, dis4, dis5, dis6, dis7, dis8, dis9);



			float3 sphere = 1 - saturate(minimum / _Radius);

			float3 sphereNoise = noisetexture.r * sphere;

			float3 DissolveLine = step(sphereNoise - _DisLineWidth, _DisAmount) * step(_DisAmount, sphereNoise); // line between two textures
			DissolveLine *= _DisLineColor; // color the line

			//Rim Stuff
			float rimPower = pow(abs(1 - dot(IN.viewDir, WorldNormalVector (IN, o.Normal))) * _RimPower, _RimLevel * 10);

			float3 primaryTex = (step(sphereNoise - _DisLineWidth, _DisAmount) * c.rgb);
			float3 secondaryTex = (step(_DisAmount, sphereNoise) * c2.rgb);
			float3 resultTex = primaryTex + secondaryTex + DissolveLine;
			o.Albedo = resultTex;

			o.Albedo += max(1 - minimum, 0) * rimPower;
			float3 primaryAlpha = (step(sphereNoise - _DisLineWidth, _DisAmount) * c.a);
			float3 secondaryAlpha = (step(_DisAmount, sphereNoise) * c2.a);
			float3 resultAlpha = primaryAlpha + secondaryAlpha + DissolveLine;

			o.Normal = UnpackNormal (tex2D (_NormalMap, IN.uv_MainTex));
			o.Smoothness = _Smoothness;
			o.Metallic = _Metallic;
			o.Emission = DissolveLine;
			o.Alpha = resultAlpha;
		}


		ENDCG
	}
	FallBack "Diffuse"
}
