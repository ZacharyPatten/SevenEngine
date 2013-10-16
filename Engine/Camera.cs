using System;

using OpenTK;
using OpenTK.Input;

using Engine.Mathematics;

namespace Engine
{
  /*public class Camera
  {
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
  }*/

  public class Camera
    {
	    private static Vector yAxis = new Vector(0,1,0);

        private double _fieldOfView;

        private double _positionSpeed;
        private double _lookSpeed;

	    private Vector _position;
	    private Vector _forward;
	    private Vector _up;

        public double FieldOfView { get { return _fieldOfView; } set { _fieldOfView = value; } }

        public double PositionSpeed { get { return _positionSpeed; } set { _positionSpeed = value; } }
        public double LookSpeed { get { return _lookSpeed; } set { _lookSpeed = value; } }

        //public Vector Position { get { return _position; } set { _position = value; } }
        //public Vector Forward { get { return _forward; } set { _forward = value; } }
        //public Vector Up { get { return _up; } set { _up = value; } }

        //public Vector Backward { get { return -_forward; } }
        //public Vector Right { get { return _up.CrossProduct(_forward.Normalise()); } }
        //public Vector Left { get { return _forward.CrossProduct(_up.Normalise()); } }
        //public Vector Down { get { return -_up; } }

        public Vector Position { get { return _position; } set { _position = value; } }
        public Vector Forward { get { return _forward; } set { _forward = value; } }
        public Vector Up { get { return _up; } set { _up = value; } }

        public Vector Backward { get { return -_forward; } }
        public Vector Right { get { return _up.CrossProduct(_forward).Normalise(); } }
        public Vector Left { get { return _forward.CrossProduct(_up).Normalise(); } }
        public Vector Down { get { return -_up; } }

	    public Camera()
	    {
		    _position = new Vector(0,0,0);
            _forward = new Vector(0,0,1);
            _up = new Vector(0,1,0);

            _fieldOfView = .5d;
	    }
	
	    public Camera(Vector pos, Vector forward, Vector up, double fieldOfView)
	    {
		    _position = pos;
            _forward = forward.Normalise();
		    _up = up.Normalise();
            _fieldOfView = fieldOfView;
	    }

	    /*bool mouseLocked = false;
	    Vector2f centerPosition = new Vector2f(Window.getWidth()/2, Window.getHeight()/2);
	
	    public void input()
	    {
		    float sensitivity = 0.5f;
		    float movAmt = (float)(10 * Time.getDelta());
    //		float rotAmt = (float)(100 * Time.getDelta());
		
		    if(Input.getKey(Input.KEY_ESCAPE))
		    {
			    Input.setCursor(true);
			    mouseLocked = false;
		    }
		    if(Input.getMouseDown(0))
		    {
			    Input.setMousePosition(centerPosition);
			    Input.setCursor(false);
			    mouseLocked = true;
		    }
		
		    if(Input.getKey(Input.KEY_W))
			    move(getForward(), movAmt);
		    if(Input.getKey(Input.KEY_S))
			    move(getBackward(), -movAmt);
		    if(Input.getKey(Input.KEY_A))
			    move(getLeft(), movAmt);
		    if(Input.getKey(Input.KEY_D))
			    move(getRight(), movAmt);
		
		    if(mouseLocked)
		    {
			    Vector2f deltaPos = Input.getMousePosition().sub(centerPosition);
			
			    bool rotY = deltaPos.getX() != 0;
			    bool rotX = deltaPos.getY() != 0;
			
			    if(rotY)
				    rotateY(deltaPos.getX() * sensitivity);
			    if(rotX)
				    rotateX(-deltaPos.getY() * sensitivity);
				
			    if(rotY || rotX)
				    Input.setMousePosition(new Vector2f(Window.getWidth()/2, Window.getHeight()/2));
		    }
	    }*/
	
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
            (float)_forward.X, (float)_forward.Y, (float)_forward.Z,
            (float)_up.X, (float)_up.Y, (float)_up.Z);
        }
    }
}