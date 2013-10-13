using System.Runtime.InteropServices;

namespace Engine.Models
{
  [StructLayout(LayoutKind.Sequential)]
  public class StaticMesh
  {
    private int _existingReferences;

    protected string _filePath;
    protected string _staticMeshId;
    protected int _vertexBufferHandle;
    protected int _colorBufferHandle;
    protected int _textureCoordinateBufferHandle;
    protected int _normalBufferHandle;
    protected int _elementBufferHandle;
    protected int _vertexCount;

    /// <summary>The number of existing hardware instances of this model reference.</summary>
    public int ExistingReferences { get { return _existingReferences; } set { _existingReferences = value; } }

    /// <summary>Holds the filepath of the imported file.</summary>
    public string FilePath { get { return _filePath; } set { _filePath = value; } }
    /// <summary>The id associated with this mesh in the "StaticModelManager".</summary>
    public string StaticMeshId { get { return _staticMeshId; } set { _staticMeshId = value; } }
    /// <summary>The handle of the vertex buffer on the GPU.</summary>
    public int VertexBufferHandle { get { return _vertexBufferHandle; } set { _vertexBufferHandle = value; } }
    /// <summary>The location of the color buffer on the GPU.</summary>
    public int ColorBufferHandle { get { return _colorBufferHandle; } set { _colorBufferHandle = value; } }
    /// <summary>The location of the texture coordinate buffer on the GPU.</summary>
    public int TextureCoordinateBufferHandle { get { return _textureCoordinateBufferHandle; } set { _textureCoordinateBufferHandle = value; } }
    /// <summary>The location of the normal buffer on the GPU.</summary>
    public int NormalBufferHandle { get { return _normalBufferHandle; } set { _normalBufferHandle = value; } }
    /// <summary>The location of the element buffer on the GPU.</summary>
    public int ElementBufferHandle { get { return _elementBufferHandle; } set { _elementBufferHandle = value; } }
    /// <summary>The number of verteces in this model.</summary>
    public int VertexCount { get { return _vertexCount; } set { _vertexCount = value; } }


    /// <summary>Creates an instance of a StaticMesh.</summary>
    /// <param name="filePath">The file path of the model that the data came from.</param>
    /// <param name="staticMeshId">The name associated with this mash when in was created.</param>
    /// <param name="vertexBufferHandle">The number reference of the vertex buffer on the GPU (default is 0).</param>
    /// <param name="colorBufferHandle">The number reference of the color buffer on the GPU (default is 0).</param>
    /// <param name="textureCoordinatesHandle">The number reference of the texture coordinate buffer on the GPU (default is 0).</param>
    /// <param name="normalBufferHandle">The number reference of the normal buffer on the GPU (default is 0).</param>
    /// <param name="elementBufferHandle">The number reference of the element buffer on the GPU (default is 0).</param>
    /// <param name="vertexCount">The number of verteces making up the mesh.</param>
    public StaticMesh(
      string filePath,
      string staticMeshId,
      int vertexBufferHandle, 
      int colorBufferHandle, 
      int textureCoordinatesHandle, 
      int normalBufferHandle,
      int elementBufferHandle,
      int vertexCount)
    {
      _existingReferences = 0;

      _filePath = filePath;
      _vertexBufferHandle = vertexBufferHandle;
      _colorBufferHandle = colorBufferHandle;
      _textureCoordinateBufferHandle = textureCoordinatesHandle;
      _normalBufferHandle = normalBufferHandle;
      _elementBufferHandle = elementBufferHandle;
      _vertexCount = vertexCount;
    }
  }
}
