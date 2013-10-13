using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    private static SpriteBatch _batch = new SpriteBatch();

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
    public static void DrawStaticModel(Camera camera, StaticModel staticModel)
    {
      // Select the model view matrix to apply the world and camera transformation.
      GL.MatrixMode(MatrixMode.Modelview);
      
      // This line is not necessary when the camera matrix is loaded in just after.
      //GL.LoadIdentity();
      
      // Get and load the camera transformatino matrix.
      Matrix4 cameraTransform = TransformationManager.CurrentCamera.GetMatrix();
      GL.LoadMatrix(ref cameraTransform);

      //GL.Translate(-camera.Position.X, -camera.Position.Y, -camera.Position.Z);
      //GL.Rotate(-camera.RotationX, 1, 0, 0);
      //GL.Rotate(-camera.RotationY, 0, 1, 0);
      //GL.Rotate(-camera.RotationZ, 0, 0, 1);

      // Apply the world transformation due to the mesh's position, scale, and rotation
      GL.Translate(staticModel.Position.X, staticModel.Position.Y, staticModel.Position.Z);
      GL.Scale(staticModel.Scale.X, staticModel.Scale.Y, staticModel.Scale.Z);
      GL.Rotate(staticModel.RotationAngle, staticModel.RotationAmmounts.X, staticModel.RotationAmmounts.Y, staticModel.RotationAmmounts.Z);


      foreach (Link<Texture, StaticMesh> link in staticModel.Meshes)
      {
        // If there is no vertex buffer, nothing will render anyway, so we can stop it now.
        if (link.Right.VertexBufferHandle == 0 ||
          // If there is no color or texture, nothing will render anyway
          (link.Right.ColorBufferHandle == 0 && link.Right.TextureCoordinateBufferHandle == 0))
          return;

        // Push current Array Buffer state so we can restore it later
        GL.PushClientAttrib(ClientAttribMask.ClientVertexArrayBit);

        if (GL.IsEnabled(EnableCap.Lighting))
        {
          // Normal Array Buffer
          if (link.Right.NormalBufferHandle != 0)
          {
            // Bind to the Array Buffer ID
            GL.BindBuffer(BufferTarget.ArrayBuffer, link.Right.NormalBufferHandle);
            // Set the Pointer to the current bound array describing how the data ia stored
            GL.NormalPointer(NormalPointerType.Float, 0, IntPtr.Zero);
            // Enable the client state so it will use this array buffer pointer
            GL.EnableClientState(ArrayCap.NormalArray);
          }
        }
        else
        {
          // Color Array Buffer (Colors not used when lighting is enabled)
          if (link.Right.ColorBufferHandle != 0)
          {
            // Bind to the Array Buffer ID
            GL.BindBuffer(BufferTarget.ArrayBuffer, link.Right.ColorBufferHandle);
            // Set the Pointer to the current bound array describing how the data ia stored
            GL.ColorPointer(3, ColorPointerType.Float, 0, IntPtr.Zero);
            // Enable the client state so it will use this array buffer pointer
            GL.EnableClientState(ArrayCap.ColorArray);
          }
        }

        // Texture Array Buffer
        if (GL.IsEnabled(EnableCap.Texture2D))
        {
          if (link.Right.TextureCoordinateBufferHandle != 0)
          {
            // Bind the texture to which the UVs are mapping to.
            GL.BindTexture(TextureTarget.Texture2D, link.Left.Handle);
            // Bind to the Array Buffer ID
            GL.BindBuffer(BufferTarget.ArrayBuffer, link.Right.TextureCoordinateBufferHandle);
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
        GL.BindBuffer(BufferTarget.ArrayBuffer, link.Right.VertexBufferHandle);
        // Set the Pointer to the current bound array describing how the data ia stored
        GL.VertexPointer(3, VertexPointerType.Float, 0, IntPtr.Zero);
        // Enable the client state so it will use this array buffer pointer
        GL.EnableClientState(ArrayCap.VertexArray);

        if (link.Right.ElementBufferHandle != 0)
        {
          // Element Array Buffer
          // Bind to the Array Buffer ID
          GL.BindBuffer(BufferTarget.ElementArrayBuffer, link.Right.ElementBufferHandle);
          // Set the Pointer to the current bound array describing how the data ia stored
          GL.IndexPointer(IndexPointerType.Int, 0, IntPtr.Zero);
          // Enable the client state so it will use this array buffer pointer
          GL.EnableClientState(ArrayCap.IndexArray);
          // Draw the elements in the element array buffer
          // Draws up items in the Color, Vertex, TexCoordinate, and Normal Buffers using indices in the ElementArrayBuffer
          GL.DrawElements(BeginMode.Triangles, link.Right.VertexCount, DrawElementsType.UnsignedInt, 0);
        }
        else
        {
          // Select the vertex buffer as the active buffer (I don't think this is necessary but I haven't tested it yet).
          GL.BindBuffer(BufferTarget.ArrayBuffer, link.Right.VertexBufferHandle);
          // There is no index buffer, so we shoudl use "DrawArrays()" instead of "DrawIndeces()".
          GL.DrawArrays(BeginMode.Triangles, 0, link.Right.VertexCount);
        }

        GL.PopClientAttrib();
      }
    }

    public static void Render()
    {
      _batch.Draw();
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