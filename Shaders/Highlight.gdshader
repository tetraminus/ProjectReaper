shader_type canvas_item;
render_mode unshaded;

uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_nearest;
uniform bool on;
void fragment() {
	vec4 c = textureLod(screen_texture, SCREEN_UV, 0.0);

	if (c.a > 0.0001) {
		c.rgb /= c.a;
	}
	
	c += vec4(1.0, 1.0, 1.0, 1.0)*0.4 * (c.a) * (on ? 1.0 : 0.0); 

	COLOR *= c;
}