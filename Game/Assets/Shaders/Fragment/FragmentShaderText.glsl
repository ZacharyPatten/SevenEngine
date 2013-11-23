uniform sampler2D texture;
uniform vec4 color;

void main() {
	gl_FragColor = texture2D(texture, gl_TexCoord[0].st) + color;
}