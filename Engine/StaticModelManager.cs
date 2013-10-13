using System;
using System.Collections.Generic;
using System.IO;

using Engine.Models;
using Engine.Imaging;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine
{
  public static class StaticModelManager
  {
    private static Dictionary<string, StaticMesh> _staticModelDatabase = new Dictionary<string, StaticMesh>();
    //private static Dictionary<string, StaticMesh> _staticModelDatabase = new Dictionary<string, StaticMesh>();

    /// <summary>The number of meshes currently loaded onto the graphics card.</summary>
    public static int Count { get { return _staticModelDatabase.Count; } }

    public static StaticMesh Get(string staticMeshId)
    {
      StaticMesh mesh = _staticModelDatabase[staticMeshId];
      mesh.ExistingReferences++;
      return mesh;
    }

    /// <summary>Loads an 3d model file. NOTE that only obj files are currently supported.</summary>
    /// <param name="textureManager">The texture manager so that the mesh can automatically texture itself.</param>
    /// <param name="staticMeshId">The key used to look up this mesh in the database.</param>
    /// <param name="filePath">The filepath of the model file you are attempting to load.</param>
    public static void LoadModel(string staticMeshId, string filePath)
    {
      _staticModelDatabase.Add(staticMeshId, LoadObj(staticMeshId, filePath));
      string[] pathSplit = filePath.Split('\\');
      Output.WriteLine("Model file loaded: \"" + pathSplit[pathSplit.Length - 1] + "\".");
    }

    public static void RemoveModel(string staticMeshId)
    {
      // Get the struct with the GPU mappings.
      StaticMesh removal = Get(staticMeshId);

      // If the game tries to remove a texture that still has active references then
        // lets warn them.
      if (removal.ExistingReferences > 1)
      {
        Output.WriteLine("WARNING: texture removal \"" + staticMeshId + "\" still has active references.");
      }

      // Delete the vertex buffer if it exists.
      int vertexBufferId = removal.VertexBufferHandle;
      if (vertexBufferId != 0)
        GL.DeleteBuffers(1, ref vertexBufferId);
      // Delete the normal buffer if it exists.
      int normalbufferId = removal.NormalBufferHandle;
      if (normalbufferId != 0)
        GL.DeleteBuffers(1, ref normalbufferId);
      // Delete the color buffer if it exists.
      int colorBufferId = removal.ColorBufferHandle;
      if (colorBufferId != 0)
        GL.DeleteBuffers(1, ref colorBufferId);
      // Delete the texture coordinate buffer if it exists.
      int textureCoordinateBufferId = removal.TextureCoordinateBufferHandle;
      if (textureCoordinateBufferId != 0)
        GL.DeleteBuffers(1, ref textureCoordinateBufferId);
      // Delete the element buffer if it exists.
      int elementBufferId = removal.ElementBufferHandle;
      if (elementBufferId != 0)
        GL.DeleteBuffers(1, ref elementBufferId);
      // Now we can remove it from the dictionary.
      _staticModelDatabase.Remove(staticMeshId);
    }

    public static StaticMesh LoadObj(string staticMeshId, string filePath)
    {
      // These are temporarily needed lists for storing the parsed data as you read it.
      List<float> fileVerteces = new List<float>();
      List<float> fileNormals = new List<float>();
      List<float> fileTextureCoordinates = new List<float>();
      List<int> fileIndeces = new List<int>();
      Texture texture;

      // Lets read the file and handle each line separately for ".obj" files
      using (StreamReader reader = new StreamReader(filePath))
      {
        while (!reader.EndOfStream)
        {
          string[] parameters = reader.ReadLine().Trim().Split(' ');
          switch (parameters[0])
          {
            // Texture
            case "t":
              TextureManager.LoadTexture(parameters[1], parameters[1]);
              texture = TextureManager.Get(parameters[1]);
              break;

            // Vertex
            case "v":
              fileVerteces.Add(float.Parse(parameters[1]));
              fileVerteces.Add(float.Parse(parameters[2]));
              fileVerteces.Add(float.Parse(parameters[3]));
              break;

            // Texture Coordinate
            case "vt":
              fileTextureCoordinates.Add(float.Parse(parameters[1]));
              fileTextureCoordinates.Add(float.Parse(parameters[2]));
              break;

            // Normal
            case "vn":
              fileNormals.Add(float.Parse(parameters[1]));
              fileNormals.Add(float.Parse(parameters[2]));
              fileNormals.Add(float.Parse(parameters[3]));
              break;

            // Face
            case "f":
              // NOTE! This does not yet triangulate faces
              // NOTE! This needs all possible values (position, texture mapping, and normal).
              for (int i = 1; i < parameters.Length; i++)
              {
                string[] indexReferences = parameters[i].Split('/');
                fileIndeces.Add(int.Parse(indexReferences[0]));
                if (indexReferences[1] != "")
                  fileIndeces.Add(int.Parse(indexReferences[1]));
                else
                  fileIndeces.Add(0);
                if (indexReferences[2] != "")
                  fileIndeces.Add(int.Parse(indexReferences[2]));
                else
                  fileIndeces.Add(0);
              }
              break;
          }
        }
      }

      // Pull the final vertex order out of the indexed references
      // Note, arrays start at 0 but the index references start at 1
      float[] verteces = new float[fileIndeces.Count];
      for (int i = 0; i < fileIndeces.Count; i += 3)
      {
        int index = (fileIndeces[i] - 1) * 3;
        verteces[i] = fileVerteces[index];
        verteces[i + 1] = fileVerteces[index + 1];
        verteces[i + 2] = fileVerteces[index + 2];
      }

      // Pull the final texture coordinates order out of the indexed references
      // Note, arrays start at 0 but the index references start at 1
      // Note, every other value needs to be inverse (not sure why but it works :P)
      float[] textureCoordinates = new float[fileIndeces.Count / 3 * 2];
      for (int i = 1; i < fileIndeces.Count; i += 3)
      {
        int index = (fileIndeces[i] - 1) * 2;
        int offset = (i - 1) / 3;
        textureCoordinates[i - 1 - offset] = fileTextureCoordinates[index];
        textureCoordinates[i - offset] = 1 - fileTextureCoordinates[(index + 1)];
      }

      // Pull the final normal order out of the indexed references
      // Note, arrays start at 0 but the index references start at 1
      float[] normals = new float[fileIndeces.Count];
      for (int i = 2; i < fileIndeces.Count; i += 3)
      {
        int index = (fileIndeces[i] - 1) * 3;
        normals[i - 2] = fileNormals[index];
        normals[i - 1] = fileNormals[(index + 1)];
        normals[i] = fileNormals[(index + 2)];
      }

      int vertexBufferId;
      if (verteces != null)
      {
        // Declare the buffer
        GL.GenBuffers(1, out vertexBufferId);
        // Select the new buffer
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferId);
        // Initialize the buffer values
        GL.BufferData<float>(BufferTarget.ArrayBuffer, (IntPtr)(verteces.Length * sizeof(float)), verteces, BufferUsageHint.StaticDraw);
        // Quick error checking
        int bufferSize;
        GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
        if (verteces.Length * sizeof(float) != bufferSize)
          throw new ApplicationException("Vertex array not uploaded correctly");
        // Deselect the new buffer
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
      }
      else { vertexBufferId = 0; }

      int textureCoordinateBufferId;
      if (textureCoordinates != null)
      {
        // Declare the buffer
        GL.GenBuffers(1, out textureCoordinateBufferId);
        // Select the new buffer
        GL.BindBuffer(BufferTarget.ArrayBuffer, textureCoordinateBufferId);
        // Initialize the buffer values
        GL.BufferData<float>(BufferTarget.ArrayBuffer, (IntPtr)(textureCoordinates.Length * sizeof(float)), textureCoordinates, BufferUsageHint.StaticDraw);
        // Quick error checking
        int bufferSize;
        GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
        if (textureCoordinates.Length * sizeof(float) != bufferSize)
          throw new ApplicationException("TexCoord array not uploaded correctly");
        // Deselect the new buffer
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
      }
      else { textureCoordinateBufferId = 0; }

      int normalBufferId;
      if (normals != null)
      {
        // Declare the buffer
        GL.GenBuffers(1, out normalBufferId);
        // Select the new buffer
        GL.BindBuffer(BufferTarget.ArrayBuffer, normalBufferId);
        // Initialize the buffer values
        GL.BufferData<float>(BufferTarget.ArrayBuffer, (IntPtr)(normals.Length * sizeof(float)), normals, BufferUsageHint.StaticDraw);
        // Quick error checking
        int bufferSize;
        GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
        if (normals.Length * sizeof(float) != bufferSize)
          throw new ApplicationException("Normal array not uploaded correctly");
        // Deselect the new buffer
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
      }
      else { normalBufferId = 0; }

      return new StaticMesh(
        filePath,
        staticMeshId,
        vertexBufferId,
        0, // Obj files don't support vertex colors
        textureCoordinateBufferId,
        normalBufferId,
        0, // I don't support an index buffer at this time
        verteces.Length);
    }
  }
}