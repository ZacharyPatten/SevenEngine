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

      _fastLoop = new FastLoop(GameLoop);

      Output.DecreaseIndent();
      Output.Print("} GAME INITIALIZATION COMPLETE;");
    }

    protected override void OnResize(EventArgs e)
    {
      base.OnResize(e);
      GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
      Setup2DGraphics(ClientSize.Width, ClientSize.Height);
      //Setup3DGraphics(ClientSize.Width, ClientSize.Height);
    }

    void InitializeInput()
    {
    }

    private void InitializeDisplay()
    {
      Output.Print("Initializing Display {");
      Output.IncreaseIndent();

      VSync = VSyncMode.On;
      Output.Print("Vertical Syncronization On;");
      GL.ClearColor(0.1f, 0.2f, 0.5f, 0.0f);
      Output.Print("Clear Color (.1, .2, .5, 0) Set;");
      GL.Enable(EnableCap.DepthTest);
      Output.Print("Depth Buffer Enabled;");
      Setup2DGraphics(ClientSize.Width, ClientSize.Height);
      Output.Print("2D Graphics Enabled;");
      //Setup3DGraphics(ClientSize.Width, ClientSize.Height);

      Output.DecreaseIndent();
      Output.Print("} Display Initialized;");
    }

    private void InitializeSounds()
    {
      Output.Print("Initializing Sounds {");
      Output.IncreaseIndent();

      // Load sound files here
      Output.Print("None;");

      Output.DecreaseIndent();
      Output.Print("} Sounds Initialized;");
    }

    private void InitializeTextures()
    {
      Output.Print("Initializing Textures {");
      Output.IncreaseIndent();

      // Load textures here using the texture manager.
      _textureManager.LoadTexture("toy", "img4d5a2fe0bf568.bmp");

      Output.DecreaseIndent();
      Output.Print("} Textures Initialized;");
    }

    private void InitializeModels()
    {
      Output.Print("Initializing Models {");
      Output.IncreaseIndent();

      // Load model files here.
      Output.Print("None;");

      Output.DecreaseIndent();
      Output.Print("} Models Initialized;");
    }

    private void InitializeStates()
    {
      Output.Print("Initializing States {");
      Output.IncreaseIndent();

      // Load the game states here
      _system.AddState("texture_test", new MultipleTexturesState(_textureManager));
      _system.ChangeState("texture_test");
      _system.AddState("model_state", new ModelState());
      //_system.ChangeState("model_state");

      Output.DecreaseIndent();
      Output.Print("} States Initialized;");
    }

    // NOTE: This method is for initializing settings that
    // are window size dependant.
    private void Setup2DGraphics(double width, double height)
    {
      double halfWidth = width / 2;
      double halfHeight = height / 2;
      GL.MatrixMode(MatrixMode.Projection);
      GL.LoadIdentity();
      GL.Ortho(-halfWidth, halfWidth, -halfHeight, halfHeight, -100, 100);
      GL.MatrixMode(MatrixMode.Modelview);
      GL.LoadIdentity();
    }

    // NOTE: This method is for initializing settings that
    // are window size dependant.
    private void Setup3DGraphics(double width, double height)
    {
      double halfWidth = width / 2;
      double halfHeight = height / 2;
      GL.MatrixMode(MatrixMode.Projection);
      GL.LoadIdentity();
      Matrix4 persp = Matrix4.CreatePerspectiveFieldOfView(.90f, (float)width / (float)height, 1, 100);
      GL.LoadMatrix(ref persp);
      //GL.Perspective(90, 4 / 3, 1, 1000);
      GL.MatrixMode(MatrixMode.Modelview);
      GL.LoadIdentity();
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
      // Update the game here.
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
      base.OnRenderFrame(e);
      // Call all rendering functions here.

      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

      Matrix4 modelview = Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);
      GL.MatrixMode(MatrixMode.Modelview);
      GL.LoadMatrix(ref modelview);

      GL.Color3(System.Drawing.Color.White);
      GL.Enable(EnableCap.Texture2D);

      _system.Render();

      SwapBuffers();
    }
  }
}