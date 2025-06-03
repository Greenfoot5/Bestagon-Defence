shader_type spatial;

// http://iquilezles.org/www/articles/smin/smin.htm
float smin( float a, float b, float k )
{
    float h = max(k-abs(a-b),0.0);
    return min(a, b) - h*h*0.25/k;
}

// http://iquilezles.org/www/articles/smin/smin.htm
float smax( float a, float b, float k )
{
    float h = max(k-abs(a-b),0.0);
    return max(a, b) + h*h*0.25/k;
}

// https://iquilezles.org/www/articles/fbmsdf/fbmsdf.htm
float sph( vec3 i, vec3 f, vec3 c )
{
    // random radius at grid vertex i+c (please replace this hash by
    // something better if you plan to use this for a real application)
    vec3  p = 17.0*fract( (i+c)*0.3183099+vec3(0.11,0.17,0.13) );
    float w = fract( p.x*p.y*p.z*(p.x+p.y+p.z) );
    float r = 0.7*w*w;
    // distance to sphere at grid vertex i+c
    return length(f-c) - r;
}

// https://iquilezles.org/www/articles/fbmsdf/fbmsdf.htm
float sdBase( in vec3 p )
{
    vec3 i = floor(p);
    vec3 f = fract(p);
    return min(min(min(sph(i,f,vec3(0,0,0)),
    sph(i,f,vec3(0,0,1))),
    min(sph(i,f,vec3(0,1,0)),
    sph(i,f,vec3(0,1,1)))),
    min(min(sph(i,f,vec3(1,0,0)),
    sph(i,f,vec3(1,0,1))),
    min(sph(i,f,vec3(1,1,0)),
    sph(i,f,vec3(1,1,1)))));
}

// https://iquilezles.org/www/articles/fbmsdf/fbmsdf.htm
float sdFbm( 
    in vec3 p, 
    in int detail, 
    in float rough, 
    in float inflate, 
    in float smooth_fac, 
    in float d)
{
    float s = 1.0;
    for( int i=0; i < min(int(detail), 12); i++ )
    {
        float n = s*sdBase(p);

        n = smax(n,d - inflate*s, smooth_fac*s );
        d = smin(n,d      , smooth_fac*s );

        p = mat3(vec3( 0.00, 1.60, 1.20),
                 vec3(-1.60, 0.72,-0.96),
                 vec3(-1.20,-0.96, 1.280))*p;
        s = rough*s;
    }
    return d;
}

render_mode unshaded;
render_mode depth_test_disabled, cull_front;

uniform sampler2D depth_tex : hint_depth_texture;

group_uniforms Color;

uniform float col_distance_scale = 1.0;
uniform vec3 col_near : source_color = vec3(1.0);
uniform vec3 col_far : source_color = vec3(0.0);
uniform float emission_boost = 1.0;

group_uniforms;

group_uniforms SDFNoise;

uniform float sphere_radius = 0.5;

uniform float noise_scale = 1.0;
uniform int detail = 4;
uniform float rough = 0.5;
uniform float inflate = 0.1;
uniform float smooth_fac = 0.2;

uniform float step_mult = 1.0;

group_uniforms;

mat3 y_rot(float angle) {
	return mat3(
		vec3(cos(angle), 0.0, -sin(angle)),
		vec3(0.0, 1.0, 0.0),
		vec3(sin(angle), 0.0, cos(angle))
	);
}

float sdf_map(vec3 pos) {
	float sphere_dist = length(pos) - sphere_radius;
	
	vec3 noise_offset = vec3(0.0, -TIME * 0.1, 0.0);
	
	pos = y_rot(pos.y * 10.0) * pos;
	
	pos = pos * noise_scale + noise_offset;
	
	sphere_dist = sdFbm(pos, detail, rough, inflate, smooth_fac, sphere_dist);
	
	return -sphere_dist;
}

void fragment() {
	float depth = textureLod(depth_tex, SCREEN_UV, 0.0).r;
	vec4 upos = INV_PROJECTION_MATRIX * vec4(SCREEN_UV * 2.0 - 1.0, depth, 1.0);
	
	vec4 wpos = INV_VIEW_MATRIX * (upos / upos.w);
	
	vec3 obj_position = -(inverse(MODEL_MATRIX) * wpos).xyz;
	vec3 obj_view_vec = inverse(mat3(MODEL_MATRIX)) * mat3(INV_VIEW_MATRIX) * VIEW;
	
	vec3 view_dir = normalize(obj_view_vec);
	
	vec3 cur_pos = obj_position;
	for (int i = 0; i < 50; i++) {
		
		float offset = sdf_map(cur_pos);
		
		if (offset < -0.005) {break;}
		
		cur_pos += view_dir * offset * step_mult;
	}
	
	float cur_dist = sdf_map(cur_pos);
	
	float alpha = 1.0 - clamp(cur_dist * -100.0, 0.0, 1.0);
	ALPHA = pow(alpha, 2.0);
	
	float vol_depth = distance(obj_position, cur_pos);
	
	float col_mix_fac = pow(1.0 - exp(-col_distance_scale * vol_depth), 2.0);
	
	vec3 vol_color = mix(col_near, col_far, col_mix_fac);
	
	ALBEDO.xyz = vol_color * emission_boost;
}