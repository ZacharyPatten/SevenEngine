using System.Runtime.InteropServices;

namespace Engine.Imaging
{
  [StructLayout(LayoutKind.Sequential)]
  public class Color
  {
    private double _red;
    private double _green;
    private double _blue;
    private double _alpha;

    public double Red { get { return _red; } set { _red = value; } }
    public double Green { get { return _green; } set { _green = value; } }
    public double Blue { get { return _blue; } set { _blue = value; } }
    public double Alpha { get { return _alpha; } set { _alpha = value; } }

    public Color()
    {
      _red = 0d;
      _green = 0d;
      _blue = 0d;
      _alpha = 0d;
    }

    public Color(float r, float g, float b, float a)
    {
      _red = r;
      _green = g;
      _blue = b;
      _alpha = a;
    }

    #region Color Library
    /// <summary>0x000000</summary>
    public static Color Black { get { return new Color(0, 0, 0, 0); } }
    /// <summary>0xffffff</summary>
    public static Color White { get { return new Color(255, 255, 255, 0); } }

    /// <summary>0x0000ff</summary>
    //public static Color Blue { get { return new Color(0, 0, 255, 0); } }
    /// <summary>0x00ff00</summary>
    //public static Color Green { get { return new Color(0, 255, 0, 0); } }
    /// <summary>0xff0000</summary>
    //public static Color Red { get { return new Color(255, 0, 0, 0); } }

    /// <summary>0x00ffff</summary>
    public static Color Cyan { get { return new Color(0, 255, 255, 0); } }
    /// <summary>0xff00ff</summary>
    public static Color Magenta { get { return new Color(255, 0, 255, 0); } }
    /// <summary>0xffff00</summary>
    public static Color Yellow { get { return new Color(255, 255, 0, 0); } }

