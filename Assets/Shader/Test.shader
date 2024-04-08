Shader "Unlit/Test"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _yval ("Float", float) = 0
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _yval;

            //code from "bgolus" on the unity forums
            float3 ClipToWorldPos(float4 clipPos)
            {
                #ifdef UNITY_REVERSED_Z
                    // unity_CameraInvProjection always in OpenGL matrix form
                    // that doesn't match the current view matrix used to calculate the clip space
 
                    // transform clip space into normalized device coordinates
                    float3 ndc = clipPos.xyz / clipPos.w;
 
                    // convert ndc's depth from 1.0 near to 0.0 far to OpenGL style -1.0 near to 1.0 far
                    ndc = float3(ndc.x, ndc.y * _ProjectionParams.x, (1.0 - ndc.z) * 2.0 - 1.0);
 
                    // transform back into clip space and apply inverse projection matrix
                    float3 viewPos =  mul(unity_CameraInvProjection, float4(ndc * clipPos.w, clipPos.w));
                #else
                    // using OpenGL, unity_CameraInvProjection matches view matrix
                    float3 viewPos = mul(unity_CameraInvProjection, clipPos);
                #endif
 
                // transform from view to world space
                return mul(unity_MatrixInvV, float4(viewPos, 1.0)).xyz;
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
                float4 clip = UnityObjectToClipPos(v.vertex); 
                o.vertex = clip;//float4(ClipToWorldPos(clip).xyz, 1);
                o.uv = v.uv;
                return o;
            }

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
            float2 scale(float2 uv, float scale){
	            return round(uv.xy * scale) / scale;
	        }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture


     
                float wy = 1 - (_yval * 0.05 + 0.1 + i.uv.y);
                wy = step(0, wy) * pow(wy, 1);
                float inten = step(0.01, wy) + pow(wy, 0.5);
                float2 dUV;// distorted UVs

                int mask = step(1.5, wy);
                float2 sUV = float2(i.uv.x, wy) * 8;//scale(, 100);
                float noise = fractal_noise(sUV.xy + _Time.y);
                noise = noise * inten * 0.01;
                dUV = i.uv.xy + noise;

                fixed4 col = tex2D(_MainTex, dUV);
                //col = col * noise;
                //return col + wy;//
                return col;// - pow(noise, 2) * wy * 100;
            }
            ENDCG
        }
    }
}
