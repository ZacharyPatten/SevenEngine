using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Engine;
using Engine.Imaging;
using Engine.Utilities;

namespace Engine
{
  public class SevenEngineWindow : GameWindow
  {
    // This timer calculates the time between updates in SECONDS.
    protected PreciseTimer _timer;

    public SevenEngineWindow() : base(800, 600, OpenTK.Graphics.GraphicsMode.Default, "Game")
    {
      Output.Write("GAME INITIALIZATION {");
      Output.IncreaseIndent();

      InitializeInput();
      BaseInitializeDisplay();
      BaseInitializeSounds();
      BaseInitializeTextures();
      BaseInitializeModels();
      BaseInitializeShaders();
      BaseInitializeStates();

      _timer = new PreciseTimer();

      Output.DecreaseIndent();
      Output.Write("} GAME INITIALIZATION COMPLETE;");
    }

    protected override void OnResize(EventArgs e)
    {
      base.OnResize(e);
      GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
    }

    /// <summary>Give the input manager a reference to the Keyboard from OpenTK.</summary>
    private void InitializeInput() { InputManager.InitializeKeyboard(Keyboard); }

    private void BaseInitializeDisplay()
    {
      Output.Write("Initializing Display {");
      Output.IncreaseIndent();
      InitializeDisplay();
      Output.DecreaseIndent();
      Output.Write("} Display Initialized;");
    }
    /// <summary>OVERRIDE THIS FUNCTION!</summary>
    public virtual void InitializeDisplay() { Output.Write("ERROR: you are not overriding the \"InitializeDisplay()\" during game initilization."); }

    private void BaseInitializeSounds()
    {
      Output.Write("Initializing Sounds {");
      Output.IncreaseIndent();
      InitializeSounds();
      Output.DecreaseIndent();
      Output.Write("} Sounds Initialized;");
    }
    /// <summary>OVERRIDE THIS FUNCTION!</summary>
    public virtual void InitializeSounds() { Output.Write("ERROR: you are not overriding the \"InitializeSounds()\" during game initilization."); }

    private void BaseInitializeTextures()
    {
      Output.Write("Initializing Textures {");
      Output.IncreaseIndent();
      InitializeTextures();
      Output.DecreaseIndent();
      Output.Write("} Textures Initialized;");
    }
    /// <summary>OVERRIDE THIS FUNCTION!</summary>
    public virtual void InitializeTextures() { Output.Write("ERROR: you are not overriding the \"InitializeTextures()\" during game initilization."); }

    private void BaseInitializeModels()
    {
      Output.Write("Initializing Models {");
      Output.IncreaseIndent();
      InitializeModels();
      Output.DecreaseIndent();
      Output.Write("} Models Initialized;");
    }
    /// <summary>OVERRIDE THIS FUNCTION!</summary>
    public virtual void InitializeModels() { Output.Write("ERROR: you are not overriding the \"InitializeModels()\" during game initilization."); }

    private void BaseInitializeShaders()
    {
      Output.Write("Initializing Shaders {");
      Output.IncreaseIndent();
      InitializeShaders();
      Output.DecreaseIndent();
      Output.Write("} Shaders Initialized;");
    }
    /// <summary>OVERRIDE THIS FUNCTION!</summary>
    public virtual void InitializeShaders() { Output.Write("ERROR: you are not overriding the \"InitializeShaders()\" during game initilization."); }

    private void BaseInitializeStates()
    {
      Output.Write("Initializing States {");
      Output.IncreaseIndent();
      InitializeStates();
      Output.DecreaseIndent();
      Output.Write("} States Initialized;");
    }
    /// <summary>OVERRIDE THIS FUNCTION!</summary>
    public virtual void InitializeStates() { Output.Write("ERROR: you are not overriding the \"InitializeState()\" during game initilization."); }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
      base.OnUpdateFrame(e);
      // Update the state within the input manager
      InputManager.Update();
      // If "ESCAPE" is pressed then lets close the game
      if (InputManager.EndProgramKey) { this.Exit(); return; }
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
