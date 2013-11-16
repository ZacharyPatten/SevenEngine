using System;
using System.IO;
using SevenEngine;
using SevenEngine.Imaging;
using SevenEngine.DataStructures;
using Game.States;

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
      GraphicsSettingsManager.ClearColor = Color.DEFAULT;
      GraphicsSettingsManager.Texture2D = true;
      GraphicsSettingsManager.Blend = true;
      GraphicsSettingsManager.SetAlphaBlending();
      GraphicsSettingsManager.Lighting = true;
    }

    public override void InitializeSounds()
    {
      // LOAD SOUNDS HERE.
      // Use the static class "SoundManager"

      // Just keep this function here. I haven't finished the SoundManager class yet...
      Output.WriteLine("No sound effects currently loaded.");
    }

    public override void InitializeTextures()
    {
      // LOAD TEXTURES HERE.
      // Use the static class "TextureManager"
      // Supported file types: bmp, jpeg, png, gif, ttf

      TextureManager.LoadTexture("grass",
        PathTool.GenerateCorrectRelativePath(@"\..\..\Assets\Textures\grass.bmp"));
      TextureManager.LoadTexture("rock",
        PathTool.GenerateCorrectRelativePath(@"\..\..\Assets\Textures\rock3.bmp"));
      TextureManager.LoadTexture("rock2",
        PathTool.GenerateCorrectRelativePath(@"\..\..\Assets\Textures\rock4.bmp"));
      TextureManager.LoadTexture("RedRanger",
        PathTool.GenerateCorrectRelativePath(@"\..\..\Assets\Textures\RedRangerBody.bmp"));
      TextureManager.LoadTexture("Tux",
        PathTool.GenerateCorrectRelativePath(@"\..\..\Assets\Textures\tux.bmp"));
      TextureManager.LoadTexture("BlueRanger",
        PathTool.GenerateCorrectRelativePath(@"\..\..\Assets\Textures\BlueRangerBody.bmp"));
      TextureManager.LoadTexture("PinkRanger",
        PathTool.GenerateCorrectRelativePath(@"\..\..\Assets\Textures\PinkRangerBody.bmp"));
      TextureManager.LoadTexture("BlackRanger",
        PathTool.GenerateCorrectRelativePath(@"\..\..\Assets\Textures\BlackRangerBody.bmp"));
      TextureManager.LoadTexture("YellowRanger",
        PathTool.GenerateCorrectRelativePath(@"\..\..\Assets\Textures\YellowRangerBody.bmp"));

      TextureManager.LoadTexture("MushroomCloud",
        PathTool.GenerateCorrectRelativePath(@"\..\..\Assets\Textures\MushCloud.bmp"));

      TextureManager.LoadTexture("Menu",
        PathTool.GenerateCorrectRelativePath(@"\..\..\Assets\Textures\Menu.bmp"));

      TextureManager.LoadTexture("SkyboxLeft",
        PathTool.GenerateCorrectRelativePath(@"\..\..\Assets\Textures\SkyBoxes\NightWalker\NightWalkerLeft.bmp"));
      TextureManager.LoadTexture("SkyboxRight",
        PathTool.GenerateCorrectRelativePath(@"\..\..\Assets\Textures\SkyBoxes\NightWalker\NightWalkerRight.bmp"));
      TextureManager.LoadTexture("SkyboxFront",
        PathTool.GenerateCorrectRelativePath(@"\..\..\Assets\Textures\SkyBoxes\NightWalker\NightWalkerFront.bmp"));
      TextureManager.LoadTexture("SkyboxBack",
        PathTool.GenerateCorrectRelativePath(@"\..\..\Assets\Textures\SkyBoxes\NightWalker\NightWalkerBack.bmp"));
      TextureManager.LoadTexture("SkyboxTop",
        PathTool.GenerateCorrectRelativePath(@"\..\..\Assets\Textures\SkyBoxes\NightWalker\NightWalkerTop.bmp"));

    }

    public override void InitializeFonts()
    {
      // LOAD Fonts HERE.
      // Use the static class "TextManager"
      // Supported file types: fnt
      // NOTE: the image files used by the fnt files must be supported by my image importer

      TextManager.LoadFontFile(
        // What you want to call this font
        "Calibri",
        // The path to the font file
        PathTool.GenerateCorrectRelativePath(@"\..\..\Assets\Texts\Calibri2.fnt"),
        // The folder location where the texture files for this text file are
        PathTool.GenerateCorrectRelativePath(@"\..\..\Assets\Texts\"));
    }

    public override void InitializeModels()
    {
      // LOAD MODEL FILES HERE.
      // Use the static class "StaticModelManager"
      // Supported file types: obj
      // NOTE: I only support obj file with single objects at the moment, please export each object separately
      // NOTE: I currently do not support materials

      StaticModelManager.LoadMesh("terrain",
        PathTool.GenerateCorrectRelativePath(@"\..\..\Assets\Models\Terrain.obj"));
      StaticModelManager.LoadMesh("RedRanger",
        PathTool.GenerateCorrectRelativePath(@"\..\..\Assets\Models\RedRanger.obj"));
      StaticModelManager.LoadMesh("Tux",
        PathTool.GenerateCorrectRelativePath(@"\..\..\Assets\Models\tux.obj"));
      StaticModelManager.LoadMesh("mountain",
        PathTool.GenerateCorrectRelativePath(@"\..\..\Assets\Models\mountain.obj"));
      StaticModelManager.LoadMesh("MushroomCloud",
        PathTool.GenerateCorrectRelativePath(@"\..\..\Assets\Models\MushCloud.obj"));

      StaticModelManager.LoadModel("MushroomCloud", new string[] { "MushroomCloud" }, new string[] { "MushroomCloud" }, new string[] { "MushroomCloud" });
      StaticModelManager.LoadModel("Terrain", new string[] { "grass" }, new string[] { "terrain" }, new string[] { "Terrain" });
      StaticModelManager.LoadModel("Mountain", new string[] { "rock" }, new string[] { "mountain" }, new string[] { "mountain" });
      StaticModelManager.LoadModel("Mountain2", new string[] { "rock2" }, new string[] { "mountain" }, new string[] { "mountain" });
      StaticModelManager.LoadModel("Tux", new string[] { "Tux" }, new string[] { "Tux" }, new string[] { "Body" });
      StaticModelManager.LoadModel("RedRanger", new string[] { "RedRanger" }, new string[] { "RedRanger" }, new string[] { "Body" });
      StaticModelManager.LoadModel("BlueRanger", new string[] { "BlueRanger" }, new string[] { "RedRanger" }, new string[] { "Body" });
      StaticModelManager.LoadModel("BlackRanger", new string[] { "BlackRanger" }, new string[] { "RedRanger" }, new string[] { "Body" });
      StaticModelManager.LoadModel("PinkRanger", new string[] { "PinkRanger" }, new string[] { "RedRanger" }, new string[] { "Body" });
      StaticModelManager.LoadModel("YellowRanger", new string[] { "YellowRanger" }, new string[] { "RedRanger" }, new string[] { "Body" });
      
      StaticModelManager.LoadSevenModel("RedRangerSeven",
        PathTool.GenerateCorrectRelativePath(@"\..\..\Assets\Models\RedRanger.obj7"));
    }

    public override void InitializeShaders()
    {
      // LOAD SHADER FILES HERE.
      // Use the static class "ShaderManager"
      // Supported file types: glsl
      // EXAMPLES:
        // ShaderManager.LoadVertexShader("vertexShaderName", PathTool.GenerateCorrectRelativePath(@"filePathToVertexShader"));
        // ShaderManager.LoadFragmentShader("fragmentShaderName", PathTool.GenerateCorrectRelativePath(@"filePathToFragmentShader"));
        // ShaderManager.LoadGeometryShader("geometryShaderName", PathTool.GenerateCorrectRelativePath(@"filePathToGeometryShader"));
        // ShaderManager.LoadExtendedGeometryShader("extendedGeometryShaderName", PathTool.GenerateCorrectRelativePath(@"filePathToExtendedGeometryShader"));
        // ShaderManager.MakeShaderProgram("shaderProgramName", "vertexShaderName", "fragmentShaderName", "geometryShaderName", "extendedGeometryShaderName");
        // // NOTE: PARAMETERS TO THE "MakeShaderProgram()" METHOD MAY BE "null" IF YOU AREN'T USING THOSE SHADERS
        // ShaderManager.SetActiveShader("shaderProgramName");
      
      // These basic shaders do not include lighting effects.
      ShaderManager.LoadVertexShader("VertexShaderBasic",
        PathTool.GenerateCorrectRelativePath(@"\..\..\Assets\Shaders\Vertex\VertexShaderBasic.glsl"));
      ShaderManager.LoadFragmentShader("FragmentShaderBasic",
        PathTool.GenerateCorrectRelativePath(@"\..\..\Assets\Shaders\Fragment\FragmentShaderBasic.glsl"));
      ShaderManager.MakeShaderProgram("ShaderProgramBasic", "VertexShaderBasic", "FragmentShaderBasic", null, null);

      ShaderManager.SetActiveShader("ShaderProgramBasic");
    }

    public override void InitializeStates()
    {
      // LOAD THE GAME STATES HERE
      // Use the static class "StateManager"

      StateManager.AddState(new GameState("gameState"));
      //StateManager.AddState(new SpriteState("spriteTesting"));
      StateManager.ChangeState("gameState");
    }

    public override void Update(double elapsedTime)
    {
      // DO NOT UPDATE LOW LEVEL GAME LOGIC HERE!!!
      // Only change states as need be with the static "StateManager" class.
      string stateStatus = StateManager.Update((float)elapsedTime);

      // Use the stateStatus string to determine the need for a state change.
      // I have my gameState returning "Don't Change States" at the moment.

      // An example is if the string is "menuState", you could call: 
      // StateManager.ChangeState("menuState");
    }
  }
}