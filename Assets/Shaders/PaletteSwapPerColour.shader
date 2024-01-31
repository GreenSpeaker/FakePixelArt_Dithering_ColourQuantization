Shader "Custom/PaletteSwapEachColour"
{
    Properties
    {        
        _MainTex ("Texture", 2D) = "white" {}
        _PaletteRed ("Palette Red", 2D) = "white" {}
        _PaletteGreen ("Palette Green", 2D) = "white" {}
        _PaletteBlue ("Palette Blue", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _PaletteRed;
            sampler2D _PaletteGreen;
            sampler2D _PaletteBlue;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                
                col.r = saturate(tex2D(_PaletteRed, float2(col.r, 0))).r;
                col.g = saturate(tex2D(_PaletteGreen, float2(col.g, 0))).g;
                col.b = saturate(tex2D(_PaletteBlue, float2(col.b, 0))).b;

                return col;
            }
            ENDCG
        }
    }
}
