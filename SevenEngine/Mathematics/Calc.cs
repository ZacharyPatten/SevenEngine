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
  /// <summary>This calculator contains static-only methods for computations.</summary>
  public static class Calc
  {
    #region Logic

    public static float Max(float first, float second)
    {
      //return Math.Max(first, second);
      if (second > first)
        return second;
      return first;
    }

    public static float Min(float first, float second)
    {
      //return Math.Min(first, second);
      if (second < first)
        return second;
      return first;
    }

    public static float Clamp(float value, float minimum, float maximum)
    {
      if (value < minimum)
        return minimum;
      if (value > maximum)
        return maximum;
      return value;
    }

    public static float Abs(float number)
    {
      //return Math.Abs(number);
      if (number < 0)
        return -number;
      return number;
    }

    #endregion

    #region Algebra

    public static bool IsPrime(int candidate)
    {
      if (candidate == 2) return true;
      for (int divisor = 3; divisor <= SquareRoot(candidate); divisor += 2)
        if ((candidate % divisor) == 0)
          return false;
      return true;
    }

    public static float SquareRoot(float number)
    {
      return (float)Math.Sqrt(number);
      // I have not written my own version of this function yet, just use the System for now...
    }

    public static float Power(float number, float power)
    {
      return (float)Math.Pow(number, power);
      // I have not written my own version of this function yet, just use the System for now...
    }

    private static long GCD(int first, int second)
    {
      if (first < 0) first = -first;
      if (second < 0) second = -second;
      int temp = first;
      do
      {
        if (first < second)
        {
          temp = first;
          first = second;
          second = temp;
        }
        first = first % second;
      } while (first != 0);
      return second;
    }

    #endregion

    #region Trigonomety

    public const float Pi = 3.1415926535897932384626433832795028841971693993751f;
    public const float PiOverTwo = Pi / 2;
    public const float PiOverThree = Pi / 3;
    public const float PiOverFour = Pi / 4;
    public const float PiOverSix = Pi / 6;
    public const float TwoPi = 2 * Pi;
    public const float ThreePiOverTwo = 3 * Pi / 2;

    public static float ToRadians(float angle) { return angle * Pi / 180f; }
    public static float ToDegrees(float angle) { return angle * 180f / Pi; }

    public static float Sin(float angle)
    {
      return (float)Math.Sin(angle);

      // THE FOLLOWING IS PERSONAL SIN FUNCTION. IT WORKS BUT IT IS NOT AS FAST AS
      // THE SYSTEM FUNCTION IN ITS CURRENT STATE
      #region Custom Sin Function
      //// get the angle to be within the unit circle
      //angle = angle % (TwoPi);

      //// if the angle is negative, inverse it against the full unit circle
      //if (angle < 0)
      //  angle = TwoPi + angle;

      //// adjust for quadrants
      //// NOTE: if you want more accuracy, you can follow this pattern
      //// sin(x) = x - x^3/3! + x^5/5! - x^7/7! ...
      //// the more terms you include the more accurate it is
      //float angleCubed;
      //float angleToTheFifth;
      //// quadrant 1
      //if (angle <= HalfPi)
      //{
      //  angleCubed = angle * angle * angle;
      //  angleToTheFifth = angleCubed * angle * angle;
      //  return angle
      //    - ((angleCubed) / 6)
      //    + ((angleToTheFifth) / 120);
      //}
      //// quadrant 2
      //else if (angle <= Pi)
      //{
      //  angle = HalfPi - (angle % HalfPi);
      //  angleCubed = angle * angle * angle;
      //  angleToTheFifth = angleCubed * angle * angle;
      //  return angle
      //    - ((angleCubed) / 6)
      //    + ((angleToTheFifth) / 120);
      //}
      //// quadrant 3
      //else if (angle <= ThreeHalvesPi)
      //{
      //  angle = angle % Pi;
      //  angleCubed = angle * angle * angle;
      //  angleToTheFifth = angleCubed * angle * angle;
      //  return -(angle
      //      - ((angleCubed) / 6)
      //      + ((angleToTheFifth) / 120));
      //}
      //// quadrant 4  
      //else
      //{
      //  angle = HalfPi - (angle % HalfPi);
      //  angleCubed = angle * angle * angle;
      //  angleToTheFifth = angleCubed * angle * angle;
      //  return -(angle
      //      - ((angleCubed) / 6)
      //      + ((angleToTheFifth) / 120));
      //}
      #endregion
    }

    public static float Cos(float angle)
    {
      return (float)Math.Cos(angle);

      // THE FOLLOWING IS MY PERSONAL FUNCTION. IT WORKS BUT IT IS NOT AS FAST AS
      // THE SYSTEM FUNCTION IN ITS CURRENT STATE
      #region Custom Cos Function
      //// If you wanted to be cheap, you could just use the following commented line...
      //// return Sin(angle + (Pi / 2));

      //// get the angle to be within the unit circle
      //angle = angle % (TwoPi);

      //// if the angle is negative, inverse it against the full unit circle
      //if (angle < 0)
      //  angle = TwoPi + angle;

      //// adjust for quadrants
      //// NOTE: if you want more accuracy, you can follow this pattern
      //// cos(x) = 1 - x^2/2! + x^4/4! - x^6/6! ...
      //// the terms you include the more accuracy it is
      //float angleSquared;
      //float angleToTheFourth;
      //float angleToTheSixth;
      //// quadrant 1
      //if (angle <= HalfPi)
      //{
      //  angleSquared = angle * angle;
      //  angleToTheFourth = angleSquared * angle * angle;
      //  angleToTheSixth = angleToTheFourth * angle * angle;
      //  return 1
      //    - (angleSquared / 2)
      //    + (angleToTheFourth / 24)
      //    - (angleToTheSixth / 720);
      //}
      //// quadrant 2
      //else if (angle <= Pi)
      //{
      //  angle = HalfPi - (angle % HalfPi);
      //  angleSquared = angle * angle;
      //  angleToTheFourth = angleSquared * angle * angle;
      //  angleToTheSixth = angleToTheFourth * angle * angle;
      //  return -(1
      //    - (angleSquared / 2)
      //    + (angleToTheFourth / 24)
      //    - (angleToTheSixth / 720));
      //}
      //// quadrant 3
      //else if (angle <= ThreeHalvesPi)
      //{
      //  angle = angle % Pi;
      //  angleSquared = angle * angle;
      //  angleToTheFourth = angleSquared * angle * angle;
      //  angleToTheSixth = angleToTheFourth * angle * angle;
      //  return -(1
      //    - (angleSquared / 2)
      //    + (angleToTheFourth / 24)
      //    - (angleToTheSixth / 720));
      //}
      //// quadrant 4  
      //else
      //{
      //  angle = HalfPi - (angle % HalfPi);
      //  angleSquared = angle * angle;
      //  angleToTheFourth = angleSquared * angle * angle;
      //  angleToTheSixth = angleToTheFourth * angle * angle;
      //  return 1
      //    - (angleSquared / 2)
      //    + (angleToTheFourth / 24)
      //    - (angleToTheSixth / 720);
      //}
      #endregion
    }

    public static float Tan(float angle)
    {
      return (float)Math.Tan(angle);

      // THE FOLLOWING IS MY PERSONAL FUNCTION. IT WORKS BUT IT IS NOT AS FAST AS
      // THE SYSTEM FUNCTION IN ITS CURRENT STATE
      #region Custom Tan Function
      //// "sin / cos" results in "opposite side / adjacent side", which is equal to tangent
      //return Sin(angle) / Cos(angle);
      #endregion
    }

    public static float Sec(float angle)
    {
      return 1.0f / (float)Math.Cos(angle);

      // THE FOLLOWING IS MY PERSONAL FUNCTION. IT WORKS BUT IT IS NOT AS FAST AS
      // THE SYSTEM FUNCTION IN ITS CURRENT STATE
      #region Custom Sec Function
      //// by definition, sec is the reciprical of cos
      //return 1 / Cos(angle);
      #endregion
    }

    public static float Csc(float angle)
    {
      return 1.0f / (float)Math.Sin(angle);

      // THE FOLLOWING IS MY PERSONAL FUNCTION. IT WORKS BUT IT IS NOT AS FAST AS
      // THE SYSTEM FUNCTION IN ITS CURRENT STATE
      #region Custom Csc Function
      //// by definition, csc is the reciprical of sin
      //return 1 / Sin(angle);
      #endregion
    }

    public static float Cot(float angle)
    {
      return 1.0f / (float)Math.Tan(angle);

      // THE FOLLOWING IS MY PERSONAL FUNCTION. IT WORKS BUT IT IS NOT AS FAST AS
      // THE SYSTEM FUNCTION IN ITS CURRENT STATE
      #region Custom Cot Function
      //// by definition, cot is the reciprical of tan
      //return 1 / Tan(angle);
      #endregion
    }

    public static float ArcSin(float sinRatio)
    {
      return (float)Math.Asin(sinRatio);
      //I haven't made a custom ArcSin function yet...
    }

    public static float ArcCos(float cosRatio)
    {
      return (float)Math.Acos(cosRatio);
      //I haven't made a custom ArcCos function yet...
    }

    public static float ArcTan(float tanRatio)
    {
      return (float)Math.Atan(tanRatio);
      //I haven't made a custom ArcTan function yet...
    }

    public static float ArcCsc(float cscRatio)
    {
      return (float)Math.Asin(1.0f / cscRatio);
      //I haven't made a custom ArcCsc function yet...
    }

    public static float ArcSec(float secRatio)
    {
      return (float)Math.Acos(1.0f / secRatio);
      //I haven't made a custom ArcSec function yet...
    }

    public static float ArcCot(float cotRatio)
    {
      return (float)Math.Atan(1.0f / cotRatio);
      //I haven't made a custom ArcCot function yet...
    }

    #endregion

    #region Linear Algebra

    /// <summary>Negates all the values in a matrix.</summary>
    /// <param name="matrix">The matrix to have its values negated.</param>
    /// <returns>The resulting matrix after the negations.</returns>
    public static float[,] Negate(float[,] matrix)
    {
      float[,] result = new float[matrix.GetLength(0), matrix.GetLength(1)];
      for (int i = 0; i < matrix.GetLength(0); i++)
        for (int j = 0; j < matrix.GetLength(1); j++)
          result[i, j] = -matrix[i, j];
      return result;
    }

    /// <summary>Does standard addition of two matrices.</summary>
    /// <param name="left">The left matrix of the addition.</param>
    /// <param name="right">The right matrix of the addition.</param>
    /// <returns>The resulting matrix after the addition.</returns>
    public static float[,] Add(float[,] left, float[,] right)
    {
      if (left.GetLength(0) != right.GetLength(0) || left.GetLength(1) != right.GetLength(1))
        throw new CalcException("invalid addition (size miss-match).");
      float[,] result = new float[left.GetLength(0), left.GetLength(1)];
      for (int i = 0; i < result.GetLength(0); i++)
        for (int j = 0; j < result.GetLength(1); j++)
          result[i, j] = left[i, j] + right[i, j];
      return result;
    }

    /// <summary>Subtracts a scalar from all the values in a matrix.</summary>
    /// <param name="left">The matrix to have the values subtracted from.</param>
    /// <param name="right">The scalar to subtract from all the matrix values.</param>
    /// <returns>The resulting matrix after the subtractions.</returns>
    public static float[,] Subtract(float[,] left, float[,] right)
    {
      if (left.GetLength(0) != right.GetLength(0) || left.GetLength(1) != right.GetLength(1))
        throw new CalcException("invalid subtraction (size miss-match).");
      float[,] result = new float[left.GetLength(0), left.GetLength(1)];
      for (int i = 0; i < result.GetLength(0); i++)
        for (int j = 0; j < result.GetLength(1); j++)
          result[i, j] = left[i, j] - right[i, j];
      return result;
    }

    /// <summary>Does a standard (triple for looped) multiplication between matrices.</summary>
    /// <param name="left">The left matrix of the multiplication.</param>
    /// <param name="right">The right matrix of the multiplication.</param>
    /// <returns>The resulting matrix of the multiplication.</returns>
    public static float[,] Multiply(float[,] left, float[,] right)
    {
      if (left.GetLength(1) != right.GetLength(0))
        throw new CalcException("invalid multiplication (size miss-match).");
      float[,] result = new float[left.GetLength(0), right.GetLength(1)];
      for (int i = 0; i < result.GetLength(0); i++)
        for (int j = 0; j < result.GetLength(1); j++)
          for (int k = 0; k < left.GetLength(1); k++)
            result[i, j] += left[i, k] * right[k, j];
      return result;
    }

    /// <summary>Multiplies all the values in a matrix by a scalar.</summary>
    /// <param name="left">The matrix to have the values multiplied.</param>
    /// <param name="right">The scalar to multiply the values by.</param>
    /// <returns>The resulting matrix after the multiplications.</returns>
    public static float[,] Multiply(float[,] left, float right)
    {
      float[,] result = new float[left.GetLength(0), left.GetLength(1)];
      for (int i = 0; i < left.GetLength(0); i++)
        for (int j = 0; j < left.GetLength(1); j++)
          result[i, j] = left[i, j] * right;
      return result;
    }

    ///// <summary>Applies a power to a square matrix.</summary>
    ///// <param name="matrix">The matrix to be powered by.</param>
    ///// <param name="power">The power to apply to the matrix.</param>
    ///// <returns>The resulting matrix of the power operation.</returns>
    //public static float[,] Power(float[,] matrix, int power)
    //{
    //  if (!(matrix.GetLength(0) == matrix.GetLength(1)))
    //    throw new CalcException("invalid power (!matrix.IsSquare).");
    //  if (!(power > -1))
    //    throw new CalcException("invalid power !(power > -1)");
    //  if (power == 0)
    //    return Matrix.FactoryIdentity(matrix.GetLength(0), matrix.GetLength(1));
    //  float[,] result = Matrix.Clone(matrix);
    //  for (int i = 0; i < power; i++)
    //    result = Multiply(result, result);
    //  return result;
    //}

    /// <summary>Divides all the values in the matrix by a scalar.</summary>
    /// <param name="matrix">The matrix to divide the values of.</param>
    /// <param name="right">The scalar to divide all the matrix values by.</param>
    /// <returns>The resulting matrix with the divided values.</returns>
    public static float[,] Divide(float[,] matrix, float right)
    {
      float[,] result = new float[matrix.GetLength(0), matrix.GetLength(1)];
      for (int i = 0; i < matrix.GetLength(0); i++)
        for (int j = 0; j < matrix.GetLength(1); j++)
          result[i, j] = matrix[i, j] / right;
      return result;
    }

    /// <summary>Gets the minor of a matrix.</summary>
    /// <param name="matrix">The matrix to get the minor of.</param>
    /// <param name="row">The restricted row to form the minor.</param>
    /// <param name="column">The restricted column to form the minor.</param>
    /// <returns>The minor of the matrix.</returns>
    public static float[,] Minor(float[,] matrix, int row, int column)
    {
      float[,] minor = new float[matrix.GetLength(0) - 1, matrix.GetLength(1) - 1];
      int m = 0, n = 0;
      for (int i = 0; i < matrix.GetLength(0); i++)
      {
        if (i == row) continue;
        n = 0;
        for (int j = 0; j < matrix.GetLength(1); j++)
        {
          if (j == column) continue;
          minor[m, n] = matrix[i, j];
          n++;
        }
        m++;
      }
      return minor;
    }

    private static void RowMultiplication(float[,] matrix, int row, float scalar)
    {
      for (int j = 0; j < matrix.GetLength(1); j++)
        matrix[row, j] *= scalar;
    }

    private static void RowAddition(float[,] matrix, int target, int second, float scalar)
    {
      for (int j = 0; j < matrix.GetLength(1); j++)
        matrix[target, j] += (matrix[second, j] * scalar);
    }

    private static void SwapRows(float[,] matrix, int row1, int row2)
    {
      for (int j = 0; j < matrix.GetLength(1); j++)
      {
        float temp = matrix[row1, j];
        matrix[row1, j] = matrix[row2, j];
        matrix[row2, j] = temp;
      }
    }

    /// <summary>Combines two matrices from left to right 
    /// (result.Rows = left.Rows && result.Columns = left.Columns + right.Columns).</summary>
    /// <param name="left">The left matrix of the concatenation.</param>
    /// <param name="right">The right matrix of the concatenation.</param>
    /// <returns>The resulting matrix of the concatenation.</returns>
    public static float[,] ConcatenateRowWise(float[,] left, float[,] right)
    {
      if (left.GetLength(0) != right.GetLength(0))
        throw new CalcException("invalid row-wise concatenation !(left.Rows == right.Rows).");
      float[,] result = new float[left.GetLength(0), left.GetLength(1) + right.GetLength(1)];
      for (int i = 0; i < result.GetLength(0); i++)
        for (int j = 0; j < result.GetLength(1); j++)
        {
          if (j < left.GetLength(1)) result[i, j] = left[i, j];
          else result[i, j] = right[i, j - left.GetLength(1)];
        }
      return result;
    }

    ///// <summary>Calculates the determinent of a square matrix.</summary>
    ///// <param name="matrix">The matrix to calculate the determinent of.</param>
    ///// <returns>The determinent of the matrix.</returns>
    //public static float Determinent(float[,] matrix)
    //{
    //  if (!(matrix.GetLength(0) == matrix.GetLength(1)))
    //    throw new CalcException("invalid determinent !(matrix.IsSquare).");
    //  float det = 1.0f;
    //  try
    //  {
    //    float[,] rref = Matrix.Clone(matrix);
    //    for (int i = 0; i < matrix.GetLength(0); i++)
    //    {
    //      if (rref[i, i] == 0)
    //        for (int j = i + 1; j < rref.GetLength(0); j++)
    //          if (rref[j, i] != 0)
    //          {
    //            Calc.SwapRows(rref, i, j);
    //            det *= -1;
    //          }
    //      det *= rref[i, i];
    //      Calc.RowMultiplication(rref, i, 1 / rref[i, i]);
    //      for (int j = i + 1; j < rref.GetLength(0); j++)
    //        Calc.RowAddition(rref, j, i, -rref[j, i]);
    //      for (int j = i - 1; j >= 0; j--)
    //        Calc.RowAddition(rref, j, i, -rref[j, i]);
    //    }
    //    return det;
    //  }
    //  catch (Exception)
    //  {
    //    throw new CalcException("determinent computation failed.");
    //  }
    //}

    ///// <summary>Calculates the echelon of a matrix (aka REF).</summary>
    ///// <param name="matrix">The matrix to calculate the echelon of (aka REF).</param>
    ///// <returns>The echelon of the matrix (aka REF).</returns>
    //public static float[,] Echelon(float[,] matrix)
    //{
    //  try
    //  {
    //    float[,] result = Matrix.Clone(matrix);
    //    for (int i = 0; i < matrix.GetLength(0); i++)
    //    {
    //      if (result[i, i] == 0)
    //        for (int j = i + 1; j < result.GetLength(0); j++)
    //          if (result[j, i] != 0)
    //            Calc.SwapRows(result, i, j);
    //      if (result[i, i] == 0)
    //        continue;
    //      if (result[i, i] != 1)
    //        for (int j = i + 1; j < result.GetLength(0); j++)
    //          if (result[j, i] == 1)
    //            Calc.SwapRows(result, i, j);
    //      Calc.RowMultiplication(result, i, 1 / result[i, i]);
    //      for (int j = i + 1; j < result.GetLength(0); j++)
    //        Calc.RowAddition(result, j, i, -result[j, i]);
    //    }
    //    return result;
    //  }
    //  catch { throw new CalcException("echelon computation failed."); }
    //}

    ///// <summary>Calculates the echelon of a matrix and reduces it (aka RREF).</summary>
    ///// <param name="matrix">The matrix matrix to calculate the reduced echelon of (aka RREF).</param>
    ///// <returns>The reduced echelon of the matrix (aka RREF).</returns>
    //public static float[,] ReducedEchelon(float[,] matrix)
    //{
    //  try
    //  {
    //    float[,] result = Matrix.Clone(matrix);
    //    for (int i = 0; i < matrix.GetLength(0); i++)
    //    {
    //      if (result[i, i] == 0)
    //        for (int j = i + 1; j < result.GetLength(0); j++)
    //          if (result[j, i] != 0)
    //            Calc.SwapRows(result, i, j);
    //      if (result[i, i] == 0) continue;
    //      if (result[i, i] != 1)
    //        for (int j = i + 1; j < result.GetLength(0); j++)
    //          if (result[j, i] == 1)
    //            Calc.SwapRows(result, i, j);
    //      Calc.RowMultiplication(result, i, 1 / result[i, i]);
    //      for (int j = i + 1; j < result.GetLength(0); j++)
    //        Calc.RowAddition(result, j, i, -result[j, i]);
    //      for (int j = i - 1; j >= 0; j--)
    //        Calc.RowAddition(result, j, i, -result[j, i]);
    //    }
    //    return result;
    //  }
    //  catch { throw new CalcException("reduced echelon calculation failed."); }
    //}

    ///// <summary>Calculates the inverse of a matrix.</summary>
    ///// <param name="matrix">The matrix to calculate the inverse of.</param>
    ///// <returns>The inverse of the matrix.</returns>
    //public static float[,] Inverse(float[,] matrix)
    //{
    //  if (Matrix.Determinent(matrix) == 0)
    //    throw new CalcException("inverse calculation failed.");
    //  try
    //  {
    //    float[,] identity = Matrix.FactoryIdentity(matrix.GetLength(0), matrix.GetLength(1));
    //    float[,] rref = Matrix.Clone(matrix);
    //    for (int i = 0; i < matrix.GetLength(0); i++)
    //    {
    //      if (rref[i, i] == 0)
    //        for (int j = i + 1; j < rref.GetLength(0); j++)
    //          if (rref[j, i] != 0)
    //          {
    //            Calc.SwapRows(rref, i, j);
    //            Calc.SwapRows(identity, i, j);
    //          }
    //      Calc.RowMultiplication(identity, i, 1 / rref[i, i]);
    //      Calc.RowMultiplication(rref, i, 1 / rref[i, i]);
    //      for (int j = i + 1; j < rref.GetLength(0); j++)
    //      {
    //        Calc.RowAddition(identity, j, i, -rref[j, i]);
    //        Calc.RowAddition(rref, j, i, -rref[j, i]);
    //      }
    //      for (int j = i - 1; j >= 0; j--)
    //      {
    //        Calc.RowAddition(identity, j, i, -rref[j, i]);
    //        Calc.RowAddition(rref, j, i, -rref[j, i]);
    //      }
    //    }
    //    return identity;
    //  }
    //  catch { throw new CalcException("inverse calculation failed."); }
    //}

    ///// <summary>Calculates the adjoint of a matrix.</summary>
    ///// <param name="matrix">The matrix to calculate the adjoint of.</param>
    ///// <returns>The adjoint of the matrix.</returns>
    //public static float[,] Adjoint(float[,] matrix)
    //{
    //  if (!(matrix.GetLength(0) == matrix.GetLength(1)))
    //    throw new CalcException("Adjoint of a non-square matrix does not exists");
    //  float[,] result = new Matrix(matrix.GetLength(0), matrix.GetLength(1));
    //  for (int i = 0; i < matrix.GetLength(0); i++)
    //    for (int j = 0; j < matrix.GetLength(1); j++)
    //      if ((i + j) % 2 == 0)
    //        result[i, j] = Calc.Determinent(Matrix.Minor(matrix, i, j));
    //      else
    //        result[i, j] = -Calc.Determinent(Matrix.Minor(matrix, i, j));
    //  return Matrix.Transpose(result);
    //}

    /// <summary>Returns the transpose of a matrix.</summary>
    /// <param name="matrix">The matrix to transpose.</param>
    /// <returns>The transpose of the matrix.</returns>
    public static float[,] Transpose(float[,] matrix)
    {
      float[,] transpose = new float[matrix.GetLength(1), matrix.GetLength(0)];
      for (int i = 0; i < transpose.GetLength(0); i++)
        for (int j = 0; j < transpose.GetLength(1); j++)
          transpose[i, j] = matrix[j, i];
      return transpose;
    }

    ///// <summary>Decomposes a matrix into lower-upper reptresentation.</summary>
    ///// <param name="matrix">The matrix to decompose.</param>
    ///// <param name="Lower">The computed lower triangular matrix.</param>
    ///// <param name="Upper">The computed upper triangular matrix.</param>
    //public static void DecomposeLU(float[,] matrix, out float[,] Lower, out float[,] Upper)
    //{
    //  if (!(matrix.GetLength(0) == matrix.GetLength(1)))
    //    throw new CalcException("The matrix is not square!");
    //  Lower = Matrix.FactoryIdentity(matrix.GetLength(0), matrix.GetLength(1));
    //  Upper = Matrix.Clone(matrix);
    //  int[] permutation = new int[matrix.GetLength(0)];
    //  for (int i = 0; i < matrix.GetLength(0); i++) permutation[i] = i;
    //  float p = 0, pom2, detOfP = 1;
    //  int k0 = 0, pom1 = 0;
    //  for (int k = 0; k < matrix.GetLength(1) - 1; k++)
    //  {
    //    p = 0;
    //    for (int i = k; i < matrix.GetLength(0); i++)
    //      if (Calc.Abs(Upper[i, k]) > p)
    //      {
    //        p = Calc.Abs(Upper[i, k]);
    //        k0 = i;
    //      }
    //    if (p == 0)
    //      throw new CalcException("The matrix is singular!");
    //    pom1 = permutation[k];
    //    permutation[k] = permutation[k0];
    //    permutation[k0] = pom1;
    //    for (int i = 0; i < k; i++)
    //    {
    //      pom2 = Lower[k, i];
    //      Lower[k, i] = Lower[k0, i];
    //      Lower[k0, i] = pom2;
    //    }
    //    if (k != k0)
    //      detOfP *= -1;
    //    for (int i = 0; i < matrix.GetLength(1); i++)
    //    {
    //      pom2 = Upper[k, i];
    //      Upper[k, i] = Upper[k0, i];
    //      Upper[k0, i] = pom2;
    //    }
    //    for (int i = k + 1; i < matrix.GetLength(0); i++)
    //    {
    //      Lower[i, k] = Upper[i, k] / Upper[k, k];
    //      for (int j = k; j < matrix.GetLength(1); j++)
    //        Upper[i, j] = Upper[i, j] - Lower[i, k] * Upper[k, j];
    //    }
    //  }
    //}

    #endregion

    #region Combinatorics

    /// <summary>calculates the factorial of a given number.</summary>
    /// <param name="integer">The number of the factorial to calculate.</param>
    /// <returns>The calculated factorial value.</returns>
    public static int Factorial(int integer)
    {
      try
      {
        checked
        {
          int result = 1;
          for (; integer > 1; integer--)
            result *= integer;
          return result;
        }
      }
      catch { throw new Exception("overflow occured in factorial."); }
    }

    /// <summary></summary>
    /// <param name="set"></param>
    /// <param name="subsets"></param>
    /// <returns></returns>
    public static float Combinations(int set, params int[] subsets)
    {
      float result = Calc.Factorial(set);
      int sum = 0;
      for (int i = 0; i < subsets.Length; i++)
      {
        result /= (float)Calc.Factorial(subsets[i]);
        sum += subsets[i];
      }
      if (sum > set)
        throw new CalcException("invalid combination (set < Count(subsets)).");
      return result;
    }

    /// <summary>Does a combinotorics choose operation.</summary>
    /// <param name="top">The number of items choosing from a set.</param>
    /// <param name="bottom">The set to be chosen from.</param>
    /// <returns>The result of the choose.</returns>
    public static float Choose(int top, int bottom)
    {
      if (!(top <= bottom || top >= 0))
        throw new CalcException("invalid choose values !(top <= bottom || top >= 0)");
      return Calc.Factorial(top) / (float)(Calc.Factorial(top) * Calc.Factorial(bottom - top));
    }



    #endregion

    private class CalcException : Exception
    {
      public CalcException(string Message) : base(Message) { }
    }
  }
}
