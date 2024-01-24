Shader "Custom/Dithering"
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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;

            float _Spread;

            int _ColourRange;

            sampler2D _PaletteTexture;

            static const int bayer2[2 * 2] = {
                0, 2,
                3, 1
            };

            static const int bayer4[4 * 4] = {
                0, 8, 2, 10,
                12, 4, 14, 6,
                3, 11, 1, 9,
                15, 7, 13, 5
            };

            static const int bayer8[8 * 8] = {
                0, 32, 8, 40, 2, 34, 10, 42,
                48, 16, 56, 24, 50, 18, 58, 26,  
                12, 44,  4, 36, 14, 46,  6, 38, 
                60, 28, 52, 20, 62, 30, 54, 22,  
                3, 35, 11, 43,  1, 33,  9, 41,  
                51, 19, 59, 27, 49, 17, 57, 25, 
                15, 47,  7, 39, 13, 45,  5, 37, 
                63, 31, 55, 23, 61, 29, 53, 21
            };

            int GetThreshMapValue(int x, int y)
            {
                return float(bayer4 [(y * 4) + x]);
            }

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                int xPos = i.uv.x * _MainTex_TexelSize.z;
                int yPos = i.uv.y * _MainTex_TexelSize.w;

                xPos = xPos % 4;
                yPos = yPos % 4; 

                float M = GetThreshMapValue(xPos, yPos);
                M = M * (1.0f/(16.0f));
                M = M - 0.5f;

                // sample the texture
                fixed4 texCol = tex2D(_MainTex, i.uv);
                
                fixed4 finalCol = fixed4(0, 0, 0, 1);
                finalCol.r = texCol.r + _Spread * M;
                finalCol.g = texCol.g + _Spread * M;
                finalCol.b = texCol.b + _Spread * M;

                int numColours = _ColourRange;

                finalCol.r = floor((finalCol.r * (numColours - 1)) + 0.5f) / (numColours - 1);
                finalCol.g = floor((finalCol.g * (numColours - 1)) + 0.5f) / (numColours - 1);
                finalCol.b = floor((finalCol.b * (numColours - 1)) + 0.5f) / (numColours - 1);

                float greyScale = (finalCol.r + finalCol.g + finalCol.b) /3;

                finalCol.rgb = saturate(greyScale);

                finalCol.rgb = tex2D(_PaletteTexture, float2(greyScale, 0));
            
                return finalCol;
            }
            ENDCG
        }
    }
}
