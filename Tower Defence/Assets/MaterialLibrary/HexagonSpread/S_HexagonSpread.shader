Shader "Custom/Hexagon/Spread"
{
    Properties
    {
        _HexagonSize ("Hexagon Size", Float) = 10

        _Duration ("Transition Duration", Float) = 5
        _AppearDuration ("Appear Duration", Float) = 1

        _Rotation ("Rotation", Float) = 90
        _Displacement ("Displacement", Float) = 90

        _FactorInvert ("Animation Inversion", Int) = 0
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
            
            float StartTime;
            float UnscaledTime;

            StructuredBuffer<float3> Positions;
            float4 Origin = float4(0, 0, 0, 0);
            float GridMax;

            float _HexagonSize;

            float _Duration;
            float _AppearDuration;
            float _Rotation;
            float _Displacement;
            int _FactorInvert;

            struct v2g
            {
                float4 pos : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float distance : TEXCOORD1;
                float factor : TEXCOORD2;
            };

            struct g2f
            {
                float4 vertex : SV_POSITION;
                float distance : TEXCOORD0;
                float factor : TEXCOORD1;
            };

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

            float4 displace( const in float4 position, const in float factor )
            {
                if ( distance( position.xy, Origin.xy ) < 1 )
                    return (float4)0;
                return float4( -normalize( position.xy - Origin.xy ) * _Displacement * pow( factor, 2 ), 0, 0 );
            }

            v2g vert( uint vertex_id: SV_VertexID, uint instance_id: SV_InstanceID )
            {
                v2g o;

                o.pos = float4(Positions[instance_id], 0);

                float d = distance( o.pos.xy, Origin.xy ) / GridMax;

                float time = StartTime + _Duration + _AppearDuration - UnscaledTime;
                // float time = fmod( UnscaledTime, _Duration + _AppearDuration );

                if (_FactorInvert == 1)
                    time = _Duration + _AppearDuration - time;

                float f = clamp( ( time - d * _Duration ) / _AppearDuration, 0, 1 );

                o.distance = d;
                o.factor = f;

                o.pos += displace( o.pos, 1 - f );

                o.vertex = UnityObjectToClipPos(o.pos);

                return o;
            }

            [maxvertexcount(12)]
            void geom( in point v2g input[1] : SV_POSITION, inout TriangleStream<g2f> triStream )
            {
                g2f o = (g2f)0;

                if (input[0].factor == 0)
                    return;

                float4 p = input[0].vertex;
                    
                o.distance = input[0].distance;
                o.factor = input[0].factor;

                float s = _HexagonSize * .01 * unity_CameraProjection[0].x * o.factor;

                float x = M * s;
                float y = 0.5 * s;

                #if UNITY_UV_STARTS_AT_TOP
                if (_ProjectionParams.x < 0.0)
                {
                    y = -y;
                    s = -s;
                }
                #endif

                p.z = 1 - o.distance * 0.0001;

                float4x4 m = rotate( 1 - o.factor );

                float4 a = mul( m, float4(0, s, 0, 0) );
                float4 v1 = mul( m, float4(-x, y, 0, 0) );
                float4 v2 = mul( m, float4(x, y, 0, 0) );
                float4 v3 = mul( m, float4(x, -y, 0, 0) );
                float4 v4 = mul( m, float4(-x, -y, 0, 0) );

                float4 verts[12] = {
                    v1, a, v2,
                    v1, v2, v3,
                    v1, v3, v4,
                    v4, v3, -a
                };

                for( uint t = 0; t < 4; t++ )
                {
                    for( uint v = 0; v < 3; v++ )
                    {
                        o.vertex = p + verts[t * 3 + v] * float4(1, _ScreenParams.x / _ScreenParams.y, 1, 1);
                        // o.color = (verts[v] - p) / s;
                        // o.color.a = 1;
                        // o.color = float4(p.x, p.y, 0, 1);
                        // o.color = float4(1-d, 1-d, 1-d, 1);
                        triStream.Append(o);
                    }
                    triStream.RestartStrip();
                }
            }

            fixed4 frag( g2f i ) : SV_TARGET
            {
                float4 o = float4(0, 0, 0, 1);
                
                if (i.factor < .5)
                {
                    o += (i.factor) * 2 * float4(.004, .4, .412, 1);
                }
                else
                {
                    o += (1 - i.factor) * 2 * float4(.004, .4, .412, 1);
                    o += (i.factor - .5) * 2 * float4(1, 1, 1, 1);
                }
                return o;
            }

            ENDCG
        }
    }
}
