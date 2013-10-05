/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Engine.Textures;

namespace Engine.Models
{
  /// <summary>
  /// The SubRigidBodyModel class represent a section of a rigid body
  /// model where all the texture mappings are on the same vertex.
  /// </summary>
  public class RigidBodyPartModel
  {
    protected Renderer _renderer; 
    
    protected Texture _texture;
    protected Vector3d _position;
    protected Vector3d _scale;

    /// <summary>The translation being applied to the x, y, and z axis of this model respectively.</summary>
    public Vector3d Position { get { return _position; } set { _position = value; } }
    /// <summary>The scale being applied to the x, y, and z axis of this model respectively.</summary>
    public Vector3d Scale { get { return _scale; } set { _scale = value; } }
    /// <summary>The texture that all the verteces of this submodel are mapped to.</summary>
    public Texture Texture { get { return _texture; } set { _texture = value; } }

    public int temp;

    protected int _vertexBufferID;
    protected int _colorBufferID;
    protected int _texCoordBufferID;
    protected int _normalBufferID;
    protected int _elementBufferID;

    /// <summary>The buffer id of the generated vertex buffer on the graphics card.</summary>
    public int VertexBufferID { get { return _vertexBufferID; } set { _vertexBufferID = value; } }
    /// <summary>The buffer id of the generated color buffer on the graphics card.</summary>
    public int ColorBufferID { get { return _colorBufferID; } set { _colorBufferID = value; } }
    /// <summary>The buffer id of the generated texture coordinates buffer on the graphics card.</summary>
    public int TexCoordBufferID { get { return _texCoordBufferID; } set { _texCoordBufferID = value; } }
    /// <summary>The buffer id of the generated normal buffer on the graphics card.</summary>
    public int NormalBufferID { get { return _normalBufferID; } set { _normalBufferID = value; } }
    /// <summary>The buffer id of the generated element buffer on the graphics card.</summary>
    public int ElementBufferID { get { return _elementBufferID; } set { _elementBufferID = value; } }

    protected float[] _verteces;
    protected float[] _normals;
    protected float[] _mappings;
    protected float[] _colors;
    protected int[] _indeces;

    public float[] Verteces { get { return _verteces; } set { _verteces = value; } }
    public float[] Normals { get { return _normals; } set { _normals = value; } }
    public float[] Mappings { get { return _mappings; } set { _mappings = value; } }
    public float[] Colors { get { return _colors; } set { _colors = value; } }
    public int[] Indeces { get { return _indeces; } set { _indeces = value; } }

    public RigidBodyPartModel(TextureManager textureManager, string texture, float[] verteces, float[] normals, float[] mappings, float[] colors, int[] indeces)
    {
      // Initialize the renderer for this instance
      _renderer = new Renderer();

      // Initialize modelview tranformational values
      _position = new Vector3d(0, 0, 0);
      _scale = new Vector3d(1, 1, 1);

      // Get the texture reference for the image on VRAM
      _texture = textureManager.Get(texture);

      // Initialize the verteces and generate the buffer for it on VRAM
      _verteces = verteces;
      GenerateVertexBuffer();

      // Initialize the texture coodinates and generate the buffer for it on VRAM
      _mappings = mappings;
      GenerateTextureMappingBuffer();

      // Initialize the normals and generate the buffer for it on VRAM
      _normals = normals;
      GenerateNormalBuffer();

      // Initialize the colors and generate the buffer for it on VRAM
      _colors = colors;
      GenerateColorBuffer();

      // Initialize the elements and generate the buffer for it on VRAM
      _indeces = indeces;
      GenerateElementBuffer();
    }

    #region Buffer Generators

    /// <summary>Sets up the vertex buffer on video RAM. NOTE: deselects the buffer when done.</summary>
    protected void GenerateVertexBuffer()
    {
      if (_verteces != null)
      {
        // Declare the buffer
        GL.GenBuffers(1, out _vertexBufferID);
        // Select the new buffer
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferID);
        // Initialize the buffer values
        GL.BufferData<float>(BufferTarget.ArrayBuffer, (IntPtr)(_verteces.Length * sizeof(float)), _verteces, BufferUsageHint.StaticDraw);
        // Quick error checking
        int bufferSize;
        GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
        if (_verteces.Length * sizeof(float) != bufferSize)
          throw new ApplicationException("Vertex array not uploaded correctly");
        // Deselect the new buffer
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
      }
      else { _vertexBufferID = 0; }
    }

