Shader "WaterDistortion"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            
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
                o.vertex = float4(ClipToWorldPos(clip).xyz, 1);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                //col *= 1;//i.vertex.y +1 ;
                return float4(i.uv.x, 1, 1, 1);
            }
            ENDCG
        }
    }
}
