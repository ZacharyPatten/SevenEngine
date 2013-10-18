using System;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Engine;
using Engine.DataStructures;
using Engine.Imaging;
using Engine.Texts;
using Engine.Models;
using Engine.Mathematics;

namespace Engine
{
  /// <summary>Utility for the engine. Handles ALL rendering. It is good to handle this in one class because vast optimizations can be handled here.</summary>
  public static class Renderer
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



    private static SpriteBatch _batch = new SpriteBatch();
    private static HeapArrayStatic<StaticModel> _staticModelBatch = new HeapArrayStatic<StaticModel>(100000);

    private static int _currentTextureId = -1;

    public static void DrawImmediateModeVertex(Vector position, Color color, Point uvs)
    {
      GL.Color4(color.Red, color.Green, color.Blue, color.Alpha);
      GL.TexCoord2(uvs.X, uvs.Y);
      GL.Vertex3(position.X, position.Y, position.Z);
    }

    public static void DrawSprite(Sprite sprite)
    {
      if (sprite.Texture.Handle == _currentTextureId)
      {
        _batch.AddSprite(sprite);
      }
      else
      {
        // Draw all with current texture
        _batch.Draw();

        // Update texture info
        _currentTextureId = sprite.Texture.Handle;
        //GL.BindTexture(TextureTarget.Texture2D, _currentTextureId);
        _batch.AddSprite(sprite);
      }
    }

    /*public void DrawSubModel(RigidBodyPartModel subModel)
    {
      GL.BindTexture(TextureTarget.Texture2D, subModel.Texture.Id);

      // Push current Array Buffer state so we can restore it later
      GL.PushClientAttrib(ClientAttribMask.ClientVertexArrayBit);

      if (subModel.VertexBufferID == 0) return;
      //if (subModel.ElementBufferID == 0) return;

      if (GL.IsEnabled(EnableCap.Lighting))
      {
        // Normal Array Buffer
        if (subModel.NormalBufferID != 0)
        {
          // Bind to the Array Buffer ID
          GL.BindBuffer(BufferTarget.ArrayBuffer, subModel.NormalBufferID);
          // Set the Pointer to the current bound array describing how the data ia stored
          GL.NormalPointer(NormalPointerType.Float, 0, IntPtr.Zero);
          // Enable the client state so it will use this array buffer pointer
          GL.EnableClientState(ArrayCap.NormalArray);
        }
      }
      else
      {
        // Color Array Buffer (Colors not used when lighting is enabled)
        if (subModel.ColorBufferID != 0)
        {
          // Bind to the Array Buffer ID
          GL.BindBuffer(BufferTarget.ArrayBuffer, subModel.ColorBufferID);
          // Set the Pointer to the current bound array describing how the data ia stored
          GL.ColorPointer(3, ColorPointerType.Float, 0, IntPtr.Zero);
          // Enable the client state so it will use this array buffer pointer
          GL.EnableClientState(ArrayCap.ColorArray);
        }
      }

      // Texture Array Buffer
      if (GL.IsEnabled(EnableCap.Texture2D))
      {
        if (subModel.TexCoordBufferID != 0)
        {
          // Bind to the Array Buffer ID
          GL.BindBuffer(BufferTarget.ArrayBuffer, subModel.TexCoordBufferID);
          // Set the Pointer to the current bound array describing how the data ia stored
          GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, IntPtr.Zero);
          // Enable the client state so it will use this array buffer pointer
          GL.EnableClientState(ArrayCap.TextureCoordArray);
        }
      }

      // Vertex Array Buffer
      // Bind to the Array Buffer ID
      GL.BindBuffer(BufferTarget.ArrayBuffer, subModel.VertexBufferID);
      // Set the Pointer to the current bound array describing how the data ia stored
      GL.VertexPointer(3, VertexPointerType.Float, 0, IntPtr.Zero);
      // Enable the client state so it will use this array buffer pointer
      GL.EnableClientState(ArrayCap.VertexArray);

      if (subModel.ElementBufferID != 0)
      {
        // Element Array Buffer
        // Bind to the Array Buffer ID
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, subModel.ElementBufferID);
        // Set the Pointer to the current bound array describing how the data ia stored
        GL.IndexPointer(IndexPointerType.Int, 0, IntPtr.Zero);
        // Enable the client state so it will use this array buffer pointer
        GL.EnableClientState(ArrayCap.IndexArray);
        // Draw the elements in the element array buffer
        // Draws up items in the Color, Vertex, TexCoordinate, and Normal Buffers using indices in the ElementArrayBuffer
        GL.DrawElements(BeginMode.Triangles, subModel.Indeces.Length, DrawElementsType.UnsignedInt, 0);
      }
      else
      {
        GL.BindBuffer(BufferTarget.ArrayBuffer, subModel.VertexBufferID);
        GL.DrawArrays(BeginMode.Triangles, 0, subModel.Verteces.Length);
      }

      GL.PopClientAttrib();
    }*/

