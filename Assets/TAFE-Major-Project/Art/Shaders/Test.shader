Shader "Unlit/Test"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorA("Color A", Color) = (0, 0, 0, 0)
        _ColorB("Color B", Color) = (1, 1, 1, 1)
    }
        SubShader
        {
            Tags 
            { 
                "RenderType" = "Opaque"
                "Queue" = "Transparent"
            }
            LOD 100

            Pass
            {
                //Cull Off
                ZWrite Off
                //ZTest Always
                Blend One One
                //Blend DstColor Zero
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            #define PI 3.141592653589

            float4 _ColorA;
            float4 _ColorB;

            fixed4 frag(v2f i) : SV_Target
            {

                float xOffset = cos(i.uv.x * PI * 4 * 8) * 0.01;

                float t = cos((i.uv.y + xOffset - _Time.y * 0.1) * PI * 4 * 5) * 0.5 + 0.5;



                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                //return col;
                return lerp(_ColorA, _ColorB, t);
            }
            ENDCG
        }
    }
}
