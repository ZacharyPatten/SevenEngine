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

namespace SevenEngine.Imaging
{
  /// <summary>Represents a single color by RGBA values.</summary>
  public class Color
  {
    private float _red;
    private float _green;
    private float _blue;
    private float _alpha;

    /// <summary>The ammount of red (0-255).</summary>
    public float R { get { return _red; } set { if (value < 0) _red = 0; else if (value > 255) _red = 255; else _red = value; } }
    /// <summary>The ammount of green (0-255).</summary>
    public float G { get { return _green; } set { if (value < 0) _green = 0; else if (value > 255) _green = 255; else _green = value; } }
    /// <summary>The ammount of blue (0-255).</summary>
    public float B { get { return _blue; } set { if (value < 0) _blue = 0; else if (value > 255) _blue = 255; else _blue = value; } }
    /// <summary>The alpha value (0-255).</summary>
    public float A { get { return _alpha; } set { if (value < 0) _alpha = 0; else if (value > 255) _alpha = 255; else _alpha = value; } }

    /// <summary>Creates an instance of a color using RGBA color values.</summary>
    /// <param name="red">The ammount of red (0-255).</param>
    /// <param name="green">The ammount of green (0-255).</param>
    /// <param name="blue">The ammount of blue (0-255).</param>
    /// <param name="alpha">The alpha value.</param>
    public Color(float red, float green, float blue, float alpha)
    {
      if (red < 0) _red = 0; else if (red > 255) _red = 255; else _red = red;
      if (green < 0) _green = 0; else if (green > 255) _green = 255; else _green = green;
      if (blue < 0) _blue = 0; else if (blue > 255) _blue = 255; else _blue = blue;
      if (alpha < 0) _alpha = 0; else if (alpha > 255) _alpha = 255; else _alpha = alpha;
    }

    #region Color Library
    /// <summary>Hex: 0x000000.</summary>
    public static readonly Color Black = new Color(0, 0, 0, 0);
    /// <summary>Hex: 0xffffff.</summary>
    public static readonly Color White = new Color(255, 255, 255, 0);

    /// <summary>Hex: 0x0000ff.</summary>
    public static readonly Color Blue = new Color(0, 0, 255, 0);
    /// <summary>Hex: 0x00ff00.</summary>
    public static readonly Color Green = new Color(0, 255, 0, 0);
    /// <summary>Hex: 0xff0000.</summary>
    public static readonly Color Red = new Color(255, 0, 0, 0);

    /// <summary>Hex: 0x00ffff.</summary>
    public static readonly Color Cyan = new Color(0, 255, 255, 0);
    /// <summary>Hex: 0xff00ff.</summary>
    public static readonly Color Magenta = new Color(255, 0, 255, 0);
    /// <summary>Hex: 0xffff00.</summary>
    public static readonly Color Yellow = new Color(255, 255, 0, 0);

