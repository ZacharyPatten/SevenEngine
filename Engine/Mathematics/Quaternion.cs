using System;

namespace SevenEngine.Mathematics
{
  public class Quaternion
  {
    private double _x;
    private double _y;
    private double _z;
    private double _w;

    public double X { get { return _x; } set { _x = value; } }
    public double Y { get { return _y; } set { _y = value; } }
    public double Z { get { return _z; } set { _z = value; } }
    public double W { get { return _w; } set { _w = value; } }

    public Quaternion(double x, double y, double z, double w)
    {
      this._x = x;
      this._y = y;
      this._z = z;
      this._w = w;
    }

    public double Length { get { return (double)System.Math.Sqrt(_x * _x + _y * _y + _z * _z + _w * _w); } }

    public Quaternion normalize()
    {
      double length = Length;
      return new Quaternion(_x / length, _y / length, _z / length, _w / length);
    }

    public Quaternion Conjugate() { return new Quaternion(-_x, -_y, -_z, _w); }

    public Quaternion Multiply(Quaternion r)
    {
      double w_ = _w * r.W - _x * r.X - _y * r.Y - _z * r.Z;
      double x_ = _x * r.W + _w * r.X + _y * r.Z - _z * r.Y;
      double y_ = _y * r.W + _w * r.Y + _z * r.X - _x * r.Z;
      double z_ = _z * r.W + _w * r.Z + _x * r.Y - _y * r.X;
      return new Quaternion(x_, y_, z_, w_);
    }

    public Quaternion Multiply(Vector r)
    {
      double w_ = -_x * r.X - _y * r.Y - _z * r.Z;
      double x_ = _w * r.X + _y * r.Z - _z * r.Y;
      double y_ = _w * r.Y + _z * r.X - _x * r.Z;
      double z_ = _w * r.Z + _x * r.Y - _y * r.X;
      return new Quaternion(x_, y_, z_, w_);
    }
  }
}