/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using Engine.Models;

namespace Engine
{
  public class StaticModelManager : IDisposable
  {
    Dictionary<string, StaticModel> _staticModelDatabase = new Dictionary<string, StaticModel>();

    public StaticModel Get(string superModelId)
    {
      return _staticModelDatabase[superModelId];
    }

    public void LoadModel(TextureManager textureManager, string staticModelId, string path)
    {
      _staticModelDatabase.Add(staticModelId, LoadObj(textureManager, path));
      Output.Print("Model file loaded: \"" + path + "\".");
    }

    public void RemoveModel(string staticModelId)
    {
      // Get the struct with the GPU mappings.
      StaticModel removal = Get(staticModelId);
      // Delete the vertex buffer if it exists.
      int vertexBufferId = removal.VertexBufferId;
      if (vertexBufferId != 0)
        GL.DeleteBuffers(1, ref vertexBufferId);
      // Delete the normal buffer if it exists.
      int normalbufferId = removal.NormalBufferId;
      if (normalbufferId != 0)
        GL.DeleteBuffers(1, ref normalbufferId);
      // Delete the color buffer if it exists.
      int colorBufferId = removal.ColorBufferId;
      if (colorBufferId != 0)
        GL.DeleteBuffers(1, ref colorBufferId);
      // Delete the texture coordinate buffer if it exists.
      int textureCoordinateBufferId = removal.TexCoordBufferId;
      if (textureCoordinateBufferId != 0)
        GL.DeleteBuffers(1, ref textureCoordinateBufferId);
      // Delete the element buffer if it exists.
      int elementBufferId = removal.ElementBufferId;
      if (elementBufferId != 0)
        GL.DeleteBuffers(1, ref elementBufferId);
      // Now we can remove it from the dictionary.
      _staticModelDatabase.Remove(staticModelId);
    }

    public StaticModel LoadObj(TextureManager texturemanager, string path)
    {
      List<Vector3> points = new List<Vector3>();
      List<Vector3> normals = new List<Vector3>();
      List<Vector2> texCoords = new List<Vector2>();
      List<Tri> tris = new List<Tri>();

      using (StreamReader reader = new StreamReader(path))
      {
        string line;
        char[] splitChars = { ' ' };
        while ((line = reader.ReadLine()) != null)
        {
          line = line.Trim(splitChars);
          line = line.Replace("  ", " ");

          string[] parameters = line.Split(splitChars);

          switch (parameters[0])
          {
            case "p":
              // Point
              break;

            case "v":
              // Vertex
              float x = float.Parse(parameters[1]);
              float y = float.Parse(parameters[2]);
              float z = float.Parse(parameters[3]);
              points.Add(new Vector3(x, y, z));
              break;

            case "vt":
              // TexCoord
              float u = float.Parse(parameters[1]);
              float v = float.Parse(parameters[2]);
              texCoords.Add(new Vector2(u, v));
              break;

            case "vn":
              // Normal
              float nx = float.Parse(parameters[1]);
              float ny = float.Parse(parameters[2]);
              float nz = float.Parse(parameters[3]);
              normals.Add(new Vector3(nx, ny, nz));
              break;

            case "f":
              // Face
              tris.AddRange(parseFace(parameters));
              break;
          }
        }
      }

      Vector3[] p = points.ToArray();
      Vector2[] tc = texCoords.ToArray();
      Vector3[] n = normals.ToArray();
      Tri[] f = tris.ToArray();

      // If there are no specified texcoords or normals, we add a dummy one.
      // That way the Points will have something to refer to.
      if (tc.Length == 0)
      {
        tc = new Vector2[1];
        tc[0] = new Vector2(0, 0);
      }
      if (n.Length == 0)
      {
        n = new Vector3[1];
        n[0] = new Vector3(1, 0, 0);
      }

      float[] p2;
      float[] tc2;
      float[] n2;
      int[] f2;

      OpenGLArrays(p, n, tc, f, out p2, out n2, out tc2, out f2);

      bool something = false;
      for (int i = 0; i < tc2.Length; i++)
      {
        if (something)
        {
          tc2[i] = 1 - tc2[i];
          something = false;
        }
        else
        {
          something = true;
        }
      }

      RigidBodyPartModel model = new RigidBodyPartModel(texturemanager, "grass", p2, n2, tc2, null, null);// f2);

      return new StaticModel(model.VertexBufferID, model.ColorBufferID, model.TexCoordBufferID, model.NormalBufferID, model.ElementBufferID, model.Verteces.Length, model.Texture, model.Position, model.Scale, new Vector3d(0,0,0), 0);
    }

