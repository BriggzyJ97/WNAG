﻿Shader "Custom/edgeDetectionShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_SensitivityDepth ("SensitivityDepth", Range(0,5)) = 3.75
		_SensitivityNormals ("SensitivityNormals", Range(0,5)) = 0.82
		_SampleDistance ("SampleDistance", Range(0,2)) = 1
		_Falloff("Falloff", Range(0,100)) = 10.0
			  _EmissionMap ("Emission Map", 2D) =  "black"{}
  [HDR] _EmissionColor ("Emission Color", Color) = (0,0,0)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows finalcolor:edgecolor

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		uniform float4 _MainTex_TexelSize;
		uniform float4 _CameraDepthNormalsTexture_TexelSize;
		sampler2D _CameraDepthNormalsTexture;

		struct Input {
			float2 uv_MainTex;
			float4 screenPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		uniform half _SensitivityDepth;
		uniform half _SensitivityNormals;
		uniform half _SampleDistance;
		uniform half _Falloff;
		float4 _EmissionColor;
		sampler2D _EmissionMap;

		inline half CheckSame(half2 centerNormal, float centerDepth, half4 theSample)
		{
			half2 diff = abs(centerNormal - theSample.xy) * _SensitivityNormals;
			int isSameNormal = (diff.x + diff.y) *_SensitivityNormals < 0.1;

			float sampleDepth = DecodeFloatRG(theSample.zw);
			float zdiff = abs(centerDepth - sampleDepth);

			int isSameDepth = zdiff * _SensitivityDepth < 0.09 *centerDepth;

			return isSameNormal * isSameDepth ? 1.0 : 0.0;

		}

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		//UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		//UNITY_INSTANCING_BUFFER_END(Props)

		void edgecolor(Input IN, SurfaceOutputStandard o, inout fixed4 color)
		{
			float2 screenUV = IN.screenPos.xy/IN.screenPos.w;
			float sampleSizeX = _CameraDepthNormalsTexture_TexelSize.x;
			float sampleSizeY = _CameraDepthNormalsTexture_TexelSize.y;
			float2 _uv2 = screenUV + float2(-sampleSizeX,-sampleSizeY)*_SampleDistance;
			float2 _uv3 = screenUV + float2(+sampleSizeX,-sampleSizeY)*_SampleDistance;

			half4 center = tex2D(_CameraDepthNormalsTexture, screenUV);
			half4 sample1 = tex2D(_CameraDepthNormalsTexture, _uv2);
			half4 sample2 = tex2D(_CameraDepthNormalsTexture, _uv3);

			half2 centerNormal = center.xy;
			
			float centerDepth = DecodeFloatRG(center.zw);

			float d = clamp(centerDepth * _Falloff - 0.05, 0.0, 1.0);
			half4 depthFade = half4(d,d,d,1.0);

			half edge = 1.0;
			edge *= CheckSame (centerNormal, centerDepth, sample1);
			edge *= CheckSame (centerNormal, centerDepth, sample2);

			color = edge * color + (1.0 - edge) *depthFade * color;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			half4 emission = tex2D(_EmissionMap, IN.uv_MainTex)*_EmissionColor;
			o.Emission = o.Emission + emission.rgb;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
