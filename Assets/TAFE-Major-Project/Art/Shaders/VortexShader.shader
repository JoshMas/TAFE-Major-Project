Shader "Unlit/VortexShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _WarpStrength("Warp Strength", Float) = 10.0
        _WarpSize("Warp Size", Range(0.1, 1)) = 0.5
        _WarpSmoothing("Warp Smoothing", Range(0, 1)) = 0.05
    }
        SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "Queue" = "Transparent"
        }

        GrabPass{}

        Pass
        {
            //Cull Off
            //ZWrite Off
            //ZTest Always
            //Blend One One
            //Blend DstColor Zero

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 uvgrab : TEXCOORD1;
                float3 normal : TEXCOORD2;
                float3 wPos : TEXCOORD3;
            };


            sampler2D _MainTex;
            sampler2D _GrabTexture;
            float4 _MainTex_TexelSize, _MainTex_ST;
            float _WarpStrength, _WarpSize, _WarpSmoothing;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                fixed4 vertexUV = o.vertex;

                #if UNITY_UV_STARTS_AT_TOP
                vertexUV.y *= -sign(_MainTex_TexelSize.y);
                #endif

                o.uvgrab.xy = (float2(vertexUV.x, vertexUV.y) + vertexUV.w) * 0.5;
                o.uvgrab.zw = vertexUV.zw;

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.wPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 V = normalize(_WorldSpaceCameraPos - i.wPos);
                fixed3 N = i.normal;
                fixed fresnel = pow(dot(V, N), 6);
                // sample the texture
                i.uvgrab.x -= N.x * fresnel * _WarpStrength;
                i.uvgrab.y -= N.y * fresnel * _WarpStrength;
                fixed4 col = tex2Dproj(_GrabTexture, i.uvgrab);
                float a = _WarpSmoothing / _WarpSize;
                float warpBoundary = 1 - smoothstep(_WarpSize - a, _WarpSize + a, fresnel);
                return warpBoundary * col;
            }
            ENDCG
        }
    }
}
