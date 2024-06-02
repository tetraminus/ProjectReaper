shader_type canvas_item;
uniform sampler2D SCREEN_TEXTURE : hint_screen_texture;


uniform sampler2D smoke;
uniform float speed = 0.3;
uniform float size = 0.08;

void fragment(){

	vec2 smoke_uv = UV + TIME * speed;
	vec4 smoke_color = texture(smoke, fract(smoke_uv));
	smoke_color = clamp(smoke_color * size, 0.0, 1.0);
	vec2 resolution = 1.0 / SCREEN_PIXEL_SIZE;
	vec2 localtoscreenscale = (SCREEN_UV * resolution) / ( (UV * resolution) - FRAGCOORD.xy);



	vec4 img_color = texture(SCREEN_TEXTURE, SCREEN_UV + vec2(smoke_color.g - size/2.0,0.0) * localtoscreenscale);

	COLOR = vec4(img_color);
}