    /// <summary>Hex: 0xf0f8ff.</summary>
    public static readonly Color AliceBlue = new Color(240, 248, 255, 0);
    /// <summary>Hex: 0xfaebd7.</summary>
    public static readonly Color AntiqueWhite = new Color(250, 235, 215, 0);
    /// <summary>Hex: 0x00ffff.</summary>
    public static readonly Color Aqua = new Color(0, 255, 255, 0);
    /// <summary>Hex: 0x7fffd4.</summary>
    public static readonly Color Aquamarine = new Color(127, 255, 212, 0);
    /// <summary>Hex: 0xf0ffff.</summary>
    public static readonly Color Azure = new Color(240, 255, 255, 0);
    /// <summary>Hex: 0xf5f5dc.</summary>
    public static readonly Color Beige = new Color(245, 245, 220, 0);
    /// <summary>Hex: 0xffe4c4.</summary>
    public static readonly Color Bisque = new Color(255, 228, 196, 0);
    /// <summary>Hex: 0xffebcd.</summary>
    public static readonly Color BlanchedAlmond = new Color(255, 235, 205, 0);
    /// <summary>Hex: 0x8a2be2.</summary>
    public static readonly Color BlueViolet = new Color(138, 43, 226, 0);
    /// <summary>Hex: 0xa52a2a.</summary>
    public static readonly Color Brown = new Color(165, 42, 42, 0);
    /// <summary>Hex: 0xdeb887.</summary>
    public static readonly Color Burlywood = new Color(222, 184, 135, 0);
    /// <summary>Hex: 0x5f9ea0.</summary>
    public static readonly Color CadetBlue = new Color(95, 158, 160, 0);
    /// <summary>Hex: 0x7fff00.</summary>
    public static readonly Color Chartreuse = new Color(127, 255, 0, 0);
    /// <summary>Hex: 0xd2691e.</summary>
    public static readonly Color Chocolate = new Color(210, 105, 30, 0);
    /// <summary>Hex: 0xff7f50.</summary>
    public static readonly Color Coral = new Color(255, 127, 80, 0);
    /// <summary>Hex: 0x6495ed.</summary>
    public static readonly Color CornflowerBlue = new Color(100, 149, 237, 0);
    /// <summary>Hex: 0xfff8dc.</summary>
    public static readonly Color Cornsilk = new Color(255, 248, 220, 0);
    /// <summary>Hex: 0x00008b.</summary>
    public static readonly Color DarkBlue = new Color(0, 0, 139, 0);
    /// <summary>Hex: 0x008b8b.</summary>
    public static readonly Color DarkCyan = new Color(0, 139, 139, 0);
    /// <summary>Hex: 0xb8860b.</summary>
    public static readonly Color DarkGoldenrod = new Color(184, 134, 11, 0);
    /// <summary>Hex: 0xa9a9a9.</summary>
    public static readonly Color DarkGray = new Color(169, 169, 169, 0);
    /// <summary>Hex: 0x006400.</summary>
    public static readonly Color DarkGreen = new Color(0, 100, 0, 0);
    /// <summary>Hex: 0xbdb76b.</summary>
    public static readonly Color DarkKhaki = new Color(189, 183, 107, 0);
    /// <summary>Hex: 0x8b008b.</summary>
    public static readonly Color DarkMagenta = new Color(139, 0, 139, 0);
    /// <summary>Hex: 0x556b2f.</summary>
    public static readonly Color DarkOliveGreen = new Color(85, 107, 47, 0);
    /// <summary>Hex: 0xff8c00.</summary>
    public static readonly Color DarkOrange = new Color(255, 140, 0, 0);
    /// <summary>Hex: 0x9932cc.</summary>
    public static readonly Color DarkOrchid = new Color(153, 50, 204, 0);
    /// <summary>Hex: 0x8b0000.</summary>
    public static readonly Color DarkRed = new Color(139, 0, 0, 0);
    /// <summary>Hex: 0xe9967a.</summary>
    public static readonly Color DarkSalmon = new Color(233, 150, 122, 0);
    /// <summary>Hex: 0x8fbc8f.</summary>
    public static readonly Color DarkSeaGreen = new Color(143, 188, 143, 0);
    /// <summary>Hex: 0x483d8b.</summary>
    public static readonly Color DarkSlateBlue = new Color(72, 61, 139, 0);
    /// <summary>Hex: 0x2f4f4f.</summary>
    public static readonly Color DarkSlateGray = new Color(47, 79, 79, 0);
    /// <summary>Hex: 0x00ced1.</summary>
    public static readonly Color DarkTurquoise = new Color(0, 206, 209, 0);
    /// <summary>Hex: 0x9400d3.</summary>
    public static readonly Color DarkViolet = new Color(148, 0, 211, 0);
    /// <summary>Hex: 0xff1493.</summary>
    public static readonly Color DeepPink = new Color(255, 20, 147, 0);
    /// <summary>Hex: 0x00bfff.</summary>
    public static readonly Color DeepSkyBlue = new Color(0, 191, 255, 0);
    /// <summary>Hex: 0x696969.</summary>
    public static readonly Color DimGray = new Color(105, 105, 105, 0);
    /// <summary>Hex: 0x1e90ff.</summary>
    public static readonly Color DodgerBlue = new Color(30, 144, 255, 0);
    /// <summary>Hex: 0xb22222.</summary>
    public static readonly Color Firebrick = new Color(178, 34, 34, 0);
    /// <summary>Hex: 0xfffaf0.</summary>
    public static readonly Color FloralWhite = new Color(255, 250, 240, 0);
    /// <summary>Hex: 0x228b22.</summary>
    public static readonly Color ForestGreen = new Color(34, 139, 34, 0);
    /// <summary>Hex: 0xff00ff.</summary>
    public static readonly Color Fuschia = new Color(255, 0, 255, 0);
    /// <summary>Hex: 0xdcdcdc.</summary>
    public static readonly Color Gainsboro = new Color(220, 220, 220, 0);
    /// <summary>Hex: 0xf8f8ff.</summary>
    public static readonly Color GhostWhite = new Color(255, 250, 250, 0);
    /// <summary>Hex: 0xffd700.</summary>
    public static readonly Color Gold = new Color(255, 215, 0, 0);
    /// <summary>Hex: 0xdaa520.</summary>
    public static readonly Color Goldenrod = new Color(218, 165, 32, 0);
    /// <summary>Hex: 0x808080.</summary>
    public static readonly Color Gray = new Color(128, 128, 128, 0);
    /// <summary>Hex: 0xadff2f.</summary>
    public static readonly Color GreenYellow = new Color(173, 255, 47, 0);
    /// <summary>Hex: 0xf0fff0.</summary>
    public static readonly Color Honeydew = new Color(240, 255, 240, 0);
    /// <summary>Hex: 0xff69b4.</summary>
    public static readonly Color HotPink = new Color(255, 105, 180, 0);
    /// <summary>Hex: 0xcd5c5c.</summary>
    public static readonly Color IndianRed = new Color(205, 92, 92, 0);
    /// <summary>Hex: 0xfffff0.</summary>
    public static readonly Color Ivory = new Color(255, 255, 240, 0);
    /// <summary>Hex: 0xf0e68c.</summary>
    public static readonly Color Khaki = new Color(240, 230, 140, 0);
    /// <summary>Hex: 0xe6e6fa.</summary>
    public static readonly Color Lavender = new Color(230, 230, 250, 0);
    /// <summary>Hex: 0xfff0f5.</summary>
    public static readonly Color LavenderBlush = new Color(255, 240, 245, 0);
    /// <summary>Hex: 0x7cfc00.</summary>
    public static readonly Color LawnGreen = new Color(124, 252, 0, 0);
    /// <summary>Hex: 0xfffacd.</summary>
    public static readonly Color LemonChiffon = new Color(255, 250, 205, 0);
    /// <summary>Hex: 0xadd8e6.</summary>
    public static readonly Color LightBlue = new Color(173, 216, 230, 0);
    /// <summary>Hex: 0xf08080.</summary>
    public static readonly Color LightCoral = new Color(240, 138, 138, 0);
    /// <summary>Hex: 0xe0ffff.</summary>
    public static readonly Color LightCyan = new Color(224, 255, 255, 0);
    /// <summary>Hex: 0xeedd82.</summary>
    public static readonly Color LightGoldenrod = new Color(238, 221, 130, 0);
    /// <summary>Hex: 0xfafad2.</summary>
    public static readonly Color LightGoldenrodYellow = new Color(250, 250, 210, 0);
    /// <summary>Hex: 0xd3d3d3.</summary>
    public static readonly Color LightGray = new Color(211, 211, 211, 0);
    /// <summary>Hex: 0x90ee90.</summary>
    public static readonly Color LightGreen = new Color(144, 238, 144, 0);
    /// <summary>Hex: 0xffb6c1.</summary>
    public static readonly Color LightPink = new Color(255, 182, 193, 0);
    /// <summary>Hex: 0xffa07a.</summary>
    public static readonly Color LightSalmon = new Color(255, 160, 122, 0);
    /// <summary>Hex: 0x20b2aa.</summary>
    public static readonly Color LightSeaGreen = new Color(32, 178, 170, 0);
    /// <summary>Hex: 0x87cefa.</summary>
    public static readonly Color LightSkyBlue = new Color(135, 206, 250, 0);
    /// <summary>Hex: 0x8470ff.</summary>
    public static readonly Color LightSlateBlue = new Color(132, 112, 255, 0);
    /// <summary>Hex: 0x778899.</summary>
    public static readonly Color LightSlateGray = new Color(119, 136, 153, 0);
    /// <summary>Hex: 0xb0c4de.</summary>
    public static readonly Color LightSteelBlue = new Color(176, 196, 222, 0);
    /// <summary>Hex: 0xffffe0.</summary>
    public static readonly Color LightYellow = new Color(255, 255, 224, 0);
    /// <summary>Hex: 0x00ff00.</summary>
    public static readonly Color Lime = new Color(0, 255, 0, 0);
    /// <summary>Hex: 0x32cd32.</summary>
    public static readonly Color LimeGreen = new Color(50, 205, 50, 0);
    /// <summary>Hex: 0xfaf0e6.</summary>
    public static readonly Color Linen = new Color(250, 240, 230, 0);
    /// <summary>Hex: 0x800000.</summary>
    public static readonly Color Maroon = new Color(128, 0, 0, 0);
    /// <summary>Hex: 0x66cdaa.</summary>
    public static readonly Color MediumAquamarine = new Color(102, 205, 170, 0);
    /// <summary>Hex: 0x0000cd.</summary>
    public static readonly Color MediumBlue = new Color(0, 0, 205, 0);
    /// <summary>Hex: 0xba55d3.</summary>
    public static readonly Color MediumOrchid = new Color(186, 85, 211, 0);
    /// <summary>Hex: 0x9370db.</summary>
    public static readonly Color MediumPurple = new Color(147, 112, 219, 0);
    /// <summary>Hex: 0x3cb371.</summary>
    public static readonly Color MediumSeaGreen = new Color(60, 179, 113, 0);
    /// <summary>Hex: 0x7b68ee.</summary>
    public static readonly Color MediumSlateBlue = new Color(123, 104, 238, 0);
    /// <summary>Hex: 0x00fa9a.</summary>
    public static readonly Color MediumSpringGreen = new Color(0, 250, 154, 0);
    /// <summary>Hex: 0x48d1cc.</summary>
    public static readonly Color MediumTurquoise = new Color(72, 209, 204, 0);
    /// <summary>Hex: 0xc71585.</summary>
    public static readonly Color MediumVioletRed = new Color(199, 21, 133, 0);
    /// <summary>Hex: 0x191970.</summary>
    public static readonly Color MidnightBlue = new Color(25, 25, 112, 0);
    /// <summary>Hex: 0xf5fffa.</summary>
    public static readonly Color MintCream = new Color(245, 255, 250, 0);
    /// <summary>Hex: 0xe1e4e1.</summary>
    public static readonly Color MistyRose = new Color(255, 228, 225, 0);
    /// <summary>Hex: 0xffe4b5.</summary>
    public static readonly Color Moccasin = new Color(255, 228, 181, 0);
    /// <summary>Hex: 0xffdead.</summary>
    public static readonly Color NavajoWhite = new Color(255, 222, 173, 0);
    /// <summary>Hex: 0x000080.</summary>
    public static readonly Color Navy = new Color(0, 0, 128, 0);
    /// <summary>Hex: 0xfdf5e6.</summary>
    public static readonly Color OldLace = new Color(253, 245, 230, 0);
    /// <summary>Hex: 0x808000.</summary>
    public static readonly Color Olive = new Color(128, 128, 0, 0);
    /// <summary>Hex: 0x6b8e23.</summary>
    public static readonly Color OliveDrab = new Color(107, 142, 35, 0);
    /// <summary>Hex: 0xffa500.</summary>
    public static readonly Color Orange = new Color(255, 165, 0, 0);
    /// <summary>Hex: 0xff4500.</summary>
    public static readonly Color OrangeRed = new Color(255, 69, 0, 0);
    /// <summary>Hex: 0xda70d6.</summary>
    public static readonly Color Orchid = new Color(218, 112, 214, 0);
    /// <summary>Hex: 0xeee8aa.</summary>
    public static readonly Color PaleGoldenrod = new Color(238, 232, 170, 0);
    /// <summary>Hex: 0x98fb98.</summary>
    public static readonly Color PaleGreen = new Color(152, 251, 152, 0);
    /// <summary>Hex: 0xafeeee.</summary>
    public static readonly Color PaleTurquoise = new Color(175, 238, 238, 0);
    /// <summary>Hex: 0xdb7093.</summary>
    public static readonly Color PaleVioletRed = new Color(219, 112, 147, 0);
    /// <summary>Hex: 0xffefd5.</summary>
    public static readonly Color PapayaWhip = new Color(255, 239, 213, 0);
    /// <summary>Hex: 0xffdab9.</summary>
    public static readonly Color PeachPuff = new Color(255, 218, 185, 0);
    /// <summary>Hex: 0xcd853f.</summary>
    public static readonly Color Peru = new Color(205, 133, 63, 0);
    /// <summary>Hex: 0xffc0cb.</summary>
    public static readonly Color Pink = new Color(255, 192, 203, 0);
    /// <summary>Hex: 0xdda0dd.</summary>
    public static readonly Color Plum = new Color(221, 160, 221, 0);
    /// <summary>Hex: 0xb0e0e6.</summary>
    public static readonly Color PowderBlue = new Color(176, 224, 230, 0);
    /// <summary>Hex: 0x800080.</summary>
    public static readonly Color Purple = new Color(128, 0, 128, 0);
    /// <summary>Hex: 0xbc8f8f.</summary>
    public static readonly Color RosyBrown = new Color(188, 143, 143, 0);
    /// <summary>Hex: 0x4169e1.</summary>
    public static readonly Color RoyalBlue = new Color(65, 105, 225, 0);
    /// <summary>Hex: 0x8b4513.</summary>
    public static readonly Color SaddleBrown = new Color(139, 69, 19, 0);
    /// <summary>Hex: 0xfa8072.</summary>
    public static readonly Color Salmon = new Color(250, 128, 114, 0);
    /// <summary>Hex: 0xf4a460.</summary>
    public static readonly Color SandyBrown = new Color(244, 164, 96, 0);
    /// <summary>Hex: 0x2e8b57.</summary>
    public static readonly Color SeaGreen = new Color(46, 139, 87, 0);
    /// <summary>Hex: 0xfff5ee.</summary>
    public static readonly Color Seashell = new Color(255, 245, 238, 0);
    /// <summary>Hex: 0xa0522d.</summary>
    public static readonly Color Sienna = new Color(160, 82, 45, 0);
    /// <summary>Hex: 0xc0c0c0.</summary>
    public static readonly Color Silver = new Color(192, 192, 192, 0);
    /// <summary>Hex: 0x87ceeb.</summary>
    public static readonly Color SkyBlue = new Color(135, 206, 235, 0);
    /// <summary>Hex: 0x6a5acd.</summary>
    public static readonly Color SlateBlue = new Color(106, 90, 205, 0);
    /// <summary>Hex: 0x708090.</summary>
    public static readonly Color SlateGray = new Color(112, 128, 144, 0);
    /// <summary>Hex: 0xfffafa.</summary>
    public static readonly Color Snow = new Color(255, 250, 250, 0);
    /// <summary>Hex: 0x00ff7f.</summary>
    public static readonly Color SpringGreen = new Color(0, 255, 127, 0);
    /// <summary>Hex: 0x4682b4.</summary>
    public static readonly Color SteelBlue = new Color(70, 130, 180, 0);
    /// <summary>Hex: 0xd2b48c.</summary>
    public static readonly Color Tan = new Color(210, 180, 140, 0);
    /// <summary>Hex: 0x008080.</summary>
    public static readonly Color Teal = new Color(0, 128, 128, 0);
    /// <summary>Hex: 0xd8bfd8.</summary>
    public static readonly Color Thistle = new Color(216, 191, 216, 0);
    /// <summary>Hex: 0xff6347.</summary>
    public static readonly Color Tomato = new Color(255, 99, 71, 0);
    /// <summary>Hex: 0x40e0d0.</summary>
    public static readonly Color Turquoise = new Color(64, 224, 208, 0);
    /// <summary>Hex: 0xee82ee.</summary>
    public static readonly Color Violet = new Color(238, 130, 238, 0);
    /// <summary>Hex: 0xd02090.</summary>
    public static readonly Color VioletRed = new Color(208, 32, 144, 0);
    /// <summary>Hex: 0xf5deb3.</summary>
    public static readonly Color Wheat = new Color(245, 222, 179, 0);
    /// <summary>Hex: 0xf5f5f5.</summary>
    public static readonly Color WhiteSmoke = new Color(245, 245, 245, 0);
    /// <summary>Hex: 0x9acd32.</summary>
    public static readonly Color YellowGreen = new Color(154, 205, 50, 0);
    #endregion

    /// <summary>YOU CAN SET A DEFAULT COLOR HERE IF YOU WANT TO!!!
    /// I personally like Teal, so it is Teal. :)</summary>
    public static readonly Color DEFAULT = Teal;
  }
}