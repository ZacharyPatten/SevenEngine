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

    /// <summary>Constructs a quaternion with the desired values.</summary>
    /// <param name="x">The x component of the quaternion.</param>
    /// <param name="y">The y component of the quaternion.</param>
    /// <param name="z">The z component of the quaternion.</param>
    /// <param name="w">The w component of the quaternion.</param>
    public Quaternion(float x, float y, float z, float w) { _x = x; _y = y; _z = z; _w = w; }

    /// <summary>Returns new Quaternion(0, 0, 0, 1).</summary>
    public static readonly Quaternion FactoryIdentity = new Quaternion(0, 0, 0, 1);

    /// <summary>Creates a quaternion from an axis and rotation.</summary>
    /// <param name="axis">The to create the quaternion from.</param>
    /// <param name="angle">The angle to create teh quaternion from.</param>
    /// <returns>The newly created quaternion.</returns>
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

    /// <summary>Adds two quaternions together.</summary>
    /// <param name="left">The first quaternion of the addition.</param>
    /// <param name="right">The second quaternion of the addition.</param>
    /// <returns>The result of the addition.</returns>
    public static Quaternion operator +(Quaternion left, Quaternion right) { return Quaternion.Add(left, right); }
    /// <summary>Subtracts two quaternions.</summary>
    /// <param name="left">The left quaternion of the subtraction.</param>
    /// <param name="right">The right quaternion of the subtraction.</param>
    /// <returns>The resulting quaternion after the subtraction.</returns>
    public static Quaternion operator -(Quaternion left, Quaternion right) { return Quaternion.Subtract(left, right); }
    /// <summary>Multiplies two quaternions together.</summary>
    /// <param name="left">The first quaternion of the multiplication.</param>
    /// <param name="right">The second quaternion of the multiplication.</param>
    /// <returns>The resulting quaternion after the multiplication.</returns>
    public static Quaternion operator *(Quaternion left, Quaternion right) { return Quaternion.Multiply(left, right); }
    /// <summary>Pre-multiplies a 3-component vector by a quaternion.</summary>
    /// <param name="left">The quaternion to pre-multiply the vector by.</param>
    /// <param name="vector">The vector to be multiplied.</param>
    /// <returns>The resulting quaternion of the multiplication.</returns>
    public static Quaternion operator *(Quaternion left, Vector right) { return Quaternion.Multiply(left, right); }
    /// <summary>Multiplies all the values of the quaternion by a scalar value.</summary>
    /// <param name="left">The quaternion of the multiplication.</param>
    /// <param name="right">The scalar of the multiplication.</param>
    /// <returns>The result of multiplying all the values in the quaternion by the scalar.</returns>
    public static Quaternion operator *(Quaternion left, float right) { return Quaternion.Multiply(left, right); }
    /// <summary>Multiplies all the values of the quaternion by a scalar value.</summary>
    /// <param name="left">The scalar of the multiplication.</param>
    /// <param name="right">The quaternion of the multiplication.</param>
    /// <returns>The result of multiplying all the values in the quaternion by the scalar.</returns>
    public static Quaternion operator *(float left, Quaternion right) { return Quaternion.Multiply(right, left); }
    /// <summary>Checks for equality by value. (beware float errors)</summary>
    /// <param name="left">The first quaternion of the equality check.</param>
    /// <param name="right">The second quaternion of the equality check.</param>
    /// <returns>true if the values were deemed equal, false if not.</returns>
    public static bool operator ==(Quaternion left, Quaternion right) { return Quaternion.Equals(left, right); }
    /// <summary>Checks for anti-equality by value. (beware float errors)</summary>
    /// <param name="left">The first quaternion of the anti-equality check.</param>
    /// <param name="right">The second quaternion of the anti-equality check.</param>
    /// <returns>false if the values were deemed equal, true if not.</returns>
    public static bool operator !=(Quaternion left, Quaternion right) { return !Quaternion.Equals(left, right); }

    /// <summary>Computes the length of quaternion.</summary>
    /// <returns>The length of the given quaternion.</returns>
    public float Length() { return Quaternion.Length(this); }
    /// <summary>Computes the length of a quaternion, but doesn't square root it
    /// for optimization possibilities.</summary>
    /// <returns>The squared length of the given quaternion.</returns>
    public float LengthSquared() { return Quaternion.LengthSquared(this); }
    /// <summary>Gets the conjugate of the quaternion.</summary>
    /// <returns>The conjugate of teh given quaternion.</returns>
    public Quaternion Conjugate() { return Quaternion.Conjugate(this); }
    /// <summary>Adds two quaternions together.</summary>
    /// <param name="right">The second quaternion of the addition.</param>
    /// <returns>The result of the addition.</returns>
    public Quaternion Add(Quaternion right) { return Quaternion.Add(this, right); }
    /// <summary>Subtracts two quaternions.</summary>
    /// <param name="right">The right quaternion of the subtraction.</param>
    /// <returns>The resulting quaternion after the subtraction.</returns>
    public Quaternion Subtract(Quaternion right) { return Quaternion.Subtract(this, right); }
    /// <summary>Multiplies two quaternions together.</summary>
    /// <param name="right">The second quaternion of the multiplication.</param>
    /// <returns>The resulting quaternion after the multiplication.</returns>
    public Quaternion Multiply(Quaternion right) { return Quaternion.Multiply(this, right); }
    /// <summary>Multiplies all the values of the quaternion by a scalar value.</summary>
    /// <param name="right">The scalar of the multiplication.</param>
    /// <returns>The result of multiplying all the values in the quaternion by the scalar.</returns>
    public Quaternion Multiply(float right) { return Quaternion.Multiply(this, right); }
    /// <summary>Pre-multiplies a 3-component vector by a quaternion.</summary>
    /// <param name="right">The vector to be multiplied.</param>
    /// <returns>The resulting quaternion of the multiplication.</returns>
    public Quaternion Multiply(Vector vector) { return Quaternion.Multiply(this, vector); }
    /// <summary>Normalizes the quaternion.</summary>
    /// <returns>The normalization of the given quaternion.</returns>
    public Quaternion Normalize() { return Quaternion.Normalize(this); }
    /// <summary>Inverts a quaternion.</summary>
    /// <returns>The inverse of the given quaternion.</returns>
    public Quaternion Invert() { return Quaternion.Invert(this); }
    /// <summary>Lenearly interpolates between two quaternions.</summary>
    /// <param name="right">The ending point of the interpolation.</param>
    /// <param name="blend">The ratio 0.0-1.0 of how far to interpolate between the left and right quaternions.</param>
    /// <returns>The result of the interpolation.</returns>
    public Quaternion Lerp(Quaternion right, float blend) { return Quaternion.Lerp(this, right, blend); }
    /// <summary>Sphereically interpolates between two quaternions.</summary>
    /// <param name="right">The ending point of the interpolation.</param>
    /// <param name="blend">The ratio of how far to interpolate between the left and right quaternions.</param>
    /// <returns>The result of the interpolation.</returns>
    public Quaternion Slerp(Quaternion right, float blend) { return Quaternion.Slerp(this, right, blend); }
    /// <summary>Rotates a vector by a quaternion.</summary>
    /// <param name="vector">The vector to be rotated by.</param>
    /// <returns>The result of the rotation.</returns>
    public Vector Rotate(Vector vector) { return Quaternion.Rotate(this, vector); }
    /// <summary>Does a value equality check.</summary>
    /// <param name="right">The second quaternion  to check for equality.</param>
    /// <returns>True if values are equal, false if not.</returns>
    public bool EqualsValue(Quaternion right) { return Quaternion.EqualsValue(this, right); }
    /// <summary>Does a value equality check with leniency.</summary>
    /// <param name="right">The second quaternion to check for equality.</param>
    /// <param name="leniency">How much the values can vary but still be considered equal.</param>
    /// <returns>True if values are equal, false if not.</returns>
    public bool EqualsValue(Quaternion right, float leniency) { return Quaternion.EqualsValue(this, right, leniency); }
    /// <summary>Checks if two matrices are equal by reverences.</summary>
    /// <param name="right">The right quaternion of the equality check.</param>
    /// <returns>True if the references are equal, false if not.</returns>
    public bool EqualsReference(Quaternion right) { return Quaternion.EqualsReference(this, right); }
    /// <summary>Converts a quaternion into a matrix.</summary>
    /// <returns>The resulting matrix.</returns>
    public Matrix ToMatrix() { return Quaternion.ToMatrix(this); }

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
    /// <param name="right">The vector to be multiplied.</param>
    /// <returns>The resulting quaternion of the multiplication.</returns>
    public static Quaternion Multiply(Quaternion left, Vector right)
    {
      if (right.Dimensions == 3)
      {
        return new Quaternion(
          left.W * right.X + left.Y * right.Z - left.Z * right.Y,
          left.W * right.Y + left.Z * right.X - left.X * right.Z,
          left.W * right.Z + left.X * right.Y - left.Y * right.X,
          -left.X * right.X - left.Y * right.Y - left.Z * right.Z);
      }
      else
        throw new QuaternionException("my quaternion rotations are only defined for 3-component vectors.");
    }

    /// <summary>Normalizes the quaternion.</summary>
    /// <param name="quaternion">The quaternion to normalize.</param>
    /// <returns>The normalization of the given quaternion.</returns>
    public static Quaternion Normalize(Quaternion quaternion)
    {
      float normalizer =
        Calc.SquareRoot(
          quaternion.X * quaternion.X +
          quaternion.Y * quaternion.Y +
          quaternion.Z * quaternion.Z +
          quaternion.W * quaternion.W);
      if (normalizer != 0)
      {
        normalizer = 1.0f / normalizer;
        return new Quaternion(
          quaternion.X * normalizer,
          quaternion.Y * normalizer,
          quaternion.Z * normalizer,
          quaternion.W * normalizer);
      }
      else
        return new Quaternion(0, 0, 0, 1);
    }

    /// <summary>Inverts a quaternion.</summary>
    /// <param name="quaternion">The quaternion to find the inverse of.</param>
    /// <returns>The inverse of the given quaternion.</returns>
    public static Quaternion Invert(Quaternion quaternion)
    {
      // EQUATION: invert = quaternion.Conjugate()).Normalized()
      float normalizer =
        quaternion.X * quaternion.X +
        quaternion.Y * quaternion.Y +
        quaternion.Z * quaternion.Z +
        quaternion.W * quaternion.W;
      if (normalizer == 0.0)
        return new Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
      normalizer = 1.0f / normalizer;
      return new Quaternion(
        -quaternion.X * normalizer,
        -quaternion.Y * normalizer,
        -quaternion.Z * normalizer,
        quaternion.W * normalizer);
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
        return Quaternion.FactoryIdentity;
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
        return Quaternion.FactoryIdentity;
    }

    /// <summary>Rotates a vector by a quaternion.</summary>
    /// <param name="rotation">The quaternion to rotate the vector by.</param>
    /// <param name="vector">The vector to be rotated by.</param>
    /// <returns>The result of the rotation.</returns>
    public static Vector Rotate(Quaternion rotation, Vector vector)
    {
      if (vector.Dimensions != 3)
        throw new QuaternionException("my quaternion rotations are only defined for 3-component vectors.");
      
      float[] vectorFloats = vector.Floats;
      // EQUATION: t = 2 * cross(q.xyz, v)
      Vector temp = new Vector(
        (rotation.Y * vectorFloats[2] - rotation.Z * vectorFloats[1]) * 2,
        (rotation.Z * vectorFloats[0] - rotation.X * vectorFloats[2]) * 2,
        (rotation.X * vectorFloats[1] - rotation.Y * vectorFloats[0]) * 2);
      // EQUATION: v' = v + q.w * t + cross(q.xyz, t)
      return new Vector(
        vector.X + (rotation.W * temp.X) + (rotation.Y * temp.Z - rotation.Z * temp.Y),
        vector.Y + (rotation.W * temp.Y) + (rotation.Z * temp.X - rotation.X * temp.Z),
        vector.Z + (rotation.W * temp.Z) + (rotation.X * temp.Y - rotation.Y * temp.X));

      //// OLD VERSION: 
      //// EQUATION: v' = qvq'
      //Quaternion answer = Quaternion.Multiply(Quaternion.Multiply(rotation, vector), Quaternion.Conjugate(rotation));
      //return new Vector(answer.X, answer.Y, answer.Z);
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
    /// <param name="left">The first quaternion to check for equality.</param>
    /// <param name="right">The second quaternion to check for equality.</param>
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