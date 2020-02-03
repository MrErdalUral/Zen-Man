// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Shockwave"
{
	Properties
	{
		_Radius ("Inner Radius", Float) = 0 
		_Width ("Ring Width",Float) = 1
		_DistortionStrength ("DistortionStrength", Float) = 1
	}
	SubShader
	{
		Tags {"Queue"="Transparent" "RenderType"="Transparent" "PreviewType"="Plane" }
		LOD 100

		GrabPass { "_GrabTexture"}

		Pass
		{
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			//Blend One Zero
			Lighting Off
			Fog { Mode Off }
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
				half2 uv : TEXCOORD0;
				half2 uv_center : TEXCOORD1;
				float4 vertex : SV_POSITION;
				float4 obj_vertex : TEXCOORD2;
			};

			sampler2D _GrabTexture;
			half _Radius;
			half _Width;
			half _DistortionStrength;
		
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = ComputeGrabScreenPos(o.vertex);
				o.obj_vertex = v.vertex;
				o.uv_center = ComputeGrabScreenPos(UnityObjectToClipPos(float4(0, 0, 0, 1)));
				return o;
			}
		
			fixed4 frag (v2f i) : COLOR
			{
				half dist = length(i.obj_vertex.xy);
				if(dist > _Radius && dist < _Radius+_Width)
				{
					half shape = (_Radius + _Width * 0.5) - dist;
					shape = (_Width*_Width * 0.25) -  shape * shape;
					fixed4 result = tex2D(_GrabTexture, i.uv +  shape * (_DistortionStrength) * normalize(i.uv - i.uv_center));
					result.a = 1;
					return result;
				}
				else
				{
					fixed4 result = tex2D(_GrabTexture, i.uv);
					result.a = 0;
					return result; 
				}
			}
			ENDCG
		}
	}
}