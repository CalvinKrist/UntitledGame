Shader "Untitled/OutlineShader" {
	Properties {
		_MainTex("Texture", 2D) = "white" {}
		_Color("Outline Color", Color) = (1, 1, 1, 1)
		_Width("Outline Width", int) = 1
	}
	
	SubShader {
	
		Cull Off
		Blend One OneMinusSrcAlpha
		
		Pass {
			CGPROGRAM
			
			#pragma vertex vertexFunc
			#pragma fragment fragmentFunc
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			
			struct v2f {
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
			};
			
			v2f vertexFunc(appdata_base v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				
				return o;
			}
			
			fixed4 _Color;
			fixed _Width;
			float4 _MainTex_TexelSize;
			
			fixed4 fragmentFunc(v2f i) : COLOR {
				half4 c = tex2D(_MainTex, i.uv);
				c.rgb *= c.a;
				
				half4 outlineC = _Color;
				outlineC.a *= ceil(c.a);
				outlineC.rgb *= outlineC.a;
				
				fixed yMag = _Width * _MainTex_TexelSize.y;
				fixed xMag = _Width * _MainTex_TexelSize.x;
				
				fixed scaledYMag = yMag * 1.4142;
				fixed scaledXMag = xMag * 1.4142;
				
				fixed ua = tex2D(_MainTex, i.uv + fixed2(0, yMag)).a;
				fixed da = tex2D(_MainTex, i.uv - fixed2(0, yMag)).a;
				fixed ra = tex2D(_MainTex, i.uv + fixed2(xMag, 0)).a;
				fixed la = tex2D(_MainTex, i.uv - fixed2(xMag, 0)).a;
				
				fixed ul = tex2D(_MainTex, i.uv + fixed2(-scaledXMag, scaledYMag)).a;
				fixed ur = tex2D(_MainTex, i.uv + fixed2(scaledXMag, scaledYMag)).a;
				fixed ll = tex2D(_MainTex, i.uv + fixed2(-scaledXMag, -scaledYMag)).a;
				fixed lr = tex2D(_MainTex, i.uv + fixed2(scaledXMag, -scaledYMag)).a;
				
				return lerp(outlineC, c, ceil(ua * da * ra * la * ul * ur * ll * lr));
			}
			
			ENDCG
		}
	}
}
