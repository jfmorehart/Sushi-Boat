Shader "Unlit/CustomerShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} 
        _col("color", Color) = (1, 1, 1, 1)
        _rh("red height", Float) = 0.5
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
            float4 _col;
            float _rh;

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
                fixed4 tcol = tex2D(_MainTex, i.uv);
                _rh += sin(30 * _rh + i.uv.x * 30 + _Time.x) * 0.01;
                tcol *= (1-step(_rh, i.uv.y)) * _col + step(_rh, i.uv.y);
                return tcol;
            }
            ENDCG
        }
    }
}
