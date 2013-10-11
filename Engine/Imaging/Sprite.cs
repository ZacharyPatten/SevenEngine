using Engine.Mathematics;

namespace Engine.Imaging
{
  public class Sprite
  {
    private const int VertexAmount = 6;

    Vector[] _vertexPositions;
    Color[] _vertexColors;
    Point[] _vertexUVs;
    Texture _texture;

    public Vector[] VertexPositions { get { return _vertexPositions; } }
    public Color[] VertexColors { get { return _vertexColors; } }
    public Point[] VertexUVs { get { return _vertexUVs; } }

    public double Width { get { return _vertexPositions[1].X - _vertexPositions[0].X; } set { double newWidth = value; InitVertexPositions(Center, newWidth, Height); } }
    public double Height { get { return _vertexPositions[0].Y - _vertexPositions[2].Y; } set { double newHeight = value; InitVertexPositions(Center, Width, newHeight); } }
    public Vector Center { get { return new Vector(_vertexPositions[0].X + (Width / 2), _vertexPositions[0].Y - (Height / 2), _vertexPositions[0].Z); } set { Vector newPosition = value; InitVertexPositions(newPosition, Width, Height); } }

    public Sprite(Texture texture)
    {
      _vertexPositions = new Vector[VertexAmount];
      _vertexColors = new Color[VertexAmount];
      _vertexUVs = new Point[VertexAmount];
      _texture = texture;

      InitVertexPositions(new Vector(0, 0, 0), 1, 1);
      SetColor(new Color(1, 1, 1, 1));
      SetUVs(new Point(0, 0), new Point(1, 1));
    }

    public Texture Texture
    {
      get { return _texture; }
      set
      {
        _texture = value;
        // By default the width and height is set
        // to that of the texture
        InitVertexPositions(Center, _texture.Width, _texture.Height);
      }
    }
    
    private void InitVertexPositions(Vector position, double width, double height)
    {
      double halfWidth = width / 2;
      double halfHeight = height / 2;
      // Clockwise creation of two triangles to make a quad.

      // TopLeft, TopRight, BottomLeft
      _vertexPositions[0] = new Vector(position.X - halfWidth, position.Y + halfHeight, position.Z);
      _vertexPositions[1] = new Vector(position.X + halfWidth, position.Y + halfHeight, position.Z);
      _vertexPositions[2] = new Vector(position.X - halfWidth, position.Y - halfHeight, position.Z);

      // TopRight, BottomRight, BottomLeft
      _vertexPositions[3] = new Vector(position.X + halfWidth, position.Y + halfHeight, position.Z);
      _vertexPositions[4] = new Vector(position.X + halfWidth, position.Y - halfHeight, position.Z);
      _vertexPositions[5] = new Vector(position.X - halfWidth, position.Y - halfHeight, position.Z);
    }

    public void SetColor(Color color)
    {
      for (int i = 0; i < Sprite.VertexAmount; i++)
      {
        _vertexColors[i] = color;
      }
    }

    public void SetUVs(Point topLeft, Point bottomRight)
    {
      // TopLeft, TopRight, BottomLeft
      _vertexUVs[0] = topLeft;
      _vertexUVs[1] = new Point(bottomRight.X, topLeft.Y);
      _vertexUVs[2] = new Point(topLeft.X, bottomRight.Y);

      // TopRight, BottomRight, BottomLeft
      _vertexUVs[3] = new Point(bottomRight.X, topLeft.Y);
      _vertexUVs[4] = bottomRight;
      _vertexUVs[5] = new Point(topLeft.X, bottomRight.Y);
    }
  }
}