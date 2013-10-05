// THIS STATE IS USED AS AN EXAMPLE OF OBJ FILE IMPORTING.
// It uses the rigidbody class, which I no longer use in the engine.
// The class is still located in the engine, but you will have to
// uncomment the class to use this state.

/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.Drawing;
using System.Drawing.Imaging;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Engine;
using Engine.Models;

namespace Game.States
{
  public class objLoaderstate2 : IGameObject
  {
    RigidBodyPartModel _subModel;

    string vShaderSource = 
@"void main() {
	gl_Position = ftransform();
	gl_TexCoord[0] = gl_MultiTexCoord0;
}
";

    string fShaderSource = 
@"uniform sampler2D tex;
void main() {
	gl_FragColor = texture2D(tex, gl_TexCoord[0].st);
}
";

    string vShaderSource2 =
@"varying vec3 normal;

void main()
{
	normal = gl_NormalMatrix * gl_Normal;
	gl_Position = ftransform();

}";

    string fShaderSource2 = 
@"uniform vec3 lightDir;
varying vec3 normal;

void main()
{
	float intensity;
	vec4 color;
	intensity = dot(lightDir,normalize(normal));

	if (intensity > 0.95)
		color = vec4(1.0,0.5,0.5,1.0);
	else if (intensity > 0.5)
		color = vec4(0.6,0.3,0.3,1.0);
	else if (intensity > 0.25)
		color = vec4(0.4,0.2,0.2,1.0);
	else
		color = vec4(0.2,0.1,0.1,1.0);
	gl_FragColor = color;

}";

    public objLoaderstate2(TextureManager texturemanager, InputManager input)
    {
      _subModel = LoadObj(texturemanager, "grass.obj");
      _subModel.Scale = new Vector3d(20, 20, 20);
    }

    public void Update(double elapsedTime)
    {
      _subModel.temp++;
    }

    public void Render()
    {
      _subModel.Render();
    }

    int CompileShaders()
    {
      int programHandle, vHandle, fHandle;
      vHandle = GL.CreateShader(ShaderType.VertexShader);
      fHandle = GL.CreateShader(ShaderType.FragmentShader);
      GL.ShaderSource(vHandle, vShaderSource);
      GL.ShaderSource(fHandle, fShaderSource);
      GL.CompileShader(vHandle);
      GL.CompileShader(fHandle);
      Console.Write(GL.GetShaderInfoLog(vHandle));
      Console.Write(GL.GetShaderInfoLog(fHandle));

      programHandle = GL.CreateProgram();
      GL.AttachShader(programHandle, vHandle);
      GL.AttachShader(programHandle, fHandle);
      GL.LinkProgram(programHandle);
      Console.Write(GL.GetProgramInfoLog(programHandle));
      return programHandle;
    }

    public RigidBodyPartModel LoadObj(TextureManager texturemanager, string path)
    {
      GL.UseProgram(CompileShaders());

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
              float x = float.Parse(parameters[1], CultureInfo.InvariantCulture.NumberFormat);
              float y = float.Parse(parameters[2], CultureInfo.InvariantCulture.NumberFormat);
              float z = float.Parse(parameters[3], CultureInfo.InvariantCulture.NumberFormat);
              points.Add(new Vector3(x, y, z));
              break;

            case "vt":
              // TexCoord
              float u = float.Parse(parameters[1], CultureInfo.InvariantCulture.NumberFormat);
              float v = float.Parse(parameters[2], CultureInfo.InvariantCulture.NumberFormat);
              texCoords.Add(new Vector2(u, v));
              break;

            case "vn":
              // Normal
              float nx = float.Parse(parameters[1], CultureInfo.InvariantCulture.NumberFormat);
              float ny = float.Parse(parameters[2], CultureInfo.InvariantCulture.NumberFormat);
              float nz = float.Parse(parameters[3], CultureInfo.InvariantCulture.NumberFormat);
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
      
      return new RigidBodyPartModel(texturemanager, "grass", p2, n2, tc2, null, null);// f2);
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
}*/