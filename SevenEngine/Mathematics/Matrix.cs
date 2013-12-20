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
using System.Text;

namespace SevenEngine.Mathematics
{
  /// <summary>A matrix wrapper for float[,] to perform matrix theory. Enjoy :)</summary>
  public class Matrix
  {
    private float[,] _matrix;
    
    /// <summary>The float[,] reference of this matrix.</summary>
    public float[,] Floats { get { return _matrix; } }
    /// <summary>The number of rows in the matrix.</summary>
    public int Rows { get { return _matrix.GetLength(0); } }
    /// <summary>The number of columns in the matrix.</summary>
    public int Columns { get { return _matrix.GetLength(1); } }
    /// <summary>Determines if the matrix is square.</summary>
    public bool IsSquare { get { return Rows == Columns; } }

    /// <summary>Standard row-major matrix indexing.</summary>
    /// <param name="row">The row index.</param>
    /// <param name="column">The column index.</param>
    /// <returns>The value at the given indeces.</returns>
    public float this[int row, int column]
    {
      get
      {
        try { return _matrix[row, column]; }
        catch { throw new MatrixException("index out of bounds."); }
      }
      set
      {
        try { _matrix[row, column] = value; }
        catch { throw new MatrixException("index out of bounds."); }
      }
    }

    /// <summary>Constructs a new zero-matrix of the given dimensions.</summary>
    /// <param name="rows">The number of row dimensions.</param>
    /// <param name="columns">The number of column dimensions.</param>
    public Matrix(int rows, int columns)
    {
      try { _matrix = new float[rows, columns]; }
      catch { throw new MatrixException("invalid dimensions."); }
    }

    /// <summary>Constructs a new array given row/column dimensions and the values to fill the matrix with.</summary>
    /// <param name="rows">The number of rows of the matrix.</param>
    /// <param name="columns">The number of columns of the matrix.</param>
    /// <param name="values">The values to fill the matrix with.</param>
    public Matrix(int rows, int columns, params float[] values)
    {
      if (values.Length != rows * columns)
        throw new MatrixException("invalid construction (number of values does not match dimensions.)");
      float[,] matrix = new float[rows, columns];
      int k = 0;
      for (int i = 0; i < rows; i++)
        for (int j = 0; j < columns; j++)
          matrix[i, j] = values[k++];
      _matrix = matrix;
    }

    /// <summary>Wraps a float[,] inside of a matrix class. WARNING: still references that float[,].</summary>
    /// <param name="matrix">The float[,] to wrap in a matrix class.</param>
    public Matrix(float[,] matrix)
    {
      _matrix = matrix;
    }

    /// <summary>Constructs a new zero-matrix of the given dimensions.</summary>
    /// <param name="rows">The number of rows of the matrix.</param>
    /// <param name="columns">The number of columns of the matrix.</param>
    /// <returns>The newly constructed zero-matrix.</returns>
    public static Matrix FactoryZero(int rows, int columns)
    {
      try { return new Matrix(rows, columns); }
      catch { throw new MatrixException("invalid dimensions."); }
    }

    /// <summary>Constructs a new identity-matrix of the given dimensions.</summary>
    /// <param name="rows">The number of rows of the matrix.</param>
    /// <param name="columns">The number of columns of the matrix.</param>
    /// <returns>The newly constructed identity-matrix.</returns>
    public static Matrix FactoryIdentity(int rows, int columns)
    {
      Matrix matrix;
      try { matrix = new Matrix(rows, columns); }
      catch { throw new MatrixException("invalid dimensions."); }
      if (rows <= columns)
        for (int i = 0; i < rows; i++)
          matrix[i, i] = 1;
      else
        for (int i = 0; i < columns; i++)
          matrix[i, i] = 1;
      return matrix;
    }

    /// <summary>Constructs a new matrix where every entry is 1.</summary>
    /// <param name="rows">The number of rows of the matrix.</param>
    /// <param name="columns">The number of columns of the matrix.</param>
    /// <returns>The newly constructed matrix filled with 1's.</returns>
    public static Matrix FactoryOne(int rows, int columns)
    {
      Matrix matrix;
      try { matrix = new Matrix(rows, columns); }
      catch { throw new MatrixException("invalid dimensions."); }
      for (int i = 0; i < rows; i++)
        for (int j = 0; j < columns; j++)
          matrix[i, j] = 1;
      return matrix;
    }

