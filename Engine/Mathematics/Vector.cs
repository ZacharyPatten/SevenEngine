// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use with the following condition: any code using or originally taken from the 
// SevenEngine project must include citation to its original author(s) located at the top of each
// source code file. Alternatively, you may include a reference to the SevenEngine project as a whole,
// but you must include the current SevenEngine official website URL and logo.
// - Thanks.  :)  (support: seven@sevenengine.com)

// Author(s):
// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com
// Last Edited: 10-26-13

using System;
using System.Runtime.InteropServices;

namespace SevenEngine.Mathematics
{
  /// <summary>Implements a 3-component vector.</summary>
  [StructLayout(LayoutKind.Sequential)]
  public class Vector
  {
    private float _x, _y, _z;

    public float X { get { return _x; } set { _x = value; } }
    public float Y { get { return _y; } set { _y = value; } }
    public float Z { get { return _z; } set { _z = value; } }

    public Vector(float x, float y, float z) { _x = x; _y = y; _z = z; }

    public static readonly Vector ZeroFactory = new Vector(0, 0, 0);

    public static Vector operator +(Vector left, Vector right) { return left.Add(right); }
    public static Vector operator -(Vector left, Vector right) { return left.Subtract(right); }
    public static Vector operator -(Vector vector) { return new Vector(-vector.X, -vector.Y, -vector.Z); }
    public static Vector operator *(Vector vector, float scalar) { return vector.Multiply(scalar); }
    public static Vector operator *(float scalar, Vector vector) { return vector.Multiply(scalar); }
    public static Vector operator /(Vector vector, float scalar) { return vector.Divide(scalar); }

    public static bool operator ==(Vector left, Vector right)
    {
      if (Object.ReferenceEquals(left, right))
        return true;
      if (left == null || right == null)
        return false;
      return left.Equals(right);
    }

    public static bool operator !=(Vector left, Vector right)
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
      _z * _z); } 
    }
    
    public float LengthSquared
    {
      get { return
        _x * _x +
        _y * _y +
        _z * _z; }
    }

    public Vector Add(Vector right)
    {
      return new Vector(
        _x + right.X, 
        _y + right.Y, 
        _z + right.Z);
    }

    public Vector Subtract(Vector right)
    {
      return new Vector(
        _x - right.X,
        _y - right.Y,
        _z - right.Z);
    }

    public Vector Multiply(float scalar)
    {
      return new Vector(
        _x * scalar,
        _y * scalar,
        _z * scalar);
    }

    public Vector Divide(float scalar)
    {
      return new Vector(
        _x / scalar,
        _y / scalar,
        _z / scalar);
    }

    public float DotProduct(Vector vector)
    {
      return
        _x * vector.X +
        _y * vector.Y +
        _z * vector.Z;
    }

    public Vector CrossProduct(Vector vector)
    {
      return new Vector(
        _y * vector.Z - _z * vector.Y,
        _z * vector.X - _x * vector.Z,
        _x * vector.Y - _y * vector.X);
    }

    public Vector Normalize()
    {
      float length = Length;
      if (length != 0.0)
        return new Vector(_x / length, _y / length, _z / length);
      else
        return new Vector(0, 0, 0);
    }

    /// <summary>Rotates this vector by quaternion values.</summary>
    /// <param name="angle">The angle of rotation in radians.</param>
    /// <param name="x">The "x" value of the axis of rotation.</param>
    /// <param name="y">The "y" value of the axis of rotation.</param>
    /// <param name="z">The "z" value of the axis of rotation.</param>
    public Vector RotateBy(float angle, float x, float y, float z)
    {
      float sinHalfAngle = Trigonometry.Sin(angle / 2);
      float cosHalfAngle = Trigonometry.Cos(angle / 2);
      //Quaternion rotation = new Quaternion(
      //  x * sinHalfAngle,
      //  y * sinHalfAngle,
      //  z * sinHalfAngle,
      //  cosHalfAngle);
      x *= sinHalfAngle;
      y *= sinHalfAngle;
      z *= sinHalfAngle;
      //Quaternion conjugate = rotation.Conjugate();
      //Quaternion w = rotation.Multiply(this).Multiply(conjugate);
      float x2 = cosHalfAngle * _x + y * _z - z * _y;
      float y2 = cosHalfAngle * _y + z * _x - x * _z;
      float z2 = cosHalfAngle * _z + x * _y - y * _x;
      float w2 = -x * _x - y * _y - z * _z;
      //return new Vector(w.X, w.Y, w.Z);
      return new Vector(
        x * w2 + cosHalfAngle * x2 + y * z2 - z * y2,
        y * w2 + cosHalfAngle * y2 + z * x2 - x * z2,
        z * w2 + cosHalfAngle * z2 + x * y2 - y * x2);
    }

    public Vector RotateBy(Quaternion rotation)
    {
      Quaternion answer = rotation.Multiply(this).Multiply(rotation.Conjugate());
      return new Vector(answer.X, answer.Y, answer.Z);
    }

    public Vector Lerp(Vector right, float blend)
    {
      if (blend < 0 || blend > 1.0f)
        throw new VectorException("Attempting linear interpolation with invalid blend (blend < 0.0f || blend > 1.0f).");
      return new Vector(
        _x + blend * (right.X - _x),
        _y + blend * (right.Y - _y),
        _z + blend * (right.Z - _z));
    }

    public Vector Slerp(Vector right, float blend)
    {
      if (blend < 0 || blend > 1.0f)
        throw new VectorException("Attempting sphereical interpolation with invalid blend (blend < 0.0f || blend > 1.0f).");
      float clampedDot = Foundations.Clamp(DotProduct(right), -1.0f, 1.0f);
      float theta = Trigonometry.ArcCos(clampedDot) * blend;
      return
        this * Trigonometry.Cos(theta) +
        (right - this * clampedDot).Normalize() * Trigonometry.Sin(theta);
    }

    public override string ToString()
    {
      return string.Format("X: |{0}|\nY: |{1}|\nZ: |{2}|",
        _x, _y, _z);
    }

    public override int GetHashCode()
    {
      return
        _x.GetHashCode() ^
        _y.GetHashCode() ^
        _z.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if (obj is Vector)
        return Equals((Vector)obj);
      return base.Equals(obj);
    }

    public bool Equals(Vector right)
    {
      return
        _x == right.X &&
        _y == right.Y &&
        _z == right.Z;
    }

    /// <summary>This is used for throwing vector exceptions only to make debugging faster.</summary>
    private class VectorException : Exception { public VectorException(string message) : base(message) { } }
  }
}