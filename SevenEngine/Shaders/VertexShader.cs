// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use under the following condition: any code using or originally taken 
// from the SevenEngine project must include citation to its original author(s) located at the
// top of each source code file, or you may include a reference to the SevenEngine project as
// a whole but you must include the current SevenEngine official website URL and logo.
// - Thanks.  :)  (support: seven@sevenengine.com)

// Author(s):
// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com
// Last Edited: 11-16-13

using SevenEngine.DataStructures.Interfaces;

namespace SevenEngine.Shaders
{
  /// <summary>Represents a single vertex shader that has been loaded on the GPU.</summary>
  public class VertexShader : InterfaceStringId
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

    public int CompareTo(VertexShader right) { return _id.CompareTo(right.Id); }
    public int CompareTo(string right) { return _id.CompareTo(right); }

    public static int CompareTo(VertexShader left, VertexShader right) { return left.Id.CompareTo(right.Id); }
    public static int CompareTo(VertexShader left, string right) { return left.Id.CompareTo(right); }

    #region Vertex Shader Library
    internal static readonly string Transform =
      @"void main() {
          gl_Position = ftransform();
      }";

    internal static readonly string Texture =
      @"void main() {
	      gl_Position = ftransform();
	      gl_TexCoord[0] = gl_MultiTexCoord0;
      }";
    #endregion
  }
}