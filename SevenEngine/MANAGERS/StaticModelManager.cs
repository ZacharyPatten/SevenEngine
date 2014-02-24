// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use under the following condition: any code using or originally taken 
// from the SevenEngine project must include citation to its original author(s) located at the
// top of each source code file, or you may include a reference to the SevenEngine project as
// a whole but you must include the current SevenEngine official website URL and logo.
// - Thanks.  :)  (support: seven@sevenengine.com)

// Author(s):
// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com
// Last Edited: 11-16-13

using System;
using System.IO;
using System.Globalization;
using SevenEngine.DataStructures;
using SevenEngine.StaticModels;
using SevenEngine.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SevenEngine
{
  /// <summary>StaticModelManager is used for rigid-body model management (loading, storing, hardware instance controling, and disposing).</summary>
  public static class StaticModelManager
  {
    private static AvlTree<StaticMesh> _staticMeshDatabase = new AvlTreeLinked<StaticMesh>(StaticMesh.CompareTo);
    private static AvlTree<StaticModel> _staticModelDatabase = new AvlTreeLinked<StaticModel>(StaticModel.CompareTo);

    /// <summary>The number of meshes currently loaded onto the graphics card.</summary>
    public static int MeshCount { get { return _staticMeshDatabase.Count; } }
    /// <summary>The number of models currently loaded onto the graphics card.</summary>
    public static int ModelCount { get { return _staticModelDatabase.Count; } }

    /// <summary>USE "GetModel" UNLESS YOU KNOW WHAT YOU ARE DOING!!!</summary>
    /// <param name="staticMeshId">The name id of the mesh you wish to obtain.</param>
    /// <returns>The desired static mesh if it exists.</returns>
    public static StaticMesh GetMesh(string staticMeshId)
    {
      StaticMesh mesh = _staticMeshDatabase.Get<string>(staticMeshId, StaticMesh.CompareTo);
      mesh.ExistingReferences++;
      return mesh;
    }

    /// <summary>Gets a static model you loaded that you have loaded.</summary>
    /// <param name="staticModelId">The name id of the model you wish to obtain.</param>
    /// <returns>The desired static model if it exists.</returns>
    public static StaticModel GetModel(string staticModelId)
    {
      StaticModel modelToGet = _staticModelDatabase.Get<string>(staticModelId, StaticModel.CompareTo);
      AvlTree<StaticMesh> meshes = new AvlTreeLinked<StaticMesh>(StaticMesh.CompareTo);
      modelToGet.Meshes.Traverse
      (
        (StaticMesh mesh) =>
        {
          mesh.Texture.ExistingReferences++;
          mesh.ExistingReferences++;
          meshes.Add(mesh);
        }
      );
      return new StaticModel(modelToGet.Id, meshes);
    }

    #region Parsers

    /// <summary>Loads an 3d model file. NOTE that only obj files are currently supported.</summary>
    /// <param name="textureManager">The texture manager so that the mesh can automatically texture itself.</param>
    /// <param name="staticMeshId">The key used to look up this mesh in the database.</param>
    /// <param name="filePath">The filepath of the model file you are attempting to load.</param>
    public static void LoadMesh(string staticMeshId, string filePath)
    {
      _staticMeshDatabase.Add(LoadObj(staticMeshId, filePath));
      string[] pathSplit = filePath.Split('\\');
      Output.WriteLine("Model file loaded: \"" + pathSplit[pathSplit.Length - 1] + "\".");
    }

    /// <summary>Loads an 3d model file. NOTE that only obj files are currently supported.</summary>
    /// <param name="textureManager">The texture manager so that the mesh can automatically texture itself.</param>
    /// <param name="staticMeshId">The key used to look up this mesh in the database.</param>
    /// <param name="filePath">The filepath of the model file you are attempting to load.</param>
    public static void LoadSevenModel(string staticModelId, string filePath)
    {
      _staticModelDatabase.Add(LoadSevenModelFromDisk(staticModelId, filePath));
      string[] pathSplit = filePath.Split('\\');
      Output.WriteLine("Model file loaded: \"" + pathSplit[pathSplit.Length - 1] + "\".");
    }

    public static void LoadModel(string staticModelId, string[] meshNames, string[] meshs, string[] textures)
    { _staticModelDatabase.Add(new StaticModel(staticModelId, meshNames, meshs, textures)); }

    public static void LoadModel(string staticModelId)
    { _staticModelDatabase.Add(new StaticModel(staticModelId)); }

    public static void RemoveModel(string staticMeshId)
    {
      // Get the struct with the GPU mappings.
      StaticMesh mesh = GetMesh(staticMeshId);

      // If the game tries to remove a texture that still has active references then
        // lets warn them.
      if (mesh.ExistingReferences > 1)
        Output.WriteLine("WARNING: texture removal \"" + staticMeshId + "\" still has active references.");

      // Delete the vertex buffer if it exists.
      int vertexBufferId = mesh.VertexBufferHandle;
      if (vertexBufferId != 0)
        GL.DeleteBuffers(1, ref vertexBufferId);
      // Delete the normal buffer if it exists.
      int normalbufferId = mesh.NormalBufferHandle;
      if (normalbufferId != 0)
        GL.DeleteBuffers(1, ref normalbufferId);
      // Delete the color buffer if it exists.
      int colorBufferId = mesh.ColorBufferHandle;
      if (colorBufferId != 0)
        GL.DeleteBuffers(1, ref colorBufferId);
      // Delete the texture coordinate buffer if it exists.
      int textureCoordinateBufferId = mesh.TextureCoordinateBufferHandle;
      if (textureCoordinateBufferId != 0)
        GL.DeleteBuffers(1, ref textureCoordinateBufferId);
      // Delete the element buffer if it exists.
      int elementBufferId = mesh.ElementBufferHandle;
      if (elementBufferId != 0)
        GL.DeleteBuffers(1, ref elementBufferId);
      // Now we can remove it from the dictionary.
      _staticMeshDatabase.Remove<string>(staticMeshId, StaticMesh.CompareTo);
    }

    private static StaticMesh LoadObj(string staticMeshId, string filePath)
    {
      // These are temporarily needed lists for storing the parsed data as you read it.
      // Its better to use "ListArrays" vs "Lists" because they will be accessed by indeces
      // by the faces of the obj file.
      ListArray<float> fileVerteces = new ListArray<float>(10000);
      ListArray<float> fileNormals = new ListArray<float>(10000);
      ListArray<float> fileTextureCoordinates = new ListArray<float>(10000);
      ListArray<int> fileIndeces = new ListArray<int>(10000);

      // Obj files are not required to include texture coordinates or normals
      bool hasTextureCoordinates = true;
      bool hasNormals = true;

      // Lets read the file and handle each line separately for ".obj" files
      using (StreamReader reader = new StreamReader(filePath))
      {
        int lineNumber = 1;
        while (!reader.EndOfStream)
        {
          try
          {
            string[] parameters = reader.ReadLine().Trim().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            switch (parameters[0])
            {
              // Vertex
              case "v":
                fileVerteces.Add(float.Parse(parameters[1], CultureInfo.InvariantCulture));
                fileVerteces.Add(float.Parse(parameters[2], CultureInfo.InvariantCulture));
                fileVerteces.Add(float.Parse(parameters[3], CultureInfo.InvariantCulture));
                break;

              // Texture Coordinate
              case "vt":
                fileTextureCoordinates.Add(float.Parse(parameters[1], CultureInfo.InvariantCulture));
                fileTextureCoordinates.Add(float.Parse(parameters[2], CultureInfo.InvariantCulture));
                break;

              // Normal
              case "vn":
                fileNormals.Add(float.Parse(parameters[1], CultureInfo.InvariantCulture));
                fileNormals.Add(float.Parse(parameters[2], CultureInfo.InvariantCulture));
                fileNormals.Add(float.Parse(parameters[3], CultureInfo.InvariantCulture));
                break;

              // Face
              case "f":
                //if (parameters.Length < 4)
                //  throw new StaticModelManagerException("obj file corrupt.");
                int first = fileIndeces.Count;
                for (int i = 1; i < parameters.Length; i++)
                {
                  if (i > 3)
                  {
                    // Triangulate using the previous two verteces
                    // NOTE: THIS MAY BE INCORRECT! I COULD NOT YET FIND DOCUMENTATION
                    // ON THE TRIANGULATION DONE BY BLENDER (WORKS FOR QUADS AT LEAST)

                    //// Last two (triangle strip)
                    //fileIndeces.Add(fileIndeces[fileIndeces.Count - 6]);
                    //fileIndeces.Add(fileIndeces[fileIndeces.Count - 6]);
                    //fileIndeces.Add(fileIndeces[fileIndeces.Count - 6]);
                    //fileIndeces.Add(fileIndeces[fileIndeces.Count - 6]);
                    //fileIndeces.Add(fileIndeces[fileIndeces.Count - 6]);
                    //fileIndeces.Add(fileIndeces[fileIndeces.Count - 6]);

                    // First then previous (triangle fan)
                    fileIndeces.Add(fileIndeces[first]);
                    fileIndeces.Add(fileIndeces[first + 1]);
                    fileIndeces.Add(fileIndeces[first + 2]);
                    fileIndeces.Add(fileIndeces[fileIndeces.Count - 6]);
                    fileIndeces.Add(fileIndeces[fileIndeces.Count - 6]);
                    fileIndeces.Add(fileIndeces[fileIndeces.Count - 6]);
                  }

                  // Now include the new vertex
                  string[] indexReferences = parameters[i].Split('/');
                  //if (indexReferences[0] == "")
                  //  throw new StaticModelManagerException("ERROR: obj file corrupted (missing vertex possition):" + filePath);
                  fileIndeces.Add(int.Parse(indexReferences[0], CultureInfo.InvariantCulture));

                  if (hasNormals && indexReferences.Length < 3)
                    hasNormals = false;
                  if (hasTextureCoordinates && (indexReferences.Length < 2 || indexReferences[1] == ""))
                    hasTextureCoordinates = false;

                  if (hasTextureCoordinates && indexReferences[1] != "")
                    fileIndeces.Add(int.Parse(indexReferences[1], CultureInfo.InvariantCulture));
                  else
                    fileIndeces.Add(0);

                  if (hasNormals && indexReferences[2] != "")
                    fileIndeces.Add(int.Parse(indexReferences[2], CultureInfo.InvariantCulture));
                  else
                    fileIndeces.Add(0);
                }
                break;
            }
          }
          catch
          {
            string[] pathSplit = filePath.Split('\\');
            throw new StaticModelManagerException("Could not load model " + pathSplit[pathSplit.Length - 1] +
              ". There is a corruption on line " + lineNumber +".");
          }
          lineNumber++;
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

      float[] textureCoordinates = null;
      if (hasTextureCoordinates)
      {
        // Pull the final texture coordinates order out of the indexed references
        // Note, arrays start at 0 but the index references start at 1
        // Note, every other value needs to be inverse (not sure why but it works :P)
        textureCoordinates = new float[fileIndeces.Count / 3 * 2];
        for (int i = 1; i < fileIndeces.Count; i += 3)
        {
          int index = (fileIndeces[i] - 1) * 2;
          int offset = (i - 1) / 3;
          textureCoordinates[i - 1 - offset] = fileTextureCoordinates[index];
          textureCoordinates[i - offset] = 1 - fileTextureCoordinates[(index + 1)];
        }
      }

      float[] normals = null;
      if (hasNormals)
      {
        // Pull the final normal order out of the indexed references
        // Note, arrays start at 0 but the index references start at 1
        normals = new float[fileIndeces.Count];
        for (int i = 2; i < fileIndeces.Count; i += 3)
        {
          int index = (fileIndeces[i] - 1) * 3;
          normals[i - 2] = fileNormals[index];
          normals[i - 1] = fileNormals[(index + 1)];
          normals[i] = fileNormals[(index + 2)];
        }
      }

      int vertexBufferId;
      if (verteces != null)
      {
        // Make the vertex buffer on the GPU
        GL.GenBuffers(1, out vertexBufferId);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferId);
        GL.BufferData<float>(BufferTarget.ArrayBuffer, (IntPtr)(verteces.Length * sizeof(float)), verteces, BufferUsageHint.StaticDraw);
        int bufferSize;
        GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
        if (verteces.Length * sizeof(float) != bufferSize)
          throw new StaticModelManagerException("Vertex array not uploaded correctly");
        // Deselect the new buffer
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
      }
      else { vertexBufferId = 0; }

      int textureCoordinateBufferId;
      if (hasTextureCoordinates && textureCoordinates != null)
      {
        // Make the texture coordinate buffer on the GPU
        GL.GenBuffers(1, out textureCoordinateBufferId);
        GL.BindBuffer(BufferTarget.ArrayBuffer, textureCoordinateBufferId);
        GL.BufferData<float>(BufferTarget.ArrayBuffer, (IntPtr)(textureCoordinates.Length * sizeof(float)), textureCoordinates, BufferUsageHint.StaticDraw);
        int bufferSize;
        GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
        if (textureCoordinates.Length * sizeof(float) != bufferSize)
          throw new StaticModelManagerException("TexCoord array not uploaded correctly");
        // Deselect the new buffer
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
      }
      else { textureCoordinateBufferId = 0; }

      int normalBufferId;
      if (hasNormals && normals != null)
      {
        // Make the normal buffer on the GPU
        GL.GenBuffers(1, out normalBufferId);
        GL.BindBuffer(BufferTarget.ArrayBuffer, normalBufferId);
        GL.BufferData<float>(BufferTarget.ArrayBuffer, (IntPtr)(normals.Length * sizeof(float)), normals, BufferUsageHint.StaticDraw);
        int bufferSize;
        GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
        if (normals.Length * sizeof(float) != bufferSize)
          throw new StaticModelManagerException("Normal array not uploaded correctly");
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
        verteces.Length,
        null);
    }

    /// <summary>DONT USE THIS FUNCTION!!! This is an experimental file type I may use in the future.</summary>
    public static StaticModel LoadSevenModelFromDisk(string staticModelId, string filePath)
    {
      // These are temporarily needed lists for storing the parsed data as you read it.
      ListArray<float> fileVerteces = new ListArray<float>(1000);
      ListArray<float> fileNormals = new ListArray<float>(1000);
      ListArray<float> fileTextureCoordinates = new ListArray<float>(1000);
      ListArray<int> fileIndeces = new ListArray<int>(1000);
      Texture texture = null;
      string meshName = "defaultMeshName";

      AvlTreeLinked<StaticMesh> meshes = new AvlTreeLinked<StaticMesh>(StaticMesh.CompareTo);

      // Lets read the file and handle each line separately for ".obj" files
      using (StreamReader reader = new StreamReader(filePath))
      {
        while (!reader.EndOfStream)
        {
          string[] parameters = reader.ReadLine().Trim().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
          switch (parameters[0])
          {
            // MeshName
            case "m":
              meshName = parameters[1];
              break;

            // Texture
            case "t":
              if (!TextureManager.TextureExists(parameters[1]))
                TextureManager.LoadTexture(parameters[1], parameters[2]);
              texture = TextureManager.Get(parameters[1]);
              break;

            // Vertex
            case "v":
              fileVerteces.Add(float.Parse(parameters[1], CultureInfo.InvariantCulture));
              fileVerteces.Add(float.Parse(parameters[2], CultureInfo.InvariantCulture));
              fileVerteces.Add(float.Parse(parameters[3], CultureInfo.InvariantCulture));
              break;

            // Texture Coordinate
            case "vt":
              fileTextureCoordinates.Add(float.Parse(parameters[1], CultureInfo.InvariantCulture));
              fileTextureCoordinates.Add(float.Parse(parameters[2], CultureInfo.InvariantCulture));
              break;

            // Normal
            case "vn":
              fileNormals.Add(float.Parse(parameters[1], CultureInfo.InvariantCulture));
              fileNormals.Add(float.Parse(parameters[2], CultureInfo.InvariantCulture));
              fileNormals.Add(float.Parse(parameters[3], CultureInfo.InvariantCulture));
              break;

            // Face
            case "f":
              // DEVELOPMENT NOTE: The following triangulation algorithm works, but it 
              // could be optimized beyond its current state.

              // The following variables are used for triangulation of a polygon
              // with greater than three verteces.
              int firstPosition, firstTextureCoordinates, firstNormal,
                secondPosition, secondTextureCoordinates, secondNormal;
              if (parameters.Length > 3)
              {
                // First Vertex (we have to store it this way for possible triangulation)
                string[] indexReferences = parameters[1].Split('/');
                if (indexReferences[0] == "")
                  throw new StaticModelManagerException("ERROR: obj file corrupted (missing vertex possition): " + filePath);
                firstPosition = int.Parse(indexReferences[0], CultureInfo.InvariantCulture);
                if (indexReferences[1] != "")
                  firstTextureCoordinates = int.Parse(indexReferences[1], CultureInfo.InvariantCulture);
                else
                  firstTextureCoordinates = 0;
                if (indexReferences[2] != "")
                  firstNormal = int.Parse(indexReferences[2], CultureInfo.InvariantCulture);
                else
                  firstNormal = 0;

                // Second Vertex (we have to store it this way for possible triangulation)
                indexReferences = parameters[2].Split('/');
                if (indexReferences[0] == "")
                  throw new StaticModelManagerException("ERROR: obj file corrupted (missing vertex possition): " + filePath);
                secondPosition = int.Parse(indexReferences[0], CultureInfo.InvariantCulture);
                if (indexReferences[1] != "")
                  secondTextureCoordinates = int.Parse(indexReferences[1], CultureInfo.InvariantCulture);
                else
                  secondTextureCoordinates = 0;
                if (indexReferences[2] != "")
                  secondNormal = int.Parse(indexReferences[2], CultureInfo.InvariantCulture);
                else
                  secondNormal = 0;
              }
              else
                throw new StaticModelManagerException("ERROR: obj file corrupted:" + filePath);

              // Verteces past the first two
              for (int i = 3; i < parameters.Length; i++)
              {
                // Triangulate using the first two verteces
                fileIndeces.Add(firstPosition);
                fileIndeces.Add(firstTextureCoordinates);
                fileIndeces.Add(firstNormal);
                fileIndeces.Add(secondPosition);
                fileIndeces.Add(secondTextureCoordinates);
                fileIndeces.Add(secondNormal);
                // Now include the new vertex
                string[] indexReferences = parameters[i].Split('/');
                if (indexReferences[0] == "")
                  throw new StaticModelManagerException("ERROR: obj file corrupted (missing vertex possition): " + filePath);
                fileIndeces.Add(int.Parse(indexReferences[0], CultureInfo.InvariantCulture));
                if (indexReferences[1] != "")
                  fileIndeces.Add(int.Parse(indexReferences[1], CultureInfo.InvariantCulture));
                else
                  fileIndeces.Add(0);
                if (indexReferences[2] != "")
                  fileIndeces.Add(int.Parse(indexReferences[2], CultureInfo.InvariantCulture));
                else
                  fileIndeces.Add(0);
              }
              break;

              //// OLD VERSION OF THE FACE PARSING
              //// NOTE! This does not yet triangulate faces
              //// NOTE! This needs all possible values (position, texture mapping, and normal).
              //for (int i = 1; i < parameters.Length; i++)
              //{
              //  string[] indexReferences = parameters[i].Split('/');
              //  fileIndeces.Add(int.Parse(indexReferences[0], CultureInfo.InvariantCulture));
              //  if (indexReferences[1] != "")
              //    fileIndeces.Add(int.Parse(indexReferences[1], CultureInfo.InvariantCulture));
              //  else
              //    fileIndeces.Add(0);
              //  if (indexReferences[2] != "")
              //    fileIndeces.Add(int.Parse(indexReferences[2], CultureInfo.InvariantCulture));
              //  else
              //    fileIndeces.Add(0);
              //}
              //break;

            // End Current Mesh
            case "7":
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
                  throw new StaticModelManagerException("Vertex array not uploaded correctly");
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
                  throw new StaticModelManagerException("TexCoord array not uploaded correctly");
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
                  throw new StaticModelManagerException("Normal array not uploaded correctly");
                // Deselect the new buffer
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
              }
              else { normalBufferId = 0; }

              meshes.Add(
                  new StaticMesh(
                  meshName,
                  staticModelId + "sub" + meshes.Count,
                  vertexBufferId,
                  0, // Obj files don't support vertex colors
                  textureCoordinateBufferId,
                  normalBufferId,
                  0, // I don't support an index buffer at this time
                  verteces.Length,
                  texture));
              fileVerteces.Clear();
              fileNormals.Clear();
              fileTextureCoordinates.Clear();
              fileIndeces.Clear();
              texture = null;
              break;
          }
        }
      }
      return new StaticModel(staticModelId, meshes);
    }

    #endregion

    /// <summary>This is used for throwing AVL Tree exceptions only to make debugging faster.</summary>
    private class StaticModelManagerException : Exception { public StaticModelManagerException(string message) : base(message) { } }
  }
}