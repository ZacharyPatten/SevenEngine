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
using SevenEngine.Texts;
using SevenEngine.DataStructures;

namespace SevenEngine
{
  /// <summary>TextManager is used for fonts management (loading, storing, hardware instance controling, and disposing). </summary>
  public static class TextManager
  {
    private static AvlTree<Font> _fontDatabase = new AvlTree<Font>();

    /// <summary>The number of fonts currently loaded onto the graphics card.</summary>
    public static int Count { get { return _fontDatabase.Count; } }

    /// <summary>Pulls out a font from the database.</summary>
    /// <param name="font">The name associated with this font when you loaded it in.</param>
    /// <returns>The font associated with the string id.</returns>
    public static Font GetFont(string font)
    {
      try { Font fontToGet = _fontDatabase.Get(font); return fontToGet; }
      catch { throw new TextManagerException("The requested font does not exists."); }
    }

    #region Parsers

    public static void LoadFontFile(string id, string filePath, string textureLocations)
    {
      ListArray<CharacterSprite> characters = new ListArray<CharacterSprite>(255);
      ListArray<string> textures = new ListArray<string>(1);
      int lineHeight = -1, fontBase = int.MinValue, widthScale = int.MinValue, heightScale = int.MinValue;
      using (StreamReader reader = new StreamReader(filePath))
      {
        int lineNumber = 1;
        while (!reader.EndOfStream)
        {
          string line = reader.ReadLine();
          string[] parameters = line.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
          switch (parameters[0].ToLower())
          {
            case "common":
              for (int i = 1; i < parameters.Length; i++)
              {
                string[] attributes = parameters[i].Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                switch (attributes[0].ToLower())
                {
                  case "lineheight":
                    if (!int.TryParse(attributes[1], out lineHeight))
                      throw new TextManagerException("The line height value of the font file is corrupt on line " + lineNumber + ".");
                    break;

                  case "base":
                    if (!int.TryParse(attributes[1], out fontBase))
                      throw new TextManagerException("The base value of the font file is corrupt on line " + lineNumber + ".");
                    break;
                }
              }
              break;

            case "page":
              string[] textureFile = parameters[2].Split("\"".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
              try { TextureManager.LoadTexture(textureFile[1], textureLocations + textureFile[1]); }
              catch { throw new TextManagerException("The font file is using a non-supported image file on line " + lineNumber + "."); }
              textures.Add(textureFile[1]);
              break;

            case "char":
              int charId = -1,
                x = -1, y = -1,
                width = -1, height = -1,
                xOffset = int.MinValue, yOffset = int.MinValue,
                xAdvance = -1,
                page = -1;
              // channel=-1;

              // Lets get all the attributes of the character
              for (int i = 1; i < parameters.Length; i++)
              {
                string[] attribute = parameters[i].Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (attribute.Length < 2)
                  throw new TextManagerException("Font file has a corrupted character attribute on line " + lineNumber + ".");
                switch (attribute[0].ToLower())
                {
                  case "id":
                    if (!int.TryParse(attribute[1], out charId))
                      throw new TextManagerException("An id value in font file is corrupted on line " + lineNumber + ".");
                    break;
                  case "x":
                    if (!int.TryParse(attribute[1], out x))
                      throw new TextManagerException("A x value in font file is corrupted on line " + lineNumber + ".");
                    break;
                  case "y":
                    if (!int.TryParse(attribute[1], out y))
                      throw new TextManagerException("A y value in font file is corrupted on line " + lineNumber + ".");
                    break;
                  case "width":
                    if (!int.TryParse(attribute[1], out width))
                      throw new TextManagerException("A width value in font file is corrupted on line " + lineNumber + ".");
                    break;
                  case "height":
                    if (!int.TryParse(attribute[1], out height))
                      throw new TextManagerException("A height value in font file is corrupted on line " + lineNumber + ".");
                    break;
                  case "xoffset":
                    if (!int.TryParse(attribute[1], out xOffset))
                      throw new TextManagerException("A xoffset value in font file is corrupted on line " + lineNumber + ".");
                    break;
                  case "yoffset":
                    if (!int.TryParse(attribute[1], out yOffset))
                      throw new TextManagerException("A yoffset value in font file is corrupted on line " + lineNumber + ".");
                    break;
                  case "xadvance":
                    if (!int.TryParse(attribute[1], out xAdvance))
                      throw new TextManagerException("A xadvance value in font file is corrupted on line " + lineNumber + ".");
                    break;
                  case "page":
                    if (!int.TryParse(attribute[1], out page))
                      throw new TextManagerException("A page value in font file is corrupted on line " + lineNumber + ".");
                    break;
                  //// This check is most likely unnecessary, an error will be thrown during image loading attempt.
                  //case "chnl":
                  //  if (!int.TryParse(attribute[1], out channel))
                  //    throw new TextManagerException("A chnl value in font file is corrupted on line " + lineNumber + ".");
                  //  if (channel != 15)
                  //    throw new TextManagerException("The font file is using a non-supported image file.");
                }
              }

              // Make sure all the necessary values were imported and are valid
              if (charId == -1)
                throw new TextManagerException("Font file is corrupt/missing on a char id on line " + lineNumber + ".");
              if (x < 0)
                throw new TextManagerException("Font file has corrupt/missing on x value on line " + lineNumber + ".");
              if (y < 0)
                throw new TextManagerException("Font file has a corrupt/missing y value on line " + lineNumber + ".");
              if (width < 0)
                throw new TextManagerException("Font file has a corrupt/missing width value on line " + lineNumber + ".");
              if (height == -1)
                throw new TextManagerException("Font file has a corrupt/missing height value on line " + lineNumber + ".");
              if (xOffset == int.MinValue)
                throw new TextManagerException("Font file is missing a xoffset value on line " + lineNumber + ".");
              if (yOffset == int.MinValue)
                throw new TextManagerException("Font file is missing a yoffset value on line " + lineNumber + ".");
              if (xAdvance < 0)
                throw new TextManagerException("Font file has a corrupt/missing xadvance value on line " + lineNumber + ".");
              if (page < 0 || page > textures.Count)
                throw new TextManagerException("Font file has a corrupt/missing page value on line " + lineNumber + ".");
              //// This check is most likely unnecessary, an error will be thrown during image loading attempt.
              //if (channel == -1)
              //  throw new TextManagerException("Font file is missing a channel value on line " + lineNumber + ".");

              characters.Add(
                new CharacterSprite(
                  TextureManager.Get(textures[page]),
                  charId,
                  xAdvance,
                  x, y,
                  width, height,
                  xOffset, yOffset));
              break;

            #region OLD CODE (I'll delete this when I'm done debugging the newer version)
            // THIS WAS MY INITIAL PARSER JUST TO GET THINGS WORKING
            //  characters.Add(new CharacterSprite(
            //    // Texture
            //    TextureManager.Get(textures[int.Parse(parameters[9].Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1], CultureInfo.InvariantCulture)]),
            //    // Id
            //    int.Parse(parameters[1].Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1], CultureInfo.InvariantCulture),
            //    // X Advance
            //    int.Parse(parameters[8].Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1], CultureInfo.InvariantCulture),
            //    // X
            //    int.Parse(parameters[2].Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1], CultureInfo.InvariantCulture),
            //    // Y
            //    int.Parse(parameters[3].Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1], CultureInfo.InvariantCulture),
            //    // Width
            //    int.Parse(parameters[4].Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1], CultureInfo.InvariantCulture),
            //    // Height
            //    int.Parse(parameters[5].Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1], CultureInfo.InvariantCulture),
            //    // X Offset
            //    int.Parse(parameters[6].Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1], CultureInfo.InvariantCulture),
            //    // Y Offset
            //    int.Parse(parameters[7].Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1], CultureInfo.InvariantCulture)));
            //  break;
            //case "kerning":
            //  int first = int.Parse(parameters[1].Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1], CultureInfo.InvariantCulture);
            //  for (int i = 0; i < characters.Count; i++)
            //    if (characters[i].Id == first)
            //      characters[i].AddKearning(
            //        int.Parse(parameters[2].Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1], CultureInfo.InvariantCulture),
            //        int.Parse(parameters[3].Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1], CultureInfo.InvariantCulture));
            //    break;
            #endregion
          }
          lineNumber++;
        }
      }
      if (lineHeight < 0)
        throw new TextManagerException("Font file has a corrupt/missing line height value.");
      _fontDatabase.Add(new Font(id, lineHeight, fontBase, characters));
      string[] pathSplit = filePath.Split('\\');
      Output.WriteLine("Font file loaded: \"" + pathSplit[pathSplit.Length - 1] + "\".");
    }

    #endregion

    /// <summary>This is used for throwing AVL Tree exceptions only to make debugging faster.</summary>
    private class TextManagerException : Exception { public TextManagerException(string message) : base(message) { } }
  }
}
