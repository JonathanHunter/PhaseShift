Shader "Toon/Lit Dissolve DoubleTex" {
    Properties {
        _Color ("Primary Color", Color) = (1,1,1,1)
        _MainTex ("Primary (RGB)", 2D) = "white" {}
		_Color2 ("Secondary Color", Color) = (1,1,1,1)
		_SecondTex ("Secondary (RGB)", 2D) = "white" {}
        _Ramp ("Toon Ramp (RGB)", 2D) = "gray" {}
        _NoiseTex("Dissolve Noise", 2D) = "white"{} 
        _NScale ("Noise Scale", Range(0, 10)) = 1 
        _DisAmount("Noise Texture Opacity", Range(0.01, 1)) =0.01
        _Radius("Radius", Range(0, 10)) = 0 
        _DisLineWidth("Line Width", Range(0, 2)) = 0 
        _DisLineColor("Line Tint", Color) = (1,1,1,1)   
    }
 
        SubShader{
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
            LOD 200            
        
CGPROGRAM
 
#pragma surface surf ToonRamp alpha:fade
sampler2D _Ramp;
 
// custom lighting function that uses a texture ramp based
// on angle between light direction and normal
#pragma lighting ToonRamp exclude_path:prepass alpha:fade
inline half4 LightingToonRamp (SurfaceOutput s, half3 lightDir, half atten)
{
    #ifndef USING_DIRECTIONAL_LIGHT
    lightDir = normalize(lightDir);
    #endif
   
    half d = dot (s.Normal, lightDir)*0.5 + 0.5;
    half3 ramp = tex2D (_Ramp, float2(d,d)).rgb;
    half4 c;
    c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
    c.a = 1;
    return c;
}


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

sampler2D _MainTex, _SecondTex;
float4 _Color, _Color2;
sampler2D _NoiseTex;
float _DisAmount, _NScale;
float _DisLineWidth;
float4 _DisLineColor;
float _Radius;
 
 
struct Input {
    float2 uv_MainTex : TEXCOORD0;
    float3 worldPos;// built in value to use the world space position
	float3 worldNormal; // built in value for world normal
   
};

float3 min10(float3 dis0, float3 dis1, float3 dis2, float3 dis3, float3 dis4, float3 dis5, float3 dis6, float3 dis7, float3 dis8, float3 dis9)
{
	return min(min(min(min(min(min(min(min(min(dis0, dis1), dis2), dis3), dis4), dis5), dis6), dis7), dis8), dis9);
}
 
void surf (Input IN, inout SurfaceOutput o) {
    half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	half4 c2 = tex2D(_SecondTex, IN.uv_MainTex) * _Color2;

	// triplanar noise
	 float3 blendNormal = saturate(pow(IN.worldNormal * 1.4,4));
    half4 nSide1 = tex2D(_NoiseTex, (IN.worldPos.xy + _Time.x) * _NScale); 
	half4 nSide2 = tex2D(_NoiseTex, (IN.worldPos.xz + _Time.x) * _NScale);
	half4 nTop = tex2D(_NoiseTex, (IN.worldPos.yz + _Time.x) * _NScale);

	float3 noisetexture = nSide1;
    noisetexture = lerp(noisetexture, nTop, blendNormal.x);
    noisetexture = lerp(noisetexture, nSide2, blendNormal.y);

	// distance influencer position to world position
	float3 dis0 = distance(_Position0, IN.worldPos);
	float3 dis1 = distance(_Position1, IN.worldPos);
	float3 dis2 = distance(_Position2, IN.worldPos);
	float3 dis3 = distance(_Position3, IN.worldPos);
	float3 dis4 = distance(_Position4, IN.worldPos);
	float3 dis5 = distance(_Position5, IN.worldPos);
	float3 dis6 = distance(_Position6, IN.worldPos);
	float3 dis7 = distance(_Position7, IN.worldPos);
	float3 dis8 = distance(_Position8, IN.worldPos);
	float3 dis9 = distance(_Position9, IN.worldPos);

	float3 minimum = min10(dis0, dis1, dis2, dis3, dis4, dis5, dis6, dis7, dis8, dis9);


	float3 sphere = 1 - saturate(minimum / _Radius);
 
	float3 sphereNoise = noisetexture.r * sphere;

	float3 DissolveLine = step(sphereNoise - _DisLineWidth, _DisAmount) * step(_DisAmount,sphereNoise) ; // line between two textures
	DissolveLine *= _DisLineColor; // color the line
	
	float3 primaryTex = (step(sphereNoise - _DisLineWidth,_DisAmount) * c.rgb);
	float3 secondaryTex = (step(_DisAmount, sphereNoise) * c2.rgb);
	float3 resultTex = primaryTex + secondaryTex + DissolveLine;
    o.Albedo = resultTex;

	float3 primaryAlpha = (step(sphereNoise - _DisLineWidth, _DisAmount) * c.a);
	float3 secondaryAlpha = (step(_DisAmount, sphereNoise) * c2.a);
	float3 resultAlpha = primaryAlpha + secondaryAlpha + DissolveLine;

	o.Emission = DissolveLine;
	o.Alpha = 0;
   
}
ENDCG
 
    }
 
    Fallback "Diffuse"
}