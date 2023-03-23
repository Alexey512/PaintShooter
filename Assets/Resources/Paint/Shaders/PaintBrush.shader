Shader "Hidden/PaintBrush"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	
	CGINCLUDE
	
	#include "UnityCG.cginc"
	
	struct v2f
	{
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
	};

	sampler2D _MainTex;
	float4 _MainTex_TexelSize;

	sampler2D _WorldPosTex;
	sampler2D _worldTangentTex;
	sampler2D _worldBinormalTex;
	sampler2D _WorldNormalTex;

	sampler2D _LastStrokeTex;

	int _TotalStrokes;
	float4x4 _StrokeMatrix[10];
	float4 _StrokeScale[10];
	float4 _StrokeChannel[10];
	
	v2f vert (appdata_img v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord.xy;
		return o;
	}
	
	float4 frag(v2f i): COLOR
	{
		
		float4 currentStroke = tex2D(_LastStrokeTex, i.uv);
		float4 wpos = tex2D(_WorldPosTex, i.uv);
		
		for (int i = 0; i < _TotalStrokes; i++) {
			float3 pos = mul(_StrokeMatrix[i], float4(wpos.xyz,1)).xyz;

			if (pos.x > -0.5 && pos.x < 0.5 && pos.y > -0.5 && pos.y < 0.5 && pos.z > -0.5 && pos.z < 0.5) {
				float2 uv = saturate(pos.xz + 0.5);
				uv *= _StrokeScale[i].xy;
				uv += _StrokeScale[i].zw;
				
				float newStrokeTex = tex2D(_MainTex, uv).x;
				newStrokeTex = saturate(newStrokeTex - abs(pos.y) * abs(pos.y));
				currentStroke = min(currentStroke, 1.0 - newStrokeTex * ( 1.0 - _StrokeChannel[i]));
				currentStroke = max(currentStroke, newStrokeTex * _StrokeChannel[i]);
			}
		}

		return currentStroke * wpos.w;
	}
	
	float4 fragClear(v2f i): COLOR
	{
		return float4(0,0,0,0);
	}
	
	float4 fragBleed(v2f i): COLOR
	{
		
		float2 uv = i.uv;
		float4 worldPos = tex2D(_MainTex, uv);
		
		if( worldPos.w < 0.5 ) {
			worldPos += tex2D(_MainTex, uv + float2(-1,-1) * _MainTex_TexelSize.xy);
			worldPos += tex2D(_MainTex, uv + float2(-1,0) * _MainTex_TexelSize.xy);
			worldPos += tex2D(_MainTex, uv + float2(-1,1) * _MainTex_TexelSize.xy);
			
			worldPos += tex2D(_MainTex, uv + float2(0,-1) * _MainTex_TexelSize.xy);
			worldPos += tex2D(_MainTex, uv + float2(0,0) * _MainTex_TexelSize.xy);
			worldPos += tex2D(_MainTex, uv + float2(0,1) * _MainTex_TexelSize.xy);
			
			worldPos += tex2D(_MainTex, uv + float2(1,-1) * _MainTex_TexelSize.xy);
			worldPos += tex2D(_MainTex, uv + float2(1,0) * _MainTex_TexelSize.xy);
			worldPos += tex2D(_MainTex, uv + float2(1,1) * _MainTex_TexelSize.xy);
		
			worldPos.xyz *= 1.0 / max( worldPos.w, 0.00001 );
			worldPos.w = min( worldPos.w, 1.0 );
		}
		
		return worldPos;
	}
	
	
	float4 fragCompile(v2f i): COLOR
	{
		float4 strokeSDF = tex2D(_MainTex, i.uv);
		float4 strokeMask = smoothstep( 0.5 - 0.01, 0.5 + 0.01, strokeSDF);
		return strokeMask;
	}

	ENDCG
	
	SubShader
	{
		ZTest Off
		Cull Off
		ZWrite Off
		Fog { Mode off }

		//Pass 0 decal rendering pass
		Pass
		{
			Name "Brush"
			CGPROGRAM
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			ENDCG
		}
		
		//Pass 1 clear stroke map pass
		Pass
		{
			Name "Clear"
			CGPROGRAM
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex vert
			#pragma fragment fragClear
			#pragma target 3.0
			ENDCG
		}
		
		//Pass 2 bleed pass
		Pass
		{
			Name "Bleed"
			CGPROGRAM
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex vert
			#pragma fragment fragBleed
			#pragma target 3.0
			ENDCG
		}
		
		//Pass 3 compile pass
		Pass
		{
			Name "Compile"
			CGPROGRAM
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex vert
			#pragma fragment fragCompile
			#pragma target 3.0
			ENDCG
		}
	}
}
