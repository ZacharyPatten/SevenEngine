using System.IO;

using Engine;
using Game.States;
using Engine.Imaging;
using Engine.DataStructures;
using Engine.Shaders;

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

      TextureManager.LoadTexture("grass",
        Directory.GetCurrentDirectory() + @"\..\..\Assets\grass.bmp");
      TextureManager.LoadTexture("RedRanger",
        Directory.GetCurrentDirectory() + @"\..\..\Assets\RedRangerBody.bmp");
      TextureManager.LoadTexture("BlueRanger",
        Directory.GetCurrentDirectory() + @"\..\..\Assets\BlueRangerBody.bmp");
      TextureManager.LoadTexture("PinkRanger",
        Directory.GetCurrentDirectory() + @"\..\..\Assets\PinkRangerBody.bmp");
      TextureManager.LoadTexture("BlackRanger",
        Directory.GetCurrentDirectory() + @"\..\..\Assets\BlackRangerBody.bmp");
      TextureManager.LoadTexture("YellowRanger",
        Directory.GetCurrentDirectory() + @"\..\..\Assets\YellowRangerBody.bmp");
    }

    public override void InitializeModels()
    {
      // LOAD MODEL FILES HERE.
      // Use the static class "StaticModelManager"

      // NOTE: only support obj files at the moment. ".obj" files MUST include vertex position, uvs, AND normals (my parser still needs to be more versatile)
      // Just use blender to export .obj model files with teh following settings:
      // (1) triangulated, (2) include normals, (3) include uvs, -z forward (i think), y up (i think) and all other settings off
      // ALSO: each model you load can only have a single texture that its mapped to (export each object separately)
      //   I know these are a lot of restrictions, but I'm only one dude writing all this... ill get to it eventually

      StaticModelManager.LoadMesh("terrain",
        Directory.GetCurrentDirectory() + @"\..\..\Assets\Terrain.obj");
      StaticModelManager.LoadMesh("RedRanger",
        Directory.GetCurrentDirectory() + @"\..\..\Assets\RedRanger.obj");

      StaticModelManager.LoadModel("Terrain", new string[] { "grass" }, new string[] { "terrain" });
      StaticModelManager.LoadModel("RedRanger", new string[] { "RedRanger" }, new string[] { "RedRanger" });
      StaticModelManager.LoadModel("BlueRanger", new string[] { "BlueRanger" }, new string[] { "RedRanger" });
      StaticModelManager.LoadModel("BlackRanger", new string[] { "BlackRanger" }, new string[] { "RedRanger" });
      StaticModelManager.LoadModel("PinkRanger", new string[] { "PinkRanger" }, new string[] { "RedRanger" });
      StaticModelManager.LoadModel("YellowRanger", new string[] { "YellowRanger" }, new string[] { "RedRanger" });

      StaticModelManager.LoadSevenModel("RedRangerSeven",
        Directory.GetCurrentDirectory() + @"\..\..\Assets\RedRanger.obj7");
    }

    public override void InitializeShaders()
    {
      // LOAD SHADER FILES HERE.
      // Use the static class "ShaderManager"

      // These basic shaders do not include lighting effects.
      ShaderManager.LoadVertexShader("VertexShaderBasic",
        Directory.GetCurrentDirectory() + @"\..\..\Assets\Shaders\VertexShaderBasic.VertexShader");
      ShaderManager.LoadFragmentShader("FragmentShaderBasic",
        Directory.GetCurrentDirectory() + @"\..\..\Assets\Shaders\FragmentShaderBasic.FragmentShader");

      // These shaders set stages based of normal values to give a solid shading toon look.
      // WARNING!!! i glitched these shaders up pretty bad, im stil new to shaders... try the other two
      ShaderManager.LoadVertexShader("VertexShaderToon",
        Directory.GetCurrentDirectory() + @"\..\..\Assets\Shaders\VertexShaderToon.vertexShader");
      ShaderManager.LoadFragmentShader("FragmentShaderToon",
        Directory.GetCurrentDirectory() + @"\..\..\Assets\Shaders\FragmentShaderToon.fragmentShader");

      // These Lambertian shaders include an algorithm for lighting.
      ShaderManager.LoadVertexShader("VertexShaderLambertian",
        Directory.GetCurrentDirectory() + @"\..\..\Assets\Shaders\VertexShaderLambertian.VertexShader");
      ShaderManager.LoadFragmentShader("FragmentShaderLambertian",
        Directory.GetCurrentDirectory() + @"\..\..\Assets\Shaders\FragmentShaderLambertian.FragmentShader");

      // LINK TOGETHER YOUR SHADERS HERE
      // Use the static class "ShaderManager"

      ShaderManager.MakeShaderProgram(
        "ShaderProgramBasic", // What to name this shader program.
        "VertexShaderBasic", // The vertex shader to use.
        "FragmentShaderBasic", // The fragment shader to use.
        null, // The geometry shader to use.
        null); // The extended geometry shader to use.

      // WARNING!!! im still new to shaders and i messed this one up pretty bad. try the others.
      ShaderManager.MakeShaderProgram(
        "ShaderProgramToon",
        "VertexShaderToon",
        "FragmentShaderToon",
        null,
        null);

      ShaderManager.MakeShaderProgram(
        "ShaderProgramLambertian",
        "VertexShaderLambertian",
        "FragmentShaderLambertian",
        null,
        null);

      // DONT FORGET TO SELECT YOUR SHADERS WHEN YOU WANT TO USE THEM
      // Use the static class "ShaderManager"

      ShaderManager.SetActiveShader("ShaderProgramLambertian");
    }

    public override void InitializeStates()
    {
      // LOAD THE GAME STATES HERE
      // Use the static class "StateManager"

      StateManager.AddState("gameState", new GameState());
      StateManager.AddState("priorityHeapTesting", new PowerRangerDNA());
      //StateManager.ChangeState("gameState");
      StateManager.ChangeState("priorityHeapTesting");
    }

    public override void Update(double elapsedTime)
    {
      //DO NOT UPDATE GAME LOGIC HERE!!! Use the state manager and only change states if need be...
      string stateStatus = StateManager.Update(elapsedTime);
      // Use the stateStatus string to determine the need for a state change.
      // It defaults to "Don't Change States". An example is if the string is
      // "menuState", you could call - StateManager.ChangeState("menuState").
    }
  }
}