using System;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using SevenEngine.Mathematics;

namespace SevenEngine.Imaging
{
  public class Sprite
  {
    private static readonly int _vertexCount = 6;

    private Vector _position;
    private Texture _texture;
    private Point _scale;
    private double _rotation;
    private static int _gpuVertexBufferHandle;
    private int _gpuTextureMappingBufferHandle;

    internal int GpuVertexBufferHandle { get { return _gpuVertexBufferHandle; } }
    internal int GPUTextureCoordinateBufferHandle { get { return _gpuTextureMappingBufferHandle; } }

    internal int VertexCount { get { return _vertexCount; } }

    /// <summary>Get and set the position of the sprite.</summary>
    public Vector Position { get { return _position; } set { _position = value; } }
    /// <summary>Get and set the size of the sprite.</summary>
    public Point Scale { get { return _scale; } set { _scale = value; } }
    /// <summary>Get and set the rotation angle of the sprite.</summary>
    public double Rotation { get { return _rotation; } set { _rotation = value; } }

    /// <summary>Get and set the texture the sprite is mapping to.</summary>
    public Texture Texture { get { return _texture; } set { _texture = value; } }

    /// <summary>Creates an instance of a sprite.</summary>
    /// <param name="texture">The texture to have this sprite mapped to.</param>
    public Sprite(Texture texture)
    {
      if (_gpuVertexBufferHandle == 0)
        GenerateVertexBuffer();
      _position = new Vector(0, 0, -10);
      _scale = new Point(1, 1);
      _rotation = 0d;
      _texture = texture;
      GenerateTextureCoordinateBuffer();
    }

    /// <summary>Generates the vertex buffer that all sprites will use.</summary>
    private void GenerateVertexBuffer()
    {
      // Every sprite uses the same vertex positions
      float[] verteces = new float[] {
        1f,1f,0f,  -1f,1f,0f,  1f,-1f,0f,
        -1f,-1f,0f,  1f,-1f,0f,  -1f,1f,0f };
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

    /// <summary>Generates the vertex buffer that all sprites will use.</summary>
    private void GenerateTextureCoordinateBuffer()
    {
      float[] textureMappings = new float[] {
        1f,0f,  0f,0f,  1f,1f,
        0f,1f,  1f,1f,  0f,0f, };
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