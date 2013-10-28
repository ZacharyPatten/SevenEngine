// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use with the following condition: any code using or originally taken 
// from the SevenEngine project must include citation to its original author(s) located at the
// top of each source code file, or you may include a reference to the SevenEngine project as
// a whole but you must include the current SevenEngine official website URL and logo.
// - Thanks.  :)  (support: seven@sevenengine.com)

// Author(s):
// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com
// Last Edited: 10-26-13

using System;

using SevenEngine.DataStructures;
using SevenEngine.Imaging;

namespace SevenEngine
{
  public class SpriteManager
  {
    private static AvlTree<Sprite> _spriteDatabase = new AvlTree<Sprite>();

    /// <summary>Gets a sprite by the id and tracks the number of hardware instancings.</summary>
    /// <param name="spriteId">The id of the desired sprite.</param>
    /// <returns>A clone of the desired sprite.</returns>
    public Sprite Get(string spriteId)
    {
      Sprite sprite = _spriteDatabase.Get(spriteId);
      sprite.Texture.ExistingReferences++;
      return new Sprite(sprite.Texture);
    }


  }
}