using System.Runtime.InteropServices;

namespace Engine
{
  [StructLayout(LayoutKind.Sequential)]
  public struct Color
  {
    public float Red { get; set; }
    public float Green { get; set; }
    public float Blue { get; set; }
    public float Alpha { get; set; }

    public Color(float r, float g, float b, float a)
      : this()
    {
      Red = r;
      Green = g;
      Blue = b;
      Alpha = a;
    }

    #region Color Library
    public static Color Black { get { return new Color(0, 0, 0, 0); } } // 0x000000
    public static Color White { get { return new Color(255, 255, 255, 0); } } // 0xffffff

    //public static Color Blue { get { return new Color(0, 0, 255, 0); } } // 0x0000ff
    //public static Color Green { get { return new Color(0, 255, 0, 0); } } // 0x00ff00
    //public static Color Red { get { return new Color(255, 0, 0, 0); } } // 0xff0000

    public static Color Cyan { get { return new Color(0, 255, 255, 0); } } // 0x00ffff
    public static Color Magenta { get { return new Color(255, 0, 255, 0); } } // 0xff00ff
    public static Color Yellow { get { return new Color(255, 255, 0, 0); } } // 0xffff00

    public static Color AliceBlue { get { return new Color(240, 248, 255, 0); } } // 0xf0f8ff
    public static Color AntiqueWhite { get { return new Color(250, 235, 215, 0); } } // 0xfaebd7
    public static Color Aqua { get { return new Color(0, 255, 255, 0); } } // 0x00ffff
    public static Color Aquamarine { get { return new Color(127, 255, 212, 0); } } // 0x7fffd4
    public static Color Azure { get { return new Color(240, 255, 255, 0); } } // 0xf0ffff
    public static Color Beige { get { return new Color(245, 245, 220, 0); } } // 0xf5f5dc
    public static Color Bisque { get { return new Color(255, 228, 196, 0); } } // 0xffe4c4
    public static Color BlanchedAlmond { get { return new Color(255, 235, 205, 0); } } // 0xffebcd
    public static Color BlueViolet { get { return new Color(138, 43, 226, 0); } } // 0x8a2be2
    public static Color Brown { get { return new Color(165, 42, 42, 0); } } // 0xa52a2a
    public static Color Burlywood { get { return new Color(222, 184, 135, 0); } } // 0xdeb887
    public static Color CadetBlue { get { return new Color(95, 158, 160, 0); } } // 0x5f9ea0
    public static Color Chartreuse { get { return new Color(127, 255, 0, 0); } } // 0x7fff00
    public static Color Chocolate { get { return new Color(210, 105, 30, 0); } } // 0xd2691e
    public static Color Coral { get { return new Color(255, 127, 80, 0); } } // 0xff7f50
    public static Color CornflowerBlue { get { return new Color(100, 149, 237, 0); } } // 0x6495ed
    public static Color Cornsilk { get { return new Color(255, 248, 220, 0); } } // 0xfff8dc
    public static Color DarkBlue { get { return new Color(0, 0, 139, 0); } } // 0x00008b
    public static Color DarkCyan { get { return new Color(0, 139, 139, 0); } } // 0x008b8b
    public static Color DarkGoldenrod { get { return new Color(184, 134, 11, 0); } } // 0xb8860b
    public static Color DarkGray { get { return new Color(169, 169, 169, 0); } } // 0xa9a9a9
    public static Color DarkGreen { get { return new Color(0, 100, 0, 0); } } // 0x006400
    public static Color DarkKhaki { get { return new Color(189, 183, 107, 0); } } // 0xbdb76b	   
    public static Color DarkMagenta { get { return new Color(139, 0, 139, 0); } } // 0x8b008b
    public static Color DarkOliveGreen { get { return new Color(85, 107, 47, 0); } } // 0x556b2f
    public static Color DarkOrange { get { return new Color(255, 140, 0, 0); } } // 0xff8c00
    public static Color DarkOrchid { get { return new Color(153, 50, 204, 0); } } // 0x9932cc
    public static Color DarkRed { get { return new Color(139, 0, 0, 0); } } // 0x8b0000
    public static Color DarkSalmon { get { return new Color(233, 150, 122, 0); } } // 0xe9967a
    public static Color DarkSeaGreen { get { return new Color(143, 188, 143, 0); } } // 0x8fbc8f
    public static Color DarkSlateBlue { get { return new Color(72, 61, 139, 0); } } // 0x483d8b
    public static Color DarkSlateGray { get { return new Color(47, 79, 79, 0); } } // 0x2f4f4f
    public static Color DarkTurquoise { get { return new Color(0, 206, 209, 0); } } // 0x00ced1
    public static Color DarkViolet { get { return new Color(148, 0, 211, 0); } } // 0x9400d3
    public static Color DeepPink { get { return new Color(255, 20, 147, 0); } } // 0xff1493
    public static Color DeepSkyBlue { get { return new Color(0, 191, 255, 0); } } // 0x00bfff
    public static Color DimGray { get { return new Color(105, 105, 105, 0); } } // 0x696969
    public static Color DodgerBlue { get { return new Color(30, 144, 255, 0); } } // 0x1e90ff
    public static Color Firebrick { get { return new Color(178, 34, 34, 0); } } // 0xb22222
    public static Color FloralWhite { get { return new Color(255, 250, 240, 0); } } // 0xfffaf0
    public static Color ForestGreen { get { return new Color(34, 139, 34, 0); } } // 0x228b22
    public static Color Fuschia { get { return new Color(255, 0, 255, 0); } } // 0xff00ff
    public static Color Gainsboro { get { return new Color(220, 220, 220, 0); } } // 0xdcdcdc
    public static Color GhostWhite { get { return new Color(255, 250, 250, 0); } } // 0xf8f8ff
    public static Color Gold { get { return new Color(255, 215, 0, 0); } } // 0xffd700
    public static Color Goldenrod { get { return new Color(218, 165, 32, 0); } } // 0xdaa520
    public static Color Gray { get { return new Color(128, 128, 128, 0); } } // 0x808080
    public static Color GreenYellow { get { return new Color(173, 255, 47, 0); } } // 0xadff2f
    public static Color Honeydew { get { return new Color(240, 255, 240, 0); } } // 0xf0fff0
    public static Color HotPink { get { return new Color(255, 105, 180, 0); } } // 0xff69b4
    public static Color IndianRed { get { return new Color(205, 92, 92, 0); } } // 0xcd5c5c
    public static Color Ivory { get { return new Color(255, 255, 240, 0); } } // 0xfffff0
    public static Color Khaki { get { return new Color(240, 230, 140, 0); } } // 0xf0e68c
    public static Color Lavender { get { return new Color(230, 230, 250, 0); } } // 0xe6e6fa
    public static Color LavenderBlush { get { return new Color(255, 240, 245, 0); } } // 0xfff0f5
    public static Color LawnGreen { get { return new Color(124, 252, 0, 0); } } // 0x7cfc00
    public static Color LemonChiffon { get { return new Color(255, 250, 205, 0); } } // 0xfffacd
    public static Color LightBlue { get { return new Color(173, 216, 230, 0); } } // 0xadd8e6
    public static Color LightCoral { get { return new Color(240, 138, 138, 0); } } // 0xf08080
    public static Color LightCyan { get { return new Color(224, 255, 255, 0); } } // 0xe0ffff
    public static Color LightGoldenrod { get { return new Color(238, 221, 130, 0); } } // 0xeedd82
    public static Color LightGoldenrodYellow { get { return new Color(250, 250, 210, 0); } } // 0xfafad2
    public static Color LightGray { get { return new Color(211, 211, 211, 0); } } // 0xd3d3d3
    public static Color LightGreen { get { return new Color(144, 238, 144, 0); } } // 0x90ee90
    public static Color LightPink { get { return new Color(255, 182, 193, 0); } } // 0xffb6c1
    public static Color LightSalmon { get { return new Color(255, 160, 122, 0); } } // 0xffa07a
    public static Color LightSeaGreen { get { return new Color(32, 178, 170, 0); } } // 0x20b2aa
    public static Color LightSkyBlue { get { return new Color(135, 206, 250, 0); } } // 0x87cefa
    public static Color LightSlateBlue { get { return new Color(132, 112, 255, 0); } } // 0x8470ff
    public static Color LightSlateGray { get { return new Color(119, 136, 153, 0); } } // 0x778899
    public static Color LightSteelBlue { get { return new Color(176, 196, 222, 0); } } // 0xb0c4de
    public static Color LightYellow { get { return new Color(255, 255, 224, 0); } } // 0xffffe0
    public static Color Lime { get { return new Color(0, 255, 0, 0); } } // 0x00ff00
    public static Color LimeGreen { get { return new Color(50, 205, 50, 0); } } // 0x32cd32
    public static Color Linen { get { return new Color(250, 240, 230, 0); } } // 0xfaf0e6
    public static Color Maroon { get { return new Color(128, 0, 0, 0); } } // 0x800000
    public static Color MediumAquamarine { get { return new Color(102, 205, 170, 0); } } // 0x66cdaa
    public static Color MediumBlue { get { return new Color(0, 0, 205, 0); } } // 0x0000cd
    public static Color MediumOrchid { get { return new Color(186, 85, 211, 0); } } // 0xba55d3
    public static Color MediumPurple { get { return new Color(147, 112, 219, 0); } } // 0x9370db
    public static Color MediumSeaGreen { get { return new Color(60, 179, 113, 0); } } // 0x3cb371
    public static Color MediumSlateBlue { get { return new Color(123, 104, 238, 0); } } // 0x7b68ee
    public static Color MediumSpringGreen { get { return new Color(0, 250, 154, 0); } } // 0x00fa9a
    public static Color MediumTurquoise { get { return new Color(72, 209, 204, 0); } } // 0x48d1cc
    public static Color MediumVioletRed { get { return new Color(199, 21, 133, 0); } } // 0xc71585
    public static Color MidnightBlue { get { return new Color(25, 25, 112, 0); } } // 0x191970
    public static Color MintCream { get { return new Color(245, 255, 250, 0); } } // 0xf5fffa
    public static Color MistyRose { get { return new Color(255, 228, 225, 0); } } // 0xe1e4e1
    public static Color Moccasin { get { return new Color(255, 228, 181, 0); } } // 0xffe4b5
    public static Color NavajoWhite { get { return new Color(255, 222, 173, 0); } } // 0xffdead
    public static Color Navy { get { return new Color(0, 0, 128, 0); } } // 0x000080
    public static Color OldLace { get { return new Color(253, 245, 230, 0); } } // 0xfdf5e6
    public static Color Olive { get { return new Color(128, 128, 0, 0); } } // 0x808000
    public static Color OliveDrab { get { return new Color(107, 142, 35, 0); } } // 0x6b8e23
    public static Color Orange { get { return new Color(255, 165, 0, 0); } } // 0xffa500
    public static Color OrangeRed { get { return new Color(255, 69, 0, 0); } }	// 0xff4500
    public static Color Orchid { get { return new Color(218, 112, 214, 0); } } // 0xda70d6
    public static Color PaleGoldenrod { get { return new Color(238, 232, 170, 0); } } // 0xeee8aa
    public static Color PaleGreen { get { return new Color(152, 251, 152, 0); } } // 0x98fb98
    public static Color PaleTurquoise { get { return new Color(175, 238, 238, 0); } } // 0xafeeee
    public static Color PaleVioletRed { get { return new Color(219, 112, 147, 0); } } // 0xdb7093
    public static Color PapayaWhip { get { return new Color(255, 239, 213, 0); } } // 0xffefd5
    public static Color PeachPuff { get { return new Color(255, 218, 185, 0); } } // 0xffdab9
    public static Color Peru { get { return new Color(205, 133, 63, 0); } } // 0xcd853f
    public static Color Pink { get { return new Color(255, 192, 203, 0); } } // 0xffc0cb
    public static Color Plum { get { return new Color(221, 160, 221, 0); } } // 0xdda0dd
    public static Color PowderBlue { get { return new Color(176, 224, 230, 0); } } // 0xb0e0e6
    public static Color Purple { get { return new Color(128, 0, 128, 0); } } // 0x800080
    public static Color RosyBrown { get { return new Color(188, 143, 143, 0); } } // 0xbc8f8f
    public static Color RoyalBlue { get { return new Color(65, 105, 225, 0); } } // 0x4169e1
    public static Color SaddleBrown { get { return new Color(139, 69, 19, 0); } } // 0x8b4513
    public static Color Salmon { get { return new Color(250, 128, 114, 0); } } // 0xfa8072
    public static Color SandyBrown { get { return new Color(244, 164, 96, 0); } } // 0xf4a460
    public static Color SeaGreen { get { return new Color(46, 139, 87, 0); } } // 0x2e8b57
    public static Color Seashell { get { return new Color(255, 245, 238, 0); } } // 0xfff5ee
    public static Color Sienna { get { return new Color(160, 82, 45, 0); } } // 0xa0522d
    public static Color Silver { get { return new Color(192, 192, 192, 0); } } // 0xc0c0c0
    public static Color SkyBlue { get { return new Color(135, 206, 235, 0); } } // 0x87ceeb
    public static Color SlateBlue { get { return new Color(106, 90, 205, 0); } } // 0x6a5acd
    public static Color SlateGray { get { return new Color(112, 128, 144, 0); } } // 0x708090
    public static Color Snow { get { return new Color(255, 250, 250, 0); } } // 0xfffafa
    public static Color SpringGreen { get { return new Color(0, 255, 127, 0); } } // 0x00ff7f
    public static Color SteelBlue { get { return new Color(70, 130, 180, 0); } } // 0x4682b4
    public static Color Tan { get { return new Color(210, 180, 140, 0); } } // 0xd2b48c
    public static Color Teal { get { return new Color(0, 128, 128, 0); } } // 0x008080
    public static Color Thistle { get { return new Color(216, 191, 216, 0); } } // 0xd8bfd8
    public static Color Tomato { get { return new Color(255, 99, 71, 0); } } // 0xff6347
    public static Color Turquoise { get { return new Color(64, 224, 208, 0); } } // 0x40e0d0
    public static Color Violet { get { return new Color(238, 130, 238, 0); } } // 0xee82ee
    public static Color VioletRed { get { return new Color(208, 32, 144, 0); } } // 0xd02090
    public static Color Wheat { get { return new Color(245, 222, 179, 0); } } // 0xf5deb3
    public static Color WhiteSmoke { get { return new Color(245, 245, 245, 0); } } // 0xf5f5f5
    public static Color YellowGreen { get { return new Color(154, 205, 50, 0); } } // 0x9acd32
    #endregion
  }
}