using System;

using SevenEngine;
using SevenEngine.DataStructures;
using SevenEngine.Imaging;
using SevenEngine.StaticModels;
using SevenEngine.Mathematics;

using Game.Units;

namespace Game.States
{
  public class AiBattle : InterfaceGameState
  {
    private string _id;
    private bool _isReady;

    public string Id { get { return _id; } set { _id = value; } }
    public bool IsReady { get { return _isReady; } }

    #region State Fields

    public static readonly float MeterLength = 10;
    //public static ListArray<Explosion> _explosions = new ListArray(10);

    public OctreeLinked<Unit, string> _octree = new OctreeLinked<Unit, string>(0, 0, 0, 1000000, 10,
      (Unit left, Unit right) => { return left.Id.CompareTo(right.Id); },
      (Unit left, string right) => { return left.Id.CompareTo(right); });

    private const int _meleeCount = 30;
    private const int _rangedCount = 30;
    private const int _kamakaziCount = 30;

    Camera _camera;
    StaticModel _terrain;
    StaticModel _mountain;
    StaticModel _mountain2;
    Unit[] _zackMelee;
    Unit[] _zackRanged;
    Unit[] _zackKamakazi;
    Unit[] _killemMelee;
    Unit[] _killemRanged;
    Unit[] _killemKamakazi;
    StaticModel _mushroomCloud;
    float _time;
    bool _bool;
    SkyBox _skybox;

    #endregion

    public AiBattle(string id)
    {
      _id = id;
      _isReady = false;
    }

    #region Loading

    public void Load()
    {
      _camera = new Camera();
      _camera.PositionSpeed = 5;
      _camera.Move(_camera.Up, 400);
      _camera.Move(_camera.Backward, 1500);
      _camera.Move(_camera.Backward, 300);

      _skybox = new SkyBox();
      _skybox.Scale.X = 10000;
      _skybox.Scale.Y = 10000;
      _skybox.Scale.Z = 10000;
      _skybox.Left = TextureManager.Get("SkyboxLeft");
      _skybox.Right = TextureManager.Get("SkyboxRight");
      _skybox.Front = TextureManager.Get("SkyboxFront");
      _skybox.Back = TextureManager.Get("SkyboxBack");
      _skybox.Top = TextureManager.Get("SkyboxTop");

      _terrain = StaticModelManager.GetModel("Terrain");
      _terrain.Scale = new Vector(500, 20, 500);
      _terrain.Orientation = new Quaternion(0, 0, 0, 0);
      _terrain.Position = new Vector(0, 0, 0);

      _mushroomCloud = StaticModelManager.GetModel("MushroomCloud");
      _mushroomCloud.Scale = new Vector(500, 20, 500);
      _mushroomCloud.Orientation = new Quaternion(0, 0, 0, 0);
      _mushroomCloud.Position.X = 0;
      _mushroomCloud.Position.Y = _terrain.Position.Y + 30;
      _mushroomCloud.Position.Z = 0;
      _time = 0;
      _bool = false;

      _mountain = StaticModelManager.GetModel("Mountain");
      _mountain.Scale = new Vector(5000, 5000, 5000);
      _mountain.Orientation = new Quaternion(0, 0, 0, 0);
      _mountain.Position = new Vector(4000, 0, 1000);

      _mountain2 = StaticModelManager.GetModel("Mountain2");
      _mountain2.Scale = new Vector(3500, 3500, 3500);
      _mountain2.Orientation = new Quaternion(0, 0, 0, 0);
      _mountain2.Position = new Vector(0, 0, 2500);

      GenerateUnits();
      
      Renderer.Font = TextManager.GetFont("Calibri");

      // ONCE YOU ARE DONE LOADING, BE SURE TO SET YOUR READY 
      // PROPERTY TO TRUE SO MY ENGINE DOESN'T SCREAM AT YOU
      _isReady = true;
    }

