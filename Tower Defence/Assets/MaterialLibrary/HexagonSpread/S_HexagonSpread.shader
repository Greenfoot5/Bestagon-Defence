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
            #pragma geometry geom
            #pragma fragment frag

            #pragma exclude_renderers gles

            #include "UnityCG.cginc"

            // hexagon magic number (√3)
            #define M 0.866
            // hexagon magic number halved (√3/2)
            #define H 0.433

            /**
             * \brief Vertex to Geometry
             */
            struct v2g
            {
                float4 pos : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float distance : TEXCOORD1;
                float factor : TEXCOORD2;
            };

            /**
             * \brief Geometry to Fragment
             */
            struct g2f
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
             * \param vertex_id The vertex ID (doesn't change as it's only one vertex per particle)
             * \param hexagon_id The hexagon ID (the instance ID)
             * \return v2g Data for the Geometry shader
             */
            v2g vert( const in uint vertex_id: SV_VertexID, const in uint hexagon_id: SV_InstanceID )
            {
                v2g o;

                // Get the particle
                o.pos = float4(Positions[hexagon_id], 0);

                // Calculate the distance from the origin
                float d = distance( o.pos.xy, Origin.xy ) / GridMax;

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
                    o.pos += displace( o.pos, 1 - f );

                // Set the position in clip space
                o.vertex = UnityObjectToClipPos(o.pos);

                // If the preview flag is set, overwrite the factor and distance data with the gradient of the X position
                if (_Preview)
                {
                    o.distance = o.vertex.x * .5 + .5;
                    o.factor = o.vertex.x * .5 + .5;
                }

                return o;
            }

            /**
             * \brief GEOMETRY SHADER
             * \param input The particle input containing all info relevant to it
             * \param triStream The mesh generator
             */
            [maxvertexcount(12)]
            void geom( const in point v2g input[1] : SV_POSITION, inout TriangleStream<g2f> triStream )
            {
                g2f o = (g2f)0;

                // 0 factor is the same as the hexagon not visible at all, so skip rendering it
                if (input[0].factor == 0)
                    return;

                // Give data new names
                float4 p = input[0].vertex;
                    
                o.distance = input[0].distance;
                o.factor = input[0].factor;

                // Calculate the Hexagon's scale
                float s = _HexagonSize * .01 * unity_CameraProjection[0].x * o.factor;

                // Side vertices
                float x = M * s;
                float y = 0.5 * s;

                // Invert on some displays to make it match
                #if UNITY_UV_STARTS_AT_TOP
                if (_ProjectionParams.x < 0.0)
                {
                    y = -y;
                    s = -s;
                }
                #endif

                // Render always on top
                p.z = .9 - o.distance * 0.0001;

                // The rotation matrix
                const float4x4 m = rotate( 1 - o.factor );

                // All vertices of the hexagon
                const float4 a = mul( m, float4(0, s, 0, 0) );
                const float4 v1 = mul( m, float4(-x, y, 0, 0) );
                const float4 v2 = mul( m, float4(x, y, 0, 0) );
                const float4 v3 = mul( m, float4(x, -y, 0, 0) );
                const float4 v4 = mul( m, float4(-x, -y, 0, 0) );

                // Vertices put in an array suitable for tris
                const float4 verts[12] = {
                    v1, a, v2,
                    v1, v2, v3,
                    v1, v3, v4,
                    v4, v3, -a
                };

                // Create every tri
                for( uint t = 0; t < 4; t++ )
                {
                    for( uint v = 0; v < 3; v++ )
                    {
                        // Correct the aspect ration and the vertex position to the particle position
                        o.vertex = p + verts[t * 3 + v] * float4( 1, _ScreenParams.x / _ScreenParams.y, 1, 1 );
                        triStream.Append(o);
                    }
                    triStream.RestartStrip();
                }
            }

            /**
             * \brief FRAGMENT SHADER
             * \param i The hexagon pixel
             * \return fixed4 The color of the pixel
             */
            fixed4 frag( const in g2f i ) : SV_TARGET
            {
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
