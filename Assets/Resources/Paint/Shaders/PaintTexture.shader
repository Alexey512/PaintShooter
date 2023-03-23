Shader "Paint/PaintTexture" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_BumpTex("Normal", 2D) = "bump" {}
		_BumpPower("Normal Scale", Range(.001,10)) = 1.0

		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0

		_StrokeColor1("Stroke Color 1", Color) = (1,.5,0,1)
		_StrokeColor2("Stroke Color 2", Color) = (1,0,0,1)
		_StrokeColor3("Stroke Color 3", Color) = (1,0,0,1)
		_StrokeColor4("Stroke Color 4", Color) = (1,0,0,1)

		_StrokeTex("Stroke Texture", 2D) = "black" {}
		_StrokeTileNormalTex("Stroke Normal", 2D) = "bump" {}
		_StrokeTileBump("Stroke Normal Scale", Range(0.001,10)) = 1.0

		_StrokeGlossiness("Stroke Smoothness", Range(0,1)) = 0.8
		_StrokeMetallic("Stroke Metallic", Range(0,1)) = 0.0
		_StrokeEdgeBump("Stroke Edge Scale", Range(0.001,10)) = 1.0
		_StrokeEdgeBumpWidth("Stroke Edge Width", Range(0,10)) = 1.0
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM

