// Draws a bunch of neon hexagons with a possible edge glow on the quad

Shader "Unlit/Hex Background"
{
    Properties
    {
        _OffsetUV ("Offset UV", Vector) = (0, 0, 0, 0)

        _HexScale ("Hexagon Scale", Float) = 5
        _Overlay ("Overlay Strength", Float) = .15
        _Opacity ("Hex Opacity", Float) = .2
        
        _GlowIntensity ("Glow Intensity", Float) = .5
        _GlowClamp ("Glow Clamp", Float) = 1
        _GlowOpacity ("Glow Opacity", Float) = .5

        _ShiftSpeed ("Luminance Shift Speed", Float) = .75
        _ScrollSpeed ("Scroll Speed", Float) = 0.03
        
        // Unity complains if the shader has no texture input when it's used in an Image component
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            // hexagon vertex (a normalized (1; √3) )
            #define R float2( 0.57735, 1.0 )
            // half a hexagon vertex (r/2)
            #define H float2( 0.28868, 0.5 )

            /**
             * \brief It's an input!
             */
            struct input
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            /**
             * \brief Guess what!? It's an output!
             */
            struct output
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };
            
            float4 offset_uv;
            float hex_scale;
            float shift_speed;
            float overlay;
            float opacity;
            
            float glow_intensity;
            float glow_clamp;
            float glow_opacity;
            
            float unscaled_time;
            
            static const float pi = 3.14159265;
            
            static float2 scroll_vector = float2( 1, 1 );
            float scroll_speed;
            
            // <https://www.shadertoy.com/view/4dS3Wd>
            // 1D Noise by Morgan McGuire @morgan3d, http://graphicscodex.com
            /**
             * \brief 1D Noise by Morgan McGuire @morgan3d, http://graphicscodex.com
             * https://www.shadertoy.com/view/4dS3Wd
             * \param p 
             * \return RETURN a float
             */
            float hash( in float p )
            {
    
	            p = frac(p * 0.011);
	            p *= p + 7.5;
	            p *= p + p;
	            return frac(p);
    
            }

            /**
             * \brief Make sure to wear headphones
             * \param x I FOUND X!
             * \return RETURN
             */
            float noise(const in float x ) 
            {
                const float i = floor(x);
                const float f = frac(x);
                const float u = f * f * ( 3.0 - 2.0 * f );
	            return lerp( hash(i), hash( i + 1.0 ), u );
    
            }
            // --- 1D NOISE
            
            /**
             * \brief Generic 2D Noise
             * <https://gist.github.com/patriciogonzalezvivo/670c22f3966e662d2f83>
             * \param n 
             * \return RETURN
             */
            float rand2d(const in float2 n )
            {
            
	            return frac( sin( dot( n, float2( 12.9898, 4.1414 ) ) ) * 43758.5453 );
                
            }

            /**
             * \brief Is it possible to have 2d headphones?
             * \param p 
             * \return RETURN
             */
            float noise2d( in float2 p )
            {
                const float2 ip = floor(p);
	            float2 u = frac(p);
	            u = u*u*(3.0-2.0*u);

                const float res = lerp(
		            lerp( rand2d(ip), rand2d( ip + float2(1.0,0.0) ), u.x ),
		            lerp( rand2d( ip + float2(0.0,1.0) ),rand2d( ip + float2(1.0,1.0) ), u.x ),
                    u.y );
	            return res*res;
                
            }
            // --- 2D NOISE
            
            /**
             * \brief Creates the hexagonal image
             * \param uv 
             * \param col 
             * \return RETURN
             */
            float4 hexagon ( in float2 uv, in float4 col )
            {
            
                // scroller
                uv.xy += unscaled_time * scroll_vector * scroll_speed;
                //uv.x = abs(uv.x) + .0418;
                uv *= hex_scale;
                
                // hexagon uv
	            float2 a = fmod( uv, R ) - H;
	            float2 b = fmod( uv - H, R ) - H;
                
                // negative uv correction
                if (uv.x < 0)
                    a.x += .5;
                if (uv.x - H.x < 0)
                    b.x += .5;
                
                // hexagon seed
                const float2 gv = length(a) < length(b) ? a : b;
                const float2 hs = uv - gv;
                
                // instance seed
                const float seed = noise2d( hs * 10 );
                
                // hexagon luminance
                const float l = noise( seed * 30 + unscaled_time * shift_speed );
                
                // overlay
                col.rgb *= .8;
	            col.rgb = col * ( 1 - l * overlay * 2 );
	            col.rgb = col.rgb + overlay * ( 1 - l );
                
                return col;
            
            }
            
            /**
             * \brief Adds an edge glow
             * \param uv 
             * \param col 
             * \return RETURN
             */
            float4 glow (const in float2 uv, in float4 col )
            {
                
                // normals to edges
                const float2 n = abs( 1 / sin( uv * pi ) * 0.05 * glow_intensity );
                
                // intensity based on edge value
                float v = min( distance( n, 0 ), glow_clamp );
                
                // color
                return float4( v * col.rgb, v * glow_opacity );
                
            }

            // VERTEX SHADER
            /**
             * \brief Only passes data to the fragment shader
             * \param i 
             * \return RETURN
             */
            output vert (const input i )
            {
                output o;
                o.vertex = UnityObjectToClipPos(i.vertex);
                o.uv = i.uv;
                o.color = i.color;
                return o;
                
            }

            // FRAGMENT SHADER
            /**
             * \brief Colours the hexagons and glow effects in
             * \param i 
             * \return RETURN
             */
            fixed4 frag ( output i ) : SV_Target
            {
            
                // offset
                i.uv += offset_uv;
            
                // aspect ratio
                const float dx = ddx(i.uv.x);
                const float dy = ddy(i.uv.y);
                const float aspect = -abs( dy/dx );
                
                // hexagon uv (corrected ratio)
                float2 huv = i.uv;
                huv.x *= aspect;
                huv += 1;
                
                // color
                float4 color = float4(0, 0, 0, 0);
                color.a = opacity;
                color.rgb += hexagon( huv, i.color );
                color += glow( i.uv, i.color );
                
                // limiting alpha (weird stuff can happen with negative or above 1)
                color.a = clamp( color.a, 0, 1 );
                
                return color;
                
            }
            ENDCG
        }
    }
}
