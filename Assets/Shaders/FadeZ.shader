Shader "Hidden/FadeZ"
{
	Properties
	{
        _Color ("Main Color", Color) = (0, 1, 0, 1) //Color when not intersecting
	}
	SubShader
	{		
		ZWrite Off
		ZTest Always
		//Blend One One
		Blend One One

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			uniform sampler2D _CameraDepthTexture; //Depth Texture

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 projPos : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				// we are in 2d, just use z on the vert
				o.projPos = ComputeScreenPos(v.vertex);
				return o;
			}
			
			sampler2D _MainTex;
			float4 _Color;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = _Color;

				col *= (1-i.projPos.z);
				return col;
			}
			ENDCG
		}
	}
}
