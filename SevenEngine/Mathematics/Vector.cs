// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use under the following condition: any code using or originally taken 
// from the SevenEngine project must include citation to its original author(s) located at the
// top of each source code file, or you may include a reference to the SevenEngine project as
// a whole but you must include the current SevenEngine official website URL and logo.
// - Thanks.  :)  (support: seven@sevenengine.com)

// Author(s):
// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com

using System;

namespace SevenEngine.Mathematics
{
  /// <summary>Implements a 3-component (x, y, z) vector matrix.</summary>
  public class Vector
  {
    protected float _x, _y, _z;

    public float X { get { return _x; } set { _x = value; } }
    public float Y { get { return _y; } set { _y = value; } }
    public float Z { get { return _z; } set { _z = value; } }

    public Vector(float x, float y, float z) { _x = x; _y = y; _z = z; }

    public static readonly Vector FactoryZero = new Vector(0, 0, 0);

    public static Vector operator +(Vector left, Vector right) { return Vector.Add(left, right); }
    public static Vector operator -(Vector left, Vector right) { return Vector.Subtract(left, right); }
    public static Vector operator -(Vector vector) { return Vector.Negate(vector); }
    public static Vector operator *(Vector left, float right) { return Vector.Multiply(left, right); }
    public static Vector operator *(float left, Vector right) { return Vector.Multiply(right, left); }
    public static Vector operator /(Vector left, float right) { return Vector.Divide(left, right); }
    public static bool operator ==(Vector left, Vector right) { return Vector.Equals(left, right); }
    public static bool operator !=(Vector left, Vector right) { return !Vector.Equals(left, right); }
    public static implicit operator float[,](Vector vector) { return Vector.ToFloats(vector); }
    public static implicit operator Matrix(Vector vector) { return Vector.ToMatrix(vector); }

    public Vector Add(Vector right) { return Vector.Add(this, right); }
    public Vector Negate() { return Vector.Negate(this); }
    public Vector Subtract(Vector right) { return Vector.Subtract(this, right); }
    public Vector Multiply(float right) { return Vector.Multiply(this, right); }
    public Vector Divide(float right) { return Vector.Divide(this, right); }
    public float DotProduct(Vector right) { return Vector.DotProduct(this, right); }
    public Vector CrossProduct(Vector right) { return Vector.CrossProduct(this, right); }
    public Vector Normalize() { return Vector.Normalize(this); }
    public float Length() { return Vector.Length(this); }
    public float LengthSquared() { return Vector.LengthSquared(this); }
    public bool Equals(Vector right) { return Vector.Equals(this, right); }
    public bool Equals(Vector right, float leniency) { return Vector.Equals(this, right, leniency); }
    public Vector RotateBy(float angle, float x, float y, float z) { return Vector.RotateBy(this, angle, x, y, z); }
    public Vector Lerp(Vector right, float blend) { return Vector.InterpolateLinear(this, right, blend); }
    public Vector Slerp(Vector right, float blend) { return Vector.InterpolateSphereical(this, right, blend); }
    public Vector RotateBy(Quaternion rotation) { return Vector.RotateBy(this, rotation); }

    public static Vector Add(Vector left, Vector right)
    {
      return new Vector(
        left.X + right.X,
        left.Y + right.Y,
        left.Z + right.Z);
    }

    public static Vector Negate(Vector vector)
    {
      return new Vector(
        -vector.X,
        -vector.Y,
        -vector.Z);
    }

    public static Vector Subtract(Vector left, Vector right)
    {
      return new Vector(
        left.X - right.X,
        left.Y - right.Y,
        left.Z - right.Z);
    }

    public static Vector Multiply(Vector left, float right)
    {
      return new Vector(
        left.X * right,
        left.Y * right,
        left.Z * right);
    }

    public static Vector Divide(Vector left, float right)
    {
      return new Vector(
        left.X / right,
        left.Y / right,
        left.Z / right);
    }

    public static float DotProduct(Vector left, Vector right)
    {
      return
        left.X * right.X +
        left.Y * right.Y +
        left.Z * right.Z;
    }

    public static Vector CrossProduct(Vector left, Vector right)
    {
      return new Vector(
        left.Y * right.Z - left.Z * right.Y,
        left.Z * right.X - left.X * right.Z,
        left.X * right.Y - left.Y * right.X);
    }

    public static Vector Normalize(Vector vector)
    {
      float length = vector.Length();
      if (length != 0.0)
        return new Vector(
          vector.X / length,
          vector.Y / length,
          vector.Z / length);
      else
        return Vector.FactoryZero;
    }

    public static float Length(Vector vector)
    {
      return Calc.SquareRoot(
        vector.X * vector.X +
        vector.Y * vector.Y +
        vector.Z * vector.Z);
    }

    public static float LengthSquared(Vector vector)
    {
      return
        vector.X * vector.X +
        vector.Y * vector.Y +
        vector.Z * vector.Z;
    }

