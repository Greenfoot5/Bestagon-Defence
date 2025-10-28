// Draws a ring of a certain radius

Shader "Custom/Ring"
{
    Properties
    {
        _RingColor ( "Ring Color", Color ) = (1, 1, 1, 1)
        
        _RingRadius ( "Ring Radius", Range(0, 1) ) = .1

        // Unity complains if the shader has no texture input when it's used in an Image component
        [NoScaleOffset] _MainTex ( "Texture", 2D ) = "white" {}
    }
    SubShader
    {
        // Set up for transparent rendering
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            /**
             * \brief App to Vertex
             */
            struct a2v {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            /**
             * \brief Vertex To Fragment
             */
            struct v2f {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };
            
            // PROPERTIES
            float4 _RingColor;
            float _RingRadius;
            
            /**
             * \brief Only passes data to the fragment shader
             * \param i 
             * \return vert
             */
            v2f vert ( const a2v i )
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(i.vertex);
                o.uv = i.uv;
                o.color = i.color;
                return o;
            }
            
            /**
             * \brief Draws the circle itself
             * \param i input
             * \return A fragment
             */
            fixed4 frag ( v2f i ) : SV_Target
            {
                float4 color = i.color * _RingColor;
            
                // center uv (0, 1) -> (-1, 1)
                i.uv = ( i.uv - .5 ) * 2;

                const float d = distance(i.uv, 0);
                
                // radius based on distance in UV
                if (d > 1 || d < (1 - _RingRadius))
                    color.a = 0;
                
                // final multiply
                return color * i.color;
            }
        
            ENDCG
        }
    }
}