    private void GenerateUnits()
    {
      _zackMelee = new Unit[_meleeCount];
      _zackRanged = new Unit[_rangedCount];
      _zackKamakazi = new Unit[_kamakaziCount];
      _killemMelee = new Unit[_meleeCount];
      _killemRanged = new Unit[_rangedCount];
      _killemKamakazi = new Unit[_kamakaziCount];

      int maxXZack = -1000;
      int minXZack = -1500;

      int maxZZack = 0;
      int minZZack = -1500;

      int maxXKillem = 1500;
      int minXKillem = 1000;

      int maxZKillem = 0;
      int minZKillem = -1500;

      Random random = new Random();
      string[] colors = new string[] { "YellowRanger", "RedRanger", "BlueRanger", "BlackRanger", "PinkRanger" };

      for (int i = 0; i < _meleeCount; i++)
      {
        _zackMelee[i] = new ZackMelee("ZackMelee" + i, StaticModelManager.GetModel("BlackRanger"));
        _zackMelee[i].StaticModel.Position.X = random.Next(minXZack, maxXZack);
        _zackMelee[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
        _zackMelee[i].StaticModel.Position.Z = random.Next(minZZack, maxZZack);
        _zackMelee[i].StaticModel.Scale = new Vector(5, 5, 5);
        _zackMelee[i].StaticModel.Orientation = new Quaternion(0, 1, 0, 0);
        _octree.Add(_zackMelee[i]);

        _killemMelee[i] = new KillemMelee("KillemMelee" + i, StaticModelManager.GetModel("Tux"));
        _killemMelee[i].StaticModel.Position.X = random.Next(minXKillem, maxXKillem);
        _killemMelee[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
        _killemMelee[i].StaticModel.Position.Z = random.Next(minZKillem, maxZKillem);
        _killemMelee[i].StaticModel.Scale = new Vector(20, 20, 20);
        _killemMelee[i].StaticModel.Orientation = new Quaternion(0, 1, 0, -Trigonometry.HalfPi);
        _octree.Add(_killemMelee[i]);
      }

      for (int i = 0; i < _rangedCount; i++)
      {
        _zackRanged[i] = new ZackRanged("ZackRanged" + i, StaticModelManager.GetModel("BlueRanger"));
        _zackRanged[i].StaticModel.Position.X = random.Next(minXZack, maxXZack);
        _zackRanged[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
        _zackRanged[i].StaticModel.Position.Z = random.Next(minZZack, maxZZack);
        _zackRanged[i].StaticModel.Scale = new Vector(5, 5, 5);
        _zackRanged[i].StaticModel.Orientation = new Quaternion(0, 1, 0, 0);
        _octree.Add(_zackRanged[i]);

        _killemRanged[i] = new KillemRanged("KillemRanged" + i, StaticModelManager.GetModel("TuxGreen"));
        _killemRanged[i].StaticModel.Position.X = random.Next(minXKillem, maxXKillem);
        _killemRanged[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
        _killemRanged[i].StaticModel.Position.Z = random.Next(minZKillem, maxZKillem);
        _killemRanged[i].StaticModel.Scale = new Vector(20, 20, 20);
        _killemRanged[i].StaticModel.Orientation = new Quaternion(0, 1, 0, -Trigonometry.HalfPi);
        _killemRanged[i].Id = "Ranger" + i;
        _octree.Add(_killemRanged[i]);
      }

      for (int i = 0; i < _kamakaziCount; i++)
      {
        _zackKamakazi[i] = new ZackKamakazi("ZackKamakazi" + i, StaticModelManager.GetModel("RedRanger"));
        _zackKamakazi[i].StaticModel.Position.X = random.Next(minXZack, maxXZack);
        _zackKamakazi[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
        _zackKamakazi[i].StaticModel.Position.Z = random.Next(minZZack, maxZZack);
        _zackKamakazi[i].StaticModel.Scale = new Vector(5, 5, 5);
        _zackKamakazi[i].StaticModel.Orientation = new Quaternion(0, 1, 0, 0);
        _octree.Add(_zackKamakazi[i]);

        _killemKamakazi[i] = new KillemKamakazi("KillemKamakazi" + i, StaticModelManager.GetModel("TuxRed"));
        _killemKamakazi[i].StaticModel.Position.X = random.Next(minXKillem, maxXKillem);
        _killemKamakazi[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
        _killemKamakazi[i].StaticModel.Position.Z = random.Next(minZKillem, maxZKillem);
        _killemKamakazi[i].StaticModel.Scale = new Vector(20, 20, 20);
        _killemKamakazi[i].StaticModel.Orientation = new Quaternion(0, 1, 0, -Trigonometry.HalfPi);
        _octree.Add(_killemKamakazi[i]);
      }
    }

    #endregion

    #region Rendering

    public void Render()
    {
      // RENDER YOUR GAME HERE
      // Use the static class "Renderer"
      // EXAMPLES:
      // Renderer.CurrentCamera = cameraYouWantToUse;
      Renderer.CurrentCamera = _camera;

      //Renderer.SetProjectionMatrix();

      _octree.Traverse(
        (Unit model) =>
        {
          Renderer.DrawStaticModel(model.StaticModel);
        },
        -100000, -100000, -100000, 100000, 100000, 100000);

      Renderer.DrawSkybox(_skybox);
      Renderer.DrawStaticModel(_terrain);
      Renderer.DrawStaticModel(_mountain);
      Renderer.DrawStaticModel(_mountain2);

      //if (_mushroomCloud.Scale.X > 0)
      if (_mushroomCloud.Scale.X > 0 && _bool)
        Renderer.DrawStaticModel(_mushroomCloud);

      // EXAMPLE:
      // Renderer.RenderText("whatToWrite", x, y, size, rotation, color);
      Renderer.RenderText("Welcome To", 0f, 1f, 50f, 0, Color.Black);
      Renderer.RenderText("SevenEngine!", .15f, .95f, 50f, 0, Color.Teal);

      Renderer.RenderText("Close: ESC", 0f, .2f, 30f, 0f, Color.White);
      Renderer.RenderText("Fullscreen: F1", 0f, .15f, 30f, 0, Color.SteelBlue);
      Renderer.RenderText("Camera Movement: w, a, s, d", 0f, .1f, 30f, 0, Color.Tomato);
      Renderer.RenderText("Camera Angle: j, k, l, u", 0f, .05f, 30f, 0, Color.Yellow);
    }

    #endregion

    #region Updating

    public string Update(float elapsedTime)
    {
      _time += elapsedTime / 4f;

      if (_time > 200)
      {
        _time = 0;
        _bool = !_bool;
      }

      CameraControls();

      /*foreach (Unit unit in _killemMelee)
      {
        Vector v1 = new Vector(0, 0, -1);
        Vector v2 = v1.RotateBy(unit.StaticModel.Orientation.W, 0, 1, 0);
        unit.Position.X += (v2.X / v2.Length) * unit.MoveSpeed;
        unit.Position.Y += (v2.Y / v2.Length) * unit.MoveSpeed;
        unit.Position.Z += (v2.Z / v2.Length) * unit.MoveSpeed;
        unit.StaticModel.Orientation.W += .01f;
      }*/

      _skybox.Position.X = _camera.Position.X;
      _skybox.Position.Y = _camera.Position.Y;
      _skybox.Position.Z = _camera.Position.Z;

      _mushroomCloud.Scale.X = _time;
      _mushroomCloud.Scale.Y = _time;
      _mushroomCloud.Scale.Z = _time;

      //_mushroomCloud.Scale.X = Trigonometry.Sin(_time / 300f) * 200f;
      //_mushroomCloud.Scale.Y = Trigonometry.Sin(_time / 300f) * 200f;
      //_mushroomCloud.Scale.Z = Trigonometry.Sin(_time / 300f) * 200f;

      return "Don't Change States";
    }

    private void CameraControls()
    {
      // Camera position movement
      if (InputManager.Keyboard.Qdown)
        if (InputManager.Keyboard.ShiftLeftdown)
          _camera.Move(_camera.Down, _camera.PositionSpeed * 100);
        else
          _camera.Move(_camera.Down, _camera.PositionSpeed);
      if (InputManager.Keyboard.Edown)
        if (InputManager.Keyboard.ShiftLeftdown)
          _camera.Move(_camera.Up, _camera.PositionSpeed * 100);
        else
          _camera.Move(_camera.Up, _camera.PositionSpeed);
      if (InputManager.Keyboard.Adown)
        if (InputManager.Keyboard.ShiftLeftdown)
          _camera.Move(_camera.Left, _camera.PositionSpeed * 100);
        else
          _camera.Move(_camera.Left, _camera.PositionSpeed);
      if (InputManager.Keyboard.Wdown)
        if (InputManager.Keyboard.ShiftLeftdown)
          _camera.Move(_camera.Forward, _camera.PositionSpeed * 100);
        else
          _camera.Move(_camera.Forward, _camera.PositionSpeed);
      if (InputManager.Keyboard.Sdown)
        if (InputManager.Keyboard.ShiftLeftdown)
          _camera.Move(_camera.Backward, _camera.PositionSpeed * 100);
        else
          _camera.Move(_camera.Backward, _camera.PositionSpeed);
      if (InputManager.Keyboard.Ddown)
        if (InputManager.Keyboard.ShiftLeftdown)
          _camera.Move(_camera.Right, _camera.PositionSpeed * 100);
        else
          _camera.Move(_camera.Right, _camera.PositionSpeed);

      // Camera look angle adjustment
      if (InputManager.Keyboard.Kdown)
        _camera.RotateX(.01f);
      if (InputManager.Keyboard.Idown)
        _camera.RotateX(-.01f);
      if (InputManager.Keyboard.Jdown)
        _camera.RotateY(.01f);
      if (InputManager.Keyboard.Ldown)
        _camera.RotateY(-.01f);
    }

    #endregion
  }
}