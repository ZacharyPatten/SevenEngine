using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Engine;
using Game.States;

namespace Engine
{
  public class Game : GameWindow
  {
    PreciseTimer _timer;
    bool _fullscreen;
    FastLoop _fastLoop;
    StateManager _stateManager;
    //InputManager _inputManager;
    TextureManager _textureManager;
    StaticModelManager _staticModelManager;
    SoundManager _soundManager;
    ShaderManager _shaderManager;

    public Game() : base(800, 600, OpenTK.Graphics.GraphicsMode.Default, "Game")
    {
      Output.Print("GAME INITIALIZATION {");
      Output.IncreaseIndent();

      InitializeInput();
      InitializeDisplay();
      InitializeSounds();
      InitializeTextures();
      InitializeModels();
      InitializeShaders();
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

    private void InitializeInput()
    {
      //_inputManager = new InputManager();
    }

    private void InitializeDisplay()
    {
      Output.Print("Initializing Display {");
      Output.IncreaseIndent();

      // SET INITIAL DISPLAY SETTINGS HERE.

      _fullscreen = false;
      VSync = VSyncMode.On;
      Output.Print("Vertical Sync On;");
      GL.ClearColor(0.1f, 0.2f, 0.5f, 0.0f);
      Output.Print("Clear Color (.1, .2, .5, 0) Set;");
      GL.Enable(EnableCap.DepthTest);
      Output.Print("Depth Buffer Enabled;");
      GL.Enable(EnableCap.CullFace);
      Output.Print("Back-face Culling Enable;");

      GL.Enable(EnableCap.Lighting);
      GL.Enable(EnableCap.Light0);
      //GL.Normal3(1, 1, 1);


      float[] mat_specular = { 1.0f, 1.0f, 1.0f, 1.0f };
      float[] mat_shininess = { 50.0f };
      float[] light_position = { 1.0f, 1.0f, 1.0f, 0.0f };
      float[] light_ambient = { 0.5f, 0.5f, 0.5f, 1.0f };

      //GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
      GL.ShadeModel(ShadingModel.Smooth);

      GL.Material(MaterialFace.Front, MaterialParameter.Specular, mat_specular);
      GL.Material(MaterialFace.Front, MaterialParameter.Shininess, mat_shininess);
      GL.Light(LightName.Light0, LightParameter.Position, light_position);
      GL.Light(LightName.Light0, LightParameter.Ambient, light_ambient);
      GL.Light(LightName.Light0, LightParameter.Diffuse, mat_specular);


      SetProjectionMatrix();

      Output.DecreaseIndent();
      Output.Print("} Display Initialized;");
    }

    private void InitializeSounds()
    {
      Output.Print("Initializing Sounds {");
      Output.IncreaseIndent();

      _soundManager = new SoundManager();

      // LOAD SOUND FILES HERE.

      Output.Print("No sound effects currently loaded.");
      
      Output.DecreaseIndent();
      Output.Print("} Sounds Initialized;");
    }

    private void InitializeTextures()
    {
      Output.Print("Initializing Textures {");
      Output.IncreaseIndent();

      _textureManager = new TextureManager();

      // LOAD TEXTURES HERE USING THE TEXTURE MANAGER.

      //_textureManager.LoadTexture("toy", "img4d5a2fe0bf568.bmp");
      //_textureManager.LoadTexture("guy", "Guy.Cecil.full.150663.bmp");
      //_textureManager.LoadTexture("face", "Face001.bmp");
      //_textureManager.LoadTexture("yoda", "yoda.bmp");
      _textureManager.LoadTexture("grass", "grass.bmp");
      //_textureManager.LoadTexture("thief", "thief.bmp");
      _textureManager.LoadTexture("terrain", "Terrain.bmp");
      _textureManager.LoadTexture("RedRanger", "RedRangerBody.bmp");
      _textureManager.LoadTexture("BlueRanger", "BlueRangerBody.bmp");
      _textureManager.LoadTexture("PinkRanger", "PinkRangerBody.bmp");
      _textureManager.LoadTexture("BlackRanger", "BlackRangerBody.bmp");
      _textureManager.LoadTexture("YellowRanger", "YellowRangerBody.bmp");

      //_textureManager.LoadTexture("NightWalkerTop", @"SkyBoxes\NightWalker\NightWalkerTop.bmp");
      //_textureManager.LoadTexture("NightWalkerFront", @"SkyBoxes\NightWalker\NightWalkerFront.bmp");
      //_textureManager.LoadTexture("NightWalkerBack", @"SkyBoxes\NightWalker\NightWalkerBack.bmp");
      //_textureManager.LoadTexture("NightWalkerLeft", @"SkyBoxes\NightWalker\NightWalkerLeft.bmp");
      //_textureManager.LoadTexture("NightWalkerRight", @"SkyBoxes\NightWalker\NightWalkerRight.bmp");


      Output.DecreaseIndent();
      Output.Print("} Textures Initialized;");
    }

    private void InitializeModels()
    {
      Output.Print("Initializing Models {");
      Output.IncreaseIndent();

      _staticModelManager = new StaticModelManager();

      // LOAD MODEL FILES HERE.

      _staticModelManager.LoadModel(_textureManager, "terrain", "Terrain.obj");
      //_staticModelManager.LoadModel(_textureManager, "grass", "grass.obj");
      //_staticModelManager.LoadModel(_textureManager, "thief", "thief2.obj");
      //_staticModelManager.LoadModel(_textureManager, "yoda", "yoda.obj");
      _staticModelManager.LoadModel(_textureManager, "RedRanger", "RedRanger.obj");
      //_staticModelManager.LoadModel(_textureManager, "Ranger", "RedRanger.obj");
      //_staticModelManager.LoadModel(_textureManager, "RangerHead", "RangerHead.obj");
      //_staticModelManager.LoadModel(_textureManager, "RangerArmRight", "RangerArmRight.obj");
      //_staticModelManager.LoadModel(_textureManager, "RangerArmLeft", "RangerArmLeft.obj");
      //_staticModelManager.LoadModel(_textureManager, "RangerTorso", "RangerTorso.obj");
      //_staticModelManager.LoadModel(_textureManager, "RangerLegRight", "RangerLegRight.obj");
      //_staticModelManager.LoadModel(_textureManager, "RangerLegLeft", "RangerLegLeft.obj");

      _staticModelManager.LoadModel(_textureManager, "skyBox", @"SkyBoxes\SkyBox.obj");

      Output.DecreaseIndent();
      Output.Print("} Models Initialized;");
    }

    private void InitializeShaders()
    {
      Output.Print("Initializing Shaders {");
      Output.IncreaseIndent();

      _shaderManager = new ShaderManager();

      // LOAD SHADER FILES HERE.

      _shaderManager.AddShader();
      
      Output.Print("Basic Vertex Shader Compiled;");
      Output.Print("Basic Vertex Shader Selected;");
      Output.Print("Basic Fragment Shader Compiled;");
      Output.Print("Basic Fragment Shader Selected;");

      Output.DecreaseIndent();
      Output.Print("} Shaders Initialized;");
    }

    private void InitializeStates()
    {
      Output.Print("Initializing States {");
      Output.IncreaseIndent();
      
      _stateManager = new StateManager();

      // LOAD THE GAME STATES HERE

      _stateManager.AddState("multipleModelState", new MultipleModelState(Keyboard, _staticModelManager, _textureManager));
      //_stateManager.AddState("RedRangerTesting", new RedRangerTestingTest(Keyboard, _staticModelManager, _textureManager));
      //_stateManager.AddState("skyBoxState", new SkyBoxTesting(Keyboard, _staticModelManager, _textureManager));
      _stateManager.ChangeState("multipleModelState");

      Output.DecreaseIndent();
      Output.Print("} States Initialized;");
    }

    private void SetProjectionMatrix()
    {
      // Initialize the Projection Matrix
      GL.MatrixMode(MatrixMode.Projection);
      GL.LoadIdentity();
      double halfWidth = ClientSize.Width / 2;
      double halfHeight = ClientSize.Height / 2;

      // USE THIIS MATRIX TO ELIMINATE DEPTH PERCEPTION
      //GL.Ortho(-halfWidth, halfWidth, -halfHeight, halfHeight, -1000, 1000);

      // USE THIS MATRIX TO HAVE DEPTH PERCEPTION
      Matrix4 perspective = OpenTK.Matrix4.CreatePerspectiveFieldOfView(.5f, 
        (float)ClientSize.Width / (float)ClientSize.Height, .1f, 10000f);
      GL.LoadMatrix(ref perspective);
    }

    private void GameLoop(double elapsedTime)
    {
      UpdateInput(elapsedTime);
      _stateManager.Update(elapsedTime);
      _stateManager.Render();
    }

    private void UpdateInput(double elapsedTime)
    {
      //_inputManager.Update(elapsedTime);
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {

      if (Keyboard[OpenTK.Input.Key.Escape]) { this.Exit(); return; }
      
      base.OnUpdateFrame(e);
      _stateManager.Update(_timer.GetElapsedTime());
      // DO NOT UPDATE HERE (USE THE UPDATE METHOD WITHIN GAME STATES)
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
      base.OnRenderFrame(e);
      // Reset the Projection Matrix.
      SetProjectionMatrix();
      // Clear the color buffer
      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
      // Call the state rendering functions.
      _stateManager.Render();
      // Swap buffers is needed to show the rendering on the screen.
      SwapBuffers();
      // DO NOT RENDER ITEMS HERE. (USE THE RENDER METHODS IN GAME STATES)
    }
  }
}