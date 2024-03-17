Shader "Custom/UnlitSplatmapShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Splatmap ("Splatmap", 2D) = "white" {}
        _Texture1 ("Texture 1", 2D) = "white" {}
        _Texture2 ("Texture 2", 2D) = "white" {}
        _Texture3 ("Texture 3", 2D) = "white" {}
        _Texture4 ("Texture 4", 2D) = "white" {}
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
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
            sampler2D _Splatmap;
            sampler2D _Texture1;
            sampler2D _Texture2;
            sampler2D _Texture3;
            sampler2D _Texture4;
            
            float4 _MainTex_ST;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the splatmap
                fixed4 splat = tex2D(_Splatmap, i.uv);
                
                // Sample textures based on splat values
                fixed4 tex1 = tex2D(_Texture1, i.uv) * splat.r;
                fixed4 tex2 = tex2D(_Texture2, i.uv) * splat.g;
                fixed4 tex3 = tex2D(_Texture3, i.uv) * splat.b;
                fixed4 tex4 = tex2D(_Texture4, i.uv) * splat.a;
                
                // Blend textures together
                fixed4 finalColor = tex1 + tex2 + tex3 + tex4;
                
                // Apply main texture
                fixed4 mainTexColor = tex2D(_MainTex, i.uv);
                finalColor *= mainTexColor;
                
                return finalColor;
            }
            ENDCG
        }
    }
}
