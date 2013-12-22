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
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SevenEngine;
using SevenEngine.Imaging;
using SevenEngine.Mathematics;

namespace SevenEngine
{
  /// <summary>INHERIT ME!</summary>
  public abstract class SevenEngineWindow : GameWindow
  {
    // This timer calculates the time between updates in SECONDS.
    protected Timer _timer;
    private bool _paused = false;

    public SevenEngineWindow(int width, int height) : base(width, height, OpenTK.Graphics.GraphicsMode.Default, "Game")
    {
      Output.WriteLine("GAME INITIALIZATION {");
      Output.IncreaseIndent();

      InitializeInput();
      BaseInitializeDisplay();
      BaseInitializeSounds();
      BaseInitializeTextures();
      BaseInitializeFonts();
      BaseInitializeModels();
      BaseInitializeShaders();
      BaseInitializeStates();

      // This is a temporary fix to changing transformation matrices
      Renderer.ScreenWidth = ClientSize.Width;
      Renderer.ScreenHeight = ClientSize.Height;

      Renderer.SetViewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

      _timer = new Timer();

      Output.DecreaseIndent();
      Output.WriteLine("} GAME INITIALIZATION COMPLETE;");
    }

    protected override void OnResize(EventArgs e)
    {
      base.OnResize(e);
      Renderer.SetViewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

      // This is a temporary fix to changing transformation matrices
      Renderer.ScreenWidth = ClientSize.Width;
      Renderer.ScreenHeight = ClientSize.Height;
    }

    /// <summary>Give the input manager a reference to the Keyboard from OpenTK.</summary>
    private void InitializeInput()
    {
      InputManager.InitializeKeyboard(Keyboard);
      InputManager.InitializeMouse(Mouse);
    }

    private void BaseInitializeDisplay()
    {
      Output.WriteLine("Initializing Display {");
      Output.IncreaseIndent();
      // This is so the GraphicsSettingsManager can change window properties
      GraphicsSettingsManager.InitializeWindow(this);
      InitializeDisplay();
      Output.DecreaseIndent();
      Output.WriteLine("} Display Initialized;");
    }
    /// <summary>OVERRIDE THIS FUNCTION!</summary>
    public virtual void InitializeDisplay() { Output.WriteLine("ERROR: you are not overriding the \"InitializeDisplay()\" during game initilization."); }

    private void BaseInitializeSounds()
    {
      Output.WriteLine("Initializing Sounds {");
      Output.IncreaseIndent();
      InitializeSounds();
      Output.DecreaseIndent();
      Output.WriteLine("} Sounds Initialized;");
    }
    /// <summary>OVERRIDE THIS FUNCTION!</summary>
    public virtual void InitializeSounds() { Output.WriteLine("ERROR: you are not overriding the \"InitializeSounds()\" during game initilization."); }

    private void BaseInitializeTextures()
    {
      Output.WriteLine("Initializing Textures {");
      Output.IncreaseIndent();
      InitializeTextures();
      Output.DecreaseIndent();
      Output.WriteLine("} Textures Initialized;");
    }
    /// <summary>OVERRIDE THIS FUNCTION!</summary>
    public virtual void InitializeTextures() { Output.WriteLine("ERROR: you are not overriding the \"InitializeTextures()\" during game initilization."); }

    private void BaseInitializeFonts()
    {
      Output.WriteLine("Initializing Fonts {");
      Output.IncreaseIndent();
      InitializeFonts();
      Output.DecreaseIndent();
      Output.WriteLine("} Fonts Initialized;");
    }
    /// <summary>OVERRIDE THIS FUNCTION!</summary>
    public virtual void InitializeFonts() { Output.WriteLine("ERROR: you are not overriding the \"InitializeFonts()\" during game initilization."); }


    private void BaseInitializeModels()
    {
      Output.WriteLine("Initializing Models {");
      Output.IncreaseIndent();
      InitializeModels();
      Output.DecreaseIndent();
      Output.WriteLine("} Models Initialized;");
    }
    /// <summary>OVERRIDE THIS FUNCTION!</summary>
    public virtual void InitializeModels() { Output.WriteLine("ERROR: you are not overriding the \"InitializeModels()\" during game initilization."); }

    private void BaseInitializeShaders()
    {
      Output.WriteLine("Initializing Shaders {");
      Output.IncreaseIndent();
      ShaderManager.SetUpBuiltInShaders();
      InitializeShaders();
      Output.DecreaseIndent();
      Output.WriteLine("} Shaders Initialized;");
    }
    /// <summary>OVERRIDE THIS FUNCTION!</summary>
    public virtual void InitializeShaders() { Output.WriteLine("ERROR: you are not overriding the \"InitializeShaders()\" during game initilization."); }

    private void BaseInitializeStates()
    {
      Output.WriteLine("Initializing States {");
      Output.IncreaseIndent();
      InitializeStates();
      Output.DecreaseIndent();
      Output.WriteLine("} States Initialized;");
    }
    /// <summary>OVERRIDE THIS FUNCTION!</summary>
    public virtual void InitializeStates() { Output.WriteLine("ERROR: you are not overriding the \"InitializeState()\" during game initilization."); }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
      base.OnUpdateFrame(e);
      // Update the state within the input manager
      InputManager.Update();
      // If "ESCAPE" is pressed then lets close the game
      if (InputManager.Keyboard.Escapepressed) { this.Exit(); return; }
      if (InputManager.Keyboard.F1pressed)
      {
        if (WindowState == WindowState.Normal) WindowState = WindowState.Fullscreen;
        else if (WindowState == WindowState.Fullscreen) WindowState = WindowState.Normal;
      }
      if (InputManager.Keyboard.F2pressed)
        _paused = !_paused;
      if (!_paused)
        Update(_timer.GetElaspedMilliseconds());
      // DO NOT UPDATE HERE (USE THE UPDATE METHOD WITHIN GAME STATES)
    }
    /// <summary>OVERRIDE THIS FUNCTION!</summary>
    public virtual void Update(double elapsedTime) { Output.WriteLine("ERROR: you are not overriding the \"Update()\" during game initilization."); }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
      if (!_paused)
      {
        base.OnRenderFrame(e);
        // Clear the color buffer and depth buffer
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        // Call the state rendering functions.
        StateManager.Render();
        // Swap buffers swaps the frame buffer for OpenGL so that the rendered frame will display.
        SwapBuffers();
        // DO NOT RENDER ITEMS HERE. (USE THE RENDER METHODS IN GAME STATES)
      }
    }
  }
}
