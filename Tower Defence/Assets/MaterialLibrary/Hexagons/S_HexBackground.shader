// Draws a bunch of neon hexagons with a possible edge glow on the quad

Shader "Unlit/Hex Background"
{
    Properties
    {
        [ShowAsVector2] _OffsetUV ("Offset UV", Vector) = (0, 0, 0, 0)

        _HexScale ("Hexagon Scale", Float) = 5
        _Overlay ("Overlay Strength", Float) = .15
        _Opacity ("Hex Opacity", Range (0, 1)) = .2
        
        _GlowIntensity ("Glow Intensity", Float) = .5
        _GlowClamp ("Glow Clamp", Float) = 1
        _GlowOpacity ("Glow Opacity", Range (0, 1)) = .5

        _ShiftSpeed ("Luminance Shift Speed", Float) = .75
        _ScrollSpeed ("Scroll Speed", Float) = 0.03
        
        // Unity complains if the shader has no texture input when it's used in an Image component
        [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
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
            #define R float2( 0.5, 0.866 )
            // half a hexagon vertex (r/2)
            #define H float2( 0.25, 0.433 )
            
            #define PI 3.14159265

            /**
             * \brief App to Vertex
             */
            struct a2v
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            /**
             * \brief Vertex To Fragment
             */
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };
            
            // PROPERTIES
            float4 _OffsetUV;
            float _HexScale;
            float _ShiftSpeed;
            float _Overlay;
            float _Opacity;
            
            float _GlowIntensity;
            float _GlowClamp;
            float _GlowOpacity;
            
            float _UnscaledTime;
            
            static float2 scroll_vector = float2( 1, 1 );
            float _ScrollSpeed;
            
            // <https://www.shadertoy.com/view/4dS3Wd>
            // 1D Noise by Morgan McGuire @morgan3d, http://graphicscodex.com
            /**
             * \brief 1D Noise by Morgan McGuire @morgan3d, http://graphicscodex.com
             * https://www.shadertoy.com/view/4dS3Wd
             * \param p 
             * \return RETURN
             */
            float hash( in float p )
            {
    
	            p = frac(p * 0.011);
	            p *= p + 7.5;
	            p *= p + p;
	            return frac(p);
    
            }

            /**
             * \brief 1D Noise
             * \param x 1D Seed
             * \return RETURN Pseudo-random 1D value
             */
            float noise( const in float x ) 
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
            float rand2d( const in float2 n )
            {
            
	            return frac( sin( dot( n, float2( 12.9898, 4.1414 ) ) ) * 43758.5453 );
                
            }

            /**
             * \brief 2D Noise
             * \param p 2D Seed
             * \return RETURN Pseudo-random 1D value
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
             * \param uv UV data of the quad
             * \param col Color option for the hexagons
             * \return RETURN Color value generated
             */
            float4 hexagon ( in float2 uv, in float4 col )
            {
            
                // scroller
                uv.xy += _UnscaledTime * scroll_vector * _ScrollSpeed;
                //uv.x = abs(uv.x) + .0418;
                uv *= _HexScale;
                
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
                const float l = noise( seed * 30 + _UnscaledTime * _ShiftSpeed );
                
                // overlay
                col.rgb *= .8;
	            col.rgb = col * ( 1 - l * _Overlay * 2 );
	            col.rgb = col.rgb + _Overlay * ( 1 - l );
                
                return col;
            
            }
            
            /**
             * \brief Generates an edge glow
             * \param uv UV data of the quad
             * \param col Color option for the glow
             * \return RETURN Color value generated
             */
            float4 glow ( const in float2 uv, in float4 col )
            {
                
                // normals to edges
                const float2 n = abs( 1 / sin( uv * PI ) * 0.05 * _GlowIntensity );
                
                // intensity based on edge value
                float v = min( distance( n, 0 ), _GlowClamp );
                
                // color
                return float4( v * col.rgb, v * _GlowOpacity );
                
            }

            // VERTEX SHADER
            /**
             * \brief Only passes data to the fragment shader
             * \param i App input
             * \return RETURN
             */
            v2f vert ( const a2v i )
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(i.vertex);
                o.uv = i.uv;
                o.color = i.color;
                return o;
                
            }

            // FRAGMENT SHADER
            /**
             * \brief Colours the hexagons and glow effects in
             * \param i Vertex input
             * \return RETURN
             */
            fixed4 frag ( v2f i ) : SV_Target
            {
            
                // offset
                i.uv += _OffsetUV;
            
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
                color.a = _Opacity;
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