    /// <summary>0xf0f8ff</summary>
    public static Color AliceBlue { get { return new Color(240, 248, 255, 0); } }
    /// <summary>0xfaebd7</summary>
    public static Color AntiqueWhite { get { return new Color(250, 235, 215, 0); } }
    /// <summary>0x00ffff</summary>
    public static Color Aqua { get { return new Color(0, 255, 255, 0); } }
    /// <summary>0x7fffd4</summary>
    public static Color Aquamarine { get { return new Color(127, 255, 212, 0); } }
    /// <summary>0xf0ffff</summary>
    public static Color Azure { get { return new Color(240, 255, 255, 0); } }
    /// <summary>0xf5f5dc</summary>
    public static Color Beige { get { return new Color(245, 245, 220, 0); } }
    /// <summary>0xffe4c4</summary>
    public static Color Bisque { get { return new Color(255, 228, 196, 0); } }
    /// <summary>0xffebcd</summary>
    public static Color BlanchedAlmond { get { return new Color(255, 235, 205, 0); } }
    /// <summary>0x8a2be2</summary>
    public static Color BlueViolet { get { return new Color(138, 43, 226, 0); } }
    /// <summary>0xa52a2a</summary>
    public static Color Brown { get { return new Color(165, 42, 42, 0); } }
    /// <summary>0xdeb887</summary>
    public static Color Burlywood { get { return new Color(222, 184, 135, 0); } }
    /// <summary>0x5f9ea0</summary>
    public static Color CadetBlue { get { return new Color(95, 158, 160, 0); } }
    /// <summary>0x7fff00</summary>
    public static Color Chartreuse { get { return new Color(127, 255, 0, 0); } }
    /// <summary>0xd2691e</summary>
    public static Color Chocolate { get { return new Color(210, 105, 30, 0); } }
    /// <summary>0xff7f50</summary>
    public static Color Coral { get { return new Color(255, 127, 80, 0); } }
    /// <summary>0x6495ed</summary>
    public static Color CornflowerBlue { get { return new Color(100, 149, 237, 0); } }
    /// <summary>0xfff8dc</summary>
    public static Color Cornsilk { get { return new Color(255, 248, 220, 0); } }
    /// <summary>0x00008b</summary>
    public static Color DarkBlue { get { return new Color(0, 0, 139, 0); } }
    /// <summary>0x008b8b</summary>
    public static Color DarkCyan { get { return new Color(0, 139, 139, 0); } }
    /// <summary>0xb8860b</summary>
    public static Color DarkGoldenrod { get { return new Color(184, 134, 11, 0); } }
    /// <summary>0xa9a9a9</summary>
    public static Color DarkGray { get { return new Color(169, 169, 169, 0); } }
    /// <summary>0x006400</summary>
    public static Color DarkGreen { get { return new Color(0, 100, 0, 0); } }
    /// <summary>0xbdb76b</summary>
    public static Color DarkKhaki { get { return new Color(189, 183, 107, 0); } }
    /// <summary>0x8b008b</summary>
    public static Color DarkMagenta { get { return new Color(139, 0, 139, 0); } }
    /// <summary>0x556b2f</summary>
    public static Color DarkOliveGreen { get { return new Color(85, 107, 47, 0); } }
    /// <summary>0xff8c00</summary>
    public static Color DarkOrange { get { return new Color(255, 140, 0, 0); } }
    /// <summary>0x9932cc</summary>
    public static Color DarkOrchid { get { return new Color(153, 50, 204, 0); } }
    /// <summary>0x8b0000</summary>
    public static Color DarkRed { get { return new Color(139, 0, 0, 0); } }
    /// <summary>0xe9967a</summary>
    public static Color DarkSalmon { get { return new Color(233, 150, 122, 0); } }
    /// <summary>0x8fbc8f</summary>
    public static Color DarkSeaGreen { get { return new Color(143, 188, 143, 0); } }
    /// <summary>0x483d8b</summary>
    public static Color DarkSlateBlue { get { return new Color(72, 61, 139, 0); } }
    /// <summary>0x2f4f4f</summary>
    public static Color DarkSlateGray { get { return new Color(47, 79, 79, 0); } }
    /// <summary>0x00ced1</summary>
    public static Color DarkTurquoise { get { return new Color(0, 206, 209, 0); } }
    /// <summary>0x9400d3</summary>
    public static Color DarkViolet { get { return new Color(148, 0, 211, 0); } }
    /// <summary>0xff1493</summary>
    public static Color DeepPink { get { return new Color(255, 20, 147, 0); } }
    /// <summary>0x00bfff</summary>
    public static Color DeepSkyBlue { get { return new Color(0, 191, 255, 0); } }
    /// <summary>0x696969</summary>
    public static Color DimGray { get { return new Color(105, 105, 105, 0); } }
    /// <summary>0x1e90ff</summary>
    public static Color DodgerBlue { get { return new Color(30, 144, 255, 0); } }
    /// <summary>0xb22222</summary>
    public static Color Firebrick { get { return new Color(178, 34, 34, 0); } }
    /// <summary>0xfffaf0</summary>
    public static Color FloralWhite { get { return new Color(255, 250, 240, 0); } }
    /// <summary>0x228b22</summary>
    public static Color ForestGreen { get { return new Color(34, 139, 34, 0); } }
    /// <summary>0xff00ff</summary>
    public static Color Fuschia { get { return new Color(255, 0, 255, 0); } }
    /// <summary>0xdcdcdc</summary>
    public static Color Gainsboro { get { return new Color(220, 220, 220, 0); } }
    /// <summary>0xf8f8ff</summary>
    public static Color GhostWhite { get { return new Color(255, 250, 250, 0); } }
    /// <summary>0xffd700</summary>
    public static Color Gold { get { return new Color(255, 215, 0, 0); } }
    /// <summary>0xdaa520</summary>
    public static Color Goldenrod { get { return new Color(218, 165, 32, 0); } }
    /// <summary>0x808080</summary>
    public static Color Gray { get { return new Color(128, 128, 128, 0); } }
    /// <summary>0xadff2f</summary>
    public static Color GreenYellow { get { return new Color(173, 255, 47, 0); } }
    /// <summary>0xf0fff0</summary>
    public static Color Honeydew { get { return new Color(240, 255, 240, 0); } }
    /// <summary>0xff69b4</summary>
    public static Color HotPink { get { return new Color(255, 105, 180, 0); } }
    /// <summary>0xcd5c5c</summary>
    public static Color IndianRed { get { return new Color(205, 92, 92, 0); } }
    /// <summary>0xfffff0</summary>
    public static Color Ivory { get { return new Color(255, 255, 240, 0); } }
    /// <summary>0xf0e68c</summary>
    public static Color Khaki { get { return new Color(240, 230, 140, 0); } }
    /// <summary>0xe6e6fa</summary>
    public static Color Lavender { get { return new Color(230, 230, 250, 0); } }
    /// <summary>0xfff0f5</summary>
    public static Color LavenderBlush { get { return new Color(255, 240, 245, 0); } }
    /// <summary>0x7cfc00</summary>
    public static Color LawnGreen { get { return new Color(124, 252, 0, 0); } }
    /// <summary>0xfffacd</summary>
    public static Color LemonChiffon { get { return new Color(255, 250, 205, 0); } }
    /// <summary>0xadd8e6</summary>
    public static Color LightBlue { get { return new Color(173, 216, 230, 0); } }
    /// <summary>0xf08080</summary>
    public static Color LightCoral { get { return new Color(240, 138, 138, 0); } }
    /// <summary>0xe0ffff</summary>
    public static Color LightCyan { get { return new Color(224, 255, 255, 0); } }
    /// <summary>0xeedd82</summary>
    public static Color LightGoldenrod { get { return new Color(238, 221, 130, 0); } }
    /// <summary>0xfafad2</summary>
    public static Color LightGoldenrodYellow { get { return new Color(250, 250, 210, 0); } }
    /// <summary>0xd3d3d3</summary>
    public static Color LightGray { get { return new Color(211, 211, 211, 0); } }
    /// <summary>0x90ee90</summary>
    public static Color LightGreen { get { return new Color(144, 238, 144, 0); } }
    /// <summary>0xffb6c1</summary>
    public static Color LightPink { get { return new Color(255, 182, 193, 0); } }
    /// <summary>0xffa07a</summary>
    public static Color LightSalmon { get { return new Color(255, 160, 122, 0); } }
    /// <summary>0x20b2aa</summary>
    public static Color LightSeaGreen { get { return new Color(32, 178, 170, 0); } }
    /// <summary>0x87cefa</summary>
    public static Color LightSkyBlue { get { return new Color(135, 206, 250, 0); } }
    /// <summary>0x8470ff</summary>
    public static Color LightSlateBlue { get { return new Color(132, 112, 255, 0); } }
    /// <summary>0x778899</summary>
    public static Color LightSlateGray { get { return new Color(119, 136, 153, 0); } }
    /// <summary>0xb0c4de</summary>
    public static Color LightSteelBlue { get { return new Color(176, 196, 222, 0); } }
    /// <summary>0xffffe0</summary>
    public static Color LightYellow { get { return new Color(255, 255, 224, 0); } }
    /// <summary>0x00ff00</summary>
    public static Color Lime { get { return new Color(0, 255, 0, 0); } }
    /// <summary>0x32cd32</summary>
    public static Color LimeGreen { get { return new Color(50, 205, 50, 0); } }
    /// <summary>0xfaf0e6</summary>
    public static Color Linen { get { return new Color(250, 240, 230, 0); } }
    /// <summary>0x800000</summary>
    public static Color Maroon { get { return new Color(128, 0, 0, 0); } }
    /// <summary>0x66cdaa</summary>
    public static Color MediumAquamarine { get { return new Color(102, 205, 170, 0); } }
    /// <summary>0x0000cd</summary>
    public static Color MediumBlue { get { return new Color(0, 0, 205, 0); } }
    /// <summary>0xba55d3</summary>
    public static Color MediumOrchid { get { return new Color(186, 85, 211, 0); } }
    /// <summary>0x9370db</summary>
    public static Color MediumPurple { get { return new Color(147, 112, 219, 0); } }
    /// <summary>0x3cb371</summary>
    public static Color MediumSeaGreen { get { return new Color(60, 179, 113, 0); } }
    /// <summary>0x7b68ee</summary>
    public static Color MediumSlateBlue { get { return new Color(123, 104, 238, 0); } }
    /// <summary>0x00fa9a</summary>
    public static Color MediumSpringGreen { get { return new Color(0, 250, 154, 0); } }
    /// <summary>0x48d1cc</summary>
    public static Color MediumTurquoise { get { return new Color(72, 209, 204, 0); } }
    /// <summary>0xc71585</summary>
    public static Color MediumVioletRed { get { return new Color(199, 21, 133, 0); } }
    /// <summary>0x191970</summary>
    public static Color MidnightBlue { get { return new Color(25, 25, 112, 0); } }
    /// <summary>0xf5fffa</summary>
    public static Color MintCream { get { return new Color(245, 255, 250, 0); } }
    /// <summary>0xe1e4e1</summary>
    public static Color MistyRose { get { return new Color(255, 228, 225, 0); } }
    /// <summary>0xffe4b5</summary>
    public static Color Moccasin { get { return new Color(255, 228, 181, 0); } }
    /// <summary>0xffdead</summary>
    public static Color NavajoWhite { get { return new Color(255, 222, 173, 0); } }
    /// <summary>0x000080</summary>
    public static Color Navy { get { return new Color(0, 0, 128, 0); } }
    /// <summary>0xfdf5e6</summary>
    public static Color OldLace { get { return new Color(253, 245, 230, 0); } }
    /// <summary>0x808000</summary>
    public static Color Olive { get { return new Color(128, 128, 0, 0); } }
    /// <summary>0x6b8e23</summary>
    public static Color OliveDrab { get { return new Color(107, 142, 35, 0); } }
    /// <summary>0xffa500</summary>
    public static Color Orange { get { return new Color(255, 165, 0, 0); } }
    /// <summary>0xff4500</summary>
    public static Color OrangeRed { get { return new Color(255, 69, 0, 0); } }
    /// <summary>0xda70d6</summary>
    public static Color Orchid { get { return new Color(218, 112, 214, 0); } }
    /// <summary>0xeee8aa</summary>
    public static Color PaleGoldenrod { get { return new Color(238, 232, 170, 0); } }
    /// <summary>0x98fb98</summary>
    public static Color PaleGreen { get { return new Color(152, 251, 152, 0); } }
    /// <summary>0xafeeee</summary>
    public static Color PaleTurquoise { get { return new Color(175, 238, 238, 0); } }
    /// <summary>0xdb7093</summary>
    public static Color PaleVioletRed { get { return new Color(219, 112, 147, 0); } }
    /// <summary>0xffefd5</summary>
    public static Color PapayaWhip { get { return new Color(255, 239, 213, 0); } }
    /// <summary>0xffdab9</summary>
    public static Color PeachPuff { get { return new Color(255, 218, 185, 0); } }
    /// <summary>0xcd853f</summary>
    public static Color Peru { get { return new Color(205, 133, 63, 0); } }
    /// <summary>0xffc0cb</summary>
    public static Color Pink { get { return new Color(255, 192, 203, 0); } }
    /// <summary>0xdda0dd</summary>
    public static Color Plum { get { return new Color(221, 160, 221, 0); } }
    /// <summary>0xb0e0e6</summary>
    public static Color PowderBlue { get { return new Color(176, 224, 230, 0); } }
    /// <summary>0x800080</summary>
    public static Color Purple { get { return new Color(128, 0, 128, 0); } }
    /// <summary>0xbc8f8f</summary>
    public static Color RosyBrown { get { return new Color(188, 143, 143, 0); } }
    /// <summary>0x4169e1</summary>
    public static Color RoyalBlue { get { return new Color(65, 105, 225, 0); } }
    /// <summary>0x8b4513</summary>
    public static Color SaddleBrown { get { return new Color(139, 69, 19, 0); } }
    /// <summary>0xfa8072</summary>
    public static Color Salmon { get { return new Color(250, 128, 114, 0); } }
    /// <summary>0xf4a460</summary>
    public static Color SandyBrown { get { return new Color(244, 164, 96, 0); } }
    /// <summary>0x2e8b57</summary>
    public static Color SeaGreen { get { return new Color(46, 139, 87, 0); } }
    /// <summary>0xfff5ee</summary>
    public static Color Seashell { get { return new Color(255, 245, 238, 0); } }
    /// <summary>0xa0522d</summary>
    public static Color Sienna { get { return new Color(160, 82, 45, 0); } }
    /// <summary>0xc0c0c0</summary>
    public static Color Silver { get { return new Color(192, 192, 192, 0); } }
    /// <summary>0x87ceeb</summary>
    public static Color SkyBlue { get { return new Color(135, 206, 235, 0); } }
    /// <summary>0x6a5acd</summary>
    public static Color SlateBlue { get { return new Color(106, 90, 205, 0); } }
    /// <summary>0x708090</summary>
    public static Color SlateGray { get { return new Color(112, 128, 144, 0); } }
    /// <summary>0xfffafa</summary>
    public static Color Snow { get { return new Color(255, 250, 250, 0); } }
    /// <summary>0x00ff7f</summary>
    public static Color SpringGreen { get { return new Color(0, 255, 127, 0); } }
    /// <summary>0x4682b4</summary>
    public static Color SteelBlue { get { return new Color(70, 130, 180, 0); } }
    /// <summary>0xd2b48c</summary>
    public static Color Tan { get { return new Color(210, 180, 140, 0); } }
    /// <summary>0x008080</summary>
    public static Color Teal { get { return new Color(0, 128, 128, 0); } }
    /// <summary>0xd8bfd8</summary>
    public static Color Thistle { get { return new Color(216, 191, 216, 0); } }
    /// <summary>0xff6347</summary>
    public static Color Tomato { get { return new Color(255, 99, 71, 0); } }
    /// <summary>0x40e0d0</summary>
    public static Color Turquoise { get { return new Color(64, 224, 208, 0); } }
    /// <summary>0xee82ee</summary>
    public static Color Violet { get { return new Color(238, 130, 238, 0); } }
    /// <summary>0xd02090</summary>
    public static Color VioletRed { get { return new Color(208, 32, 144, 0); } }
    /// <summary>0xf5deb3</summary>
    public static Color Wheat { get { return new Color(245, 222, 179, 0); } }
    /// <summary>0xf5f5f5</summary>
    public static Color WhiteSmoke { get { return new Color(245, 245, 245, 0); } }
    /// <summary>0x9acd32</summary>
    public static Color YellowGreen { get { return new Color(154, 205, 50, 0); } }
    #endregion
  }
}