using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine
{
  public class SubModel
  {
    Sprite _toy;
    Renderer _renderer;

    private int _vertexBufferID;
    private int _colorBufferID;
    private int _texCoordBufferID;
    private int _normalBufferID;
    private int _elementBufferID;
    private int _numElements;

    public int VertexBufferID { get { return _vertexBufferID; } set { _vertexBufferID = value; } }
    public int ColorBufferID { get { return _colorBufferID; } set { _colorBufferID = value; } }
    public int TexCoordBufferID { get { return _texCoordBufferID; } set { _texCoordBufferID = value; } }
    public int NormalBufferID { get { return _normalBufferID; } set { _normalBufferID = value; } }
    public int ElementBufferID { get { return _elementBufferID; } set { _elementBufferID = value; } }
    public int NumElements { get { return _numElements; } set { _numElements = value; } }

    int[] _vboLocations = new int[1];
    bool temp;

    float[] _verteces;
    float[] _normals;
    float[] _mappings;
    float[] _colors;
    int[] _indeces;

    public SubModel(TextureManager textureManager, float[] verteces, float[] mappings)
    {
      _verteces = new float[verteces.Length];
      verteces.CopyTo(_verteces, 0);
      _mappings = new float[mappings.Length];
      mappings.CopyTo(_mappings, 0);


      // Initialize the renderer for this instance
      _renderer = new Renderer();

      // Get the texture reference for the image on VRAM
      _toy = new Sprite();
      _toy.Texture = textureManager.Get("guy");

      // Generate buffers on VRAM
      GenerateVertexBuffer();
      //GenerateColorBuffer();
      GenerateTextureMappingBuffer();
      //GenerateNormalBuffer();
      //GenerateElementBuffer();
    }

    #region Buffer Generators

    /// <summary>Sets up the vertex buffer on video RAM. NOTE: deselects the buffer when done.</summary>
    private void GenerateVertexBuffer()
    {
      // Declare the buffer
      int temp;
      GL.GenBuffers(1, out temp);
      _vertexBufferID = temp;
      // Select the new buffer
      GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferID);
      // Initialize the buffer values
      GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(_verteces.Length * Vector3.SizeInBytes), _verteces, BufferUsageHint.DynamicDraw);
      // Quick error checking
      int temp2;
      GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out temp2);
      if (_verteces.Length * Vector3.SizeInBytes != temp2)
        throw new ApplicationException("Vertex array not uploaded correctly");
      // Deselect the new buffer
      GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
    }

    /// <summary>Sets up the color buffer on video RAM. NOTE: deselects the buffer when done.</summary>
    private void GenerateColorBuffer()
    {
      // Declare the buffer
      int temp;
      GL.GenBuffers(1, out temp);
      _colorBufferID = temp;
      // Select the new buffer
      GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBufferID);
      // Initialize the buffer values
      GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(_colors.Length * sizeof(int)), _colors, BufferUsageHint.StaticDraw);
      // Quick error checking
      int temp2;
      GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out temp2);
      if (_colors.Length * sizeof(int) != temp2)
        throw new ApplicationException("Vertex array not uploaded correctly");
      // Deselect the new buffer
      GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
    }


    /// <summary>Sets up the texture mapping buffer on video RAM. NOTE: deselects the buffer when done.</summary>
    private void GenerateTextureMappingBuffer()
    {
      // Declare the buffer
      int temp;
      GL.GenBuffers(1, out temp);
      _texCoordBufferID = temp;
      // Select the new buffer
      GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBufferID);
      // Initialize the buffer values
      GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(_mappings.Length * 8), _mappings, BufferUsageHint.StaticDraw);
      // Quick error checking
      int temp2;
      GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out temp2);
      if (_mappings.Length * 8 != temp2)
        throw new ApplicationException("TexCoord array not uploaded correctly");
      // Deselect the new buffer
      GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
    }

    /// <summary>Sets up the normal buffer on video RAM. NOTE: deselects the buffer when done.</summary>
    private void GenerateNormalBuffer()
    {
      // Declare the buffer
      int temp;
      GL.GenBuffers(1, out temp);
      _normalBufferID = temp;
      // Select the new buffer
      GL.BindBuffer(BufferTarget.ArrayBuffer, _normalBufferID);
      // Initialize the buffer values
      GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(_normals.Length * Vector3.SizeInBytes), _normals, BufferUsageHint.StaticDraw);
      // Quick error checking
      int temp2;
      GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out temp2);
      if (_normals.Length * Vector3.SizeInBytes != temp2)
        throw new ApplicationException("Normal array not uploaded correctly");
      // Deselect the new buffer
      GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
    }

    /// <summary>Sets up the element buffer on video RAM. NOTE: deselects the buffer when done.</summary>
    private void GenerateElementBuffer()
    {
      // Declare the buffer
      int temp;
      GL.GenBuffers(1, out temp);
      _elementBufferID = temp;
      // Select the new buffer
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferID);
      // Initialize the buffer values
      GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(_indeces.Length * sizeof(int)), _indeces, BufferUsageHint.StaticDraw);
      // Quick error checking
      int temp2;
      GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out temp2);
      if (_indeces.Length * sizeof(int) != temp2)
        throw new ApplicationException("Element array not uploaded correctly");
      // Deselect the new buffer
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
    }

    #endregion

    public void Update(double elapsedTime) { }

    public void Render()
    {
      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
      GL.BindTexture(TextureTarget.Texture2D, _toy.Texture.Id);
      GL.LoadIdentity();
      _renderer.DrawSubModel(this);
    }
  }
}