using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Engine;
using Engine.Imaging;
using Engine.Mathematics;

namespace Engine
{
  public class SkyBox
  {
    /// <summary>The location of the model in world-space (used in Renderer.cs).</summary>
    protected Vector _position;
    /// <summary>The scale of the model in world-space (used in Renderer.cs).</summary>
    protected Vector _scale;

    protected Texture _up;
    protected Texture _right;
    protected Texture _left;
    protected Texture _back;
    protected Texture _front;

    public Vector Position { get { return _position; } set { _position = value; } }
    public Vector Scale { get { return _scale; } set { _scale = value; } }

    public Texture Up { get { return _up; } set { _up = value; } }
    public Texture Right { get { return _right; } set { _right = value; } }
    public Texture Left { get { return _left; } set { _left = value; } }
    public Texture Back { get { return _back; } set { _back = value; } }
    public Texture Front { get { return _front; } set { _front = value; } }

    public SkyBox()
    {
      _position = new Vector(0, 0, 0);
      _scale = new Vector(100, 100, 100);
    }
  }
}
