// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use under the following condition: any code using or originally taken 
// from the SevenEngine project must include citation to its original author(s) located at the
// top of each source code file, or you may include a reference to the SevenEngine project as
// a whole but you must include the current SevenEngine official website URL and logo.
// - Thanks.  :)  (support: seven@sevenengine.com)

// Author(s):
// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com
// Last Edited: 11-16-13

using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SevenEngine.Imaging;
using SevenEngine.DataStructures;

namespace SevenEngine
{
  /// <summary>TextureManager is used for image management (loading, storing, hardware instance controling, and disposing).</summary>
  public static class TextureManager
  {
    //private static Dictionary<string, Texture> _textureDatabase = new Dictionary<string, Texture>();
    public static AvlTreeLinked<Texture, string> _textureDatabase =
      new AvlTreeLinked<Texture, string>(Texture.CompareTo, Texture.CompareTo);

    /// <summary>The number of textures currently loaded onto the graphics card.</summary>
    public static int Count { get { return _textureDatabase.Count; } }

    /// <summary>Checks to see if a texture id exists.</summary>
    /// <param name="textureId">The id to check for existance.</param>
    /// <returns>True if the texture is exists, false if it does not.</returns>
    public static bool TextureExists(string textureId) { return _textureDatabase.Contains(textureId); }

    /// <summary>Pulls out a reference to a texture and increments the hardware instancing tracker.</summary>
    /// <param name="textureId">The name associated with the texture (what you caled it when you added it).</param>
    /// <returns>A reference to the desired texture.</returns>
    public static Texture Get(string textureId)
    {
      Texture texture = _textureDatabase.Get(textureId);
      texture.ExistingReferences++;
      return texture;
    }

    /// <summary>Loads a texture file ontot the GPU.</summary>
    /// <param name="textureId">The id used to look up this texture in the future.</param>
    /// <param name="path">The file path of the texture file to load.</param>
    public static void LoadTexture(string textureId, string path)
    {
      int textureIdNum;
      int width;
      int height;

      string[] pathSplit = path.Split('\\');

      // Attempt to load the file
      if (!LoadTextureFromDisk(path, out textureIdNum, out height, out width))
        Output.WriteLine("ERROR loading texture file: \"" + pathSplit[pathSplit.Length - 1] + "\".");
      else
      {
        _textureDatabase.Add(new Texture(textureId, textureIdNum, width, height));
        Output.WriteLine("Texture file loaded: \"" + pathSplit[pathSplit.Length - 1] + "\".");
      }
    }

    #region Parsers

    private static bool LoadTextureFromDisk(string path, out int handle, out int height, out int width)
    {
      try
      {
        if (String.IsNullOrEmpty(path))
          throw new ArgumentException(path);

        handle = GL.GenTexture();

        GL.BindTexture(TextureTarget.Texture2D, handle);

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
        handle = 0;
        height = 0;
        width = 0;
        return false;
      }
    }

    #endregion
  }
}