using System;

namespace SevenEngine.Mathematics
{
  public class Vector
  {
    private double _x, _y, _z;

    public double X { get { return _x; } set { _x = value; } }
    public double Y { get { return _y; } set { _y = value; } }
    public double Z { get { return _z; } set { _z = value; } }

    public Vector(double x, double y, double z) { _x = x; _y = y; _z = z; }

    public static double operator *(Vector vectorOne, Vector vectorTwo) { return vectorOne.DotProduct(vectorTwo); }
    public static Vector operator +(Vector v1, Vector v2) { return v1.Add(v2); }
    public static Vector operator -(Vector v1, Vector v2) { return v1.Subtract(v2); }
    public static Vector operator -(Vector v1) { return v1 * -1; }
    public static Vector operator *(Vector vector, double scalar) { return vector.Multiply(scalar); }
    public static bool operator !=(Vector v1, Vector v2) { return !v1.Equals(v2); }

    public double Length { get {return Math.Sqrt(LengthSquared); } }
    public double LengthSquared { get { return (X * X + Y * Y + Z * Z); } }

    public Vector Add(Vector r) { return new Vector(X + r.X, Y + r.Y, Z + r.Z); }
    public Vector Subtract(Vector r) { return new Vector(X - r.X, Y - r.Y, Z - r.Z); }
    public Vector Multiply(double v) { return new Vector(X * v, Y * v, Z * v); }
    public double DotProduct(Vector vector) { return (vector.X * X) + (Y * vector.Y) + (Z * vector.Z); }

    public bool Equals(Vector v) { return (X == v.X) && (Y == v.Y) && (Z == v.Z); }

    public override int GetHashCode() { return (int)X ^ (int)Y ^ (int)Z; }

    public static bool operator ==(Vector v1, Vector v2)
    {
      if (System.Object.ReferenceEquals(v1, v2))
        return true;
      if (v1 == null || v2 == null)
        return false;
      return v1.Equals(v2);
    }

    public override bool Equals(object obj)
    {
      if (obj is Vector)
        return Equals((Vector)obj);
      return base.Equals(obj);
    }

    public Vector CrossProduct(Vector vector)
    {
      double nx = Y * vector.Z - Z * vector.Y;
      double ny = Z * vector.X - X * vector.Z;
      double nz = X * vector.Y - Y * vector.X;
      return new Vector(nx, ny, nz);
    }

    public Vector Normalise()
    {
      double length = Length;
      if (length != 0.0)
        return new Vector(X / length, Y / length, Z / length);
      else
        return new Vector(0, 0, 0);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="angle">RADIANS!!!</param>
    /// <param name="axis"></param>
    /// <returns></returns>
    public Vector Rotate(double angle, Vector axis)
    {
      double sinHalfAngle = Math.Sin(angle / 2);
      double cosHalfAngle = Math.Cos(angle / 2);

      double rX = axis.X * sinHalfAngle;
      double rY = axis.Y * sinHalfAngle;
      double rZ = axis.Z * sinHalfAngle;
      double rW = cosHalfAngle;

      Quaternion rotation = new Quaternion(rX, rY, rZ, rW);
      Quaternion conjugate = rotation.Conjugate();

      Quaternion w = rotation.Multiply(this).Multiply(conjugate);

      return new Vector(w.X, w.Y, w.Z);
    }

    public override string ToString() { return string.Format("X:{0}, Y:{1}, Z:{2}", X, Y, Z); }
  }
}

