// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Light"
{
    Properties
    {
        _MainTex ("Diffuse Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _ContrastFactor ("Contrast Factor", Float) = 1.0
		_ColorFactor ("Color Factor", Float) = 0.5
    }
     
    SubShader
    {
         Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "ForceNoShadowCasting"="True"
        }
         
        Pass
        {
            AlphaTest Greater 0.0 // Pixel with an alpha of 0 should be ignored
            Blend DstColor One // Keep black values
             
            CGPROGRAM
              
            #pragma vertex vert
            #pragma fragment frag
              
            #include "UnityCG.cginc"
              
            // User-specified properties
            uniform sampler2D _MainTex;
            uniform float4 _Color;
            uniform float _ContrastFactor;
              
            struct VertexInput
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
                float4 color : COLOR;
            };
              
            struct VertexOutput
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };
              
            VertexOutput vert(VertexInput input)
            {
                VertexOutput output;
                output.pos = UnityObjectToClipPos(input.vertex);
                output.uv = input.uv;
                output.color = input.color;
                 
                return output;
            }
              
            float4 frag(VertexOutput input) : COLOR
            {
                float4 diffuseColor = tex2D(_MainTex, input.uv);
                // Retrieve color from texture and multiply it by tint color and by sprite color
                // Multiply everything by texture alpha to emulate transparency
                diffuseColor.rgb = diffuseColor.rgb * _Color.rgb * input.color.rgb;
                diffuseColor.rgb *= diffuseColor.a * _Color.a * input.color.a;
                diffuseColor *= _ContrastFactor;
                 
                return float4(diffuseColor);
             }
          
             ENDCG
        }
		Pass
        {
            AlphaTest Greater 0.0 // Pixel with an alpha of 0 should be ignored
            Blend SrcAlpha One // Add colours to the previous pixels
 
            CGPROGRAM
              
            #pragma vertex vert
            #pragma fragment frag
              
            #include "UnityCG.cginc"
              
            // User-specified properties
            uniform sampler2D _MainTex;
            uniform float4 _Color;
            uniform float _ColorFactor;
              
            struct VertexInput
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
                float4 color : COLOR;
            };
              
            struct VertexOutput
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };
              
            VertexOutput vert(VertexInput input)
            {
                VertexOutput output;
                output.pos = UnityObjectToClipPos(input.vertex);
                output.uv = input.uv;
                output.color = input.color;
                 
                return output;
            }
              
            float4 frag(VertexOutput input) : COLOR
            {
                float4 diffuseColor = tex2D(_MainTex, input.uv);
                diffuseColor.rgb = _Color.rgb * diffuseColor.rgb * input.color.rgb * diffuseColor.a;
                diffuseColor *= _ColorFactor;
                diffuseColor.a = _Color.a * input.color.a;
 
                return float4(diffuseColor);
            }
          
            ENDCG
        }
    }
}