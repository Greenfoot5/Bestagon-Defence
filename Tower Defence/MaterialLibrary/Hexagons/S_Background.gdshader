shader_type canvas_item;

uniform vec2 offset_uv = vec2(0);

uniform float hexagon_scale = float(5);
uniform float overlay_strength = float(5);
uniform float hex_opacity : hint_range(0.0, 1.0, 0.001) = float(0.2);

group_uniforms Glow;
uniform float glow_intensity = float(0.5);
uniform float glow_clamp = float(1);
uniform float glow_opacity : hint_range(0.0, 1.0, 0.001) = float(0.5);

group_uniforms Movement;
uniform float luminance_shift_speed = float(0.75);
uniform float scroll_speed = float(0.03);

// hexagon vertex (a normalized (1; √3) )
const vec2 R = vec2( 0.5, 0.866 );
// half a hexagon vertex (r/2)
const vec2 H = vec2( 0.25, 0.433 );
global uniform float UNSCALED_TIME;

// <https://www.shadertoy.com/view/4dS3Wd>
// 1D Noise by Morgan McGuire @morgan3d, http://graphicscodex.com
/**
 * brief 1D Noise by Morgan McGuire @morgan3d, http://graphicscodex.com
 * https://www.shadertoy.com/view/4dS3Wd
 * param p
 * return RETURN
 */
float hash( in float p )
{

    p = fract(p * 0.011);
    p *= p + 7.5;
    p *= p + p;
    return fract(p);
}

/**
 * brief 1D Noise
 * param x 1D Seed
 * return RETURN Pseudo-random 1D value
 */
float noise( const in float x )
{
    const float i = floor(x);
    const float f = fract(x);
    const float u = f * f * ( 3.0 - 2.0 * f );
    return mix( hash(i), hash( i + 1.0 ), u );

}
// --- 1D NOISE

/**
 * brief Generic 2D Noise
 * <https://gist.github.com/patriciogonzalezvivo/670c22f3966e662d2f83>
 * param n
 * return RETURN
 */
float rand2d( const in vec2 n )
{
    return fract( sin( dot( n, vec2( 12.9898, 4.1414 ) ) ) * 43758.5453 );
}

/**
 * brief 2D Noise
 * param p 2D Seed
 * return RETURN Pseudo-random 1D value
 */
float noise2d( in vec2 p )
{
    vec2 ip = floor(p);
    vec2 u = fract(p);
    u = u*u*(3.0-2.0*u);

    float res = mix(
        mix( rand2d(ip), rand2d( ip + vec2(1.0,0.0) ), u.x ),
        mix( rand2d( ip + vec2(0.0,1.0) ),rand2d( ip + vec2(1.0,1.0) ), u.x ),
        u.y );
    return res*res;
}
// --- 2D NOISE

vec4 hexagon(in vec2 uv, in vec4 col) {
    // scrolling
    uv.x += UNSCALED_TIME * scroll_speed;
    uv.y -= UNSCALED_TIME * scroll_speed;
    uv *= hexagon_scale;

    // hexagon uv
    vec2 a = mod(uv, R) - H;
    vec2 b = mod(uv - H, R) - H;

    // hexagon seed
    vec2 gv = length(a) < length(b) ? a : b;
    vec2 hs = uv - gv;

    // instance seed
    float seed = noise2d( hs * 10. );
    float l = noise( seed * 30. + UNSCALED_TIME * luminance_shift_speed );

    // overlay
    col.rgb *= .8;
    col.rgb *= ( 1. - l * overlay_strength * 2. );
    col.rgb += overlay_strength * ( 1. - l );

    return col;
}

/**
 * brief Generates an edge glow
 * param uv UV data of the quad
 * param col Color option for the glow
 * return RETURN Color value generated
 */
vec4 glow ( const in vec2 uv, in vec4 col ) {
    // normals to edges
    vec2 n = abs( 1. / sin( uv * PI ) * 0.05 * glow_intensity );

    // intensity based on edge value
    float v = min( distance( n, vec2(0) ), glow_clamp );

    // color
    return vec4( v * col.rgb, v * glow_opacity );
}

void fragment() {
    // offset
    vec2 uv = offset_uv + UV;

    // aspect ratio
    float dx = dFdx(uv.x);
    float dy = dFdy(uv.y);
    float aspect = -abs( dy/dx );

    // hexagon uv (corrected ratio)
    vec2 huv = mod(uv - H, R) - H;
    huv.x *= aspect;
    huv += 1.;


    // color
    vec4 color = vec4(.5);
    color.a = hex_opacity;
    color.rgb += hexagon( huv, color ).rgb;
    color += glow( uv, color );

    // limiting alpha (weird stuff can happen with negative or above 1)
    color.a = clamp( color.a, 0, 1 );

    COLOR = color * COLOR;
}

