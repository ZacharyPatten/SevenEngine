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
  /// <summary>Implements a 4-component (x, y, z, and w) quaternion. X, Y, and Z represent the axis
  /// of rotation, and W is the rotation ammount.</summary>
  public class Quaternion
  {
    protected float _x, _y, _z, _w;

    /// <summary>The X component of the quaternion. (axis, NOT rotation ammount)</summary>
    public float X { get { return _x; } set { _x = value; } }
    /// <summary>The Y component of the quaternion. (axis, NOT rotation ammount)</summary>
    public float Y { get { return _y; } set { _y = value; } }
    /// <summary>The Z component of the quaternion. (axis, NOT rotation ammount)</summary>
    public float Z { get { return _z; } set { _z = value; } }
    /// <summary>The W component of the quaternion. (rotation ammount, NOT axis)</summary>
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
    public Vector Rotate(Vector vector) { return Quaternion.Rotate(this, vector); }

    /// <summary>Computes the length of quaternion.</summary>
    /// <param name="quaternion">The quaternion to compute the length of.</param>
    /// <returns>The length of the given quaternion.</returns>
    public static float Length(Quaternion quaternion)
    {
      return
        Calc.SquareRoot(
          quaternion.X * quaternion.X +
          quaternion.Y * quaternion.Y +
          quaternion.Z * quaternion.Z +
          quaternion.W * quaternion.W);
    }
    
    /// <summary>Computes the length of a quaternion, but doesn't square root it
    /// for optimization possibilities.</summary>
    /// <param name="quaternion">The quaternion to compute the length squared of.</param>
    /// <returns>The squared length of the given quaternion.</returns>
    public static float LengthSquared(Quaternion quaternion)
    {
      return
        quaternion.X * quaternion.X +
        quaternion.Y * quaternion.Y +
        quaternion.Z * quaternion.Z +
        quaternion.W * quaternion.W;
    }
    
    /// <summary>Gets the conjugate of the quaternion.</summary>
    /// <param name="quaternion">The quaternion to conjugate.</param>
    /// <returns>The conjugate of teh given quaternion.</returns>
    public static Quaternion Conjugate(Quaternion quaternion)
    {
      return new Quaternion(
        -quaternion.X,
        -quaternion.Y,
        -quaternion.Z,
        quaternion.W);
    }
    
    /// <summary>Adds two quaternions together.</summary>
    /// <param name="left">The first quaternion of the addition.</param>
    /// <param name="right">The second quaternion of the addition.</param>
    /// <returns>The result of the addition.</returns>
    public static Quaternion Add(Quaternion left, Quaternion right)
    {
      return new Quaternion(
        left.X + right.X,
        left.Y + right.Y,
        left.Z + right.Z,
        left.W + right.W);
    }
    
    /// <summary>Subtracts two quaternions.</summary>
    /// <param name="left">The left quaternion of the subtraction.</param>
    /// <param name="right">The right quaternion of the subtraction.</param>
    /// <returns>The resulting quaternion after the subtraction.</returns>
    public static Quaternion Subtract(Quaternion left, Quaternion right)
    {
      return new Quaternion(
        left.X - right.X,
        left.Y - right.Y,
        left.Z - right.Z,
        left.W - right.W);
    }

    /// <summary>Multiplies two quaternions together.</summary>
    /// <param name="left">The first quaternion of the multiplication.</param>
    /// <param name="right">The second quaternion of the multiplication.</param>
    /// <returns>The resulting quaternion after the multiplication.</returns>
    public static Quaternion Multiply(Quaternion left, Quaternion right)
    {
      return new Quaternion(
        left.X * right.W + left.W * right.X + left.Y * right.Z - left.Z * right.Y,
        left.Y * right.W + left.W * right.Y + left.Z * right.X - left.X * right.Z,
        left.Z * right.W + left.W * right.Z + left.X * right.Y - left.Y * right.X,
        left.W * right.W - left.X * right.X - left.Y * right.Y - left.Z * right.Z);
    }

    /// <summary>Multiplies all the values of the quaternion by a scalar value.</summary>
    /// <param name="left">The quaternion of the multiplication.</param>
    /// <param name="right">The scalar of the multiplication.</param>
    /// <returns>The result of multiplying all the values in the quaternion by the scalar.</returns>
    public static Quaternion Multiply(Quaternion left, float right)
    {
      return new Quaternion(
        left.X * right,
        left.Y * right,
        left.Z * right,
        left.W * right);
    }

    /// <summary>Pre-multiplies a 3-component vector by a quaternion.</summary>
    /// <param name="left">The quaternion to pre-multiply the vector by.</param>
    /// <param name="vector">The vector to be multiplied.</param>
    /// <returns>The resulting quaternion of the multiplication.</returns>
    public static Quaternion Multiply(Quaternion left, Vector vector)
    {
      if (vector.Dimensions == 3)
      {
        return new Quaternion(
          left.W * vector.X + left.Y * vector.Z - left.Z * vector.Y,
          left.W * vector.Y + left.Z * vector.X - left.X * vector.Z,
          left.W * vector.Z + left.X * vector.Y - left.Y * vector.X,
          -left.X * vector.X - left.Y * vector.Y - left.Z * vector.Z);
      }
      else
        throw new QuaternionException("my quaternion rotations are only defined for 3-component vectors.");
    }

    /// <summary>Normalizes the quaternion.</summary>
    /// <param name="quaternion">The quaternion to normalize.</param>
    /// <returns>The normalization of the given quaternion.</returns>
    public static Quaternion Normalize(Quaternion quaternion)
    {
      float length = Quaternion.Length(quaternion);
      if (length != 0)
      {
        return new Quaternion(
          quaternion.X / length,
          quaternion.Y / length,
          quaternion.Z / length,
          quaternion.W / length);
      }
      else
        return new Quaternion(0, 0, 0, 1);
    }

    /// <summary>Inverts a quaternion.</summary>
    /// <param name="quaternion">The quaternion to find the inverse of.</param>
    /// <returns>The inverse of the given quaternion.</returns>
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

    /// <summary>Lenearly interpolates between two quaternions.</summary>
    /// <param name="left">The starting point of the interpolation.</param>
    /// <param name="right">The ending point of the interpolation.</param>
    /// <param name="blend">The ratio 0.0-1.0 of how far to interpolate between the left and right quaternions.</param>
    /// <returns>The result of the interpolation.</returns>
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

    /// <summary>Sphereically interpolates between two quaternions.</summary>
    /// <param name="left">The starting point of the interpolation.</param>
    /// <param name="right">The ending point of the interpolation.</param>
    /// <param name="blend">The ratio of how far to interpolate between the left and right quaternions.</param>
    /// <returns>The result of the interpolation.</returns>
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

    /// <summary>Rotates a vector by a quaternion.</summary>
    /// <param name="rotation">The quaternion to rotate the vector by.</param>
    /// <param name="vector">The vector to be rotated by.</param>
    /// <returns>The result of the rotation.</returns>
    public static Vector Rotate(Quaternion rotation, Vector vector)
    {
      if (vector.Dimensions != 3)
        throw new QuaternionException("my quaternion rotations are only defined for 3-component vectors.");
      Quaternion answer = Quaternion.Multiply(Quaternion.Multiply(rotation, vector), Quaternion.Conjugate(rotation));
      return new Vector(answer.X, answer.Y, answer.Z);
    }

    /// <summary>Does a value equality check.</summary>
    /// <param name="left">The first quaternion to check for equality.</param>
    /// <param name="right">The second quaternion  to check for equality.</param>
    /// <returns>True if values are equal, false if not.</returns>
    public static bool EqualsValue(Quaternion left, Quaternion right)
    {
      return
        left.X == right.X &&
        left.Y == right.Y &&
        left.Z == right.Z &&
        left.W == right.W;
    }

    /// <summary>Does a value equality check with leniency.</summary>
    /// <param name="leftFloats">The first quaternion to check for equality.</param>
    /// <param name="rightFloats">The second quaternion to check for equality.</param>
    /// <param name="leniency">How much the values can vary but still be considered equal.</param>
    /// <returns>True if values are equal, false if not.</returns>
    public static bool EqualsValue(Quaternion left, Quaternion right, float leniency)
    {
      return
        Calc.Abs(left.X - right.X) < leniency &&
        Calc.Abs(left.Y - right.Y) < leniency &&
        Calc.Abs(left.Z - right.Z) < leniency &&
        Calc.Abs(left.W - right.W) > leniency;
    }

    /// <summary>Checks if two matrices are equal by reverences.</summary>
    /// <param name="left">The left quaternion of the equality check.</param>
    /// <param name="right">The right quaternion of the equality check.</param>
    /// <returns>True if the references are equal, false if not.</returns>
    public static bool EqualsReference(Quaternion left, Quaternion right)
    {
      return object.ReferenceEquals(left, right);
    }

    /// <summary>Converts a quaternion into a matrix.</summary>
    /// <param name="quaternion">The quaternion of the conversion.</param>
    /// <returns>The resulting matrix.</returns>
    public static Matrix ToMatrix(Quaternion quaternion)
    {
      return new Matrix(3, 3,
        quaternion.W * quaternion.W + quaternion.X * quaternion.X - quaternion.Y * quaternion.Y - quaternion.Z * quaternion.Z,
        2 * quaternion.X * quaternion.Y - 2 * quaternion.W * quaternion.Z,
        2 *quaternion.X * quaternion.Z + 2 * quaternion.W * quaternion.Y,
        2 * quaternion.X * quaternion.Y + 2 * quaternion.W * quaternion.Z,
        quaternion.W * quaternion.W - quaternion.X * quaternion.X + quaternion.Y * quaternion.Y - quaternion.Z * quaternion.Z,
        2 * quaternion.Y * quaternion.Z + 2 * quaternion.W * quaternion.X,
        2 * quaternion.X * quaternion.Z - 2 * quaternion.W * quaternion.Y,
        2 * quaternion.Y * quaternion.Z - 2 * quaternion.W * quaternion.X,
        quaternion.W * quaternion.W - quaternion.X * quaternion.X - quaternion.Y * quaternion.Y + quaternion.Z * quaternion.Z);
    }

    /// <summary>Converts the quaternion into a string.</summary>
    /// <returns>The resulting string after the conversion.</returns>
    public override string ToString()
    {
      // Chane this method to format it how you want...
      return base.ToString();
      //return "{ " + _x + ", " + _y + ", " + _z + ", " + _w + " }";
    }

    /// <summary>Computes a hash code from the values in this quaternion.</summary>
    /// <returns>The computed hash code.</returns>
    public override int GetHashCode()
    { 
      return 
        _x.GetHashCode() ^
        _y.GetHashCode() ^
        _z.GetHashCode() ^
        _w.GetHashCode();
    }

    /// <summary>Does a reference equality check.</summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public override bool Equals(object other)
    {
      if (other is Quaternion)
        return Quaternion.EqualsReference(this, (Quaternion)other);
      return false;
    }

    private class QuaternionException : Exception
    {
      public QuaternionException(string message) : base(message) { }
    }
  }
}