shader_type canvas_item;

uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, filter_nearest;
uniform sampler2D NOISE_TEXTURE;
uniform sampler2D abberration_texture : repeat_enable;
uniform float scale : hint_range(0.0, 8.0);
uniform float threshold : hint_range(0.0, 1.0);
uniform float Aberration : hint_range(0.0, 1.0);
uniform bool pixelize;
uniform bool alphaNoise = false;
uniform float alphaNoiseScale : hint_range(0.0, 8.0);


void fragment() {
	vec4 original = COLOR;
	vec2 pixelUV = SCREEN_UV;
	vec2 purePixelUV = SCREEN_UV;


	pixelUV.y += TIME * 0.1;
	pixelUV.x += sin(TIME* 0.5)/ 50.;
	//pixelize 480 x 270

	vec2 res = 1.0/SCREEN_PIXEL_SIZE;
	res /= 4.0;

	pixelUV.x = floor(pixelUV.x * res.x) / res.x;
	pixelUV.y = floor(pixelUV.y * res.y) / res.y;



	if (pixelize) {
		purePixelUV.x = floor(purePixelUV.x * res.x) / res.x;
		purePixelUV.y = floor(purePixelUV.y * res.y) / res.y;

	} else {
		purePixelUV = SCREEN_UV;
	}

	vec2 alphaUV = purePixelUV;

	if (alphaNoise) {
		// apply pixelized noise to alpha channel

		// snapped offset for animation
		float offset = TIME * 0.01;

		vec4 alphax = texture(NOISE_TEXTURE, mod(purePixelUV*scale*alphaNoiseScale + offset + 0.1, 1.0));
		vec4 alphay = texture(NOISE_TEXTURE, mod(purePixelUV*scale*alphaNoiseScale + offset, 1.0));
		//alphaUV += (vec2(alpha.r, alpha.g)-0.5 ) * 0.1;
		alphaUV.x += (alphax.r - 0.5) * 0.1;
		alphaUV.y += (alphay.g - 0.5) * 0.1;
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

	COLOR = vec4(alphaUV.x, alphaUV.y, 0.0, 1.0);

	COLOR = vec4(red.r, green.g, blue.b, texture(SCREEN_TEXTURE, alphaUV).a);
}

void light() {}
