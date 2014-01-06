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
  /// <summary>Represents a vector with an arbitrary number of components.</summary>
  public class Vector
  {
    private float[] _vector;

    /// <summary>Sane as accessing index 0.</summary>
    public float X
    {
      get { return _vector[0]; }
      set { _vector[0] = value; }
    }

    /// <summary>Same as accessing index 1.</summary>
    public float Y
    {
      get { try { return _vector[1]; } catch { throw new VectorException("vector does not contains a y component."); } }
      set { try { _vector[1] = value; } catch { throw new VectorException("vector does not contains a y component."); } }
    }

    /// <summary>Same as accessing index 2.</summary>
    public float Z
    {
      get { try { return _vector[2]; } catch { throw new VectorException("vector does not contains a z component."); } }
      set { try { _vector[2] = value; } catch { throw new VectorException("vector does not contains a z component."); } }
    }

    /// <summary>Same as accessing index 3.</summary>
    public float W
    {
      get { try { return _vector[3]; } catch { throw new VectorException("vector does not contains a w component."); } }
      set { try { _vector[3] = value; } catch { throw new VectorException("vector does not contains a w component."); } }
    }

    /// <summary>Gives you direct access to the values in this vector.</summary>
    public float[] Floats { get { return _vector; } }

    /// <summary>The number of components in this vector.</summary>
    public int Dimensions { get { return _vector.Length; } }

    /// <summary>Allows indexed access to this vector.</summary>
    /// <param name="index">The index to access.</param>
    /// <returns>The value of the given index.</returns>
    public float this[int index]
    {
      get { try { return _vector[index]; } catch { throw new VectorException("index out of bounds."); } }
      set { try { _vector[index] = value; } catch { throw new VectorException("index out of bounds."); } }
    }

    /// <summary>Creates a new vector with the given number of components.</summary>
    /// <param name="dimensions">The number of dimensions this vector will have.</param>
    public Vector(int dimensions) { try { _vector = new float[dimensions]; } catch { throw new VectorException("invalid dimensions on vector contruction."); } }

    /// <summary>Creates a vector out of the given values.</summary>
    /// <param name="vector">The values to initialize the vector to.</param>
    public Vector(params float[] vector)
    {
      float[] floats = new float[vector.Length];
      Buffer.BlockCopy(vector, 0, floats, 0, floats.Length * sizeof(float));
      _vector = floats;
    }

    /// <summary>Creates a vector with the given number of components with the values initialized to zeroes.</summary>
    /// <param name="dimensions">The number of components in the vector.</param>
    /// <returns>The newly constructed vector.</returns>
    public static Vector FactoryZero(int dimensions) { return new Vector(dimensions); }
    /// <summary>Creates a vector with the given number of components with the values initialized to ones.</summary>
    /// <param name="dimensions">The number of components in the vector.</param>
    /// <returns>The newly constructed vector.</returns>
    public static Vector FactoryOne(int dimensions) { return new Vector(new float[dimensions]); }

    /// <summary>Adds two vectors together.</summary>
    /// <param name="left">The first vector of the addition.</param>
    /// <param name="right">The second vector of the addition.</param>
    /// <returns>The result of the addition.</returns>
    public static Vector operator +(Vector left, Vector right) { return Vector.Add(left, right); }
    /// <summary>Subtracts two vectors.</summary>
    /// <param name="left">The left operand of the subtraction.</param>
    /// <param name="right">The right operand of the subtraction.</param>
    /// <returns>The result of the subtraction.</returns>
    public static Vector operator -(Vector left, Vector right) { return Vector.Subtract(left, right); }
    /// <summary>Negates a vector.</summary>
    /// <param name="vector">The vector to negate.</param>
    /// <returns>The result of the negation.</returns>
    public static Vector operator -(Vector vector) { return Vector.Negate(vector); }
    /// <summary>Multiplies all the values in a vector by a scalar.</summary>
    /// <param name="left">The vector to have all its values multiplied.</param>
    /// <param name="right">The scalar to multiply all the vector values by.</param>
    /// <returns>The result of the multiplication.</returns>
    public static Vector operator *(Vector left, float right) { return Vector.Multiply(left, right); }
    /// <summary>Multiplies all the values in a vector by a scalar.</summary>
    /// <param name="left">The scalar to multiply all the vector values by.</param>
    /// <param name="right">The vector to have all its values multiplied.</param>
    /// <returns>The result of the multiplication.</returns>
    public static Vector operator *(float left, Vector right) { return Vector.Multiply(right, left); }
    /// <summary>Divides all the values in the vector by a scalar.</summary>
    /// <param name="left">The vector to have its values divided.</param>
    /// <param name="right">The scalar to divide all the vectors values by.</param>
    /// <returns>The vector after the divisions.</returns>
    public static Vector operator /(Vector left, float right) { return Vector.Divide(left, right); }
    /// <summary>Does an equality check by value. (warning for float errors)</summary>
    /// <param name="left">The first vector of the equality check.</param>
    /// <param name="right">The second vector of the equality check.</param>
    /// <returns>true if the values are equal, false if not.</returns>
    public static bool operator ==(Vector left, Vector right) { return Vector.EqualsValue(left, right); }
    /// <summary>Does an anti-equality check by value. (warning for float errors)</summary>
    /// <param name="left">The first vector of the anit-equality check.</param>
    /// <param name="right">The second vector of the anti-equality check.</param>
    /// <returns>true if the values are not equal, false if they are.</returns>
    public static bool operator !=(Vector left, Vector right) { return !Vector.EqualsValue(left, right); }
    /// <summary>Automatically converts a vector into a matrix.</summary>
    /// <param name="vector">The vector of the conversion.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator Matrix(Vector vector) { return Vector.ToMatrix(vector); }

    /// <summary>Adds two vectors together.</summary>
    /// <param name="right">The vector to add to this one.</param>
    /// <returns>The result of the vector.</returns>
    public Vector Add(Vector right) { return Vector.Add(this, right); }
    /// <summary>Negates this vector.</summary>
    /// <returns>The result of the negation.</returns>
    public Vector Negate() { return Vector.Negate(this); }
    /// <summary>Subtracts another vector from this one.</summary>
    /// <param name="right">The vector to subtract from this one.</param>
    /// <returns>The result of the subtraction.</returns>
    public Vector Subtract(Vector right) { return Vector.Subtract(this, right); }
    /// <summary>Multiplies the values in this vector by a scalar.</summary>
    /// <param name="right">The scalar to multiply these values by.</param>
    /// <returns>The result of the multiplications</returns>
    public Vector Multiply(float right) { return Vector.Multiply(this, right); }
    /// <summary>Divides all the values in this vector by a scalar.</summary>
    /// <param name="right">The scalar to divide the values of the vector by.</param>
    /// <returns>The resulting vector after teh divisions.</returns>
    public Vector Divide(float right) { return Vector.Divide(this, right); }
    /// <summary>Computes the dot product between this vector and another.</summary>
    /// <param name="right">The second vector of the dot product operation.</param>
    /// <returns>The result of the dot product.</returns>
    public float DotProduct(Vector right) { return Vector.DotProduct(this, right); }
    /// <summary>Computes the cross product between this vector and another.</summary>
    /// <param name="right">The second vector of the dot product operation.</param>
    /// <returns>The result of the dot product operation.</returns>
    public Vector CrossProduct(Vector right) { return Vector.CrossProduct(this, right); }
    /// <summary>Normalizes this vector.</summary>
    /// <returns>The result of the normalization.</returns>
    public Vector Normalize() { return Vector.Normalize(this); }
    /// <summary>Computes the length of this vector.</summary>
    /// <returns>The length of this vector.</returns>
    public float Length() { return Vector.Length(this); }
    /// <summary>Computes the length of this vector, but doesn't square root it for 
    /// possible optimization purposes.</summary>
    /// <returns>The squared length of the vector.</returns>
    public float LengthSquared() { return Vector.LengthSquared(this); }
    /// <summary>Check for equality by value.</summary>
    /// <param name="right">The other vector of the equality check.</param>
    /// <returns>true if the values were equal, false if not.</returns>
    public bool EqualsValue(Vector right) { return Vector.EqualsValue(this, right); }
    /// <summary>Checks for equality by value with some leniency.</summary>
    /// <param name="right">The other vector of the equality check.</param>
    /// <param name="leniency">The ammount the values can differ but still be considered equal.</param>
    /// <returns>true if the values were cinsidered equal, false if not.</returns>
    public bool EqualsValue(Vector right, float leniency) { return Vector.EqualsValue(this, right, leniency); }
    /// <summary>Checks for equality by reference.</summary>
    /// <param name="right">The other vector of the equality check.</param>
    /// <returns>true if the references are equal, false if not.</returns>
    public bool EqualsReference(Vector right) { return Vector.EqualsReference(this, right); }
    /// <summary>Rotates this vector by quaternon values.</summary>
    /// <param name="angle">The amount of rotation about the axis.</param>
    /// <param name="x">The x component deterniming the axis of rotation.</param>
    /// <param name="y">The y component determining the axis of rotation.</param>
    /// <param name="z">The z component determining the axis of rotation.</param>
    /// <returns>The resulting vector after the rotation.</returns>
    public Vector RotateBy(float angle, float x, float y, float z) { return Vector.RotateBy(this, angle, x, y, z); }
    /// <summary>Computes the linear interpolation between two vectors.</summary>
    /// <param name="right">The ending vector of the interpolation.</param>
    /// <param name="blend">The ratio 0.0 to 1.0 of the interpolation between the start and end.</param>
    /// <returns>The result of the interpolation.</returns>
    public Vector Lerp(Vector right, float blend) { return Vector.Lerp(this, right, blend); }
    /// <summary>Sphereically interpolates between two vectors.</summary>
    /// <param name="right">The ending vector of the interpolation.</param>
    /// <param name="blend">The ratio 0.0 to 1.0 defining the interpolation distance between the two vectors.</param>
    /// <returns>The result of the slerp operation.</returns>
    public Vector Slerp(Vector right, float blend) { return Vector.Slerp(this, right, blend); }
    /// <summary>Rotates a vector by a quaternion.</summary>
    /// <param name="rotation">The quaternion to rotate the 3-component vector by.</param>
    /// <returns>The result of the rotation.</returns>
    public Vector RotateBy(Quaternion rotation) { return Vector.RotateBy(this, rotation); }

    /// <summary>Adds two vectors together.</summary>
    /// <param name="leftFloats">The first vector of the addition.</param>
    /// <param name="rightFloats">The second vector of the addiiton.</param>
    /// <returns>The result of the addiion.</returns>
    public static Vector Add(Vector left, Vector right)
    {
      float[] leftFloats = left.Floats;
      float[] rightFloats = right.Floats;
      if (leftFloats.Length != rightFloats.Length)
        throw new VectorException("invalid dimensions on vector addition.");
      Vector result = new Vector(leftFloats.Length);
      float[] resultFloats = result.Floats;
      for (int i = 0; i < leftFloats.Length; i++)
        resultFloats[i] = leftFloats[i] + rightFloats[i];
      return result;
    }

    /// <summary>Negates all the values in a vector.</summary>
    /// <param name="vector">The vector to have its values negated.</param>
    /// <returns>The result of the negations.</returns>
    public static Vector Negate(Vector vector)
    {
      float[] floats = vector.Floats;
      Vector result = new Vector(floats.Length);
      float[] resultFloats = result.Floats;
      for (int i = 0; i < floats.Length; i++)
        resultFloats[i] = -floats[i];
      return result;
    }

    /// <summary>Subtracts two vectors.</summary>
    /// <param name="left">The left vector of the subtraction.</param>
    /// <param name="right">The right vector of the subtraction.</param>
    /// <returns>The result of the vector subtracton.</returns>
    public static Vector Subtract(Vector left, Vector right)
    {
      float[] leftFloats = left.Floats;
      float[] rightFloats = right.Floats;
      Vector result = new Vector(leftFloats.Length);
      float[] resultFloats = result.Floats;
      if (leftFloats.Length != rightFloats.Length)
        throw new VectorException("invalid dimensions on vector subtraction.");
      for (int i = 0; i < leftFloats.Length; i++)
        resultFloats[i] = leftFloats[i] - rightFloats[i];
      return result;
    }

    /// <summary>Multiplies all the components of a vecotr by a scalar.</summary>
    /// <param name="left">The vector to have the components multiplied by.</param>
    /// <param name="right">The scalars to multiply the vector components by.</param>
    /// <returns>The result of the multiplications.</returns>
    public static Vector Multiply(Vector left, float right)
    {
      float[] leftFloats = left.Floats;
      Vector result = new Vector(leftFloats.Length);
      float[] resultFloats = result.Floats;
      for (int i = 0; i < leftFloats.Length; i++)
        resultFloats[i] = leftFloats[i] * right;
      return result;
    }

    /// <summary>Divides all the components of a vector by a scalar.</summary>
    /// <param name="left">The vector to have the components divided by.</param>
    /// <param name="right">The scalar to divide the vector components by.</param>
    /// <returns>The resulting vector after teh divisions.</returns>
    public static Vector Divide(Vector left, float right)
    {
      float[] floats = left.Floats;
      Vector result = new Vector(floats.Length);
      float[] resultFloats = result.Floats;
      int arrayLength = floats.Length;
      for (int i = 0; i < arrayLength; i++)
        resultFloats[i] = floats[i] / right;
      return result;
    }

    /// <summary>Computes the dot product between two vectors.</summary>
    /// <param name="left">The first vector of the dot product operation.</param>
    /// <param name="right">The second vector of the dot product operation.</param>
    /// <returns>The result of the dot product operation.</returns>
    public static float DotProduct(Vector left, Vector right)
    {
      float[] leftFloats = left.Floats;
      float[] rightFloats = right.Floats;
      if (leftFloats.Length != rightFloats.Length)
        throw new VectorException("invalid dimensions on vector dot product.");
      float result = 0;
      for (int i = 0; i < leftFloats.Length; i++)
        result += (leftFloats[i] * rightFloats[i]);
      return result;
    }

    /// <summary>Computes teh cross product of two vectors.</summary>
    /// <param name="left">The first vector of the cross product operation.</param>
    /// <param name="right">The second vector of the cross product operation.</param>
    /// <returns>The result of the cross product operation.</returns>
    public static Vector CrossProduct(Vector left, Vector right)
    {
      if (left.Dimensions != right.Dimensions)
        throw new VectorException("invalid cross product !(left.Dimensions == right.Dimensions)");
      if (left.Dimensions == 3 || left.Dimensions == 4)
      {
        return new Vector(
          left[1] * right[2] - left[2] * right[1],
          left[2] * right[0] - left[0] * right[2],
          left[0] * right[1] - left[1] * right[0]);
      }
      throw new VectorException("my cross product function is only defined for 3-component vectors.");
    }

    /// <summary>Normalizes a vector.</summary>
    /// <param name="vector">The vector to normalize.</param>
    /// <returns>The result of the normalization.</returns>
    public static Vector Normalize(Vector vector)
    {
      float length = Vector.Length(vector);
      if (length != 0.0)
      {
        float[] floats = vector.Floats;
        Vector result = new Vector(floats.Length);
        float[] resultFloats = result.Floats;
        int arrayLength = floats.Length;
        for (int i = 0; i < arrayLength; i++)
          resultFloats[i] = floats[i] / length;
        return result;
      }
      else
        return new Vector(vector.Dimensions);
    }

    /// <summary>Computes the length of a vector.</summary>
    /// <param name="vector">The vector to calculate the length of.</param>
    /// <returns>The computed length of the vector.</returns>
    public static float Length(Vector vector)
    {
      float[] floats = vector.Floats;
      float result = 0;
      int arrayLength = floats.Length;
      for (int i = 0; i < arrayLength; i++)
        result += (floats[i] * floats[i]);
      return Calc.SquareRoot(result);
    }

    /// <summary>Computes the length of a vector but doesn't square root it for efficiency (length remains squared).</summary>
    /// <param name="vector">The vector to compute the length squared of.</param>
    /// <returns>The computed length squared of the vector.</returns>
    public static float LengthSquared(Vector vector)
    {
      float[] floats = vector.Floats;
      float result = 0;
      int arrayLength = floats.Length;
      for (int i = 0; i < arrayLength; i++)
        result += (floats[i] * floats[i]);
      return result;
    }

    /// <summary>Computes the angle between two vectors.</summary>
    /// <param name="first">The first vector to determine the angle between.</param>
    /// <param name="second">The second vector to determine the angle between.</param>
    /// <returns>The angle between the two vectors in radians.</returns>
    public static float Angle(Vector first, Vector second)
    {
      return Calc.ArcCos(Vector.DotProduct(first, second) / (Vector.Length(first) * Vector.Length(second)));
    }
    
    /// <summary>Rotates a vector by the specified quaternion values.</summary>
    /// <param name="vector">The vector to rotate.</param>
    /// <param name="angle">The angle of the rotation.</param>
    /// <param name="x">The x component of the axis vector to rotate about.</param>
    /// <param name="y">The y component of the axis vector to rotate about.</param>
    /// <param name="z">The z component of the axis vector to rotate about.</param>
    /// <returns>The result of the rotation.</returns>
    public static Vector RotateBy(Vector vector, float angle, float x, float y, float z)
    {
      if (vector.Dimensions == 3)
      {
        float[] floats = vector.Floats;
        // Note: the angle is in radians
        float sinHalfAngle = Calc.Sin(angle / 2);
        float cosHalfAngle = Calc.Cos(angle / 2);
        x *= sinHalfAngle;
        y *= sinHalfAngle;
        z *= sinHalfAngle;
        float x2 = cosHalfAngle * floats[0] + y * floats[2] - z * floats[1];
        float y2 = cosHalfAngle * floats[1] + z * floats[0] - x * floats[2];
        float z2 = cosHalfAngle * floats[2] + x * floats[1] - y * floats[0];
        float w2 = -x * floats[0] - y * floats[1] - z * floats[2];
        return new Vector(
          x * w2 + cosHalfAngle * x2 + y * z2 - z * y2,
          y * w2 + cosHalfAngle * y2 + z * x2 - x * z2,
          z * w2 + cosHalfAngle * z2 + x * y2 - y * x2);
      }
      throw new VectorException("my RotateBy() function is only defined for 3-component vectors.");
    }

    /// <summary>Rotates a vector by a quaternion.</summary>
    /// <param name="vector">The vector to rotate.</param>
    /// <param name="rotation">The quaternion to rotate the 3-component vector by.</param>
    /// <returns>The result of the rotation.</returns>
    public static Vector RotateBy(Vector vector, Quaternion rotation)
    {
      if (vector.Dimensions == 3)
      {
        Quaternion answer = Quaternion.Multiply(Quaternion.Multiply(rotation, vector), Quaternion.Conjugate(rotation));
        return new Vector(answer.X, answer.Y, answer.Z);
      }
      else
        throw new VectorException("my quaternion rotations are only defined for 3-component vectors.");
    }

    /// <summary>Computes the linear interpolation between two vectors.</summary>
    /// <param name="left">The starting vector of the interpolation.</param>
    /// <param name="right">The ending vector of the interpolation.</param>
    /// <param name="blend">The ratio 0.0 to 1.0 of the interpolation between the start and end.</param>
    /// <returns>The result of the interpolation.</returns>
    public static Vector Lerp(Vector left, Vector right, float blend)
    {
      float[] leftFloats = left.Floats;
      float[] rightFloats = right.Floats;
      if (blend < 0 || blend > 1.0f)
        throw new VectorException("invalid lerp blend value: (blend < 0.0f || blend > 1.0f).");
      if (leftFloats.Length != rightFloats.Length)
        throw new VectorException("invalid lerp matrix length: (left.Dimensions != right.Dimensions)");
      Vector result = new Vector(leftFloats.Length);
      float[] resultFloats = result.Floats;
      for (int i = 0; i < leftFloats.Length; i++)
        resultFloats[i] = leftFloats[i] + blend * (rightFloats[i] - leftFloats[i]);
      return result;
    }

    /// <summary>Sphereically interpolates between two vectors.</summary>
    /// <param name="left">The starting vector of the interpolation.</param>
    /// <param name="right">The ending vector of the interpolation.</param>
    /// <param name="blend">The ratio 0.0 to 1.0 defining the interpolation distance between the two vectors.</param>
    /// <returns>The result of the slerp operation.</returns>
    public static Vector Slerp(Vector left, Vector right, float blend)
    {
      if (blend < 0 || blend > 1.0f)
        throw new VectorException("invalid slerp blend value: (blend < 0.0f || blend > 1.0f).");
      float dot = Calc.Clamp(Vector.DotProduct(left, right), -1.0f, 1.0f);
      float theta = Calc.ArcCos(dot) * blend;
      return left * Calc.Cos(theta) + (right - left * dot).Normalize() * Calc.Sin(theta);
    }

    /// <summary>Interpolates between three vectors using barycentric coordinates.</summary>
    /// <param name="a">The first vector of the interpolation.</param>
    /// <param name="b">The second vector of the interpolation.</param>
    /// <param name="c">The thrid vector of the interpolation.</param>
    /// <param name="u">The "U" value of the barycentric interpolation equation.</param>
    /// <param name="v">The "V" value of the barycentric interpolation equation.</param>
    /// <returns>The resulting vector of the barycentric interpolation.</returns>
    public static Vector Blerp(Vector a, Vector b, Vector c, float u, float v)
    {
      return Vector.Add(Vector.Add(Vector.Multiply(Vector.Subtract(b, a), u), Vector.Multiply(Vector.Subtract(c, a), v)), a);
    }

    /// <summary>Does a value equality check.</summary>
    /// <param name="left">The first vector to check for equality.</param>
    /// <param name="right">The second vector  to check for equality.</param>
    /// <returns>True if values are equal, false if not.</returns>
    public static bool EqualsValue(Vector left, Vector right)
    {
      float[] leftFloats = left.Floats;
      float[] rightFloats = right.Floats;
      if (leftFloats.GetLength(0) != rightFloats.GetLength(0))
        return false;
      for (int i = 0; i < leftFloats.GetLength(0); i++)
          if (leftFloats[i] != rightFloats[i])
            return false;
      return true;
    }

    /// <summary>Does a value equality check with leniency.</summary>
    /// <param name="leftFloats">The first vector to check for equality.</param>
    /// <param name="rightFloats">The second vector to check for equality.</param>
    /// <param name="leniency">How much the values can vary but still be considered equal.</param>
    /// <returns>True if values are equal, false if not.</returns>
    public static bool EqualsValue(Vector left, Vector right, float leniency)
    {
      float[] leftFloats = left.Floats;
      float[] rightFloats = right.Floats;
      if (leftFloats.GetLength(0) != rightFloats.GetLength(0))
        return false;
      for (int i = 0; i < leftFloats.GetLength(0); i++)
          if (Calc.Abs(leftFloats[i] - rightFloats[i]) > leniency)
            return false;
      return true;
    }

    /// <summary>Checks if two matrices are equal by reverences.</summary>
    /// <param name="left">The left vector of the equality check.</param>
    /// <param name="right">The right vector of the equality check.</param>
    /// <returns>True if the references are equal, false if not.</returns>
    public static bool EqualsReference(Vector left, Vector right)
    {
      return object.ReferenceEquals(left, right) ||
        // also, if they point to the same float array
        object.ReferenceEquals(left.Floats, right.Floats);
    }

    /// <summary>Converts the vector into a matrix.</summary>
    /// <param name="vector">The vecotr to convert.</param>
    /// <returns>The matrix of the conversion.</returns>
    public static Matrix ToMatrix(Vector vector)
    {
      return new Matrix(vector.Dimensions, 1, vector.Floats);
    }

    /// <summary>Prints out a string representation of this matrix.</summary>
    /// <returns>A string representing this matrix.</returns>
    public override string ToString()
    {
      // Change this method to what ever you want.
      return base.ToString();
    }

    /// <summary>Computes a hash code from the values of this matrix.</summary>
    /// <returns>A hash code for the matrix.</returns>
    public override int GetHashCode()
    {
      // return base.GetHashCode();
      int hash = _vector[0].GetHashCode();
      for (int i = 1; i < _vector.Length; i++)
        hash ^= _vector[i].GetHashCode();
      return hash;
    }

    /// <summary>Does an equality check by reference.</summary>
    /// <param name="right">The object to compare to.</param>
    /// <returns>True if the references are equal, false if not.</returns>
    public override bool Equals(object right)
    {
      if (!(right is Vector)) return false;
      return Vector.EqualsReference(this, (Vector)right);
    }

    private class VectorException : Exception
    {
      public VectorException(string message) : base(message) { }
    }
  }

  // This is my old version of vectors. It only supported 3-component vectors.
  // The newer version supports arbitrary sized  vectors as well as multiplications
  // with arbitrary sized matrices. Learn your linear algebra!
  #region Vector-OLD
  ///// <summary>Implements a 3-component (x, y, z) vector matrix.</summary>
  //public class Vector37
  //{
  //  protected float _x, _y, _z;

  //  public float X { get { return _x; } set { _x = value; } }
  //  public float Y { get { return _y; } set { _y = value; } }
  //  public float Z { get { return _z; } set { _z = value; } }

  //  public Vector37(float x, float y, float z) { _x = x; _y = y; _z = z; }

  //  public static readonly Vector37 FactoryZero = new Vector37(0, 0, 0);

  //  public static Vector37 operator +(Vector37 left, Vector37 right) { return Vector37.Add(left, right); }
  //  public static Vector37 operator -(Vector37 left, Vector37 right) { return Vector37.Subtract(left, right); }
  //  public static Vector37 operator -(Vector37 vector) { return Vector37.Negate(vector); }
  //  public static Vector37 operator *(Vector37 left, float right) { return Vector37.Multiply(left, right); }
  //  public static Vector37 operator *(float left, Vector37 right) { return Vector37.Multiply(right, left); }
  //  public static Vector37 operator /(Vector37 left, float right) { return Vector37.Divide(left, right); }
  //  public static bool operator ==(Vector37 left, Vector37 right) { return Vector37.Equals(left, right); }
  //  public static bool operator !=(Vector37 left, Vector37 right) { return !Vector37.Equals(left, right); }
  //  public static implicit operator float[,](Vector37 vector) { return Vector37.ToFloats(vector); }
  //  public static implicit operator Matrix(Vector37 vector) { return Vector37.ToMatrix(vector); }

  //  public Vector37 Add(Vector37 right) { return Vector37.Add(this, right); }
  //  public Vector37 Negate() { return Vector37.Negate(this); }
  //  public Vector37 Subtract(Vector37 right) { return Vector37.Subtract(this, right); }
  //  public Vector37 Multiply(float right) { return Vector37.Multiply(this, right); }
  //  public Vector37 Divide(float right) { return Vector37.Divide(this, right); }
  //  public float DotProduct(Vector37 right) { return Vector37.DotProduct(this, right); }
  //  public Vector37 CrossProduct(Vector37 right) { return Vector37.CrossProduct(this, right); }
  //  public Vector37 Normalize() { return Vector37.Normalize(this); }
  //  public float Length() { return Vector37.Length(this); }
  //  public float LengthSquared() { return Vector37.LengthSquared(this); }
  //  public bool Equals(Vector37 right) { return Vector37.Equals(this, right); }
  //  public bool Equals(Vector37 right, float leniency) { return Vector37.Equals(this, right, leniency); }
  //  public Vector37 RotateBy(float angle, float x, float y, float z) { return Vector37.RotateBy(this, angle, x, y, z); }
  //  public Vector37 Lerp(Vector37 right, float blend) { return Vector37.InterpolateLinear(this, right, blend); }
  //  public Vector37 Slerp(Vector37 right, float blend) { return Vector37.InterpolateSphereical(this, right, blend); }
  //  public Vector37 RotateBy(Quaternion rotation) { return Vector37.RotateBy(this, rotation); }

  //  public static Vector37 Add(Vector37 left, Vector37 right)
  //  {
  //    return new Vector37(
  //      left.X + right.X,
  //      left.Y + right.Y,
  //      left.Z + right.Z);
  //  }

  //  public static Vector37 Negate(Vector37 vector)
  //  {
  //    return new Vector37(
  //      -vector.X,
  //      -vector.Y,
  //      -vector.Z);
  //  }

  //  public static Vector37 Subtract(Vector37 left, Vector37 right)
  //  {
  //    return new Vector37(
  //      left.X - right.X,
  //      left.Y - right.Y,
  //      left.Z - right.Z);
  //  }

  //  public static Vector37 Multiply(Vector37 left, float right)
  //  {
  //    return new Vector37(
  //      left.X * right,
  //      left.Y * right,
  //      left.Z * right);
  //  }

  //  public static Vector37 Divide(Vector37 left, float right)
  //  {
  //    return new Vector37(
  //      left.X / right,
  //      left.Y / right,
  //      left.Z / right);
  //  }

  //  public static float DotProduct(Vector37 left, Vector37 right)
  //  {
  //    return
  //      left.X * right.X +
  //      left.Y * right.Y +
  //      left.Z * right.Z;
  //  }

  //  public static Vector37 CrossProduct(Vector37 left, Vector37 right)
  //  {
  //    return new Vector37(
  //      left.Y * right.Z - left.Z * right.Y,
  //      left.Z * right.X - left.X * right.Z,
  //      left.X * right.Y - left.Y * right.X);
  //  }

  //  public static Vector37 Normalize(Vector37 vector)
  //  {
  //    float length = vector.Length();
  //    if (length != 0.0)
  //      return new Vector37(
  //        vector.X / length,
  //        vector.Y / length,
  //        vector.Z / length);
  //    else
  //      return Vector37.FactoryZero;
  //  }

  //  public static float Length(Vector37 vector)
  //  {
  //    return Calc.SquareRoot(
  //      vector.X * vector.X +
  //      vector.Y * vector.Y +
  //      vector.Z * vector.Z);
  //  }

  //  public static float LengthSquared(Vector37 vector)
  //  {
  //    return
  //      vector.X * vector.X +
  //      vector.Y * vector.Y +
  //      vector.Z * vector.Z;
  //  }

  //  public static bool Equals(Vector37 left, Vector37 right)
  //  {
  //    if (object.ReferenceEquals(left, right))
  //      return true;
  //    else if (object.ReferenceEquals(left, null) || object.ReferenceEquals(null, right))
  //      return false;
  //    else return
  //      left.X == right.X && 
  //      left.Y == right.Y &&
  //      left.Z == right.Z;
  //  }

  //  public static bool Equals(Vector37 left, Vector37 right, float leniency)
  //  {
  //    if (object.ReferenceEquals(left, right))
  //      return true;
  //    else if (object.ReferenceEquals(left, null) || object.ReferenceEquals(null, right))
  //      return false;
  //    else return
  //      Calc.Abs(left.X - right.X) < leniency &&
  //      Calc.Abs(left.Y - right.Y) < leniency &&
  //      Calc.Abs(left.Z - right.Z) < leniency;
  //  }

  //  public static Vector37 DirectionTowardsPosition(Vector37 from, Vector37 to)
  //  {
  //    return (to - from).Normalize();
  //  }

  //  public static Vector37 MoveTowardsPosition(Vector37 position, Vector37 goal, float distance)
  //  {
  //    Vector37 direction = DirectionTowardsPosition(position, goal);
  //    return new Vector37(
  //      position.X + direction.X * distance,
  //      position.Y + direction.Y * distance,
  //      position.Z + direction.Z * distance);
  //  }

  //  public static Vector37 MoveTowardsDirection(Vector37 position, Vector37 direction, float distance)
  //  {
  //    direction = direction.Normalize();
  //    return new Vector37(
  //      position.X + direction.X * distance,
  //      position.Y + direction.Y * distance,
  //      position.Z + direction.Z * distance);
  //  }

  //  public static float AngleBetween(Vector37 first, Vector37 second)
  //  {
  //    return Calc.ArcCos(Vector37.DotProduct(first, second) / (first.Length() * second.Length()));
  //  }

  //  public static Vector37 RotateBy(Vector37 vector, float angle, float x, float y, float z)
  //  {
  //    // Note: the angle is in radians
  //    float sinHalfAngle = Calc.Sin(angle / 2);
  //    float cosHalfAngle = Calc.Cos(angle / 2);
  //    x *= sinHalfAngle;
  //    y *= sinHalfAngle;
  //    z *= sinHalfAngle;
  //    float x2 = cosHalfAngle * vector.X + y * vector.Z - z * vector.Y;
  //    float y2 = cosHalfAngle * vector.Y + z * vector.X - x * vector.Z;
  //    float z2 = cosHalfAngle * vector.Z + x * vector.Y - y * vector.X;
  //    float w2 = -x * vector.X - y * vector.Y - z * vector.Z;
  //    return new Vector37(
  //      x * w2 + cosHalfAngle * x2 + y * z2 - z * y2,
  //      y * w2 + cosHalfAngle * y2 + z * x2 - x * z2,
  //      z * w2 + cosHalfAngle * z2 + x * y2 - y * x2);
  //  }

  //  public static Vector37 RotateBy(Vector37 vector, Quaternion rotation)
  //  {
  //    Quaternion answer = (rotation * vector) * Quaternion.Conjugate(rotation);
  //    return new Vector37(answer.X, answer.Y, answer.Z);
  //  }

  //  public static Vector37 InterpolateLinear(Vector37 a, Vector37 b, float blend)
  //  {
  //    if (blend < 0 || blend > 1.0f)
  //      throw new VectorException("invalid lerp blend value: (blend < 0.0f || blend > 1.0f).");
  //    return new Vector37(
  //      a.X + blend * (b.X - a.X),
  //      a.Y + blend * (b.Y - a.Y),
  //      a.Z + blend * (b.Z - a.Z));
  //  }

  //  public static Vector37 InterpolateSphereical(Vector37 a, Vector37 b, float blend)
  //  {
  //    if (blend < 0 || blend > 1.0f)
  //      throw new VectorException("invalid slerp blend value: (blend < 0.0f || blend > 1.0f).");
  //    float dot = Calc.Clamp(Vector37.DotProduct(a, b), -1.0f, 1.0f);
  //    float theta = Calc.ArcCos(dot) * blend;
  //    return a * Calc.Cos(theta) + (b - a * dot).Normalize() * Calc.Sin(theta);
  //  }

  //  public static Vector37 InterpolateBarycentric(Vector37 a, Vector37 b, Vector37 c, float u, float v)
  //  {
  //    return a + u * (b - a) + v * (c - a);
  //  }

  //  public static float[,] ToFloats(Vector37 vector)
  //  {
  //    return new float[,] { { vector.X }, { vector.Y }, { vector.Z } };
  //  }

  //  public static Matrix ToMatrix(Vector37 vector)
  //  {
  //    return new Matrix(3, 1, new float[] { vector.X, vector.Y, vector.Z });
  //    //Matrix matrix = new Matrix(vector.X, vector.Y, vector.Z);
  //    //matrix[0, 0] = vector.X;
  //    //matrix[1, 0] = vector.Y;
  //    //matrix[2, 0] = vector.Z;
  //    //return matrix;
  //  }

  //  public override string ToString()
  //  {
  //    return base.ToString();
  //    //return
  //    //  X.ToString() + "\n" + 
  //    //  Y.ToString() + "\n" +
  //    //  Z.ToString() + "\n";
  //  }

  //  public override int GetHashCode()
  //  {
  //    return
  //      X.GetHashCode() ^
  //      Y.GetHashCode() ^
  //      Z.GetHashCode();
  //  }

  //  public override bool Equals(object obj)
  //  {
  //    return base.Equals(obj);
  //  }

  //  private class VectorException : Exception
  //  {
  //    public VectorException(string message) : base(message) { }
  //  }
  //}
  #endregion
}
