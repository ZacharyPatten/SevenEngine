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

namespace SevenEngine.Physics.Primitives
{
  public class Cylinder
  {
    float _x, _y, _z, _radius, _scale;

    public float X { get { return _x; } set { _x = value; } }
    public float Y { get { return _y; } set { _y = value; } }
    public float Z { get { return _z; } set { _z = value; } }
    public float Radius { get { return _radius; } set { _radius = value; } }
    public float Scale { get { return _scale; } set { _scale = value; } }

    public float MinimumX { get { return _x - _radius; } }
    public float MaximumX { get { return _x + _radius; } }
    public float MinimumY { get { return _y - _radius; } }
    public float MaximumY { get { return _y + _radius; } }
    public float MinimumZ { get { return _z - _radius; } }
    public float MaximumZ { get { return _z + _radius; } }

    public Cylinder(float x, float y, float z, float radius)
    { _x = x; _y = y; _z = z; _radius = radius; }
  }
}
