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
using SevenEngine.Imaging;
using SevenEngine.DataStructures;
using SevenEngine.DataStructures.Interfaces;

namespace SevenEngine.Texts
{
  public class Font : InterfaceStringId
  {
    private string _id;
    private ListArray<CharacterSprite> _characterDatum;
    private int _existingHardwareInstances;

    public string Id { get { return _id; } set { _id = value; } }

    public Font(string id, ListArray<CharacterSprite> characters)
    {
      _id = id;
      _characterDatum = characters;
    }

    public CharacterSprite Get(int id)
    {
      for (int i = 0; i < _characterDatum.Count; i++)
        if (_characterDatum[i].Id == id)
          return _characterDatum[i];
      throw new Exception("Character not found");
    }
  }
}