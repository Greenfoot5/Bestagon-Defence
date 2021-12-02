Shader "Unlit/Circle"
{
    Properties
    {
        _MainTex ( "Texture", 2D ) = "white" {}
        
        _CircleColor ( "Circle Color", Color ) = (1, 1, 1, 1)
        
        _GlowColor ( "Glow Color", Color ) = (1, 1, 1, 1)
        _GlowRadius ( "Glow Radius", Float ) = 1
        _GlowSize ( "Glow Size", Float ) = 1
    }
    SubShader
    {
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

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };
            
            float4 _CircleColor;
            float4 _GlowColor;
            float _GlowRadius;
            float _GlowSize;

            v2f vert ( appdata v )
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag ( v2f i ) : SV_Target
            {
                float4 color = 0;
            
                // center uv (0, 1) -> (-1, 1)
                i.uv = ( i.uv - .5 ) * 2;
                
                // circle
                float alpha = floor( 2. - length(i.uv) );
                if (alpha <= 0) // optimisation (skip glow if outside circle)
                    return 0;
                color += fixed4( _CircleColor.rgb, _CircleColor.a * alpha );
                
                // glow
                float value = max( _GlowRadius - _GlowSize / length(i.uv), 0 );
                color += fixed4( _GlowColor.rgb * value, _GlowColor.a * value );
                
                return color * i.color;
            }
        
            ENDCG
        }
    }
}