shader_type canvas_item;

#include "sdf_noise.gdshaderinc"

global uniform float UNSCALED_TIME;

uniform vec2 animate_direction = vec2(1,1);

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

mat2 y_rot(float angle) {
	return mat2(
		vec2(cos(angle), -sin(angle)),
		vec2(sin(angle), cos(angle))
	);
}

float sdf_map(in vec2 pos, in vec2 uv) {
    float sphere_dist = length(pos) - sphere_radius;

    pos = y_rot(length(pos) * 10.0) * pos;

    // Movement
    vec2 noise_offset = vec2(TIME * .1);

    pos = pos * noise_scale + noise_offset * animate_direction;

    sphere_dist = sdFbm(pos, detail, rough, inflate, smooth_fac, sphere_dist);

    return -sphere_dist;
}

void fragment() {
    vec2 cur_pos = UV - .5;

	for (int i = 0; i < 50; i++) {

		float offset = sdf_map(cur_pos, UV);

        // Ignore if the difference is too small
		if (offset < -.005) {break;}

		cur_pos += offset * normalize(cur_pos) * step_mult;
	}

	float cur_dist = sdf_map(cur_pos, UV);

    float alpha = 1. - clamp(cur_dist * -75., 0., 1.);
	COLOR.a = pow(alpha, 2.);

    // Setup color
    float vol_depth = distance(cur_pos, UV - .5);

	float col_mix_fac = pow( exp(-col_distance_scale * vol_depth), 1.5);

    vec3 vol_color = mix(col_far, col_near, col_mix_fac);

    COLOR.rgb *= vol_color * emission_boost;
}
