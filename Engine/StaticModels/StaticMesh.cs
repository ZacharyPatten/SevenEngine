
using SevenEngine.Imaging;
using SevenEngine.DataStructures.Interfaces;

namespace SevenEngine.StaticModels
{
  /// <summary>Represents a small wrapper for static meshes that adds a texture reference, and string id.
  /// The reason for this wrapper is for possible dismemberment from its StaticModel.</summary>
  public class StaticMesh : InterfaceStringId
  {
    private string _id;
    private Texture _texture;
    private StaticMeshInstance _staticMesh;

    public string Id { get { return _id; } set { _id = value; } }
    public Texture Texture { get { return _texture; } set { _texture = value; } }
    internal StaticMeshInstance StaticMeshInstance { get { return _staticMesh; } set { _staticMesh = value; } }

    internal StaticMesh(string id, Texture texture, StaticMeshInstance staticMesh)
    {
      _id = id;
      _texture = texture;
      _staticMesh = staticMesh;
    }
  }
}