    public static void DrawSkyBox(SkyBox skyBox)
    {
      GL.MatrixMode(MatrixMode.Modelview);
      GL.LoadIdentity();
      GL.Translate(skyBox.Position.X, skyBox.Position.Y, skyBox.Position.Z);
      GL.Scale(skyBox.Scale.X, skyBox.Scale.Y, skyBox.Scale.Z);

      GL.BindTexture(TextureTarget.Texture2D, skyBox.Up.Handle);
      GL.Begin(BeginMode.Triangles);
      GL.Vertex3(-1, -1, -1);
      GL.TexCoord2(1, 1);
      GL.Vertex3(-1, -1, 1);
      GL.TexCoord2(0, 1);
      GL.Vertex3(-1, 1, 1);
      GL.TexCoord2(0, 0);
      GL.Vertex3(-1, 1, -1);
      GL.TexCoord2(1, 0);
      GL.End();

      GL.BindTexture(TextureTarget.Texture2D, skyBox.Back.Handle);
      GL.Begin(BeginMode.Triangles);
      GL.Vertex3(-1, 1, -1);
      GL.TexCoord2(0, 0);
      GL.Vertex3(1, 1, -1);
      GL.TexCoord2(1, 0);
      GL.Vertex3(1, -1, -1);
      GL.TexCoord2(1, 1);
      GL.Vertex3(-1, -1, -1);
      GL.TexCoord2(0, 1);
      GL.End();

      GL.BindTexture(TextureTarget.Texture2D, skyBox.Left.Handle);
      GL.Begin(BeginMode.Triangles);
      GL.Vertex3(1, 1, -1);
      GL.TexCoord2(1, 1);
      GL.Vertex3(1, 1, 1);
      GL.TexCoord2(0, 1);
      GL.Vertex3(1, -1, 1);
      GL.TexCoord2(0, 0);
      GL.Vertex3(1, -1, -1);
      GL.TexCoord2(1, 0);
      GL.End();

      GL.BindTexture(TextureTarget.Texture2D, skyBox.Right.Handle);
      GL.Begin(BeginMode.Triangles);
      GL.Vertex3(-1, -1, 1);
      GL.TexCoord2(0, 0);
      GL.Vertex3(1, -1, 1);
      GL.TexCoord2(1, 0);
      GL.Vertex3(1, 1, 1);
      GL.TexCoord2(1, 1);
      GL.Vertex3(-1, 1, 1);
      GL.TexCoord2(0, 1);
      GL.End();

      GL.BindTexture(TextureTarget.Texture2D, skyBox.Front.Handle);
      GL.Begin(BeginMode.Triangles);
      GL.Vertex3(1, 1, 1);
      GL.TexCoord2(1, 0);
      GL.Vertex3(1, 1, -1);
      GL.TexCoord2(1, 1);
      GL.Vertex3(-1, 1, -1);
      GL.TexCoord2(0, 1);
      GL.Vertex3(-1, 1, 1);
      GL.TexCoord2(0, 0);
      GL.End();
    }
    
