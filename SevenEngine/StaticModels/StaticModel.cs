// Seven
// https://github.com/53V3N1X/SevenEngine
// LISCENSE: See "LISCENSE.txt" in th root project directory.
// SUPPORT: See "README.txt" in the root project directory.

using Seven;
using Seven.Structures;
using SevenEngine.Imaging;
using Seven.Mathematics;
using SevenEngine.Shaders;

namespace SevenEngine.StaticModels
{
  /// <summary>Represents a collection of static meshes that all use the same model-view matrix.</summary>
  public class StaticModel
  {
    protected string _id;
    protected Vector<float> _position;
    protected Vector<float> _scale;
    protected Quaternion<float> _orientation;
    protected ShaderProgram _shaderOverride;
    protected AvlTree<StaticMesh> _meshes;
    
    /// <summary>Gets the list of meshes that make up this model.</summary>
    public AvlTree<StaticMesh> Meshes { get { return _meshes; } set { _meshes = value; } }
    /// <summary>Look-up id for pulling the static model out of the databases.</summary>
    public string Id { get { return _id; } set { _id = value; } }
    /// <summary>The position vector of this static model (used in rendering transformations).</summary>
    public Vector<float> Position { get { return _position; } set { _position = value; } }
    /// <summary>The scale vector (scale of each axis separately) of this static model (used in rendering transformations).</summary>
    public Vector<float> Scale { get { return _scale; } set { _scale = value; } }
    /// <summary>Represents the orientation of a static model by a quaternion rotation.</summary>
    public Quaternion<float> Orientation { get { return _orientation; } set { _orientation = value; } }
    /// <summary>Overrides the default shader while rendering this specific model.</summary>
    public ShaderProgram ShaderOverride { get { return _shaderOverride; } set { _shaderOverride = value; } }

    /// <summary>Creates a blank template for a static model (you will have to construct the model yourself).</summary>
    public StaticModel(string id)
    {
      _id = id;
      _shaderOverride = null;
      _meshes = new AvlTree_Linked<StaticMesh>(StaticMesh.CompareTo);
      _position = new Vector<float>(0, 0, 0);
      _scale = new Vector<float>(1, 1, 1);
      _orientation = Quaternion<float>.FactoryIdentity;
    }

    /// <summary>Creates a static model from the ids provided.</summary>
    /// <param name="staticModelId">The id to represent this model as.</param>
    /// <param name="textures">An array of the texture ids for each sub-mesh of this model.</param>
    /// <param name="meshes">An array of each mesh id for this model.</param>
    /// <param name="meshNames">An array of mesh names for this specific instanc3e of a static model.</param>
    internal StaticModel(string staticModelId, string[] meshNames, string[] meshes, string[] textures)
    {
      if (textures.Length != meshes.Length && textures.Length != meshNames.Length)
        throw new System.Exception("Attempting to create a static model with non-matching number of components.");
      _id = staticModelId;
      _meshes = new AvlTree_Linked<StaticMesh>(StaticMesh.CompareTo);
      for (int i = 0; i < textures.Length; i++)
      {
        StaticMesh mesh = StaticModelManager.GetMesh(meshes[i]);
        mesh.Texture = TextureManager.Get(textures[i]);
        _meshes.Add(mesh);

      }
      _shaderOverride = null;
      _position = new Vector<float>(0, 0, 0);
      _scale = new Vector<float>(1, 1, 1);
      _orientation = Quaternion<float>.FactoryIdentity;
    }

    /// <summary>Creates a static model out of the parameters.</summary>
    /// <param name="staticModelId">The id of this model for look up purposes.</param>
    /// <param name="meshes">A list of mesh ids, textures, and buffer references that make up this model.</param>
    internal StaticModel(string staticModelId, AvlTree<StaticMesh> meshes)
    {
      _id = staticModelId;
      _shaderOverride = null;
      _meshes = meshes;
      _position = new Vector<float>(0, 0, 0);
      _scale = new Vector<float>(1, 1, 1);
      _orientation = Quaternion<float>.FactoryIdentity;
    }

    public static Comparison CompareTo(StaticModel left, StaticModel right)
    {
      int comparison = left.Id.CompareTo(right.Id);
      if (comparison > 0)
        return Comparison.Greater;
      else if (comparison < 0)
        return Comparison.Less;
      else
        return Comparison.Equal;
    }

    public static Comparison CompareTo(StaticModel left, string right)
    {
      int comparison = left.Id.CompareTo(right);
      if (comparison > 0)
        return Comparison.Greater;
      else if (comparison < 0)
        return Comparison.Less;
      else
        return Comparison.Equal;
    }
  }
}