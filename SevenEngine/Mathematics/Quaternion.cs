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
  /// <summary>Implements a 4-component (x, y, z, and w) quaternion.</summary>
  public class Quaternion
  {
    protected float _x, _y, _z, _w;

    public float X { get { return _x; } set { _x = value; } }
    public float Y { get { return _y; } set { _y = value; } }
    public float Z { get { return _z; } set { _z = value; } }
    public float W { get { return _w; } set { _w = value; } }

    public Quaternion(float x, float y, float z, float w) { _x = x; _y = y; _z = z; _w = w; }

    public static readonly Quaternion FactoryIdentity = new Quaternion(0, 0, 0, 1);

    public static Quaternion FactoryFromAxisAngle(Vector axis, float angle)
    {
      if (axis.LengthSquared() == 0.0f)
        return FactoryIdentity;
      float sinAngleOverAxisLength = Calc.Sin(angle / 2) / axis.Length();
      return Quaternion.Normalize(new Quaternion(
        axis.X * sinAngleOverAxisLength,
        axis.Y * sinAngleOverAxisLength,
        axis.Z * sinAngleOverAxisLength,
        Calc.Cos(angle / 2)));
    }

    public static Quaternion operator +(Quaternion left, Quaternion right) { return Quaternion.Add(left, right); }
    public static Quaternion operator -(Quaternion left, Quaternion right) { return Quaternion.Subtract(left, right); }
    public static Quaternion operator *(Quaternion left, Quaternion right) { return Quaternion.Multiply(left, right); }
    public static Quaternion operator *(Quaternion left, Vector right) { return Quaternion.Multiply(left, right); }
    public static Quaternion operator *(Quaternion left, float right) { return Quaternion.Multiply(left, right); }
    public static Quaternion operator *(float scalor, Quaternion quaternion) { return Quaternion.Multiply(quaternion, scalor); }
    public static bool operator ==(Quaternion left, Quaternion right) { return Quaternion.Equals(left, right); }
    public static bool operator !=(Quaternion left, Quaternion right) { return !Quaternion.Equals(left, right); }

    public float Length() { return Quaternion.Length(this); }
    public float LengthSquared() { return Quaternion.LengthSquared(this); }
    public Quaternion Conjugate() { return Quaternion.Conjugate(this); }
    public Quaternion Add(Quaternion right) { return Quaternion.Add(this, right); }
    public Quaternion Subtract(Quaternion right) { return Quaternion.Subtract(this, right); }
    public Quaternion Multiply(Quaternion right) { return Quaternion.Multiply(this, right); }
    public Quaternion Multiply(float right) { return Quaternion.Multiply(this, right); }
    public Quaternion Multiply(Vector vector) { return Quaternion.Multiply(this, vector); }
    public Quaternion Normalize() { return Quaternion.Normalize(this); }
    public Quaternion Invert() { return Quaternion.Invert(this); }
    public Quaternion Lerp(Quaternion right, float blend) { return Quaternion.Lerp(this, right, blend); }
    public Quaternion Slerp(Quaternion right, float blend) { return Quaternion.Slerp(this, right, blend); }
    public Vector Rotate(Vector vector) { return Quaternion.Rotate(vector, this); }

    public static float Length(Quaternion quaternion)
    {
      return
        Calc.SquareRoot(
          quaternion.X * quaternion.X +
          quaternion.Y * quaternion.Y +
          quaternion.Z * quaternion.Z +
          quaternion.W * quaternion.W);
    }
    
    public static float LengthSquared(Quaternion quaternion)
    {
      return
        quaternion.X * quaternion.X +
        quaternion.Y * quaternion.Y +
        quaternion.Z * quaternion.Z +
        quaternion.W * quaternion.W;
    }
    
    public static Quaternion Conjugate(Quaternion quaternion)
    {
      return new Quaternion(
        -quaternion.X,
        -quaternion.Y,
        -quaternion.Z,
        quaternion.W);
    }
    
    public static Quaternion Add(Quaternion left, Quaternion right)
    {
      return new Quaternion(
        left.X + right.X,
        left.Y + right.Y,
        left.Z + right.Z,
        left.W + right.W);
    }
    
    public static Quaternion Subtract(Quaternion left, Quaternion right)
    {
      return new Quaternion(
        left.X - right.X,
        left.Y - right.Y,
        left.Z - right.Z,
        left.W - right.W);
    }

    public static Quaternion Multiply(Quaternion left, Quaternion right)
    {
      return new Quaternion(
        left.X * right.W + left.W * right.X + left.Y * right.Z - left.Z * right.Y,
        left.Y * right.W + left.W * right.Y + left.Z * right.X - left.X * right.Z,
        left.Z * right.W + left.W * right.Z + left.X * right.Y - left.Y * right.X,
        left.W * right.W - left.X * right.X - left.Y * right.Y - left.Z * right.Z);
    }

    public static Quaternion Multiply(Quaternion left, float right)
    {
      return new Quaternion(
        left.X * right,
        left.Y * right,
        left.Z * right,
        left.W * right);
    }

    public static Quaternion Multiply(Quaternion left, Vector vector)
    {
      return new Quaternion(
        left.W * vector.X + left.Y * vector.Z - left.Z * vector.Y,
        left.W * vector.Y + left.Z * vector.X - left.X * vector.Z,
        left.W * vector.Z + left.X * vector.Y - left.Y * vector.X,
        -left.X * vector.X - left.Y * vector.Y - left.Z * vector.Z);
    }


    public static Quaternion Normalize(Quaternion quaternion)
    {
      float length = Quaternion.Length(quaternion);
      return new Quaternion(
        quaternion.X / length,
        quaternion.Y / length,
        quaternion.Z / length,
        quaternion.W / length);
    }

    public static Quaternion Invert(Quaternion quaternion)
    {
      float lengthSquared = Quaternion.LengthSquared(quaternion);
      if (lengthSquared == 0.0)
        return new Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
      return new Quaternion(
        -quaternion.X / lengthSquared,
        -quaternion.Y / lengthSquared,
        -quaternion.Z / lengthSquared,
        quaternion.W / lengthSquared);
    }

    public static Quaternion Lerp(Quaternion left, Quaternion right, float blend)
    {
      if (blend < 0 || blend > 1.0f)
        throw new QuaternionException("invalid blending value during lerp !(blend < 0.0f || blend > 1.0f).");
      if (Quaternion.LengthSquared(left) == 0.0f)
      {
        if (Quaternion.LengthSquared(right) == 0.0f)
          return FactoryIdentity;
        else
          return new Quaternion(right.X, right.Y, right.Z, right.W); }
      else if (Quaternion.LengthSquared(right) == 0.0f)
        return new Quaternion(left.X, left.Y, left.Z, left.W);
      Quaternion result = new Quaternion(
        (1.0f - blend) * left.X + blend * right.X,
        (1.0f - blend) * left.Y + blend * right.Y,
        (1.0f - blend) * left.Z + blend * right.Z,
        (1.0f - blend) * left.W + blend * right.W);
      if (Quaternion.LengthSquared(result) > 0.0f)
        return Quaternion.Normalize(result);
      else
        return FactoryIdentity;
    }

    public static Quaternion Slerp(Quaternion left, Quaternion right, float blend)
    {
      if (blend < 0 || blend > 1.0f)
        throw new QuaternionException("invalid blending value during slerp !(blend < 0.0f || blend > 1.0f).");
      if (Quaternion.LengthSquared(left) == 0.0f)
      {
        if (Quaternion.LengthSquared(right) == 0.0f)
          return FactoryIdentity;
        else
          return new Quaternion(right.X, right.Y, right.Z, right.W); }
      else if (Quaternion.LengthSquared(right) == 0.0f)
        return new Quaternion(left.X, left.Y, left.Z, left.W);
      float cosHalfAngle = left.X * right.X + left.Y * right.Y + left.Z * right.Z + left.W * right.W;
      if (cosHalfAngle >= 1.0f || cosHalfAngle <= -1.0f)
        return new Quaternion(left.X, left.Y, left.Z, left.W);
      else if (cosHalfAngle < 0.0f)
      {
        right = new Quaternion(-left.X, -left.Y, -left.Z, -left.W);
        cosHalfAngle = -cosHalfAngle;
      }
      float halfAngle = (float)Math.Acos(cosHalfAngle);
      float sinHalfAngle = Calc.Sin(halfAngle);
      float blendA = Calc.Sin(halfAngle * (1.0f - blend)) / sinHalfAngle;
      float blendB = Calc.Sin(halfAngle * blend) / sinHalfAngle;
      Quaternion result = new Quaternion(
        blendA * left.X + blendB * right.X,
        blendA * left.Y + blendB * right.Y,
        blendA * left.Z + blendB * right.Z,
        blendA * left.W + blendB * right.W);
      if (Quaternion.LengthSquared(result) > 0.0f)
        return Quaternion.Normalize(result);
      else
        return FactoryIdentity;
    }

    public static Vector Rotate(Vector vector, Quaternion rotation)
    {
      Quaternion answer = (rotation * vector) * Quaternion.Conjugate(rotation);
      return new Vector(answer.X, answer.Y, answer.Z);
    }

    public static bool Equals(Quaternion left, Quaternion right)
    {
      if (object.ReferenceEquals(left, right))
        return true;
      else if (left == null || right == null)
        return false;
      else return
        left.X == right.X &&
        left.Y == right.Y &&
        left.Z == right.Z &&
        left.W == right.W;
    }

    public static Matrix3x3 ToMatrix(Quaternion q)
    {
      return new Matrix3x3(
        q.W * q.W + q.X * q.X - q.Y * q.Y - q.Z * q.Z,
        2 * q.X * q.Y - 2 * q.W * q.Z,
        2 *q.X * q.Z + 2 * q.W * q.Y,
        2 * q.X * q.Y + 2 * q.W * q.Z,
        q.W * q.W - q.X * q.X + q.Y * q.Y - q.Z * q.Z,
        2 * q.Y * q.Z + 2 * q.W * q.X,
        2 * q.X * q.Z - 2 * q.W * q.Y,
        2 * q.Y * q.Z - 2 * q.W * q.X,
        q.W * q.W - q.X * q.X - q.Y * q.Y + q.Z * q.Z);
    }

    public override string ToString()
    {
      return
        String.Format("X: |{0}|\nY: |{1}|\nZ: |{2}|\nW: |{3}|",
        _x, _y, _z, _w);
    }

    public override int GetHashCode()
    { 
      return 
        _x.GetHashCode() ^
        _y.GetHashCode() ^
        _z.GetHashCode() ^
        _w.GetHashCode();
    }

    public override bool Equals(object other)
    {
      if (other is Quaternion)
        return Equals((Quaternion)other);
      return base.Equals(other);
    }

    public bool Equals(Quaternion right)
    {
      return
        _x == right.X &&
        _y == right.Y &&
        _z == right.Z &&
        W == right.W;
    }

    private class QuaternionException : Exception
    {
      public QuaternionException(string message) : base(message) { }
    }
  }
}