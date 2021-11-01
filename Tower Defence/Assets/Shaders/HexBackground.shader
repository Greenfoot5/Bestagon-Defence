Shader "Unlit/Hex Background"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        //_Tint ("Background Tint", Color) = (1,1,1,1)
        _HexScale ("Hexagon Scale", Float) = 5
        _Overlay ("Overlay Strength", Float) = .15
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

            fixed4 _Tint;
            float _HexScale;
            float _Overlay;
            
            static float2 ScrollVector = float2( 1, 1 );
            static float ScrollSpeed = 0.03;
            
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

            float getId ( in float2 hp )
            {
	            
                // RANDOM VALUES FOR NOISE
	            return noise( 
		            hp.x * 5.5 +
		            hp.y * -5.5 +
		            2.5
	            );
	
            }
            
            float4 hexagon ( in float2 uv, in float4 col )
            {
                
                // HEX UV
                float2 r = normalize( float2( 1.0, 1.73 ) );
	            float2 h = r * 0.5;
	            float2 a = fmod( uv, r ) - h;
	            float2 b = fmod( uv - h, r ) - h;
                
                // HEX ID
                float2 gv = length(a) < length(b) ? a : b;
                float2 hi = uv - gv;
                
                // INSTANCE ID
                float id = getId( hi );
                
                // HEX LUMINANCE
                float l = noise( id * 7.5 + _Time.y * .75 * id );
                
                // OVERLAY
                col.rgb *= .8;
	            col.rgb = col * ( 1 - l * _Overlay * 2 );
	            col.rgb = col.rgb + _Overlay * ( 1 - l );
                
                return col;
            
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
            
                // ASPECT RATIO
                float dx = ddx(i.uv.x);
                float dy = ddy(i.uv.y);
                float aspect = dy/dx;
                
                i.uv.x *= aspect;
                i.uv += 1;
                
                // SCROLLER
                i.uv += _Time.y * ScrollVector * ScrollSpeed;
                i.uv *= _HexScale;
                
                // COLOR
                i.color = hexagon( i.uv, i.color );
                
                return i.color;
                
            }
            ENDCG
        }
    }
}
