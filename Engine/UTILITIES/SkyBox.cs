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
using SevenEngine;
using SevenEngine.Imaging;
using SevenEngine.Mathematics;

namespace SevenEngine
{
  public class SkyBox
  {
    private static readonly int _vertexCount = 30;
    protected Vector _position;
    protected Vector _scale;
    protected Texture _top;
    protected Texture _right;
    protected Texture _left;
    protected Texture _back;
    protected Texture _front;
    private static int _gpuVertexBufferHandle = 0;
    private static int _gpuTextureMappingBufferHandle = 0;

    /// <summary>Returns 30, because skyboxes always have 30 verteces.</summary>
    internal int VertexCount { get { return _vertexCount; } }
    /// <summary>The handle to the memory of the texture buffer on the GPU.</summary>
    internal int GpuVertexBufferHandle { get { return _gpuVertexBufferHandle; } }
    /// <summary>The handle to the memory of the texture buffer on the GPU.</summary>
    internal int GPUTextureCoordinateBufferHandle { get { return _gpuTextureMappingBufferHandle; } }
    /// <summary>The center of this skybox. YOU PROBABLY WANT THE SKYBOXES POSITION TO FOLLOW THE CAMERA!!!</summary>
    public Vector Position { get { return _position; } set { _position = value; } }
    /// <summary>The scale of the skybox. YOU MAY WANT TO SET THE SCALE TO BE 1/2 OF THE FAR CLIP PLANE FOR EACH DIMENSION.</summary>
    public Vector Scale { get { return _scale; } set { _scale = value; } }
    /// <summary>The texture being applied to the top of the skybox.</summary>
    public Texture Top { get { return _top; } set { _top = value; } }
    /// <summary>The texture being applied to the right of the skybox.</summary>
    public Texture Right { get { return _right; } set { _right = value; } }
    /// <summary>The texture being applied to the left of the skybox.</summary>
    public Texture Left { get { return _left; } set { _left = value; } }
    /// <summary>The texture being applied to the back of the skybox.</summary>
    public Texture Back { get { return _back; } set { _back = value; } }
    /// <summary>The texture being applied to the front fo the skybox.</summary>
    public Texture Front { get { return _front; } set { _front = value; } }

    public SkyBox()
    {
      _position = new Vector(0, 0, 0);
      _scale = new Vector(100, 100, 100);
      if (_gpuVertexBufferHandle == 0)
        GenerateVertexBuffer();
      if (_gpuTextureMappingBufferHandle == 0)
        GenerateTextureCoordinateBuffer();
    }

    /// <summary>Generates the vertex buffer that all sprites will use.</summary>
    private void GenerateVertexBuffer()
    {
      // Every skybox uses the same vertex positions
      #region Right Hand Rule
      //float[] verteces = new float[]
      //{
      //  // Face 1 (Left)
      //  // Starts at index 0
      //  -1, -1, 1,   -1, 1, 1,   -1, 1, -1,
      //  -1, -1, 1,   -1, 1, -1,   -1, -1, -1,
      //  // Face 2 (Front)
      //  // Starts at index 18
      //  -1, -1, 1,   1, -1, 1,   1, 1, 1,
      //  -1, -1, 1,   1, 1, 1,   -1, 1, 1,
      //  // Face 3 (Right)
      //  // Starts at index 36
      //  1, -1, 1,   1, -1, -1,   1, 1, -1,
      //  1, -1, 1,   1, 1, -1,   1, 1, 1,
      //  // Face 4 (Back)
      //  // Starts at index 54
      //  -1, -1, -1,   -1, 1, -1,   1, -1, -1,
      //  -1, 1, -1,   1, 1, -1,   1, -1, -1,
      //  // Face 5 (Top)
      //  // Starts at index 72
      //  -1, 1, 1,   1, 1, 1,   1, 1, -1,
      //  -1, 1, 1,   1, 1, -1,   -1, 1, -1
      //};
      #endregion
      #region Left Hand Rule
      float[] verteces = new float[]
      {
        // Face 1 (Left)
        // Starts at index 0
        -1, -1, 1,   -1, 1, -1,   -1, 1, 1,
        -1, -1, 1,   -1, -1, -1,   -1, 1, -1,
        // Face 2 (Front)
        // Starts at index 18
        -1, -1, 1,   1, 1, 1,   1, -1, 1,
        -1, -1, 1,   -1, 1, 1,   1, 1, 1,
        // Face 3 (Right)
        // Starts at index 36
        1, -1, 1,   1, 1, -1,   1, -1, -1,
        1, -1, 1,   1, 1, 1,   1, 1, -1,
        // Face 4 (Back)
        // Starts at index 54
        -1, -1, -1,   1, -1, -1,   -1, 1, -1,
        -1, 1, -1,   1, -1, -1,   1, 1, -1,
        // Face 5 (Top)
        // Starts at index 72
        -1, 1, 1,   1, 1, -1,   1, 1, 1,
        -1, 1, 1,   -1, 1, -1,   1, 1, -1
      };
      #endregion

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
      // The texture Coordinates for every skybox
      #region Right Hand Rule
      //float[] textureMappings = new float[]
      //{
      //  // Face 1 (Left)
      //  // Starts at index 0
      //  1, 1,   1, 0,   0, 0,
      //  1, 1,   0, 0,   0, 1,
      //  // Face 2 (Front)
      //  // Starts at index 12
      //  0, 1,   1, 1,   1, 0,
      //  0, 1,   1, 0,   0, 0,
      //  // Face 3 (Right)
      //  // Starts at index 24
      //  0, 1,   1, 1,   1, 0,
      //  0, 1,   1, 0,   0, 0,
      //  // Face 4 (Back)
      //  // Starts at index 36
      //  1, 1,   1, 0,   0, 1,
      //  1, 0,   0, 0,   0, 1,
      //  // Face 5 (Top)
      //  // Starts at index 48
      //  0, 1,   1, 1,   1, 0,
      //  0, 1,   1, 0,   0, 0
      //};
      #endregion
      #region Left Hand Rule
      float[] textureMappings = new float[]
      {
        // Face 1 (Left)
        // Starts at index 0
        1, 1,   0, 0,   1, 0,
        1, 1,   0, 1,   0, 0,
        // Face 2 (Front)
        // Starts at index 12
        0, 1,   1, 0,   1, 1,
        0, 1,   0, 0,   1, 0,
        // Face 3 (Right)
        // Starts at index 24
        0, 1,   1, 0,   1, 1,
        0, 1,   0, 0,   1, 0,
        // Face 4 (Back)
        // Starts at index 36
        1, 1,   0, 1,   1, 0,
        1, 0,   0, 1,   0, 0,
        // Face 5 (Top)
        // Starts at index 48
        0, 1,   1, 0,   1, 1,
        0, 1,   0, 0,   1, 0
      };
      #endregion

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