    protected Point2[] Points(Tri[] Tris)
    {
      List<Point2> points = new List<Point2>();
      foreach (Tri t in Tris)
      {
        points.Add(t.P1);
        points.Add(t.P2);
        points.Add(t.P3);
      }
      return points.ToArray();
    }

    public void OpenGLArrays(Vector3[] vertsPre, Vector3[] normsPre, Vector2[] texcoordsPre, Tri[] indicesPre, out float[] verts, out float[] norms, out float[] texcoords, out int[] indices)
    {
      Point2[] points = Points(indicesPre);
      verts = new float[points.Length * 3];
      norms = new float[points.Length * 3];
      texcoords = new float[points.Length * 2];
      indices = new int[points.Length];

      for (uint i = 0; i < points.Length; i++)
      {
        Point2 p = points[i];
        verts[i * 3] = (float)vertsPre[p.Vertex].X;
        verts[i * 3 + 1] = (float)vertsPre[p.Vertex].Y;
        verts[i * 3 + 2] = (float)vertsPre[p.Vertex].Z;

        norms[i * 3] = (float)normsPre[p.Normal].X;
        norms[i * 3 + 1] = (float)normsPre[p.Normal].Y;
        norms[i * 3 + 2] = (float)normsPre[p.Normal].Z;

        texcoords[i * 2] = (float)texcoordsPre[p.TexCoord].X;
        texcoords[i * 2 + 1] = (float)texcoordsPre[p.TexCoord].Y;

        indices[i] = (int)i;
      }
    }

    private static Tri[] parseFace(string[] indices)
    {
      Point2[] p = new Point2[indices.Length - 1];
      for (int i = 0; i < p.Length; i++)
      {
        p[i] = parsePoint(indices[i + 1]);
      }
      return Triangulate(p);
      //return new Face(p);
    }

    // Takes an array of points and returns an array of triangles.
    // The points form an arbitrary polygon.
    private static Tri[] Triangulate(Point2[] ps)
    {
      List<Tri> ts = new List<Tri>();
      if (ps.Length < 3)
      {
        throw new Exception("Invalid shape!  Must have >2 points");
      }

      Point2 lastButOne = ps[1];
      Point2 lastButTwo = ps[0];
      for (int i = 2; i < ps.Length; i++)
      {
        Tri t = new Tri(lastButTwo, lastButOne, ps[i]);
        lastButOne = ps[i];
        lastButTwo = ps[i - 1];
        ts.Add(t);
      }
      return ts.ToArray();
    }

    private static Point2 parsePoint(string s)
    {
      char[] splitChars = { '/' };
      string[] parameters = s.Split(splitChars);
      int vert, tex, norm;
      vert = tex = norm = 0;
      vert = int.Parse(parameters[0]) - 1;
      // Texcoords and normals are optional in .obj files
      if (parameters[1] != "")
      {
        tex = int.Parse(parameters[1]) - 1;
      }
      if (parameters[2] != "")
      {
        norm = int.Parse(parameters[2]) - 1;
      }
      return new Point2(vert, norm, tex);
    }

    #region IDisposable Members

    public void Dispose()
    {
      foreach (StaticModel t in _staticModelDatabase.Values)
      {
        throw new NotImplementedException();
      }
    }

    #endregion

  }

