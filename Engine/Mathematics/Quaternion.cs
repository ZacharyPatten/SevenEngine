// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use with the following condition: any code using or originally taken 
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
  /// <summary>Implements a 4-component (x, y, z, and w) quaternion for 3D rotatoins.</summary>
  public class Quaternion
  {
    float _x, _y, _z, _w;

    public float X { get { return _x; } set { _x = value; } }
    public float Y { get { return _y; } set { _y = value; } }
    public float Z { get { return _z; } set { _z = value; } }
    public float W { get { return _w; } set { _w = value; } }

    public static readonly Quaternion FactoryIdentity = new Quaternion(0, 0, 0, 1);

    public Quaternion(float x, float y, float z, float w) { _x = x; _y = y; _z = z; _w = w; }

    public static Quaternion operator +(Quaternion left, Quaternion right) { return left.Add(right); }
    public static Quaternion operator -(Quaternion left, Quaternion right) { return left.Subtract(right); }
    public static Quaternion operator *(Quaternion left, Quaternion right) { return left.Multiply(right); }
    public static Quaternion operator *(Quaternion quaternion, Vector vector) { return quaternion.Multiply(vector); }
    public static Quaternion operator *(Quaternion quaternion, float scalor) { return quaternion.Multiply(scalor); }
    public static Quaternion operator *(float scalor, Quaternion quaternion) { return quaternion.Multiply(scalor); }
    
    public static bool operator ==(Quaternion left, Quaternion right)
    {
      if (Object.ReferenceEquals(left, right))
        return true;
      if (left == null || right == null)
        return false;
      return left.Equals(right);
    }

    public static bool operator !=(Quaternion left, Quaternion right)
    {
      if (Object.ReferenceEquals(left, right))
        return true;
      if (left == null || right == null)
        return false;
      return !left.Equals(right);
    }

    public float Length
    {
      get { return
        (float)Math.Sqrt(
        _x * _x +
        _y * _y +
        _z * _z +
        _w * _w); }
    }
    
    public float LengthSquared
    {
      get { return
        _x * _x +
        _y * _y +
        _z * _z + 
        _w * _w; }
    }
    
    public Quaternion Conjugate()
    {
      return new Quaternion(
        -_x,
        -_y,
        -_z,
        _w);
    }
    
    public Quaternion Add(Quaternion right)
    {
      return new Quaternion(
        _x + right.X,
        _y + right.Y,
        _z + right.Z,
        _w + right.W);
    }
    
    public Quaternion Subtract(Quaternion right)
    {
      return new Quaternion(
        _x - right.X,
        _y - right.Y,
        _z - right.Z,
        _w - right.W);
    }

    public Quaternion Multiply(Quaternion right)
    {
      return new Quaternion(
        _x * right.W + _w * right.X + _y * right.Z - _z * right.Y,
        _y * right.W + _w * right.Y + _z * right.X - _x * right.Z,
        _z * right.W + _w * right.Z + _x * right.Y - _y * right.X,
        _w * right.W - _x * right.X - _y * right.Y - _z * right.Z);
    }

    public Quaternion Multiply(float scalor)
    {
      return new Quaternion(
        _x * scalor,
        _y * scalor,
        _z * scalor,
        _w * scalor);
    }

    public Quaternion Multiply(Vector vector)
    {
      return new Quaternion(
        _w * vector.X + _y * vector.Z - _z * vector.Y,
        _w * vector.Y + _z * vector.X - _x * vector.Z,
        _w * vector.Z + _x * vector.Y - _y * vector.X,
        -_x * vector.X - _y * vector.Y - _z * vector.Z);
    }


    public Quaternion Normalize()
    {
      float length = Length;
      return new Quaternion(
        _x / length,
        _y / length,
        _z / length,
        _w / length);
    }

    public Quaternion Invert()
    {
      float lengthSq = LengthSquared;
      if (lengthSq == 0.0)
        return new Quaternion(_x, _y, _z, _w);
      return new Quaternion(
        -_x / lengthSq,
        -_y / lengthSq,
        -_z / lengthSq,
        _w / lengthSq);
    }

    public static Quaternion FromAxisAngle(Vector axis, float angle)
    {
      if (axis.LengthSquared == 0.0f)
        return FactoryIdentity;
      float sinAngleOverAxisLength = Trigonometry.Sin(angle / 2) / axis.Length;
      return new Quaternion(
        axis.X * sinAngleOverAxisLength,
        axis.Y * sinAngleOverAxisLength,
        axis.Z * sinAngleOverAxisLength,
        Trigonometry.Cos(angle / 2)).Normalize();
    }

    public Quaternion Lerp(Quaternion right, float blend)
    {
      if (blend < 0 || blend > 1.0f)
        throw new QuaternionException("Attempting linear interpolation with invalid blend (blend < 0.0f || blend > 1.0f).");

      if (LengthSquared == 0.0f)
      { if (right.LengthSquared == 0.0f)
          return FactoryIdentity;
        else
          return new Quaternion(right.X, right.Y, right.Z, right.W); }
      else if (right.LengthSquared == 0.0f)
        return new Quaternion(_x, _y, _z, _w);

      Quaternion result = new Quaternion(
        (1.0f - blend) * _x + blend * right.X,
        (1.0f - blend) * _y + blend * right.Y,
        (1.0f - blend) * _z + blend * right.Z,
        (1.0f - blend) * _w + blend * right.W);

      if (result.LengthSquared > 0.0f)
        return result.Normalize();
      else
        return FactoryIdentity;
    }

    public Quaternion Slerp(Quaternion right, float blend)
    {
      if (blend < 0 || blend > 1.0f)
        throw new QuaternionException("Attempting sphereical interpolation with invalid blend (blend < 0.0f || blend > 1.0f).");

      if (LengthSquared == 0.0f)
      { if (right.LengthSquared == 0.0f)
          return FactoryIdentity;
        else
          return new Quaternion(right.X, right.Y, right.Z, right.W); }
      else if (right.LengthSquared == 0.0f)
        return new Quaternion(_x, _y, _z, _w);

      float cosHalfAngle = _x * right.X + _y * right.Y + _z * right.Z + _w * right.W;

      if (cosHalfAngle >= 1.0f || cosHalfAngle <= -1.0f)
        return new Quaternion(_x, _y, _z, _w);
      else if (cosHalfAngle < 0.0f)
      {
        right = new Quaternion(-_x, -_y, -_z, -_w);
        cosHalfAngle = -cosHalfAngle;
      }

      float halfAngle = (float)Math.Acos(cosHalfAngle);
      float sinHalfAngle = Trigonometry.Sin(halfAngle);
      float blendA = Trigonometry.Sin(halfAngle * (1.0f - blend)) / sinHalfAngle;
      float blendB = Trigonometry.Sin(halfAngle * blend) / sinHalfAngle;
      
      Quaternion result = new Quaternion(
        blendA * _x + blendB * right.X,
        blendA * _y + blendB * right.Y,
        blendA * _z + blendB * right.Z,
        blendA * _w + blendB * right.W);

      if (result.LengthSquared > 0.0f)
        return result.Normalize();
      else
        return FactoryIdentity;
    }

    public Matrix ToMatrix()
    {
      return new Matrix(
        _w*_w + _x*_x - _y*_y - _z*_z, 2*_x*_y-2*_w*_z, 2*_x*_z + 2*_w*_y,
        2*_x*_y + 2*_w*_z, _w*_w - _x*_x + _y*_y - _z*_z, 2*_y*_z + 2*_w*_x,
        2*_x*_z - 2*_w*_y, 2*_y*_z - 2*_w*_x, _w*_w - _x*_x - _y*_y + _z*_z);
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

    /// <summary>This is used for throwing quaternion exceptions only to make debugging faster.</summary>
    private class QuaternionException : Exception { public QuaternionException(string message) : base(message) { } }
  }
}