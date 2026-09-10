#version 460 core

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNorm;
layout (location = 2) in vec2 aUv;

out vec2 texCoords;

uniform mat4 uModel;
uniform mat4 uView;
uniform mat4 uProjection;

void main(){
	texCoords = aUv;
	gl_Position = uProjection * uView * uModel * vec4(aPos, 1.0);
}