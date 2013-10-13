using System.IO;

using Engine;
using Game.States;
using Engine.Imaging;

namespace Game
{
  // This is an example of how to use my engine. Read this file and the "GameState.cs" file within the "State" folder.
  // Hope you enjoy using my engine :)
  public class Game : SevenEngineWindow
  {
    public Game() : base() { }

    public override void InitializeDisplay()
    {
      // SET INITIAL DISPLAY SETTINGS HERE.
      // Use the static class "GraphicsSettingsManager"

      GraphicsSettingsManager.BackFaceCulling = true;
      GraphicsSettingsManager.DepthBuffer = true;
      GraphicsSettingsManager.VerticalSyncronization = true;
      GraphicsSettingsManager.ClearColor = Color.DarkBlue;
      GraphicsSettingsManager.Texture2D = true;
      GraphicsSettingsManager.Blend = true;
      GraphicsSettingsManager.SetAlphaBlending();
    }

    public override void InitializeSounds()
    {
      // LOAD SOUNDS HERE.
      // Use the static class "SoundManager"

      // Just keep this function here. I havn't finished the SoundManager class yet...
      Output.WriteLine("No sound effects currently loaded.");
    }

    public override void InitializeTextures()
    {
      // LOAD TEXTURES HERE.
      // Use the static class "TextureManager"

      // Note: I only support ".bmp"files at the moment. Just pull non-bitmaps into any standard image editor and export them as bitmap files.

      TextureManager.LoadTexture("grass", Directory.GetCurrentDirectory() + @"\..\..\Assets\grass.bmp");
      TextureManager.LoadTexture("RedRanger", Directory.GetCurrentDirectory() + @"\..\..\Assets\RedRangerBody.bmp");
      TextureManager.LoadTexture("BlueRanger", Directory.GetCurrentDirectory() + @"\..\..\Assets\BlueRangerBody.bmp");
      TextureManager.LoadTexture("PinkRanger", Directory.GetCurrentDirectory() + @"\..\..\Assets\PinkRangerBody.bmp");
      TextureManager.LoadTexture("BlackRanger", Directory.GetCurrentDirectory() + @"\..\..\Assets\BlackRangerBody.bmp");
      TextureManager.LoadTexture("YellowRanger", Directory.GetCurrentDirectory() + @"\..\..\Assets\YellowRangerBody.bmp");
    }

    public override void InitializeModels()
    {
      // LOAD MODEL FILES HERE.
      // Use the static class "StaticModelManager"

      // NOTE: only support obj files at the moment. ".obj" files MUST include vertex position, uvs, AND normals (my parser still needs to be more versatile)
      // Just use blender to export .obj model files with teh following settings:
      // (1) triangulated, (2) include normals, (3) include uvs, -z forward (i think), y up (i think) and all other settings off

      StaticModelManager.LoadModel("terrain", Directory.GetCurrentDirectory() + @"\..\..\Assets\Terrain.obj");
      StaticModelManager.LoadModel("RedRanger", Directory.GetCurrentDirectory() + @"\..\..\Assets\RedRanger.obj");
    }

    public override void InitializeShaders()
    {
      // LOAD SHADER FILES HERE.
      // Use the static class "ShaderManager"

      // These basic shaders do not include lighting effects.
      ShaderManager.LoadVertexShader("VertexShaderBasic", Directory.GetCurrentDirectory() + @"\..\..\Assets\Shaders\VertexShaderBasic.VertexShader");
      ShaderManager.LoadFragmentShader("FragmentShaderBasic", Directory.GetCurrentDirectory() + @"\..\..\Assets\Shaders\FragmentShaderBasic.FragmentShader");

      // These Lambertian shaders include an algorithm for lighting.
      // Warning!!! I'm still working on these shaders, but I thought I'd keep the example of how to load them.
      ShaderManager.LoadVertexShader("VertexShaderLambertian", Directory.GetCurrentDirectory() + @"\..\..\Assets\Shaders\VertexShaderLambertian.VertexShader");
      ShaderManager.LoadFragmentShader("FragmentShaderLambertian", Directory.GetCurrentDirectory() + @"\..\..\Assets\Shaders\FragmentShaderLambertian.FragmentShader");

      // LINK TOGETHER YOUR SHADERS HERE
      // Use the static class "ShaderManager"

      ShaderManager.MakeShaderProgram(
        "ShaderProgramBasic", // What to name this shader program.
        ShaderManager.GetVertexShader("VertexShaderBasic"), // The vertex shader to use.
        ShaderManager.GetFragmentShader("FragmentShaderBasic"), // The fragment shader to use.
        null, // The geometry shader to use.
        null); // The extended geometry shader to use.

      ShaderManager.MakeShaderProgram(
        "ShaderProgramLambertian", // What to name this shader program.
        ShaderManager.GetVertexShader("VertexShaderLambertian"), // The vertex shader to use.
        ShaderManager.GetFragmentShader("FragmentShaderLambertian"), // The fragment shader to use.
        null, // The geometry shader to use.
        null); // The extended geometry shader to use.

      // DONT FORGET TO SELECT YOUR SHADERS WHEN YOU WANT TO USE THEM
      // Use the static class "ShaderManager"

      ShaderManager.SetActiveShader(ShaderManager.GetShaderProgram("ShaderProgramLambertian"));

      // Just keep this function here. I havn't finished the ShaderManager class yet...
      //ShaderManager.AddShader();
    }

    public override void InitializeStates()
    {
      // LOAD THE GAME STATES HERE
      // Use the static class "StateManager"
      
      //StateManager.AddState("multipleModelState", new MultipleModelState(Keyboard));
      //StateManager.ChangeState("multipleModelState");
      StateManager.AddState("gameState", new GameState());
      StateManager.ChangeState("gameState");
    }
  }
}