// Seven
// https://github.com/53V3N1X/SevenEngine
// LISCENSE: See "LISCENSE.txt" in th root project directory.
// SUPPORT: See "README.txt" in the root project directory.

//using System;
using Seven;
using Seven.Structures;

namespace SevenEngine.Shaders
{
  /// <summary>Represents a single vertex shader that has been loaded on the GPU.</summary>
  public class VertexShader
  {
    protected string _filePath;
    protected string _id;
    protected int _gpuHandle;
    protected int _existingReferences;

    /// <summary>The file path form which the shader was loaded.</summary>
    public string FilePath { get { return _filePath; } set { _filePath = value; } }
    /// <summary>The name associated with this shader when it was loaded.</summary>
    public string Id { get { return _id; } set { _id = value; } }
    /// <summary>The location of the shader program on the GPU.</summary>
    public int GpuHandle { get { return _gpuHandle; } set { _gpuHandle = value; } }
    /// <summary>The number of existing hardware instances of this model reference.</summary>
    public int ExistingReferences { get { return _existingReferences; } set { _existingReferences = value; } }

    /// <summary>Creates an instance of a GPU referencing class for a vertex shader.</summary>
    /// <param name="id">The id of this vertex shader used for look-up purposes.</param>
    /// <param name="filePath">The filepath from which this vertex shader was loaded.</param>
    /// <param name="gpuHandle">The GPU handle or location where the memory starts on VRAM.</param>
    public VertexShader(string id, string filePath, int gpuHandle)
    {
      _id = id;
      _filePath = filePath;
      _gpuHandle = gpuHandle;
      _existingReferences = 0;
    }

    public static Comparison CompareTo(VertexShader left, VertexShader right)
    {
      int comparison = left.Id.CompareTo(right.Id);
      if (comparison > 0)
        return Comparison.Greater;
      else if (comparison < 0)
        return Comparison.Less;
      else
        return Comparison.Equal;
    }

    public static Comparison CompareTo(VertexShader left, string right)
    {
      int comparison = left.Id.CompareTo(right);
      if (comparison > 0)
        return Comparison.Greater;
      else if (comparison < 0)
        return Comparison.Less;
      else
        return Comparison.Equal;
    }

    #region Vertex Shader Library
    internal const string StaticModelPhong =
    @"uniform mat4 transformation;
    varying vec3 light;
    varying vec3 normal;
    void main()
    {
      normal = normalize(gl_NormalMatrix * gl_Normal);
      light = normalize(vec3(gl_LightSource[0].position));
      gl_TexCoord[0] = gl_MultiTexCoord0;
      gl_Position = gl_Vertex * transformation;
    }";

    internal const string UniformTransform =
    @"uniform mat4 transformation;
    void main()
    {
      gl_position = transformation * gl_Vertex;
    }";

    internal const string FTransform =
    @"void main()
    {
      gl_Position = ftransform();
    }";

    internal const string Texture =
    @"void main()
    {
	    gl_Position = ftransform();
	    gl_TexCoord[0] = gl_MultiTexCoord0;
    }";

    internal const string Light =
    @"varying vec3 light;
    varying vec3 normal;

    void main()
    {
      normal = normalize(gl_NormalMatrix * gl_Normal);
      light = normalize(vec3(gl_LightSource[0].position));
      gl_TexCoord[0] = gl_MultiTexCoord0;
      gl_Position = ftransform();
    }";

    internal const string Transformation =
    @"uniform mat4 transformation;
    varying vec3 normal;

    void main()
    {
      normal = normalize(gl_NormalMatrix * gl_Normal);
      light = normalize(vec3(gl_LightSource[0].position));
      gl_TexCoord[0] = gl_MultiTexCoord0;
      gl_Position = ftransform();
    }";
    #endregion
  }
}