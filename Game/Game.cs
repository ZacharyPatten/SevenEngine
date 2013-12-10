using System;
using System.IO;
using SevenEngine;
using SevenEngine.Imaging;
using SevenEngine.DataStructures;
using SevenEngine.Shaders;
using Game.States;

using OpenTK.Graphics.OpenGL;

namespace Game
{
  // This is an example of how to use my engine. 
  // Read this file and the "GameState.cs" file within the "State" folder.
  // Hope you enjoy using my engine :)
  public class Game : SevenEngineWindow
  {
    public Game(int width, int height) : base(width, height) { }

    public override void InitializeDisplay()
    {
      // SET INITIAL DISPLAY SETTINGS HERE.
      // Use the static class "GraphicsSettingsManager"
      // EXAMPLES:
        // GraphicsSettingsManager.SettingToChange = newValue;

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
      // EXAMPLES:
        // TextureManager.LoadTexture("nameOfTexture", "filePath");
      // NOTE: If you use my static "FilePath" class the directory should be cross platform
      
      // Textures for models
      TextureManager.LoadTexture("grass", FilePath.FromRelative(@"\..\..\Assets\Textures\grass.bmp"));
      TextureManager.LoadTexture("rock", FilePath.FromRelative(@"\..\..\Assets\Textures\rock3.bmp"));
      TextureManager.LoadTexture("rock2", FilePath.FromRelative(@"\..\..\Assets\Textures\rock4.bmp"));
      TextureManager.LoadTexture("RedRanger", FilePath.FromRelative(@"\..\..\Assets\Textures\RedRangerBody.bmp"));
      TextureManager.LoadTexture("Tux", FilePath.FromRelative(@"\..\..\Assets\Textures\tux.bmp"));
      TextureManager.LoadTexture("BlueRanger", FilePath.FromRelative(@"\..\..\Assets\Textures\BlueRangerBody.bmp"));
      TextureManager.LoadTexture("PinkRanger", FilePath.FromRelative(@"\..\..\Assets\Textures\PinkRangerBody.bmp"));
      TextureManager.LoadTexture("BlackRanger", FilePath.FromRelative(@"\..\..\Assets\Textures\BlackRangerBody.bmp"));
      TextureManager.LoadTexture("YellowRanger", FilePath.FromRelative(@"\..\..\Assets\Textures\YellowRangerBody.bmp"));
      TextureManager.LoadTexture("MushroomCloud", FilePath.FromRelative(@"\..\..\Assets\Textures\MushCloud.bmp"));

      // Textures for menus
      TextureManager.LoadTexture("Menu", FilePath.FromRelative(@"\..\..\Assets\Textures\Menu.bmp"));

      // Textrues for skybox
      TextureManager.LoadTexture("SkyboxLeft", FilePath.FromRelative(@"\..\..\Assets\Textures\SkyBoxes\NightWalker\NightWalkerLeft.bmp"));
      TextureManager.LoadTexture("SkyboxRight", FilePath.FromRelative(@"\..\..\Assets\Textures\SkyBoxes\NightWalker\NightWalkerRight.bmp"));
      TextureManager.LoadTexture("SkyboxFront", FilePath.FromRelative(@"\..\..\Assets\Textures\SkyBoxes\NightWalker\NightWalkerFront.bmp"));
      TextureManager.LoadTexture("SkyboxBack", FilePath.FromRelative(@"\..\..\Assets\Textures\SkyBoxes\NightWalker\NightWalkerBack.bmp"));
      TextureManager.LoadTexture("SkyboxTop", FilePath.FromRelative(@"\..\..\Assets\Textures\SkyBoxes\NightWalker\NightWalkerTop.bmp"));
    }

