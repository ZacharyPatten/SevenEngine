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
using OpenTK.Graphics.OpenGL;
using SevenEngine.Imaging;
using SevenEngine.DataStructures;

namespace SevenEngine.Texts
{
  public class CharacterSprite
  {
    // Every character sprite uses the same vertex positions
    private static readonly float[] _verteces = new float[] {
      1f, 1f, 0f,   -1f, 1f, 0f,   1f, -1f, 0f,
      -1f, -1f, 0f,   1f, -1f, 0f,   -1f, 1f, 0f };
    private static readonly int _vertexCount = 6;
    private Texture _texture;
    private static int _gpuVertexBufferHandle;
    private int _gpuTextureMappingBufferHandle;
    private int _xAdvance;
    private ListArray<Link2<int, int>> _kearnings;
    private int _id;
    private int _xOffset;
    private int _yOffset;

    /// <summary>The handle to the memory of the texture buffer on the GPU.</summary>
    internal int GpuVertexBufferHandle { get { return _gpuVertexBufferHandle; } }
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

    /// <summary>Creates an instance of a sprite.</summary>
    /// <param name="texture">The texture to have this sprite mapped to.</param>
    /// <param name="textureMappings">The texture mappings for this sprite.</param>
    public CharacterSprite(Texture texture, int id, int xAdvance, int x, int y, int width, int height, int xOffset, int yOffset)
    {
      // OMFG this part sucked...
      float[] textureMappings = new float[] {
        (float)(x + width) / (float)texture.Width, (float)y / (float)texture.Height,
        (float)x / (float)texture.Width, (float)y / (float)texture.Height,
        (float)(x + width) / (float)texture.Width, (float)(y + height) / (float)texture.Height,
        (float)x / (float)texture.Width, (float)(y + height) / (float)texture.Height,
        (float)(x + width) / (float)texture.Width, (float)(y + height) / (float)texture.Height,
        (float)x / (float)texture.Width, (float)y / (float)texture.Height,
        };
      if (_gpuVertexBufferHandle == 0)
        GenerateVertexBuffer(_verteces);
      _texture = texture;
      if (textureMappings.Length != 12)
        throw new Exception("Invalid number of texture coordinates in sprite constructor.");
      GenerateTextureCoordinateBuffer(textureMappings);
      _id = id;
      _xAdvance = xAdvance;
      _xOffset = xOffset;
      _yOffset = yOffset;
      _kearnings = new ListArray<Link2<int, int>>(1);
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





  //  private int _id,
  //    _x, _y,
  //    _width, _height,
  //    _xOffset, _yOffset,
  //    _xAdvance;
  //  private List<Link2<int, int>> _kernings;

  //  public int Id { get { return _id; } set { _id = value; } }
  //  internal int Width { get { return _width; } set { _width = value; } }
  //  internal int Height { get { return _height; } set { _height = value; } }
  //  internal int XOffset { get { return _xOffset; } set { _xOffset = value; } }
  //  internal int YOffset { get { return _yOffset; } set { _yOffset = value; } }
  //  internal int XAdvance { get { return _xAdvance; } set { _xAdvance = value; } }
  //  internal List<Link2<int, int>> Kernings { get { return _kernings; } }

  //  public CharacterSprite(string textureId, int id, int x, int y,
  //    int width, int height, int xOffset, int yOffset, int xAdvance) : base(textureId,
  //    new float[] { x + xOffset, y + yOffset, })
  //  {
  //    //Data = data;
  //    //Sprite = sprite;
  //  }
  }
}