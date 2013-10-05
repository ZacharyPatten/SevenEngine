using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine
{
  public class ShaderManager : IDisposable
  {
    Dictionary<string, Shader> _shaderDatabase = new Dictionary<string, Shader>();

    uint _programId;

    public ShaderManager()
    {
    }

    public void AddShader()
    {
      GL.UseProgram(CompileShaders());
    }

    public void SelectVertexShader(string shaderId)
    {

    }

    public Shader Get(string shaderId)
    {
      return _shaderDatabase[shaderId];
    }

    private bool LoadShaderFromDisk(string path, out int id, out int height, out int width)
    {
      throw new NotImplementedException();
    }

    public void LoadShader(string shaderId, string path)
    {
      throw new NotImplementedException();
    }
    
    string vShaderSource =
@"void main() {
	gl_Position = ftransform();
	gl_TexCoord[0] = gl_MultiTexCoord0;
}
";

    string fShaderSource =
@"uniform sampler2D tex;
void main() {
	gl_FragColor = texture2D(tex, gl_TexCoord[0].st);
}
";

    string vShaderSource2 =
@"varying float intensity;

void main()
{
	vec3 lightDir = normalize(vec3(gl_LightSource[0].position));
	intensity = dot(lightDir,gl_Normal);

	gl_Position = ftransform();
}";

    string fShaderSource2 =
@"varying float intensity;

void main()
{
	vec4 color;
	if (intensity > 0.95)

		color = vec4(1.0,0.5,0.5,1.0);
	else if (intensity > 0.5)
		color = vec4(0.6,0.3,0.3,1.0);
	else if (intensity > 0.25)
		color = vec4(0.4,0.2,0.2,1.0);
	else
		color = vec4(0.2,0.1,0.1,1.0);
	gl_FragColor = color;

}";

    string vShaderSource3 =
@"varying vec3 normal;

void main()
{
	normal = gl_Normal;
	gl_Position = ftransform();

}";

    string fShaderSource3 =
@"//uniform vec3 lightDir;
varying vec3 normal;

void main()
{
    float intensity;
	vec4 color;
    vec3 lightDir = vec3(1, 1, 1);
	intensity = dot(lightDir,normalize(normal));

	if (intensity > 0.95)
		color = vec4(1.0,0.5,0.5,1.0);
	else if (intensity > 0.5)
		color = vec4(0.6,0.3,0.3,1.0);
	else if (intensity > 0.25)
		color = vec4(0.4,0.2,0.2,1.0);
	else
		color = vec4(0.2,0.1,0.1,1.0);
	gl_FragColor = color;

}";

    string vShaderSource4 =
"attribute vec3 vertex;" +
"attribute vec3 normal;" +
"attribute vec2 uv1;" +

"uniform mat4 _mvProj;" +
"uniform mat3 _norm;" +
"uniform float _time;" +

"varying vec2 uv;" +
"varying vec3 vColor;\n" +

"#pragma include \"light.glsl\"\n" +

// constants
"vec3 materialColor = vec3(1.0,0.7,0.8);" +
"vec3 specularColor = vec3(1.0,1.0,1.0);" +

"void main(void) {" +
	// compute position
"	gl_Position = _mvProj * vec4(vertex, 1.0);" +

"	uv = uv1;" +
	// compute light info
"	vec3 n = normalize(_norm * normal);" +
"	vec3 diffuse;" +
"	float specular;" +
"	float glowingSpecular = sin(_time*0.003)*20.0+20.0;" +
"	getDirectionalLight(n, _dLight, glowingSpecular, diffuse, specular);" +
"	vColor = max(diffuse,_ambient.xyz)*materialColor+specular*specularColor;" +
"} ";

    string fShaderSource4 =
@"#ifdef GL_ES
precision highp float;
#endif
varying vec3 vColor;
varying vec2 uv;

uniform sampler2D tex;

void main(void)
{
	gl_FragColor = texture2D(tex,uv)*vec4(vColor, 1.0);
}";

    int CompileShaders()
    {
      string vShader;
      string fShader;
      //using (StreamReader reader = new StreamReader(@"C:\Users\Seven\Programming\SevenEngine\Engine\Shaders\VertexShaderLit.VertexShader"))
      //using (StreamReader reader = new StreamReader(@"C:\Users\Seven\Programming\SevenEngine\Engine\Shaders\VertexShaderToon.VertexShader"))
      using (StreamReader reader = new StreamReader(@"C:\Users\Seven\Programming\SevenEngine\Engine\Shaders\VertexShaderBasic.VertexShader"))
      {
        vShader = reader.ReadToEnd();
      }
      //using (StreamReader reader = new StreamReader(@"C:\Users\Seven\Programming\SevenEngine\Engine\Shaders\FragmentShaderLit.FragmentShader"))
      //using (StreamReader reader = new StreamReader(@"C:\Users\Seven\Programming\SevenEngine\Engine\Shaders\FragmentShaderToon.FragmentShader"))
      using (StreamReader reader = new StreamReader(@"C:\Users\Seven\Programming\SevenEngine\Engine\Shaders\FragmentShaderBasic.FragmentShader"))
      {
        fShader = reader.ReadToEnd();
      }

      int programHandle, vHandle, fHandle;
      vHandle = GL.CreateShader(ShaderType.VertexShader);
      fHandle = GL.CreateShader(ShaderType.FragmentShader);
      GL.ShaderSource(vHandle, vShader);
      GL.ShaderSource(fHandle, fShader);
      GL.CompileShader(vHandle);
      GL.CompileShader(fHandle);
      Output.Print(GL.GetShaderInfoLog(vHandle));
      Output.Print(GL.GetShaderInfoLog(fHandle));

      programHandle = GL.CreateProgram();
      GL.AttachShader(programHandle, vHandle);
      GL.AttachShader(programHandle, fHandle);
      GL.LinkProgram(programHandle);
      Console.Write(GL.GetProgramInfoLog(programHandle));
      return programHandle;
    }

    #region IDisposable Members

    public void Dispose()
    {
      foreach (Shader t in _shaderDatabase.Values)
      {
        throw new NotImplementedException();
      }
    }

    #endregion

  }
}
