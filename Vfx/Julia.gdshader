shader_type canvas_item;
uniform float alpha = 1.0;

void vertex() {
    
	
}

vec3 JuliaSet(vec2 c, vec2 z, float max_iter) {
    float iter = 0.0;
    for (int i = 0; i < int(max_iter); i++) {
        z = vec2(z.x * z.x - z.y * z.y, 2.0 * z.x * z.y) + c;
        if (length(z) > 2.0) {
            iter = float(i);
            break;
        }
    }
    return vec3(iter / max_iter);
}


void fragment() {
	vec2 uv = UV;
	//animation
    vec2 c = vec2(sin(TIME/2.1), cos(TIME));
    
    
    vec2 z = vec2(uv.x * 3.0 - 1.5, uv.y * 2.0 - 1.0);
    vec3 color = JuliaSet(c, z, 50.0);
    COLOR = vec4(color, alpha);
	
}

