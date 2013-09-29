using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Engine;
using Engine.Textures;

namespace Engine
{
  class DrawSpriteState : IGameObject
  {

    TextureManager _textureManager;

    public DrawSpriteState(TextureManager textureManager)
    {
      _textureManager = textureManager;
      Texture texture = _textureManager.Get("font");
      GL.Enable(EnableCap.Texture2D);
      GL.BindTexture(TextureTarget.Texture2D, texture.Id);
      GL.Enable(EnableCap.Blend);
      GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
    }


    public void Update(double elapsedTime)
    {

    }

    public void Render()
    {
      GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
      GL.Clear(ClearBufferMask.ColorBufferBit);

      // Quad dimensions
      double height = 200;
      double width = 200;
      double halfHeight = height / 2;
      double halfWidth = width / 2;

      // Quad positions
      double x = 0;
      double y = 0;
      double z = 0;

      // Quad color
      float red = 1;
      float green = 0;
      float blue = 0;
      float alpha = 1;

      GL.Begin(BeginMode.Triangles);
      {
        GL.Color4(red, green, blue, alpha);

        GL.TexCoord2(0, 0);
        GL.Vertex3(x - halfWidth, y + halfHeight, z); // top left
        GL.TexCoord2(1, 0);
        GL.Vertex3(x + halfWidth, y + halfHeight, z); // top right
        GL.TexCoord2(0, 1);
        GL.Vertex3(x - halfWidth, y - halfHeight, z); // bottom left

        GL.TexCoord2(1, 0);
        GL.Vertex3(x + halfWidth, y + halfHeight, z); // top right
        GL.TexCoord2(1, 1);
        GL.Vertex3(x + halfWidth, y - halfHeight, z); // bottom right
        GL.TexCoord2(0, 1);
        GL.Vertex3(x - halfWidth, y - halfHeight, z); // bottom left

      }
      GL.End();
    }
  }
}