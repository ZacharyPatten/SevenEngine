using OpenTK;

namespace Engine
{
  public struct Camera
  {
    /*private Vector3 _position, _right, _up, _forward;
    private double _fieldOfView, _maximumViewDistance, _minimumViewDistance;
    private Matrix _viewOrientation;

    public Vector3 Position
    {
      get { return _position; }
      set { _position = value; }
    }

    public Vector3 Right
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

    public Vector3 Up
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

    public Vector3 Forward
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

    public Camera(Vector3 position, Vector3 right, Vector3 up, Vector3 forward, double fieldOfView, double maximumViewDistance, double minimumViewDistance)
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
  }*/

    /// <summary>The current position of this model in the world.</summary>
    public Vector3d Position { get; set; }
    /// <summary>The current scale of this model in the world.</summary>
    public Vector3d Scale { get; set; }
    /// <summary>Represents the ammount of rotation around each axis (by the RotationAngle).</summary>
    public Vector3d RotationAmmounts { get; set; }
    /// <summary>Represents the amount of rotation applied to the x, y, and z of RotationAmmounts.</summary>
    public float RotationAngle { get; set; }
  }
}
