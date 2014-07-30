// Seven
// https://github.com/53V3N1X/SevenEngine
// LISCENSE: See "LISCENSE.txt" in th root project directory.
// SUPPORT: See "README.txt" in the root project directory.

using SevenEngine.Imaging;

using Seven;
using Seven.Structures;

namespace SevenEngine.StaticModels
{
  /// <summary>Represents a single mesh that has been loaded on the GPU.</summary>
  public class StaticMesh
  {
    private int _existingReferences;
    protected string _filePath;
    protected string _id;
    protected int _vertexBufferHandle;
    protected int _colorBufferHandle;
    protected int _textureCoordinateBufferHandle;
    protected int _normalBufferHandle;
    protected int _elementBufferHandle;
    protected int _vertexCount;
    protected Texture _texture;

    /// <summary>The number of existing hardware instances of this model reference.</summary>
    internal int ExistingReferences { get { return _existingReferences; } set { _existingReferences = value; } }
    /// <summary>Holds the filepath of the imported file.</summary>
    internal string FilePath { get { return _filePath; } set { _filePath = value; } }
    /// <summary>The id associated with this mesh in the "StaticModelManager".</summary>
    public string Id { get { return _id; } set { _id = value; } }
    /// <summary>The handle of the vertex buffer on the GPU.</summary>
    internal int VertexBufferHandle { get { return _vertexBufferHandle; } set { _vertexBufferHandle = value; } }
    /// <summary>The location of the color buffer on the GPU.</summary>
    internal int ColorBufferHandle { get { return _colorBufferHandle; } set { _colorBufferHandle = value; } }
    /// <summary>The location of the texture coordinate buffer on the GPU.</summary>
    internal int TextureCoordinateBufferHandle { get { return _textureCoordinateBufferHandle; } set { _textureCoordinateBufferHandle = value; } }
    /// <summary>The location of the normal buffer on the GPU.</summary>
    internal int NormalBufferHandle { get { return _normalBufferHandle; } set { _normalBufferHandle = value; } }
    /// <summary>The location of the element buffer on the GPU.</summary>
    internal int ElementBufferHandle { get { return _elementBufferHandle; } set { _elementBufferHandle = value; } }
    /// <summary>The number of verteces in this model.</summary>
    internal int VertexCount { get { return _vertexCount; } set { _vertexCount = value; } }
    /// <summary>The original texture that was assigned when the mesh was loaded.</summary>
    internal Texture Texture { get { return _texture; } set { _texture = value; } }

    /// <summary>Creates an instance of a StaticMesh.</summary>
    /// <param name="filePath">The file path of the model that the data came from.</param>
    /// <param name="id">The name associated with this mash when in was created.</param>
    /// <param name="vertexBufferHandle">The number reference of the vertex buffer on the GPU (default is 0).</param>
    /// <param name="colorBufferHandle">The number reference of the color buffer on the GPU (default is 0).</param>
    /// <param name="textureCoordinatesHandle">The number reference of the texture coordinate buffer on the GPU (default is 0).</param>
    /// <param name="normalBufferHandle">The number reference of the normal buffer on the GPU (default is 0).</param>
    /// <param name="elementBufferHandle">The number reference of the element buffer on the GPU (default is 0).</param>
    /// <param name="vertexCount">The number of verteces making up the mesh.</param>
    internal StaticMesh(
      string filePath,
      string id,
      int vertexBufferHandle,
      int colorBufferHandle,
      int textureCoordinatesHandle,
      int normalBufferHandle,
      int elementBufferHandle,
      int vertexCount,
      Texture originalTexture)
    {
      _existingReferences = 0;

      _filePath = filePath;
      _id = id;
      _vertexBufferHandle = vertexBufferHandle;
      _colorBufferHandle = colorBufferHandle;
      _textureCoordinateBufferHandle = textureCoordinatesHandle;
      _normalBufferHandle = normalBufferHandle;
      _elementBufferHandle = elementBufferHandle;
      _vertexCount = vertexCount;
      _texture = originalTexture;
    }

    public static Comparison CompareTo(StaticMesh left, StaticMesh right)
    {
      int comparison = left.Id.CompareTo(right.Id);
      if (comparison > 0)
        return Comparison.Greater;
      else if (comparison < 0)
        return Comparison.Less;
      else
        return Comparison.Equal;
    }

    public static Comparison CompareTo(StaticMesh left, string right)
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
