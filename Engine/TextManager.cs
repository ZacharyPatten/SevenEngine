// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use with the following condition: any code using or originally taken 
// from the SevenEngine project must include citation to its original author(s) located at the
// top of each source code file, or you may include a reference to the SevenEngine project as
// a whole but you must include the current SevenEngine official website URL and logo.
// - Thanks.  :)  (support: seven@sevenengine.com)

// Author(s):
// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com
// Last Edited: 11-13-13

using System;
using System.IO;
using System.Globalization;
using SevenEngine.Texts;
using SevenEngine.DataStructures;
using SevenEngine.DataStructures.Interfaces;

namespace SevenEngine
{
  public static class TextManager
  {
    private static AvlTree<Font> _fontDatabase = new AvlTree<Font>();
    
    public static void LoadFontFile(string id, string filePath, string textureLocations)
    {
      ListArray<CharacterSprite> characters = new ListArray<CharacterSprite>(100);
      ListArray<string> textures = new ListArray<string>(1);
      using (StreamReader reader = new StreamReader(filePath))
      {
        while (!reader.EndOfStream)
        {
          string line = reader.ReadLine();
          string[] parameters = line.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

          switch (parameters[0])
          {
            case "page":
              string[] textureFile = parameters[2].Split("\"".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
              TextureManager.LoadTexture(textureFile[1], textureLocations + textureFile[1]);
              textures.Add(textureFile[1]);
              break;
            case "char":
              characters.Add(new CharacterSprite(
                // Texture
                TextureManager.Get(textures[int.Parse(parameters[9].Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1], CultureInfo.InvariantCulture)]),
                // Id
                int.Parse(parameters[1].Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1], CultureInfo.InvariantCulture),
                // X Advance
                int.Parse(parameters[8].Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1], CultureInfo.InvariantCulture),
                // X
                int.Parse(parameters[2].Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1], CultureInfo.InvariantCulture),
                // Y
                int.Parse(parameters[3].Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1], CultureInfo.InvariantCulture),
                // Width
                int.Parse(parameters[4].Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1], CultureInfo.InvariantCulture),
                // Height
                int.Parse(parameters[5].Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1], CultureInfo.InvariantCulture),
                // X Offset
                int.Parse(parameters[6].Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1], CultureInfo.InvariantCulture),
                // Y Offset
                int.Parse(parameters[7].Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1], CultureInfo.InvariantCulture)));
                break;
            case "kerning":
              int first = int.Parse(parameters[1].Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1], CultureInfo.InvariantCulture);
              for (int i = 0; i < characters.Count; i++)
                if (characters[i].Id == first)
                  characters[i].AddKearning(
                    int.Parse(parameters[2].Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1], CultureInfo.InvariantCulture),
                    int.Parse(parameters[3].Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1], CultureInfo.InvariantCulture));
                break;
          }
        }
      }
      _fontDatabase.Add(new Font(id, characters));
    }

    public static Font GetFont(string font)
    {
      Font fontToGet = _fontDatabase.Get(font);
      return fontToGet;
    }
  }
}
