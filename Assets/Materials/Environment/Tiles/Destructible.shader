Shader "Custom/Destructible" {
	Properties {
		_OuterTex ("Outer Texture", 2D) = "white" {}
		_InnerTex ("Inner Texture", 2D) = "black" {}
		_MaskTex ("Mask", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Transparent" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _OuterTex;
		sampler2D _InnerTex;
		sampler2D _MaskTex;

		struct Input {
			float2 uv_OuterTex;
			float2 uv_InnerTex;
			float2 uv_MaskTex;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 mc = tex2D(_MaskTex, IN.uv_MaskTex);
			fixed4 c = lerp( tex2D(_InnerTex, IN.uv_InnerTex), tex2D (_OuterTex, IN.uv_OuterTex), mc.r);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
