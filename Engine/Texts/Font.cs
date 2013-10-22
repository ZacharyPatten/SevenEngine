using System;

using SevenEngine.Imaging;
using SevenEngine.Mathematics;
using SevenEngine.DataStructures;

namespace SevenEngine.Texts
{
  //public class Font
  //{
  //  Texture _texture;
  //  AvlTree<CharacterData> _characterData;

  //  public Font(Texture texture, AvlTree<CharacterData> characterData)
  //  {
  //    _texture = texture;
  //    _characterData = characterData;
  //  }

  //  public Vector MeasureFont(string text)
  //  {
  //    return MeasureFont(text, -1);
  //  }

  //  public Vector MeasureFont(string text, double maxWidth)
  //  {
  //    Vector dimensions = new Vector(0, 0, 0);

  //    foreach (char c in text)
  //    {
  //      CharacterData data = _characterData.Get(c.ToString());
  //      dimensions.X += data.XAdvance;
  //      dimensions.Y = Math.Max(dimensions.Y, data.Height + data.YOffset);
  //    }
  //    return dimensions;
  //  }

  //  public CharacterSprite CreateSprite(char c)
  //  {
  //    CharacterData charData = _characterData.Get(c.ToString());
  //    Sprite sprite = new Sprite(_texture);

  //    // Setup UVs
  //    Point topLeft = new Point((float)charData.X / (float)_texture.Width,
  //                                (float)charData.Y / (float)_texture.Height);
  //    Point bottomRight = new Point(topLeft.X + ((float)charData.Width / (float)_texture.Width),
  //                                  topLeft.Y + ((float)charData.Height / (float)_texture.Height));
  //    sprite.SetUVs(topLeft, bottomRight);
  //    sprite.Width = charData.Width;
  //    sprite.Height = charData.Height;
  //    sprite.SetColor(new Color(1, 1, 1, 1));

  //    return new CharacterSprite(sprite, charData);
  //  }
  //}
}