// Draws a bunch of neon hexagons with a possible edge glow on the quad

//Shader "Custom/GlowBox/Background"
Shader "UI/Default/GlowBox"
{
    Properties
    {
        _Opacity ("Hex Opacity", Range (0, 1)) = .1
        
        _GlowIntensity ("Glow Intensity", Float) = .3
        _GlowClamp ("Glow Clamp", Float) = .5
        _GlowOpacity ("Glow Opacity", Range (0, 1)) = .7
        
        // required for UI.Mask
		_StencilComp ("Stencil Comparison", Float) = 2
		_Stencil ("Stencil ID", Float) = 1
		_StencilOp ("Stencil Operation", Float) = 3
		_StencilWriteMask ("Stencil Write Mask", Float) = 1
		_StencilReadMask ("Stencil Read Mask", Float) = 1
		_ColorMask ("Color Mask", Float) = 15
        
        // Unity complains if the shader has no texture input when it's used in an Image component
        [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100
        
        // required for UI.Mask
		Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp] 
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}
 		ColorMask [_ColorMask]

        Pass
        {
            Name "GlowBox"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"
            
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
                float4 worldPosition : TEXCOORD1;
            };
            
            // PROPERTIES
            float _Opacity;
            
            float _GlowIntensity;
            float _GlowClamp;
            float _GlowOpacity;

            float4 _ClipRect;
            
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
                o.worldPosition = i.vertex;
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
                // color
                float4 color = i.color;
                color.a = _Opacity;
                color += glow( i.uv, i.color );

                color.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);
                
                // limiting alpha (weird stuff can happen with negative or above 1)
                color.a = clamp( color.a, -1, 1 );
                
                return color;
                
            }
            ENDCG
        }
    }

    //CustomEditor "Editor.Shader.HexagonsShaderEditor"
}
