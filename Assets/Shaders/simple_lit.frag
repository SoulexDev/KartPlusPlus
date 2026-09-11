#version 460 core
out vec4 FragColor;

in vec3 normal;
in vec2 texCoords;

uniform sampler2D uMainTex;

void main(){
	float nl = dot(normal, normalize(vec3(1.0, 2.0, 1.0)));
	nl *= 0.5;
	nl += 0.5;
	nl *= nl;

	FragColor = texture(uMainTex, texCoords) * nl;
}