    /// <summary>Renders a single static model using "GL.DrawArrays()".</summary>
    /// <param name="camera">The camera used to capture the world (needed for the world to camera transformation).</param>
    /// <param name="staticModel">The mesh to be rendered.</param>
    public static void DrawStaticModel(StaticModel staticModel)
    {
      // Select the model view matrix to apply the world and camera transformation.
      GL.MatrixMode(MatrixMode.Modelview);
      
      // This line is not necessary when the camera matrix is loaded in just after.
      //GL.LoadIdentity();
      
      // Get and load the camera transformatino matrix.
      Matrix4 cameraTransform = _currentCamera.GetMatrix();
      GL.LoadMatrix(ref cameraTransform);

      //GL.Translate(-camera.Position.X, -camera.Position.Y, -camera.Position.Z);
      //GL.Rotate(-camera.RotationX, 1, 0, 0);
      //GL.Rotate(-camera.RotationY, 0, 1, 0);
      //GL.Rotate(-camera.RotationZ, 0, 0, 1);

      // Apply the world transformation due to the mesh's position, scale, and rotation
      GL.Translate(staticModel.Position.X, staticModel.Position.Y, staticModel.Position.Z);
      GL.Rotate(staticModel.RotationAngle, staticModel.RotationAmmounts.X, staticModel.RotationAmmounts.Y, staticModel.RotationAmmounts.Z);
      GL.Scale(staticModel.Scale.X, staticModel.Scale.Y, staticModel.Scale.Z);

      for (int i = 0; i < staticModel.Meshes.Count; i++)
      //foreach (Link<Texture, StaticMesh> link in staticModel.Meshes)
      {
        // If there is no vertex buffer, nothing will render anyway, so we can stop it now.
        if (staticModel.Meshes[i].Right.VertexBufferHandle == 0 ||
          // If there is no color or texture, nothing will render anyway
          (staticModel.Meshes[i].Right.ColorBufferHandle == 0 && staticModel.Meshes[i].Right.TextureCoordinateBufferHandle == 0))
          return;

        // Push current Array Buffer state so we can restore it later
        GL.PushClientAttrib(ClientAttribMask.ClientVertexArrayBit);

        if (GL.IsEnabled(EnableCap.Lighting))
        {
          // Normal Array Buffer
          if (staticModel.Meshes[i].Right.NormalBufferHandle != 0)
          {
            // Bind to the Array Buffer ID
            GL.BindBuffer(BufferTarget.ArrayBuffer, staticModel.Meshes[i].Right.NormalBufferHandle);
            // Set the Pointer to the current bound array describing how the data ia stored
            GL.NormalPointer(NormalPointerType.Float, 0, IntPtr.Zero);
            // Enable the client state so it will use this array buffer pointer
            GL.EnableClientState(ArrayCap.NormalArray);
          }
        }
        else
        {
          // Color Array Buffer (Colors not used when lighting is enabled)
          if (staticModel.Meshes[i].Right.ColorBufferHandle != 0)
          {
            // Bind to the Array Buffer ID
            GL.BindBuffer(BufferTarget.ArrayBuffer, staticModel.Meshes[i].Right.ColorBufferHandle);
            // Set the Pointer to the current bound array describing how the data ia stored
            GL.ColorPointer(3, ColorPointerType.Float, 0, IntPtr.Zero);
            // Enable the client state so it will use this array buffer pointer
            GL.EnableClientState(ArrayCap.ColorArray);
          }
        }

        // Texture Array Buffer
        if (GL.IsEnabled(EnableCap.Texture2D))
        {
          if (staticModel.Meshes[i].Right.TextureCoordinateBufferHandle != 0)
          {
            // Bind the texture to which the UVs are mapping to.
            GL.BindTexture(TextureTarget.Texture2D, staticModel.Meshes[i].Left.Handle);
            // Bind to the Array Buffer ID
            GL.BindBuffer(BufferTarget.ArrayBuffer, staticModel.Meshes[i].Right.TextureCoordinateBufferHandle);
            // Set the Pointer to the current bound array describing how the data ia stored
            GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, IntPtr.Zero);
            // Enable the client state so it will use this array buffer pointer
            GL.EnableClientState(ArrayCap.TextureCoordArray);
          }
          else
            // Nothing will render if this branching is reached.
            return;
        }

        // Vertex Array Buffer
        // Bind to the Array Buffer ID
        GL.BindBuffer(BufferTarget.ArrayBuffer, staticModel.Meshes[i].Right.VertexBufferHandle);
        // Set the Pointer to the current bound array describing how the data ia stored
        GL.VertexPointer(3, VertexPointerType.Float, 0, IntPtr.Zero);
        // Enable the client state so it will use this array buffer pointer
        GL.EnableClientState(ArrayCap.VertexArray);

        if (staticModel.Meshes[i].Right.ElementBufferHandle != 0)
        {
          // Element Array Buffer
          // Bind to the Array Buffer ID
          GL.BindBuffer(BufferTarget.ElementArrayBuffer, staticModel.Meshes[i].Right.ElementBufferHandle);
          // Set the Pointer to the current bound array describing how the data ia stored
          GL.IndexPointer(IndexPointerType.Int, 0, IntPtr.Zero);
          // Enable the client state so it will use this array buffer pointer
          GL.EnableClientState(ArrayCap.IndexArray);
          // Draw the elements in the element array buffer
          // Draws up items in the Color, Vertex, TexCoordinate, and Normal Buffers using indices in the ElementArrayBuffer
          GL.DrawElements(BeginMode.Triangles, staticModel.Meshes[i].Right.VertexCount, DrawElementsType.UnsignedInt, 0);
        }
        else
        {
          // Select the vertex buffer as the active buffer (I don't think this is necessary but I haven't tested it yet).
          GL.BindBuffer(BufferTarget.ArrayBuffer, staticModel.Meshes[i].Right.VertexBufferHandle);
          // There is no index buffer, so we shoudl use "DrawArrays()" instead of "DrawIndeces()".
          GL.DrawArrays(BeginMode.Triangles, 0, staticModel.Meshes[i].Right.VertexCount);
        }

        GL.PopClientAttrib();
      }
    }

    public static void AddStaticModel(StaticModel model)
    {
      _staticModelBatch.Enqueue(
        model,
        (int)Math.Sqrt(
        Math.Pow(Renderer.CurrentCamera.Position.X - model.Position.X, 2) +
        Math.Pow(Renderer.CurrentCamera.Position.Y - model.Position.Y, 2) +
        Math.Pow(Renderer.CurrentCamera.Position.Z - model.Position.Z, 2)));
    }

    public static void Render()
    {
      //_batch.Draw();
      StaticModel model = _staticModelBatch.Dequeue();
      while (_staticModelBatch.Count > 0)
        DrawStaticModel(_staticModelBatch.Dequeue());
    }

    public static void DrawText(Text text)
    {
      foreach (CharacterSprite c in text.CharacterSprites)
      {
        DrawSprite(c.Sprite);
      }
    }
  }
}