    /// <summary>Constructs a new matrix where every entry is the same uniform value.</summary>
    /// <param name="rows">The number of rows of the matrix.</param>
    /// <param name="columns">The number of columns of the matrix.</param>
    /// <param name="uniform">The value to assign every spot in the matrix.</param>
    /// <returns>The newly constructed matrix filled with the uniform value.</returns>
    public static Matrix FactoryUniform(int rows, int columns, float uniform)
    {
      Matrix matrix;
      try { matrix = new Matrix(rows, columns); }
      catch { throw new MatrixException("invalid dimensions."); }
      for (int i = 0; i < rows; i++)
        for (int j = 0; j < columns; j++)
          matrix[i, j] = uniform;
      return matrix;
    }

    /// <summary>Negates all the values in a matrix.</summary>
    /// <param name="matrix">The matrix to have its values negated.</param>
    /// <returns>The resulting matrix after the negations.</returns>
    public static Matrix operator -(Matrix matrix) { return Matrix.Negate(matrix); }
    /// <summary>Does a standard matrix addition.</summary>
    /// <param name="left">The left matrix of the addition.</param>
    /// <param name="right">The right matrix of the addition.</param>
    /// <returns>The resulting matrix after teh addition.</returns>
    public static Matrix operator +(Matrix left, Matrix right) { return Matrix.Add(left, right); }
    /// <summary>Does a standard matrix subtraction.</summary>
    /// <param name="left">The left matrix of the subtraction.</param>
    /// <param name="right">The right matrix of the subtraction.</param>
    /// <returns>The result of the matrix subtraction.</returns>
    public static Matrix operator -(Matrix left, Matrix right) { return Matrix.Subtract(left, right); }
    /// <summary>Does a standard matrix multiplication.</summary>
    /// <param name="left">The left matrix of the multiplication.</param>
    /// <param name="right">The right matrix of the multiplication.</param>
    /// <returns>The resulting matrix after the multiplication.</returns>
    public static Matrix operator *(Matrix left, Matrix right) { return Matrix.Multiply(left, right); }
    /// <summary>Multiplies all the values in a matrix by a scalar.</summary>
    /// <param name="left">The matrix to have its values multiplied.</param>
    /// <param name="right">The scalar to multiply the values by.</param>
    /// <returns>The resulting matrix after the multiplications.</returns>
    public static Matrix operator *(Matrix left, float right) { return Matrix.Multiply(left, right); }
    /// <summary>Multiplies all the values in a matrix by a scalar.</summary>
    /// <param name="left">The scalar to multiply the values by.</param>
    /// <param name="right">The matrix to have its values multiplied.</param>
    /// <returns>The resulting matrix after the multiplications.</returns>
    public static Matrix operator *(float left, Matrix right) { return Matrix.Multiply(right, left); }
    /// <summary>Divides all the values in a matrix by a scalar.</summary>
    /// <param name="left">The matrix to have its values divided.</param>
    /// <param name="right">The scalar to divide the values by.</param>
    /// <returns>The resulting matrix after the divisions.</returns>
    public static Matrix operator /(Matrix left, float right) { return Matrix.Divide(left, right); }
    /// <summary>Applies a power to a matrix.</summary>
    /// <param name="left">The matrix to apply a power to.</param>
    /// <param name="right">The power to apply to the matrix.</param>
    /// <returns>The result of the power operation.</returns>
    public static Matrix operator ^(Matrix left, int right) { return Matrix.Power(left, right); }
    /// <summary>Checks for equality by value.</summary>
    /// <param name="left">The left matrix of the equality check.</param>
    /// <param name="right">The right matrix of the equality check.</param>
    /// <returns>True if the values of the matrices are equal, false if not.</returns>
    public static bool operator ==(Matrix left, Matrix right) { return Matrix.EqualsByValue(left, right); }
    /// <summary>Checks for false-equality by value.</summary>
    /// <param name="left">The left matrix of the false-equality check.</param>
    /// <param name="right">The right matrix of the false-equality check.</param>
    /// <returns>True if the values of the matrices are not equal, false if they are.</returns>
    public static bool operator !=(Matrix left, Matrix right) { return !Matrix.EqualsByValue(left, right); }
    /// <summary>Automatically converts a matrix into a float[,] if necessary.</summary>
    /// <param name="matrix">The matrix to convert to a float[,].</param>
    /// <returns>The reference to the float[,] representing the matrix.</returns>
    public static implicit operator float[,](Matrix matrix) { return matrix.Floats; }

