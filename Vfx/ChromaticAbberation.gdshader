shader_type canvas_item;

uniform float strength : hint_range(0.0, 1.0);
uniform float Timescale : hint_range(0.0, 4.0);

uniform sampler2D SCREEN_TEXTURE : hint_screen_texture;
void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	// Called for every pixel on the material.

	//rotate the direction of the abberration
	vec2 dir = vec2(cos(TIME*Timescale), sin(TIME*Timescale));
	// apply the direction to the red, green and blue channels
	vec2 REDDIR = vec2(1.0, 0.0) * dir;
	vec2 GREENDIR = vec2(0.0, 1.0) * dir;
	vec2 BLUEDIR = vec2(0.0, 0.0) * dir;




	//get the color of the screen texture

	
	float red = texture(SCREEN_TEXTURE, SCREEN_UV + REDDIR * strength).r;
	float green = texture(SCREEN_TEXTURE, SCREEN_UV + GREENDIR * strength).g;
	float blue = texture(SCREEN_TEXTURE, SCREEN_UV + BLUEDIR * strength).b;
	float alpha =  red + green + blue/3.0;

	COLOR = vec4(red, green, blue, alpha);

}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
