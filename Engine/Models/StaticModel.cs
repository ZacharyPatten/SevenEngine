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

using System;

using SevenEngine.DataStructures;
using SevenEngine.DataStructures.Interfaces;
using SevenEngine.Imaging;
using SevenEngine.Mathematics;
using SevenEngine.Shaders;

namespace SevenEngine.Models
{
  /// <summary>Represents a single mesh that has been loaded on the GPU. multiple references of this class SHOULD exist,
  /// because each reference of this class means another hardware instance. Hardware instancing is when you re-use the
  /// same buffers on the GPU, which is good for both speed and memory space.</summary>
  public class StaticMesh : InterfaceStringId
  {
    private int _existingReferences;
    protected string _filePath;
    protected string _id;
    protected int _vertexBufferHandle;
    protected int _colorBufferHandle;
    protected int _textureCoordinateBufferHandle;
    protected int _normalBufferHandle;
    protected int _elementBufferHandle;
    protected int _vertexCount;

    /// <summary>The number of existing hardware instances of this model reference.</summary>
    public int ExistingReferences { get { return _existingReferences; } set { _existingReferences = value; } }
    /// <summary>Holds the filepath of the imported file.</summary>
    internal string FilePath { get { return _filePath; } set { _filePath = value; } }
    /// <summary>The id associated with this mesh in the "StaticModelManager".</summary>
    public string Id { get { return _id; } set { _id = value; } }
    /// <summary>The handle of the vertex buffer on the GPU.</summary>
    internal int VertexBufferHandle { get { return _vertexBufferHandle; } set { _vertexBufferHandle = value; } }
    /// <summary>The location of the color buffer on the GPU.</summary>
    internal int ColorBufferHandle { get { return _colorBufferHandle; } set { _colorBufferHandle = value; } }
    /// <summary>The location of the texture coordinate buffer on the GPU.</summary>
    internal int TextureCoordinateBufferHandle { get { return _textureCoordinateBufferHandle; } set { _textureCoordinateBufferHandle = value; } }
    /// <summary>The location of the normal buffer on the GPU.</summary>
    internal int NormalBufferHandle { get { return _normalBufferHandle; } set { _normalBufferHandle = value; } }
    /// <summary>The location of the element buffer on the GPU.</summary>
    internal int ElementBufferHandle { get { return _elementBufferHandle; } set { _elementBufferHandle = value; } }
    /// <summary>The number of verteces in this model.</summary>
    internal int VertexCount { get { return _vertexCount; } set { _vertexCount = value; } }

    /// <summary>Creates an instance of a StaticMesh.</summary>
    /// <param name="filePath">The file path of the model that the data came from.</param>
    /// <param name="id">The name associated with this mash when in was created.</param>
    /// <param name="vertexBufferHandle">The number reference of the vertex buffer on the GPU (default is 0).</param>
    /// <param name="colorBufferHandle">The number reference of the color buffer on the GPU (default is 0).</param>
    /// <param name="textureCoordinatesHandle">The number reference of the texture coordinate buffer on the GPU (default is 0).</param>
    /// <param name="normalBufferHandle">The number reference of the normal buffer on the GPU (default is 0).</param>
    /// <param name="elementBufferHandle">The number reference of the element buffer on the GPU (default is 0).</param>
    /// <param name="vertexCount">The number of verteces making up the mesh.</param>
    internal StaticMesh(
      string filePath,
      string id,
      int vertexBufferHandle,
      int colorBufferHandle,
      int textureCoordinatesHandle,
      int normalBufferHandle,
      int elementBufferHandle,
      int vertexCount)
    {
      _existingReferences = 0;

      _filePath = filePath;
      _id = id;
      _vertexBufferHandle = vertexBufferHandle;
      _colorBufferHandle = colorBufferHandle;
      _textureCoordinateBufferHandle = textureCoordinatesHandle;
      _normalBufferHandle = normalBufferHandle;
      _elementBufferHandle = elementBufferHandle;
      _vertexCount = vertexCount;
    }
  }

  public class StaticModelMesh : InterfaceStringId
  {
    private string _id;
    private Texture _texture;
    private StaticMesh _staticMesh;

    public string Id { get { return _id; } set { _id = value; } }
    public Texture Texture { get { return _texture; } set { _texture = value; } }
    public StaticMesh StaticMesh { get { return _staticMesh; } set { _staticMesh = value; } }

    public StaticModelMesh(string id, Texture texture, StaticMesh staticMesh)
    {
      _id = id;
      _texture = texture;
      _staticMesh = staticMesh;
    }
  }

  /// <summary>Represents a collection of static meshes that all use the same transformational matrices. Multiples references to the same 
  /// StaticModel should not exists because the contents are NOT being hardware instanced on the GPU.</summary>
  public class StaticModel : InterfaceStringId, InterfacePositionVector
  {
    protected string _id;
    protected Vector _position;
    protected Vector _scale;
    protected Quaternion _orientation;
    protected ShaderProgram _shaderOverride;
    protected List<StaticModelMesh> _meshes;
    
    /// <summary>Gets the list of meshes that make up this model.</summary>
    public List<StaticModelMesh> Meshes { get { return _meshes; } set { _meshes = value; } }
    /// <summary>Look-up id for pulling the static model out of the databases.</summary>
    public string Id { get { return _id; } set { _id = value; } }
    /// <summary>The position vector of this static model (used in rendering transformations).</summary>
    public Vector Position { get { return _position; } set { _position = value; } }
    /// <summary>The scale vector (scale of each axis separately) of this static model (used in rendering transformations).</summary>
    public Vector Scale { get { return _scale; } set { _scale = value; } }
    /// <summary>Represents the orientation of a static model by a quaternion rotation.</summary>
    public Quaternion Orientation { get { return _orientation; } set { _orientation = value; } }
    
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
      _meshes = new List<StaticModelMesh>();
      _position = new Vector(0, 0, 0);
      _scale = new Vector(1, 1, 1);
      _orientation = Quaternion.FactoryIdentity;
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
      _meshes = new List<StaticModelMesh>();

      for (int i = 0; i < textures.Length; i++)
        _meshes.Add(new StaticModelMesh(meshNames[i], TextureManager.Get(textures[i]), StaticModelManager.GetMesh(meshes[i])));

      _shaderOverride = null;
      _position = new Vector(0, 0, 0);
      _scale = new Vector(1, 1, 1);
      _orientation = Quaternion.FactoryIdentity;
    }

    /// <summary>Creates a static model out of the parameters.</summary>
    /// <param name="staticModelId">The id of this model for look up purposes.</param>
    /// <param name="meshes">A list of mesh ids, textures, and buffer references that make up this model.</param>
    internal StaticModel(string staticModelId, List<StaticModelMesh> meshes)
    {
      _id = staticModelId;
      _shaderOverride = null;
      _meshes = meshes;
      _position = new Vector(0, 0, 0);
      _scale = new Vector(1, 1, 1);
      _orientation = Quaternion.FactoryIdentity;
    }
  }
}