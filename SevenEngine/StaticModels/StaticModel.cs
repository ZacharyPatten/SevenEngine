// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use under the following condition: any code using or originally taken 
// from the SevenEngine project must include citation to its original author(s) located at the
// top of each source code file, or you may include a reference to the SevenEngine project as
// a whole but you must include the current SevenEngine official website URL and logo.
// - Thanks.  :)  (support: seven@sevenengine.com)

// Author(s):
// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com

using System;
using SevenEngine.DataStructures;
using SevenEngine.Imaging;
using SevenEngine.Mathematics;
using SevenEngine.Shaders;

namespace SevenEngine.StaticModels
{
  /// <summary>Represents a collection of static meshes that all use the same model-view matrix.</summary>
  public class StaticModel : IOctreeEntry
  {
    protected string _id;
    protected Vector _position;
    protected Vector _scale;
    protected Quaternion _orientation;
    protected ShaderProgram _shaderOverride;
    protected AvlTree<StaticMesh, string> _meshes;
    
    /// <summary>Gets the list of meshes that make up this model.</summary>
    public AvlTree<StaticMesh, string> Meshes { get { return _meshes; } set { _meshes = value; } }
    /// <summary>Look-up id for pulling the static model out of the databases.</summary>
    public string Id { get { return _id; } set { _id = value; } }
    /// <summary>The position vector of this static model (used in rendering transformations).</summary>
    public Vector Position { get { return _position; } set { _position = value; } }
    /// <summary>The scale vector (scale of each axis separately) of this static model (used in rendering transformations).</summary>
    public Vector Scale { get { return _scale; } set { _scale = value; } }
    /// <summary>Represents the orientation of a static model by a quaternion rotation.</summary>
    public Quaternion Orientation { get { return _orientation; } set { _orientation = value; } }
    /// <summary>Overrides the default shader while rendering this specific model.</summary>
    public ShaderProgram ShaderOverride { get { return _shaderOverride; } set { _shaderOverride = value; } }

    /// <summary>Creates a blank template for a static model (you will have to construct the model yourself).</summary>
    public StaticModel(string id)
    {
      _id = id;
      _shaderOverride = null;
      _meshes = new AvlTreeLinked<StaticMesh, string>(StaticMesh.CompareTo, StaticMesh.CompareTo);
      _position = new Vector(0, 0, 0);
      _scale = new Vector(1, 1, 1);
      _orientation = Quaternion.FactoryIdentity;
    }

    /// <summary>Creates a static model from the ids provided.</summary>
    /// <param name="staticModelId">The id to represent this model as.</param>
    /// <param name="textures">An array of the texture ids for each sub-mesh of this model.</param>
    /// <param name="meshes">An array of each mesh id for this model.</param>
    /// <param name="meshNames">An array of mesh names for this specific instanc3e of a static model.</param>
    internal StaticModel(string staticModelId, string[] meshNames, string[] meshes, string[] textures)
    {
      if (textures.Length != meshes.Length && textures.Length != meshNames.Length)
        throw new Exception("Attempting to create a static model with non-matching number of components.");
      _id = staticModelId;
      _meshes = new AvlTreeLinked<StaticMesh, string>(StaticMesh.CompareTo, StaticMesh.CompareTo);
      for (int i = 0; i < textures.Length; i++)
      {
        StaticMesh mesh = StaticModelManager.GetMesh(meshes[i]);
        mesh.Texture = TextureManager.Get(textures[i]);
        _meshes.Add(mesh);

      }
      _shaderOverride = null;
      _position = new Vector(0, 0, 0);
      _scale = new Vector(1, 1, 1);
      _orientation = Quaternion.FactoryIdentity;
    }

    /// <summary>Creates a static model out of the parameters.</summary>
    /// <param name="staticModelId">The id of this model for look up purposes.</param>
    /// <param name="meshes">A list of mesh ids, textures, and buffer references that make up this model.</param>
    internal StaticModel(string staticModelId, AvlTree<StaticMesh, string> meshes)
    {
      _id = staticModelId;
      _shaderOverride = null;
      _meshes = meshes;
      _position = new Vector(0, 0, 0);
      _scale = new Vector(1, 1, 1);
      _orientation = Quaternion.FactoryIdentity;
    }

    public static int CompareTo(StaticModel left, StaticModel right) { return left.Id.CompareTo(right.Id); }
    public static int CompareTo(StaticModel left, string right) { return left.Id.CompareTo(right); }
  }
}