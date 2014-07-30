// Seven
// https://github.com/53V3N1X/SevenEngine
// LISCENSE: See "LISCENSE.txt" in th root project directory.
// SUPPORT: See "README.txt" in the root project directory.

using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SevenEngine;
using Seven.Structures;
using SevenEngine.Imaging;
using SevenEngine.Texts;
using SevenEngine.StaticModels;
using Seven.Mathematics;
using SevenEngine.Shaders;

namespace SevenEngine
{
  /// <summary>Utility for the engine. Handles ALL rendering. It is good to handle this in one class because vast optimizations can be handled here.</summary>
  public static class Renderer
  {
    #region Shaders

    private static ShaderProgram _defaultShaderProgram;

    /// <summary>The default shader that will be used unless the item has a shader override.</summary>
    public static ShaderProgram DefaultShaderProgram { get { return _defaultShaderProgram; } set { _defaultShaderProgram = value; } }

    #endregion

    #region Transformations
    
    private static Matrix<float> _projectionMatrix;
    private static Matrix<float> _viewMatrix;
    private static Matrix<float> _modelMatrix;

    public static Matrix<float> MatrixProjection { get { return _projectionMatrix; } set { _projectionMatrix = value; } }
    public static Matrix<float> MatrixView { get { return _viewMatrix; } set { _viewMatrix = value; } }
    public static Matrix<float> MatrixModel { get { return _modelMatrix; } set { _modelMatrix = value; } }

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
        //GL.MatrixMode(MatrixMode.Projection);
        ////GL.LoadIdentity(); // this is not needed because I use "LoadMatrix()" just after it (but you may want it if you change the following code)
        //Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView((float)_currentCamera.FieldOfView, (float)_screenWidth / (float)_screenHeight, .1f, 10000f);
        //GL.LoadMatrix(ref perspective);
      }
    }

    internal static int ScreenHeight
    {
      get { return _screenHeight; }
      set
      {
        _screenHeight = value;
        //GL.MatrixMode(MatrixMode.Projection);
        ////GL.LoadIdentity(); // this is not needed because I use "LoadMatrix()" just after it (but you may want it if you change the following code)
        //Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView((float)_currentCamera.FieldOfView, (float)_screenWidth / (float)_screenHeight, .1f, 10000f);
        //GL.LoadMatrix(ref perspective);
      }
    }

    public static void SetViewport(int x, int y, int width, int height)
    {
      GL.Viewport(x, y, width, height);
    }

    public static void SetOrthographicMatrix()
    {
      GL.MatrixMode(MatrixMode.Projection);
      GL.LoadIdentity();
      float halfWidth = _screenWidth / 2;
      float halfHeight = _screenHeight / 2;
      GL.Ortho(-halfWidth, halfWidth, -halfHeight, halfHeight, -1000, 1000);
    }

    public static void SetProjectionMatrix()
    {
      // This creates a projection matrix that transforms objects due to depth. (applies depth perception)
      GL.MatrixMode(MatrixMode.Projection);
      //GL.LoadIdentity(); // this is not needed because I use "LoadMatrix()" just after it (but you may want it if you change the following code)
      Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(
        (float)_currentCamera.FieldOfView, 
        (float)_screenWidth / (float)_screenHeight, 
        (float)_currentCamera.NearClipPlane, 
        (float)_currentCamera.FarClipPlane);
      GL.LoadMatrix(ref perspective);
    }

    #endregion

    #region Generic Drawing Tools

    /// <summary>Draws triangles from provided float arrays.</summary>
    /// <param name="verteces">The list of vertex positions (x, y, z) to draw.</param>
    /// <param name="colors">The list of vertex colors (r, g, b, a) to draw.</param>
    /// <param name="textureCoordinates">The list of vertex texture mappings (u, v) to draw.</param>
    /// <param name="texture">The parameter this drawing will be mapped to.</param>
    public static void DrawTriangles(float[] verteces, float[] colors, float[] textureCoordinates, Texture texture)
    {
      GL.EnableClientState(ArrayCap.ColorArray);
      GL.EnableClientState(ArrayCap.VertexArray);
      GL.EnableClientState(ArrayCap.TextureCoordArray);

      GL.VertexPointer(3, VertexPointerType.Float, 0, verteces);
      GL.ColorPointer(4, ColorPointerType.Float, 0, colors);
      GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, textureCoordinates);

      GL.BindTexture(TextureTarget.Texture2D, texture.GpuHandle);

      GL.DrawArrays(OpenTK.Graphics.OpenGL.BeginMode.Triangles, 0, verteces.Length / 3);
      //GL.DrawArrays(PrimitiveType.Triangles, 0, verteces.Length / 3);
    }

    public static void DrawLine(Vector<float> from, Vector<float> to, Color color)
    {
      SetProjectionMatrix();
      GL.MatrixMode(MatrixMode.Modelview);
      Matrix4 camera = _currentCamera.GetMatrix();
      GL.LoadMatrix(ref camera);

      GL.UseProgram(ShaderManager.ColorShader.GpuHandle);
      int uniformLocation = GL.GetUniformLocation(ShaderManager.ColorShader.GpuHandle, "color");
      GL.Uniform4(uniformLocation, new Color4(color.R / 255f, color.G / 255f, color.B / 255f, 1));//color.A / 255f));

      //GL.Begin(PrimitiveType.Lines);
      GL.Begin(BeginMode.Lines);
      GL.Vertex3(from.X, from.Y, from.Z);
      GL.Vertex3(to.X, to.Y, to.Z);
      GL.Color3(color.R / 255f, color.G / 255f, color.B / 255f);
      GL.End();
    }

    #endregion

    #region Text

    private static Font _font;

    /// <summary>The current font that will be used when rendering text.</summary>
    public static Font Font { get { return _font; } set { _font = value; } }

    public static void RenderText(string message, float x, float y, float scale, float rotation, Color color)
    {
      // Apply the 2D orthographic matrix transformation
      SetOrthographicMatrix();

      rotation = rotation * 180f / Constants.pi_float;

      // Set the text shader program and pass in the color as a parameter
      GL.UseProgram(ShaderManager.TextShader.GpuHandle);
      int uniformLocation = GL.GetUniformLocation(ShaderManager.TextShader.GpuHandle, "color");
      GL.Uniform4(uniformLocation, new Color4(color.R / 255f, color.G/ 255f, color.B / 255f, color.A / 255f));

      // Apply the model view matrix transformations
      GL.MatrixMode(MatrixMode.Modelview);
      GL.LoadIdentity();
      GL.Translate((x * _screenWidth - _screenWidth / 2f), (y * _screenHeight - _screenHeight / 2f), 0f);
      if (rotation != 0)
        GL.Rotate(rotation, 0, 0, 1);

      // Set up the verteces (hardware instanced for all character sprites)
      GL.BindBuffer(BufferTarget.ArrayBuffer, CharacterSprite.GpuVertexBufferHandle);
      GL.VertexPointer(3, VertexPointerType.Float, 0, IntPtr.Zero);
      GL.EnableClientState(ArrayCap.VertexArray);

      // this standardizes the size (not depedent on sprite sheet size)
      float sizeRatio = scale / (float)_font.LineHeight;

      for (int i = 0; i < message.Length; i++)
      {
        CharacterSprite sprite = _font.Get(message[i]);

        // Calculate the per-character transformational values
        float ySize = sizeRatio * (float)sprite.OriginalHeight;
        float yOffset = sizeRatio * (float)sprite.YOffset;
        float xSize = sizeRatio * (float)sprite.OriginalWidth;
        float xRatio = xSize / (float)(sprite.OriginalWidth);
        float xOffset = xRatio * (float)sprite.XOffset;
        float xAdvance = xRatio * (float)sprite.XAdvance;
        // Kearning (extra adjustments between specific charasters)
        if (i + 1 < message.Length)
          xAdvance += xRatio * (float)sprite.CheckKearning(message[i + 1]);

        // Apply the character offsets and scaling
        GL.Translate(xOffset, -yOffset, 0f);
        GL.Scale(xSize, ySize, 0f);

        // Bind the texture and set up the texture coordinates
        GL.BindTexture(TextureTarget.Texture2D, sprite.Texture.GpuHandle);
        GL.BindBuffer(BufferTarget.ArrayBuffer, sprite.GPUTextureCoordinateBufferHandle);
        GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, IntPtr.Zero);
        GL.EnableClientState(ArrayCap.TextureCoordArray);

        // Perform the render
        //GL.DrawArrays(PrimitiveType.Triangles, 0, sprite.VertexCount);
        GL.DrawArrays(BeginMode.Triangles, 0, sprite.VertexCount);

        // Remove the per character transforms and advance to the next charachers position
        GL.Scale(1 / xSize, 1 / ySize, 0f);
        GL.Translate(-xOffset + xAdvance, yOffset, 0f);
      }
    }

    #endregion

    #region Sprite

    public static void DrawSprite(Sprite sprite)
    {

      SetOrthographicMatrix();

      GL.MatrixMode(MatrixMode.Modelview);
      GL.LoadIdentity();

      //Matrix4 cameraTransform = _currentCamera.GetMatrix();
      //GL.LoadMatrix(ref cameraTransform);

      //GL.Translate(-_currentCamera.Position.X, -_currentCamera.Position.Y, -_currentCamera.Position.Z);

      GL.Translate(sprite.Position.X, sprite.Position.Y, sprite.Position.Z);
      //GL.Rotate(1, -_currentCamera.Forward.X, -_currentCamera.Forward.Y, -_currentCamera.Forward.Z);
      //GL.Rotate(-1, 0, _currentCamera.Forward.Y, 0);
      GL.Rotate(sprite.Rotation, 0, 0, 1);
      GL.Scale(sprite.Scale.X, sprite.Scale.Y, 1);

      // Set up verteces
      GL.BindBuffer(BufferTarget.ArrayBuffer, sprite.GpuVertexBufferHandle);
      GL.VertexPointer(3, VertexPointerType.Float, 0, IntPtr.Zero);
      // Enable the client state so it will use this array buffer pointer
      GL.EnableClientState(ArrayCap.VertexArray);

      // Select texture and set up texture coordinates
      GL.BindTexture(TextureTarget.Texture2D, sprite.Texture.GpuHandle);
      GL.BindBuffer(BufferTarget.ArrayBuffer, sprite.GPUTextureCoordinateBufferHandle);
      GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, IntPtr.Zero);
      GL.EnableClientState(ArrayCap.TextureCoordArray);

      // Select the vertex buffer as the active buffer (I don't think this is necessary but I haven't tested it yet).
      GL.BindBuffer(BufferTarget.ArrayBuffer, sprite.GpuVertexBufferHandle);
      // There is no index buffer, so we shoudl use "DrawArrays()" instead of "DrawIndeces()".
      //GL.DrawArrays(PrimitiveType.Triangles, 0, sprite.VertexCount);
      GL.DrawArrays(BeginMode.Triangles, 0, sprite.VertexCount);
    }

    #endregion

    #region Skybox

    public static void DrawSkybox(SkyBox skybox)
    {
      // Apply the 3D projection matrix transformation
      SetProjectionMatrix();

      //if (skybox.ShaderOverride != null)
      //  GL.UseProgram(_defaultShaderProgram.GpuHandle);
      //else
      //  GL.UseProgram(ShaderManager.DefaultShader.GpuHandle);
      GL.UseProgram(_defaultShaderProgram.GpuHandle);

      // Apply the model view matrix transformations
      GL.MatrixMode(MatrixMode.Modelview);
      // Apply the camera transformation
      Matrix4 cameraTransform = _currentCamera.GetMatrix();
      GL.LoadMatrix(ref cameraTransform);
      // Apply the world transformation
      GL.Translate(skybox.Position.X, skybox.Position.Y, skybox.Position.Z);
      GL.Scale(skybox.Scale.X, skybox.Scale.Y, skybox.Scale.Z);
      
      // Set up verteces
      GL.BindBuffer(BufferTarget.ArrayBuffer, skybox.GpuVertexBufferHandle);
      GL.VertexPointer(3, VertexPointerType.Float, 0, IntPtr.Zero);
      GL.EnableClientState(ArrayCap.VertexArray);

      // Select texture and set up texture coordinates
      GL.BindBuffer(BufferTarget.ArrayBuffer, skybox.GPUTextureCoordinateBufferHandle);
      GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, IntPtr.Zero);
      GL.EnableClientState(ArrayCap.TextureCoordArray);

      // Render left side of skybox
      GL.BindTexture(TextureTarget.Texture2D, skybox.Left.GpuHandle);
      //GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
      GL.DrawArrays(BeginMode.Triangles, 0, 6);

      // Render front side of skybox
      GL.BindTexture(TextureTarget.Texture2D, skybox.Front.GpuHandle);
      //GL.DrawArrays(PrimitiveType.Triangles, 6, 6);
      GL.DrawArrays(BeginMode.Triangles, 6, 6);

      // Render right side of skybox
      GL.BindTexture(TextureTarget.Texture2D, skybox.Right.GpuHandle);
      //GL.DrawArrays(PrimitiveType.Triangles, 12, 6);
      GL.DrawArrays(BeginMode.Triangles, 12, 6);

      // Render back side of skybox
      GL.BindTexture(TextureTarget.Texture2D, skybox.Back.GpuHandle);
      //GL.DrawArrays(PrimitiveType.Triangles, 18, 6);
      GL.DrawArrays(BeginMode.Triangles, 18, 6);

      // Render top side of skybox
      GL.BindTexture(TextureTarget.Texture2D, skybox.Top.GpuHandle);
      //GL.DrawArrays(PrimitiveType.Triangles, 24, 6);
      GL.DrawArrays(BeginMode.Triangles, 24, 6);
    }

    #endregion

    #region StaticModel

    /// <summary>Renders a single static model using "GL.DrawArrays()".</summary>
    /// <param name="staticModel">The mesh to be rendered.</param>
    public static void DrawStaticModel(StaticModel staticModel)
    {
      /*Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(
        (float)_currentCamera.FieldOfView,
        (float)_screenWidth / (float)_screenHeight,
        (float)_currentCamera.NearClipPlane,
        (float)_currentCamera.FarClipPlane);
      perspective.Transpose();
      Matrix4 camera = _currentCamera.GetMatrix();
      camera = perspective;*/

      // Apply the 3D projection matrix transformations
      SetProjectionMatrix();

      if (staticModel.ShaderOverride != null)
        GL.UseProgram(staticModel.ShaderOverride.GpuHandle);
      else
        GL.UseProgram(DefaultShaderProgram.GpuHandle);

      //GL.UseProgram(ShaderManager.LightShader.GpuHandle);

      // Apply the model view matrix transformations
      GL.MatrixMode(MatrixMode.Modelview);
      // Apply the camera transformation
      Matrix4 cameraTransform = _currentCamera.GetMatrix();
      GL.LoadMatrix(ref cameraTransform);
      // Apply the world transformation
      GL.Translate(staticModel.Position.X, staticModel.Position.Y, staticModel.Position.Z);
      GL.Rotate(staticModel.Orientation.W * 180f / 3.14f, staticModel.Orientation.X, staticModel.Orientation.Y, staticModel.Orientation.Z);
      GL.Scale(staticModel.Scale.X, staticModel.Scale.Y, staticModel.Scale.Z);

      // Call the drawing functions for each mesh within the model
      staticModel.Meshes.Foreach((Foreach<StaticMesh>)DrawStaticModelPart);
    }

    private static void DrawStaticModelPart(StaticMesh staticMesh)
    {
      // Make sure something will render
      if (staticMesh.VertexBufferHandle == 0 ||
        (staticMesh.ColorBufferHandle == 0 && staticMesh.TextureCoordinateBufferHandle == 0))
        return;

      // Push current Array Buffer state so we can restore it later
      GL.PushClientAttrib(ClientAttribMask.ClientVertexArrayBit);

      if (GL.IsEnabled(EnableCap.Lighting))
      {
        // Normal Array Buffer
        if (staticMesh.NormalBufferHandle != 0)
        {
          // Set up normals
          GL.BindBuffer(BufferTarget.ArrayBuffer, staticMesh.NormalBufferHandle);
          GL.NormalPointer(NormalPointerType.Float, 0, IntPtr.Zero);
          GL.EnableClientState(ArrayCap.NormalArray);
        }
      }
      else
      {
        // Color Array Buffer
        if (staticMesh.ColorBufferHandle != 0)
        {
          // Set up colors
          GL.BindBuffer(BufferTarget.ArrayBuffer, staticMesh.ColorBufferHandle);
          GL.ColorPointer(3, ColorPointerType.Float, 0, IntPtr.Zero);
          GL.EnableClientState(ArrayCap.ColorArray);
        }
      }

      // Texture Array Buffer
      if (GL.IsEnabled(EnableCap.Texture2D) && staticMesh.TextureCoordinateBufferHandle != 0)
      {
        // Select the texture and set up texture coordinates
        GL.BindTexture(TextureTarget.Texture2D, staticMesh.Texture.GpuHandle);
        GL.BindBuffer(BufferTarget.ArrayBuffer, staticMesh.TextureCoordinateBufferHandle);
        GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, IntPtr.Zero);
        GL.EnableClientState(ArrayCap.TextureCoordArray);
      }
      else
        // Nothing will render if this branching is reached.
        return;

      // Set up verteces
      GL.BindBuffer(BufferTarget.ArrayBuffer, staticMesh.VertexBufferHandle);
      GL.VertexPointer(3, VertexPointerType.Float, 0, IntPtr.Zero);
      GL.EnableClientState(ArrayCap.VertexArray);

      if (staticMesh.ElementBufferHandle != 0)
      {
        // Set up indeces
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, staticMesh.ElementBufferHandle);
        GL.IndexPointer(IndexPointerType.Int, 0, IntPtr.Zero);
        GL.EnableClientState(ArrayCap.IndexArray);
        
        // Ready to render using an index buffer
        int elements = 0;
        //GL.DrawElements(PrimitiveType.Triangles, staticMesh.VertexCount, DrawElementsType.UnsignedInt, ref elements);
        GL.DrawElements(BeginMode.Triangles, staticMesh.VertexCount, DrawElementsType.UnsignedInt, ref elements);
      }
      else
        // Ready to render
        //GL.DrawArrays(PrimitiveType.Triangles, 0, staticMesh.VertexCount);
        GL.DrawArrays(BeginMode.Triangles, 0, staticMesh.VertexCount);


      GL.PopClientAttrib();
    }

    #endregion

    #region DrawStaticModel - OLD!

    ///// <summary>Renders a single static model using "GL.DrawArrays()".</summary>
    ///// <param name="camera">The camera used to capture the world (needed for the world to camera transformation).</param>
    ///// <param name="staticModel">The mesh to be rendered.</param>
    //public static void DrawStaticModel(StaticModel staticModel)
    //{
    //  SetProjectionMatrix();

    //  // Select the model view matrix to apply the world and camera transformation.
    //  GL.MatrixMode(MatrixMode.Modelview);

    //  // This line is not necessary when the camera matrix is loaded in just after.
    //  //GL.LoadIdentity();

    //  // Get and load the camera transformatino matrix.
    //  Matrix4 cameraTransform = _currentCamera.GetMatrix();
    //  GL.LoadMatrix(ref cameraTransform);

    //  //GL.Translate(-camera.Position.X, -camera.Position.Y, -camera.Position.Z);
    //  //GL.Rotate(-camera.RotationX, 1, 0, 0);
    //  //GL.Rotate(-camera.RotationY, 0, 1, 0);
    //  //GL.Rotate(-camera.RotationZ, 0, 0, 1);

    //  // Apply the world transformation due to the mesh's position, scale, and rotation
    //  GL.Translate(staticModel.Position.X, staticModel.Position.Y, staticModel.Position.Z);
    //  GL.Rotate(staticModel.RotationAngle, staticModel.RotationAmmounts.X, staticModel.RotationAmmounts.Y, staticModel.RotationAmmounts.Z);
    //  GL.Scale(staticModel.Scale.X, staticModel.Scale.Y, staticModel.Scale.Z);

    //  for (int i = 0; i < staticModel.Meshes.Count; i++)
    //  //foreach (Link<Texture, StaticMesh> link in staticModel.Meshes)
    //  {
    //    // If there is no vertex buffer, nothing will render anyway, so we can stop it now.
    //    if (staticModel.Meshes[i].Right.VertexBufferHandle == 0 ||
    //      // If there is no color or texture, nothing will render anyway
    //      (staticModel.Meshes[i].Right.ColorBufferHandle == 0 && staticModel.Meshes[i].Right.TextureCoordinateBufferHandle == 0))
    //      return;

    //    // Push current Array Buffer state so we can restore it later
    //    GL.PushClientAttrib(ClientAttribMask.ClientVertexArrayBit);

    //    if (GL.IsEnabled(EnableCap.Lighting))
    //    {
    //      // Normal Array Buffer
    //      if (staticModel.Meshes[i].Right.NormalBufferHandle != 0)
    //      {
    //        // Bind to the Array Buffer ID
    //        GL.BindBuffer(BufferTarget.ArrayBuffer, staticModel.Meshes[i].Right.NormalBufferHandle);
    //        // Set the Pointer to the current bound array describing how the data ia stored
    //        GL.NormalPointer(NormalPointerType.Float, 0, IntPtr.Zero);
    //        // Enable the client state so it will use this array buffer pointer
    //        GL.EnableClientState(ArrayCap.NormalArray);
    //      }
    //    }
    //    else
    //    {
    //      // Color Array Buffer (Colors not used when lighting is enabled)
    //      if (staticModel.Meshes[i].Right.ColorBufferHandle != 0)
    //      {
    //        // Bind to the Array Buffer ID
    //        GL.BindBuffer(BufferTarget.ArrayBuffer, staticModel.Meshes[i].Right.ColorBufferHandle);
    //        // Set the Pointer to the current bound array describing how the data ia stored
    //        GL.ColorPointer(3, ColorPointerType.Float, 0, IntPtr.Zero);
    //        // Enable the client state so it will use this array buffer pointer
    //        GL.EnableClientState(ArrayCap.ColorArray);
    //      }
    //    }

    //    // Texture Array Buffer
    //    if (GL.IsEnabled(EnableCap.Texture2D))
    //    {
    //      if (staticModel.Meshes[i].Right.TextureCoordinateBufferHandle != 0)
    //      {
    //        // Bind the texture to which the UVs are mapping to.
    //        GL.BindTexture(TextureTarget.Texture2D, staticModel.Meshes[i].Left.GpuHandle);
    //        // Bind to the Array Buffer ID
    //        GL.BindBuffer(BufferTarget.ArrayBuffer, staticModel.Meshes[i].Right.TextureCoordinateBufferHandle);
    //        // Set the Pointer to the current bound array describing how the data ia stored
    //        GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, IntPtr.Zero);
    //        // Enable the client state so it will use this array buffer pointer
    //        GL.EnableClientState(ArrayCap.TextureCoordArray);
    //      }
    //      else
    //        // Nothing will render if this branching is reached.
    //        return;
    //    }

    //    // Vertex Array Buffer
    //    // Bind to the Array Buffer ID
    //    GL.BindBuffer(BufferTarget.ArrayBuffer, staticModel.Meshes[i].Right.VertexBufferHandle);
    //    // Set the Pointer to the current bound array describing how the data ia stored
    //    GL.VertexPointer(3, VertexPointerType.Float, 0, IntPtr.Zero);
    //    // Enable the client state so it will use this array buffer pointer
    //    GL.EnableClientState(ArrayCap.VertexArray);

    //    if (staticModel.Meshes[i].Right.ElementBufferHandle != 0)
    //    {
    //      // Element Array Buffer
    //      // Bind to the Array Buffer ID
    //      GL.BindBuffer(BufferTarget.ElementArrayBuffer, staticModel.Meshes[i].Right.ElementBufferHandle);
    //      // Set the Pointer to the current bound array describing how the data ia stored
    //      GL.IndexPointer(IndexPointerType.Int, 0, IntPtr.Zero);
    //      // Enable the client state so it will use this array buffer pointer
    //      GL.EnableClientState(ArrayCap.IndexArray);
    //      // Draw the elements in the element array buffer
    //      // Draws up items in the Color, Vertex, TexCoordinate, and Normal Buffers using indices in the ElementArrayBuffer
    //      GL.DrawElements(BeginMode.Triangles, staticModel.Meshes[i].Right.VertexCount, DrawElementsType.UnsignedInt, 0);
    //    }
    //    else
    //    {
    //      // Select the vertex buffer as the active buffer (I don't think this is necessary but I haven't tested it yet).
    //      GL.BindBuffer(BufferTarget.ArrayBuffer, staticModel.Meshes[i].Right.VertexBufferHandle);
    //      // There is no index buffer, so we shoudl use "DrawArrays()" instead of "DrawIndeces()".
    //      GL.DrawArrays(BeginMode.Triangles, 0, staticModel.Meshes[i].Right.VertexCount);
    //    }

    //    GL.PopClientAttrib();
    //  }
    //}

    #endregion

    /// <summary>This is used for throwing rendering exceptions only to make debugging faster.</summary>
    private class RendererException : Exception { public RendererException(string message) : base(message) { } }
  }
}