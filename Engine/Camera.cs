using OpenTK;
using System;
using System.Windows.Input;
using OpenTK.Input;

namespace Engine
{
  public class Camera
  {
    /*private Vector _position, _right, _up, _forward;
    private double _fieldOfView, _maximumViewDistance, _minimumViewDistance;
    private Matrix _viewOrientation;

    public Vector Position
    {
      get { return _position; }
      set { _position = value; }
    }

    public Vector Right
    {
      get { return _right; }
      set
      {
        _right = value;
        _viewOrientation =
          new Matrix(
            _right.X, _right.Y, _right.Z,
            _up.X, _up.Y, _up.Z,
            _forward.Z, _forward.Y, _forward.Z);
      }
    }

    public Vector Up
    {
      get { return _up; }
      set
      {
        _up = value;
        _viewOrientation =
          new Matrix(
            _right.X, _right.Y, _right.Z,
            _up.X, _up.Y, _up.Z,
            _forward.Z, _forward.Y, _forward.Z);
      }
    }

    public Vector Forward
    {
      get { return _forward; }
      set
      {
        _forward = value;
        _viewOrientation =
          new Matrix(
            _right.X, _right.Y, _right.Z,
            _up.X, _up.Y, _up.Z,
            _forward.Z, _forward.Y, _forward.Z);
      }
    }

    public double FieldOfView
    {
      get { return _fieldOfView; }
      set { _fieldOfView = value; }
    }

    public double MaximumViewDistance
    {
      get { return _maximumViewDistance; }
      set { _maximumViewDistance = value; }
    }

    public double MinimumViewDistance
    {
      get { return _minimumViewDistance; }
      set { _minimumViewDistance = value; }
    }

    public Camera(Vector position, Vector right, Vector up, Vector forward, double fieldOfView, double maximumViewDistance, double minimumViewDistance)
    {
      _position = position;
      _right = right;
      _up = up;
      _forward = forward;
      _fieldOfView = fieldOfView;
      _maximumViewDistance = maximumViewDistance;
      _minimumViewDistance = minimumViewDistance;
      _viewOrientation =
          new Matrix(
            _right.X, _right.Y, _right.Z,
            _up.X, _up.Y, _up.Z,
            _forward.Z, _forward.Y, _forward.Z);
    }

    public Matrix4 GetCameraTransform()
    {
      return Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);
    }*/

    protected Vector _position;
    protected Vector _up;
    protected Vector _forward;

    protected float _fieldOfView;

    protected float _rotationX;
    protected float _rotationY;
    protected float _rotationZ;

    float _horizontalAngle = 3.14f;
    float _verticalAngle = 0.0f;

    public Vector Position { get { return _position; } set { _position = value; } }
    public Vector Up { get { return _up; } set { _up = value; } }
    public Vector Forward { get { return _forward; } set { _forward = value; } }

    public float FieldOfView { get { return _fieldOfView; } set { _fieldOfView = value; } }

    public float RotationX { get { return _rotationX; } set { _rotationX = value; } }
    public float RotationY { get { return _rotationY; } set { _rotationY = value; } }
    public float RotationZ { get { return _rotationZ; } set { _rotationZ = value; } }

    public Camera()
    {
      _position = new Vector(0, 0, 0);
      _up = new Vector(0, 1, 0);
      _forward = new Vector(0, 0, -1);

      _fieldOfView = .5f;

      _rotationX = 0;
      _rotationY = 0;
      _rotationZ = 0;
    }

    public void RotateX(float angle)
    {
      _up = new Vector(Math.Cos(angle) * _up.X - Math.Sin(angle) * _up.X, Math.Sin(angle) * _up.Y + Math.Cos(angle) * _up.Y, _up.Z);
    }

    public void RotateY(float angle)
    {
      _forward = new Vector(Math.Cos(angle) * _forward.X - Math.Sin(angle) * _forward.X, _forward.Y, Math.Sin(angle) * _forward.Z + Math.Cos(angle) * _forward.Z);
    }

    public void RotateZ(float angle)
    {

    }

    public Matrix4 GetViewTransform(double elapsedTime, int mousex, int mousey)
    {
      // Compute new orientation
      _horizontalAngle += .005f * (float)(1024 / 2 - mousex);
      _verticalAngle += .005f * (float)(768 / 2 - mousey);

	  // Direction : Spherical coordinates to Cartesian coordinates conversion
	  Vector3 direction = new Vector3(
		  (float)(Math.Cos(_verticalAngle) * Math.Sin(_horizontalAngle)), 
		  (float)Math.Sin(_verticalAngle),
		  (float)(Math.Cos(_verticalAngle) * Math.Cos(_horizontalAngle)));
	
	  // Right vector
	  Vector3 right = new Vector3(
		  (float)Math.Sin(_horizontalAngle - 3.14f/2.0f), 
		  0,
		  (float)Math.Cos(_horizontalAngle - 3.14f/2.0f));
	
	  // Up vector
	  Vector3 up = Vector3.Cross(right, direction);
        Vector3 position = new Vector3((float)_position.X, (float)_position.Y, (float)_position.Z);
      return Matrix4.LookAt(position, position+direction, up);
    }
  }
}
