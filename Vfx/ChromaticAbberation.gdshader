shader_type canvas_item;

uniform float strength : hint_range(0.0, 1.0);
uniform float Timescale : hint_range(0.0, 4.0);

uniform sampler2D SCREEN_TEXTURE : hint_screen_texture;


void fragment() {

	vec2 dir = vec2(cos(TIME*Timescale), sin(TIME*Timescale));

	vec2 REDDIR = vec2(1.0, 0.0) * dir;
	vec2 GREENDIR = vec2(0.0, 1.0) * dir;
	vec2 BLUEDIR = vec2(0.0, 0.0) * dir;

	float red = texture(SCREEN_TEXTURE, SCREEN_UV + REDDIR * strength).r;
	float green = texture(SCREEN_TEXTURE, SCREEN_UV + GREENDIR * strength).g;
	float blue = texture(SCREEN_TEXTURE, SCREEN_UV + BLUEDIR * strength).b;
	float alpha =  red + green + blue/3.0;

	COLOR = vec4(red, green, blue, alpha);

}
