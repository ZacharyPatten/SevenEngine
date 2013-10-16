using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Engine;
//using Engine.Imaging;
using Engine.Utilities;

namespace Engine
{
  public class SevenEngineWindow : GameWindow
  {
    // This timer calculates the time between updates in SECONDS.
    protected PreciseTimer _timer;

    public SevenEngineWindow() : base(800, 600, OpenTK.Graphics.GraphicsMode.Default, "Game")
    {
      Output.WriteLine("GAME INITIALIZATION {");
      Output.IncreaseIndent();

      InitializeInput();
      BaseInitializeDisplay();
      BaseInitializeSounds();
      BaseInitializeTextures();
      BaseInitializeModels();
      BaseInitializeShaders();
      BaseInitializeStates();

      TransformationManager.ScreenWidth = this.ClientSize.Width;
      TransformationManager.ScreenHeight = this.ClientSize.Height;

      _timer = new PreciseTimer();

      Output.DecreaseIndent();
      Output.WriteLine("} GAME INITIALIZATION COMPLETE;");
    }

    protected override void OnResize(EventArgs e)
    {
      base.OnResize(e);
      GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
      TransformationManager.ScreenWidth = this.ClientSize.Width;
      TransformationManager.ScreenHeight = this.ClientSize.Height;
    }

    /// <summary>Give the input manager a reference to the Keyboard from OpenTK.</summary>
    private void InitializeInput() { InputManager.InitializeKeyboard(Keyboard); }

    private void BaseInitializeDisplay()
    {
      Output.WriteLine("Initializing Display {");
      Output.IncreaseIndent();

      // This is kinda hack-y, but I want people to be able to change the settings from my "GraphicsSettingsManager"
      // class instead of calling the OpenTK "GameWindow" code. (needed for full OpenTK abstraction)
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
      if (InputManager.Escapepressed) { this.Exit(); return; }
      if (InputManager.F1pressed)
      {
        if (this.WindowState == WindowState.Normal) this.WindowState = WindowState.Fullscreen;
        else if (this.WindowState == WindowState.Fullscreen) this.WindowState = WindowState.Normal;
      }
      StateManager.Update(_timer.GetElapsedTime());
      // DO NOT UPDATE HERE (USE THE UPDATE METHOD WITHIN GAME STATES)
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
      base.OnRenderFrame(e);
      // Clear the color buffer
      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
      // Call the state rendering functions.
      StateManager.Render();
      // Swap buffers swaps the frame buffer for OpenGL so that the rendered frame will display.
      SwapBuffers();
      // DO NOT RENDER ITEMS HERE. (USE THE RENDER METHODS IN GAME STATES)
    }
  }
}