    /// <summary>Negates all the values in this matrix.</summary>
    /// <returns>The resulting matrix after the negations.</returns>
    private Matrix Negate() { return Matrix.Negate(this); }
    /// <summary>Does a standard matrix addition.</summary>
    /// <param name="right">The matrix to add with with matrix.</param>
    /// <returns>The resulting matrix after the addition.</returns>
    private Matrix Add(Matrix right) { return Matrix.Add(this, right); }
    /// <summary>Does a standard matrix multiplication (triple for loop).</summary>
    /// <param name="right">The matrix to multiply this matrix with.</param>
    /// <returns>The resulting matrix after the multiplication.</returns>
    private Matrix Multiply(Matrix right) { return Matrix.Multiply(this, right); }
    /// <summary>Multiplies all the values in this matrix by a scalar.</summary>
    /// <param name="right">The scalar to multiply all the matrix values by.</param>
    /// <returns>The retulting matrix after the multiplications.</returns>
    private Matrix Multiply(float right) { return Matrix.Multiply(this, right); }
    /// <summary>Divides all the values in this matrix by a scalar.</summary>
    /// <param name="right">The scalar to divide the matrix values by.</param>
    /// <returns>The resulting matrix after teh divisions.</returns>
    private Matrix Divide(float right) { return Matrix.Divide(this, right); }
    /// <summary>Gets the minor of a matrix.</summary>
    /// <param name="row">The restricted row of the minor.</param>
    /// <param name="column">The restricted column of the minor.</param>
    /// <returns>The minor from the row/column restrictions.</returns>
    public Matrix Minor(int row, int column) { return Matrix.Minor(this, row, column); }
    /// <summary>Combines two matrices from left to right 
    /// (result.Rows = left.Rows && result.Columns = left.Columns + right.Columns).</summary>
    /// <param name="right">The matrix to combine with on the right side.</param>
    /// <returns>The resulting row-wise concatination.</returns>
    public Matrix ConcatenateRowWise(Matrix right) { return Matrix.ConcatenateRowWise(this, right); }
    /// <summary>Computes the determinent if this matrix is square.</summary>
    /// <returns>The computed determinent if this matrix is square.</returns>
    public float Determinent() { return Matrix.Determinent(this); }
    /// <summary>Computes the echelon form of this matrix (aka REF).</summary>
    /// <returns>The computed echelon form of this matrix (aka REF).</returns>
    public Matrix Echelon() { return Matrix.Echelon(this); }
    /// <summary>Computes the reduced echelon form of this matrix (aka RREF).</summary>
    /// <returns>The computed reduced echelon form of this matrix (aka RREF).</returns>
    public Matrix ReducedEchelon() { return Matrix.ReducedEchelon(this); }
    /// <summary>Computes the inverse of this matrix.</summary>
    /// <returns>The inverse of this matrix.</returns>
    public Matrix Inverse() { return Matrix.Inverse(this); }
    /// <summary>Gets the adjoint of this matrix.</summary>
    /// <returns>The adjoint of this matrix.</returns>
    public Matrix Adjoint() { return Matrix.Adjoint(this); }
    /// <summary>Transposes this matrix.</summary>
    /// <returns>The transpose of this matrix.</returns>
    public Matrix Transpose() { return Matrix.Transpose(this); }
    /// <summary>Copies this matrix.</summary>
    /// <returns>The copy of this matrix.</returns>
    public Matrix Clone() { return Matrix.Clone(this); }

    /// <summary>Negates all the values in a matrix.</summary>
    /// <param name="matrix">The matrix to have its values negated.</param>
    /// <returns>The resulting matrix after the negations.</returns>
    public static Matrix Negate(float[,] matrix)
    {
      Matrix result = new Matrix(matrix.GetLength(0), matrix.GetLength(1));
      for (int i = 0; i < matrix.GetLength(0); i++)
        for (int j = 0; j < matrix.GetLength(1); j++)
          result[i, j] = -matrix[i, j];
      return result;
    }

