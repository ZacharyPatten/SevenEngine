﻿// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use under the following condition: any code using or originally taken 
// from the SevenEngine project must include citation to its original author(s) located at the
// top of each source code file, or you may include a reference to the SevenEngine project as
// a whole but you must include the current SevenEngine official website URL and logo.
// - Thanks.  :)  (support: seven@sevenengine.com)

// Author(s):
// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com
// Last Edited: 11-16-13

using System;
using System.Text;

namespace SevenEngine.Mathematics
{
  public class Matrix
  {
    protected float[,] _matrix;
    
    public float[,] Values { get { return (float[,])_matrix.Clone(); } }
    public int Rows { get { return _matrix.GetLength(0); } }
    public int Columns { get { return _matrix.GetLength(1); } }
    public bool IsSquare { get { return Rows == Columns; } }

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

    public Matrix(float[,] matrix)
    {
      // _matrix = matrix; // To make is fool proof or not? Speed increase here!
      _matrix = (float[,])matrix.Clone();
    }

    public static Matrix FactoryZero(int rows, int columns)
    {
      try { return new Matrix(new float[rows, columns]); }
      catch { throw new MatrixException("invalid dimensions."); }
    }

    public static Matrix FactoryIdentity(int rows, int columns)
    {
      float[,] matrix;
      try { matrix = new float[rows, columns]; }
      catch { throw new MatrixException("invalid dimensions."); }
      if (rows <= columns)
        for (int i = 0; i < rows; i++)
          matrix[i, i] = 1;
      else
        for (int i = 0; i < columns; i++)
          matrix[i, i] = 1;
      return new Matrix(matrix);
    }

    public static Matrix FactoryOne(int rows, int columns)
    {
      float[,] matrix;
      try { matrix = new float[rows, columns]; }
      catch { throw new MatrixException("invalid dimensions."); }
      for (int i = 0; i < rows; i++)
        for (int j = 0; j < columns; j++)
          matrix[i, j] = 1;
      return new Matrix(matrix);
    }

    public static Matrix FactoryUniform(int rows, int columns, float uniform)
    {
      float[,] matrix;
      try { matrix = new float[rows, columns]; }
      catch { throw new MatrixException("invalid dimensions."); }
      for (int i = 0; i < rows; i++)
        for (int j = 0; j < columns; j++)
          matrix[i, j] = uniform;
      return new Matrix(matrix);
    }

    public static Matrix operator -(Matrix matrix) { return Matrix.Negate(matrix); }
    public static Matrix operator +(Matrix left, Matrix right) { return Matrix.Add(left, right); }
    public static Matrix operator -(Matrix left, Matrix right) { return Matrix.Subtract(left, right); }
    public static Matrix operator *(Matrix left, Matrix right) { return Matrix.Multiply(left, right); }
    public static Matrix operator *(Matrix left, float right) { return Matrix.Multiply(left, right); }
    public static Matrix operator *(float left, Matrix right) { return Matrix.Multiply(right, left); }
    public static Matrix operator /(Matrix left, float right) { return Matrix.Divide(left, right); }
    public static Matrix operator ^(Matrix left, int right) { return Matrix.Power(left, right); }
    public static bool operator ==(Matrix left, Matrix right) { return Matrix.Equals(left, right); }
    public static bool operator !=(Matrix left, Matrix right) { return !Matrix.Equals(left, right); }
    public static implicit operator float[,](Matrix matrix) { return matrix.Values; }

    private Matrix Negate() { return Matrix.Negate(this); }
    private Matrix Add(Matrix right) { return Matrix.Add(this, right); }
    private Matrix Multiply(Matrix right) { return Matrix.Multiply(this, right); }
    private Matrix Multiply(float right) { return Matrix.Multiply(this, right); }
    private Matrix Divide(float right) { return Matrix.Divide(this, right); }
    public Matrix Minor(int row, int column) { return Matrix.Minor(this, row, column); }
    public Matrix ConcatenateByRows(Matrix right) { return Matrix.ConcatenateRowWise(this, right); }
    public float Determinent() { return Matrix.Determinent(this); }
    public Matrix Echelon() { return Matrix.Echelon(this); }
    public Matrix ReducedEchelon() { return Matrix.ReducedEchelon(this); }
    public Matrix Inverse() { return Matrix.Inverse(this); }
    public Matrix Adjoint() { return Matrix.Adjoint(this); }
    public Matrix Transpose() { return Matrix.Transpose(this); }
    public Matrix Clone() { return Matrix.Clone(this); }

