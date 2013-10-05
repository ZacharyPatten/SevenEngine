using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Engine.Textures;

namespace Engine
{
  public class TextureManager : IDisposable
  {
    Dictionary<string, Texture> _textureDatabase = new Dictionary<string, Texture>();

    /// <summary>The number of textures currently loaded onto the graphics card.</summary>
    public int Count { get { return _textureDatabase.Count; } }

    public Texture Get(string textureId)
    {
      Texture texture = _textureDatabase[textureId];
      texture.ExistingReferences++;
      return texture;
    }

    private bool LoadTextureFromDisk(string path, out int id, out int height, out int width)
    {
      try
      {
        if (String.IsNullOrEmpty(path))
          throw new ArgumentException(path);

        id = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, id);

        Bitmap bitmap = new Bitmap(path);

        height = bitmap.Height;
        width = bitmap.Width;

        BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
          ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
          bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
          PixelType.UnsignedByte, bitmapData.Scan0);

        bitmap.UnlockBits(bitmapData);

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

      // Attempt to load the file
      if (!LoadTextureFromDisk(path, out textureIdNum, out height, out width))
      {
        Console.WriteLine("Could not open texture file: \"" + path + "\".");
      }
      else
      {
        _textureDatabase.Add(textureId, new Texture(textureIdNum, width, height));
        Output.Print("Texture file loaded: \"" + path + "\".");
      }
    }

    #region IDisposable Members

    public void Dispose()
    {
      foreach (Texture t in _textureDatabase.Values)
      {
        GL.DeleteTextures(1, new int[] { t.Id });
      }
    }

    #endregion

  }
}
