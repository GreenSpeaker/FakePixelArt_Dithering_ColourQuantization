Shader "Custom/PaletteSwapFromGreyScale"
{
    Properties
    {        
        _MainTex ("Texture", 2D) = "white" {}
        _PaletteTexture ("Palette Texture", 2D) = "white" {}
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

            sampler2D _PaletteTexture;

            float GetGreyScaleFromColour(float3 colourRGB)
            {
                return (colourRGB.r + colourRGB.g + colourRGB.b)/3;
            }

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

                float greyScale = saturate(GetGreyScaleFromColour(col.rgb));

                col.rgb = tex2D(_PaletteTexture, float2(greyScale, 0));

                return col;
            }
            ENDCG
        }
    }
}