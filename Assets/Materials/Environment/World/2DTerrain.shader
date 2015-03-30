Shader "Custom/2DTerrain" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Control ("Control (RGB)", 2D) = "black" {}
		_Texture1 ("Texture 1", 2D) = "black" {}
		_Texture2 ("Texture 2", 2D) = "black" {}

	}
	SubShader {
		Tags { "RenderType"="Opaque"}
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows 

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _Control;
		sampler2D _Texture1;
		sampler2D _Texture2;

		struct Input {
			float2 uv_Control;
			float2 uv_Texture1;
			float2 uv_Texture2;
		};

		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard ou) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_Control, IN.uv_Control);
			fixed3 o = c.r * tex2D (_Texture1, IN.uv_Texture1).rgb;
			o += c.g * tex2D(_Texture2, IN.uv_Texture2).rgb;
			//o += tex2D(_Texture3, IN.uv_Texture).rgb * c.b;
			//o += tex2D(_Texture4, IN.uv_Texture).rgb * (1.0 - c.a);
			ou.Albedo = o.rgb;
			ou.Alpha = 0.0;
			ou.Metallic = 0;
			ou.Smoothness = 0;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
