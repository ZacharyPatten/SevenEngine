using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Engine.Textures;

namespace Engine.Models
{
  public struct StaticModel
  {
    /// <summary>The location of the vertex buffer on the GPU.</summary>
    public int VertexBufferId { get; set; }
    /// <summary>The location of the color buffer on the GPU.</summary>
    public int ColorBufferId { get; set; }
    /// <summary>The location of the texture coordinate buffer on the GPU.</summary>
    public int TexCoordBufferId { get; set; }
    /// <summary>The location of the normal buffer on the GPU.</summary>
    public int NormalBufferId { get; set; }
    /// <summary>The location of the element buffer on the GPU.</summary>
    public int ElementBufferId { get; set; }
    /// <summary>The number of verteces in this model.</summary>
    public int VertexCount { get; set; }
    /// <summary>The texture that this model maps to.</summary>
    public Texture Texture { get; set; }
    /// <summary>The current position of this model in the world.</summary>
    public Vector3d Position { get; set; }
    /// <summary>The current scale of this model in the world.</summary>
    public Vector3d Scale { get; set; }
    /// <summary>Represents the ammount of rotation around each axis (by the RotationAngle).</summary>
    public Vector3d RotationAmmounts { get; set; }
    /// <summary>Represents the amount of rotation applied to the x, y, and z of RotationAmmounts.</summary>
    public float RotationAngle { get; set; }

    public StaticModel(
      int vertexBufferId, 
      int colorBufferId, 
      int textureCoordinatesId, 
      int normalBufferId,
      int elementBufferId,
      int vertexCount,
      Texture texture,
      Vector3d position,
      Vector3d scale,
      Vector3d rotationAmmounts,
      float rotationAngle) : this()
    {
      VertexBufferId = vertexBufferId;
      ColorBufferId = colorBufferId;
      TexCoordBufferId = textureCoordinatesId;
      NormalBufferId = normalBufferId;
      ElementBufferId = elementBufferId;
      VertexCount = vertexCount;
      Texture = texture;
      Position = position;
      Scale = scale;
      RotationAmmounts = rotationAmmounts;
      RotationAngle = rotationAngle;
    }
  }
}
