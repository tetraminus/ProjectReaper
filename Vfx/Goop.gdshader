shader_type canvas_item;

uniform sampler2D SCREEN_TEXTURE : hint_screen_texture;
uniform sampler2D NOISE_TEXTURE;

varying float distanceToVertex;

void vertex() {

	// store the distance to the vertex in the varying variable
	distanceToVertex = distance(VERTEX, vec2(0.0, 0.0));
}

void fragment() {
	vec4 original = COLOR;
	vec2 pixelUV = SCREEN_UV;
	//pixelize 480 x 270
	pixelUV.x = floor(pixelUV.x * 480.0) / 480.0;
	pixelUV.y = floor(pixelUV.y * 270.0) / 270.0;

	vec4 c = texture(NOISE_TEXTURE, mod(pixelUV*4.0, 1.0));
	c = c.x < 0.1 ? vec4(1.0, 1.0, 1.0, 1.0) : vec4(0.0, 0.0, 0.0, 1.0);
	//c.a = distanceToVertex;

	COLOR = c;
}

void light() {


	
	
}