    /// <summary>Does standard addition of two matrices.</summary>
    /// <param name="left">The left matrix of the addition.</param>
    /// <param name="right">The right matrix of the addition.</param>
    /// <returns>The resulting matrix after the addition.</returns>
    public static Matrix Add(float[,] left, float[,] right)
    {
      if (left.GetLength(0) != right.GetLength(0) || left.GetLength(1) != right.GetLength(1))
        throw new MatrixException("invalid addition (size miss-match).");
      Matrix result = new Matrix(left.GetLength(0), left.GetLength(1));
      for (int i = 0; i < result.Rows; i++)
        for (int j = 0; j < result.Columns; j++)
          result[i, j] = left[i, j] + right[i, j];
      return result;
    }

    /// <summary>Subtracts a scalar from all the values in a matrix.</summary>
    /// <param name="left">The matrix to have the values subtracted from.</param>
    /// <param name="right">The scalar to subtract from all the matrix values.</param>
    /// <returns>The resulting matrix after the subtractions.</returns>
    public static Matrix Subtract(float[,] left, float[,] right)
    {
      if (left.GetLength(0) != right.GetLength(0) || left.GetLength(1) != right.GetLength(1))
        throw new MatrixException("invalid subtraction (size miss-match).");
      Matrix result = new Matrix(left.GetLength(0), left.GetLength(1));
      for (int i = 0; i < result.Rows; i++)
        for (int j = 0; j < result.Columns; j++)
          result[i, j] = left[i, j] - right[i, j];
      return result;
    }

    /// <summary>Does a standard (triple for looped) multiplication between matrices.</summary>
    /// <param name="left">The left matrix of the multiplication.</param>
    /// <param name="right">The right matrix of the multiplication.</param>
    /// <returns>The resulting matrix of the multiplication.</returns>
    public static Matrix Multiply(float[,] left, float[,] right)
    {
      if (left.GetLength(1) != right.GetLength(0))
        throw new MatrixException("invalid multiplication (size miss-match).");
      Matrix result = new Matrix(left.GetLength(0), right.GetLength(1));
      for (int i = 0; i < result.Rows; i++)
        for (int j = 0; j < result.Columns; j++)
          for (int k = 0; k < left.GetLength(1); k++)
            result[i, j] += left[i, k] * right[k, j];
      return result;
    }

    /// <summary>Multiplies all the values in a matrix by a scalar.</summary>
    /// <param name="left">The matrix to have the values multiplied.</param>
    /// <param name="right">The scalar to multiply the values by.</param>
    /// <returns>The resulting matrix after the multiplications.</returns>
    public static Matrix Multiply(float[,] left, float right)
    {
      Matrix result = new Matrix(left.GetLength(0), left.GetLength(1));
      for (int i = 0; i < left.GetLength(0); i++)
        for (int j = 0; j < left.GetLength(1); j++)
          result[i, j] = left[i, j] * right;
      return result;
    }

    /// <summary>Applies a power to a square matrix.</summary>
    /// <param name="matrix">The matrix to be powered by.</param>
    /// <param name="power">The power to apply to the matrix.</param>
    /// <returns>The resulting matrix of the power operation.</returns>
    public static Matrix Power(float[,] matrix, int power)
    {
      if (!(matrix.GetLength(0) == matrix.GetLength(1)))
        throw new MatrixException("invalid power (!matrix.IsSquare).");
      if (!(power > -1))
        throw new MatrixException("invalid power !(power > -1)");
      if (power == 0)
        return Matrix.FactoryIdentity(matrix.GetLength(0), matrix.GetLength(1));
      Matrix result = Matrix.Clone(matrix);
      for (int i = 0; i < power; i++)
        result *= result;
      return result;
    }

