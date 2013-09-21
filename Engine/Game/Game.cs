using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Engine;
using Engine.Input;

namespace Engine
{
  public class Game : GameWindow
  {
    PreciseTimer _timer;
    bool _fullscreen = false;
    //Batch _batch;
    FastLoop _fastLoop;
    StateSystem _system = new StateSystem();
    InputManager _input = new InputManager();
    TextureManager _textureManager = new TextureManager();
    SoundManager _soundManager = new SoundManager();

    public Game() : base(800, 600, OpenTK.Graphics.GraphicsMode.Default, "Game")
    {
      Output.Print("GAME INITIALIZATION {");
      Output.IncreaseIndent();

      InitializeInput();
      InitializeDisplay();
      InitializeSounds();
      InitializeTextures();
      InitializeStates();

      _timer = new PreciseTimer();
      _fastLoop = new FastLoop(GameLoop);

      Output.DecreaseIndent();
      Output.Print("} GAME INITIALIZATION COMPLETE;");
    }

    protected override void OnResize(EventArgs e)
    {
      base.OnResize(e);
      GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

      // Adjust the Projection Transformation Matrix
      GL.MatrixMode(MatrixMode.Projection);
      GL.LoadIdentity();
      double halfWidth = ClientSize.Width / 2;
      double halfHeight = ClientSize.Height / 2;
      GL.Ortho(-halfWidth, halfWidth, -halfHeight, halfHeight, -1000, 1000);

      // Return to the ModelView matrix for safety
      GL.MatrixMode(MatrixMode.Modelview);
      GL.LoadIdentity();
    }

    void InitializeInput()
    {
    }

    private void InitializeDisplay()
    {
      Output.Print("Initializing Display {");
      Output.IncreaseIndent();

      // SET INITIAL DISPLAY SETTINGS HERE.
      VSync = VSyncMode.On;
      Output.Print("Vertical Sync On;");
      GL.ClearColor(0.1f, 0.2f, 0.5f, 0.0f);
      Output.Print("Clear Color (.1, .2, .5, 0) Set;");
      GL.Enable(EnableCap.DepthTest);
      Output.Print("Depth Buffer Enabled;");
      //GL.Enable(EnableCap.CullFace);
      //Output.Print("Back-face Culling Enable;");

      // Initialize the Projection Matrix
      GL.MatrixMode(MatrixMode.Projection);
      GL.LoadIdentity();
      double halfWidth = ClientSize.Width / 2;
      double halfHeight = ClientSize.Height / 2;
      GL.Ortho(-halfWidth, halfWidth, -halfHeight, halfHeight, -1000, 1000);

      GL.MatrixMode(MatrixMode.Modelview);
      GL.LoadIdentity();

      Output.DecreaseIndent();
      Output.Print("} Display Initialized;");
    }

    private void InitializeSounds()
    {
      Output.Print("Initializing Sounds {");
      Output.IncreaseIndent();

      // LOAD SOUND FILES HERE.

      Output.DecreaseIndent();
      Output.Print("} Sounds Initialized;");
    }

    private void InitializeTextures()
    {
      Output.Print("Initializing Textures {");
      Output.IncreaseIndent();

      // LOAD TEXTURES HERE USING THE TEXTURE MANAGER.
      _textureManager.LoadTexture("toy", "img4d5a2fe0bf568.bmp");
      _textureManager.LoadTexture("guy", "Guy.Cecil.full.150663.bmp");

      Output.DecreaseIndent();
      Output.Print("} Textures Initialized;");
    }

    private void InitializeModels()
    {
      Output.Print("Initializing Models {");
      Output.IncreaseIndent();

      // LOAD MODEL FILES HERE.
      Output.Print("None;");

      Output.DecreaseIndent();
      Output.Print("} Models Initialized;");
    }

    private void InitializeStates()
    {
      Output.Print("Initializing States {");
      Output.IncreaseIndent();

      // LOAD THE GAME STATES HERE
      //_system.AddState("texture_test", new MultipleTexturesState(_textureManager));
      //_system.ChangeState("texture_test");
      //_system.AddState("model_state", new ModelState(_textureManager));
      //_system.ChangeState("model_state");
      _system.AddState("model_state2", new ModelState2(_textureManager));
      _system.ChangeState("model_state2");

      Output.DecreaseIndent();
      Output.Print("} States Initialized;");
    }

    private void GameLoop(double elapsedTime)
    {
      UpdateInput(elapsedTime);
      _system.Update(elapsedTime);
      _system.Render();
    }

    private void UpdateInput(double elapsedTime)
    {
      // Previous mouse code removed.
      _input.Update(elapsedTime);
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
      base.OnUpdateFrame(e);
      _system.Update(_timer.GetElapsedTime());
      // DO NOT UPDATE HERE (USE THE UPDATE METHOD WITHIN GAME STATES)
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
      base.OnRenderFrame(e);
      _system.Render();
      SwapBuffers();
      // DO NOT RENDER ITEMS HERE. (USE THE RENDER METHODS IN GAME STATES)
    }
  }
}