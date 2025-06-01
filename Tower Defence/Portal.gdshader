shader_type canvas_item;

#include "sdf_noise.gdshaderinc"

group_uniforms Colour;
uniform float col_distance_scale = 5.;
uniform vec3 col_near : source_color = vec3(1);
uniform vec3 col_far : source_color = vec3(0);
uniform float emission_boost = 1.;
group_uniforms;

group_uniforms SDFNoise;
/** Size of the sphere generated using the noise */
uniform float sphere_radius = 0.15;
uniform float noise_scale = 3.2;
uniform int detail = 6;
uniform float rough = 0.5;
uniform float inflate = 0.1;
uniform float smooth_fac = 0.5;
uniform float step_mult = .5;
group_uniforms;

float sdf_map(vec2 pos) {
    float sphere_dist = length(pos) - sphere_radius;

    // Movement
    vec2 noise_offset = vec2(TIME * .1);
    vec2 off_sign = -sign(vec2(pos * 1. - 1.));

    pos = pos * noise_scale + noise_offset;// * off_sign;

    sphere_dist = sdFbm(pos, detail, rough, inflate, smooth_fac, sphere_dist);

    return -sphere_dist;
}

void fragment() {
    vec2 cur_pos = UV - .5;
	for (int i = 0; i < 50; i++) {

		float offset = sdf_map(cur_pos);

		if (offset < -.005) {break;}

		cur_pos += offset * step_mult;
	}

	float cur_dist = sdf_map(cur_pos);

    float alpha = 1. - clamp(cur_dist * -75., 0., 1.);
	COLOR.a = pow(alpha, 2.);

    // Setup color
    float vol_depth = distance(UV - .5, cur_pos);

	float col_mix_fac = pow( exp(-col_distance_scale * vol_depth), 1.5);

    vec3 vol_color = mix(col_far, col_near, col_mix_fac);

    COLOR.rgb *= vol_color * emission_boost;
    //COLOR.rgb = vec3(cur_pos, 0.0);
}