    public static bool Equals(Vector left, Vector right)
    {
      if (object.ReferenceEquals(left, right))
        return true;
      else if (object.ReferenceEquals(left, null) || object.ReferenceEquals(null, right))
        return false;
      else return
        left.X == right.X && 
        left.Y == right.Y &&
        left.Z == right.Z;
    }

    public static bool Equals(Vector left, Vector right, float leniency)
    {
      if (object.ReferenceEquals(left, right))
        return true;
      else if (object.ReferenceEquals(left, null) || object.ReferenceEquals(null, right))
        return false;
      else return
        Calc.Abs(left.X - right.X) < leniency &&
        Calc.Abs(left.Y - right.Y) < leniency &&
        Calc.Abs(left.Z - right.Z) < leniency;
    }

    public static Vector DirectionTowardsPosition(Vector from, Vector to)
    {
      return (to - from).Normalize();
    }

    public static Vector MoveTowardsPosition(Vector position, Vector goal, float distance)
    {
      Vector direction = DirectionTowardsPosition(position, goal);
      return new Vector(
        position.X + direction.X * distance,
        position.Y + direction.Y * distance,
        position.Z + direction.Z * distance);
    }

    public static Vector MoveTowardsDirection(Vector position, Vector direction, float distance)
    {
      direction = direction.Normalize();
      return new Vector(
        position.X + direction.X * distance,
        position.Y + direction.Y * distance,
        position.Z + direction.Z * distance);
    }

    public static float AngleBetween(Vector first, Vector second)
    {
      return Calc.ArcCos(Vector.DotProduct(first, second) / (first.Length() * second.Length()));
    }

    public static Vector RotateBy(Vector vector, float angle, float x, float y, float z)
    {
      // Note: the angle is in radians
      float sinHalfAngle = Calc.Sin(angle / 2);
      float cosHalfAngle = Calc.Cos(angle / 2);
      x *= sinHalfAngle;
      y *= sinHalfAngle;
      z *= sinHalfAngle;
      float x2 = cosHalfAngle * vector.X + y * vector.Z - z * vector.Y;
      float y2 = cosHalfAngle * vector.Y + z * vector.X - x * vector.Z;
      float z2 = cosHalfAngle * vector.Z + x * vector.Y - y * vector.X;
      float w2 = -x * vector.X - y * vector.Y - z * vector.Z;
      return new Vector(
        x * w2 + cosHalfAngle * x2 + y * z2 - z * y2,
        y * w2 + cosHalfAngle * y2 + z * x2 - x * z2,
        z * w2 + cosHalfAngle * z2 + x * y2 - y * x2);
    }

    public static Vector RotateBy(Vector vector, Quaternion rotation)
    {
      Quaternion answer = (rotation * vector) * Quaternion.Conjugate(rotation);
      return new Vector(answer.X, answer.Y, answer.Z);
    }

    public static Vector InterpolateLinear(Vector a, Vector b, float blend)
    {
      if (blend < 0 || blend > 1.0f)
        throw new VectorException("invalid lerp blend value: (blend < 0.0f || blend > 1.0f).");
      return new Vector(
        a.X + blend * (b.X - a.X),
        a.Y + blend * (b.Y - a.Y),
        a.Z + blend * (b.Z - a.Z));
    }

    public static Vector InterpolateSphereical(Vector a, Vector b, float blend)
    {
      if (blend < 0 || blend > 1.0f)
        throw new VectorException("invalid slerp blend value: (blend < 0.0f || blend > 1.0f).");
      float dot = Calc.Clamp(Vector.DotProduct(a, b), -1.0f, 1.0f);
      float theta = Calc.ArcCos(dot) * blend;
      return a * Calc.Cos(theta) + (b - a * dot).Normalize() * Calc.Sin(theta);
    }

    public static Vector InterpolateBarycentric(Vector a, Vector b, Vector c, float u, float v)
    {
      return a + u * (b - a) + v * (c - a);
    }

    public static float[,] ToFloats(Vector vector)
    {
      return new float[,] { { vector.X }, { vector.Y }, { vector.Z } };
    }

    public static Matrix ToMatrix(Vector vector)
    {
      return new Matrix(new float[,] { { vector.X }, { vector.Y }, { vector.Z } });
      //Matrix matrix = new Matrix(vector.X, vector.Y, vector.Z);
      //matrix[0, 0] = vector.X;
      //matrix[1, 0] = vector.Y;
      //matrix[2, 0] = vector.Z;
      //return matrix;
    }

    public override string ToString()
    {
      return base.ToString();
      //return
      //  X.ToString() + "\n" + 
      //  Y.ToString() + "\n" +
      //  Z.ToString() + "\n";
    }

    public override int GetHashCode()
    {
      return
        X.GetHashCode() ^
        Y.GetHashCode() ^
        Z.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      return base.Equals(obj);
    }

    private class VectorException : Exception
    {
      public VectorException(string message) : base(message) { }
    }
  }
}
