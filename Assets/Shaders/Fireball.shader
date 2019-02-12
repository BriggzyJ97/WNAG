Shader "Custom/Fireball" {
    Properties {
        _RampTex ("Ramp texture", 2D) = "white" {}
        _NoiseTex ("Noise texture",2D) = "grey" {}
		_MainTint ("Main Tint", Color) = (1,1,1,1)
        _RampVal ("Ramp offset", Range(-0.5, 0.5)) = 0
        _Amplitude ("Amplitude factor", Range(0, 0.03)) = 0.01
		_ScrollXSpeed ("X Scroll Speed", Range(0,10)) = 0
		_ScrollYSpeed ("Y Scroll Speed", Range(0,10)) = 0
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
       
        CGPROGRAM
        #pragma surface surf Lambert vertex:vert
 
        sampler2D _NoiseTex;
        sampler2D _RampTex;
		fixed4 _MainTint;
        fixed _RampVal;
        fixed _Amplitude;
		fixed _ScrollXSpeed;
		fixed _ScrollYSpeed;
 
        struct Input {
            float2 uv_NoiseTex;
        };
 
        void vert(inout appdata_full v) {
            half noiseVal = tex2Dlod(_NoiseTex, float4(v.texcoord.xy, 0, 0)).r;
            v.vertex.xyz += v.normal * sin(_Time.w + noiseVal * 100)* _Amplitude;
        }
 
        void surf (Input IN, inout SurfaceOutput o) {
			fixed2 scrolledUV = IN.uv_NoiseTex;
			fixed xScrollValue = _ScrollXSpeed*_Time;
			fixed yScrollValue = _ScrollYSpeed*_Time;
			scrolledUV += fixed2(xScrollValue, yScrollValue);
            half noiseVal = tex2D(_NoiseTex, scrolledUV).r + (sin(_Time.y)) / 15;
            half4 color = tex2D(_RampTex, float2(saturate(_RampVal + noiseVal), 0.5));
            o.Albedo = color.rgb*_MainTint;
            o.Emission = color.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}