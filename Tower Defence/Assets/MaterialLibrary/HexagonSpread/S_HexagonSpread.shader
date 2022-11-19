// Renders hexagons on the particle points given in a buffer

Shader "Custom/Hexagon/Spread"
{
    Properties
    {
        _HexagonSize ("Hexagon Size", Float) = 10

        _Duration ("Transition Duration", Float) = 5
        _AppearDuration ("Appear Duration", Float) = 1

        _Rotation ("Rotation", Float) = 90
        _Displacement ("Displacement", Float) = 1

        [Toggle] _FactorInvert ("Animation Inversion", Int) = 0

        [Toggle] _Preview ("Preview", Int) = 0
        [Toggle] _Loop ("Loop", Int) = 0
    }
    SubShader
    {
        Tags { "Queue"="Transparent+1" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass 
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #pragma exclude_renderers gles

            #include "UnityCG.cginc"

            // hexagon magic number halved (√3/2)
            #define M 0.86602540378
            // hexagon magic number quartered (√3/4)
            #define H 0.43301270189
            
            /**
             * \brief Vertex to Fragment
             */
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float distance : TEXCOORD0;
                float factor : TEXCOORD1;
            };

            /**
             * \brief Data container for a color step in a gradient
             */
            struct ColorStep
            {
                float step : TEXCOORD0;
                float3 color : TEXCOORD1;
            };

            /**
             * \brief Data container for an alpha step in a gradient
             */
            struct AlphaStep
            {
                float step : TEXCOORD0;
                float alpha : TEXCOORD1;
            };
            
            // Time uniforms
            float StartTime;
            float UnscaledTime;

            // Hexagon positions
            StructuredBuffer<float3> Positions;

            // Gradient data
            int ColorCount;
            StructuredBuffer<ColorStep> Colors;
            int AlphaCount;
            StructuredBuffer<AlphaStep> Alphas;

            // Grid data
            float4 Origin = (float4)0;
            float GridMax;

            // Visual data
            float _HexagonSize;

            // Animation data
            float _Duration;
            float _AppearDuration;
            float _Rotation;
            float _Displacement;

            // Flags
            int _FactorInvert;
            int _Preview;
            int _Loop;

            /**
             * \brief Generates a float4x4 transformation matrix for rotation
             * \param factor The factor of transition of the hexagon
             * \return float4x4 The transformation matrix
             */
            float4x4 rotate( const in float factor )
            {
                const float s = sin( radians(_Rotation) * factor );
                const float c = cos( radians(_Rotation) * factor );

                return float4x4(
                    c, s, 0, 0,
                    -s, c, 0, 0,
                    0, 0, 1, 0,
                    0, 0, 0, 1
                );
            }

            /**
             * \brief Moves the hexagon away from or towards the origin based on the factor
             * \param position The position of the hexagon
             * \param factor The factor of transformation of the hexagon
             * \return float4 The displacement of XY
             */
            float4 displace( const in float4 position, const in float factor )
            {
                if ( distance( position.xy, Origin.xy ) < 1 )
                    return (float4)0;
                return float4( -normalize( position.xy - Origin.xy ) * _Displacement * pow( factor, 2 ), 0, 0 );
            }

            /**
             * \brief VERTEX SHADER
             * \param vertex The vertex coordinate (a vertex of the hexagon relative to its centre)
             * \param hexagon_id The hexagon ID (the instance ID), used to get the absolute position of the hexagon from the Position buffer
             * \return v2g Data for the Geometry shader
             */
            v2f vert( const in float4 vertex: POSITION, const in uint hexagon_id: SV_InstanceID )
            {
                v2f o;

                // Get the particle
                float4 pos = float4(Positions[hexagon_id], 0);

                // Calculate the distance from the origin
                float d = distance( pos.xy, Origin.xy ) / GridMax;

                // Calculate the time
                float time;
                if (_Loop)
                    time = fmod( UnscaledTime, _Duration + _AppearDuration );
                else
                    time = UnscaledTime - StartTime;

                // Invert the time if enabled
                if (_FactorInvert)
                    time = _Duration + _AppearDuration - time;

                // Calculate the factor of the hexagon based on the time and distance
                float f = clamp( ( time - d * _Duration ) / _AppearDuration, 0, 1 );

                // Set the distance and factor data
                o.distance = d;
                o.factor = f;

                // Do not displace if the preview flag is set
                if (!_Preview)
                    pos += displace( pos, 1 - f );

                // Rotate based off time + scale based off of time
                pos += mul( rotate( 1 - o.factor ), vertex * o.factor );

                // Set the position in clip space
                o.vertex = UnityObjectToClipPos( pos );

                // Render always on top
                o.vertex.z = .9 - o.distance * 0.0001;

                // If the preview flag is set, overwrite the factor and distance data with the gradient of the X position
                if (_Preview)
                {
                    o.distance = o.vertex.x * .5 + .5;
                    o.factor = o.vertex.x * .5 + .5;
                }

                return o;
            }

            /**
             * \brief FRAGMENT SHADER
             * \param i The hexagon pixel
             * \return fixed4 The color of the pixel
             */
            fixed4 frag( const in v2f i ) : SV_TARGET
            {
                // Discard if the hexagon has no factor
                if ( i.factor == 0 )
                    discard;

                float4 o = float4(0, 0, 0, 0);
                
                // Color gradient

                // Left side of the gradient
                if ( Colors[0].step > i.factor )
                    o.rgb += Colors[0].color;

                // Right side of the gradient
                else if ( Colors[ ColorCount - 1 ].step < i.factor )
                    o.rgb += Colors[ ColorCount - 1 ].color;

                // Anywhere in-between
                else
                {
                    int n = 0;

                    // Find the index where the factor is between 2 gradient steps
                    while ( n + 1 < ColorCount )
                    {
                        if ( Colors[n].step <= i.factor && Colors[n + 1].step > i.factor )
                            break;
                        n += 1;
                    }

                    // Calculate the mixing value
                    const float df = Colors[n + 1].step - Colors[n].step;
                    const float f = (i.factor - Colors[n].step) / df;

                    // Mix the steps
                    o.rgb += Colors[n].color * (1 - f) + Colors[n + 1].color * f;
                }
                
                // Alpha gradient

                // Left side of the gradient
                if ( Alphas[0].step > i.factor )
                    o.a += Alphas[0].alpha;

                // Right side of the gradient
                else if ( Alphas[ AlphaCount - 1 ].step < i.factor )
                    o.a += Alphas[ AlphaCount - 1 ].alpha;

                // Anywhere in-between
                else
                {
                    int n = 0;

                    // Find the index where the factor is between 2 gradient steps
                    while ( n + 1 < AlphaCount )
                    {
                        if ( Alphas[n].step <= i.factor && Alphas[n + 1].step > i.factor )
                            break;
                        n += 1;
                    }
                    
                    // Calculate the mixing value
                    const float df = Alphas[n + 1].step - Alphas[n].step;
                    const float f = (i.factor - Alphas[n].step) / df;
                    
                    // Mix the steps
                    o.a += Alphas[n].alpha * (1 - f) + Alphas[n + 1].alpha * f;
                }

                return o;
            }

            ENDCG
        }
    }

    CustomEditor "HexagonSpreadEditor"
}
