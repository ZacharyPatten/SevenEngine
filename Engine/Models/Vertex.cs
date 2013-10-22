using System.Runtime.InteropServices;

using SevenEngine.Imaging;
using SevenEngine.Mathematics;

namespace SevenEngine.Models
{
  [StructLayout(LayoutKind.Sequential)]
  public class Vertex
  {
    protected Vector _position;
    protected Vector _normal;
    protected Point _mapping;
    protected Color _color;

    /// <summary>The position (X, Y, Z) of the vertex.</summary>
    public Vector Position { get { return _position; } set { _position = value; } }
    /// <summary>The normal (X, Y, Z) of the vertex.</summary>
    public Vector Normal { get { return _normal; } set { _normal = value; } }
    /// <summary>The texture coordinates (u, v) of the vertex.</summary>
    public Point TextureCoordinates { get { return _mapping; } set { _mapping = value; } }
    /// <summary>The color of the vertex (RBG).</summary>
    public Color Color { get { return _color; } set { _color = value; } }

    public Vertex()
    {
      _position = new Vector(0, 0, 0);
      _normal = new Vector(0, 0, 0);
      _mapping = new Point();
      _color = new Color();
    }

    public Vertex(Vector modelPosition, Vector normal, Point textureMapping, Color color)
    {
      _position = modelPosition;
      _normal = normal;
      _mapping = textureMapping;
     _color = color;
    }
  }
}