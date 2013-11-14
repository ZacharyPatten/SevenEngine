// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use with the following condition: any code using or originally taken 
// from the SevenEngine project must include citation to its original author(s) located at the
// top of each source code file, or you may include a reference to the SevenEngine project as
// a whole but you must include the current SevenEngine official website URL and logo.
// - Thanks.  :)  (support: seven@sevenengine.com)

// Author(s):
// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com
// Last Edited: 10-26-13

using System;
using OpenTK;
using OpenTK.Input;
using SevenEngine.Mathematics;

namespace SevenEngine
{
  public class Camera
  {
    private static Vector yAxis = new Vector(0, 1, 0);

    private float _fieldOfView;

    public float _nearClipPlane = 1f;
    public float _farClipPlane = 1000000f;

    private float _positionSpeed;
    private float _lookSpeed;

    private Vector _position;
    private Vector _forward;
    private Vector _up;

    public float NearClipPlane { get { return _nearClipPlane; } set { _nearClipPlane = value; } }
    public float FarClipPlane { get { return _farClipPlane; } set { _farClipPlane = value; } }

    /// <summary>The field of view applied to the projection matrix during rendering transformations.</summary>
    public float FieldOfView { get { return _fieldOfView; } set { _fieldOfView = value; } }

    /// <summary>The speed at which the camera's position moves (camera movement sensitivity).</summary>
    public float PositionSpeed { get { return _positionSpeed; } set { _positionSpeed = value; } }
    public float LookSpeed { get { return _lookSpeed; } set { _lookSpeed = value; } }

    public Vector Position { get { return _position; } set { _position = value; } }
    public Vector Forward { get { return _forward; } set { _forward = value; } }
    public Vector Up { get { return _up; } set { _up = value; } }

    public Vector Backward { get { return -_forward; } }
    public Vector Right { get { return _forward.CrossProduct(_up).Normalize(); } }
    public Vector Left { get { return _up.CrossProduct(_forward).Normalize(); } }
    public Vector Down { get { return -_up; } }

    public Camera()
    {
      _position = new Vector(0, 0, 0);
      _forward = new Vector(0, 0, 1);
      _up = new Vector(0, 1, 0);

      _fieldOfView = .5f;
    }

    public Camera(Vector pos, Vector forward, Vector up, float fieldOfView)
    {
      _position = pos;
      _forward = forward.Normalize();
      _up = up.Normalize();
      _fieldOfView = fieldOfView;
    }

    public void Move(Vector direction, float ammount)
    {
      _position = _position + (direction * ammount);
    }

    public void RotateY(float angle)
    {
      Vector Haxis = yAxis.CrossProduct(_forward.Normalize());
      _forward = _forward.RotateBy(angle, 0, 1, 0).Normalize();
      _up = _forward.CrossProduct(Haxis.Normalize());
    }

    public void RotateX(float angle)
    {
      Vector Haxis = yAxis.CrossProduct(_forward.Normalize());
      _forward = _forward.RotateBy(angle, Haxis.X, Haxis.Y, Haxis.Z).Normalize();
      _up = _forward.CrossProduct(Haxis.Normalize());
    }

    public Matrix4 GetMatrix()
    {
      return Matrix4.LookAt(
        _position.X, _position.Y, _position.Z,
        _position.X + _forward.X, _position.Y + _forward.Y, _position.Z + _forward.Z,
        _up.X, _up.Y, _up.Z);
    }
  }
}