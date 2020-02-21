Shader "Custom/WindGrass" {
Properties {
// Surface shader parameters
_Color ("Color", Color) = (1,1,1,1)
_MainTex ("Albedo (RGB)", 2D) = "white" {}
_Glossiness ("Smoothness", Range(0,1)) = 0.5
_Metallic ("Metallic", Range(0,1)) = 0.0
 
// Wind effect parameteres
_WindFrecuency("Wind Frecuency",Range(0.001,100)) = 1
_WindStrength("Wind Strength", Range( 0, 2 )) = 0.3
_WindGustDistance("Distance between gusts",Range(0.001,50)) = .25
_WindDirection("Wind Direction", vector) = (1,0, 1,0)
 
}
SubShader {
Tags { "Queue"="Transparent"
"RenderType"="TransparentCutout"
}
LOD 200
 
CGPROGRAM
 
#pragma surface surf Standard vertex:vert alpha:fade
//#pragma target 3.0
 
sampler2D _MainTex;
 
struct Input {
float2 uv_MainTex;
};
 
half _Glossiness;
half _Metallic;
fixed4 _Color;
 
half _WindFrecuency;
half _WindGustDistance;
half _WindStrength;
float3 _WindDirection;
 
// our vert modification function
void vert( inout appdata_full v )
{
float4 localSpaceVertex = v.vertex;
// Takes the mesh's verts and turns it into a point in world space
// this is the equivalent of Transform.TransformPoint on the scripting side
float4 worldSpaceVertex = mul( unity_ObjectToWorld, localSpaceVertex );
 
// Height of the vertex in the range (0,1)
float height = (localSpaceVertex.y/2 + .5);
 
worldSpaceVertex.x += sin( _Time.x * _WindFrecuency + worldSpaceVertex.x * _WindGustDistance) * height * _WindStrength * _WindDirection.x;
worldSpaceVertex.z += sin( _Time.x * _WindFrecuency + worldSpaceVertex.z * _WindGustDistance) * height * _WindStrength * _WindDirection.z;
 
// takes the new modified position of the vert in world space and then puts it back in local space
v.vertex = mul( unity_WorldToObject, worldSpaceVertex );
 
}
 
void surf (Input IN, inout SurfaceOutputStandard o) {
// Albedo comes from a texture tinted by color
fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
o.Albedo = c.rgb;
// Metallic and smoothness come from slider variables
o.Metallic = _Metallic;
o.Smoothness = _Glossiness;
o.Alpha = c.a;
}
ENDCG
 
}
FallBack "Diffuse"
}