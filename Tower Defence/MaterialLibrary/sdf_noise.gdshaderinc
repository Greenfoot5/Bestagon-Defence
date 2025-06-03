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

// https://www.youtube.com/watch?v=LWFzPP8ZbdU
float hash(in vec2 pos, in vec2 seed)
{
    const int Noise1 = int(0xb5297a4d);
    const int Noise2 = int(0x68e31da4);
    const int Noise3 = int(0x1b56c4e9);
    const int BigPrime = int(198491317);
    const int Cap = 2139095039;

    int n = int(floor(pos.x)) + (BigPrime * int(floor(pos.y)));
    int s = int(floor(seed.x)) + (BigPrime * int(floor(seed.y)));

    n *= Noise1;
    n += s;
    n ^= n >> 8;
    n += Noise2;
    n ^= n << 8;
    n *= Noise3;
    n ^= n >> 8;
    float rng = float(n % Cap);
    return rng / float(Cap);
}

// https://iquilezles.org/www/articles/fbmsdf/fbmsdf.htm
/**
 * Random circle
 */
float circ( in vec2 i, vec2 f, in vec2 c )
{
    float r = 0.5*hash(i+c, vec2(0.2,1));
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