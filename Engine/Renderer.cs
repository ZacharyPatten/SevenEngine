using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Engine.Textures;
using Engine.Texts;
using Engine.Models;

namespace Engine
{
  public class Renderer
  {
    SpriteBatch _batch = new SpriteBatch();

    int _currentTextureId = -1;

    public Renderer()
    {
      GL.Enable(EnableCap.Texture2D);
      GL.Enable(EnableCap.Blend);
      GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
    }

    public void DrawImmediateModeVertex(Vector position, Color color, Point uvs)
    {
      GL.Color4(color.Red, color.Green, color.Blue, color.Alpha);
      GL.TexCoord2(uvs.X, uvs.Y);
      GL.Vertex3(position.X, position.Y, position.Z);
    }

    public void DrawSprite(Sprite sprite)
    {
      if (sprite.Texture.Id == _currentTextureId)
      {
        _batch.AddSprite(sprite);
      }
      else
      {
        // Draw all with current texture
        _batch.Draw();

        // Update texture info
        _currentTextureId = sprite.Texture.Id;
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

    public void DrawSkyBox(SkyBox skyBox)
    {
      GL.MatrixMode(MatrixMode.Modelview);
      GL.LoadIdentity();
      GL.Translate(skyBox.Position.X, skyBox.Position.Y, skyBox.Position.Z);
      GL.Scale(skyBox.Scale.X, skyBox.Scale.Y, skyBox.Scale.Z);

      GL.BindTexture(TextureTarget.Texture2D, skyBox.Up.Id);
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

      GL.BindTexture(TextureTarget.Texture2D, skyBox.Back.Id);
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

      GL.BindTexture(TextureTarget.Texture2D, skyBox.Left.Id);
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

      GL.BindTexture(TextureTarget.Texture2D, skyBox.Right.Id);
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

      GL.BindTexture(TextureTarget.Texture2D, skyBox.Front.Id);
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
    
    public void DrawStaticModel(Camera camera, StaticModel staticModel)
    {
      GL.MatrixMode(MatrixMode.Modelview);
      GL.LoadIdentity();

      Matrix4 view = Matrix4.Identity;
      
      GL.Translate(-camera.Position.X, -camera.Position.Y, -camera.Position.Z);
      GL.Rotate(-camera.RotationX, 1, 0, 0);
      GL.Rotate(-camera.RotationY, 0, 1, 0);
      GL.Rotate(-camera.RotationZ, 0, 0, 1);

      GL.Translate(staticModel.Position.X, staticModel.Position.Y, staticModel.Position.Z);
      GL.Scale(staticModel.Scale.X, staticModel.Scale.Y, staticModel.Scale.Z);
      GL.Rotate(staticModel.RotationAngle, staticModel.RotationAmmounts.X, staticModel.RotationAmmounts.Y, staticModel.RotationAmmounts.Z);
      //GL.Translate(staticModel.Position.X, staticModel.Position.Y, staticModel.Position.Z);

      //GL.Translate(-camera.Position.X, -camera.Position.Y, -camera.Position.Z);

      foreach (Tuple<Texture, StaticMesh> tuple in staticModel.Meshes)
      {
        GL.BindTexture(TextureTarget.Texture2D, tuple.Item1.Id);

        // Push current Array Buffer state so we can restore it later
        GL.PushClientAttrib(ClientAttribMask.ClientVertexArrayBit);

        if (tuple.Item2.VertexBufferId == 0) return;

        if (GL.IsEnabled(EnableCap.Lighting))
        {
          // Normal Array Buffer
          if (tuple.Item2.NormalBufferId != 0)
          {
            // Bind to the Array Buffer ID
            GL.BindBuffer(BufferTarget.ArrayBuffer, tuple.Item2.NormalBufferId);
            // Set the Pointer to the current bound array describing how the data ia stored
            GL.NormalPointer(NormalPointerType.Float, 0, IntPtr.Zero);
            // Enable the client state so it will use this array buffer pointer
            GL.EnableClientState(ArrayCap.NormalArray);
          }
        }
        else
        {
          // Color Array Buffer (Colors not used when lighting is enabled)
          if (tuple.Item2.ColorBufferId != 0)
          {
            // Bind to the Array Buffer ID
            GL.BindBuffer(BufferTarget.ArrayBuffer, tuple.Item2.ColorBufferId);
            // Set the Pointer to the current bound array describing how the data ia stored
            GL.ColorPointer(3, ColorPointerType.Float, 0, IntPtr.Zero);
            // Enable the client state so it will use this array buffer pointer
            GL.EnableClientState(ArrayCap.ColorArray);
          }
        }

        // Texture Array Buffer
        if (GL.IsEnabled(EnableCap.Texture2D))
        {
          if (tuple.Item2.TextureCoordinateBufferId != 0)
          {
            // Bind to the Array Buffer ID
            GL.BindBuffer(BufferTarget.ArrayBuffer, tuple.Item2.TextureCoordinateBufferId);
            // Set the Pointer to the current bound array describing how the data ia stored
            GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, IntPtr.Zero);
            // Enable the client state so it will use this array buffer pointer
            GL.EnableClientState(ArrayCap.TextureCoordArray);
          }
        }

        // Vertex Array Buffer
        // Bind to the Array Buffer ID
        GL.BindBuffer(BufferTarget.ArrayBuffer, tuple.Item2.VertexBufferId);
        // Set the Pointer to the current bound array describing how the data ia stored
        GL.VertexPointer(3, VertexPointerType.Float, 0, IntPtr.Zero);
        // Enable the client state so it will use this array buffer pointer
        GL.EnableClientState(ArrayCap.VertexArray);

        if (tuple.Item2.ElementBufferId != 0)
        {
          // Element Array Buffer
          // Bind to the Array Buffer ID
          GL.BindBuffer(BufferTarget.ElementArrayBuffer, tuple.Item2.ElementBufferId);
          // Set the Pointer to the current bound array describing how the data ia stored
          GL.IndexPointer(IndexPointerType.Int, 0, IntPtr.Zero);
          // Enable the client state so it will use this array buffer pointer
          GL.EnableClientState(ArrayCap.IndexArray);
          // Draw the elements in the element array buffer
          // Draws up items in the Color, Vertex, TexCoordinate, and Normal Buffers using indices in the ElementArrayBuffer
          GL.DrawElements(BeginMode.Triangles, tuple.Item2.VertexCount, DrawElementsType.UnsignedInt, 0);
        }
        else
        {
          GL.BindBuffer(BufferTarget.ArrayBuffer, tuple.Item2.VertexBufferId);
          GL.DrawArrays(BeginMode.Triangles, 0, tuple.Item2.VertexCount);
        }

        GL.PopClientAttrib();
      }
    }

    public void Render()
    {
      _batch.Draw();
    }

    public void DrawText(Text text)
    {
      foreach (CharacterSprite c in text.CharacterSprites)
      {
        DrawSprite(c.Sprite);
      }
    }
  }
}