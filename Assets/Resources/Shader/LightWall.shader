Shader "LightWall/lightWall"
{
	Properties
	{
		_Text("(Texture)",2D) = "White"{}
		_Color("(Color)",color)=(1,1,1,1)
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			// #pragma Type MethodName  (基本宣告 型態 函式名稱)
			#pragma vertex vertexFunc
			#pragma fragment fragmentFunc

			// 固定寫法 去參考 UnityCG.cginc 的函式
			#include "UnityCG.cginc"

			// 看我的input有甚麼資料格式 目前只有兩個
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
			// 看我的output有甚麼資料格式 目前只有兩個 SV_POSITION(固定寫法 告訴系統我拿這個當基礎值)
			struct v2f
			{
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			//上層變數宣告 來此使用
			float4 _Color;
			sampler2D _Text;

			//將 appdata的資料型態 轉成 v2f 的資料型態
			v2f vertexFunc(appdata IN)
			{
				v2f OUT;
				OUT.position = UnityObjectToClipPos(IN.vertex);
				OUT.uv = IN.uv;

				return OUT;
			}

			///將 v2f 的資料型態 轉成 物件上的顏色
			fixed4 fragmentFunc(v2f IN) :SV_Target
			{
				fixed4 pixelColor = tex2D(_Text,IN.uv);
				pixelColor *= _Color;
				return pixelColor;
			}
			ENDCG
		}
	}
}
