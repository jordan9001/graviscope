Shader "Hidden/FadeZ"
{
	Properties
	{
        _Color ("Main Color", Color) = (0, 1, 0, 1) //Color
		_Seed ("Seed", Float) = 13.37 // random seed
		_Jitter ("Jitter", Float) = 0.1 // jitter
		_ZJitter ("Z Jitter", Float) = 0.06 // Z jitter
		_JitterF ("Jitter Speed", Float) = 0.0000012 // Jitter flash rate
	}
	SubShader
	{		
		ZWrite Off
		ZTest Always
		Blend One One

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 projPos : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			float _Seed;
			float _Jitter;
			float _ZJitter;
			float _JitterF;

			float randoff(float co)
			{
				return frac(sin((_Time[0] * _JitterF) * dot(float2(co, _Seed) ,float2(12.9898,78.233))) * 43758.5453) - 0.5;
				//return frac(sin(dot(float2(co, _Seed) ,float2(12.9898,78.233))) * 43758.5453);
			}

			v2f vert (appdata v)
			{
				v2f o;
				float4 vrtx = v.vertex;
				
				vrtx.x += (randoff(vrtx.x + vrtx.y) * _Jitter);
				vrtx.y += (randoff(vrtx.x - vrtx.y) * _Jitter);
				vrtx.z += (randoff(vrtx.z + vrtx.y + vrtx.x) * _ZJitter);

				o.vertex = UnityObjectToClipPos(vrtx);
				o.projPos = ComputeScreenPos(vrtx);
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
