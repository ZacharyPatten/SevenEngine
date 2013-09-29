using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine
{
  public struct Shader
  {
    public int Id { get; set; }
    public ShaderType Type { get; set; }

    public Shader(int id, ShaderType shaderType) : this()
    {
      Id = id;
      Type = shaderType;
    }
  }
}
