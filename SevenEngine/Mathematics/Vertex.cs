/*// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use under the following condition: any code using or originally taken 
// from the SevenEngine project must include citation to its original author(s) located at the
// top of each source code file, or you may include a reference to the SevenEngine project as
// a whole but you must include the current SevenEngine official website URL and logo.
// - Thanks.  :)  (support: seven@sevenengine.com)

// Author(s):
// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com
// Last Edited: 11-16-13

using SevenEngine.Imaging;

namespace SevenEngine.Mathematics
{
  /// <summary>Represents a single vertex with (x, y, z) positions, (x, y, z) normals,
  /// (u, v) texture coordinates, and (r, g, b, a) colors.</summary>
  public class Vertex
  {
    protected Vector37 _position;
    protected Vector37 _normal;
    protected Point _mapping;
    protected Color _color;

    /// <summary>The position (X, Y, Z) of the vertex.</summary>
    public Vector37 Position { get { return _position; } set { _position = value; } }
    /// <summary>The normal (X, Y, Z) of the vertex.</summary>
    public Vector37 Normal { get { return _normal; } set { _normal = value; } }
    /// <summary>The texture coordinates (u, v) of the vertex.</summary>
    public Point TextureCoordinates { get { return _mapping; } set { _mapping = value; } }
    /// <summary>The color of the vertex (RBG).</summary>
    public Color Color { get { return _color; } set { _color = value; } }

    public Vertex()
    {
      _position = new Vector37(0, 0, 0);
      _normal = new Vector37(0, 0, 0);
      _mapping = new Point(0, 0);
      _color = new Color(0, 0, 0, 0);
    }

    public Vertex(Vector37 modelPosition, Vector37 normal, Point textureMapping, Color color)
    {
      _position = modelPosition;
      _normal = normal;
      _mapping = textureMapping;
     _color = color;
    }
  }
}*/