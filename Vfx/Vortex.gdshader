shader_type canvas_item;

uniform vec2 resolution = vec2(600.0, 400.0);
uniform float color_steps = 5.0;
uniform vec3 spiral_color: source_color = vec3(0.2, 0.6, 0.3);
uniform float frequency: hint_range(1.0, 10.0, 0.1) = 8.0;
uniform float speed: hint_range(1.0, 20.0, 0.1) = 8.0;
uniform float brightness: hint_range(1.0, 10.0, 0.1) = 5.0;
uniform float balance: hint_range(0.0, 1.0, 0.01) = 0.5;
uniform float octaves: hint_range(1.0, 10.0, 1.0) = 4.0;
uniform sampler2D noise;
uniform float noise_scale = 0.1;

vec3 hueShift(vec3 color, float hueAdjust) {
	const vec3 kRGBToYPrime = vec3(0.299, 0.587, 0.114);
	const vec3 kRGBToI = vec3(0.596, -0.275, -0.321);
	const vec3 kRGBToQ = vec3(0.212, -0.523, 0.311);

	const vec3 kYIQToR = vec3(1.0, 0.956, 0.621);
	const vec3 kYIQToG = vec3(1.0, -0.272, -0.647);
	const vec3 kYIQToB = vec3(1.0, -1.107, 1.704);

	float YPrime = dot(color, kRGBToYPrime);
	float I = dot(color, kRGBToI);
	float Q = dot(color, kRGBToQ);
	float hue = atan(Q, I);
	float chroma = sqrt(I * I + Q * Q);

	hue += hueAdjust;

	Q = chroma * sin(hue);
	I = chroma * cos(hue);

	vec3 yIQ = vec3(YPrime, I, Q);

	return vec3(dot(yIQ, kYIQToR), dot(yIQ, kYIQToG), dot(yIQ, kYIQToB));
}


float draw_spiral(vec2 uv, float rotation, vec2 trueuv) {
	float uv_radius = length(uv);
	float wobble = texture(noise, trueuv).r * noise_scale;

    uv_radius += wobble;
	//return uv_radius;
	float uv_phi = atan(uv.y, uv.x) + rotation;
	float spiral_phi = (log(uv_radius) * frequency - uv_phi) / TAU;
	float spiral_ceil_radius = exp((TAU * ceil(spiral_phi) + uv_phi) / frequency);
	float spiral_floor_radius = exp((TAU * floor(spiral_phi) + uv_phi) / frequency);
	return mix(mix(abs(uv_radius - spiral_ceil_radius), abs(uv_radius - spiral_floor_radius), balance), max(abs(uv_radius - spiral_ceil_radius), abs(uv_radius - spiral_floor_radius)), balance);
}

void fragment() {
	vec2 uv = UV;
	uv -= 0.5 ;
    uv = uv * 2.0;

	//wobble the uv


	//pixelate the uv
    uv = floor(uv * resolution) / resolution;

	float spiral = 0.0;
	for (float i = 0.0; i < octaves; i += 1.0) {
		spiral += draw_spiral(uv, TIME * speed * (0.5 + sin(i)),UV);
	}
	spiral /= octaves;
	vec3 color = spiral * spiral_color;
	//hue shift depending on the distance from the center
	color = hueShift(color, length(uv) * PI);

    //quantize the color brightness
    color = floor(color * color_steps) / color_steps;



	COLOR = vec4(color * brightness, 1.0);
}

