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
            #define r float2( 0.57735, 1.0 )
            // half a hexagon vertex (r/2)
            #define h float2( 0.28868, 0.5 )

            struct Input
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Output
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };
            
            float4 _OffsetUV;
            float _HexScale;
            float _ShiftSpeed;
            float _Overlay;
            float _Opacity;
            
            float _GlowIntensity;
            float _GlowClamp;
            float _GlowOpacity;
            
            float _UnscaledTime;
            
            static const float pi = 3.14159265;
            
            static float2 ScrollVector = float2( 1, 1 );
            float _ScrollSpeed;
            
            // <https://www.shadertoy.com/view/4dS3Wd>
            // 1D Noise by Morgan McGuire @morgan3d, http://graphicscodex.com
            float hash( in float p )
            {
    
	            p = frac(p * 0.011);
	            p *= p + 7.5;
	            p *= p + p;
	            return frac(p);
    
            }

            float noise( in float x ) 
            {

                float i = floor(x);
	            float f = frac(x);
	            float u = f * f * ( 3.0 - 2.0 * f );
	            return lerp( hash(i), hash( i + 1.0 ), u );
    
            }
            // --- 1D NOISE
            
            // <https://gist.github.com/patriciogonzalezvivo/670c22f3966e662d2f83>
            // Generic 2D Noise
            float rand2d( in float2 n )
            {
            
	            return frac( sin( dot( n, float2( 12.9898, 4.1414 ) ) ) * 43758.5453 );
                
            }

            float noise2d( in float2 p )
            {
            
	            float2 ip = floor(p);
	            float2 u = frac(p);
	            u = u*u*(3.0-2.0*u);
	
	            float res = lerp(
		            lerp( rand2d(ip), rand2d( ip + float2(1.0,0.0) ), u.x ),
		            lerp( rand2d( ip + float2(0.0,1.0) ),rand2d( ip + float2(1.0,1.0) ), u.x ),
                    u.y );
	            return res*res;
                
            }
            // --- 2D NOISE
            
            // Creates the hexagonal image
            float4 hexagon ( in float2 uv, in float4 col )
            {
            
                // scroller
                uv.xy += _UnscaledTime * ScrollVector * _ScrollSpeed;
                //uv.x = abs(uv.x) + .0418;
                uv *= _HexScale;
                
                // hexagon uv
	            float2 a = fmod( uv, r ) - h;
	            float2 b = fmod( uv - h, r ) - h;
                
                // negative uv correction
                if (uv.x < 0)
                    a.x += .5;
                if (uv.x - h.x < 0)
                    b.x += .5;
                
                // hexagon seed
                float2 gv = length(a) < length(b) ? a : b;
                float2 hs = uv - gv;
                
                // instance seed
                float seed = noise2d( hs * 10 );
                
                // hexagon luminance
                float l = noise( seed * 30 + _UnscaledTime * _ShiftSpeed );
                
                // overlay
                col.rgb *= .8;
	            col.rgb = col * ( 1 - l * _Overlay * 2 );
	            col.rgb = col.rgb + _Overlay * ( 1 - l );
                
                return col;
            
            }
            
            // Adds an edge glow
            float4 glow ( in float2 uv, in float4 col )
            {
                
                // normals to edges
                float2 n = abs( 1 / sin( uv * pi ) * 0.05 * _GlowIntensity );
                
                // intensity based on edge value
                float v = min( distance( n, 0 ), _GlowClamp );
                
                // color
                return float4( v * col.rgb, v * _GlowOpacity );
                
            }

            // VERTEX SHADER
            // Only passes data to the fragment shader
            Output vert ( Input i )
            {
                Output o;
                o.vertex = UnityObjectToClipPos(i.vertex);
                o.uv = i.uv;
                o.color = i.color;
                return o;
                
            }

            // FRAGMENT SHADER
            // Colors the hexagons and glow effects in
            fixed4 frag ( Output i ) : SV_Target
            {
            
                // offset
                i.uv += _OffsetUV;
            
                // aspect ratio
                float dx = ddx(i.uv.x);
                float dy = ddy(i.uv.y);
                float aspect = -abs( dy/dx );
                
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
