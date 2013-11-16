// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use with the following condition: any code using or originally taken 
// from the SevenEngine project must include citation to its original author(s) located at the
// top of each source code file, or you may include a reference to the SevenEngine project as
// a whole but you must include the current SevenEngine official website URL and logo.
// - Thanks.  :)  (support: seven@sevenengine.com)

// Author(s):
// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com
// Last Edited: 11-16-13

using System;
using OpenTK.Graphics.OpenGL;
using SevenEngine.Imaging;
using SevenEngine.DataStructures;

namespace SevenEngine.Texts
{
  public class CharacterSprite
  {
    // Every character sprite uses the same vertex positions
    private static readonly float[] _verteces = new float[] {
      1f, 1f, 0f,   0f, 1f, 0f,   1f, 0f, 0f,
      0f, 0f, 0f,   1f, 0f, 0f,   0f, 1f, 0f };
    private static readonly int _vertexCount = 6;
    private Texture _texture;
    private static int _gpuVertexBufferHandle;
    private int _gpuTextureMappingBufferHandle;
    private int _xAdvance;
    private ListArray<Link2<int, int>> _kearnings;
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
      _kearnings = new ListArray<Link2<int, int>>(1);

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
    {
      _kearnings.Add(new Link2<int,int>(followingCharacter, ammount));
    }

    internal int CheckKearning(int followinCharacter)
    {
      for (int i = 0; i < _kearnings.Count; i++)
        if (_kearnings[i].Left == followinCharacter)
          return _kearnings[i].Right;
      return 0;
    }
  }
}