shader_type canvas_item;

uniform vec4 circle_color : source_color = vec4(0);

group_uniforms Glow;
uniform vec4 glow_color : source_color = vec4(1);
uniform float glow_radius = float(2);
uniform float glow_size = float(1.6);

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
    vec4 color = vec4(0);

    // center uv (0, 1) -> (-1, 1)
    vec2 uv = (UV - .5) * 2.;

    // circle
    float alpha = floor(2. - length(uv));
    if (alpha <= 0.) // optimisation (skip glow if outside circle)
        discard;
    color += vec4(circle_color.rgb, circle_color.a * alpha);

    // glow
    float value = max(glow_radius - glow_size / length(uv), 0);
    color += glow_color * value;

    // final multiply
    COLOR = color * texture(TEXTURE, UV) * COLOR;
}
