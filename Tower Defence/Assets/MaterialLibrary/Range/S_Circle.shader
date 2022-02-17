// Draws a circle that gradients out in the middle of the quad

Shader "Unlit/Circle"
{
    Properties
    {
        _CircleColor ( "Circle Color", Color ) = (1, 1, 1, 1)
        
        _GlowColor ( "Glow Color", Color ) = (1, 1, 1, 1)
        _GlowRadius ( "Glow Radius", Float ) = 1
        _GlowSize ( "Glow Size", Float ) = 1

        // Unity complains if the shader has no texture input when it's used in an Image component
        _MainTex ( "Texture", 2D ) = "white" {}
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
             * \brief The input the shader gets
             */
            struct input {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            /**
             * \brief The output the shader gives
             */
            struct output {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };
            
            // PROPERTIES
            float4 circle_color;
            float4 glow_color;
            float glow_radius;
            float glow_size;
            
            /**
             * \brief Only passes data to the fragment shader
             * \param i 
             * \return vert
             */
            output vert (const input i )
            {
                output o;
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
            fixed4 frag ( output i ) : SV_Target
            {
                float4 color = 0;
            
                // center uv (0, 1) -> (-1, 1)
                i.uv = ( i.uv - .5 ) * 2;
                
                // circle
                const float alpha = floor( 2. - length(i.uv) );
                if (alpha <= 0) // optimisation (skip glow if outside circle)
                    return 0;
                color += fixed4( circle_color.rgb, circle_color.a * alpha );
                
                // glow
                const float value = max( glow_radius - glow_size / length(i.uv), 0 );
                color += fixed4( glow_color.rgb * value, glow_color.a * value );
                
                // final multiply
                return color * i.color;
            }
        
            ENDCG
        }
    }
}