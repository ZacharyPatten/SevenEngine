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