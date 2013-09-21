using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine
{
  public class Renderer
  {
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

    Batch _batch = new Batch();

    int _currentTextureId = -1;

    public void DrawSprite(Sprite sprite)
    {
      if (sprite.Texture.Id == _currentTextureId)
      {
        _batch.AddSprite(sprite);
      }
      else
      {
        _batch.Draw(); // Draw all with current texture

        // Update texture info
        _currentTextureId = sprite.Texture.Id;
        GL.BindTexture(TextureTarget.Texture2D, _currentTextureId);
        _batch.AddSprite(sprite);
      }
    }

    public void DrawSubModel(SubModel subModel)
    {
      // Push current Array Buffer state so we can restore it later
      GL.PushClientAttrib(ClientAttribMask.ClientVertexArrayBit);

      if (subModel.VertexBufferID == 0) return;
      //if (subModel.ElementBufferID == 0) return;

      /*if (GL.IsEnabled(EnableCap.Lighting))
      {
        // Normal Array Buffer
        if (subModel.NormalBufferID != 0)
        {
          // Bind to the Array Buffer ID
          GL.BindBuffer(BufferTarget.ArrayBuffer, subModel.NormalBufferID);
          // Set the Pointer to the current bound array describing how the data ia stored
          GL.NormalPointer(NormalPointerType.Float, Vector3.SizeInBytes, IntPtr.Zero);
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
          GL.ColorPointer(4, ColorPointerType.UnsignedByte, sizeof(int), IntPtr.Zero);
          // Enable the client state so it will use this array buffer pointer
          GL.EnableClientState(ArrayCap.ColorArray);
        }
      }*/

      // Texture Array Buffer
      if (GL.IsEnabled(EnableCap.Texture2D))
      {
        if (subModel.TexCoordBufferID != 0)
        {
          // Bind to the Array Buffer ID
          GL.BindBuffer(BufferTarget.ArrayBuffer, subModel.TexCoordBufferID);
          // Set the Pointer to the current bound array describing how the data ia stored
          GL.TexCoordPointer(2, TexCoordPointerType.Float, 8, IntPtr.Zero);
          // Enable the client state so it will use this array buffer pointer
          GL.EnableClientState(ArrayCap.TextureCoordArray);
        }
      }

      // Vertex Array Buffer
      // Bind to the Array Buffer ID
      GL.BindBuffer(BufferTarget.ArrayBuffer, subModel.VertexBufferID);
      // Set the Pointer to the current bound array describing how the data ia stored
      GL.VertexPointer(3, VertexPointerType.Float, Vector3.SizeInBytes, IntPtr.Zero);
      // Enable the client state so it will use this array buffer pointer
      //GL.EnableClientState(EnableCap.VertexArray);
      GL.EnableClientState(ArrayCap.VertexArray);
      // Element Array Buffer
      // Bind to the Array Buffer ID
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, subModel.ElementBufferID);
      // Draw the elements in the element array buffer
      // Draws up items in the Color, Vertex, TexCoordinate, and Normal Buffers using indices in the ElementArrayBuffer
      GL.BindBuffer(BufferTarget.ArrayBuffer, subModel.VertexBufferID);
      GL.DrawArrays(BeginMode.Triangles, 0, 18);
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