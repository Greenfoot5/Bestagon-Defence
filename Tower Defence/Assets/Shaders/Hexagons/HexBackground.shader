Shader "Unlit/Hex Background"
{
    Properties
    {
        _OffsetUV ("Offset UV", Vector) = (0, 0, 0, 0)
        _HexScale ("Hexagon Scale", Float) = 5
        _ShiftSpeed ("Luminance Shift Speed", Float) = .75
        _Overlay ("Overlay Strength", Float) = .15
        _Opacity ("Hex Opacity", Float) = .2
        _GlowIntensity ("Glow Intensity", Float) = .5
        _GlowClamp ("Glow Clamp", Float) = 1
        _GlowOpacity ("Glow Opacity", Float) = .5
        _ScrollSpeed ("Scroll Speed", Float) = 0.03
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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
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
            
            float rand2d( in float2 n )
            {
            
	            return frac( sin( dot( n, float2(12.9898, 4.1414) ) ) * 43758.5453 );
                
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
            
            float4 hexagon ( in float2 uv, in float4 col )
            {
            
                // SCROLLER
                uv.xy += _UnscaledTime * ScrollVector * _ScrollSpeed;
                //uv.x = abs(uv.x) + .0418;
                uv *= _HexScale;
                
                // HEX UV
                float2 r = normalize( float2( 1.0, 1.73 ) );
	            float2 h = r * 0.5;
	            float2 a = fmod( uv, r ) - h;
	            float2 b = fmod( uv - h, r ) - h;
                
                // NEGATIVE CORRECTION
                if (uv.x < 0)
                    a.x += .5;
                if (uv.x - h.x < 0)
                    b.x += .5;
                
                // HEX SEED
                float2 gv = length(a) < length(b) ? a : b;
                float2 hs = uv - gv;
                
                // INSTANCE SEED
                float seed = noise2d( hs * 10 );
                
                // HEX LUMINANCE
                float l = noise( seed * 30 + _UnscaledTime * _ShiftSpeed * seed );
                
                // OVERLAY
                col.rgb *= .8;
	            col.rgb = col * ( 1 - l * _Overlay * 2 );
	            col.rgb = col.rgb + _Overlay * ( 1 - l );
                
                return col;
            
            }
            
            float4 glow ( in float2 uv, in float4 col )
            {
                
                // NORMALS TO GLOW
                float2 n = abs( 1 / sin( uv * pi ) * 0.05 * _GlowIntensity );
                
                // INTENSITY BASED ON NORMAL VALUE
                float v = min( distance( n, 0 ), _GlowClamp );
                
                // COLOR
                return float4( v * col.rgb, v * _GlowOpacity );
                
            }

            v2f vert (appdata v)
            {
            
                // VERTEX OUT
                v2f o;
                
                // DATA
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                
                return o;
                
            }

            fixed4 frag(v2f i) : SV_Target
            {
            
                // OFFSET
                i.uv += _OffsetUV;
            
                // ASPECT RATIO
                float dx = ddx(i.uv.x);
                float dy = ddy(i.uv.y);
                float aspect = -abs( dy/dx );
                
                // HEXAGON UV (CORRECTED RATIO)
                float2 huv = i.uv;
                huv.x *= aspect;
                huv += 1;
                
                // COLOR
                float4 color = float4(0, 0, 0, 0);
                color.a = _Opacity;
                color.rgb += hexagon( huv, i.color );
                color += glow( i.uv, i.color );
                
                color.a = clamp( color.a, 0, 1 );
                
                return color;
                
            }
            ENDCG
        }
    }
}
