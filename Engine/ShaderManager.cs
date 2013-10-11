using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine
{
  public static class ShaderManager
  {
    private static Dictionary<string, Shader> _shaderDatabase = new Dictionary<string, Shader>();
    private static Dictionary<string, Shader> _programDatabase = new Dictionary<string, Shader>();

    private static int _programId;

    public static void AddShader()
    {
      GL.UseProgram(CompileShaders());
    }

    public static void SelectVertexShader(string shaderId)
    {

    }

    public static Shader Get(string shaderId)
    {
      return _shaderDatabase[shaderId];
    }

    private static bool LoadShaderFromDisk(string path, out int id, out int height, out int width)
    {
      throw new NotImplementedException();
    }

    public static void LoadShader(string shaderId, string path)
    {
      throw new NotImplementedException();
    }

    private static string vertexShader =
@"void main() {
	gl_Position = ftransform();
	gl_TexCoord[0] = gl_MultiTexCoord0;
}";
    private static string fragmentshader =
@"uniform sampler2D tex;
void main() {
	gl_FragColor = texture2D(tex, gl_TexCoord[0].st);
}";

    private static int CompileShaders()
    {
      /*string vShader;
      string fShader;
      //using (StreamReader reader = new StreamReader(@"C:\Users\Seven\Programming\SevenEngine\Engine\Shaders\VertexShaderLit.VertexShader"))
      //using (StreamReader reader = new StreamReader(@"C:\Users\Seven\Programming\SevenEngine\Engine\Shaders\VertexShaderToon.VertexShader"))
      using (StreamReader reader = new StreamReader(@"C:\Users\Seven\Programming\SevenEngine\Engine\Shaders\VertexShaderBasic.VertexShader"))
      {
        vShader = reader.ReadToEnd();
      }
      //using (StreamReader reader = new StreamReader(@"C:\Users\Seven\Programming\SevenEngine\Engine\Shaders\FragmentShaderLit.FragmentShader"))
      //using (StreamReader reader = new StreamReader(@"C:\Users\Seven\Programming\SevenEngine\Engine\Shaders\FragmentShaderToon.FragmentShader"))
      using (StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + @"\..\..\Shaders\FragmentShaderBasic.FragmentShader"))
      {
        fShader = reader.ReadToEnd();
      }*/

      int programHandle, vHandle, fHandle;
      vHandle = GL.CreateShader(ShaderType.VertexShader);
      fHandle = GL.CreateShader(ShaderType.FragmentShader);
      GL.ShaderSource(vHandle, vertexShader);
      GL.ShaderSource(fHandle, fragmentshader);
      GL.CompileShader(vHandle);
      GL.CompileShader(fHandle);
      Output.Write(GL.GetShaderInfoLog(vHandle));
      Output.Write(GL.GetShaderInfoLog(fHandle));

      programHandle = GL.CreateProgram();
      GL.AttachShader(programHandle, vHandle);
      GL.AttachShader(programHandle, fHandle);
      GL.LinkProgram(programHandle);
      Console.Write(GL.GetProgramInfoLog(programHandle));

      Output.Write("Basic Vertex Shader Compiled;");
      Output.Write("Basic Vertex Shader Selected;");
      Output.Write("Basic Fragment Shader Compiled;");
      Output.Write("Basic Fragment Shader Selected;");

      return programHandle;
    }
  }
}
