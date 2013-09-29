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

    public void DrawSubModel(RigidBodyPartModel subModel)
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
    }

    public void DrawStaticModel(StaticModel staticModel)
    {
      GL.MatrixMode(MatrixMode.Modelview);
      GL.LoadIdentity();
      GL.Translate(staticModel.Position);
      GL.Scale(staticModel.Scale);
      GL.Rotate(staticModel.RotationAngle, staticModel.RotationAmmounts);

      GL.BindTexture(TextureTarget.Texture2D, staticModel.Texture.Id);

      // Push current Array Buffer state so we can restore it later
      GL.PushClientAttrib(ClientAttribMask.ClientVertexArrayBit);

      if (staticModel.VertexBufferId == 0) return;
      //if (subModel.ElementBufferID == 0) return;

      if (GL.IsEnabled(EnableCap.Lighting))
      {
        // Normal Array Buffer
        if (staticModel.NormalBufferId != 0)
        {
          // Bind to the Array Buffer ID
          GL.BindBuffer(BufferTarget.ArrayBuffer, staticModel.NormalBufferId);
          // Set the Pointer to the current bound array describing how the data ia stored
          GL.NormalPointer(NormalPointerType.Float, 0, IntPtr.Zero);
          // Enable the client state so it will use this array buffer pointer
          GL.EnableClientState(ArrayCap.NormalArray);
        }
      }
      else
      {
        // Color Array Buffer (Colors not used when lighting is enabled)
        if (staticModel.ColorBufferId != 0)
        {
          // Bind to the Array Buffer ID
          GL.BindBuffer(BufferTarget.ArrayBuffer, staticModel.ColorBufferId);
          // Set the Pointer to the current bound array describing how the data ia stored
          GL.ColorPointer(3, ColorPointerType.Float, 0, IntPtr.Zero);
          // Enable the client state so it will use this array buffer pointer
          GL.EnableClientState(ArrayCap.ColorArray);
        }
      }

      // Texture Array Buffer
      if (GL.IsEnabled(EnableCap.Texture2D))
      {
        if (staticModel.TexCoordBufferId != 0)
        {
          // Bind to the Array Buffer ID
          GL.BindBuffer(BufferTarget.ArrayBuffer, staticModel.TexCoordBufferId);
          // Set the Pointer to the current bound array describing how the data ia stored
          GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, IntPtr.Zero);
          // Enable the client state so it will use this array buffer pointer
          GL.EnableClientState(ArrayCap.TextureCoordArray);
        }
      }

      // Vertex Array Buffer
      // Bind to the Array Buffer ID
      GL.BindBuffer(BufferTarget.ArrayBuffer, staticModel.VertexBufferId);
      // Set the Pointer to the current bound array describing how the data ia stored
      GL.VertexPointer(3, VertexPointerType.Float, 0, IntPtr.Zero);
      // Enable the client state so it will use this array buffer pointer
      GL.EnableClientState(ArrayCap.VertexArray);

      if (staticModel.ElementBufferId != 0)
      {
        // Element Array Buffer
        // Bind to the Array Buffer ID
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, staticModel.ElementBufferId);
        // Set the Pointer to the current bound array describing how the data ia stored
        GL.IndexPointer(IndexPointerType.Int, 0, IntPtr.Zero);
        // Enable the client state so it will use this array buffer pointer
        GL.EnableClientState(ArrayCap.IndexArray);
        // Draw the elements in the element array buffer
        // Draws up items in the Color, Vertex, TexCoordinate, and Normal Buffers using indices in the ElementArrayBuffer
        GL.DrawElements(BeginMode.Triangles, staticModel.VertexCount, DrawElementsType.UnsignedInt, 0);
      }
      else
      {
        GL.BindBuffer(BufferTarget.ArrayBuffer, staticModel.VertexBufferId);
        GL.DrawArrays(BeginMode.Triangles, 0, staticModel.VertexCount);
      }

      GL.PopClientAttrib();
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