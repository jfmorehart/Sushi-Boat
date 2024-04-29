Shader "Unlit/FishQuality"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _qual ("Quality0-1", Float) = 1
        _tint("Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull off
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _qual;
            float4 _tint;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                int mask = step(col.a, 0.2f);
                col = float4(0, 0, 0, 0) * mask + col * 1 - mask;
                col = (_qual + 0.3) * col + (1 - _qual) * _tint * (1 - mask);
                col = pow(col, min(1.4, min(_qual + 0.8, 2))); 
              
                //int rmask = step(0.5, _qual) * step(0.5, col.b);
                //col = rmask * float4(0.7, 0.6, 0.1, 1) + (1 - rmask) * col;
                //col *= 1 - mask;
                return col;
            }
            ENDCG
        }
    }
}
