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

namespace SevenEngine.Mathematics
{
  public class Point
  {
    private float _x;
    private float _y;

    public float X { get { return _x; } set { _x = value; } }
    public float Y { get { return _y; } set { _y = value; } }

    public Point(float x, float y)
    {
      _x = x;
      _y = y;
    }

    public Point Lerp(Point right, float blend)
    {
      if (blend < 0 || blend > 1.0f)
        throw new PointException("Attempting linear interpolation with invalid blend (blend < 0.0f || blend > 1.0f).");
      return new Point(
        _x + blend * (right.X - _x),
        _y + blend * (right.Y - _y));
    }

    /// <summary>This is used for throwing vector exceptions only to make debugging faster.</summary>
    private class PointException : Exception { public PointException(string message) : base(message) { } }
  }
}