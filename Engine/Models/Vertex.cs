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

using SevenEngine.Imaging;
using SevenEngine.Mathematics;

namespace SevenEngine.Models
{
  /// <summary>I currenlty DON'T use this class, and I probably never will need to. I am just keeping it for the time being.</summary>
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
      _mapping = new Point(0, 0);
      _color = new Color(0, 0, 0, 0);
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