    public static Matrix Negate(Matrix matrix)
    {
      Matrix result = Matrix.FactoryZero(matrix.Rows, matrix.Columns);
      for (int i = 0; i < matrix.Rows; i++)
        for (int j = 0; j < matrix.Columns; j++)
          result[i, j] = -matrix[i, j];
      return result;
    }

    public static Matrix Add(Matrix left, Matrix right)
    {
      if (left.Rows != right.Rows || left.Columns != right.Columns)
        throw new MatrixException("invalid addition (size miss-match).");
      Matrix result = Matrix.FactoryZero(left.Rows, left.Columns);
      for (int i = 0; i < result.Rows; i++)
        for (int j = 0; j < result.Columns; j++)
          result[i, j] = left[i, j] + right[i, j];
      return result;
    }

    public static Matrix Subtract(Matrix left, Matrix right)
    {
      if (left.Rows != right.Rows || left.Columns != right.Columns)
        throw new MatrixException("invalid subtraction (size miss-match).");
      Matrix result = Matrix.FactoryZero(left.Rows, left.Columns);
      for (int i = 0; i < result.Rows; i++)
        for (int j = 0; j < result.Columns; j++)
          result[i, j] = left[i, j] - right[i, j];
      return result;
    }

    public static Matrix Multiply(Matrix left, Matrix right)
    {
      if (left.Columns != right.Rows)
        throw new MatrixException("invalid multiplication (size miss-match).");
      Matrix result = Matrix.FactoryZero(left.Rows, right.Columns);
      for (int i = 0; i < result.Rows; i++)
        for (int j = 0; j < result.Columns; j++)
          for (int k = 0; k < left.Columns; k++)
            result[i, j] += left[i, k] * right[k, j];
      return result;
    }

    public static Matrix Multiply(Matrix matrix, float right)
    {
      Matrix result = Matrix.FactoryZero(matrix.Rows, matrix.Columns);
      for (int i = 0; i < matrix.Rows; i++)
        for (int j = 0; j < matrix.Columns; j++)
          result[i, j] = matrix[i, j] * right;
      return result;
    }

    public static Matrix Power(Matrix matrix, int power)
    {
      if (!matrix.IsSquare)
        throw new MatrixException("invalid power (!matrix.IsSquare).");
      Matrix result = matrix.Clone();
      for (int i = 0; i < power; i++)
        result *= result;
      return result;
    }

    public static Matrix Divide(Matrix matrix, float right)
    {
      Matrix result = Matrix.FactoryZero(matrix.Rows, matrix.Columns);
      for (int i = 0; i < matrix.Rows; i++)
        for (int j = 0; j < matrix.Columns; j++)
          result[i, j] = matrix[i, j] / right;
      return result;
    }

    public static Matrix Minor(Matrix matrix, int row, int column)
    {
      Matrix minor = Matrix.FactoryZero(matrix.Rows - 1, matrix.Columns - 1);
      int m = 0, n = 0;
      for (int i = 0; i < matrix.Rows; i++)
      {
        if (i == row) continue;
        n = 0;
        for (int j = 0; j < matrix.Columns; j++)
        {
          if (j == column) continue;
          minor[m, n] = matrix[i, j];
          n++;
        }
        m++;
      }
      return minor;
    }

    private static void RowMultiplication(Matrix matrix, int row, float scalar)
    {
      for (int j = 0; j < matrix.Columns; j++)
        matrix[row, j] *= scalar;
    }

    private static void RowAddition(Matrix matrix, int target, int second, float multiple)
    {
      for (int j = 0; j < matrix.Columns; j++)
        matrix[target, j] += (matrix[second, j] * multiple);
    }

    private static void SwapRows(Matrix matrix, int row1, int row2)
    {
      for (int j = 0; j < matrix.Columns; j++)
      {
        float temp = matrix[row1, j];
        matrix[row1, j] = matrix[row2, j];
        matrix[row2, j] = temp;
      }
    }

    public static Matrix ConcatenateRowWise(Matrix left, Matrix right)
    {
      if (left.Rows != right.Rows)
        throw new MatrixException("invalid row-wise concatenation !(left.Rows == right.Rows).");
      float[,] result = new float[left.Rows, left.Columns + right.Columns];
      for (int i = 0; i < result.GetLength(0); i++)
        for (int j = 0; j < result.GetLength(1); j++)
        {
          if (j < left.Columns) result[i, j] = left[i, j];
          else result[i, j] = right[i, j - left.Columns];
        }
      return new Matrix(result);
    }

