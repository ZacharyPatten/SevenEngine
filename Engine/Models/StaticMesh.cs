using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Engine.Textures;

namespace Engine.Models
{
  public class StaticMesh
  {
    private int _existingReferences;

    protected string _filePath;
    protected int _vertexBufferId;
    protected int _colorBufferId;
    protected int _textureCoordinateBufferId;
    protected int _normalBufferId;
    protected int _elementBufferId;
    protected int _vertexCount;

    /// <summary>The number of existing hardware instances of this model reference.</summary>
    public int ExistingReferences { get { return _existingReferences; } set { _existingReferences = value; } }

    /// <summary>Holds the filepath of the imported file.</summary>
    public string FilePath;
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

    public StaticMesh(
      string filePath,
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