    /// <summary>Divides all the values in the matrix by a scalar.</summary>
    /// <param name="matrix">The matrix to divide the values of.</param>
    /// <param name="right">The scalar to divide all the matrix values by.</param>
    /// <returns>The resulting matrix with the divided values.</returns>
    public static Matrix Divide(float[,] matrix, float right)
    {
      Matrix result = new Matrix(matrix.GetLength(0), matrix.GetLength(1));
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
    public static Matrix Minor(float[,] matrix, int row, int column)
    {
      Matrix minor = new Matrix(matrix.GetLength(0) - 1, matrix.GetLength(1) - 1);
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

    private static void RowAddition(float[,] matrix, int target, int second, float multiple)
    {
      for (int j = 0; j < matrix.GetLength(1); j++)
        matrix[target, j] += (matrix[second, j] * multiple);
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
    public static Matrix ConcatenateRowWise(float[,] left, float[,] right)
    {
      if (left.GetLength(0) != right.GetLength(0))
        throw new MatrixException("invalid row-wise concatenation !(left.Rows == right.Rows).");
      Matrix result = new Matrix(left.GetLength(0), left.GetLength(1) + right.GetLength(1));
      for (int i = 0; i < result.Rows; i++)
        for (int j = 0; j < result.Columns; j++)
        {
          if (j < left.GetLength(1)) result[i, j] = left[i, j];
          else result[i, j] = right[i, j - left.GetLength(1)];
        }
      return result;
    }

    /// <summary>Calculates the determinent of a square matrix.</summary>
    /// <param name="matrix">The matrix to calculate the determinent of.</param>
    /// <returns>The determinent of the matrix.</returns>
    public static float Determinent(float[,] matrix)
    {
      if (!(matrix.GetLength(0) == matrix.GetLength(1)))
        throw new MatrixException("invalid determinent !(matrix.IsSquare).");
      float det = 1.0f;
      try
      {
        Matrix rref = Matrix.Clone(matrix);
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
          if (rref[i, i] == 0)
            for (int j = i + 1; j < rref.Rows; j++)
              if (rref[j, i] != 0)
              {
                Matrix.SwapRows(rref, i, j);
                det *= -1;
              }
          det *= rref[i, i];
          Matrix.RowMultiplication(rref, i, 1 / rref[i, i]);
          for (int j = i + 1; j < rref.Rows; j++)
            Matrix.RowAddition(rref, j, i, -rref[j, i]);
          for (int j = i - 1; j >= 0; j--)
            Matrix.RowAddition(rref, j, i, -rref[j, i]);
        }
        return det;
      }
      catch (Exception)
      {
        throw new MatrixException("determinent computation failed.");
      }
    }

    /// <summary>Calculates the echelon of a matrix (aka REF).</summary>
    /// <param name="matrix">The matrix to calculate the echelon of (aka REF).</param>
    /// <returns>The echelon of the matrix (aka REF).</returns>
    public static Matrix Echelon(float[,] matrix)
    {
      try
      {
        Matrix result = Matrix.Clone(matrix);
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
          if (result[i, i] == 0)
            for (int j = i + 1; j < result.Rows; j++)
              if (result[j, i] != 0)
                Matrix.SwapRows(result, i, j);
          if (result[i, i] == 0)
            continue;
          if (result[i, i] != 1)
            for (int j = i + 1; j < result.Rows; j++)
              if (result[j, i] == 1)
                Matrix.SwapRows(result, i, j);
          Matrix.RowMultiplication(result, i, 1 / result[i, i]);
          for (int j = i + 1; j < result.Rows; j++)
            Matrix.RowAddition(result, j, i, -result[j, i]);
        }
        return result;
      }
      catch { throw new MatrixException("echelon computation failed."); }
    }

    /// <summary>Calculates the echelon of a matrix and reduces it (aka RREF).</summary>
    /// <param name="matrix">The matrix matrix to calculate the reduced echelon of (aka RREF).</param>
    /// <returns>The reduced echelon of the matrix (aka RREF).</returns>
    public static Matrix ReducedEchelon(float[,] matrix)
    {
      try
      {
        Matrix result = Matrix.Clone(matrix);
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
          if (result[i, i] == 0)
            for (int j = i + 1; j < result.Rows; j++)
              if (result[j, i] != 0)
                Matrix.SwapRows(result, i, j);
          if (result[i, i] == 0) continue;
          if (result[i, i] != 1)
            for (int j = i + 1; j < result.Rows; j++)
              if (result[j, i] == 1)
                Matrix.SwapRows(result, i, j);
          Matrix.RowMultiplication(result, i, 1 / result[i, i]);
          for (int j = i + 1; j < result.Rows; j++)
            Matrix.RowAddition(result, j, i, -result[j, i]);
          for (int j = i - 1; j >= 0; j--)
            Matrix.RowAddition(result, j, i, -result[j, i]);
        }
        return result;
      }
      catch { throw new MatrixException("reduced echelon calculation failed."); }
    }

