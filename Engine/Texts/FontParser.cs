// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use with the following condition: any code using or originally taken from the 
// SevenEngine project must include citation to its original author(s) located at the top of each
// source code file. Alternatively, you may include a reference to the SevenEngine project as a whole,
// but you must include the current SevenEngine official website URL and logo.
// - Thanks.  :)  (support: seven@sevenengine.com)

// Author(s):
// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com
// Last Edited: 10-26-13

using System;
using System.IO;

using SevenEngine.DataStructures;

namespace SevenEngine.Texts
{
  ///// <summary></summary>
  //public class FontParser
  //{
  //  static int HeaderSize = 4;

  //  // Gets the value after an equal sign and converts it
  //  // from a string to an integer
  //  private static int GetValue(string s)
  //  {
  //    string value = s.Substring(s.IndexOf('=') + 1);
  //    return int.Parse(value);
  //  }

  //  public static AvlTree<CharacterSprite> Parse(string filePath)
  //  {
  //    AvlTree<CharacterSprite> charDictionary = new AvlTree<CharacterSprite>();

  //    string[] lines = File.ReadAllLines(filePath);

  //    for (int i = HeaderSize; i < lines.Length; i += 1)
  //    {
  //      string firstLine = lines[i];
  //      string[] typesAndValues = firstLine.Split(" ".ToCharArray(),
  //          StringSplitOptions.RemoveEmptyEntries);

  //      // All the data comes in a certain order,
  //      // used to make the parser shorter
  //      CharacterSprite charData = new CharacterSprite
  //      {
  //        Id = typesAndValues[1],
  //        X = GetValue(typesAndValues[2]),
  //        Y = GetValue(typesAndValues[3]),
  //        Width = GetValue(typesAndValues[4]),
  //        Height = GetValue(typesAndValues[5]),
  //        XOffset = GetValue(typesAndValues[6]),
  //        YOffset = GetValue(typesAndValues[7]),
  //        XAdvance = GetValue(typesAndValues[8])
  //      };
  //      charDictionary.Add(charData);
  //    }
  //    return charDictionary;
  //  }
  //}
}