    /// <summary>Sets up the color buffer on video RAM. NOTE: deselects the buffer when done.</summary>
    protected void GenerateColorBuffer()
    {
      if (_colors != null)
      {
        // Declare the buffer
        GL.GenBuffers(1, out _colorBufferID);
        // Select the new buffer
        GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBufferID);
        // Initialize the buffer values
        GL.BufferData<float>(BufferTarget.ArrayBuffer, (IntPtr)(_colors.Length * sizeof(float)), _colors, BufferUsageHint.StaticDraw);
        // Quick error checking
        int bufferSize;
        GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
        if (_colors.Length * sizeof(float) != bufferSize)
          throw new ApplicationException("Color array not uploaded correctly");
        // Deselect the new buffer
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
      }
      else { _colorBufferID = 0; }
    }


    /// <summary>Sets up the texture mapping buffer on video RAM. NOTE: deselects the buffer when done.</summary>
    protected void GenerateTextureMappingBuffer()
    {
      if (_mappings != null)
      {
        // Declare the buffer
        GL.GenBuffers(1, out _texCoordBufferID);
        // Select the new buffer
        GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBufferID);
        // Initialize the buffer values
        GL.BufferData<float>(BufferTarget.ArrayBuffer, (IntPtr)(_mappings.Length * sizeof(float)), _mappings, BufferUsageHint.StaticDraw);
        // Quick error checking
        int bufferSize;
        GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
        if (_mappings.Length * sizeof(float) != bufferSize)
          throw new ApplicationException("TexCoord array not uploaded correctly");
        // Deselect the new buffer
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
      }
      else { _texCoordBufferID = 0; }
    }

    /// <summary>Sets up the normal buffer on video RAM. NOTE: deselects the buffer when done.</summary>
    protected void GenerateNormalBuffer()
    {
      if (_normals != null)
      {
        // Declare the buffer
        int temp;
        GL.GenBuffers(1, out temp);
        _normalBufferID = temp;
        // Select the new buffer
        GL.BindBuffer(BufferTarget.ArrayBuffer, _normalBufferID);
        // Initialize the buffer values
        GL.BufferData<float>(BufferTarget.ArrayBuffer, (IntPtr)(_normals.Length * sizeof(float)), _normals, BufferUsageHint.StaticDraw);
        // Quick error checking
        int bufferSize;
        GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
        if (_normals.Length * sizeof(float) != bufferSize)
          throw new ApplicationException("Normal array not uploaded correctly");
        // Deselect the new buffer
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
      }
      else { _normalBufferID = 0; }
    }

    /// <summary>Sets up the element buffer on video RAM. NOTE: deselects the buffer when done.</summary>
    protected void GenerateElementBuffer()
    {
      if (_indeces != null)
      {
        // Declare the buffer
        int temp;
        GL.GenBuffers(1, out temp);
        _elementBufferID = temp;
        // Select the new buffer
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferID);
        // Initialize the buffer values
        GL.BufferData<int>(BufferTarget.ElementArrayBuffer, (IntPtr)(_indeces.Length * sizeof(int)), _indeces, BufferUsageHint.StaticDraw);
        // Quick error checking
        int bufferSize;
        GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
        if (_indeces.Length * sizeof(int) != bufferSize)
          throw new ApplicationException("Element array not uploaded correctly");
        // Deselect the new buffer
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
      }
      else { _elementBufferID = 0; }
    }

    #endregion

    public void Update(double elapsedTime)
    {
    }

    public void Render()
    {
      // Switch to modelview editing
      GL.MatrixMode(MatrixMode.Modelview);
      // Reset the model view to an identity matrix
      GL.LoadIdentity();
      // Translate the modelview by this model's position
      GL.Translate(_position);
      // Scale the modelview by this model's scale
      GL.Scale(_scale);

      GL.Rotate(temp, 1, 0, 0);

      //_renderer.DrawSubModel(this);
      throw new Exception("Dont use this calss!!!!!!!!!!!!!!!!!!!!!!!!!");
    }
  }
}*/