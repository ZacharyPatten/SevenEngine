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
using Seven.Mathematics;

namespace SevenEngine.Physics.Primitives
{
  public class RectangularPrism
  {
    private float _width, _height;
    private Vector<float> _position;
    private Quaternion<float> _orientation;

    public RectangularPrism(float width, float height, float x, float y, float z)
    {
      _width = width;
      _height = height;
      _position = new Vector<float>(x, y, z);
    }

    public RectangularPrism(float width, float height, Vector<float> position)
    {
      if (position.Dimensions != 3)
        throw new Exception();
      _width = width;
      _height = height;
      _position = position;
    }
  }
}
