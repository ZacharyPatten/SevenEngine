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

using System;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using SevenEngine.Mathematics;

namespace SevenEngine.Imaging
{
  public class Sprite
  {
    // Every sprite uses the same vertex positions
    private static readonly float[] _verteces = new float[] {
      1f, 1f, 0f,   -1f, 1f, 0f,   1f, -1f, 0f,
      -1f, -1f, 0f,   1f, -1f, 0f,   -1f, 1f, 0f };
    private static readonly int _vertexCount = 6;
    private static readonly float[] _textureMappingsDefault = new float[] {
      1f,0f,  0f,0f,  1f,1f,
      0f,1f,  1f,1f,  0f,0f };
    private Vector _position;
    private Texture _texture;
    private Point _scale;
    private float _rotation;
    private static int _gpuVertexBufferHandle;
    private static int _gpuTextureMappingBufferHandleDefault;
    private int _gpuTextureMappingBufferHandle;

    /// <summary>The handle to the memory of the texture buffer on the GPU.</summary>
    internal int GpuVertexBufferHandle { get { return _gpuVertexBufferHandle; } }
    /// <summary>The handle to the memory of the texture buffer on the GPU.</summary>
    internal int GPUTextureCoordinateBufferHandle { get { return _gpuTextureMappingBufferHandle; } }
    /// <summary>Returns 6, because sprites always have 6 verteces.</summary>
    internal int VertexCount { get { return _vertexCount; } }
    /// <summary>Get and set the position of the sprite.</summary>
    public Vector Position { get { return _position; } set { _position = value; } }
    /// <summary>Get and set the size of the sprite.</summary>
    public Point Scale { get { return _scale; } set { _scale = value; } }
    /// <summary>Get and set the rotation angle of the sprite.</summary>
    public float Rotation { get { return _rotation; } set { _rotation = value; } }
    /// <summary>Get and set the texture the sprite is mapping to.</summary>
    public Texture Texture { get { return _texture; } set { _texture = value; } }

    /// <summary>Creates an instance of a sprite.</summary>
    /// <param name="texture">The texture to have this sprite mapped to.</param>
    public Sprite(Texture texture)
    {
      if (_gpuVertexBufferHandle == 0)
        GenerateVertexBuffer(_verteces);
      _position = new Vector(0, 0, -10);
      _scale = new Point(1, 1);
      _rotation = 0f;
      _texture = texture;
      if (_gpuTextureMappingBufferHandleDefault == 0)
      {
        GenerateTextureCoordinateBuffer(_textureMappingsDefault);
        _gpuTextureMappingBufferHandleDefault = _gpuTextureMappingBufferHandle;
      }
      else
        _gpuTextureMappingBufferHandle = _gpuTextureMappingBufferHandleDefault;
    }

    /// <summary>Creates an instance of a sprite.</summary>
    /// <param name="texture">The texture to have this sprite mapped to.</param>
    /// <param name="textureMappings">The texture mappings for this sprite.</param>
    public Sprite(Texture texture, float[] textureMappings)
    {
      if (_gpuVertexBufferHandle == 0)
        GenerateVertexBuffer(_verteces);
      _position = new Vector(0, 0, -10);
      _scale = new Point(1, 1);
      _rotation = 0f;
      _texture = texture;
      if (textureMappings.Length != 12)
        throw new Exception("Invalid number of texture coordinates in sprite constructor.");
      GenerateTextureCoordinateBuffer(textureMappings);
    }

    /// <summary>Generates the vertex buffer that all sprites will use.</summary>
    private void GenerateVertexBuffer(float[] verteces)
    {
      // Declare the buffer
      GL.GenBuffers(1, out _gpuVertexBufferHandle);
      // Select the new buffer
      GL.BindBuffer(BufferTarget.ArrayBuffer, _gpuVertexBufferHandle);
      // Initialize the buffer values
      GL.BufferData<float>(BufferTarget.ArrayBuffer, (IntPtr)(verteces.Length * sizeof(float)), verteces, BufferUsageHint.StaticDraw);
      // Quick error checking
      int bufferSize;
      GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
      if (verteces.Length * sizeof(float) != bufferSize)
        throw new ApplicationException("Vertex array not uploaded correctly");
      // Deselect the new buffer
      GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
    }

    /// <summary>Generates the texture coordinate buffer that sprite will default to.</summary>
    private void GenerateTextureCoordinateBuffer(float[] textureMappings)
    {
      // Declare the buffer
      GL.GenBuffers(1, out _gpuTextureMappingBufferHandle);
      // Select the new buffer
      GL.BindBuffer(BufferTarget.ArrayBuffer, _gpuTextureMappingBufferHandle);
      // Initialize the buffer values
      GL.BufferData<float>(BufferTarget.ArrayBuffer, (IntPtr)(textureMappings.Length * sizeof(float)), textureMappings, BufferUsageHint.StaticDraw);
      // Quick error checking
      int bufferSize;
      GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
      if (textureMappings.Length * sizeof(float) != bufferSize)
        throw new ApplicationException("Texture mapping array not uploaded correctly");
      // Deselect the new buffer
      GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
    }
  }
}