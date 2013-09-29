using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

    int CompileShaders()
    {
      int programHandle, vHandle, fHandle;
      vHandle = GL.CreateShader(ShaderType.VertexShader);
      fHandle = GL.CreateShader(ShaderType.FragmentShader);
      GL.ShaderSource(vHandle, vShaderSource);
      GL.ShaderSource(fHandle, fShaderSource);
      GL.CompileShader(vHandle);
      GL.CompileShader(fHandle);
      Console.Write(GL.GetShaderInfoLog(vHandle));
      Console.Write(GL.GetShaderInfoLog(fHandle));

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