    public static float Determinent(Matrix matrix)
    {
      if (!matrix.IsSquare)
        throw new MatrixException("invalid determinent !(matrix.IsSquare).");
      float det = 1.0f;
      try
      {
        Matrix rref = Matrix.Clone(matrix);
        for (int i = 0; i < matrix.Rows; i++)
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

    public static Matrix Echelon(Matrix matrix)
    {
      try
      {
        Matrix result = Matrix.Clone(matrix);
        for (int i = 0; i < matrix.Rows; i++)
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
            Matrix.RowAddition(result, j, i, (int)-result[j, i]);
        }
        return result;
      }
      catch { throw new MatrixException("echelon computation failed."); }
    }

    public static Matrix ReducedEchelon(Matrix matrix)
    {
      try
      {
        Matrix result = Matrix.Clone(matrix);
        for (int i = 0; i < matrix.Rows; i++)
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
          Matrix.RowMultiplication(result, i, result[i, i]);
          for (int j = i + 1; j < result.Rows; j++)
            Matrix.RowAddition(result, j, i, -result[j, i]);
          for (int j = i - 1; j >= 0; j--)
            Matrix.RowAddition(result, j, i, -result[j, i]);
        }
        return result;
      }
      catch { throw new MatrixException("reduced echelon calculation failed."); }
    }

    public static Matrix Inverse(Matrix matrix)
    {
      if (Matrix.Determinent(matrix) == 0)
        throw new MatrixException("inverse calculation failed.");
      try
      {
        Matrix identity = Matrix.FactoryIdentity(matrix.Rows, matrix.Columns);
        Matrix rref = Matrix.Clone(matrix);
        for (int i = 0; i < matrix.Rows; i++)
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

    public static Matrix Adjoint(Matrix matrix)
    {
      if (!matrix.IsSquare)
        throw new MatrixException("Adjoint of a non-square matrix does not exists");
      Matrix AdjointMatrix = Matrix.FactoryZero(matrix.Rows, matrix.Columns);
      for (int i = 0; i < matrix.Rows; i++)
        for (int j = 0; j < matrix.Columns; j++)
          if ((i + j) % 2 == 0)
            AdjointMatrix[i, j] = Matrix.Determinent(Matrix.Minor(matrix, i, j));
          else
            AdjointMatrix[i, j] = -Matrix.Determinent(Matrix.Minor(matrix, i, j));
      return Matrix.Transpose(AdjointMatrix); ;
    }

    public static Matrix Transpose(Matrix matrix)
    {
      Matrix transpose = Matrix.FactoryZero(matrix.Columns, matrix.Rows);
      for (int i = 0; i < transpose.Rows; i++)
        for (int j = 0; j < transpose.Columns; j++)
          transpose[i, j] = matrix[j, i];
      return transpose;
    }

    public static Matrix Clone(Matrix matrix)
    {
      Matrix result = Matrix.FactoryZero(matrix.Rows, matrix.Columns);
      for (int i = 0; i < matrix.Rows; i++)
        for (int j = 0; j < matrix.Columns; j++)
          result[i, j] = matrix[i, j];
      return result;
    }

    public static bool Equals(Matrix left, Matrix right)
    {
      if (left.Rows != right.Rows || left.Columns != right.Columns)
        return false;
      for (int i = 0; i < left.Rows; i++)
        for (int j = 0; j < left.Columns; j++)
          if (left[i, j] != right[i, j])
            return false;
      return true;
    }

    public override string ToString()
    {
      // return base.ToString();
      StringBuilder matrix = new StringBuilder();
      for (int i = 0; i < Rows; i++)
      {
        for (int j = 0; j < Columns; j++)
          matrix.Append(_matrix[i, j] + " ");
        matrix.Append("\n");
      }
      return matrix.ToString();
    }

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

    public override bool Equals(object obj)
    {
      return base.Equals(obj);
    }

    private class MatrixException : Exception
    {
      public MatrixException(string Message) : base(Message) { }
    }

    #region Alternate Methods

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
    //    throw new MatrixException("Inverse of a singular matrix is not possible");
    //  return Matrix.Adjoint(matrix) / determinent;
    //}

    #endregion
  }
}
