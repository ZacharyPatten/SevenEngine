using System;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine
{
  /// <summary>Use this class to load transformation matrices into the rendering pipline.</summary>
  public static class TransformationManager
  {
    private static Camera _currentCamera;

    // I will change this class in general is a short term fix. will probably use the renderer to store the transformations.
    private static int _screenHeight = 600;
    private static int _screenWidth = 800; 

    public static Camera CurrentCamera { get { return _currentCamera; } set { _currentCamera = value; } }

    internal static int ScreenWidth
    {
      get { return _screenWidth; }
      set
      {
        _screenWidth = value;
        GL.MatrixMode(MatrixMode.Projection);
        //GL.LoadIdentity(); // this is not needed because I use "LoadMatrix()" just after it (but you may want it if you change the following code)
        Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView((float)_currentCamera.FieldOfView, (float)_screenWidth / (float)_screenHeight, .1f, 10000f);
        GL.LoadMatrix(ref perspective);
      } 
    }

    internal static int ScreenHeight
    {
      get { return _screenHeight; }
      set
      {
        _screenHeight = value;
        GL.MatrixMode(MatrixMode.Projection);
        //GL.LoadIdentity(); // this is not needed because I use "LoadMatrix()" just after it (but you may want it if you change the following code)
        Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView((float)_currentCamera.FieldOfView, (float)_screenWidth / (float)_screenHeight, .1f, 10000f);
        GL.LoadMatrix(ref perspective);
      }
    }


    public static void SetProjectionMatrix()
    {
      // This creates a projection matrix that transforms objects due to depth. (applies depth perception)
      GL.MatrixMode(MatrixMode.Projection);
      //GL.LoadIdentity(); // this is not needed because I use "LoadMatrix()" just after it (but you may want it if you change the following code)
      Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView((float)_currentCamera.FieldOfView, (float)800 / (float)600, .1f, 10000f);
      GL.LoadMatrix(ref perspective);
    }
  }
}