using System.Runtime.InteropServices;

namespace Engine.Imaging
{
  [StructLayout(LayoutKind.Sequential)]
  public class Texture
  {
    protected int _existingReferences;

    protected int _id;
    protected int _width;
    protected int _height;

    /// <summary>The number of existing hardware instances of this model reference.</summary>
    public int ExistingReferences { get { return _existingReferences; } set { _existingReferences = value; } }

    /// <summary>The id of the texture on the GPU.</summary>
    public int Id { get { return _id; } set { _id = value; } }
    /// <summary>The width of the texture.</summary>
    public int Width { get { return _width; } set { _width = value; } }
    /// <summary>The height of the texture.</summary>
    public int Height { get { return _height; } set { _height = value; } }

    public Texture(int id, int width, int height)
    {
      _existingReferences = 0;

     _id = id;
      _width = width;
      _height = height;
    }
  }
}