    /// <summary>Calculates the inverse of a matrix.</summary>
    /// <param name="matrix">The matrix to calculate the inverse of.</param>
    /// <returns>The inverse of the matrix.</returns>
    public static Matrix Inverse(float[,] matrix)
    {
      if (Matrix.Determinent(matrix) == 0)
        throw new MatrixException("inverse calculation failed.");
      try
      {
        Matrix identity = Matrix.FactoryIdentity(matrix.GetLength(0), matrix.GetLength(1));
        Matrix rref = Matrix.Clone(matrix);
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
          if (rref[i, i] == 0)
            for (int j = i + 1; j < rref.Rows; j++)
              if (rref[j, i] != 0)
              {
                Matrix.SwapRows(rref, i, j);
                Matrix.SwapRows(identity, i, j);
              }
          Matrix.RowMultiplication(identity, i, 1 / rref[i, i]);
          Matrix.RowMultiplication(rref, i, 1 / rref[i, i]);
          for (int j = i + 1; j < rref.Rows; j++)
          {
            Matrix.RowAddition(identity, j, i, -rref[j, i]);
            Matrix.RowAddition(rref, j, i, -rref[j, i]);
          }
          for (int j = i - 1; j >= 0; j--)
          {
            Matrix.RowAddition(identity, j, i, -rref[j, i]);
            Matrix.RowAddition(rref, j, i, -rref[j, i]);
          }
        }
        return identity;
      }
      catch { throw new MatrixException("inverse calculation failed."); }
    }

    /// <summary>Calculates the adjoint of a matrix.</summary>
    /// <param name="matrix">The matrix to calculate the adjoint of.</param>
    /// <returns>The adjoint of the matrix.</returns>
    public static Matrix Adjoint(float[,] matrix)
    {
      if (!(matrix.GetLength(0) == matrix.GetLength(1)))
        throw new MatrixException("Adjoint of a non-square matrix does not exists");
      Matrix AdjointMatrix = new Matrix(matrix.GetLength(0), matrix.GetLength(1));
      for (int i = 0; i < matrix.GetLength(0); i++)
        for (int j = 0; j < matrix.GetLength(1); j++)
          if ((i + j) % 2 == 0)
            AdjointMatrix[i, j] = Matrix.Determinent(Matrix.Minor(matrix, i, j));
          else
            AdjointMatrix[i, j] = -Matrix.Determinent(Matrix.Minor(matrix, i, j));
      return Matrix.Transpose(AdjointMatrix);
    }

    /// <summary>Returns the transpose of a matrix.</summary>
    /// <param name="matrix">The matrix to transpose.</param>
    /// <returns>The transpose of the matrix.</returns>
    public static Matrix Transpose(float[,] matrix)
    {
      Matrix transpose = new Matrix(matrix.GetLength(1), matrix.GetLength(0));
      for (int i = 0; i < transpose.Rows; i++)
        for (int j = 0; j < transpose.Columns; j++)
          transpose[i, j] = matrix[j, i];
      return transpose;
    }

    #region Work-In-Progress-functions

    //public static void DecomposeLU(float[,] matrix, out Matrix Lower, out Matrix Upper)
    //{
    //  if (!(matrix.GetLength(0) == matrix.GetLength(1)))
    //    throw new MatrixException("The matrix is not square!");

    //  Lower = Matrix.FactoryIdentity(matrix.GetLength(0), matrix.GetLength(1));
    //  Upper = Matrix.Clone(matrix);
    //  int[] pi = new int[matrix.GetLength(0)];
    //  for (int i = 0; i < matrix.GetLength(0); i++) pi[i] = i;
    //  float p = 0;
    //  float pom2;
    //  int k0 = 0;
    //  int pom1 = 0;
    //  float detOfP = 1;

    //  for (int k = 0; k < matrix.GetLength(1) - 1; k++)
    //  {
    //    p = 0;
    //    for (int i = k; i < matrix.GetLength(0); i++)      // find the row with the biggest pivot
    //    {
    //      if (Math.Abs(Upper[i, k]) > p)
    //      {
    //        p = Math.Abs(Upper[i, k]);
    //        k0 = i;
    //      }
    //    }
    //    if (p == 0) // samé nuly ve sloupci
    //      throw new MatrixException("The matrix is singular!");

    //    pom1 = pi[k]; pi[k] = pi[k0]; pi[k0] = pom1;    // switch two rows in permutation matrix

    //    for (int i = 0; i < k; i++)
    //    {
    //      pom2 = Lower[k, i]; Lower[k, i] = Lower[k0, i]; Lower[k0, i] = pom2;
    //    }

    //    if (k != k0) detOfP *= -1;

    //    for (int i = 0; i < matrix.GetLength(1); i++)                  // Switch rows in U
    //    {
    //      pom2 = Upper[k, i]; Upper[k, i] = Upper[k0, i]; Upper[k0, i] = pom2;
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

