using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine.Models
{
  public class RigidBodyModel
  {
    /*List<RigidBodyPartModel> _parts;

    public RigidBodyModel(TextureManager textureManager, string path)
    {
      using (StreamReader reader = new StreamReader(path))
      {
        List<float> points = new List<float>();
        List<float> normals = new List<float>();
        List<float> texCoords = new List<float>();
        List<int> indeces = new List<int>();

        string line;
        char[] splitChars = { ' ' };

        while (reader.EndOfStream == false)
        {
          try
          {
            line = reader.ReadLine().Trim();

            string[] parameters = line.Split(splitChars);

            switch (parameters[0])
            {
              case "p": // Point
                break;

              case "v": // Vertex
                points.Add(float.Parse(parameters[1]));
                points.Add(float.Parse(parameters[2]));
                points.Add(float.Parse(parameters[3]));
                break;

              case "vt": // TexCoord
                texCoords.Add(float.Parse(parameters[1]));
                texCoords.Add(float.Parse(parameters[2]));
                break;

              case "vn": // Normal
                normals.Add(float.Parse(parameters[1]));
                normals.Add(float.Parse(parameters[2]));
                normals.Add(float.Parse(parameters[3]));
                break;

              case "f": // Face
                indeces;
                break;
            }
          }
          catch (Exception e)
          {
            throw new Exception("obj file is probably corrupted");
          }
        }
      }
      Vector3[] p = points.ToArray();
      Vector2[] tc = texCoords.ToArray();
      Vector3[] n = normals.ToArray();
      Tri[] f = tris.ToArray();
      
      return new SubModel(p, n, tc, f);
    }*/
  }
}
