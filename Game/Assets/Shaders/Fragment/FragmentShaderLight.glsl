in vec2 texCoord;
in vec3 normal;

out vec4 outputColor;

uniform sampler2D shaderTexture;
uniform vec3 lightDirection;
uniform vec4 diffuseLightColor;
uniform vec4 ambientLight;

void main(void)
{
	vec4 textureColor;
	vec4 color;
	vec3 lightDir;
	float lightIntensity;


	// Sample the pixel color from the texture using the sampler at this texture coordinate location.
	textureColor = texture(shaderTexture, texCoord);

	// Set the default output color to the ambient light value for all pixels.
	color = ambientLight;

	// Invert the light direction for calculations.
	lightDir = -lightDirection;

	// Calculate the amount of light on this pixel.
	lightIntensity = clamp(dot(normal, lightDir), 0.0f, 1.0f);

	if(lightIntensity > 0.0f) {
	  // Determine the final diffuse color based on the diffuse color and the amount of light intensity.
      color += (diffuseLightColor * lightIntensity); }

	// Clamp the final light color.
	color = clamp(color, 0.0f, 1.0f);

	// Multiply the texture pixel and the final diffuse color to get the final pixel color result.
	outputColor = color * textureColor;
}