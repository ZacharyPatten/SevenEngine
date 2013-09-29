using System;

using Engine.Textures;

namespace Engine.Models
{
  [Serializable]
  public class Vertex
  {
    protected Vector _position;
    protected Vector _normal;
    protected Point _mapping; // texture mapping
    protected Color _color; // default color

    public Vector Position { get { return _position; } set { _position = value; } }
    public Vector Normal { get { return _normal; } set { _normal = value; } }
    public Point Mapping { get { return _mapping; } set { _mapping = value; } }
    public Color Color { get { return _color; } set { _color = value; } }

    public Vertex()
    {
      _position = new Vector();
      _normal = new Vector();
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