using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace Converter
{
  class Program
  {
    static void Main(string[] args)
    {
      float[] verteces, nomalsV, mappings;
      int[] indeces;
      load("foxhead.obj", out verteces, out nomalsV, out mappings, out indeces);
      export("foxhead.cs", verteces, nomalsV, mappings, indeces);
    }

    public static void export(string path, float[] verteces, float[] nomalsV, float[] mappings, int[] indeces)
    {
      using (StreamWriter writer = new StreamWriter(path))
      {
        writer.Write("float[] _verteces = new float[] { ");
        for (int i = 0; i < verteces.Length; i++)
          writer.Write(verteces[i] + "f, ");
        writer.Write("};\n");

        writer.Write("float[] _normals = new float[] { ");
        for (int i = 0; i < nomalsV.Length; i++)
          writer.Write(nomalsV[i] + "f, ");
        writer.Write("};\n");

        writer.Write("float[] _mappings = new float[] { ");
        for (int i = 0; i < nomalsV.Length; i++)
          writer.Write(nomalsV[i] + "f, ");
        writer.Write("};\n");

        //writer.Write("float[] _colors = new float[] { ");
        //for (int i = 0; i < nomalsV.Length; i++)
        //  writer.Write(nomalsV[i] + ", ");

        writer.Write("int[] _indeces = new int[] { ");
        for (int i = 0; i < indeces.Length; i++)
          writer.Write(indeces[i] + ", ");
        writer.Write("};\n");

      }

    }

    public static void load(string path, out float[] verteces, out float[] nomalsV, out float[] mappings, out int[] indeces)
    {
      List<float> points = new List<float>();
      List<float> normals = new List<float>();
      List<float> texCoords = new List<float>();
      List<int> faces = new List<int>();

      using (StreamReader reader = new StreamReader(path))
      {
        string line;
        char[] splitChars = { ' ' };

        while (reader.EndOfStream != true)
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
                points.Add(Convert.ToSingle(parameters[1]));
                points.Add(Convert.ToSingle(parameters[2]));
                points.Add(Convert.ToSingle(parameters[3]));
                break;

              case "vt": // TexCoord
                texCoords.Add(Convert.ToSingle(parameters[1]));
                texCoords.Add(Convert.ToSingle(parameters[2]));
                break;

              case "vn": // Normal
                normals.Add(Convert.ToSingle(parameters[1]));
                normals.Add(Convert.ToSingle(parameters[2]));
                normals.Add(Convert.ToSingle(parameters[3]));
                break;

              case "f": // Face
                //faces.Add(Convert.ToInt32(parameters[1]));
                //faces.Add(Convert.ToInt32(parameters[2]));
                //faces.Add(Convert.ToInt32(parameters[3]));
                string[] first = parameters[1].Split('/');
                faces.Add(Convert.ToInt32(first[0]));
                faces.Add(Convert.ToInt32(first[1]));
                faces.Add(Convert.ToInt32(first[2]));
                string[] second = parameters[2].Split('/');
                faces.Add(Convert.ToInt32(second[0]));
                faces.Add(Convert.ToInt32(second[1]));
                faces.Add(Convert.ToInt32(second[2]));
                string[] third = parameters[3].Split('/');
                faces.Add(Convert.ToInt32(third[0]));
                faces.Add(Convert.ToInt32(third[1]));
                faces.Add(Convert.ToInt32(third[2]));
                break;

              default:
                break;
            }
          }
          catch (Exception e)
          {
            throw new Exception("obj file is probably corrupted");
          }
        }
      }

      verteces = points.ToArray();
      nomalsV  = normals.ToArray();
      mappings = texCoords.ToArray();
      indeces = faces.ToArray();
    }

  }
}