using System.Runtime.InteropServices;

namespace Engine.Shaders
{
  [StructLayout(LayoutKind.Sequential)]
  public class GeometryShader
  {
    protected string _filePath;
    protected string _id;
    protected int _handle;

    protected int _existingReferences;

    /// <summary>The file path form which the shader was loaded.</summary>
    public string FilePath { get { return _filePath; } set { _filePath = value; } }
    /// <summary>The name associated with this shader when it was loaded.</summary>
    public string Id { get { return _id; } set { _id = value; } }
    /// <summary>The location of the shader program on the GPU.</summary>
    public int Handle { get { return _handle; } set { _handle = value; } }

    /// <summary>The number of existing hardware instances of this model reference.</summary>
    public int ExistingReferences { get { return _existingReferences; } set { _existingReferences = value; } }

    public GeometryShader(string id, string filePath, int handle)
    {
      _id = id;
      _filePath = filePath;
      _handle = handle;
      _existingReferences = 0;
    }
  }
}