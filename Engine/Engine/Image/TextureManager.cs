using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;

namespace Engine
{
  public class TextureManager : IDisposable
  {
    Dictionary<string, Texture> _textureDatabase = new Dictionary<string, Texture>();

    public Texture Get(string textureId)
    {
      return _textureDatabase[textureId];
    }

    private bool LoadTextureFromDisk(string path, out int id, out int height, out int width)
    {
      try
      {
        if (String.IsNullOrEmpty(path))
          throw new ArgumentException(path);

        id = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, id);

        Bitmap bmp = new Bitmap(path);

        height = bmp.Height;
        width = bmp.Width;

        BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
          ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
          bmp_data.Width, bmp_data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
          PixelType.UnsignedByte, bmp_data.Scan0);

        bmp.UnlockBits(bmp_data);

        // We haven't uploaded mipmaps, so disable mipmapping (otherwise the texture will not appear).
        // On newer video cards, we can use GL.GenerateMipmaps() or GL.Ext.GenerateMipmaps() to create
        // mipmaps automatically. In that case, use TextureMinFilter.LinearMipmapLinear to enable them.
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        return true;
      }
      catch
      {
        id = -1;
        height = -1;
        width = -1;
        return false;
      }
    }

    public void LoadTexture(string textureId, string path)
    {
      int textureIdNum;
      int width;
      int height;

      if (!LoadTextureFromDisk(path, out textureIdNum, out height, out width))
      {
        Console.WriteLine("Could not open file, [" + path + "] or file was not a supported format (Bitmap).");
      }
      GL.BindTexture(TextureTarget.Texture2D, textureIdNum);
      System.Diagnostics.Debug.Assert(textureIdNum != 0);
      _textureDatabase.Add(textureId, new Texture(textureIdNum, width, height));
      Output.Print("\"" + path + "\" texture loaded;");
    }

    #region IDisposable Members

    public void Dispose()
    {
      foreach (Texture t in _textureDatabase.Values)
      {
        GL.DeleteTextures(1, new int[] { t.Id });
        //Gl.glDeleteTextures(1, new int[] { t.Id });
      }
    }

    #endregion

  }
}
