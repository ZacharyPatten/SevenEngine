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

namespace SevenEngine.Imaging
{
  /// <summary>Represents a single texture that has been loaded on the GPU. multiple references of this class SHOULD exist,
  /// because each reference of this class means another hardware instance. Hardware instancing is when you re-use the
  /// same buffers on the GPU, which is good for both speed and memory space.</summary>
  public class Texture : InterfaceStringId
  {
    protected int _existingReferences;
    protected string _id;
    protected int _gpuHandle;
    protected int _width;
    protected int _height;

    /// <summary>The number of existing hardware instances of this model reference.</summary>
    public int ExistingReferences { get { return _existingReferences; } set { _existingReferences = value; } }
    /// <summary>The string id associated with this specific texture when it was loaded.</summary>
    public string Id { get { return _id; } set { _id = value; } }
    /// <summary>The handle of the texture on the GPU.</summary>
    public int GpuHandle { get { return _gpuHandle; } set { _gpuHandle = value; } }
    /// <summary>The width of the texture.</summary>
    public int Width { get { return _width; } set { _width = value; } }
    /// <summary>The height of the texture.</summary>
    public int Height { get { return _height; } set { _height = value; } }

    /// <summary>Constructs an instance of a texture, which is a GPU mapping class.</summary>
    /// <param name="id">The id associated with this texture used for look-up purposes.</param>
    /// <param name="gpuHandle">The GPU handle or location for the start of memory on VRAM.</param>
    /// <param name="width">The width of the texture.</param>
    /// <param name="height">The height of the texture.</param>
    internal Texture(string id, int gpuHandle, int width, int height)
    {
      _id = id;
      _gpuHandle = gpuHandle;
      _width = width;
      _height = height;
      _existingReferences = 0;
    }
  }
}