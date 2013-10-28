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

using SevenEngine.Imaging;

namespace SevenEngine.Texts
{
  public class CharacterSprite
  {
    public Sprite Sprite { get; set; }
    public CharacterData Data { get; set; }

    public CharacterSprite(Sprite sprite, CharacterData data)
    {
      Data = data;
      Sprite = sprite;
    }
  }
}