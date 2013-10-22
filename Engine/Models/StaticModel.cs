using System;

using SevenEngine.DataStructures;
using SevenEngine.Imaging;
using SevenEngine.Mathematics;
using SevenEngine.Shaders;

using OpenTK;

namespace SevenEngine.Models
{
  public class StaticModel
  {
    /// <summary>All the meshes and textures for those meshes that make up the entire model.</summary>
    protected ListArray<Link<Texture, StaticMesh>> _meshes;

    /// <summary>The name id associated with this model.</summary>
    protected string _id;
    /// <summary>The location of the model in world-space (used in Renderer.cs).</summary>
    protected Vector _position;
    /// <summary>The scale of the model in world-space (used in Renderer.cs).</summary>
    protected Vector _scale;
    /// <summary>The axis about which the model will be rotated in world space (used in Renderer.cs).</summary>
    protected Vector _rotationAxis;
    /// <summary>The angle of rotation about the </summary>
    protected float _rotationAngle;
    /// <summary>The shader program to use when rendering this model (overrides the global shader program).</summary>
    protected ShaderProgram _shaderOverride;
    
    /// <summary>Gets the list of meshes that make up this model.</summary>
    internal ListArray<Link<Texture, StaticMesh>> Meshes { get { return _meshes; } set { _meshes = value; } }

    public Vector Position { get { return _position; } set { _position = value; } }
    public Vector Scale { get { return _scale; } set { _scale = value; } }
    public Vector RotationAmmounts { get { return _rotationAxis; } set { _rotationAxis = value; } }
    public float RotationAngle { get { return _rotationAngle; } set { _rotationAngle = value; } }

    /// <summary>This lets you change the shader per model. Instead of using the global default shader, it will use this one.</summary>
    /// <param name="shaderProgram">The id of the shader program you want to apply to this model.</param>
    public void SetShaderOverride(string shaderProgram) { _shaderOverride = ShaderManager.GetShaderProgram(shaderProgram); }

    internal StaticModel()
    {
      _meshes = new ListArray<Link<Texture, StaticMesh>>(10);

      _position = new Vector(0, 0, 0);
      _scale = new Vector(1, 1, 1);
      _rotationAxis = new Vector(0, 0, 0);
      _rotationAngle = 0;
    }

    internal StaticModel(string staticModelId, string[] textures, string[] meshes)
    {
      if (textures.Length != meshes.Length)
        throw new Exception("Attepting to make a static model but the number of meshes and number of textures are not equal.");
      _id = staticModelId;
      _meshes = new ListArray<Link<Texture, StaticMesh>>(10);
      for (int i = 0; i < textures.Length; i++)
        _meshes.Add(new Link<Texture, StaticMesh>(TextureManager.Get(textures[i]), StaticModelManager.GetMesh(meshes[i])));

      _position = new Vector(0, 0, 0);
      _scale = new Vector(1, 1, 1);
      _rotationAxis = new Vector(0, 0, 0);
      _rotationAngle = 0;
    }

    internal StaticModel(string staticModelId, ListArray<Link<Texture, StaticMesh>> meshes)
    {
      _id = staticModelId;

      _meshes = meshes;

      for (int i = 0; i < meshes.Count; i++)
      {
        meshes[i].Left.ExistingReferences++;
        meshes[i].Right.ExistingReferences++;
      }

      // increment the references of the GPU mappings
      //foreach (Link<Texture, StaticMesh> link in meshes)
      //{
      //  link.Left.ExistingReferences++;
      //  link.Right.ExistingReferences++;
      //}

      _position = new Vector(0, 0, 0);
      _scale = new Vector(1, 1, 1);
      _rotationAxis = new Vector(0, 0, 0);
      _rotationAngle = 0;
    }

    internal StaticModel Clone()
    {
      ListArray<Link<Texture, StaticMesh>> meshesClone = new ListArray<Link<Texture, StaticMesh>>(_meshes.Count);
      for (int i = 0; i < _meshes.Count; i++)
        meshesClone.Add(_meshes[i]);
      return new StaticModel(_id, meshesClone);
    }
  }
}