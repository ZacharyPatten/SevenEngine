// Seven
// https://github.com/53V3N1X/SevenEngine
// LISCENSE: See "LISCENSE.txt" in th root project directory.
// SUPPORT: See "README.txt" in the root project directory.

using Seven;
using Seven.Structures;

namespace SevenEngine.Shaders
{
  /// <summary>Represents a single extended geometry shader that has been loaded on the GPU.</summary>
  public class ExtendedGeometryShader
  {
    protected string _id;
    protected string _filePath;
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

    /// <summary>Creates an instance of a GPU referencing class for an extended geometry shader.</summary>
    /// <param name="id">The id of this extended geometry shader used for look-up purposes.</param>
    /// <param name="filePath">The filepath where this shader was loaded from.</param>
    /// <param name="gpuHandle">The GPU handle or location where its memory starts on VRAM.</param>
    public ExtendedGeometryShader(string id, string filePath, int gpuHandle)
    {
      _id = id;
      _filePath = filePath;
      _gpuHandle = gpuHandle;
      _existingReferences = 0;
    }

    public static Comparison CompareTo(ExtendedGeometryShader left, ExtendedGeometryShader right)
    {
      int comparison = left.Id.CompareTo(right.Id);
      if (comparison > 0)
        return Comparison.Greater;
      else if (comparison < 0)
        return Comparison.Less;
      else
        return Comparison.Equal;
    }

    public static Comparison CompareTo(ExtendedGeometryShader left, string right)
    {
      int comparison = left.Id.CompareTo(right);
      if (comparison > 0)
        return Comparison.Greater;
      else if (comparison < 0)
        return Comparison.Less;
      else
        return Comparison.Equal;
    }
  }
}