Shader "Custom/TimedExplosion" {
	Properties {

		_Cutoff ("Cutoff Texture", 2D) = "white" {}
		_Noise("Noise Texture", 2D) = "white" {}
		_Decay("Decay pattern Texture", 2D) = "white" {}
		_Smoke("Smoke overlay Texture", 2D) = "white" {}
		_Gradient("Explosion Gradient Texture", 2D) = "white" {}
		_Progression ("% Progression of animation", Range(.1,.9)) = 0.5
		_Brightness ("Brightness (Ups exlosive color)", Range(0,10)) = 0.0
		_CutThres("Cutoff Threshold", Range(-2,2)) = 0.0
	}
	SubShader {
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows alpha:fade

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _Cutoff;
		sampler2D _Noise;
		sampler2D _Decay;
		sampler2D _Smoke;
		sampler2D _Gradient;

		struct Input {
			float2 uv_Cutoff;
			float2 uv_Noise;
			float2 uv_Decay;
			float2 uv_Smoke;
			float2 uv_Gradient;
		};

		float _Progression;
		float _Brightness;
		float _CutThres;


		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		half cutoffCheckBasic(float4 tex, float progression)
		{
			progression = max(progression +_CutThres, 0.1);
			float ret = tex.r < progression ? 0 : 1;
			return ret;
		}

		half cutoffCheckWhite(float4 tex, float progression) 
		{
			progression = max(progression + _CutThres, 0.1);
			float ret = tex.r > progression ? 1 : 0;
			return ret;
		}

		half cutoffCheckRed(float4 tex, float progression)
		{
			progression = max(progression + _CutThres, 0.1);
			float ret = tex.r > 1- progression ? 1 : 0;
			return ret;
		}

		float2 max2D(float2 inVec, float maxVal) {
			return float2(max(inVec.x, maxVal), max(inVec.y, maxVal));
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			float4 cutoffTex = tex2D(_Cutoff, IN.uv_Cutoff);
			float4 noiseTex = tex2D(_Noise, IN.uv_Noise);
			float4 decayTex = tex2D(_Decay, IN.uv_Decay);
			float4 smokeTex = tex2D(_Smoke, IN.uv_Smoke);

			// Albedo comes from a texture tinted by color
			float invNoise = 1 - noiseTex.r;
			float4 inverseR = float4(invNoise, invNoise, invNoise, 0);

			float invDecay = 1 - decayTex.g;
			float4 inverseG = float4(invDecay, invDecay, invDecay, 0);

			float invSmoke = smokeTex.b;
			float4 inverseB = float4(invSmoke, invSmoke, invSmoke, 0);

			float4 brightness = float4(_Brightness, _Brightness, _Brightness, 0);

			fixed4 cutoffMap = (cutoffTex * (inverseR * inverseB * (1 - _Progression) + brightness));

			cutoffMap = (cutoffMap)* cutoffCheckBasic(cutoffMap, _Progression) * (1- _Progression);

			fixed4 cutoffMapFull = cutoffCheckBasic(cutoffMap, _Progression);

			//cutoffMap += ;

			float2 gradientUVS = float2(max(1 - cutoffMap.r,0) * inverseG.g, (1- _Progression));
			gradientUVS.y = max(gradientUVS.y, 1);

			fixed4 c = float4(_Progression, _Progression, _Progression, 0);
			o.Albedo =  tex2D(_Gradient, gradientUVS) * cutoffMap;
			o.Emission = o.Albedo;
			o.Alpha = cutoffMapFull;
			//o.Alpha = cutoffCheckRed(inverse,(1- _Progression));
		}
		ENDCG
	}
	FallBack "Diffuse"
}
