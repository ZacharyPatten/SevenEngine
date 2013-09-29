using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Engine;
using Engine.Models;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game.States
{
  public class objectImporterState : IGameObject
  {
    Renderer _renderer;
    RigidBodyPartModel _subModel;
    //InputManager _input;

    public objectImporterState(TextureManager textureManager) //, InputManager input)
    {
      List<float> points = new List<float>();
      List<float> normals = new List<float>();
      List<float> texCoords = new List<float>();
      List<int[]> indeces = new List<int[]>();

      List<float> points2 = new List<float>();
      List<float> normals2 = new List<float>();
      List<float> texCoords2 = new List<float>();
      List<int> indeces2 = new List<int>();

      using (StreamReader reader = new StreamReader("yoda.obj"))
      {
        _renderer = new Renderer();
        //_input = input;

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
                indeces.Add(new int[9] {
                  int.Parse(parameters[1]),
                  int.Parse(parameters[2]),
                  int.Parse(parameters[3]),
                  int.Parse(parameters[4]),
                  int.Parse(parameters[5]),
                  int.Parse(parameters[6]),
                  int.Parse(parameters[7]),
                  int.Parse(parameters[8]),
                  int.Parse(parameters[9])});
                break;
            }
          }
          catch (Exception e)
          {
            throw new Exception("obj file is probably corrupted");
          }
        }
      }

      //points2.AddRange(points);
      //texCoords2.AddRange(texCoords);
      //normals2.AddRange(normals);

      foreach (int[] references in indeces)
      {
        int index = (references[0] - 1) * 3;
        points2.Add(points[index]);
        points2.Add(points[index + 1]);
        points2.Add(points[index + 2]);

        int index2 = (references[1] - 1) * 2;
        texCoords2.Add(texCoords[index2]);
        texCoords2.Add(texCoords[index2 + 1]);

        int index3 = (references[2] - 1) * 3;
        normals2.Add(normals[index3]);
        normals2.Add(normals[index3 + 1]);
        normals2.Add(normals[index3 + 2]);

        int index4 = (references[3] - 1) * 3;
        points2.Add(points[index4]);
        points2.Add(points[index4 + 1]);
        points2.Add(points[index4 + 2]);

        int index5 = (references[4] - 1) * 2;
        texCoords2.Add(texCoords[index5]);
        texCoords2.Add(texCoords[index5 + 1]);

        int index6 = (references[5] - 1) * 3;
        normals2.Add(normals[index6]);
        normals2.Add(normals[index6 + 1]);
        normals2.Add(normals[index6 + 2]);

        int index7 = (references[6] - 1) * 3;
        points2.Add(points[index7]);
        points2.Add(points[index7 + 1]);
        points2.Add(points[index7 + 2]);

        int index8 = (references[7] - 1) * 2;
        texCoords2.Add(texCoords[index8]);
        texCoords2.Add(texCoords[index8 + 1]);

        int index9 = (references[8] - 1) * 3;
        normals2.Add(normals[index9]);
        normals2.Add(normals[index9 + 1]);
        normals2.Add(normals[index9 + 2]);
      }

      for (int i = 0; i < indeces.Count; i++)
      {
        indeces2.Add(i);
      }

      _subModel = new RigidBodyPartModel(
        textureManager,
        "yoda",
        points.ToArray(),
        normals.ToArray(),
        texCoords.ToArray(),
        null,
        null);
        //indeces2.ToArray());


      _subModel.Scale = new Vector3d(200, 200, 200);
    }

    public void Update(double elapsedTime)
    {
      _subModel.temp++;
    }

    public void Render()
    {
      _subModel.Render();
    }
  }
}