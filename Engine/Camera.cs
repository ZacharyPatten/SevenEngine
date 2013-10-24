using System;

using OpenTK;
using OpenTK.Input;

using SevenEngine.Mathematics;

namespace SevenEngine
{
  public class Camera
  {
    private static Vector yAxis = new Vector(0, 1, 0);

    private double _fieldOfView;

    private double _positionSpeed;
    private double _lookSpeed;

    private Vector _position;
    private Vector _forward;
    private Vector _up;

    /// <summary>The field of view applied to the projection matrix during rendering transformations.</summary>
    public double FieldOfView { get { return _fieldOfView; } set { _fieldOfView = value; } }

    /// <summary>The speed at which the camera's position moves (camera movement sensitivity).</summary>
    public double PositionSpeed { get { return _positionSpeed; } set { _positionSpeed = value; } }
    public double LookSpeed { get { return _lookSpeed; } set { _lookSpeed = value; } }

    public Vector Position { get { return _position; } set { _position = value; } }
    public Vector Forward { get { return _forward; } set { _forward = value; } }
    public Vector Up { get { return _up; } set { _up = value; } }

    public Vector Backward { get { return -_forward; } }
    public Vector Right { get { return _forward.CrossProduct(_up).Normalise(); } }
    public Vector Left { get { return _up.CrossProduct(_forward).Normalise(); } }
    public Vector Down { get { return -_up; } }

    public Camera()
    {
      _position = new Vector(0, 0, 0);
      _forward = new Vector(0, 0, 1);
      _up = new Vector(0, 1, 0);

      _fieldOfView = .5d;
    }

    public Camera(Vector pos, Vector forward, Vector up, double fieldOfView)
    {
      _position = pos;
      _forward = forward.Normalise();
      _up = up.Normalise();
      _fieldOfView = fieldOfView;
    }

    public void Move(Vector direction, double ammount)
    {
      _position = _position + (direction * ammount);
    }

    public void RotateY(double angle)
    {
      Vector Haxis = yAxis.CrossProduct(_forward.Normalise());
      _forward = _forward.Rotate(angle, yAxis).Normalise();
      _up = _forward.CrossProduct(Haxis.Normalise());
    }

    public void RotateX(double angle)
    {
      Vector Haxis = yAxis.CrossProduct(_forward.Normalise());
      _forward = _forward.Rotate(angle, Haxis).Normalise();
      _up = _forward.CrossProduct(Haxis.Normalise());
    }

    public Matrix4 GetMatrix()
    {
      return Matrix4.LookAt(
        (float)_position.X, (float)_position.Y, (float)_position.Z,
        (float)_position.X + (float)_forward.X, (float)_position.Y + (float)_forward.Y, (float)_position.Z + (float)_forward.Z,
        (float)_up.X, (float)_up.Y, (float)_up.Z);
    }
  }
}