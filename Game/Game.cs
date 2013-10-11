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
      Output.Write("No sound effects currently loaded.");
    }

    public override void InitializeTextures()
    {
      // LOAD TEXTURES HERE.
      // Use the static class "TextureManager"

      // Note: I only support ".bmp"files at the moment. Just pull non-bitmaps into any standard image editor and export them as bitmap files.

      TextureManager.LoadTexture("grass", Directory.GetCurrentDirectory() + @"\..\..\Assets\grass.bmp");
      TextureManager.LoadTexture("terrain", Directory.GetCurrentDirectory() + @"\..\..\Assets\Terrain.bmp");
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

      // Just keep this function here. I havn't finished the ShaderManager class yet...
      ShaderManager.AddShader();
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