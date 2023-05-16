Shader "Custom/ProgressBar" {
    Properties {
        _BarColorA ( "Bar Color", Color ) = (1,1,1,1)
        _BarColorB ( "Bar Cover", Color ) = (1,1,1,1)
        _BgColorA ( "BG Color", Color ) = (0,0,0,1)
        _BgColorB ( "BG Color", Color ) = (0,0,0,1)
        _OutColorA ( "BG Color", Color ) = (0.5,0.5,0.5,1)
        _OutColorB ( "BG Color", Color ) = (0.5,0.5,0.5,1)
        
        _Percentage ( "Percentage", Range(0, 1)) = 1
        _TrapPercent ( "Trapezium Percentage", Range(0, 1)) = 1
        _Flip ( "Flip Trapezium", Range(0, 1)) = 1
        _OutX ( "Outline in X", Range(0, 1)) = 0
        _OutOffset ( "Outline Offset", Range(0, 1)) = 0
        _OutY ( "Outline in Y", Range(0, 1)) = 0
        
        _ShowHalf ( "Show Half", Range(-1, 1)) = 0
    }
    SubShader {
        Tags { "RenderType"="Trasparent" "Queue"="Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct MeshData {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 barColor : COLOR;
            };

            struct Interpolators {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 barColor : COLOR;
            };
            
            float4 _BarColorA;
            float4 _BarColorB;
            float4 _BgColorA;
            float4 _BgColorB;
            float4 _OutColorA;
            float4 _OutColorB;
            float _Percentage;
            float _TrapPercent;
            bool _Flip;
            float _OutX;
            float _OutOffset;
            float _OutY;
            float _ShowHalf;

            float trapezoid(float2 pos) {
                // Show Both
                if (_ShowHalf < 0.5 && _ShowHalf > -0.5) {
                    pos.x = abs(pos.x - 0.5) * 2;
                }
                // Show Left
                if (_ShowHalf < -0.5) {
                    pos.x = 1 - pos.x;
                }
                // Calculate Trapezium
                if (_Flip)
                    return pos.x < lerp(1, _TrapPercent, pos.y);
                return pos.x < lerp(_TrapPercent, 1, pos.y);
            }

            float outline(float2 pos) {
                if (_ShowHalf > 0.5) {
                    if (_Flip)
                        return (pos.x + _OutX > lerp(1 - _OutOffset, _TrapPercent - _OutOffset, pos.y)
                            && pos.x < lerp(1 - _OutOffset, _TrapPercent - _OutOffset, pos.y))
                            || abs(pos.y - 0.5) * 2 + _OutY > 1 - _OutOffset * (_OutY > 1)
                            || _OutOffset < pos.x < _TrapPercent;
                    return (pos.x + _OutX > lerp(_TrapPercent - _OutOffset, 1 - _OutOffset, pos.y)
                        && pos.x < lerp(_TrapPercent - _OutOffset, 1 - _OutOffset, pos.y))
                        || abs(pos.y - 0.5) * 2 + _OutY > 1 - _OutOffset * (_OutY > 1)
                        || _OutOffset < pos.x < _TrapPercent;
                }
                if (_ShowHalf < -0.5) {
                    pos.x = 1 - pos.x;
                    if (_Flip)
                        return (pos.x + _OutX > lerp(1 - _OutOffset, _TrapPercent - _OutOffset, pos.y)
                            && pos.x < lerp(1 - _OutOffset, _TrapPercent - _OutOffset, pos.y))
                            || abs(pos.y - 0.5) * 2 + _OutY > 1 - _OutOffset * (_OutY > 1)
                            || _OutOffset > pos.x > _TrapPercent;
                    return (pos.x + _OutX > lerp(_TrapPercent - _OutOffset, 1 - _OutOffset, pos.y)
                        && pos.x < lerp(_TrapPercent - _OutOffset, 1 - _OutOffset, pos.y))
                        || abs(pos.y - 0.5) * 2 + _OutY > 1 - _OutOffset * (_OutY > 1)
                        || _OutOffset > pos.x > _TrapPercent;
                }
                if (0.5 > _ShowHalf > -0.5) {
                    pos.x = abs(pos.x - 0.5) * 2;
                    if (_Flip)
                        return (pos.x + _OutX > lerp(1 - _OutOffset, _TrapPercent - _OutOffset, pos.y)
                            && pos.x < lerp(1 - _OutOffset, _TrapPercent - _OutOffset, pos.y))
                            || abs(pos.y - 0.5) * 2 + _OutY > 1 - _OutOffset * (_OutY > 1);
                    return (pos.x + _OutX > lerp(_TrapPercent - _OutOffset, 1 - _OutOffset, pos.y)
                        && pos.x < lerp(_TrapPercent - _OutOffset, 1 - _OutOffset, pos.y))
                        || abs(pos.y - 0.5) * 2 + _OutY > 1 - _OutOffset * (_OutY > 1);
                }
                if (_Flip)
                    return (pos.x + _OutX > lerp(1 - _OutOffset, _TrapPercent - _OutOffset, pos.y)
                        && pos.x < lerp(1 - _OutOffset, _TrapPercent - _OutOffset, pos.y))
                        || abs(pos.y - 0.5) * 2 + _OutY > 1 - _OutOffset * (_OutY > 1);
                return (pos.x + _OutX > lerp(_TrapPercent - _OutOffset, 1 - _OutOffset, pos.y)
                    && pos.x < lerp(_TrapPercent - _OutOffset, 1 - _OutOffset, pos.y))
                    || abs(pos.y - 0.5) * 2 + _OutY > 1 - _OutOffset * (_OutY > 1);
            }

            Interpolators vert (MeshData v) {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.barColor = v.barColor;
                return o;
            }

            float4 frag (Interpolators i) : SV_Target {
                float2 trap = trapezoid(i.uv);
                clip(trap - 0.5);
                float2 outln = outline(i.uv);
                
                float barMask = i.uv.x < _Percentage;
                float4 barGradient = lerp(_BarColorB, _BarColorA, i.uv.y);
                float4 barBgGrad = lerp(_BgColorB, _BgColorA, i.uv.y);
                float4 barOutGrad = lerp(_OutColorB, _OutColorA, i.uv.y);
                float4 bar = lerp(barBgGrad, barGradient, barMask);
                //return bar;
                float4 barWOut = lerp(bar, barOutGrad, outln.y);
                return barWOut;
            }
            ENDCG
        }
    }
}