    public override void InitializeFonts()
    {
      // LOAD Fonts HERE.
      // Use the static class "TextManager"
      // Supported file types: fnt
      // NOTE: the image files used by the fnt files must be supported by my image importer
      // EXAMPLES:
        // TextManager.LoadFontFile("nameOfFont", "filePathToFont", "filePathToFontTextures");
        // Renderer.Font = TextManager.Get("nameOfFont");
      // NOTE: If you use my static "FilePath" class the directory should be cross platform

      TextManager.LoadFontFile("Calibri", FilePath.FromRelative(@"\..\..\Assets\Texts\Calibri2.fnt"), FilePath.FromRelative(@"\..\..\Assets\Texts\"));
    }

    public override void InitializeModels()
    {
      // LOAD MODEL FILES HERE.
      // Use the static class "StaticModelManager"
      // Supported file types: obj
      // NOTE: I only support obj file with single objects at the moment, please export each object separately
      // NOTE: I currently do not support materials

      // I WILL BE CHANGING THESE FUNCTIONS AROUND SOON (ONCE I GET A FULLY FEATURED OBJ IMPORTER WORKING),
        // BUT HERE IS THER CURRENT TWO FUNCTIONS YOU SHOULD USE...
      // EXAMPLES:
        // StaticModelManager.LoadMesh("meshName", "filePath");
        // string[] textures; string[] meshes; string[] meshNamesRelativeToTheModel;
        // StaticModelManager.LoadModel("modelName", textures, meshes, meshNamesRelativeToTheModel);
      // NOTE: If you use my static "FilePath" class the directory should be cross platform

      // Loading the meshes
      // Meshes are parts of a static model that have the same texture. You cannot render static 
      //   meshes because they do not have transformations. Put them in a static model to render them.
      StaticModelManager.LoadMesh("terrain", FilePath.FromRelative(@"\..\..\Assets\Models\Terrain.obj"));
      StaticModelManager.LoadMesh("RedRanger", FilePath.FromRelative(@"\..\..\Assets\Models\RedRanger.obj"));
      StaticModelManager.LoadMesh("Tux", FilePath.FromRelative(@"\..\..\Assets\Models\tux.obj"));
      StaticModelManager.LoadMesh("mountain", FilePath.FromRelative(@"\..\..\Assets\Models\mountain.obj"));
      StaticModelManager.LoadMesh("MushroomCloud", FilePath.FromRelative(@"\..\..\Assets\Models\MushCloud.obj"));

      // Forming the static models out of the meshes and textures
      // Static models represent a collection of static meshes that all have the same transformational values.
      StaticModelManager.LoadModel("MushroomCloud", new string[] { "MushroomCloud" }, new string[] { "MushroomCloud" }, new string[] { "MushroomCloud" });
      StaticModelManager.LoadModel("Terrain", new string[] { "Terrain" }, new string[] { "terrain" }, new string[] { "grass" });
      StaticModelManager.LoadModel("Mountain", new string[] { "mountain" }, new string[] { "mountain" }, new string[] { "rock" });
      StaticModelManager.LoadModel("Mountain2", new string[] { "mountain" }, new string[] { "mountain" }, new string[] { "rock2" });
      StaticModelManager.LoadModel("Tux", new string[] { "Body" }, new string[] { "Tux" }, new string[] { "Tux" });
      StaticModelManager.LoadModel("RedRanger", new string[] { "Body" }, new string[] { "RedRanger" }, new string[] { "RedRanger" });
      StaticModelManager.LoadModel("BlueRanger", new string[] { "Body" }, new string[] { "RedRanger" }, new string[] { "BlueRanger" });
      StaticModelManager.LoadModel("BlackRanger", new string[] { "Body" }, new string[] { "RedRanger" }, new string[] { "BlackRanger" });
      StaticModelManager.LoadModel("PinkRanger", new string[] { "Body" }, new string[] { "RedRanger" }, new string[] { "PinkRanger" });
      StaticModelManager.LoadModel("YellowRanger", new string[] { "Body" }, new string[] { "RedRanger" }, new string[] { "YellowRanger" });
    }

    public override void InitializeShaders()
    {
      // LOAD SHADER FILES HERE.
      // Use the static class "ShaderManager"
      // Supported file types: glsl
      // EXAMPLES:
        // ShaderManager.LoadVertexShader("vertexShaderName", FilePath.FromRelative((@"filePathToVertexShader"));
        // ShaderManager.LoadFragmentShader("fragmentShaderName", FilePath.FromRelative((@"filePathToFragmentShader"));
        // ShaderManager.LoadGeometryShader("geometryShaderName", FilePath.FromRelative((@"filePathToGeometryShader"));
        // ShaderManager.LoadExtendedGeometryShader("extendedGeometryShaderName", FilePath.FromRelative((@"filePathToExtendedGeometryShader"));
        // ShaderManager.MakeShaderProgram("shaderProgramName", "vertexShaderName", "fragmentShaderName", "geometryShaderName", "extendedGeometryShaderName");
        // // NOTE: PARAMETERS TO THE "MakeShaderProgram()" METHOD MAY BE "null" IF YOU AREN'T USING THOSE SHADERS
        // ShaderManager.SetActiveShader("shaderProgramName");
      // NOTE: If you use my static "FilePath" class the directory should be cross platform

      // These basic shaders do not include lighting effects.
      ShaderManager.LoadVertexShader("VertexShaderBasic", FilePath.FromRelative(@"\..\..\Assets\Shaders\Vertex\VertexShaderBasic.glsl"));
      ShaderManager.LoadFragmentShader("FragmentShaderBasic", FilePath.FromRelative(@"\..\..\Assets\Shaders\Fragment\FragmentShaderBasic.glsl"));
      ShaderManager.MakeShaderProgram("ShaderProgramBasic", "VertexShaderBasic", "FragmentShaderBasic", null, null);

      ShaderManager.LoadVertexShader("VertexShaderLight", FilePath.FromRelative(@"\..\..\Assets\Shaders\Vertex\VertexShaderLight2.glsl"));
      ShaderManager.LoadFragmentShader("FragmentShaderLight", FilePath.FromRelative(@"\..\..\Assets\Shaders\Fragment\FragmentShaderLight2.glsl"));
      ShaderManager.MakeShaderProgram("ShaderProgramLight", "VertexShaderLight", "FragmentShaderLight", null, null);

      ShaderManager.SetActiveShader("ShaderProgramBasic");
    }

    public override void InitializeStates()
    {
      // LOAD THE GAME STATES HERE
      // Use the static class "StateManager"
      // EXAMPLES:
        // StateManager.AddState(new YourStateClass("nameOfState"));
        // StateManager.StateManager.TriggerStateLoad("nameOfState");
        // StateManager.ChangeState("nameOfState");

      StateManager.AddState(new GameState("gameState"));
      // The following line calls the "Load" function of your state.
      // The state must be loaded before you make it the current state.
      StateManager.TriggerStateLoad("gameState");
      StateManager.ChangeState("gameState");
    }

    public override void Update(double elapsedTime)
    {
      // DO NOT UPDATE LOW LEVEL GAME LOGIC HERE!!!
      // Only change states as need be with the static "StateManager" class.
      // EXAMPLES:
        // string stateStatus = StateManager.Update((float)elapsedTime);
        // if (stateStatus == "menuState")
        //  StateManager.ChangeState("menuState");

      // NOTE: DO NOT alter this function unless you fully understand it
      string stateStatus = StateManager.Update((float)elapsedTime);
    }
  }
}