using System.Runtime.InteropServices;

namespace Engine.Models
{
  [StructLayout(LayoutKind.Sequential)]
  public class StaticMesh
  {
    private int _existingReferences;

    protected string _filePath;
    protected string _staticMeshId;
    protected int _vertexBufferId;
    protected int _colorBufferId;
    protected int _textureCoordinateBufferId;
    protected int _normalBufferId;
    protected int _elementBufferId;
    protected int _vertexCount;

    /// <summary>The number of existing hardware instances of this model reference.</summary>
    public int ExistingReferences { get { return _existingReferences; } set { _existingReferences = value; } }

    /// <summary>Holds the filepath of the imported file.</summary>
    public string FilePath { get { return _filePath; } set { _filePath = value; } }
    /// <summary>The id associated with this mesh in the "StaticModelManager".</summary>
    public string StaticMeshId { get { return _staticMeshId; } set { _staticMeshId = value; } }
    /// <summary>The location of the vertex buffer on the GPU.</summary>
    public int VertexBufferId { get { return _vertexBufferId; } set { _vertexBufferId = value; } }
    /// <summary>The location of the color buffer on the GPU.</summary>
    public int ColorBufferId { get { return _colorBufferId; } set { _colorBufferId = value; } }
    /// <summary>The location of the texture coordinate buffer on the GPU.</summary>
    public int TextureCoordinateBufferId { get { return _textureCoordinateBufferId; } set { _textureCoordinateBufferId = value; } }
    /// <summary>The location of the normal buffer on the GPU.</summary>
    public int NormalBufferId { get { return _normalBufferId; } set { _normalBufferId = value; } }
    /// <summary>The location of the element buffer on the GPU.</summary>
    public int ElementBufferId { get { return _elementBufferId; } set { _elementBufferId = value; } }
    /// <summary>The number of verteces in this model.</summary>
    public int VertexCount { get { return _vertexCount; } set { _vertexCount = value; } }


    /// <summary>Creates an instance of a StaticMesh.</summary>
    /// <param name="filePath">The file path of the model that the data came from.</param>
    /// <param name="vertexBufferId">The number reference of the vertex buffer on the GPU (default is 0).</param>
    /// <param name="colorBufferId">The number reference of the color buffer on the GPU (default is 0).</param>
    /// <param name="textureCoordinatesId">The number reference of the texture coordinate buffer on the GPU (default is 0).</param>
    /// <param name="normalBufferId">The number reference of the normal buffer on the GPU (default is 0).</param>
    /// <param name="elementBufferId">The number reference of the element buffer on the GPU (default is 0).</param>
    /// <param name="vertexCount">The number of verteces making up the mesh.</param>
    public StaticMesh(
      string filePath,
      string staticMeshId,
      int vertexBufferId, 
      int colorBufferId, 
      int textureCoordinatesId, 
      int normalBufferId,
      int elementBufferId,
      int vertexCount)
    {
      _existingReferences = 0;

      _filePath = filePath;
      _vertexBufferId = vertexBufferId;
      _colorBufferId = colorBufferId;
      _textureCoordinateBufferId = textureCoordinatesId;
      _normalBufferId = normalBufferId;
      _elementBufferId = elementBufferId;
      _vertexCount = vertexCount;
    }
  }
}
