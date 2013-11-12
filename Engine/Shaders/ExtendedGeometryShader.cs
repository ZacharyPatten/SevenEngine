// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use with the following condition: any code using or originally taken from the 
// SevenEngine project must include citation to its original author(s) located at the top of each
// source code file. Alternatively, you may include a reference to the SevenEngine project as a whole,
// but you must include the current SevenEngine official website URL and logo.
// - Thanks.  :)  (support: seven@sevenengine.com)

// Author(s):
// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com
// Last Edited: 10-26-13

using SevenEngine.DataStructures.Interfaces;

namespace SevenEngine.Shaders
{
  /// <summary>Represents a single extended geometry shader that has been loaded on the GPU. multiple references of this class SHOULD exist,
  /// because each reference of this class means another hardware instance. Hardware instancing is when you re-use the
  /// same buffers on the GPU, which is good for with speed and memory space.</summary>
  public class ExtendedGeometryShader : InterfaceStringId
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
  }
}