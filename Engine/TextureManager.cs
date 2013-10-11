using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Engine.Imaging;

namespace Engine
{
  public static class TextureManager
  {
    private static Dictionary<string, Texture> _textureDatabase = new Dictionary<string, Texture>();

    /// <summary>The number of textures currently loaded onto the graphics card.</summary>
    public static int Count { get { return _textureDatabase.Count; } }

    /// <summary>Pull out a reference to a texture.</summary>
    /// <param name="textureId">The name associated with the texture (what you caled it when you added it).</param>
    /// <returns>A reference to the desired texture.</returns>
    public static Texture Get(string textureId)
    {
      Texture texture = _textureDatabase[textureId];
      texture.ExistingReferences++;
      return texture;
    }

    /// <summary>Loads a ".obj" file. NOTE: check my parser for importing proterties.</summary>
    /// <param name="textureId"></param>
    /// <param name="path"></param>
    public static void LoadTexture(string textureId, string path)
    {
      int textureIdNum;
      int width;
      int height;

      // Attempt to load the file
      if (!LoadTextureFromDisk(path, out textureIdNum, out height, out width))
        Console.WriteLine("ERROR loading texture file: \"" + path + "\".");
      else
      {
        _textureDatabase.Add(textureId, new Texture(textureIdNum, width, height));
        Output.Write("Texture file loaded: \"" + path + "\".");
      }
    }

    private static bool LoadTextureFromDisk(string path, out int id, out int height, out int width)
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
  }
}
