Shader "Unlit/SkyShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
      Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha
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
                float4 wsp: TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            float rand (float2 uv) { 
                return frac(sin(dot(uv.xy, float2(12.9898, 78.233))) * 43758.5453123);
            }

            float noise (float2 uv) {
                float2 ipos = floor(uv);
                float2 fpos = frac(uv); 
                
                float o  = rand(ipos);
                float x  = rand(ipos + float2(1, 0));
                float y  = rand(ipos + float2(0, 1));
                float xy = rand(ipos + float2(1, 1));

                float2 smooth = smoothstep(0, 1, fpos);
                return lerp( lerp(o,  x, smooth.x), 
                                lerp(y, xy, smooth.x), smooth.y);
            }

            float fractal_noise (float2 uv) {
                float n = 0;
                // fractal noise is created by adding together "octaves" of a noise
                // an octave is another noise value that is half the amplitude and double the frequency of the previously added noise
                // below the uv is multiplied by a value double the previous. multiplying the uv changes the "frequency" or scale of the noise becuase it scales the underlying grid that is used to create the value noise
                // the noise result from each line is multiplied by a value half of the previous value to change the "amplitude" or intensity or just how much that noise contributes to the overall resulting fractal noise.

                n  = (1 / 2.0)  * noise( uv * 1);
                n += (1 / 4.0)  * noise( uv * 2); 
                n += (1 / 8.0)  * noise( uv * 4); 
                n += (1 / 16.0) * noise( uv * 8);
                
                return n;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                //Woldspace Position Calculation
                o.wsp = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            float2 scale(float2 uv, float res){
	            return round(uv * res) / res ;
	        }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                col = 0;

                float time =  (_Time.x * 1);
                float2 starUV = i.uv + _Time.x * 0.01;

                float2 uv = i.wsp + float2(0, -20);
                float d = length(uv);
                float angle = atan2(uv.y, uv.x) + 180;

                float2 pUV = float2((angle + time)/ UNITY_TWO_PI, pow(d, 0.5));
                pUV *= 10;
                float noise1 = rand(scale(pUV, 2) * 5);
                //noise1 *= frac(pUV.x * 100);
                //noise1 *= frac(pUV.y * 100);
                col = step(0.9, pow(noise1, 2));
                
                //test
                //col = frac(pUV.x * 15) * frac(pUV.y);
                return float4(col.xyz, 1);
            }
            ENDCG
        }
    }
}
