Shader "taecg/Dissolve"
{
	Properties
	{
		[Header(Base)]
		_MainTex("MainTex", 2D) = "black" {}
		
		[Header(Dissolve)]
		[Toggle]_DissolveEnabled("Dissolve Enabled",int) = 0
		_DissolveTex("DissolveTex",2D) = "white"{}
		[NoScaleOffset]_RampTex("RampTex(RGB)",2D)= "white"{}
		_Clip("Clip",Range(0,1)) = 0
	}
	
	SubShader
	{
		Tags { "Queue"="Geometry" "RenderPipeline" = "UniversalPipeline" }

		Pass
		{
			HLSLPROGRAM
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.5
			#pragma multi_compile_local _ _DISSOLVEENABLED_ON
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			
			struct Attributes
			{
				float4 vertex : POSITION;
				float2 texcoord0 : TEXCOORD0;
			};
			
			struct Varyings
			{
				float4 positionCS 	: SV_POSITION;
				float4 uv 			: TEXCOORD0;
			};

			CBUFFER_START(UnityPerMaterial)
				float4 _MainTex_ST,_DissolveTex_ST;
				half _Clip;
			CBUFFER_END
			TEXTURE2D (_MainTex);SAMPLER(sampler_MainTex);
			TEXTURE2D (_DissolveTex);SAMPLER(sampler_DissolveTex);
			TEXTURE2D (_RampTex);SAMPLER(sampler_RampTex);
			
			Varyings vert(Attributes v)
			{
				Varyings o = (Varyings)0;

				o.positionCS = TransformObjectToHClip(v.vertex.xyz);
				o.uv.xy = TRANSFORM_TEX(v.texcoord0.xy, _MainTex);
				o.uv.zw = TRANSFORM_TEX(v.texcoord0.xy, _DissolveTex);

				return o;
			}
			
			half4 frag(Varyings i) : SV_Target
			{
				half4 c;
				half4 mainTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv.xy);
				c = mainTex;

				#if _DISSOLVEENABLED_ON
					half4 dissolveTex=SAMPLE_TEXTURE2D(_DissolveTex,sampler_DissolveTex,i.uv.zw);
					clip(dissolveTex.r-_Clip);
					half dissolveValue = saturate((dissolveTex.r-_Clip)/(_Clip+0.1-_Clip));
					half4 rampTex = SAMPLE_TEXTURE2D(_RampTex,sampler_DissolveTex,dissolveValue);
					c += rampTex;
				#endif

				return c;
			}
			ENDHLSL
		}
	}
}
