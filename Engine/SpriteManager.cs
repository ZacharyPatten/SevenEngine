using System;

using SevenEngine.DataStructures;
using SevenEngine.Imaging;

namespace SevenEngine
{
  public class SpriteManager
  {
    private static AvlTree<Sprite> _textureDatabase = new AvlTree<Sprite>();

    /// <summary>Gets a sprite by the id and tracks the number of hardware instancings.</summary>
    /// <param name="spriteId">The id of the desired sprite.</param>
    /// <returns>A clone of the desired sprite.</returns>
    public Sprite Get(string spriteId)
    {
      Sprite sprite = _textureDatabase.Get(spriteId);
      sprite.Texture.ExistingReferences++;
      return new Sprite(sprite.Texture);
    }


  }
}
