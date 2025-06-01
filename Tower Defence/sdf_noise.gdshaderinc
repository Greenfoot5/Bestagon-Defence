// http://iquilezles.org/www/articles/smin/smin.htm
/**
 * Quadratic smooth min
 */
float smin( float a, float b, float k )
{
    float h = max(k-abs(a-b),0.0);
    return min(a, b) - h*h*0.25/k;
}

// http://iquilezles.org/www/articles/smin/smin.htm
/**
 * Quadratic smooth max
 */
float smax( float a, float b, float k )
{
    float h = max(k-abs(a-b),0.0);
    return max(a, b) + h*h*0.25/k;
}

// https://iquilezles.org/www/articles/fbmsdf/fbmsdf.htm
/**
 * Random circle
 */
float circ( in vec2 i, vec2 f, in vec2 c )
{
    // random radius at grid vertex i+c (please replace this hash by
    // something better if you plan to use this for a real application)
    vec2  p = 17.0*fract( (i+c)*0.3183099+vec2(0.17,0.11) );
    float w = fract( p.x*p.y*(p.x+p.y) );
    float r = 0.7*w*w;
    // distance to circle at grid vertex i+c
    return length(f-c) - r;
}

// https://iquilezles.org/www/articles/fbmsdf/fbmsdf.htm
/**
 * Quadratic smooth min
 */
float sdBase( in vec2 p )
{
    vec2 i = floor(p);
    vec2 f = fract(p);
    // distance to the 4 corners circles
    return min(
        min(circ(i,f,vec2(0,0)),
            circ(i,f,vec2(0,1))),
        min(circ(i,f,vec2(1,0)),
            circ(i,f,vec2(1,1))));
}

// https://iquilezles.org/www/articles/fbmsdf/fbmsdf.htm
float sdFbm(
    in vec2 p,
    in int detail,
    in float rough,
    in float inflate,
    in float smooth_fac,
    in float d)
{
    float s = 1.00;
    for(int i=0; i < min(int(detail), 12); i++)
    {
        // Evaluate new octave
        float n = s*sdBase(p);

        // Add
        n = smax(n,d - inflate*s, smooth_fac*s );
        d = smin(n,d, smooth_fac*s );

        // Prep new octave
        p = mat2(vec2(0.00, 2.90),
                    vec2(-1.70, 2.28))*p;
        s = rough*s;
    }
    return d;
}