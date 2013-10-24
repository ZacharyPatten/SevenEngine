using System;

using SevenEngine.DataStructures;
using SevenEngine.Imaging;
using SevenEngine.Mathematics;
using SevenEngine.Shaders;

namespace SevenEngine.Models
{
  /// <summary>Represents single static model of values stored on RAM. Multiples references to the same 
  /// StaticModel should not exists because the contents are not being hardware instanced on the GPU.</summary>
  public class StaticModel
  {
    protected string _id;
    protected Vector _position;
    protected Vector _scale;
    protected Vector _rotationAxis;
    protected float _rotationAngle;
    protected ShaderProgram _shaderOverride;
    protected List<Link3<string, Texture, StaticMesh>> _meshes;
    
    /// <summary>Gets the list of meshes that make up this model.</summary>
    public List<Link3<string, Texture, StaticMesh>> Meshes { get { return _meshes; } set { _meshes = value; } }
    /// <summary>Look-up id for pulling the static model out of the databases.</summary>
    internal string Id { get { return _id; } set { _id = value; } }
    /// <summary>The position vector of this static model (used in rendering transformations).</summary>
    public Vector Position { get { return _position; } set { _position = value; } }
    /// <summary>The scale vector (scale of each axis separately) of this static model (used in rendering transformations).</summary>
    public Vector Scale { get { return _scale; } set { _scale = value; } }
    /// <summary>The axis of rotation for determining the orientation of this model (used in rendering transformations).</summary>
    public Vector RotationAmmounts { get { return _rotationAxis; } set { _rotationAxis = value; } }
    /// <summary>The angle of rotation about the "RotationAmmounts" vector for this model (used in rendering transformations).</summary>
    public float RotationAngle { get { return _rotationAngle; } set { _rotationAngle = value; } }
    /// <summary>This lets you change the shader per model. Instead of using the global default shader, it will use this one.</summary>
    public string ShaderOverride
    {
      get
      {
        // I haven't decided whether to throw an exception here or not, for now I will...
        if (_shaderOverride == null)
          //return "None";
          throw new NullReferenceException("There is no shader override for this model: " + _id);
        return _shaderOverride.Id;
      }
      set
      {
        // Decrease the number of hardware instancings of old shader
        if (_shaderOverride != null)
          _shaderOverride.ExistingReferences--;
        // Set the new shader by pulling it out of the ShaderProgram database (hardware instancings handle by the "Get" method)
        _shaderOverride = ShaderManager.GetShaderProgram(value);
      }
    }

    /// <summary>Creates a blank template for a static model (you will have to construct the model yourself).</summary>
    public StaticModel()
    {
      _id = "From Scratch";
      _shaderOverride = null;
      _meshes = new List<Link3<string, Texture, StaticMesh>>();
      _position = new Vector(0, 0, 0);
      _scale = new Vector(1, 1, 1);
      _rotationAxis = new Vector(0, 0, 0);
      _rotationAngle = 0;
    }

    /// <summary>Creates a static model from the ids provided.</summary>
    /// <param name="staticModelId">The id to represent this model as.</param>
    /// <param name="textures">An array of the texture ids for each sub-mesh of this model.</param>
    /// <param name="meshes">An array of each mesh id for this model.</param>
    /// <param name="meshNames">An array of mesh names for this specific instanc3e of a static model.</param>
    internal StaticModel(string staticModelId, string[] textures, string[] meshes, string[] meshNames)
    {
      if (textures.Length != meshes.Length && textures.Length != meshNames.Length)
        throw new Exception("Attempting to create a static model with non-matching number of components.");

      _id = staticModelId;
      //_meshes = new ListArray<Link<Texture, StaticMesh>>(10);
      _meshes = new List<Link3<string, Texture, StaticMesh>>();

      for (int i = 0; i < textures.Length; i++)
        _meshes.Add(meshNames[i], new Link3<string, Texture, StaticMesh>(meshNames[i], TextureManager.Get(textures[i]), StaticModelManager.GetMesh(meshes[i])));

      _shaderOverride = null;
      _position = new Vector(0, 0, 0);
      _scale = new Vector(1, 1, 1);
      _rotationAxis = new Vector(0, 0, 0);
      _rotationAngle = 0;
    }

    /// <summary>Creates a static model out of the parameters.</summary>
    /// <param name="staticModelId">The id of this model for look up purposes.</param>
    /// <param name="meshes">A list of mesh ids, textures, and buffer references that make up this model.</param>
    internal StaticModel(string staticModelId, List<Link3<string, Texture, StaticMesh>> meshes)
    {
      _id = staticModelId;
      _shaderOverride = null;
      _meshes = meshes;
      _position = new Vector(0, 0, 0);
      _scale = new Vector(1, 1, 1);
      _rotationAxis = new Vector(0, 0, 0);
      _rotationAngle = 0;
    }
  }
}