#pragma surface surf Standard fullforwardshadows addshadow
#pragma target 3.0

		static const float _Clip = 0.5;

		sampler2D _MainTex;
		sampler2D _BumpTex;
		sampler2D _StrokeTex;
		sampler2D _StrokeTileNormalTex;
		sampler2D _WorldTangentTex;
		sampler2D _WorldBinormalTex;

		float _StrokeEdgeBump;
		float _StrokeEdgeBumpWidth;
		float _StrokeTileBump;

		fixed4 _Color;
		fixed4 _StrokeColor1;
		fixed4 _StrokeColor2;
		fixed4 _StrokeColor3;
		fixed4 _StrokeColor4;

		half _BumpPower;
		half _Glossiness;
		half _Metallic;
		half _StrokeGlossiness;
		half _StrokeMetallic;

		float4 _StrokeTex_TexelSize;
		float4 _BumpTex_ST;
		float4 _StrokeTileNormalTex_ST;


	struct Input {
		float2 uv_MainTex;
		float2 uv2_StrokeTex;
		float3 worldNormal;
		float3 worldTangent;
		float3 worldBinormal;
		float3 worldPos;
		INTERNAL_DATA
	};

	float3x3 cotangent_frame(float3 N, float3 p, float2 uv)
	{
		// get edge vectors of the pixel triangle
		float3 dp1 = ddx(p);
		float3 dp2 = ddy(p);
		float2 duv1 = ddx(uv);
		float2 duv2 = ddy(uv);

		// solve the linear system
		float3 dp2perp = cross(dp2, N);
		float3 dp1perp = cross(N, dp1);
		float3 T = dp2perp * duv1.x + dp1perp * duv2.x;
		float3 B = dp2perp * duv1.y + dp1perp * duv2.y;

		// construct a scale-invariant frame 
		float invmax = rsqrt(max(dot(T,T), dot(B,B)));
		float3 TinvMax = normalize(T * invmax);
		float3 BinvMax = normalize(B * invmax);
		return float3x3(float3(TinvMax.x, BinvMax.x, N.x), float3(TinvMax.y, BinvMax.y, N.y), float3(TinvMax.z, BinvMax.z, N.z));
		//return float3x3( TinvMax, BinvMax, N );
	}

	half3 perturb_normal(float3 localNormal, float3 N, float3 V, float2 uv)
	{
		// assume N, the interpolated vertex normal and 
		// V, the view vector (vertex to eye)
		float3x3 TBN = cotangent_frame(N, -V, uv);
		return normalize(mul(TBN, localNormal));
	}

	void surf(Input IN, inout SurfaceOutputStandard o) {

		// Sample Stroke map texture with offsets
		float4 StrokeSDF = tex2D(_StrokeTex, IN.uv2_StrokeTex);
		float4 StrokeSDFx = tex2D(_StrokeTex, IN.uv2_StrokeTex + float2(_StrokeTex_TexelSize.x,0));
		float4 StrokeSDFy = tex2D(_StrokeTex, IN.uv2_StrokeTex + float2(0,_StrokeTex_TexelSize.y));

		// Use ddx ddy to figure out a max clip amount to keep edge aliasing at bay when viewing from extreme angles or distances
		half StrokeDDX = length(ddx(IN.uv2_StrokeTex * _StrokeTex_TexelSize.zw));
		half StrokeDDY = length(ddy(IN.uv2_StrokeTex * _StrokeTex_TexelSize.zw));
		half clipDist = sqrt(StrokeDDX * StrokeDDX + StrokeDDY * StrokeDDY);
		half clipDistHard = max(clipDist * 0.01, 0.01);
		half clipDistSoft = 0.01 * _StrokeEdgeBumpWidth;

		// Smoothstep to make a soft mask for the Strokes
		float4 StrokeMask = smoothstep((_Clip - 0.01) - clipDistHard, (_Clip - 0.01) + clipDistHard, StrokeSDF);
		float StrokeMaskTotal = max(max(StrokeMask.x, StrokeMask.y), max(StrokeMask.z, StrokeMask.w));

		// Smoothstep to make the edge bump mask for the Strokes
		float4 StrokeMaskInside = smoothstep(_Clip - clipDistSoft, _Clip + clipDistSoft, StrokeSDF);
		StrokeMaskInside = max(max(StrokeMaskInside.x, StrokeMaskInside.y), max(StrokeMaskInside.z, StrokeMaskInside.w));

		// Create normal offset for each Stroke channel
		float4 offsetStrokeX = StrokeSDF - StrokeSDFx;
		float4 offsetStrokeY = StrokeSDF - StrokeSDFy;

		// Combine all normal offsets into single offset
		float2 offsetStroke = lerp(float2(offsetStrokeX.x,offsetStrokeY.x), float2(offsetStrokeX.y,offsetStrokeY.y), StrokeMask.y);
		offsetStroke = lerp(offsetStroke, float2(offsetStrokeX.z,offsetStrokeY.z), StrokeMask.z);
		offsetStroke = lerp(offsetStroke, float2(offsetStrokeX.w,offsetStrokeY.w), StrokeMask.w);
		offsetStroke = normalize(float3(offsetStroke, 0.0001)).xy; // Normalize to ensure parity between texture sizes
		offsetStroke = offsetStroke * (1.0 - StrokeMaskInside) * _StrokeEdgeBump;

		// Add some extra bump over the Stroke areas
		float2 StrokeTileNormalTex = tex2D(_StrokeTileNormalTex, TRANSFORM_TEX(IN.uv2_StrokeTex, _StrokeTileNormalTex) * 10.0).xy;
		//offsetStroke += (StrokeTileNormalTex.xy - 0.5) * _StrokeTileBump  * 0.2;
		offsetStroke += (StrokeTileNormalTex.xy - 0.5) * _StrokeTileBump;

		// Create the world normal of the Strokes
#if 0
		// Use tangentless technique to get world normals
		float3 worldNormal = WorldNormalVector(IN, float3(0,0,1));
		float3 offsetStrokeLocal2 = normalize(float3(offsetStroke, sqrt(1.0 - saturate(dot(offsetStroke, offsetStroke)))));
		float3 offsetStrokeWorld = perturb_normal(offsetStrokeLocal2, worldNormal, normalize(IN.worldPos - _WorldSpaceCameraPos), IN.uv2_StrokeTex);
#else
		// Sample the world tangent and binormal textures for texcoord1 (the second uv channel)
		// you could skip the binormal texture and cross the vertex normal with the tangent texture to get the bitangent
		float3 worldTangentTex = tex2D(_WorldTangentTex, IN.uv2_StrokeTex).xyz * 2.0 - 1.0;
		float3 worldBinormalTex = tex2D(_WorldBinormalTex, IN.uv2_StrokeTex).xyz * 2.0 - 1.0;

		// Create the world normal of the Strokes
		float3 offsetStrokeWorld = offsetStroke.x * worldTangentTex + offsetStroke.y * worldBinormalTex;
#endif

		// Get the tangent and binormal for the texcoord0 (this is just the actual tangent and binormal that comes in from the vertex shader)
		float3 worldTangent = WorldNormalVector(IN, float3(1,0,0));
		float3 worldBinormal = WorldNormalVector(IN, float3(0,1,0));

		// Convert the Stroke world normal to tangent normal for texcood0
		float2 offsetStrokeLocal = 0;
		offsetStrokeLocal.x = dot(worldTangent, offsetStrokeWorld);
		offsetStrokeLocal.y = dot(worldBinormal, offsetStrokeWorld);

		// sample the normal map from the main material
		float4 normalMap = tex2D(_BumpTex, TRANSFORM_TEX(IN.uv_MainTex, _BumpTex));
		normalMap.xyz = UnpackNormal(normalMap);
		normalMap.z = normalMap.z / _BumpPower;

		float3 tanNormal = normalMap.xyz;

		// Add the Stroke normal to the tangent normal
		tanNormal.xy += offsetStrokeLocal * StrokeMaskTotal;
		tanNormal = normalize(tanNormal);

		// Albedo comes from a texture tinted by color
		float4 MainTex = tex2D(_MainTex, IN.uv_MainTex);
		fixed4 c = MainTex * _Color;

		// Lerp the color with the Stroke colors based on the Stroke mask channels
		c.xyz = lerp(c.xyz, _StrokeColor1.xyz, StrokeMask.x);
		c.xyz = lerp(c.xyz, _StrokeColor2.xyz, StrokeMask.y);
		c.xyz = lerp(c.xyz, _StrokeColor3.xyz, StrokeMask.z);
		c.xyz = lerp(c.xyz, _StrokeColor4.xyz, StrokeMask.w);

		o.Albedo = c.rgb;
		o.Normal = tanNormal;
		o.Metallic = lerp(_Metallic, _StrokeMetallic, StrokeMaskTotal);
		o.Smoothness = lerp(_Glossiness, _StrokeGlossiness, StrokeMaskTotal);
		o.Alpha = c.a;

	}
	ENDCG
	}
	FallBack "Diffuse"
}
