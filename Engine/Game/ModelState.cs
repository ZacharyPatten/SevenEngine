using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Engine;
//using OpenTK;

namespace Engine
{
  public class ModelState : IGameObject
  {
    public ModelState() { }
    public void Update(double elapsedTime) { }

    public void Render()
    {
      GL.Disable(EnableCap.Texture2D);
      GL.ClearColor(1, 1, 1, 0);
      GL.Clear(ClearBufferMask.ColorBufferBit);

      // This is a simple way of using a camera
      GL.MatrixMode(MatrixMode.Modelview);
      GL.LoadIdentity();
      Vector cameraPosition = new Vector(-75, 125, -500); // half a meter back on the Z axis
      Vector cameraLookAt = new Vector(0, 0, 0); // make the camera look at the world origin.
      Vector cameraUpVector = new Vector(0, 1, 0);
      //static Matrix4 OpenTK.Matrix4.LookAt ( Vector3 eye, Vector3 target, Vector3 up )
      Matrix4 lookat = Matrix4.LookAt((float)cameraPosition.X, (float)cameraPosition.Y, (float)cameraPosition.Z,
                      (float)cameraLookAt.X, (float)cameraLookAt.Y, (float)cameraLookAt.Z,
                      (float)cameraUpVector.X, (float)cameraUpVector.Y, (float)cameraUpVector.Z);
      /*Glu.gluLookAt(cameraPosition.X, cameraPosition.Y, cameraPosition.Z,
                      cameraLookAt.X, cameraLookAt.Y, cameraLookAt.Z,
                      cameraUpVector.X, cameraUpVector.Y, cameraUpVector.Z);*/


      // This draws a pyramid - can you make it draw a cube?
      GL.Begin(BeginMode.TriangleFan);
      {
        GL.Color3(1, 0, 0);
        GL.Vertex3(100.0f, 100, 0.0f);

        GL.Color3(0, 1, 0);
        GL.Vertex3(-100, -100, 100);

        GL.Color3(0, 0, 1);
        GL.Vertex3(100, -100, 100);

        GL.Color3(0, 1, 0);
        GL.Vertex3(100, -100, -100);

        GL.Color3(0, 0, 1);
        GL.Vertex3(-100, -100, -100);

        GL.Color3(0, 1, 0);
        GL.Vertex3(-100, -100, 100);
      }
      GL.End();
    }
  }
}
