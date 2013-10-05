using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine
{
  public class Shader
  {
    protected string _filePath;
    protected int _id;
    protected ShaderType _type;

    /// <summary>The file path form which the shader was loaded.</summary>
    public string FilePath { get { return _filePath; } set { _filePath = value; } }
    /// <summary>The location of the shader program on the GPU.</summary>
    public int Id { get { return _id; } set { _id = value; } }
    /// <summary>The type of the shader (Vertex, Fragment, etc.).</summary>
    public ShaderType Type { get { return _type; } set { _type = value; } }

    public Shader(string filePath, int id, ShaderType shaderType)
    {
      _filePath = filePath;
      Id = id;
      Type = shaderType;
    }
  }
}
