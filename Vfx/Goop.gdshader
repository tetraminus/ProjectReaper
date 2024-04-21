shader_type canvas_item;

uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, filter_nearest;
uniform sampler2D NOISE_TEXTURE;
uniform sampler2D abberration_texture : repeat_enable;
uniform float scale : hint_range(0.0, 8.0);
uniform float threshold : hint_range(0.0, 1.0);
uniform float Aberration : hint_range(0.0, 1.0);
uniform bool pixelize;


void fragment() {
	vec4 original = COLOR;
	vec2 pixelUV = SCREEN_UV;
	vec2 purePixelUV = SCREEN_UV;

	
	pixelUV.y += TIME * 0.1;
	pixelUV.x += sin(TIME* 0.5)/ 50.;
	//pixelize 480 x 270
	
	pixelUV.x = floor(pixelUV.x * 480.0) / 480.0;
	pixelUV.y = floor(pixelUV.y * 270.0) / 270.0;
	if (pixelize) {
		purePixelUV.x = floor(purePixelUV.x * 480.0) / 480.0;
		purePixelUV.y = floor(purePixelUV.y * 270.0) / 270.0;
	} else {
		purePixelUV = SCREEN_UV;
	}
	
	
	

	//vec4 c = texture(NOISE_TEXTURE, mod(pixelUV*4.0, 1.0));
	
	//c.a = distanceToVertex;

	// Chromatic Aberration based on noise texture and time
	vec2 abberrationuv = pixelUV;
	abberrationuv.x += TIME * 0.1;

	float offset = Aberration * 0.01 * texture(abberration_texture, abberrationuv).r;
	

	vec4 red = texture(NOISE_TEXTURE, mod(pixelUV*scale + offset, 1.0));
	vec4 green = texture(NOISE_TEXTURE, mod(pixelUV*scale, 1.0));
	vec4 blue = texture(NOISE_TEXTURE, mod(pixelUV*scale - offset, 1.0));
	red = red.x < threshold ? vec4(1.0, 1.0, 1.0, 1.0) : vec4(0.0, 0.0, 0.0, 1.0);
	green = green.x < threshold ? vec4(1.0, 1.0, 1.0, 1.0) : vec4(0.0, 0.0, 0.0, 1.0);
	blue = blue.x < threshold ? vec4(1.0, 1.0, 1.0, 1.0) : vec4(0.0, 0.0, 0.0, 1.0);


	COLOR = vec4(red.r, green.g, blue.b, texture(SCREEN_TEXTURE, purePixelUV).a);
}

void light() {


	
	
}
