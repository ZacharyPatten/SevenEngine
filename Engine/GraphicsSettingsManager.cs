using Engine.Imaging;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine
{
  public static class GraphicsSettingsManager
  {
    // This is kinda hack-y, but some graphics setting need to be changed in the "GameWindow" class.
    // Thus I either need to make a reference to every setting, or do like I'm doing at the moment and just reference the entire class.
    private static GameWindow _sevenEngineWindow;

    private static bool _verticalSyncronization;
    private static Color _clearColor;
    private static bool _depthBuffer;
    private static bool _backFaceCulling;
    private static bool _texture2d;
    private static bool _blend;

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
        GL.ClearColor((float)value.Red / 255f, (float)value.Green / 255f, (float)value.Blue / 255f, 1.0f);
        _clearColor = value;
        Output.WriteLine("Clear color set to: red " + value.Red + ", green " + value.Green + ", blue " + value.Blue + ";");
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

    public static void SetAlphaBlending()
    {
      GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
    }
  }
}