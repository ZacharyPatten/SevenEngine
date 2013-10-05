using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Engine.Textures;

using OpenTK;

namespace Engine.Models
{
  public class StaticModel
  {
    /// <summary>All the meshes and textures for those meshes that make up the entire model.</summary>
    protected List<Tuple<Texture, StaticMesh>> _meshes;

    /// <summary>The location of the model in world-space (used in Renderer.cs).</summary>
    protected Vector _position;
    /// <summary>The scale of the model in world-space (used in Renderer.cs).</summary>
    protected Vector _scale;
    /// <summary>The axis about which the model will be rotated in world space (used in Renderer.cs).</summary>
    protected Vector _rotationAxis;
    /// <summary>The angle of rotation about the </summary>
    protected float _rotationAngle;
    
    public List<Tuple<Texture, StaticMesh>> Meshes { get { return _meshes; } set { _meshes = value; } }

    public Vector Position { get { return _position; } set { _position = value; } }
    public Vector Scale { get { return _scale; } set { _scale = value; } }
    public Vector RotationAmmounts { get { return _rotationAxis; } set { _rotationAxis = value; } }
    public float RotationAngle { get { return _rotationAngle; } set { _rotationAngle = value; } }

    public StaticModel()
    {
      _meshes = new List<Tuple<Texture, StaticMesh>>();

      _position = new Vector(0, 0, 0);
      _scale = new Vector(1, 1, 1);
      _rotationAxis = new Vector(0, 0, 0);
      _rotationAngle = 0;

    }
  }
}