  public class Tri
  {
    public Point2 P1, P2, P3;
    public Tri()
    {
      P1 = new Point2();
      P2 = new Point2();
      P3 = new Point2();
    }
    public Tri(Point2 a, Point2 b, Point2 c)
    {
      P1 = a;
      P2 = b;
      P3 = c;
    }

    public Point2[] Points()
    {
      return new Point2[3] { P1, P2, P3 };
    }

    public override string ToString() { return String.Format("Tri: {0}, {1}, {2}", P1, P2, P3); }
  }

  public struct Point2
  {
    public int Vertex;
    public int Normal;
    public int TexCoord;

    public Point2(int v, int n, int t)
    {
      Vertex = v;
      Normal = n;
      TexCoord = t;
    }

    public override string ToString() { return String.Format("Point: {0},{1},{2}", Vertex, Normal, TexCoord); }

  }
}
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using Engine.Models;
using Engine.Textures;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine
{
  public class StaticModelManager : IDisposable
  {
    Dictionary<string, StaticMesh> _staticModelDatabase = new Dictionary<string, StaticMesh>();

    /// <summary>The number of meshes currently loaded onto the graphics card.</summary>
    public int Count { get { return _staticModelDatabase.Count; } }

    public StaticMesh Get(string superModelId)
    {
      StaticMesh mesh = _staticModelDatabase[superModelId];
      mesh.ExistingReferences++;
      return mesh;
    }

    /// <summary>Loads an 3d model file. NOTE taht only obj files are currently supported.</summary>
    /// <param name="textureManager">The texture manager so that the mesh can automatically texture itself.</param>
    /// <param name="staticModelId">The key used to look up this mesh in the database.</param>
    /// <param name="filePath">The filepath of the model file you are attempting to load.</param>
    public void LoadModel(TextureManager textureManager, string staticModelId, string filePath)
    {
      _staticModelDatabase.Add(staticModelId, LoadObj(textureManager, filePath));
      Output.Print("Model file loaded: \"" + filePath + "\".");
    }

    public void RemoveModel(string staticModelId)
    {
      // Get the struct with the GPU mappings.
      StaticMesh removal = Get(staticModelId);

      // If the game tries to remove a texture that still has active references then
        // lets warn them.
      if (removal.ExistingReferences > 1)
      {
        Output.Print("WARNING: texture removal \"" + staticModelId + "\" still has active references.");
      }

      // Delete the vertex buffer if it exists.
      int vertexBufferId = removal.VertexBufferId;
      if (vertexBufferId != 0)
        GL.DeleteBuffers(1, ref vertexBufferId);
      // Delete the normal buffer if it exists.
      int normalbufferId = removal.NormalBufferId;
      if (normalbufferId != 0)
        GL.DeleteBuffers(1, ref normalbufferId);
      // Delete the color buffer if it exists.
      int colorBufferId = removal.ColorBufferId;
      if (colorBufferId != 0)
        GL.DeleteBuffers(1, ref colorBufferId);
      // Delete the texture coordinate buffer if it exists.
      int textureCoordinateBufferId = removal.TextureCoordinateBufferId;
      if (textureCoordinateBufferId != 0)
        GL.DeleteBuffers(1, ref textureCoordinateBufferId);
      // Delete the element buffer if it exists.
      int elementBufferId = removal.ElementBufferId;
      if (elementBufferId != 0)
        GL.DeleteBuffers(1, ref elementBufferId);
      // Now we can remove it from the dictionary.
      _staticModelDatabase.Remove(staticModelId);
    }

    public StaticMesh LoadObj(TextureManager texturemanager, string filePath)
    {
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
              texturemanager.LoadTexture(parameters[1], parameters[1]);
              texture = texturemanager.Get(parameters[1]);
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
        vertexBufferId,
        0,
        textureCoordinateBufferId,
        normalBufferId,
        0,
        verteces.Length);
    }

    #region IDisposable Members

    public void Dispose()
    {
      foreach (StaticMesh t in _staticModelDatabase.Values)
      {
        throw new NotImplementedException();
      }
    }

    #endregion

  }
}