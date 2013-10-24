using System.Runtime.InteropServices;

namespace SevenEngine.Shaders
{
  /// <summary>Represents a single vertex shader that has been loaded on the GPU. multiple references of this class SHOULD exist,
  /// because each reference of this class means another hardware instance. Hardware instancing is when you re-use the
  /// same buffers on the GPU, which is good for with speed and memory space.</summary>
  [StructLayout(LayoutKind.Sequential)]
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
  }
}