    /// <summary>Creates a copy of a matrix.</summary>
    /// <param name="matrix">The matrix to copy.</param>
    /// <returns>A copy of the matrix.</returns>
    public static Matrix Clone(float[,] matrix)
    {
      Matrix result = new Matrix(matrix.GetLength(0), matrix.GetLength(1));
      for (int i = 0; i < matrix.GetLength(0); i++)
        for (int j = 0; j < matrix.GetLength(1); j++)
          result[i, j] = matrix[i, j];
      return result;
    }

    /// <summary>Does a value equality check.</summary>
    /// <param name="left">The first matrix to check for equality.</param>
    /// <param name="right">The second matrix to check for equality.</param>
    /// <returns>True if values are equal, false if not.</returns>
    public static bool EqualsByValue(float[,] left, float[,] right)
    {
      if (left.GetLength(0) != right.GetLength(0) || left.GetLength(1) != right.GetLength(1))
        return false;
      for (int i = 0; i < left.GetLength(0); i++)
        for (int j = 0; j < left.GetLength(1); j++)
          if (left[i, j] != right[i, j])
            return false;
      return true;
    }

    /// <summary>Checks if two matrices are equal by reverences.</summary>
    /// <param name="left">The left matric of the equality check.</param>
    /// <param name="right">The right matrix of the equality check.</param>
    /// <returns>True if the references are equal, false if not.</returns>
    public static bool EqualsByReference(float[,] left, float[,] right)
    {
      return left.Equals(right);
    }

    /// <summary>CHANGE THIS FUNCTION TO YOUR SPECIFIC NEEDS.</summary>
    /// <returns>THE BASE OBJECT STRING REPRESENTATION.</returns>
    public override string ToString()
    {
      return base.ToString();
      //StringBuilder matrix = new StringBuilder();
      //for (int i = 0; i < Rows; i++)
      //{
      //  for (int j = 0; j < Columns; j++)
      //    matrix.Append(String.Format("{0:0.##}\t", _matrix[i, j]));
      //  matrix.Append("\n");
      //}
      //return matrix.ToString();
    }

    /// <summary>WARNING: THIS METHOD IS VERY COMPUTATIONALLY EXPENSIVE (YOU MAY WANT TO CHANGE IT).</summary>
    /// <returns>A hash code for the matrix.</returns>
    public override int GetHashCode()
    {
      // return base.GetHashCode();
      int hash = _matrix[0, 0].GetHashCode();
      for (int j = 1; j < Columns; j++)
        hash = hash ^ _matrix[0, j].GetHashCode();
      for (int i = 0; i < Rows; i++)
        for (int j = 0; j < Columns; j++)
          hash = hash ^ _matrix[i, j].GetHashCode();
      return hash;
    }

    /// <summary>Does a value equality check by reference.</summary>
    /// <param name="obj">The object to compare to.</param>
    /// <returns>True if the references are equal, false if not.</returns>
    public override bool Equals(object obj)
    {
      return base.Equals(obj) || _matrix.Equals(obj);
      //if (obj is Matrix || obj is float[,])
      //  return Matrix.Equals(this, (float[,])obj);
      //else
      //  return false;
    }

    private class MatrixException : Exception
    {
      public MatrixException(string Message) : base(Message) { }
    }

    #region Alternate Compututation Methods

    //public static float Determinent(Matrix matrix)
    //{
    //  if (!matrix.IsSquare)
    //    throw new MatrixException("invalid determinent !(matrix.IsSquare).");
    //  return DeterminentRecursive(matrix);
    //}
    //private static float DeterminentRecursive(Matrix matrix)
    //{
    //  if (matrix.Rows == 1)
    //    return matrix[0, 0];
    //  float det = 0.0f;
    //  for (int j = 0; j < matrix.Columns; j++)
    //    det += (matrix[0, j] * DeterminentRecursive(Matrix.Minor(matrix, 0, j)) * (int)System.Math.Pow(-1, 0 + j));
    //  return det;
    //}

    //public static Matrix Inverse(Matrix matrix)
    //{
    //  float determinent = Matrix.Determinent(matrix);
    //  if (determinent == 0)
    //    throw new MatrixException("inverse calculation failed.");
    //  return Matrix.Adjoint(matrix) / determinent;
    //}

    #endregion
  }
}
