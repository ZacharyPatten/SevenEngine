using System;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Engine.Mathematics;

namespace Engine.Imaging
{
  public class SpriteBatch
  {
    const int MaxVertexNumber = 1000;
    Vector[] _vertexPositions = new Vector[MaxVertexNumber];
    Color[] _vertexColors = new Color[MaxVertexNumber];
    Point[] _vertexUVs = new Point[MaxVertexNumber];
    const int VertexDimensions = 3;
    const int ColorDimensions = 4;
    const int UVDimensions = 2;
    int _batchSize = 0;

    public void AddSprite(Sprite sprite)
    {
      // If the batch is full, draw it, empty and start again.
      if (sprite.VertexPositions.Length + _batchSize > MaxVertexNumber)
      {
        Draw();
      }

      // Add the current sprite vertices to the batch.
      for (int i = 0; i < sprite.VertexPositions.Length; i++)
      {
        _vertexPositions[_batchSize + i] = sprite.VertexPositions[i];
        _vertexColors[_batchSize + i] = sprite.VertexColors[i];
        _vertexUVs[_batchSize + i] = sprite.VertexUVs[i];
      }
      _batchSize += sprite.VertexPositions.Length;
    }

    void SetupPointers()
    {
      GL.EnableClientState(ArrayCap.ColorArray);
      GL.EnableClientState(ArrayCap.VertexArray);
      GL.EnableClientState(ArrayCap.TextureCoordArray);

      //throw new NotImplementedException();
      double[] verteces = new double[_batchSize];
      for (int i = 0; i < _batchSize; i += 3)
      {
        verteces[i] = _vertexPositions[i / 3].X;
        verteces[i + 1] = _vertexPositions[i / 3].Y;
        verteces[i + 2] = _vertexPositions[i / 3].Z;
      }

      double[] colors = new double[_batchSize];
      for (int i = 0; i < _batchSize; i += 4)
      {
        colors[i] = _vertexColors[i / 4].Red;
        colors[i + 1] = _vertexColors[i / 4].Green;
        colors[i + 2] = _vertexColors[i / 4].Blue;
        colors[i + 3] = _vertexColors[i / 4].Alpha;
      }

      GL.VertexPointer(VertexDimensions, VertexPointerType.Double, 0, verteces);
      GL.ColorPointer(ColorDimensions, ColorPointerType.Double, 0, colors);
      GL.TexCoordPointer(UVDimensions, TexCoordPointerType.Float, 0, _vertexUVs);
    }

    public void Draw()
    {
      if (_batchSize == 0)
      {
        return;
      }
      SetupPointers();
      GL.DrawArrays(BeginMode.Triangles, 0, _batchSize);
      _batchSize = 0;
    }
  }
}