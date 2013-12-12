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

using SevenEngine.Imaging;
using SevenEngine.DataStructures.Interfaces;

namespace SevenEngine.StaticModels
{
  /// <summary>Represents a single mesh that has been loaded on the GPU.</summary>
  public class StaticMeshInstance : InterfaceStringId
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
    protected Texture _originalTexture;

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
    internal Texture OriginalTexture { get { return _originalTexture; } set { _originalTexture = value; } }

    /// <summary>Creates an instance of a StaticMesh.</summary>
    /// <param name="filePath">The file path of the model that the data came from.</param>
    /// <param name="id">The name associated with this mash when in was created.</param>
    /// <param name="vertexBufferHandle">The number reference of the vertex buffer on the GPU (default is 0).</param>
    /// <param name="colorBufferHandle">The number reference of the color buffer on the GPU (default is 0).</param>
    /// <param name="textureCoordinatesHandle">The number reference of the texture coordinate buffer on the GPU (default is 0).</param>
    /// <param name="normalBufferHandle">The number reference of the normal buffer on the GPU (default is 0).</param>
    /// <param name="elementBufferHandle">The number reference of the element buffer on the GPU (default is 0).</param>
    /// <param name="vertexCount">The number of verteces making up the mesh.</param>
    internal StaticMeshInstance(
      string filePath,
      string id,
      int vertexBufferHandle,
      int colorBufferHandle,
      int textureCoordinatesHandle,
      int normalBufferHandle,
      int elementBufferHandle,
      int vertexCount)
      //Texture originalTexture)
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
      // _originalTexture = originalTexture;
    }

    public static int CompareTo(StaticMeshInstance left, StaticMeshInstance right) { return left.Id.CompareTo(right.Id); }
    public static int CompareTo(StaticMeshInstance left, string right) { return left.Id.CompareTo(right); }
  }
}
