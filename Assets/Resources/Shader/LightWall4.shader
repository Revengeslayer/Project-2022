Shader "LightWall/RimLightingTranslucent"
{
    Properties
    {
        _Text("(Texture)",2D) = "White"{}
        _Color("Color", Color) = (1,1,1,1)
        _AlphaRange("Alpha Range",Range(0,1)) = 0
        _RimColor("Rim Color",Color) = (1,1,1,1)
    }
        SubShader
    {
        Pass
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "IngoreProject" = "True"}
            LOD 200
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _NormalMap;
            sampler2D _Text;
            float _AlphaRange;
            float4 _RimColor;
            fixed4 _Color;

            struct a2v
            {
                float2 uv : TEXCOORD0;
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 normalDir : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };

            v2f vert(a2v v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld,v.vertex).xyz;
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f v) : COLOR
            {
                float3 viewDir = normalize(_WorldSpaceCameraPos - v.worldPos);
                float normalDotViewDir = saturate(dot(normalize(v.normalDir),viewDir));
                fixed4 pixelColor = tex2D(_Text, v.uv);
                fixed3 diffuse = normalDotViewDir * pixelColor *_Color;
                return fixed4(diffuse + _RimColor ,(1 - normalDotViewDir) * (1 - _AlphaRange) + _AlphaRange);
            }
            ENDCG
        }
    }
        FallBack "Diffuse"
}