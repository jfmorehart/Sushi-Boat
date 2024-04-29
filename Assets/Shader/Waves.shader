Shader "Unlit/Waves"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _mcol ("Main Color", Color) = (0, 0, 1, 1)
        _off("timing offset", Float) = 0.5
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
            float4 _mcol;
            float _off;

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
                float speed = lerp(sin((_Time.y * 0.1) + _off + 0.3), 1, 0.9);
                float sterm =  0.05 * sin(i.uv.x * 40 + _Time.y * _off * speed);
                int alpha = step(i.uv.y - 0.5 - sin(_Time.y) * 0.1, sterm);
                float4 col = _mcol - sterm;
                col.a = 1;
                return col * alpha;
            }
            ENDCG
        }
    }
}
