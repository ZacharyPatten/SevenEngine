varying vec3 diffuseColor; 
// the interpolated diffuse Phong lighting
varying vec3 specularColor; 
// the interpolated specular Phong lighting
varying vec4 texCoords; 
// the interpolated texture coordinates 
uniform sampler2D textureUnit;

void main()
{
   vec2 longitudeLatitude = vec2(
     (atan(texCoords.y, texCoords.x) / 3.1415926 + 1.0) * 0.5, 
      1.0 - acos(texCoords.z) / 3.1415926);
      // unusual processing of texture coordinates

   gl_FragColor = vec4(diffuseColor 
      * vec3(texture2D(textureUnit, longitudeLatitude))
      + specularColor, 1.0);
}