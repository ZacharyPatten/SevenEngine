using System;
using System.Runtime.InteropServices;

namespace Engine.Mathematics
{
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  public struct Matrix
  {
    private double
      _row0Column0, _row0Column1, _row0Column2,
      _row1Column0, _row1Column1, _row1Column2,
      _row2Column0, _row2Column1, _row2Column2;

    /// <summary>Indexing for getting matrix values in row major order.</summary>
    public double this[int row, int column]
    {
      get
      {
        switch (row)
        {
          case 0:
            switch (column)
            {
              case 0: return _row0Column0;
              case 1: return _row0Column1;
              case 2: return _row0Column2;
            }
            break;
          case 1:
            switch (column)
            {
              case 0: return _row1Column0;
              case 1: return _row1Column1;
              case 2: return _row1Column2;
            }
            break;
          case 2:
            switch (column)
            {
              case 0: return _row2Column0;
              case 1: return _row2Column1;
              case 2: return _row2Column2;
            }
            break;
        }
        throw new MatrixException("index out of range during indexed look up.");
      }
      set
      {
        switch (row)
        {
          case 0:
            switch (column)
            {
              case 0: _row0Column0 = value; return;
              case 1: _row0Column1 = value; return;
              case 2: _row0Column2 = value; return;
            }
            break;
          case 1:
            switch (column)
            {
              case 0: _row1Column0 = value; return;
              case 1: _row1Column1 = value; return;
              case 2: _row1Column2 = value; return;
            }
            break;
          case 2:
            switch (column)
            {
              case 0: _row2Column0 = value; return;
              case 1: _row2Column1 = value; return;
              case 2: _row2Column2 = value; return;
            }
            break;
        }
        throw new MatrixException("index out of range during indexed look up.");
      }
    }

    /// <summary>Constructs a matrix with the given values.</summary>
    public Matrix(
      double row0Column0, double row0Column1, double row0Column2,
      double row1Column0, double row1Column1, double row1Column2,
      double row2Column0, double row2Column1, double row2Column2)
    {
      _row0Column0 = row0Column0; _row0Column1 = row0Column1; _row0Column2 = row0Column2;
      _row1Column0 = row1Column0; _row1Column1 = row1Column1; _row1Column2 = row1Column2;
      _row2Column0 = row2Column0; _row2Column1 = row2Column1; _row2Column2 = row2Column2;
    }

    /// <summary>Constructs an instance of a matrix with all values initialized to zero.</summary>
    public static Matrix FactoryZero() { return new Matrix(0, 0, 0, 0, 0, 0, 0, 0, 0); }

    /// <summary>Constructs an instance of a matrix initialized as an identity matrix.</summary>
    public static Matrix FactoryIdentity() { return new Matrix(1, 0, 0, 0, 1, 0, 0, 0, 1); }

    /// <summary>Constructs a matrix initialized as a transformation matrix about the X-axis.</summary>
    /// <param name="angle">Angle of rotation in radians.</param>
    public static Matrix FactoryRotationX(double angle)
    {
      double cos = Math.Cos(angle);
      double sin = Math.Sin(angle);
      return new Matrix(
        1, 0, 0,
        0, cos, sin,
        0, -sin, cos);
    }

    /// <summary>Constructs a matrix initialized as a transformation matrix about the Y-axis.</summary>
    /// <param name="angle">Angle of rotation in radians.</param>
    public static Matrix FactoryRotationY(double angle)
    {
      double cos = Math.Cos(angle);
      double sin = Math.Sin(angle);
      return new Matrix(
        cos, 0, -sin,
        0, 1, 0,
        sin, 0, cos);
    }

    /// <summary>Constructs a matrix initialized as a transformation matrix about the Z-axis.</summary>
    /// <param name="angle">Angle of rotation in radians.</param>
    public static Matrix FactoryRotationZ(double angle)
    {
      double cos = Math.Cos(angle);
      double sin = Math.Sin(angle);
      return new Matrix(
        cos, -sin, 0,
        sin, cos, 0,
        0, 0, 1);
    }

    /// <summary>Constructs a matrix instance initialized to rotate geometry about the X-axis, THEN the Y-axis, THEN the Z-axis.</summary>
    /// <param name="angleX">Angle about the X-axis in radians.</param>
    /// <param name="angleY">Angle about the Y-axis in radians.</param>
    /// <param name="angleZ">Angle about the Z-axis in radians.</param>
    public static Matrix FactoryRotationXthenYthenZ(double angleX, double angleY, double angleZ)
    {
      double
        xCos = Math.Cos(angleX), xSin = Math.Sin(angleX),
        yCos = Math.Cos(angleY), ySin = Math.Sin(angleY),
        zCos = Math.Cos(angleZ), zSin = Math.Sin(angleZ);
      return new Matrix(
        yCos * zCos, -yCos * zSin, ySin,
        xCos * zSin + xSin * ySin * zCos, xCos * zCos + xSin * ySin * zSin, -xSin * yCos,
        xSin * zSin - xCos * ySin * zCos, xSin * zCos + xCos * ySin * zSin, xCos * yCos);
    }

    /// <summary>Constructs a matrix instance initialized to rotate geometry about the Z-axis, THEN the Y-axis, THEN the X-axis.</summary>
    /// <param name="angleX">Angle about the X-axis in radians.</param>
    /// <param name="angleY">Angle about the Y-axis in radians.</param>
    /// <param name="angleZ">Angle about the Z-axis in radians.</param>
    public static Matrix FactoryRotationZthenYthenX(double angleX, double angleY, double angleZ)
    {
      double
        xCos = Math.Cos(angleX), xSin = Math.Sin(angleX),
        yCos = Math.Cos(angleY), ySin = Math.Sin(angleY),
        zCos = Math.Cos(angleZ), zSin = Math.Sin(angleZ);
      return new Matrix(
        yCos * zCos, zCos * xSin * ySin - xCos * zSin, xCos * zCos * ySin + xSin * zSin,
        yCos * zSin, xCos * zCos + xSin * ySin * zSin, -zCos * xSin + xCos * ySin * zSin,
        -ySin, yCos * xSin, xCos * yCos);
    }

    /// <summary>Cunstructs an instance of a matrix initialized to shear 3D geometry according to the parameters.</summary>
    /// <param name="shearXbyY">The shear along the X-axis in the Y-direction.</param>
    /// <param name="shearXbyZ">The shear along the X-axis in the Z-direction.</param>
    /// <param name="shearYbyX">The shear along the Y-axis in the X-direction.</param>
    /// <param name="shearYbyZ">The shear along the Y-axis in the Z-direction.</param>
    /// <param name="shearZbyX">The shear along the Z-axis in the X-direction.</param>
    /// <param name="shearZbyY">The shear along the Z-axis in the Y-direction.</param>
    public static Matrix FactoryShear(
      double shearXbyY, double shearXbyZ,
      double shearYbyX, double shearYbyZ,
      double shearZbyX, double shearZbyY)
    {
      return new Matrix(
        1, shearYbyX, shearZbyX,
        shearXbyY, 1, shearYbyZ,
        shearXbyZ, shearYbyZ, 1);
    }

    /// <summary>Constructs left matrix from the given array of double-precision floating-point numbers.</summary>
    public Matrix(double[,] doubleArray)
    {
      if (doubleArray == null) { throw new MissingFieldException(); }
      else if (doubleArray.GetLength(0) < 3) { throw new MissingFieldException(); }
      else if (doubleArray.GetLength(1) < 3) { throw new MissingFieldException(); }
      _row0Column0 = doubleArray[0, 0]; _row0Column1 = doubleArray[0, 1]; _row0Column2 = doubleArray[0, 2];
      _row1Column0 = doubleArray[1, 0]; _row1Column1 = doubleArray[1, 1]; _row1Column2 = doubleArray[1, 2];
      _row2Column0 = doubleArray[2, 0]; _row2Column1 = doubleArray[2, 1]; _row2Column2 = doubleArray[2, 2];
    }

    /// <summary>Indicates whether the current matrix is equal to another matrix.</summary>
    public bool Equals(Matrix matrix)
    {
      return
        _row0Column0 == matrix._row0Column0 && _row0Column1 == matrix._row0Column1 && _row0Column2 == matrix._row0Column2 &&
        _row1Column0 == matrix._row1Column0 && _row1Column1 == matrix._row1Column1 && _row1Column2 == matrix._row1Column2 &&
        _row2Column0 == matrix._row2Column0 && _row2Column1 == matrix._row2Column1 && _row2Column2 == matrix._row2Column2;
    }

    /// <summary>Indicates whether the current matrix is approximately equal to another matrix.</summary>
    public bool EqualsApproximation(Matrix matrix, double tolerance)
    {
      return
        Math.Abs(_row0Column0 - matrix._row0Column0) <= tolerance &&
        Math.Abs(_row0Column1 - matrix._row0Column1) <= tolerance &&
        Math.Abs(_row0Column2 - matrix._row0Column2) <= tolerance &&
        Math.Abs(_row1Column0 - matrix._row1Column0) <= tolerance &&
        Math.Abs(_row1Column1 - matrix._row1Column1) <= tolerance &&
        Math.Abs(_row1Column2 - matrix._row1Column2) <= tolerance &&
        Math.Abs(_row2Column0 - matrix._row2Column0) <= tolerance &&
        Math.Abs(_row2Column1 - matrix._row2Column1) <= tolerance &&
        Math.Abs(_row2Column2 - matrix._row2Column2) <= tolerance;
    }

    /// <summary>Add left matrix to this matrix.</summary>
    public Matrix Add(Matrix matrix)
    {
      return new Matrix(
        _row0Column0 + matrix._row0Column0, _row0Column1 + matrix._row0Column1, _row0Column2 + matrix._row0Column2,
        _row1Column0 + matrix._row1Column0, _row1Column1 + matrix._row1Column1, _row1Column2 + matrix._row1Column2,
        _row2Column0 + matrix._row2Column0, _row2Column1 + matrix._row2Column1, _row2Column2 + matrix._row2Column2);
    }

    /// <summary>Multiply matrix times this matrix.</summary>
    public Matrix Multiply(Matrix matrix)
    {
      return new Matrix(
        matrix._row0Column0 * _row0Column0 + matrix._row0Column1 * _row1Column0 + matrix._row0Column2 * _row2Column0,
        matrix._row0Column0 * _row0Column1 + matrix._row0Column1 * _row1Column1 + matrix._row0Column2 * _row2Column1,
        matrix._row0Column0 * _row0Column2 + matrix._row0Column1 * _row1Column2 + matrix._row0Column2 * _row2Column2,
        matrix._row1Column0 * _row0Column0 + matrix._row1Column1 * _row1Column0 + matrix._row1Column2 * _row2Column0,
        matrix._row1Column0 * _row0Column1 + matrix._row1Column1 * _row1Column1 + matrix._row1Column2 * _row2Column1,
        matrix._row1Column0 * _row0Column2 + matrix._row1Column1 * _row1Column2 + matrix._row1Column2 * _row2Column2,
        matrix._row2Column0 * _row0Column0 + matrix._row2Column1 * _row1Column0 + matrix._row2Column2 * _row2Column0,
        matrix._row2Column0 * _row0Column1 + matrix._row2Column1 * _row1Column1 + matrix._row2Column2 * _row2Column1,
        matrix._row2Column0 * _row0Column2 + matrix._row2Column1 * _row1Column2 + matrix._row2Column2 * _row2Column2);
    }

    /// <summary>Multiply matrix times this matrix.</summary>
    /// <param name="matrix">The matrix to multiply.</param>
    public Matrix Multiply(double scalar)
    {
      return new Matrix(
        scalar * _row0Column0, scalar * _row0Column1, scalar * _row0Column2,
        scalar * _row1Column0, scalar * _row1Column1, scalar * _row1Column2,
        scalar * _row2Column0, scalar * _row2Column1, scalar * _row2Column2);
    }

    /// <summary>Computes the determinent of the matrix.</summary>
    public double Determinant
    {
      get
      {
        return
          _row0Column0 * _row1Column1 * _row2Column2 -
          _row0Column0 * _row1Column2 * _row2Column1 -
          _row0Column1 * _row1Column0 * _row2Column2 +
          _row0Column2 * _row1Column0 * _row2Column1 +
          _row0Column1 * _row1Column2 * _row2Column0 -
          _row0Column2 * _row1Column1 * _row2Column0;
      }
    }

    /// <summary>Returns the transpose of the matrix.</summary>
    public Matrix Transpose()
    {
      return new Matrix(
        _row0Column0, _row1Column0, _row2Column0,
        _row0Column1, _row1Column1, _row2Column1,
        _row0Column2, _row1Column1, _row2Column2);
    }

    //public Quaterniond ToQuaternion()
    /*public Quaternion ToQuaternion()
    {
      throw new MatrixException("ToQuaternion() method is not yet implemented.");
      //return new Quaterniond(this);
    }*/

    /// <summary>Computes a unique hash code for the instaance (overflow warning).</summary>
    public override int GetHashCode()
    {
      return
        _row0Column0.GetHashCode() ^ _row0Column1.GetHashCode() ^ _row0Column2.GetHashCode() ^
        _row1Column0.GetHashCode() ^ _row1Column1.GetHashCode() ^ _row1Column2.GetHashCode() ^
        _row2Column0.GetHashCode() ^ _row2Column1.GetHashCode() ^ _row2Column2.GetHashCode();
    }

    /// <summary>Gets a string representation of the matrix.</summary>
    public override string ToString()
    {
      return String.Format(
        "|{00}, {01}, {02}|\n" +
        "|{03}, {04}, {05}|\n" +
        "|{06}, {07}, {18}|\n" +
        _row0Column0, _row0Column1, _row0Column2,
        _row1Column0, _row1Column1, _row1Column2,
        _row2Column0, _row2Column1, _row2Column2);
    }
  }

  /// <summary>Class for throwing unique matrix related exceptions.</summary>
  public class MatrixException : Exception
  {
    public MatrixException(string message) : base(message) { }
  }
}