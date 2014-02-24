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
using SevenEngine.Imaging;
using SevenEngine.DataStructures;

namespace SevenEngine.Texts
{
  public class Font
  {
    private string _id;
    private int _lineHeight;
    private int _base;
    private ListArray<CharacterSprite> _characterDatum;
    private int _existingHardwareInstances;

    public string Id { get { return _id; } set { _id = value; } }
    public int ExistingHardwareInstances { get { return _existingHardwareInstances; } }
    internal int LineHeight { get { return _lineHeight; } }
    internal int Base { get { return _base; } }

    internal Font(string id, int lineHeight, int fontBase, ListArray<CharacterSprite> characters)
    {
      _id = id;
      _lineHeight = lineHeight;
      _base = fontBase;
      _characterDatum = characters;
    }

    internal CharacterSprite Get(int id)
    {
      for (int i = 0; i < _characterDatum.Count; i++)
        if (_characterDatum[i].Id == id)
          return _characterDatum[i];
      throw new Exception("Character not found");
    }

    public static int CompareTo(Font left, Font right) { return left.Id.CompareTo(right.Id); }
    public static int CompareTo(Font left, string right) { return left.Id.CompareTo(right); }
  }
}