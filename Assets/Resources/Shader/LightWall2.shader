Shader "LightWall/LightWall2"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _RimColor("Rim Color",Color) =(1,1,1,1)
        _RimPower("Rim Power",range(0,10))=2
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

           /* struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };*/

            struct v2f
            {
                float4 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 NdotV:COLOR;
            };

            sampler2D _MainTex;
            float4 _RimColor;
            float _RimPower;

            v2f vert (appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                float3 V = WorldSpaceViewDir(v.vertex);
                V = mul(unity_WorldToObject,V);
                o.NdotV.x = saturate(dot(v.normal, normalize(V)));
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                // sample the texture
                half4 col = tex2D(_MainTex, i.uv);
                // apply fog
                col.rgb += pow((1 - i.NdotV.x), _RimPower) * _RimColor.rgb;
                return col;
            }
            ENDCG
        }
    }
            FallBack "Diffuse"
}
