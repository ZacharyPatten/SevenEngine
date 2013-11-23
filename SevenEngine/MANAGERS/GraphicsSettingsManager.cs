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

using SevenEngine.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SevenEngine
{
  /// <summary>GraphicsSettingsManager is used for ma</summary>
  public static class GraphicsSettingsManager
  {
    private static GameWindow _sevenEngineWindow;

    private static bool _verticalSyncronization;
    private static Color _clearColor;
    private static bool _depthBuffer;
    private static bool _backFaceCulling;
    private static bool _texture2d;
    private static bool _blend;
    private static bool _lighting;

    public static void InitializeWindow(GameWindow sevenEngineWindow) { _sevenEngineWindow = sevenEngineWindow; }

    public static bool VerticalSyncronization
    { 
      get { return _verticalSyncronization; } 
      set
      {
        if (value == true)
        {
          _sevenEngineWindow.VSync = VSyncMode.On;
          _verticalSyncronization = value;
          Output.WriteLine("Vertical Syncronization enabled;");
        }
        else
        {
          _sevenEngineWindow.VSync = VSyncMode.Off;
          _verticalSyncronization = value;
          Output.WriteLine("Vertical Syncronization disabled;");
        }
      } 
    }

    public static Color ClearColor 
    { 
      get { return _clearColor; } 
      set
      {
        GL.ClearColor(value.R / 255f, value.G / 255f, value.B / 255f, 1.0f);
        _clearColor = value;
        Output.WriteLine("Clear color set to: red " + value.R + ", green " + value.G + ", blue " + value.B + ";");
      } 
    }

    public static bool DepthBuffer 
    { 
      get { return _depthBuffer; } 
      set
      {
        if (value == true)
        {
          GL.Enable(EnableCap.DepthTest);
          _depthBuffer = value;
          Output.WriteLine("Depth buffer enabled;");
        }
        else
        {
          GL.Disable(EnableCap.DepthTest);
          _depthBuffer = value;
          Output.WriteLine("Depth buffer disabled;");
        }
      } 
    }

    public static bool BackFaceCulling 
    { 
      get { return _backFaceCulling; } 
      set
      {
        if (value == true)
        {
          GL.Enable(EnableCap.CullFace);
          _backFaceCulling = value;
          Output.WriteLine("Back face culling enabled;");
        }
        else
        {
          GL.Disable(EnableCap.CullFace);
          _backFaceCulling = value;
          Output.WriteLine("Back face culling disabled;");
        }
      } 
    }

    public static bool Texture2D 
    { 
      get { return _texture2d; } 
      set
      {
        if (value == true)
        {
          GL.Enable(EnableCap.Texture2D);
          _texture2d = value;
          Output.WriteLine("2D texture enabled;");
        }
        else
        {
          GL.Disable(EnableCap.Texture2D);
          _texture2d = value;
          Output.WriteLine("2D texture disabled;");
        }
      } 
    }

    public static bool Blend 
    { 
      get { return _blend; } 
      set
      {
        if (value == true)
        {
          GL.Enable(EnableCap.Blend);
          _blend = value;
          Output.WriteLine("Alpha blanding enabled;");
        }
        else
        {
          GL.Disable(EnableCap.Blend);
          _blend = value;
          Output.WriteLine("Alpha blanding disabled;");
        }
      } 
    }

    public static bool Lighting
    {
      get { return _lighting; }
      set
      {
        if (value == true)
        {
          GL.Enable(EnableCap.Lighting);
          _lighting = value;
          Output.WriteLine("Lighting enabled;");
        }
        else
        {
          GL.Disable(EnableCap.Lighting);
          _lighting = value;
          Output.WriteLine("Lighting disabled;");
        }
      }
    }

    public static void SetAlphaBlending()
    {
      GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
    }
  }
}