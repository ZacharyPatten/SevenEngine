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
using SevenEngine.Mathematics;
using SevenEngine.DataStructures;

namespace SevenEngine.Texts
{
  /// <summary>Represents a message to be rendered, along with its font, color, and transformational attributes.</summary>
  public class Text
  {
    Font _font;
    string _message;
    int _scale;
    Vector _position;
    Quaternion _orientation;
    Color _color;

    





  //  Font _font;
  //  ListArray<CharacterSprite> _bitmapText = new ListArray<CharacterSprite>(10);
  //  string _text;
  //  Color _color = new Color(1, 1, 1, 1);
  //  Vector _dimensions;
  //  int _maxWidth = -1;

  //  public float Width
  //  {
  //    get { return _dimensions.X; }
  //  }

  //  public float Height
  //  {
  //    get { return _dimensions.Y; }
  //  }

  //  public ListArray<CharacterSprite> CharacterSprites
  //  {
  //    get { return _bitmapText; }
  //  }

  //  public Text(string text, Font font) : this(text, font, -1) { }
  //  public Text(string text, Font font, int maxWidth)
  //  {
  //    _text = text;
  //    _font = font;
  //    _maxWidth = maxWidth;
  //    CreateText(0, 0, _maxWidth);
  //  }

  //  private void CreateText(float x, float y)
  //  {
  //    CreateText(x, y, _maxWidth);
  //  }

  //  private void CreateText(float x, float y, float maxWidth)
  //  {
  //    _bitmapText.Clear();
  //    float currentX = 0;
  //    float currentY = 0;

  //    string[] words = _text.Split(' ');

  //    foreach (string word in words)
  //    {
  //      Vector nextWordLength = _font.MeasureFont(word);

  //      if (maxWidth != -1 &&
  //          (currentX + nextWordLength.X) > maxWidth)
  //      {
  //        currentX = 0;
  //        currentY += nextWordLength.Y;
  //      }

  //      string wordWithSpace = word + " "; // add the space character that was removed.

  //      foreach (char c in wordWithSpace)
  //      {
  //        CharacterSprite sprite = _font.CreateSprite(c);
  //        float xOffset = ((float)sprite.Data.XOffset) / 2;
  //        float yOffset = (((float)sprite.Data.Height) * 0.5f) + ((float)sprite.Data.YOffset);
  //        // sprite.Sprite.Center = new Vector(x + currentX + xOffset, y - currentY - yOffset, 0);
  //        sprite.Sprite.Position = new Point(x + currentX + xOffset, y - currentY - yOffset);
  //        currentX += sprite.Data.XAdvance;
  //        _bitmapText.Add(sprite);
  //      }
  //    }
  //    _dimensions = _font.MeasureFont(_text, _maxWidth);
  //    _dimensions.Y = currentY;
  //    SetColor(_color);
  //  }

  //  public void SetColor()
  //  {
  //    //for (int i = 0; i < _bitmapText.Count; i++)
  //    //  _bitmapText[i].Sprite.SetColor(_color);
  //    throw new Exception("I need this off for now.");
  //  }

  //  public void SetColor(Color color)
  //  {
  //    _color = color;
  //    SetColor();
  //  }

  //  public void SetPosition(float x, float y)
  //  {
  //    CreateText(x, y);
  //  }
  }
}