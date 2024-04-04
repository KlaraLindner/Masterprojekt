// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/DataSetToTexture"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DataSet ("Data Set", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // Texture properties
            uniform sampler2D _MainTex;
            uniform float4 _MainTex_ST;
            // Data set properties
            uniform sampler2D _DataSet;
            uniform float4 _DataSet_ST;

            struct vertexInput
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct vertexOutput
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            vertexOutput vert(vertexInput input)
            {
                vertexOutput output;
                output.pos = UnityObjectToClipPos(input.vertex);
                output.uv = input.uv;
                return output;
            }

            float4 frag(vertexOutput input) : SV_Target
            {
                float2 uv = input.uv;
                // Sample the dataset texture
                float4 dataSetColor = tex2D(_DataSet, uv);
                // Sample the main texture (if needed)
                float4 mainTextureColor = tex2D(_MainTex, uv);
                // Combine colors as needed
                return dataSetColor * mainTextureColor;
            }
            ENDCG
        }
    }
}
