// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use under the following condition: any code using or originally taken 
// from the SevenEngine project must include citation to its original author(s) located at the
// top of each source code file, or you may include a reference to the SevenEngine project as
// a whole but you must include the current SevenEngine official website URL and logo.
// - Thanks.  :)  (support: seven@sevenengine.com)

// Author(s):
// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com
// Last Edited: 12-11-13

using SevenEngine.Imaging;
using SevenEngine.DataStructures;

namespace SevenEngine.StaticModels
{
  /// <summary>Represents a small wrapper for static meshes that adds a texture reference, and string id.
  /// The reason for this wrapper is for possible dismemberment from its StaticModel.</summary>
  public class StaticMesh
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

    public static int CompareTo(StaticMesh left, StaticMesh right) { return left.Id.CompareTo(right.Id); }
    public static int CompareTo(StaticMesh left, string right) { return left.Id.CompareTo(right); }
  }
}
