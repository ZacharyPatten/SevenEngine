// Seven
// https://github.com/53V3N1X/SevenEngine
// LISCENSE: See "LISCENSE.txt" in th root project directory.
// SUPPORT: See "README.txt" in the root project directory.

using Seven;

namespace SevenEngine.Imaging
{
  /// <summary>Represents a single image that has been loaded on the GPU.</summary>
  public class Texture
  {
    protected int _existingReferences;
    protected string _id;
    protected int _gpuHandle;
    protected int _width;
    protected int _height;

    /// <summary>The number of existing hardware instances of this model reference.</summary>
    internal int ExistingReferences { get { return _existingReferences; } set { _existingReferences = value; } }
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
    public Texture(string id, int gpuHandle, int width, int height)
    {
      _id = id;
      _gpuHandle = gpuHandle;
      _width = width;
      _height = height;
      _existingReferences = 0;
    }

    public static Comparison CompareTo(Texture left, Texture right)
    {
      int comparison = left.Id.CompareTo(right.Id);
      if (comparison > 0)
        return Comparison.Greater;
      else if (comparison < 0)
        return Comparison.Less;
      else
        return Comparison.Equal;
    }

    public static Comparison CompareTo(Texture left, string right)
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