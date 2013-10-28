attribute vec3 vertex;
attribute vec3 normal;
attribute vec2 uv1;

uniform mat4 _mvProj;
uniform mat3 _norm;
uniform float _time;

varying vec2 uv;
varying vec3 vColor;

#pragma include "light.glsl"

// constants
vec3 materialColor = vec3(1.0,0.7,0.8);
vec3 specularColor = vec3(1.0,1.0,1.0);

void main(void) {
	// compute position
	gl_Position = _mvProj * vec4(vertex, 1.0);

	uv = uv1;
	// compute light info
	vec3 n = normalize(_norm * normal);
	vec3 diffuse;
	float specular;
	float glowingSpecular = sin(_time*0.003)*20.0+20.0;
	getDirectionalLight(n, _dLight, glowingSpecular, diffuse, specular);
	vColor = max(diffuse,_ambient.xyz)*materialColor+specular*specularColor;
}