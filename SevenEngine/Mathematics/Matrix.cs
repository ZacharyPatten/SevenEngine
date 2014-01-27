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
  /// <summary>A matrix implemented as a flattened float array to perform matrix theory in row major order. Enjoy :)</summary>
  public class Matrix
  {
    private float[] _matrix;
    private int _columns;
    private int _rows;

    /// <summary>The float[] reference of this matrix.</summary>
    public float[] Floats 
    {
      get { return _matrix; }
      set
      {
        if (value.Length == _rows * _columns) _matrix = value; 
        else throw new MatrixException("you cannot change the dimension of matrix when setting its float values");
      }
    }
    /// <summary>The number of rows in the matrix.</summary>
    public int Rows { get { return _rows; } }
    /// <summary>The number of columns in the matrix.</summary>
    public int Columns { get { return _columns; } }
    /// <summary>The number of elements in the matrix (rows * columns).</summary>
    public int Size { get { return _matrix.Length; } }
    /// <summary>Determines if the matrix is square.</summary>
    public bool IsSquare { get { return _rows == _columns; } }
    /// <summary>Determines if the matrix is a vector.</summary>
    public bool IsVector { get { return _columns == 1; } }
    /// <summary>Determines if the matrix is a 2 component vector.</summary>
    public bool Is2x1 { get { return _rows == 2 && _columns == 1; } }
    /// <summary>Determines if the matrix is a 3 component vector.</summary>
    public bool Is3x1 { get { return _rows == 3 && _columns == 1; } }
    /// <summary>Determines if the matrix is a 4 component vector.</summary>
    public bool Is4x1 { get { return _rows == 4 && _columns == 1; } }
    /// <summary>Determines if the matrix is a 2 square matrix.</summary>
    public bool Is2x2 { get { return _rows == 2 && _columns == 2; } }
    /// <summary>Determines if the matrix is a 3 square matrix.</summary>
    public bool Is3x3 { get { return _rows == 3 && _columns == 3; } }
    /// <summary>Determines if the matrix is a 4 square matrix.</summary>
    public bool Is4x4 { get { return _rows == 4 && _columns == 4; } }

    /// <summary>Standard row-major matrix indexing.</summary>
    /// <param name="row">The row index.</param>
    /// <param name="column">The column index.</param>
    /// <returns>The value at the given indeces.</returns>
    public float this[int row, int column]
    {
      get
      {
        if (row > _rows - 1 || column > _columns - 1)
          throw new MatrixException("index out of bounds.");
        return _matrix[row * _columns + column];
      }
      set
      {
        if (row > _rows - 1 || column > _columns - 1)
          throw new MatrixException("index out of bounds.");
        else _matrix[row * _columns + column] = value;
      }
    }

    /// <summary>Constructs a new zero-matrix of the given dimensions.</summary>
    /// <param name="rows">The number of row dimensions.</param>
    /// <param name="columns">The number of column dimensions.</param>
    public Matrix(int rows, int columns)
    {
      _rows = rows;
      _columns = columns;
      try { _matrix = new float[rows * columns]; }
      catch { throw new MatrixException("invalid dimensions."); }
    }

    /// <summary>Wraps a float[] inside of a matrix class.</summary>
    /// <param name="matrix">The float[] to wrap in a matrix class.</param>
    public Matrix(int rows, int columns, params float[] matrix)
    {
      float[] floats = new float[matrix.Length];
      Buffer.BlockCopy(matrix, 0, floats, 0, floats.Length * sizeof(float));
      _matrix = matrix;
      _columns = columns;
      _rows = rows;
    }

    /// <summary>This is a special constructor to make a vector into a matrix
    /// without copying the data for efficiency purposes.</summary>
    /// <param name="vector">The values the new matrix will point to.</param>
    private Matrix(Vector vector)
    {
      _matrix = vector.Floats;
      _rows = _matrix.Length;
      _columns = 1;
    }

    /// <summary>Constructs a matrix that points to the values in a vector. So the vector and this
    /// new matrix point to the same float[].</summary>
    /// <param name="vector">The vector who will share the data as the constructed matrix.</param>
    /// <returns>The constructed matrix sharing the data with the vector.</returns>
    public static Matrix UnsafeFactoryFromVector(Vector vector) { return new Matrix(vector); }

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

    /// <summary>Constructs a 2-component vector matrix with all values being 0.</summary>
    /// <returns>The constructed 2-component vector matrix.</returns>
    public static Matrix Factory2x1() { return new Matrix(2, 1); }
    /// <summary>Constructs a 3-component vector matrix with all values being 0.</summary>
    /// <returns>The constructed 3-component vector matrix.</returns>
    public static Matrix Factory3x1() { return new Matrix(3, 1); }
    /// <summary>Constructs a 4-component vector matrix with all values being 0.</summary>
    /// <returns>The constructed 4-component vector matrix.</returns>
    public static Matrix Factory4x1() { return new Matrix(4, 1); }

    /// <summary>Constructs a 2x2 matrix with all values being 0.</summary>
    /// <returns>The constructed 2x2 matrix.</returns>
    public static Matrix Factory2x2() { return new Matrix(2, 2); }
    /// <summary>Constructs a 3x3 matrix with all values being 0.</summary>
    /// <returns>The constructed 3x3 matrix.</returns>
    public static Matrix Factory3x3() { return new Matrix(3, 3); }
    /// <summary>Constructs a 4x4 matrix with all values being 0.</summary>
    /// <returns>The constructed 4x4 matrix.</returns>
    public static Matrix Factory4x4() { return new Matrix(4, 4); }

    /// <param name="angle">Angle of rotation in radians.</param>
    public static Matrix Factory3x3RotationX(float angle)
    {
      float cos = Calc.Cos(angle);
      float sin = Calc.Sin(angle);
      return new Matrix(3, 3, new float[] { 1, 0, 0, 0, cos, sin, 0, -sin, cos });
    }

    /// <param name="angle">Angle of rotation in radians.</param>
    public static Matrix Factory3x3RotationY(float angle)
    {
      float cos = Calc.Cos(angle);
      float sin = Calc.Sin(angle);
      return new Matrix(3, 3, new float[] { cos, 0, -sin, 0, 1, 0, sin, 0, cos });
    }

    /// <param name="angle">Angle of rotation in radians.</param>
    public static Matrix Factory3x3RotationZ(float angle)
    {
      float cos = Calc.Cos(angle);
      float sin = Calc.Sin(angle);
      return new Matrix(3, 3, new float[] { cos, -sin, 0, sin, cos, 0, 0, 0, 1 });
    }

    /// <param name="angleX">Angle about the X-axis in radians.</param>
    /// <param name="angleY">Angle about the Y-axis in radians.</param>
    /// <param name="angleZ">Angle about the Z-axis in radians.</param>
    public static Matrix Factory3x3RotationXthenYthenZ(float angleX, float angleY, float angleZ)
    {
      float
        xCos = Calc.Cos(angleX), xSin = Calc.Sin(angleX),
        yCos = Calc.Cos(angleY), ySin = Calc.Sin(angleY),
        zCos = Calc.Cos(angleZ), zSin = Calc.Sin(angleZ);
      return new Matrix(3, 3,
        new float[] {
          yCos * zCos, -yCos * zSin, ySin,
          xCos * zSin + xSin * ySin * zCos, xCos * zCos + xSin * ySin * zSin, -xSin * yCos,
          xSin * zSin - xCos * ySin * zCos, xSin * zCos + xCos * ySin * zSin, xCos * yCos });
    }

    /// <param name="angleX">Angle about the X-axis in radians.</param>
    /// <param name="angleY">Angle about the Y-axis in radians.</param>
    /// <param name="angleZ">Angle about the Z-axis in radians.</param>
    public static Matrix Factory3x3RotationZthenYthenX(float angleX, float angleY, float angleZ)
    {
      float
        xCos = Calc.Cos(angleX), xSin = Calc.Sin(angleX),
        yCos = Calc.Cos(angleY), ySin = Calc.Sin(angleY),
        zCos = Calc.Cos(angleZ), zSin = Calc.Sin(angleZ);
      return new Matrix(3, 3, new float[] { yCos * zCos, zCos * xSin * ySin - xCos * zSin, xCos * zCos * ySin + xSin * zSin,
        yCos * zSin, xCos * zCos + xSin * ySin * zSin, -zCos * xSin + xCos * ySin * zSin, -ySin, yCos * xSin, xCos * yCos });
    }

    /// <summary>Creates a 3x3 matrix initialized with a shearing transformation.</summary>
    /// <param name="shearXbyY">The shear along the X-axis in the Y-direction.</param>
    /// <param name="shearXbyZ">The shear along the X-axis in the Z-direction.</param>
    /// <param name="shearYbyX">The shear along the Y-axis in the X-direction.</param>
    /// <param name="shearYbyZ">The shear along the Y-axis in the Z-direction.</param>
    /// <param name="shearZbyX">The shear along the Z-axis in the X-direction.</param>
    /// <param name="shearZbyY">The shear along the Z-axis in the Y-direction.</param>
    /// <returns>The constructed shearing matrix.</returns>
    public static Matrix Factory3x3Shear(
      float shearXbyY, float shearXbyZ, float shearYbyX,
      float shearYbyZ, float shearZbyX, float shearZbyY)
    {
      return new Matrix(3, 3, new float[] { 1, shearYbyX, shearZbyX, shearXbyY, 1, shearYbyZ, shearXbyZ, shearYbyZ, 1 });
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
    //public static implicit operator float[](Matrix matrix) { return matrix.Floats; }

    /// <summary>Negates all the values in this matrix.</summary>
    /// <returns>The resulting matrix after the negations.</returns>
    private Matrix Negate() { return Matrix.Negate(this); }
    /// <summary>Does a standard matrix addition.</summary>
    /// <param name="right">The matrix to add to this matrix.</param>
    /// <returns>The resulting matrix after the addition.</returns>
    private Matrix Add(Matrix right) { return Matrix.Add(this, right); }
    /// <summary>Does a standard matrix multiplication (triple for loop).</summary>
    /// <param name="right">The matrix to multiply this matrix by.</param>
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
    /// <summary>Combines two matrices from left to right (result.Columns == left.Columns + right.Columns).</summary>
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
    /// <returns>A copy of this matrix.</returns>
    public Matrix Clone() { return Matrix.Clone(this); }

    /// <summary>Negates all the values in a matrix.</summary>
    /// <param name="matrix">The matrix to have its values negated.</param>
    /// <returns>The resulting matrix after the negations.</returns>
    public static Matrix Negate(Matrix matrix)
    {
      Matrix result = new Matrix(matrix.Rows, matrix.Columns, matrix.Floats);
      float[] resultFloats = result.Floats;
      float[] matrixFloats = matrix.Floats;
      int length = resultFloats.Length;
      for (int i = 0; i < length; i++)
        resultFloats[i] = -matrixFloats[i];
      return result;
    }

    /// <summary>Does standard addition of two matrices.</summary>
    /// <param name="left">The left matrix of the addition.</param>
    /// <param name="right">The right matrix of the addition.</param>
    /// <returns>The resulting matrix after the addition.</returns>
    public static Matrix Add(Matrix left, Matrix right)
    {
      if (left.Rows != right.Rows || left.Columns != right.Columns)
        throw new MatrixException("invalid addition (size miss-match).");
      Matrix result = new Matrix(left.Rows, left.Columns);
      float[]
        resultFloats = result.Floats,
        leftFloats = left.Floats,
        rightFloats = right.Floats;
      int length = resultFloats.Length;
      for (int i = 0; i < length; i++)
        resultFloats[i] = leftFloats[i] + rightFloats[i];
      return result;
    }

    /// <summary>Subtracts a scalar from all the values in a matrix.</summary>
    /// <param name="left">The matrix to have the values subtracted from.</param>
    /// <param name="right">The scalar to subtract from all the matrix values.</param>
    /// <returns>The resulting matrix after the subtractions.</returns>
    public static Matrix Subtract(Matrix left, Matrix right)
    {
      if (left.Rows != right.Rows || left.Columns != right.Columns)
        throw new MatrixException("invalid subtraction (size miss-match).");
      Matrix result = new Matrix(left.Rows, left.Columns);
      float[] resultFloats = result.Floats,
        leftFloats = left.Floats,
        rightFloats = right.Floats;
      int length = resultFloats.Length;
      for (int i = 0; i < length; i++)
        resultFloats[i] = leftFloats[i] - rightFloats[i];
      return result;
    }

    /// <summary>Does a standard (triple for looped) multiplication between matrices.</summary>
    /// <param name="left">The left matrix of the multiplication.</param>
    /// <param name="right">The right matrix of the multiplication.</param>
    /// <returns>The resulting matrix of the multiplication.</returns>
    public static Matrix Multiply(Matrix left, Matrix right)
    {
      float[] leftFloats = left.Floats, rightFloats = right.Floats, resultFloats;
      int leftRows = left.Rows, leftCols = left.Columns, rightRows = right.Rows, rightCols = right.Columns;
      Matrix result = new Matrix(leftRows, rightCols);
      resultFloats = result.Floats;
      #region Optimizations
      if (leftRows < 5)
      {
        if (leftRows == 4 && leftCols == 4 && rightRows == 4 && rightCols == 4)
        {
          resultFloats = new float[16];
          float
            l11 = leftFloats[0], l12 = leftFloats[1], l13 = leftFloats[2], l14 = leftFloats[3],
            l21 = leftFloats[4], l22 = leftFloats[5], l23 = leftFloats[6], l24 = leftFloats[7],
            l31 = leftFloats[8], l32 = leftFloats[9], l33 = leftFloats[10], l34 = leftFloats[11],
            l41 = leftFloats[12], l42 = leftFloats[13], l43 = leftFloats[14], l44 = leftFloats[15],
            r11 = rightFloats[0], r12 = rightFloats[1], r13 = rightFloats[2], r14 = rightFloats[3],
            r21 = rightFloats[4], r22 = rightFloats[5], r23 = rightFloats[6], r24 = rightFloats[7],
            r31 = rightFloats[8], r32 = rightFloats[9], r33 = rightFloats[10], r34 = rightFloats[11],
            r41 = rightFloats[12], r42 = rightFloats[13], r43 = rightFloats[14], r44 = rightFloats[15];
          resultFloats[0] = l11 * r11 + l12 * r21 + l13 * r31 + l14 * r41;
          resultFloats[1] = l11 * r12 + l12 * r22 + l13 * r32 + l14 * r42;
          resultFloats[2] = l11 * r13 + l12 * r23 + l13 * r33 + l14 * r43;
          resultFloats[3] = l11 * r14 + l12 * r24 + l13 * r34 + l14 * r44;
          resultFloats[4] = l21 * r11 + l22 * r21 + l23 * r31 + l24 * r41;
          resultFloats[5] = l21 * r12 + l22 * r22 + l23 * r32 + l24 * r42;
          resultFloats[6] = l21 * r13 + l22 * r23 + l23 * r33 + l24 * r43;
          resultFloats[7] = l21 * r14 + l22 * r24 + l23 * r34 + l24 * r44;
          resultFloats[8] = l31 * r11 + l32 * r21 + l33 * r31 + l34 * r41;
          resultFloats[9] = l31 * r12 + l32 * r22 + l33 * r32 + l34 * r42;
          resultFloats[10] = l31 * r13 + l32 * r23 + l33 * r33 + l34 * r43;
          resultFloats[11] = l31 * r14 + l32 * r24 + l33 * r34 + l34 * r44;
          resultFloats[12] = l41 * r11 + l42 * r21 + l43 * r31 + l44 * r41;
          resultFloats[13] = l41 * r12 + l42 * r22 + l43 * r32 + l44 * r42;
          resultFloats[14] = l41 * r13 + l42 * r23 + l43 * r33 + l44 * r43;
          resultFloats[15] = l41 * r14 + l42 * r24 + l43 * r34 + l44 * r44;
          return result;
        }
      }
      #endregion
      if (leftCols != right.Rows)
        throw new MatrixException("invalid multiplication (size miss-match).");
      resultFloats = new float[leftRows * rightCols];
      for (int i = 0; i < leftRows; i++)
        for (int j = 0; j < rightCols; j++)
          for (int k = 0; k < leftCols; k++)
            resultFloats[i * rightCols + j] += leftFloats[i * leftCols + k] * rightFloats[k * rightCols + j];
      return result;
    }

    /// <summary>Multiplies all the values in a matrix by a scalar.</summary>
    /// <param name="left">The matrix to have the values multiplied.</param>
    /// <param name="right">The scalar to multiply the values by.</param>
    /// <returns>The resulting matrix after the multiplications.</returns>
    public static Matrix Multiply(Matrix left, float right)
    {
      Matrix result = new Matrix(left.Rows, left.Columns);
      float[] resultFloats = result.Floats;
      float[] leftFloats = left.Floats;
      for (int i = 0; i < resultFloats.Length; i++)
        resultFloats[i] = leftFloats[i] * right;
      return result;
    }

    /// <summary>Applies a power to a square matrix.</summary>
    /// <param name="matrix">The matrix to be powered by.</param>
    /// <param name="power">The power to apply to the matrix.</param>
    /// <returns>The resulting matrix of the power operation.</returns>
    public static Matrix Power(Matrix matrix, int power)
    {
      if (!(matrix.Rows == matrix.Columns))
        throw new MatrixException("invalid power (!matrix.IsSquare).");
      if (!(power > -1))
        throw new MatrixException("invalid power !(power > -1)");
      if (power == 0)
        return Matrix.FactoryIdentity(matrix.Rows, matrix.Columns);
      Matrix result = matrix.Clone();
      for (int i = 0; i < power; i++)
        result *= matrix;
      return result;
    }

    /// <summary>Divides all the values in the matrix by a scalar.</summary>
    /// <param name="left">The matrix to divide the values of.</param>
    /// <param name="right">The scalar to divide all the matrix values by.</param>
    /// <returns>The resulting matrix with the divided values.</returns>
    public static Matrix Divide(Matrix left, float right) { return Matrix.Multiply(left, 1.0f / right); }

    /// <summary>Gets the minor of a matrix.</summary>
    /// <param name="matrix">The matrix to get the minor of.</param>
    /// <param name="row">The restricted row to form the minor.</param>
    /// <param name="column">The restricted column to form the minor.</param>
    /// <returns>The minor of the matrix.</returns>
    public static Matrix Minor(Matrix matrix, int row, int column)
    {
      int matrixRows = matrix.Rows, matrixCols = matrix.Columns, resultCols = matrix.Columns - 1;
      Matrix result = new Matrix(matrix.Rows - 1, resultCols);
      float[] resultFloats = result.Floats;
      float[] matrixFloats = matrix.Floats;
      int m = 0, n = 0;
      for (int i = 0; i < matrixRows; i++)
      {
        if (i == row) continue;
        n = 0;
        for (int j = 0; j < matrixCols; j++)
          if (j == column) continue;
          else resultFloats[m * resultCols + n++] = matrixFloats[i * matrixCols + j];
        m++;
      }
      return result;
    }

    private static void RowMultiplication(Matrix matrix, int row, float scalar)
    {
      float[] matrixFloats = matrix.Floats;
      int start = row * matrix.Columns, end = row * matrix.Columns + matrix.Columns;
      for (int j = start; j < end; j++)
        matrixFloats[j] *= scalar;
    }

    private static void RowAddition(Matrix matrix, int target, int second, float scalar)
    {
      float[] matrixfloats = matrix.Floats;
      int columns = matrix.Columns,
        targetOffset = target * columns,
        secondOffset = second * columns;
      for (int j = 0; j < columns; j++)
        matrixfloats[targetOffset + j] += (matrixfloats[secondOffset + j] * scalar);
    }

    private static void SwapRows(Matrix matrix, int row1, int row2)
    {
      float[] matrixFloats = matrix.Floats;
      int columns = matrix.Columns, row1Offset = row1 * columns, row2Offset = row2 * columns;
      for (int j = 0; j < columns; j++)
      {
        float temp = matrixFloats[row1Offset + j];
        matrixFloats[row1Offset + j] = matrixFloats[row2Offset + j];
        matrixFloats[row2Offset + j] = temp;
      }
    }

    /// <summary>Combines two matrices from left to right 
    /// (result.Rows = left.Rows && result.Columns = left.Columns + right.Columns).</summary>
    /// <param name="left">The left matrix of the concatenation.</param>
    /// <param name="right">The right matrix of the concatenation.</param>
    /// <returns>The resulting matrix of the concatenation.</returns>
    public static Matrix ConcatenateRowWise(Matrix left, Matrix right)
    {
      if (left.Rows != right.Rows)
        throw new MatrixException("invalid row-wise concatenation !(left.Rows == right.Rows).");
      int resultRows = left.Rows, resultCols = left.Columns + right.Columns,
        leftCols = left.Columns, rightCols = right.Columns;
      Matrix result = new Matrix(resultRows, resultCols);
      float[]
        resultfloats = result.Floats,
        leftFloats = left.Floats,
        rightFloats = right.Floats;
      for (int i = 0; i < resultRows; i++)
        for (int j = 0; j < resultCols; j++)
        {
          if (j < left.Columns) resultfloats[i * resultCols + j] = leftFloats[i * leftCols + j];
          else resultfloats[i * resultCols + j] = rightFloats[i * rightCols + j - leftCols];
        }
      return result;
    }

    /// <summary>Calculates the determinent of a square matrix.</summary>
    /// <param name="matrix">The matrix to calculate the determinent of.</param>
    /// <returns>The determinent of the matrix.</returns>
    public static float Determinent(Matrix matrix)
    {
      int rows = matrix.Rows, columns = matrix.Columns;
      if (!(rows == matrix.Columns))
        throw new MatrixException("invalid determinent !(matrix.IsSquare).");
      float det = 1.0f;
      try
      {
        float[] rref = new float[matrix.Floats.Length];
        Buffer.BlockCopy(matrix.Floats, 0, rref, 0, rref.Length * sizeof(float));
        for (int i = 0; i < rows; i++)
        {
          if (rref[i * columns + i] == 0)
            for (int j = i + 1; j < rows; j++)
              if (rref[j * columns + i] != 0)
              {
                Matrix.SwapRows(matrix, i, j);
                det *= -1;
              }
          det *= rref[i * columns + i];
          Matrix.RowMultiplication(matrix, i, 1 / rref[i * columns + i]);
          for (int j = i + 1; j < rows; j++)
            Matrix.RowAddition(matrix, j, i, -rref[j * columns + i]);
          for (int j = i - 1; j >= 0; j--)
            Matrix.RowAddition(matrix, j, i, -rref[j * columns + i]);
        }
        return det;
      }
      catch (Exception) { throw new MatrixException("determinent computation failed."); }
    }

    /// <summary>Calculates the echelon of a matrix (aka REF).</summary>
    /// <param name="matrix">The matrix to calculate the echelon of (aka REF).</param>
    /// <returns>The echelon of the matrix (aka REF).</returns>
    public static Matrix Echelon(Matrix matrix)
    {
      try
      {
        int rows = matrix.Rows, columns = matrix.Columns;
        Matrix result = new Matrix(rows, columns, matrix.Floats);
        float[] resultfloats = result.Floats;
        for (int i = 0; i < rows; i++)
        {
          if (resultfloats[i * columns + i] == 0)
            for (int j = i + 1; j < rows; j++)
              if (resultfloats[j * columns + i] != 0)
                Matrix.SwapRows(result, i, j);
          if (resultfloats[i * columns + i] == 0)
            continue;
          if (resultfloats[i * columns + i] != 1)
            for (int j = i + 1; j < rows; j++)
              if (resultfloats[j * columns + i] == 1)
                Matrix.SwapRows(result, i, j);
          Matrix.RowMultiplication(result, i, 1 / resultfloats[i * columns + i]);
          for (int j = i + 1; j < rows; j++)
            Matrix.RowAddition(result, j, i, -resultfloats[j * columns + i]);
        }
        return result;
      }
      catch { throw new MatrixException("echelon computation failed."); }
    }

    /// <summary>Calculates the echelon of a matrix and reduces it (aka RREF).</summary>
    /// <param name="matrix">The matrix matrix to calculate the reduced echelon of (aka RREF).</param>
    /// <returns>The reduced echelon of the matrix (aka RREF).</returns>
    public static Matrix ReducedEchelon(Matrix matrix)
    {
      try
      {
        int rows = matrix.Rows, columns = matrix.Columns;
        Matrix result = new Matrix(rows, columns, matrix.Floats);
        float[] resultFloats = result.Floats;
        for (int i = 0; i < rows; i++)
        {
          if (resultFloats[i * columns + i] == 0)
            for (int j = i + 1; j < rows; j++)
              if (resultFloats[j * columns + i] != 0)
                Matrix.SwapRows(result, i, j);
          if (resultFloats[i * columns + i] == 0) continue;
          if (resultFloats[i * columns + i] != 1)
            for (int j = i + 1; j < rows; j++)
              if (resultFloats[j * columns + i] == 1)
                Matrix.SwapRows(result, i, j);
          Matrix.RowMultiplication(result, i, 1 / resultFloats[i * columns + i]);
          for (int j = i + 1; j < rows; j++)
            Matrix.RowAddition(result, j, i, -resultFloats[j * columns + i]);
          for (int j = i - 1; j >= 0; j--)
            Matrix.RowAddition(result, j, i, -resultFloats[j * columns + i]);
        }
        return result;
      }
      catch { throw new MatrixException("reduced echelon calculation failed."); }
    }

    /// <summary>Calculates the inverse of a matrix.</summary>
    /// <param name="matrix">The matrix to calculate the inverse of.</param>
    /// <returns>The inverse of the matrix.</returns>
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

    /// <summary>Calculates the adjoint of a matrix.</summary>
    /// <param name="matrix">The matrix to calculate the adjoint of.</param>
    /// <returns>The adjoint of the matrix.</returns>
    public static Matrix Adjoint(Matrix matrix)
    {
      if (!(matrix.Rows == matrix.Columns))
        throw new MatrixException("Adjoint of a non-square matrix does not exists");
      Matrix AdjointMatrix = new Matrix(matrix.Rows, matrix.Columns);
      for (int i = 0; i < matrix.Rows; i++)
        for (int j = 0; j < matrix.Columns; j++)
          if ((i + j) % 2 == 0)
            AdjointMatrix[i, j] = Matrix.Determinent(Matrix.Minor(matrix, i, j));
          else
            AdjointMatrix[i, j] = -Matrix.Determinent(Matrix.Minor(matrix, i, j));
      return Matrix.Transpose(AdjointMatrix);
    }

    /// <summary>Returns the transpose of a matrix.</summary>
    /// <param name="matrix">The matrix to transpose.</param>
    /// <returns>The transpose of the matrix.</returns>
    public static Matrix Transpose(Matrix matrix)
    {
      Matrix result = new Matrix(matrix.Columns, matrix.Rows);
      float[] matrixfloats = matrix.Floats;
      int rows = matrix.Columns, columns = matrix.Rows;
      float[] resultFloats = result.Floats;
      for (int i = 0; i < rows; i++)
        for (int j = 0; j < columns; j++)
          resultFloats[i * columns + j] = matrixfloats[j * rows + i];
      return result;
    }

    /// <summary>Decomposes a matrix into lower-upper reptresentation.</summary>
    /// <param name="matrix">The matrix to decompose.</param>
    /// <param name="Lower">The computed lower triangular matrix.</param>
    /// <param name="Upper">The computed upper triangular matrix.</param>
    public static void DecomposeLU(Matrix matrix, out Matrix Lower, out Matrix Upper)
    {
      if (!(matrix.Rows == matrix.Columns))
        throw new MatrixException("The matrix is not square!");
      Lower = Matrix.FactoryIdentity(matrix.Rows, matrix.Columns);
      Upper = Matrix.Clone(matrix);
      int[] permutation = new int[matrix.Rows];
      for (int i = 0; i < matrix.Rows; i++) permutation[i] = i;
      float p = 0, pom2, detOfP = 1;
      int k0 = 0, pom1 = 0;
      for (int k = 0; k < matrix.Columns - 1; k++)
      {
        p = 0;
        for (int i = k; i < matrix.Rows; i++)
          if (Calc.Abs(Upper[i, k]) > p)
          {
            p = Calc.Abs(Upper[i, k]);
            k0 = i;
          }
        if (p == 0)
          throw new MatrixException("The matrix is singular!");
        pom1 = permutation[k];
        permutation[k] = permutation[k0];
        permutation[k0] = pom1;
        for (int i = 0; i < k; i++)
        {
          pom2 = Lower[k, i];
          Lower[k, i] = Lower[k0, i];
          Lower[k0, i] = pom2;
        }
        if (k != k0)
          detOfP *= -1;
        for (int i = 0; i < matrix.Columns; i++)
        {
          pom2 = Upper[k, i];
          Upper[k, i] = Upper[k0, i];
          Upper[k0, i] = pom2;
        }
        for (int i = k + 1; i < matrix.Rows; i++)
        {
          Lower[i, k] = Upper[i, k] / Upper[k, k];
          for (int j = k; j < matrix.Columns; j++)
            Upper[i, j] = Upper[i, j] - Lower[i, k] * Upper[k, j];
        }
      }
    }

    /// <summary>Does a value equality check.</summary>
    /// <param name="left">The first matrix to check for equality.</param>
    /// <param name="right">The second matrix to check for equality.</param>
    /// <returns>True if values are equal, false if not.</returns>
    public static bool EqualsByValue(Matrix left, Matrix right)
    {
      int rows = left.Rows, columns = left.Columns;
      if (rows != right.Rows || columns != right.Columns)
        return false;
      float[] leftFloats = left.Floats;
      float[] rightFloats = right.Floats;
      for (int i = 0; i < leftFloats.Length; i++)
        if (leftFloats[i] != rightFloats[i])
          return false;
      return true;
    }

    /// <summary>Does a value equality check with leniency.</summary>
    /// <param name="left">The first matrix to check for equality.</param>
    /// <param name="right">The second matrix to check for equality.</param>
    /// <param name="leniency">How much the values can vary but still be considered equal.</param>
    /// <returns>True if values are equal, false if not.</returns>
    public static bool EqualsByValue(Matrix left, Matrix right, float leniency)
    {
      int rows = left.Rows, columns = left.Columns;
      if (rows != right.Rows || columns != right.Columns)
        return false;
      float[] leftFloats = left.Floats;
      float[] rightFloats = right.Floats;
      for (int i = 0; i < leftFloats.Length; i++)
        if (Calc.Abs(leftFloats[i] - rightFloats[i]) > leniency)
          return false;
      return true;
    }

    /// <summary>Checks if two matrices are equal by reverences.</summary>
    /// <param name="left">The left matric of the equality check.</param>
    /// <param name="right">The right matrix of the equality check.</param>
    /// <returns>True if the references are equal, false if not.</returns>
    public static bool EqualsByReference(Matrix left, Matrix right)
    {
      if (left == null || right == null) return false;
      return object.ReferenceEquals(left, right) || object.ReferenceEquals(left.Floats, right.Floats);
    }

    /// <summary>Copies a matrix.</summary>
    /// <returns>The copy of this matrix.</returns>
    public static Matrix Clone(Matrix matrix)
    {
      float[] floats = new float[matrix.Size];
      Buffer.BlockCopy(matrix.Floats, 0, floats, 0, floats.Length * sizeof(float));
      return new Matrix(matrix.Rows, matrix.Columns, floats);
    }

    /// <summary>Converts the matrix into a vector if (matrix.IsVector).</summary>
    /// <param name="matrix">The matrix to convert.</param>
    /// <returns>The resulting vector.</returns>
    public static Vector ToVector(Matrix matrix)
    {
      if (!matrix.IsVector)
        throw new MatrixException("invalid conversion from matrix to vector.");
      return new Vector(matrix.Floats);
    }

    /// <summary>Prints out a string representation of this matrix.</summary>
    /// <returns>A string representing this matrix.</returns>
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

    /// <summary>Computes a hash code from the values of this matrix.</summary>
    /// <returns>A hash code for the matrix.</returns>
    public override int GetHashCode()
    {
      // return base.GetHashCode();
      int hash = _matrix[0].GetHashCode();
      for (int i = 0; i < _matrix.Length; i++)
        hash = hash ^ _matrix[i].GetHashCode();
      return hash;
    }

    /// <summary>Does an equality check by reference.</summary>
    /// <param name="obj">The object to compare to.</param>
    /// <returns>True if the references are equal, false if not.</returns>
    public override bool Equals(object right)
    {
      if (!(right is Matrix)) return false;
      return Matrix.EqualsByReference(this, (Matrix)right);
    }

    private class MatrixException : Exception
    {
      public MatrixException(string Message) : base(Message) { }
    }
  }

  // This version uses a 2D float array; however, it is not as faster as the 
  // flattened array implementation. If you are attempting to copy the matrix
  // code into another project, you shoudl probably start with this version.
  #region Matrix (2-D Array)

  ///// <summary>A matrix wrapper for float[,] to perform matrix theory in row major order. Enjoy :)</summary>
  //public class Matrix
  //{
  //  private float[,] _matrix;

  //  /// <summary>The float[,] reference of this matrix.</summary>
  //  public float[,] Floats { get { return _matrix; } }
  //  /// <summary>The number of rows in the matrix.</summary>
  //  public int Rows { get { return _matrix.GetLength(0); } }
  //  /// <summary>The number of columns in the matrix.</summary>
  //  public int Columns { get { return _matrix.GetLength(1); } }
  //  /// <summary>Determines if the matrix is square.</summary>
  //  public bool IsSquare { get { return Rows == Columns; } }
  //  /// <summary>Determines if the matrix is a vector.</summary>
  //  public bool IsVector { get { return Columns == 1; } }
  //  /// <summary>Determines if the matrix is a 2 component vector.</summary>
  //  public bool Is2x1 { get { return Rows == 2 && Columns == 1; } }
  //  /// <summary>Determines if the matrix is a 3 component vector.</summary>
  //  public bool Is3x1 { get { return Rows == 3 && Columns == 1; } }
  //  /// <summary>Determines if the matrix is a 4 component vector.</summary>
  //  public bool Is4x1 { get { return Rows == 4 && Columns == 1; } }
  //  /// <summary>Determines if the matrix is a 2 square matrix.</summary>
  //  public bool Is2x2 { get { return Rows == 2 && Columns == 2; } }
  //  /// <summary>Determines if the matrix is a 3 square matrix.</summary>
  //  public bool Is3x3 { get { return Rows == 3 && Columns == 3; } }
  //  /// <summary>Determines if the matrix is a 4 square matrix.</summary>
  //  public bool Is4x4 { get { return Rows == 4 && Columns == 4; } }

  //  /// <summary>Standard row-major matrix indexing.</summary>
  //  /// <param name="row">The row index.</param>
  //  /// <param name="column">The column index.</param>
  //  /// <returns>The value at the given indeces.</returns>
  //  public float this[int row, int column]
  //  {
  //    get
  //    {
  //      try { return _matrix[row, column]; }
  //      catch { throw new MatrixException("index out of bounds."); }
  //    }
  //    set
  //    {
  //      try { _matrix[row, column] = value; }
  //      catch { throw new MatrixException("index out of bounds."); }
  //    }
  //  }

  //  /// <summary>Constructs a new zero-matrix of the given dimensions.</summary>
  //  /// <param name="rows">The number of row dimensions.</param>
  //  /// <param name="columns">The number of column dimensions.</param>
  //  public Matrix(int rows, int columns)
  //  {
  //    try { _matrix = new float[rows, columns]; }
  //    catch { throw new MatrixException("invalid dimensions."); }
  //  }

  //  /// <summary>Constructs a new array given row/column dimensions and the values to fill the matrix with.</summary>
  //  /// <param name="rows">The number of rows of the matrix.</param>
  //  /// <param name="columns">The number of columns of the matrix.</param>
  //  /// <param name="values">The values to fill the matrix with.</param>
  //  public Matrix(int rows, int columns, params float[] values)
  //  {
  //    if (values.Length != rows * columns)
  //      throw new MatrixException("invalid construction (number of values does not match dimensions.)");
  //    float[,] matrix;
  //    try { matrix = new float[rows, columns]; }
  //    catch { throw new MatrixException("invalid dimensions."); }
  //    int k = 0;
  //    for (int i = 0; i < rows; i++)
  //      for (int j = 0; j < columns; j++)
  //        matrix[i, j] = values[k++];
  //    _matrix = matrix;
  //  }

  //  /// <summary>Wraps a float[,] inside of a matrix class. WARNING: still references that float[,].</summary>
  //  /// <param name="matrix">The float[,] to wrap in a matrix class.</param>
  //  public Matrix(float[,] matrix)
  //  {
  //    _matrix = matrix;
  //  }

  //  /// <summary>Constructs a new zero-matrix of the given dimensions.</summary>
  //  /// <param name="rows">The number of rows of the matrix.</param>
  //  /// <param name="columns">The number of columns of the matrix.</param>
  //  /// <returns>The newly constructed zero-matrix.</returns>
  //  public static Matrix FactoryZero(int rows, int columns)
  //  {
  //    try { return new Matrix(rows, columns); }
  //    catch { throw new MatrixException("invalid dimensions."); }
  //  }

  //  /// <summary>Constructs a new identity-matrix of the given dimensions.</summary>
  //  /// <param name="rows">The number of rows of the matrix.</param>
  //  /// <param name="columns">The number of columns of the matrix.</param>
  //  /// <returns>The newly constructed identity-matrix.</returns>
  //  public static Matrix FactoryIdentity(int rows, int columns)
  //  {
  //    Matrix matrix;
  //    try { matrix = new Matrix(rows, columns); }
  //    catch { throw new MatrixException("invalid dimensions."); }
  //    if (rows <= columns)
  //      for (int i = 0; i < rows; i++)
  //        matrix[i, i] = 1;
  //    else
  //      for (int i = 0; i < columns; i++)
  //        matrix[i, i] = 1;
  //    return matrix;
  //  }

  //  /// <summary>Constructs a new matrix where every entry is 1.</summary>
  //  /// <param name="rows">The number of rows of the matrix.</param>
  //  /// <param name="columns">The number of columns of the matrix.</param>
  //  /// <returns>The newly constructed matrix filled with 1's.</returns>
  //  public static Matrix FactoryOne(int rows, int columns)
  //  {
  //    Matrix matrix;
  //    try { matrix = new Matrix(rows, columns); }
  //    catch { throw new MatrixException("invalid dimensions."); }
  //    for (int i = 0; i < rows; i++)
  //      for (int j = 0; j < columns; j++)
  //        matrix[i, j] = 1;
  //    return matrix;
  //  }

  //  /// <summary>Constructs a new matrix where every entry is the same uniform value.</summary>
  //  /// <param name="rows">The number of rows of the matrix.</param>
  //  /// <param name="columns">The number of columns of the matrix.</param>
  //  /// <param name="uniform">The value to assign every spot in the matrix.</param>
  //  /// <returns>The newly constructed matrix filled with the uniform value.</returns>
  //  public static Matrix FactoryUniform(int rows, int columns, float uniform)
  //  {
  //    Matrix matrix;
  //    try { matrix = new Matrix(rows, columns); }
  //    catch { throw new MatrixException("invalid dimensions."); }
  //    for (int i = 0; i < rows; i++)
  //      for (int j = 0; j < columns; j++)
  //        matrix[i, j] = uniform;
  //    return matrix;
  //  }

  //  /// <summary>Constructs a 2-component vector matrix with all values being 0.</summary>
  //  /// <returns>The constructed 2-component vector matrix.</returns>
  //  public static Matrix Factory2x1() { return new Matrix(2, 1); }
  //  /// <summary>Constructs a 3-component vector matrix with all values being 0.</summary>
  //  /// <returns>The constructed 3-component vector matrix.</returns>
  //  public static Matrix Factory3x1() { return new Matrix(3, 1); }
  //  /// <summary>Constructs a 4-component vector matrix with all values being 0.</summary>
  //  /// <returns>The constructed 4-component vector matrix.</returns>
  //  public static Matrix Factory4x1() { return new Matrix(4, 1); }

  //  /// <summary>Constructs a 2x2 matrix with all values being 0.</summary>
  //  /// <returns>The constructed 2x2 matrix.</returns>
  //  public static Matrix Factory2x2() { return new Matrix(2, 2); }
  //  /// <summary>Constructs a 3x3 matrix with all values being 0.</summary>
  //  /// <returns>The constructed 3x3 matrix.</returns>
  //  public static Matrix Factory3x3() { return new Matrix(3, 3); }
  //  /// <summary>Constructs a 4x4 matrix with all values being 0.</summary>
  //  /// <returns>The constructed 4x4 matrix.</returns>
  //  public static Matrix Factory4x4() { return new Matrix(4, 4); }

  //  /// <param name="angle">Angle of rotation in radians.</param>
  //  public static Matrix Factory3x3RotationX(float angle)
  //  {
  //    float cos = Calc.Cos(angle);
  //    float sin = Calc.Sin(angle);
  //    return new Matrix(new float[,] {
  //      { 1, 0, 0 },
  //      { 0, cos, sin },
  //      { 0, -sin, cos }});
  //  }

  //  /// <param name="angle">Angle of rotation in radians.</param>
  //  public static Matrix Factory3x3RotationY(float angle)
  //  {
  //    float cos = Calc.Cos(angle);
  //    float sin = Calc.Sin(angle);
  //    return new Matrix(new float[,] {
  //      { cos, 0, -sin },
  //      { 0, 1, 0 },
  //      { sin, 0, cos }});
  //  }

  //  /// <param name="angle">Angle of rotation in radians.</param>
  //  public static Matrix Factory3x3RotationZ(float angle)
  //  {
  //    float cos = Calc.Cos(angle);
  //    float sin = Calc.Sin(angle);
  //    return new Matrix(new float[,] {
  //      { cos, -sin, 0 },
  //      { sin, cos, 0 },
  //      { 0, 0, 1 }});
  //  }

  //  /// <param name="angleX">Angle about the X-axis in radians.</param>
  //  /// <param name="angleY">Angle about the Y-axis in radians.</param>
  //  /// <param name="angleZ">Angle about the Z-axis in radians.</param>
  //  public static Matrix Factory3x3RotationXthenYthenZ(float angleX, float angleY, float angleZ)
  //  {
  //    float
  //      xCos = Calc.Cos(angleX), xSin = Calc.Sin(angleX),
  //      yCos = Calc.Cos(angleY), ySin = Calc.Sin(angleY),
  //      zCos = Calc.Cos(angleZ), zSin = Calc.Sin(angleZ);
  //    return new Matrix(new float[,] {
  //      { yCos * zCos, -yCos * zSin, ySin },
  //      { xCos * zSin + xSin * ySin * zCos, xCos * zCos + xSin * ySin * zSin, -xSin * yCos },
  //      { xSin * zSin - xCos * ySin * zCos, xSin * zCos + xCos * ySin * zSin, xCos * yCos }});
  //  }

  //  /// <param name="angleX">Angle about the X-axis in radians.</param>
  //  /// <param name="angleY">Angle about the Y-axis in radians.</param>
  //  /// <param name="angleZ">Angle about the Z-axis in radians.</param>
  //  public static Matrix Factory3x3RotationZthenYthenX(float angleX, float angleY, float angleZ)
  //  {
  //    float
  //      xCos = Calc.Cos(angleX), xSin = Calc.Sin(angleX),
  //      yCos = Calc.Cos(angleY), ySin = Calc.Sin(angleY),
  //      zCos = Calc.Cos(angleZ), zSin = Calc.Sin(angleZ);
  //    return new Matrix(new float[,] {
  //      { yCos * zCos, zCos * xSin * ySin - xCos * zSin, xCos * zCos * ySin + xSin * zSin },
  //      { yCos * zSin, xCos * zCos + xSin * ySin * zSin, -zCos * xSin + xCos * ySin * zSin },
  //      { -ySin, yCos * xSin, xCos * yCos }});
  //  }

  //  /// <summary>Creates a 3x3 matrix initialized with a shearing transformation.</summary>
  //  /// <param name="shearXbyY">The shear along the X-axis in the Y-direction.</param>
  //  /// <param name="shearXbyZ">The shear along the X-axis in the Z-direction.</param>
  //  /// <param name="shearYbyX">The shear along the Y-axis in the X-direction.</param>
  //  /// <param name="shearYbyZ">The shear along the Y-axis in the Z-direction.</param>
  //  /// <param name="shearZbyX">The shear along the Z-axis in the X-direction.</param>
  //  /// <param name="shearZbyY">The shear along the Z-axis in the Y-direction.</param>
  //  /// <returns>The constructed shearing matrix.</returns>
  //  public static Matrix Factory3x3Shear(
  //    float shearXbyY, float shearXbyZ, float shearYbyX,
  //    float shearYbyZ, float shearZbyX, float shearZbyY)
  //  {
  //    return new Matrix(new float[,] {
  //      { 1, shearYbyX, shearZbyX },
  //      { shearXbyY, 1, shearYbyZ },
  //      { shearXbyZ, shearYbyZ, 1 }});
  //  }

  //  /// <summary>Negates all the values in a matrix.</summary>
  //  /// <param name="matrix">The matrix to have its values negated.</param>
  //  /// <returns>The resulting matrix after the negations.</returns>
  //  public static Matrix operator -(Matrix matrix) { return Matrix.Negate(matrix); }
  //  /// <summary>Does a standard matrix addition.</summary>
  //  /// <param name="left">The left matrix of the addition.</param>
  //  /// <param name="right">The right matrix of the addition.</param>
  //  /// <returns>The resulting matrix after teh addition.</returns>
  //  public static Matrix operator +(Matrix left, Matrix right) { return Matrix.Add(left, right); }
  //  /// <summary>Does a standard matrix subtraction.</summary>
  //  /// <param name="left">The left matrix of the subtraction.</param>
  //  /// <param name="right">The right matrix of the subtraction.</param>
  //  /// <returns>The result of the matrix subtraction.</returns>
  //  public static Matrix operator -(Matrix left, Matrix right) { return Matrix.Subtract(left, right); }
  //  /// <summary>Does a standard matrix multiplication.</summary>
  //  /// <param name="left">The left matrix of the multiplication.</param>
  //  /// <param name="right">The right matrix of the multiplication.</param>
  //  /// <returns>The resulting matrix after the multiplication.</returns>
  //  public static Matrix operator *(Matrix left, Matrix right) { return Matrix.Multiply(left, right); }
  //  /// <summary>Multiplies all the values in a matrix by a scalar.</summary>
  //  /// <param name="left">The matrix to have its values multiplied.</param>
  //  /// <param name="right">The scalar to multiply the values by.</param>
  //  /// <returns>The resulting matrix after the multiplications.</returns>
  //  public static Matrix operator *(Matrix left, float right) { return Matrix.Multiply(left, right); }
  //  /// <summary>Multiplies all the values in a matrix by a scalar.</summary>
  //  /// <param name="left">The scalar to multiply the values by.</param>
  //  /// <param name="right">The matrix to have its values multiplied.</param>
  //  /// <returns>The resulting matrix after the multiplications.</returns>
  //  public static Matrix operator *(float left, Matrix right) { return Matrix.Multiply(right, left); }
  //  /// <summary>Divides all the values in a matrix by a scalar.</summary>
  //  /// <param name="left">The matrix to have its values divided.</param>
  //  /// <param name="right">The scalar to divide the values by.</param>
  //  /// <returns>The resulting matrix after the divisions.</returns>
  //  public static Matrix operator /(Matrix left, float right) { return Matrix.Divide(left, right); }
  //  /// <summary>Applies a power to a matrix.</summary>
  //  /// <param name="left">The matrix to apply a power to.</param>
  //  /// <param name="right">The power to apply to the matrix.</param>
  //  /// <returns>The result of the power operation.</returns>
  //  public static Matrix operator ^(Matrix left, int right) { return Matrix.Power(left, right); }
  //  /// <summary>Checks for equality by value.</summary>
  //  /// <param name="left">The left matrix of the equality check.</param>
  //  /// <param name="right">The right matrix of the equality check.</param>
  //  /// <returns>True if the values of the matrices are equal, false if not.</returns>
  //  public static bool operator ==(Matrix left, Matrix right) { return Matrix.EqualsByValue(left, right); }
  //  /// <summary>Checks for false-equality by value.</summary>
  //  /// <param name="left">The left matrix of the false-equality check.</param>
  //  /// <param name="right">The right matrix of the false-equality check.</param>
  //  /// <returns>True if the values of the matrices are not equal, false if they are.</returns>
  //  public static bool operator !=(Matrix left, Matrix right) { return !Matrix.EqualsByValue(left, right); }
  //  /// <summary>Automatically converts a matrix into a float[,] if necessary.</summary>
  //  /// <param name="matrix">The matrix to convert to a float[,].</param>
  //  /// <returns>The reference to the float[,] representing the matrix.</returns>
  //  public static implicit operator float[,](Matrix matrix) { return matrix.Floats; }

  //  /// <summary>Negates all the values in this matrix.</summary>
  //  /// <returns>The resulting matrix after the negations.</returns>
  //  private Matrix Negate() { return Matrix.Negate(this); }
  //  /// <summary>Does a standard matrix addition.</summary>
  //  /// <param name="right">The matrix to add to this matrix.</param>
  //  /// <returns>The resulting matrix after the addition.</returns>
  //  private Matrix Add(Matrix right) { return Matrix.Add(this, right); }
  //  /// <summary>Does a standard matrix multiplication (triple for loop).</summary>
  //  /// <param name="right">The matrix to multiply this matrix by.</param>
  //  /// <returns>The resulting matrix after the multiplication.</returns>
  //  private Matrix Multiply(Matrix right) { return Matrix.Multiply(this, right); }
  //  /// <summary>Multiplies all the values in this matrix by a scalar.</summary>
  //  /// <param name="right">The scalar to multiply all the matrix values by.</param>
  //  /// <returns>The retulting matrix after the multiplications.</returns>
  //  private Matrix Multiply(float right) { return Matrix.Multiply(this, right); }
  //  /// <summary>Divides all the values in this matrix by a scalar.</summary>
  //  /// <param name="right">The scalar to divide the matrix values by.</param>
  //  /// <returns>The resulting matrix after teh divisions.</returns>
  //  private Matrix Divide(float right) { return Matrix.Divide(this, right); }
  //  /// <summary>Gets the minor of a matrix.</summary>
  //  /// <param name="row">The restricted row of the minor.</param>
  //  /// <param name="column">The restricted column of the minor.</param>
  //  /// <returns>The minor from the row/column restrictions.</returns>
  //  public Matrix Minor(int row, int column) { return Matrix.Minor(this, row, column); }
  //  /// <summary>Combines two matrices from left to right 
  //  /// (result.Rows = left.Rows && result.Columns = left.Columns + right.Columns).</summary>
  //  /// <param name="right">The matrix to combine with on the right side.</param>
  //  /// <returns>The resulting row-wise concatination.</returns>
  //  public Matrix ConcatenateRowWise(Matrix right) { return Matrix.ConcatenateRowWise(this, right); }
  //  /// <summary>Computes the determinent if this matrix is square.</summary>
  //  /// <returns>The computed determinent if this matrix is square.</returns>
  //  public float Determinent() { return Matrix.Determinent(this); }
  //  /// <summary>Computes the echelon form of this matrix (aka REF).</summary>
  //  /// <returns>The computed echelon form of this matrix (aka REF).</returns>
  //  public Matrix Echelon() { return Matrix.Echelon(this); }
  //  /// <summary>Computes the reduced echelon form of this matrix (aka RREF).</summary>
  //  /// <returns>The computed reduced echelon form of this matrix (aka RREF).</returns>
  //  public Matrix ReducedEchelon() { return Matrix.ReducedEchelon(this); }
  //  /// <summary>Computes the inverse of this matrix.</summary>
  //  /// <returns>The inverse of this matrix.</returns>
  //  public Matrix Inverse() { return Matrix.Inverse(this); }
  //  /// <summary>Gets the adjoint of this matrix.</summary>
  //  /// <returns>The adjoint of this matrix.</returns>
  //  public Matrix Adjoint() { return Matrix.Adjoint(this); }
  //  /// <summary>Transposes this matrix.</summary>
  //  /// <returns>The transpose of this matrix.</returns>
  //  public Matrix Transpose() { return Matrix.Transpose(this); }
  //  /// <summary>Copies this matrix.</summary>
  //  /// <returns>The copy of this matrix.</returns>
  //  public Matrix Clone() { return Matrix.Clone(this); }

  //  /// <summary>Negates all the values in a matrix.</summary>
  //  /// <param name="matrix">The matrix to have its values negated.</param>
  //  /// <returns>The resulting matrix after the negations.</returns>
  //  public static Matrix Negate(float[,] matrix)
  //  {
  //    Matrix result = new Matrix(matrix.GetLength(0), matrix.GetLength(1));
  //    for (int i = 0; i < matrix.GetLength(0); i++)
  //      for (int j = 0; j < matrix.GetLength(1); j++)
  //        result[i, j] = -matrix[i, j];
  //    return result;
  //  }

  //  /// <summary>Does standard addition of two matrices.</summary>
  //  /// <param name="left">The left matrix of the addition.</param>
  //  /// <param name="right">The right matrix of the addition.</param>
  //  /// <returns>The resulting matrix after the addition.</returns>
  //  public static Matrix Add(float[,] left, float[,] right)
  //  {
  //    if (left.GetLength(0) != right.GetLength(0) || left.GetLength(1) != right.GetLength(1))
  //      throw new MatrixException("invalid addition (size miss-match).");
  //    Matrix result = new Matrix(left.GetLength(0), left.GetLength(1));
  //    for (int i = 0; i < result.Rows; i++)
  //      for (int j = 0; j < result.Columns; j++)
  //        result[i, j] = left[i, j] + right[i, j];
  //    return result;
  //  }

  //  /// <summary>Subtracts a scalar from all the values in a matrix.</summary>
  //  /// <param name="left">The matrix to have the values subtracted from.</param>
  //  /// <param name="right">The scalar to subtract from all the matrix values.</param>
  //  /// <returns>The resulting matrix after the subtractions.</returns>
  //  public static Matrix Subtract(float[,] left, float[,] right)
  //  {
  //    if (left.GetLength(0) != right.GetLength(0) || left.GetLength(1) != right.GetLength(1))
  //      throw new MatrixException("invalid subtraction (size miss-match).");
  //    Matrix result = new Matrix(left.GetLength(0), left.GetLength(1));
  //    for (int i = 0; i < result.Rows; i++)
  //      for (int j = 0; j < result.Columns; j++)
  //        result[i, j] = left[i, j] - right[i, j];
  //    return result;
  //  }

  //  ///// <summary>Subtracts a scalar from all the values in a matrix.</summary>
  //  ///// <param name="left">The matrix to have the values subtracted from.</param>
  //  ///// <param name="right">The scalar to subtract from all the matrix values.</param>
  //  ///// <returns>The resulting matrix after the subtractions.</returns>
  //  //public static Vector Subtract(float[,] left, float[] right)
  //  //{
  //  //  if (!(left.GetLength(1) == 1 && left.GetLength(0) == right.Length))
  //  //    throw new MatrixException("invalid subtraction (size miss-match).");
  //  //  Vector result = new Vector(left.GetLength(0));
  //  //  for (int i = 0; i < result.Dimensions; i++)
  //  //    result[i] = left[i, 0] - right[i];
  //  //  return result;
  //  //}

  //  /// <summary>Does a standard (triple for looped) multiplication between matrices.</summary>
  //  /// <param name="left">The left matrix of the multiplication.</param>
  //  /// <param name="right">The right matrix of the multiplication.</param>
  //  /// <returns>The resulting matrix of the multiplication.</returns>
  //  public static Matrix Multiply(float[,] left, float[,] right)
  //  {
  //    if (left.GetLength(1) != right.GetLength(0))
  //      throw new MatrixException("invalid multiplication (size miss-match).");
  //    Matrix result = new Matrix(left.GetLength(0), right.GetLength(1));
  //    for (int i = 0; i < result.Rows; i++)
  //      for (int j = 0; j < result.Columns; j++)
  //        for (int k = 0; k < left.GetLength(1); k++)
  //          result[i, j] += left[i, k] * right[k, j];
  //    return result;
  //  }

  //  ///// <summary>Does a standard multiplication between a matrix and a vector.</summary>
  //  ///// <param name="left">The left matrix of the multiplication.</param>
  //  ///// <param name="right">The right vector of the multiplication.</param>
  //  ///// <returns>The resulting matrix/vector of the multiplication.</returns>
  //  //public static Matrix Multiply(float[,] left, float[] right)
  //  //{
  //  //  if (left.GetLength(1) != right.GetLength(0))
  //  //    throw new MatrixException("invalid multiplication (size miss-match).");
  //  //  Matrix result = new Matrix(left.GetLength(0), right.GetLength(1));
  //  //  for (int i = 0; i < result.Rows; i++)
  //  //      for (int k = 0; k < left.GetLength(1); k++)
  //  //        result[i, j] += left[i, k] * right[k];
  //  //  return result;
  //  //}

  //  /// <summary>Multiplies all the values in a matrix by a scalar.</summary>
  //  /// <param name="left">The matrix to have the values multiplied.</param>
  //  /// <param name="right">The scalar to multiply the values by.</param>
  //  /// <returns>The resulting matrix after the multiplications.</returns>
  //  public static Matrix Multiply(float[,] left, float right)
  //  {
  //    Matrix result = new Matrix(left.GetLength(0), left.GetLength(1));
  //    for (int i = 0; i < left.GetLength(0); i++)
  //      for (int j = 0; j < left.GetLength(1); j++)
  //        result[i, j] = left[i, j] * right;
  //    return result;
  //  }

  //  /// <summary>Applies a power to a square matrix.</summary>
  //  /// <param name="matrix">The matrix to be powered by.</param>
  //  /// <param name="power">The power to apply to the matrix.</param>
  //  /// <returns>The resulting matrix of the power operation.</returns>
  //  public static Matrix Power(float[,] matrix, int power)
  //  {
  //    if (!(matrix.GetLength(0) == matrix.GetLength(1)))
  //      throw new MatrixException("invalid power (!matrix.IsSquare).");
  //    if (!(power > -1))
  //      throw new MatrixException("invalid power !(power > -1)");
  //    if (power == 0)
  //      return Matrix.FactoryIdentity(matrix.GetLength(0), matrix.GetLength(1));
  //    Matrix result = Matrix.Clone(matrix);
  //    for (int i = 0; i < power; i++)
  //      result *= result;
  //    return result;
  //  }

  //  /// <summary>Divides all the values in the matrix by a scalar.</summary>
  //  /// <param name="matrix">The matrix to divide the values of.</param>
  //  /// <param name="right">The scalar to divide all the matrix values by.</param>
  //  /// <returns>The resulting matrix with the divided values.</returns>
  //  public static Matrix Divide(float[,] matrix, float right)
  //  {
  //    Matrix result = new Matrix(matrix.GetLength(0), matrix.GetLength(1));
  //    for (int i = 0; i < matrix.GetLength(0); i++)
  //      for (int j = 0; j < matrix.GetLength(1); j++)
  //        result[i, j] = matrix[i, j] / right;
  //    return result;
  //  }

  //  /// <summary>Gets the minor of a matrix.</summary>
  //  /// <param name="matrix">The matrix to get the minor of.</param>
  //  /// <param name="row">The restricted row to form the minor.</param>
  //  /// <param name="column">The restricted column to form the minor.</param>
  //  /// <returns>The minor of the matrix.</returns>
  //  public static Matrix Minor(float[,] matrix, int row, int column)
  //  {
  //    Matrix minor = new Matrix(matrix.GetLength(0) - 1, matrix.GetLength(1) - 1);
  //    int m = 0, n = 0;
  //    for (int i = 0; i < matrix.GetLength(0); i++)
  //    {
  //      if (i == row) continue;
  //      n = 0;
  //      for (int j = 0; j < matrix.GetLength(1); j++)
  //      {
  //        if (j == column) continue;
  //        minor[m, n] = matrix[i, j];
  //        n++;
  //      }
  //      m++;
  //    }
  //    return minor;
  //  }

  //  private static void RowMultiplication(float[,] matrix, int row, float scalar)
  //  {
  //    for (int j = 0; j < matrix.GetLength(1); j++)
  //      matrix[row, j] *= scalar;
  //  }

  //  private static void RowAddition(float[,] matrix, int target, int second, float scalar)
  //  {
  //    for (int j = 0; j < matrix.GetLength(1); j++)
  //      matrix[target, j] += (matrix[second, j] * scalar);
  //  }

  //  private static void SwapRows(float[,] matrix, int row1, int row2)
  //  {
  //    for (int j = 0; j < matrix.GetLength(1); j++)
  //    {
  //      float temp = matrix[row1, j];
  //      matrix[row1, j] = matrix[row2, j];
  //      matrix[row2, j] = temp;
  //    }
  //  }

  //  /// <summary>Combines two matrices from left to right 
  //  /// (result.Rows = left.Rows && result.Columns = left.Columns + right.Columns).</summary>
  //  /// <param name="left">The left matrix of the concatenation.</param>
  //  /// <param name="right">The right matrix of the concatenation.</param>
  //  /// <returns>The resulting matrix of the concatenation.</returns>
  //  public static Matrix ConcatenateRowWise(float[,] left, float[,] right)
  //  {
  //    if (left.GetLength(0) != right.GetLength(0))
  //      throw new MatrixException("invalid row-wise concatenation !(left.Rows == right.Rows).");
  //    Matrix result = new Matrix(left.GetLength(0), left.GetLength(1) + right.GetLength(1));
  //    for (int i = 0; i < result.Rows; i++)
  //      for (int j = 0; j < result.Columns; j++)
  //      {
  //        if (j < left.GetLength(1)) result[i, j] = left[i, j];
  //        else result[i, j] = right[i, j - left.GetLength(1)];
  //      }
  //    return result;
  //  }

  //  /// <summary>Calculates the determinent of a square matrix.</summary>
  //  /// <param name="matrix">The matrix to calculate the determinent of.</param>
  //  /// <returns>The determinent of the matrix.</returns>
  //  public static float Determinent(float[,] matrix)
  //  {
  //    if (!(matrix.GetLength(0) == matrix.GetLength(1)))
  //      throw new MatrixException("invalid determinent !(matrix.IsSquare).");
  //    float det = 1.0f;
  //    try
  //    {
  //      Matrix rref = Matrix.Clone(matrix);
  //      for (int i = 0; i < matrix.GetLength(0); i++)
  //      {
  //        if (rref[i, i] == 0)
  //          for (int j = i + 1; j < rref.Rows; j++)
  //            if (rref[j, i] != 0)
  //            {
  //              Matrix.SwapRows(rref, i, j);
  //              det *= -1;
  //            }
  //        det *= rref[i, i];
  //        Matrix.RowMultiplication(rref, i, 1 / rref[i, i]);
  //        for (int j = i + 1; j < rref.Rows; j++)
  //          Matrix.RowAddition(rref, j, i, -rref[j, i]);
  //        for (int j = i - 1; j >= 0; j--)
  //          Matrix.RowAddition(rref, j, i, -rref[j, i]);
  //      }
  //      return det;
  //    }
  //    catch (Exception) { throw new MatrixException("determinent computation failed."); }
  //  }

  //  /// <summary>Calculates the echelon of a matrix (aka REF).</summary>
  //  /// <param name="matrix">The matrix to calculate the echelon of (aka REF).</param>
  //  /// <returns>The echelon of the matrix (aka REF).</returns>
  //  public static Matrix Echelon(float[,] matrix)
  //  {
  //    try
  //    {
  //      Matrix result = Matrix.Clone(matrix);
  //      for (int i = 0; i < matrix.GetLength(0); i++)
  //      {
  //        if (result[i, i] == 0)
  //          for (int j = i + 1; j < result.Rows; j++)
  //            if (result[j, i] != 0)
  //              Matrix.SwapRows(result, i, j);
  //        if (result[i, i] == 0)
  //          continue;
  //        if (result[i, i] != 1)
  //          for (int j = i + 1; j < result.Rows; j++)
  //            if (result[j, i] == 1)
  //              Matrix.SwapRows(result, i, j);
  //        Matrix.RowMultiplication(result, i, 1 / result[i, i]);
  //        for (int j = i + 1; j < result.Rows; j++)
  //          Matrix.RowAddition(result, j, i, -result[j, i]);
  //      }
  //      return result;
  //    }
  //    catch { throw new MatrixException("echelon computation failed."); }
  //  }

  //  /// <summary>Calculates the echelon of a matrix and reduces it (aka RREF).</summary>
  //  /// <param name="matrix">The matrix matrix to calculate the reduced echelon of (aka RREF).</param>
  //  /// <returns>The reduced echelon of the matrix (aka RREF).</returns>
  //  public static Matrix ReducedEchelon(float[,] matrix)
  //  {
  //    try
  //    {
  //      Matrix result = Matrix.Clone(matrix);
  //      for (int i = 0; i < matrix.GetLength(0); i++)
  //      {
  //        if (result[i, i] == 0)
  //          for (int j = i + 1; j < result.Rows; j++)
  //            if (result[j, i] != 0)
  //              Matrix.SwapRows(result, i, j);
  //        if (result[i, i] == 0) continue;
  //        if (result[i, i] != 1)
  //          for (int j = i + 1; j < result.Rows; j++)
  //            if (result[j, i] == 1)
  //              Matrix.SwapRows(result, i, j);
  //        Matrix.RowMultiplication(result, i, 1 / result[i, i]);
  //        for (int j = i + 1; j < result.Rows; j++)
  //          Matrix.RowAddition(result, j, i, -result[j, i]);
  //        for (int j = i - 1; j >= 0; j--)
  //          Matrix.RowAddition(result, j, i, -result[j, i]);
  //      }
  //      return result;
  //    }
  //    catch { throw new MatrixException("reduced echelon calculation failed."); }
  //  }

  //  /// <summary>Calculates the inverse of a matrix.</summary>
  //  /// <param name="matrix">The matrix to calculate the inverse of.</param>
  //  /// <returns>The inverse of the matrix.</returns>
  //  public static Matrix Inverse(float[,] matrix)
  //  {
  //    if (Matrix.Determinent(matrix) == 0)
  //      throw new MatrixException("inverse calculation failed.");
  //    try
  //    {
  //      Matrix identity = Matrix.FactoryIdentity(matrix.GetLength(0), matrix.GetLength(1));
  //      Matrix rref = Matrix.Clone(matrix);
  //      for (int i = 0; i < matrix.GetLength(0); i++)
  //      {
  //        if (rref[i, i] == 0)
  //          for (int j = i + 1; j < rref.Rows; j++)
  //            if (rref[j, i] != 0)
  //            {
  //              Matrix.SwapRows(rref, i, j);
  //              Matrix.SwapRows(identity, i, j);
  //            }
  //        Matrix.RowMultiplication(identity, i, 1 / rref[i, i]);
  //        Matrix.RowMultiplication(rref, i, 1 / rref[i, i]);
  //        for (int j = i + 1; j < rref.Rows; j++)
  //        {
  //          Matrix.RowAddition(identity, j, i, -rref[j, i]);
  //          Matrix.RowAddition(rref, j, i, -rref[j, i]);
  //        }
  //        for (int j = i - 1; j >= 0; j--)
  //        {
  //          Matrix.RowAddition(identity, j, i, -rref[j, i]);
  //          Matrix.RowAddition(rref, j, i, -rref[j, i]);
  //        }
  //      }
  //      return identity;
  //    }
  //    catch { throw new MatrixException("inverse calculation failed."); }
  //  }

  //  /// <summary>Calculates the adjoint of a matrix.</summary>
  //  /// <param name="matrix">The matrix to calculate the adjoint of.</param>
  //  /// <returns>The adjoint of the matrix.</returns>
  //  public static Matrix Adjoint(float[,] matrix)
  //  {
  //    if (!(matrix.GetLength(0) == matrix.GetLength(1)))
  //      throw new MatrixException("Adjoint of a non-square matrix does not exists");
  //    Matrix AdjointMatrix = new Matrix(matrix.GetLength(0), matrix.GetLength(1));
  //    for (int i = 0; i < matrix.GetLength(0); i++)
  //      for (int j = 0; j < matrix.GetLength(1); j++)
  //        if ((i + j) % 2 == 0)
  //          AdjointMatrix[i, j] = Matrix.Determinent(Matrix.Minor(matrix, i, j));
  //        else
  //          AdjointMatrix[i, j] = -Matrix.Determinent(Matrix.Minor(matrix, i, j));
  //    return Matrix.Transpose(AdjointMatrix);
  //  }

  //  /// <summary>Returns the transpose of a matrix.</summary>
  //  /// <param name="matrix">The matrix to transpose.</param>
  //  /// <returns>The transpose of the matrix.</returns>
  //  public static Matrix Transpose(float[,] matrix)
  //  {
  //    Matrix transpose = new Matrix(matrix.GetLength(1), matrix.GetLength(0));
  //    for (int i = 0; i < transpose.Rows; i++)
  //      for (int j = 0; j < transpose.Columns; j++)
  //        transpose[i, j] = matrix[j, i];
  //    return transpose;
  //  }

  //  /// <summary>Decomposes a matrix into lower-upper reptresentation.</summary>
  //  /// <param name="matrix">The matrix to decompose.</param>
  //  /// <param name="Lower">The computed lower triangular matrix.</param>
  //  /// <param name="Upper">The computed upper triangular matrix.</param>
  //  public static void DecomposeLU(float[,] matrix, out Matrix Lower, out Matrix Upper)
  //  {
  //    if (!(matrix.GetLength(0) == matrix.GetLength(1)))
  //      throw new MatrixException("The matrix is not square!");
  //    Lower = Matrix.FactoryIdentity(matrix.GetLength(0), matrix.GetLength(1));
  //    Upper = Matrix.Clone(matrix);
  //    int[] permutation = new int[matrix.GetLength(0)];
  //    for (int i = 0; i < matrix.GetLength(0); i++) permutation[i] = i;
  //    float p = 0, pom2, detOfP = 1;
  //    int k0 = 0, pom1 = 0;
  //    for (int k = 0; k < matrix.GetLength(1) - 1; k++)
  //    {
  //      p = 0;
  //      for (int i = k; i < matrix.GetLength(0); i++)
  //        if (Calc.Abs(Upper[i, k]) > p)
  //        {
  //          p = Calc.Abs(Upper[i, k]);
  //          k0 = i;
  //        }
  //      if (p == 0)
  //        throw new MatrixException("The matrix is singular!");
  //      pom1 = permutation[k];
  //      permutation[k] = permutation[k0];
  //      permutation[k0] = pom1;
  //      for (int i = 0; i < k; i++)
  //      {
  //        pom2 = Lower[k, i];
  //        Lower[k, i] = Lower[k0, i];
  //        Lower[k0, i] = pom2;
  //      }
  //      if (k != k0)
  //        detOfP *= -1;
  //      for (int i = 0; i < matrix.GetLength(1); i++)
  //      {
  //        pom2 = Upper[k, i];
  //        Upper[k, i] = Upper[k0, i];
  //        Upper[k0, i] = pom2;
  //      }
  //      for (int i = k + 1; i < matrix.GetLength(0); i++)
  //      {
  //        Lower[i, k] = Upper[i, k] / Upper[k, k];
  //        for (int j = k; j < matrix.GetLength(1); j++)
  //          Upper[i, j] = Upper[i, j] - Lower[i, k] * Upper[k, j];
  //      }
  //    }
  //  }

  //  /// <summary>Creates a copy of a matrix.</summary>
  //  /// <param name="matrix">The matrix to copy.</param>
  //  /// <returns>A copy of the matrix.</returns>
  //  public static Matrix Clone(float[,] matrix)
  //  {
  //    Matrix result = new Matrix(matrix.GetLength(0), matrix.GetLength(1));
  //    for (int i = 0; i < matrix.GetLength(0); i++)
  //      for (int j = 0; j < matrix.GetLength(1); j++)
  //        result[i, j] = matrix[i, j];
  //    return result;
  //  }

  //  /// <summary>Does a value equality check.</summary>
  //  /// <param name="left">The first matrix to check for equality.</param>
  //  /// <param name="right">The second matrix to check for equality.</param>
  //  /// <returns>True if values are equal, false if not.</returns>
  //  public static bool EqualsByValue(float[,] left, float[,] right)
  //  {
  //    if (left.GetLength(0) != right.GetLength(0) || left.GetLength(1) != right.GetLength(1))
  //      return false;
  //    for (int i = 0; i < left.GetLength(0); i++)
  //      for (int j = 0; j < left.GetLength(1); j++)
  //        if (left[i, j] != right[i, j])
  //          return false;
  //    return true;
  //  }

  //  /// <summary>Does a value equality check with leniency.</summary>
  //  /// <param name="left">The first matrix to check for equality.</param>
  //  /// <param name="right">The second matrix to check for equality.</param>
  //  /// <param name="leniency">How much the values can vary but still be considered equal.</param>
  //  /// <returns>True if values are equal, false if not.</returns>
  //  public static bool EqualsByValue(float[,] left, float[,] right, float leniency)
  //  {
  //    if (left.GetLength(0) != right.GetLength(0) || left.GetLength(1) != right.GetLength(1))
  //      return false;
  //    for (int i = 0; i < left.GetLength(0); i++)
  //      for (int j = 0; j < left.GetLength(1); j++)
  //        if (Calc.Abs(left[i, j] - right[i, j]) > leniency)
  //          return false;
  //    return true;
  //  }

  //  /// <summary>Checks if two matrices are equal by reverences.</summary>
  //  /// <param name="left">The left matric of the equality check.</param>
  //  /// <param name="right">The right matrix of the equality check.</param>
  //  /// <returns>True if the references are equal, false if not.</returns>
  //  public static bool EqualsByReference(float[,] left, float[,] right)
  //  {
  //    return left.Equals(right);
  //  }

  //  /// <summary>Prints out a string representation of this matrix.</summary>
  //  /// <returns>A string representing this matrix.</returns>
  //  public override string ToString()
  //  {
  //    return base.ToString();
  //    //StringBuilder matrix = new StringBuilder();
  //    //for (int i = 0; i < Rows; i++)
  //    //{
  //    //  for (int j = 0; j < Columns; j++)
  //    //    matrix.Append(String.Format("{0:0.##}\t", _matrix[i, j]));
  //    //  matrix.Append("\n");
  //    //}
  //    //return matrix.ToString();
  //  }

  //  /// <summary>Computes a hash code from the values of this matrix.</summary>
  //  /// <returns>A hash code for the matrix.</returns>
  //  public override int GetHashCode()
  //  {
  //    // return base.GetHashCode();
  //    int hash = _matrix[0, 0].GetHashCode();
  //    for (int i = 0; i < Rows; i++)
  //      for (int j = 0; j < Columns; j++)
  //        hash = hash ^ _matrix[i, j].GetHashCode();
  //    return hash;
  //  }

  //  /// <summary>Does an equality check by reference.</summary>
  //  /// <param name="obj">The object to compare to.</param>
  //  /// <returns>True if the references are equal, false if not.</returns>
  //  public override bool Equals(object obj)
  //  {
  //    return base.Equals(obj) || _matrix.Equals(obj);
  //  }

  //  private class MatrixException : Exception
  //  {
  //    public MatrixException(string Message) : base(Message) { }
  //  }

  //  #region Alternate Compututation Methods

  //  //public static float Determinent(Matrix matrix)
  //  //{
  //  //  if (!matrix.IsSquare)
  //  //    throw new MatrixException("invalid determinent !(matrix.IsSquare).");
  //  //  return DeterminentRecursive(matrix);
  //  //}
  //  //private static float DeterminentRecursive(Matrix matrix)
  //  //{
  //  //  if (matrix.Rows == 1)
  //  //    return matrix[0, 0];
  //  //  float det = 0.0f;
  //  //  for (int j = 0; j < matrix.Columns; j++)
  //  //    det += (matrix[0, j] * DeterminentRecursive(Matrix.Minor(matrix, 0, j)) * (int)System.Math.Pow(-1, 0 + j));
  //  //  return det;
  //  //}

  //  //public static Matrix Inverse(Matrix matrix)
  //  //{
  //  //  float determinent = Matrix.Determinent(matrix);
  //  //  if (determinent == 0)
  //  //    throw new MatrixException("inverse calculation failed.");
  //  //  return Matrix.Adjoint(matrix) / determinent;
  //  //}

  //  #endregion
  //}

  #endregion

  // This is the original version of matrices that I used in my engine. It only supported
  // 3x3 matrices, which obviously needed improvement.
  #region Matrix-OLD
  ///// <summary>An optimized matrix class for 3x3 tranfromation matrices only.</summary>
  //public struct Matrix3x3
  //{
  //  private float
  //    _r0c0, _r0c1, _r0c2,
  //    _r1c0, _r1c1, _r1c2,
  //    _r2c0, _r2c1, _r2c2;

  //  public float this[int row, int column]
  //  {
  //    get
  //    {
  //      switch (row)
  //      {
  //        case 0:
  //          switch (column)
  //          {
  //            case 0: return _r0c0;
  //            case 1: return _r0c1;
  //            case 2: return _r0c2;
  //          }
  //          break;
  //        case 1:
  //          switch (column)
  //          {
  //            case 0: return _r1c0;
  //            case 1: return _r1c1;
  //            case 2: return _r1c2;
  //          }
  //          break;
  //        case 2:
  //          switch (column)
  //          {
  //            case 0: return _r2c0;
  //            case 1: return _r2c1;
  //            case 2: return _r2c2;
  //          }
  //          break;
  //      }
  //      throw new MatrixException("index out of range.");
  //    }
  //    set
  //    {
  //      switch (row)
  //      {
  //        case 0:
  //          switch (column)
  //          {
  //            case 0: _r0c0 = value; return;
  //            case 1: _r0c1 = value; return;
  //            case 2: _r0c2 = value; return;
  //          }
  //          break;
  //        case 1:
  //          switch (column)
  //          {
  //            case 0: _r1c0 = value; return;
  //            case 1: _r1c1 = value; return;
  //            case 2: _r1c2 = value; return;
  //          }
  //          break;
  //        case 2:
  //          switch (column)
  //          {
  //            case 0: _r2c0 = value; return;
  //            case 1: _r2c1 = value; return;
  //            case 2: _r2c2 = value; return;
  //          }
  //          break;
  //      }
  //      throw new MatrixException("index out of range.");
  //    }
  //  }

  //  public Matrix3x3(
  //    float r0c0, float r0c1, float r0c2,
  //    float r1c0, float r1c1, float r1c2,
  //    float r2c0, float r2c1, float r2c2)
  //  {
  //    _r0c0 = r0c0; _r0c1 = r0c1; _r0c2 = r0c2;
  //    _r1c0 = r1c0; _r1c1 = r1c1; _r1c2 = r1c2;
  //    _r2c0 = r2c0; _r2c1 = r2c1; _r2c2 = r2c2;
  //  }

  //  public Matrix3x3(float[,] floatArray)
  //  {
  //    if (floatArray == null)
  //      throw new MatrixException("Attempting to create a matrix with an null float[,].");
  //    else if (floatArray.GetLength(0) != 3)
  //      throw new MatrixException("Attempting to create a matrix with an invalid sized float[,].");
  //    else if (floatArray.GetLength(1) != 3)
  //      throw new MatrixException("Attempting to create a matrix with an invalid sized float[,].");
  //    _r0c0 = floatArray[0, 0]; _r0c1 = floatArray[0, 1]; _r0c2 = floatArray[0, 2];
  //    _r1c0 = floatArray[1, 0]; _r1c1 = floatArray[1, 1]; _r1c2 = floatArray[1, 2];
  //    _r2c0 = floatArray[2, 0]; _r2c1 = floatArray[2, 1]; _r2c2 = floatArray[2, 2];
  //  }

  //  public static Matrix3x3 FactoryZero = new Matrix3x3(0, 0, 0, 0, 0, 0, 0, 0, 0);
  //  public static Matrix3x3 FactoryIdentity = new Matrix3x3(1, 0, 0, 0, 1, 0, 0, 0, 1);

  //  /// <param name="angle">Angle of rotation in radians.</param>
  //  public static Matrix3x3 FactoryRotationX(float angle)
  //  {
  //    float cos = Calc.Cos(angle);
  //    float sin = Calc.Sin(angle);
  //    return new Matrix3x3(1, 0, 0, 0, cos, sin, 0, -sin, cos);
  //  }

  //  /// <param name="angle">Angle of rotation in radians.</param>
  //  public static Matrix3x3 FactoryRotationY(float angle)
  //  {
  //    float cos = Calc.Cos(angle);
  //    float sin = Calc.Sin(angle);
  //    return new Matrix3x3(cos, 0, -sin, 0, 1, 0, sin, 0, cos);
  //  }

  //  /// <param name="angle">Angle of rotation in radians.</param>
  //  public static Matrix3x3 FactoryRotationZ(float angle)
  //  {
  //    float cos = Calc.Cos(angle);
  //    float sin = Calc.Sin(angle);
  //    return new Matrix3x3(cos, -sin, 0, sin, cos, 0, 0, 0, 1);
  //  }

  //  /// <param name="angleX">Angle about the X-axis in radians.</param>
  //  /// <param name="angleY">Angle about the Y-axis in radians.</param>
  //  /// <param name="angleZ">Angle about the Z-axis in radians.</param>
  //  public static Matrix3x3 FactoryRotationXthenYthenZ(float angleX, float angleY, float angleZ)
  //  {
  //    float
  //      xCos = Calc.Cos(angleX), xSin = Calc.Sin(angleX),
  //      yCos = Calc.Cos(angleY), ySin = Calc.Sin(angleY),
  //      zCos = Calc.Cos(angleZ), zSin = Calc.Sin(angleZ);
  //    return new Matrix3x3(
  //      yCos * zCos, -yCos * zSin, ySin,
  //      xCos * zSin + xSin * ySin * zCos, xCos * zCos + xSin * ySin * zSin, -xSin * yCos,
  //      xSin * zSin - xCos * ySin * zCos, xSin * zCos + xCos * ySin * zSin, xCos * yCos);
  //  }

  //  /// <param name="angleX">Angle about the X-axis in radians.</param>
  //  /// <param name="angleY">Angle about the Y-axis in radians.</param>
  //  /// <param name="angleZ">Angle about the Z-axis in radians.</param>
  //  public static Matrix3x3 FactoryRotationZthenYthenX(float angleX, float angleY, float angleZ)
  //  {
  //    float
  //      xCos = Calc.Cos(angleX), xSin = Calc.Sin(angleX),
  //      yCos = Calc.Cos(angleY), ySin = Calc.Sin(angleY),
  //      zCos = Calc.Cos(angleZ), zSin = Calc.Sin(angleZ);
  //    return new Matrix3x3(
  //      yCos * zCos, zCos * xSin * ySin - xCos * zSin, xCos * zCos * ySin + xSin * zSin,
  //      yCos * zSin, xCos * zCos + xSin * ySin * zSin, -zCos * xSin + xCos * ySin * zSin,
  //      -ySin, yCos * xSin, xCos * yCos);
  //  }

  //  /// <param name="shearXbyY">The shear along the X-axis in the Y-direction.</param>
  //  /// <param name="shearXbyZ">The shear along the X-axis in the Z-direction.</param>
  //  /// <param name="shearYbyX">The shear along the Y-axis in the X-direction.</param>
  //  /// <param name="shearYbyZ">The shear along the Y-axis in the Z-direction.</param>
  //  /// <param name="shearZbyX">The shear along the Z-axis in the X-direction.</param>
  //  /// <param name="shearZbyY">The shear along the Z-axis in the Y-direction.</param>
  //  public static Matrix3x3 FactoryShear(
  //    float shearXbyY, float shearXbyZ, float shearYbyX,
  //    float shearYbyZ, float shearZbyX, float shearZbyY)
  //  {
  //    return new Matrix3x3(
  //      1, shearYbyX, shearZbyX,
  //      shearXbyY, 1, shearYbyZ,
  //      shearXbyZ, shearYbyZ, 1);
  //  }

  //  public static Matrix3x3 operator +(Matrix3x3 left, Matrix3x3 right) { return left.Add(right); }
  //  public static Matrix3x3 operator -(Matrix3x3 left, Matrix3x3 right) { return left.Add(-right); }
  //  public static Matrix3x3 operator -(Matrix3x3 matrix) { return matrix.Negate(); }
  //  public static Matrix3x3 operator *(Matrix3x3 left, Matrix3x3 right) { return left.Multiply(right); }
  //  public static Vector operator *(Matrix3x3 matrix, Vector vector) { return matrix.Multiply(vector); }
  //  public static Matrix3x3 operator *(Matrix3x3 matrix, float scalar) { return matrix.Multiply(scalar); }
  //  public static Matrix3x3 operator /(Matrix3x3 matrix, float scalar) { return matrix.Divide(scalar); }
  //  public static Matrix3x3 operator ^(Matrix3x3 matrix, int power) { return matrix.Power(power); }
  //  public static implicit operator Matrix(Matrix3x3 matrix) { return Matrix3x3.ToMatrix(matrix); }
  //  public static implicit operator float[,](Matrix3x3 matrix) { return Matrix3x3.ToFloats(matrix); }

  //  public float Determinant
  //  {
  //    get
  //    {
  //      return
  //        _r0c0 * _r1c1 * _r2c2 -
  //        _r0c0 * _r1c2 * _r2c1 -
  //        _r0c1 * _r1c0 * _r2c2 +
  //        _r0c2 * _r1c0 * _r2c1 +
  //        _r0c1 * _r1c2 * _r2c0 -
  //        _r0c2 * _r1c1 * _r2c0;
  //    }
  //  }

  //  public bool EqualsApproximation(Matrix3x3 matrix, float tolerance)
  //  {
  //    return
  //      Calc.Abs(_r0c0 - matrix._r0c0) <= tolerance &&
  //      Calc.Abs(_r0c1 - matrix._r0c1) <= tolerance &&
  //      Calc.Abs(_r0c2 - matrix._r0c2) <= tolerance &&
  //      Calc.Abs(_r1c0 - matrix._r1c0) <= tolerance &&
  //      Calc.Abs(_r1c1 - matrix._r1c1) <= tolerance &&
  //      Calc.Abs(_r1c2 - matrix._r1c2) <= tolerance &&
  //      Calc.Abs(_r2c0 - matrix._r2c0) <= tolerance &&
  //      Calc.Abs(_r2c1 - matrix._r2c1) <= tolerance &&
  //      Calc.Abs(_r2c2 - matrix._r2c2) <= tolerance;
  //  }

  //  public Matrix3x3 Negate()
  //  {
  //    return new Matrix3x3(
  //      -_r0c0, -_r0c1, -_r0c2,
  //      -_r1c0, -_r1c1, -_r1c2,
  //      -_r2c0, -_r2c1, -_r2c2);
  //  }

  //  public Matrix3x3 Add(Matrix3x3 matrix)
  //  {
  //    return new Matrix3x3(
  //      _r0c0 + matrix._r0c0, _r0c1 + matrix._r0c1, _r0c2 + matrix._r0c2,
  //      _r1c0 + matrix._r1c0, _r1c1 + matrix._r1c1, _r1c2 + matrix._r1c2,
  //      _r2c0 + matrix._r2c0, _r2c1 + matrix._r2c1, _r2c2 + matrix._r2c2);
  //  }

  //  public Matrix3x3 Multiply(Matrix3x3 matrix)
  //  {
  //    return new Matrix3x3(
  //      matrix._r0c0 * _r0c0 + matrix._r0c1 * _r1c0 + matrix._r0c2 * _r2c0,
  //      matrix._r0c0 * _r0c1 + matrix._r0c1 * _r1c1 + matrix._r0c2 * _r2c1,
  //      matrix._r0c0 * _r0c2 + matrix._r0c1 * _r1c2 + matrix._r0c2 * _r2c2,
  //      matrix._r1c0 * _r0c0 + matrix._r1c1 * _r1c0 + matrix._r1c2 * _r2c0,
  //      matrix._r1c0 * _r0c1 + matrix._r1c1 * _r1c1 + matrix._r1c2 * _r2c1,
  //      matrix._r1c0 * _r0c2 + matrix._r1c1 * _r1c2 + matrix._r1c2 * _r2c2,
  //      matrix._r2c0 * _r0c0 + matrix._r2c1 * _r1c0 + matrix._r2c2 * _r2c0,
  //      matrix._r2c0 * _r0c1 + matrix._r2c1 * _r1c1 + matrix._r2c2 * _r2c1,
  //      matrix._r2c0 * _r0c2 + matrix._r2c1 * _r1c2 + matrix._r2c2 * _r2c2);
  //  }

  //  public Vector Multiply(Vector vector)
  //  {
  //    return new Vector(
  //      _r0c0 * vector.X + _r0c1 * vector.Y + _r0c2 * vector.Z,
  //      _r1c0 * vector.X + _r1c1 * vector.Y + _r1c2 * vector.Z,
  //      _r2c0 * vector.X + _r2c1 * vector.Y + _r2c2 * vector.Z);
  //  }

  //  public Matrix3x3 Multiply(float scalar)
  //  {
  //    return new Matrix3x3(
  //      scalar * _r0c0, scalar * _r0c1, scalar * _r0c2,
  //      scalar * _r1c0, scalar * _r1c1, scalar * _r1c2,
  //      scalar * _r2c0, scalar * _r2c1, scalar * _r2c2);
  //  }

  //  public Matrix3x3 Divide(float scalar)
  //  {
  //    return new Matrix3x3(
  //      _r0c0 / scalar, _r0c1 / scalar, _r0c2 / scalar,
  //      _r1c0 / scalar, _r1c1 / scalar, _r1c2 / scalar,
  //      _r2c0 / scalar, _r2c1 / scalar, _r2c2 / scalar);
  //  }

  //  public Matrix3x3 Power(int power)
  //  {
  //    if (power < 0)
  //      throw new MatrixException("Attempting to raise a matrix by a power less than zero. (can't do dat)");
  //    else if (power == 0)
  //      return FactoryIdentity;
  //    else
  //    {
  //      Matrix3x3 result = Clone();
  //      for (int i = 1; i < power; i++)
  //        result = result * result;
  //      return result;
  //    }
  //  }

  //  public Matrix3x3 Transpose()
  //  {
  //    return new Matrix3x3(
  //      _r0c0, _r1c0, _r2c0,
  //      _r0c1, _r1c1, _r2c1,
  //      _r0c2, _r1c1, _r2c2);
  //  }

  //  public Quaternion ToQuaternion()
  //  {
  //    float qX = (_r0c0 + _r1c1 + _r2c2 + 1.0f) / 4.0f;
  //    float qY = (_r0c0 - _r1c1 - _r2c2 + 1.0f) / 4.0f;
  //    float qZ = (-_r0c0 + _r1c1 - _r2c2 + 1.0f) / 4.0f;
  //    float qW = (-_r0c0 - _r1c1 + _r2c2 + 1.0f) / 4.0f;

  //    if (qX < 0.0f) qX = 0.0f;
  //    if (qY < 0.0f) qY = 0.0f;
  //    if (qZ < 0.0f) qZ = 0.0f;
  //    if (qW < 0.0f) qW = 0.0f;

  //    qX = Calc.SquareRoot(qX);
  //    qY = Calc.SquareRoot(qY);
  //    qZ = Calc.SquareRoot(qZ);
  //    qW = Calc.SquareRoot(qW);

  //    if (qX >= qY && qX >= qZ && qX >= qW)
  //    {
  //      qX *= +1.0f;
  //      qY *= Calc.Sin(_r2c1 - _r1c2);
  //      qZ *= Calc.Sin(_r0c2 - _r2c0);
  //      qW *= Calc.Sin(_r1c0 - _r0c1);
  //    }
  //    else if (qY >= qX && qY >= qZ && qY >= qW)
  //    {
  //      qX *= Calc.Sin(_r2c1 - _r1c2);
  //      qY *= +1.0f;
  //      qZ *= Calc.Sin(_r1c0 + _r0c1);
  //      qW *= Calc.Sin(_r0c2 + _r2c0);
  //    }
  //    else if (qZ >= qX && qZ >= qY && qZ >= qW)
  //    {
  //      qX *= Calc.Sin(_r0c2 - _r2c0);
  //      qY *= Calc.Sin(_r1c0 + _r0c1);
  //      qZ *= +1.0f;
  //      qW *= Calc.Sin(_r2c1 + _r1c2);
  //    }
  //    else if (qW >= qX && qW >= qY && qW >= qZ)
  //    {
  //      qX *= Calc.Sin(_r1c0 - _r0c1);
  //      qY *= Calc.Sin(_r2c0 + _r0c2);
  //      qZ *= Calc.Sin(_r2c1 + _r1c2);
  //      qW *= +1.0f;
  //    }
  //    else
  //      throw new MatrixException("There is a glitch in my my matrix to quaternion function. Sorry..");

  //    float length = Calc.SquareRoot(qX * qX + qY * qY + qZ * qZ + qW * qW);

  //    return new Quaternion(
  //      qX /= length,
  //      qY /= length,
  //      qZ /= length,
  //      qW /= length);
  //  }

  //  public Matrix3x3 Clone()
  //  {
  //    return new Matrix3x3(
  //      _r0c0, _r0c1, _r0c2,
  //      _r1c0, _r1c1, _r1c2,
  //      _r2c0, _r2c1, _r2c2);
  //  }

  //  public bool Equals(Matrix3x3 matrix)
  //  {
  //    return
  //      _r0c0 == matrix._r0c0 && _r0c1 == matrix._r0c1 && _r0c2 == matrix._r0c2 &&
  //      _r1c0 == matrix._r1c0 && _r1c1 == matrix._r1c1 && _r1c2 == matrix._r1c2 &&
  //      _r2c0 == matrix._r2c0 && _r2c1 == matrix._r2c1 && _r2c2 == matrix._r2c2;
  //  }

  //  public static Matrix ToMatrix(Matrix3x3 matrix)
  //  {
  //    Matrix result = new Matrix(3, 3);
  //    result[0, 0] = matrix._r0c0; result[0, 1] = matrix._r0c1; result[0, 2] = matrix._r0c2;
  //    result[1, 0] = matrix._r1c0; result[1, 1] = matrix._r1c1; result[1, 2] = matrix._r1c2;
  //    result[1, 0] = matrix._r2c0; result[2, 1] = matrix._r2c1; result[2, 2] = matrix._r2c2;
  //    return result;
  //  }

  //  public static float[,] ToFloats(Matrix3x3 matrix)
  //  {
  //    return new float[,]
  //      { { matrix[0, 0], matrix[0, 1], matrix[0, 2] },
  //      { matrix[1, 0], matrix[1, 1], matrix[1, 2] },
  //      { matrix[2, 0], matrix[2, 1], matrix[2, 2] } };
  //  }

  //  public override int GetHashCode()
  //  {
  //    return
  //      _r0c0.GetHashCode() ^ _r0c1.GetHashCode() ^ _r0c2.GetHashCode() ^
  //      _r1c0.GetHashCode() ^ _r1c1.GetHashCode() ^ _r1c2.GetHashCode() ^
  //      _r2c0.GetHashCode() ^ _r2c1.GetHashCode() ^ _r2c2.GetHashCode();
  //  }

  //  public override string ToString()
  //  {
  //    return base.ToString();
  //    //return
  //    //  _r0c0 + " " + _r0c1 + " " + _r0c2 + "\n" +
  //    //  _r1c0 + " " + _r1c1 + " " + _r1c2 + "\n" +
  //    //  _r2c0 + " " + _r2c1 + " " + _r2c2 + "\n";
  //  }

  //  private class MatrixException : Exception
  //  {
  //    public MatrixException(string message) : base(message) { }
  //  }
  //}
  #endregion
}
