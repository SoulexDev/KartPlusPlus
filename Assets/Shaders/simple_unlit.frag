#version 460 core
out vec4 FragColor;

in vec2 texCoords;

uniform sampler2D uMainTex;

void main(){
	FragColor = texture(uMainTex, texCoords);
}