// Seven
// https://github.com/53V3N1X/SevenEngine
// LISCENSE: See "LISCENSE.txt" in th root project directory.
// SUPPORT: See "README.txt" in the root project directory.

using System;
using System.IO;
using System.Xml;

namespace SevenEngine.Importer
{
  public static class Collada
  {
    public static void ParseFile(string filePath)
    {
      using (XmlReader reader = XmlReader.Create(new StringReader(filePath))) {
        // Parse the file and display each of the nodes.
        while (reader.Read())
        {
          switch (reader.Name)
          {
            case "COLLADA":
              ParseCollada(reader);
              break;

            default:
              reader.Skip();
              break;
          }
        }
      }
    }

    /// <summary>Parses the "COLLADA" XML node.</summary>
    /// <param name='reader'></para>XML reader.</param>
    private static void ParseCollada (XmlReader reader)
    {
      while (reader.Read())
      {
        switch (reader.Name)
        {  
          case "library_animations":
            ParseAnimationLibrary(reader);
            break;

          case "library_images":
            ParseImageLibrary(reader);
            break;
            
          case "library_materials":
            ParseMaterialLibrary(reader);
            break;
            
          case "library_geometries":
            ParseGeometryLibrary(reader);
            break;
            
          default:
            reader.Skip ();
            break;
        }
      }
    }

    /// <summary>Parses the "library_animations" XML node.</summary>
    /// <param name='reader'>XML reader.</param>
    private static void ParseAnimationLibrary (XmlReader reader)
    {
      if (reader.IsEmptyElement)
        return;

      while(reader.Read())
      {
        switch (reader.Name)
        {
          case "library_geometries":
            ParseGeometryLibrary(reader);
            break;
            
          default:
            reader.Skip();
            break;
        }
      }
    }

    /// <summary>Parses the "library_images" XML node.</summary>
    /// <param name='reader'>XML reader.</param>
    private static void ParseImageLibrary(XmlReader reader)
    {
      throw new NotImplementedException();
    }

    /// <summary>Parses the "library_geometries" XML node.</summary>
    /// <param name='reader'>XML reader.</param>
    private static void ParseGeometryLibrary(XmlReader reader)
    {
      throw new NotImplementedException();

      if (reader.IsEmptyElement)
        return;

      while(reader.Read())
      {
        switch (reader.Name)
        {
            
          default:
            break;
        }
      }
    }

    /// <summary>Parses the "library_materials" XML node.</summary>
    /// <param name='reader'>XML reader.</param>
    private static void ParseMaterialLibrary(XmlReader reader)
    {
      throw new NotImplementedException();
    }
  }
}
