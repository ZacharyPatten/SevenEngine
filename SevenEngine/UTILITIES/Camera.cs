// Seven
// https://github.com/53V3N1X/SevenEngine
// LISCENSE: See "LISCENSE.txt" in th root project directory.
// SUPPORT: See "README.txt" in the root project directory.

using System;
using OpenTK;
using OpenTK.Input;
using Seven.Mathematics;

namespace SevenEngine
{
  /// <summary>Represents a camera to assist a game by generating a view matrix transformation.</summary>
  public class Camera
  {
    private static Vector<float> yAxis = new Vector<float>(0, 1, 0);

    private float _fieldOfView;

    public float _nearClipPlane = 1f;
    public float _farClipPlane = 1000000f;

    private float _positionSpeed;
    private float _lookSpeed;

    private Vector<float> _position;
    private Vector<float> _forward;
    private Vector<float> _up;

    public float NearClipPlane { get { return _nearClipPlane; } set { _nearClipPlane = value; } }
    public float FarClipPlane { get { return _farClipPlane; } set { _farClipPlane = value; } }

    /// <summary>The field of view applied to the projection matrix during rendering transformations.</summary>
    public float FieldOfView { get { return _fieldOfView; } set { _fieldOfView = value; } }

    /// <summary>The speed at which the camera's position moves (camera movement sensitivity).</summary>
    public float PositionSpeed { get { return _positionSpeed; } set { _positionSpeed = value; } }
    public float LookSpeed { get { return _lookSpeed; } set { _lookSpeed = value; } }

    public Vector<float> Position { get { return _position; } set { _position = value; } }
    public Vector<float> Forward { get { return _forward; } set { _forward = value; } }
    public Vector<float> Up { get { return _up; } set { _up = value; } }

    public Vector<float> Backward { get { return -_forward; } }
    public Vector<float> Right { get { return _forward.CrossProduct(_up).Normalize(); } }
    public Vector<float> Left { get { return _up.CrossProduct(_forward).Normalize(); } }
    public Vector<float> Down { get { return -_up; } }

    public Camera()
    {
      _position = new Vector<float>(0, 0, 0);
      _forward = new Vector<float>(0, 0, 1);
      _up = new Vector<float>(0, 1, 0);

      _fieldOfView = .5f;
    }

    public Camera(Vector<float> pos, Vector<float> forward, Vector<float> up, float fieldOfView)
    {
      _position = pos;
      _forward = forward.Normalize();
      _up = up.Normalize();
      _fieldOfView = fieldOfView;
    }

    public void Move(Vector<float> direction, float ammount)
    {
      _position = _position + (direction * ammount);
    }

    public void RotateY(float angle)
    {
      Vector<float> Haxis = yAxis.CrossProduct(_forward.Normalize());
      _forward = _forward.RotateBy(angle, 0, 1, 0).Normalize();
      _up = _forward.CrossProduct(Haxis.Normalize());
    }

    public void RotateX(float angle)
    {
      Vector<float> Haxis = yAxis.CrossProduct(_forward.Normalize());
      _forward = _forward.RotateBy(angle, Haxis.X, Haxis.Y, Haxis.Z).Normalize();
      _up = _forward.CrossProduct(Haxis.Normalize());
    }

    public Matrix4 GetMatrix()
    {
      Matrix4 camera = Matrix4.LookAt(
        _position.X, _position.Y, _position.Z,
        _position.X + _forward.X, _position.Y + _forward.Y, _position.Z + _forward.Z,
        _up.X, _up.Y, _up.Z);
      return camera;
    }
  }
}