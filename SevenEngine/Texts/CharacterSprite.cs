// Seven
// https://github.com/53V3N1X/SevenEngine
// LISCENSE: See "LISCENSE.txt" in th root project directory.
// SUPPORT: See "README.txt" in the root project directory.

using System;
using OpenTK.Graphics.OpenGL;
using SevenEngine.Imaging;
using Seven.Structures;

namespace SevenEngine.Texts
{
  public class CharacterSprite
  {
    // Every character sprite uses the same vertex positions
    //private static readonly float[] _verteces = new float[] {
    //  1f, 1f, 0f,   0f, 1f, 0f,   1f, 0f, 0f,
    //  0f, 0f, 0f,   1f, 0f, 0f,   0f, 1f, 0f };
    private static readonly float[] _verteces = new float[] {
      1f, 0f, 0f,   0f, 0f, 0f,   1f, -1f, 0f,
      0f, -1f, 0f,   1f, -1f, 0f,   0f, 0f, 0f };
    private static readonly int _vertexCount = 6;
    private Texture _texture;
    private static int _gpuVertexBufferHandle;
    private int _gpuTextureMappingBufferHandle;
    private int _xAdvance;
    private List_Array<Link<int, int>> _kearnings;
    private int _id;
    private int _xOffset;
    private int _yOffset;
    private int _originalWidth;
    private int _originalHeight;

    /// <summary>The handle to the memory of the texture buffer on the GPU.</summary>
    internal static int GpuVertexBufferHandle { get { return _gpuVertexBufferHandle; } }
    /// <summary>The handle to the memory of the texture buffer on the GPU.</summary>
    internal int GPUTextureCoordinateBufferHandle { get { return _gpuTextureMappingBufferHandle; } }
    /// <summary>Returns 6, because character sprites always have 6 verteces.</summary>
    internal int VertexCount { get { return _vertexCount; } }
    /// <summary>Get and set the texture the sprite is mapping to.</summary>
    internal Texture Texture { get { return _texture; } set { _texture = value; } }

    // These are font specific values.
    // Look up documentation on fnt files for more info.
    internal int Id { get { return _id; } }
    internal int XAdvance { get { return _xAdvance; } }
    internal int XOffset { get { return _xOffset; } }
    internal int YOffset { get { return _yOffset; } }
    internal int OriginalWidth { get { return _originalWidth; } }
    internal int OriginalHeight { get { return _originalHeight; } }

    /// <summary>Creates an instance of a sprite.</summary>
    /// <param name="texture">The texture to have this sprite mapped to.</param>
    /// <param name="textureMappings">The texture mappings for this sprite.</param>
    public CharacterSprite(Texture texture, int id, int xAdvance, int x, int y, int width, int height, int xOffset, int yOffset)
    {
      _texture = texture;
      _id = id;
      _xAdvance = xAdvance;
      _xOffset = xOffset;
      _yOffset = yOffset;
      _originalWidth = width;
      _originalHeight = height;
      _kearnings = new List_Array<Link<int, int>>(1);

      if (_gpuVertexBufferHandle == 0)
        GenerateVertexBuffer(_verteces);
      GenerateTextureCoordinateBuffer( new float[] {
        (x + width) / (float)_texture.Width, y / (float)_texture.Height,
        x / (float)_texture.Width, y / (float)_texture.Height,
        (x + width) / (float)_texture.Width, (y + height) / (float)_texture.Height,
        x / (float)_texture.Width, (y + height) / (float)_texture.Height,
        (x + width) / (float)_texture.Width, (y + height) / (float)_texture.Height,
        x / (float)_texture.Width, y / (float)_texture.Height });
    }

    /// <summary>Generates the vertex buffer that all character sprites will use.</summary>
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

    /// <summary>Generates the texture coordinate buffer that character sprite will default to.</summary>
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

    internal void AddKearning(int followingCharacter, int ammount)
    { _kearnings.Add(new Link<int,int>(followingCharacter, ammount)); }

    internal int CheckKearning(int followinCharacter)
    {
      for (int i = 0; i < _kearnings.Count; i++)
        if (_kearnings[i].One == followinCharacter)
          return _kearnings[i].Two;
      return 0;
    }
  }
}