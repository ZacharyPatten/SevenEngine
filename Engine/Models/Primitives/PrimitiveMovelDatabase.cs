namespace SevenEngine.Models.Primitives
{
  public class Cylinder
  {
    float _x, _y, _z, _radius, _scale;

    public float X { get { return _x; } set { _x = value; } }
    public float Y { get { return _y; } set { _y = value; } }
    public float Z { get { return _z; } set { _z = value; } }
    public float Radius { get { return _radius; } set { _radius = value; } }

    public float MinimumX { get { return _x - _radius; } }
    public float MaximumX { get { return _x + _radius; } }
    public float MinimumY { get { return _y - _radius; } }
    public float MaximumY { get { return _y + _radius; } }
    public float MinimumZ { get { return _z - _radius; } }
    public float MaximumZ { get { return _z + _radius; } }

    public Cylinder(float x, float y, float z, float radius)
    { _x = x; _y = y; _z = z; _radius = radius; }
  }

  public class Sphere
  {
    float _x, _y, _z, _radius;

    public float X { get { return _x; } set { _x = value; } }
    public float Y { get { return _y; } set { _y = value; } }
    public float Z { get { return _z; } set { _z = value; } }
    public float Radius { get { return _radius; } set { _radius = value; } }

    public float MinimumX { get { return _x - _radius; } }
    public float MaximumX { get { return _x + _radius; } }
    public float MinimumY { get { return _y - _radius; } }
    public float MaximumY { get { return _y + _radius; } }
    public float MinimumZ { get { return _z - _radius; } }
    public float MaximumZ { get { return _z + _radius; } }

    public Sphere(float x, float y, float z, float radius)
    { _x = x; _y = y; _z = z; _radius = radius; }
  }

  public class Cube
  {
    float _x, _y, _z, _scale;

    public float X { get { return _x; } set { _x = value; } }
    public float Y { get { return _y; } set { _y = value; } }
    public float Z { get { return _z; } set { _z = value; } }
    public float Scale { get { return _scale; } set { _scale = value; } }

    public float MinimumX { get { return _x - _scale; } }
    public float MaximumX { get { return _x + _scale; } }
    public float MinimumY { get { return _y - _scale; } }
    public float MaximumY { get { return _y + _scale; } }
    public float MinimumZ { get { return _z - _scale; } }
    public float MaximumZ { get { return _z + _scale; } }

    public Cube(float x, float y, float z, float scale)
    { _x = x; _y = y; _z = z; _scale = scale; }
  }
}