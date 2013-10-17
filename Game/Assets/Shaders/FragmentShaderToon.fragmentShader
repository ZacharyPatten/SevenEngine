//uniform vec3 lightDir;
varying vec3 normal;
uniform sampler2D tex;

void main()
{
    float intensity;
    vec3 lightDir = vec3(1, 1, 1);
	intensity = dot(lightDir,normalize(normal));

	if (intensity > 0.95)
		intensity = .9;
	else if (intensity > 0.5)
		intensity = .5;
	else if (intensity > 0.25)
		intensity = .25;
	else
		intensity = 1;
	gl_FragColor = intensity  * texture2D(tex, gl_TexCoord[0].st);
}