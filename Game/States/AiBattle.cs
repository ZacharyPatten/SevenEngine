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
    public static bool _3d = false;
    public static int _map = 0;
    public static bool _paused = false;
    public static bool _showlines = false;

    private string _id;
    private bool _isReady;

    public string Id { get { return _id; } set { _id = value; } }
    public bool IsReady { get { return _isReady; } }

    #region State Fields

    //public static readonly float MeterLength = 10;
    //public static ListArray<Explosion> _explosions = new ListArray(10);

    public static AvlTree<Link3<Vector, Vector, Color>> lines;// = new ListArray<Link3<Vector, Vector, Color>>(1);
    public static ListArray<Explosion> explosions;// = new ListArray<Explosion>(1);
    public static int _powerRangerCount;
    public static int _tuxCount;

    /*public static OctreeLinked<Unit, string> _octree = new OctreeLinked<Unit, string>(0, 0, 0, 1000000, 10,
      (Unit left, Unit right) => { return left.Id.CompareTo(right.Id); },
      (Unit left, string right) => { return left.Id.CompareTo(right); });*/

    public static OctreeLinked<Unit, string> _octree;
      //= new OctreeLinked<Unit, string>(0, 0, 0, 1000000, 10, Unit.CompareTo, Unit.CompareTo);

    private int _meleeCount = 30;
    private int _rangedCount = 30;
    private int _kamakaziCount = 30;

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
      _powerRangerCount = _rangedCount + _meleeCount + _kamakaziCount;
      _tuxCount = _rangedCount + _meleeCount + _kamakaziCount;

      _octree = new OctreeLinked<Unit, string>(0, 0, 0, 1000000, 10, Unit.CompareTo, Unit.CompareTo);
      lines = new AvlTreeLinked<Link3<Vector, Vector, Color>>(
        (Link3<Vector, Vector, Color> link, Link3<Vector, Vector, Color> link2) => { if (link.First == link2.First && link.Second == link2.Second) return 0; else return 1; }
        );
      explosions = new ListArray<Explosion>(1);

      _zackMelee = new Unit[_meleeCount];
      _zackRanged = new Unit[_rangedCount];
      _zackKamakazi = new Unit[_kamakaziCount];
      _killemMelee = new Unit[_meleeCount];
      _killemRanged = new Unit[_rangedCount];
      _killemKamakazi = new Unit[_kamakaziCount];

      Random random = new Random();
      string[] colors = new string[] { "YellowRanger", "RedRanger", "BlueRanger", "BlackRanger", "PinkRanger" };
      #region Map 0
      if (_map == 0)
      {
        int maxXZack = -1500;
        int minXZack = -2000;

        int maxZZack = 0;
        int minZZack = -1500;

        int maxXKillem = 2000;
        int minXKillem = 1500;

        int maxZKillem = 0;
        int minZKillem = -1500;

        for (int i = 0; i < _meleeCount; i++)
        {
          _zackMelee[i] = new ZackMelee("ZackMelee" + i, StaticModelManager.GetModel("BlackRanger"));
          _zackMelee[i].StaticModel.Position.X = random.Next(minXZack, maxXZack);
          if (_3d)
            _zackMelee[i].StaticModel.Position.Y = _terrain.Position.Y + 10 + random.Next(0, 1000);
          else
            _zackMelee[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
          _zackMelee[i].StaticModel.Position.Z = random.Next(minZZack, maxZZack);
          _zackMelee[i].StaticModel.Scale = new Vector(5, 5, 5);
          _zackMelee[i].StaticModel.Orientation = new Quaternion(0, 1, 0, 0);
          _octree.Add(_zackMelee[i]);

          _killemMelee[i] = new KillemMelee("KillemMelee" + i, StaticModelManager.GetModel("Tux"));
          _killemMelee[i].StaticModel.Position.X = random.Next(minXKillem, maxXKillem);
          if (_3d)
            _killemMelee[i].StaticModel.Position.Y = _terrain.Position.Y + 10 + random.Next(0, 1000);
          else
            _killemMelee[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
          _killemMelee[i].StaticModel.Position.Z = random.Next(minZKillem, maxZKillem);
          _killemMelee[i].StaticModel.Scale = new Vector(20, 20, 20);
          _killemMelee[i].StaticModel.Orientation = new Quaternion(0, 1, 0, -Calc.PiOverTwo);
          _octree.Add(_killemMelee[i]);
        }

        for (int i = 0; i < _rangedCount; i++)
        {
          _zackRanged[i] = new ZackRanged("ZackRanged" + i, StaticModelManager.GetModel("BlueRanger"));
          _zackRanged[i].StaticModel.Position.X = random.Next(minXZack, maxXZack);
          if (_3d)
            _zackRanged[i].StaticModel.Position.Y = _terrain.Position.Y + 10 + random.Next(0, 1000);
          else
            _zackRanged[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
          _zackRanged[i].StaticModel.Position.Z = random.Next(minZZack, maxZZack);
          _zackRanged[i].StaticModel.Scale = new Vector(5, 5, 5);
          _zackRanged[i].StaticModel.Orientation = new Quaternion(0, 1, 0, 0);
          _octree.Add(_zackRanged[i]);

          _killemRanged[i] = new KillemRanged("KillemRanged" + i, StaticModelManager.GetModel("TuxGreen"));
          _killemRanged[i].StaticModel.Position.X = random.Next(minXKillem, maxXKillem);
          if (_3d)
            _killemRanged[i].StaticModel.Position.Y = _terrain.Position.Y + 10 + random.Next(0, 1000);
          else
            _killemRanged[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
          _killemRanged[i].StaticModel.Position.Z = random.Next(minZKillem, maxZKillem);
          _killemRanged[i].StaticModel.Scale = new Vector(20, 20, 20);
          _killemRanged[i].StaticModel.Orientation = new Quaternion(0, 1, 0, -Calc.PiOverTwo);
          _killemRanged[i].Id = "Ranger" + i;
          _octree.Add(_killemRanged[i]);
        }

        for (int i = 0; i < _kamakaziCount; i++)
        {
          _zackKamakazi[i] = new ZackKamakazi("ZackKamakazi" + i, StaticModelManager.GetModel("RedRanger"));
          _zackKamakazi[i].StaticModel.Position.X = random.Next(minXZack, maxXZack);
          if (_3d)
            _zackKamakazi[i].StaticModel.Position.Y = _terrain.Position.Y + 10 + random.Next(0, 1000);
          else
            _zackKamakazi[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
          _zackKamakazi[i].StaticModel.Position.Z = random.Next(minZZack, maxZZack);
          _zackKamakazi[i].StaticModel.Scale = new Vector(5, 5, 5);
          _zackKamakazi[i].StaticModel.Orientation = new Quaternion(0, 1, 0, 0);
          _octree.Add(_zackKamakazi[i]);

          _killemKamakazi[i] = new KillemKamakazi("KillemKamakazi" + i, StaticModelManager.GetModel("TuxRed"));
          _killemKamakazi[i].StaticModel.Position.X = random.Next(minXKillem, maxXKillem);
          if (_3d)
            _killemKamakazi[i].StaticModel.Position.Y = _terrain.Position.Y + 10 + random.Next(0, 1000);
          else
            _killemKamakazi[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
          _killemKamakazi[i].StaticModel.Position.Z = random.Next(minZKillem, maxZKillem);
          _killemKamakazi[i].StaticModel.Scale = new Vector(20, 20, 20);
          _killemKamakazi[i].StaticModel.Orientation = new Quaternion(0, 1, 0, -Calc.PiOverTwo);
          _octree.Add(_killemKamakazi[i]);
        }
      }
      #endregion
      #region Map 1
      else if (_map == 1)
      {
        int maxXZack = -800;
        int minXZack = -1300;
        int maxZZack = -800;
        int minZZack = -1300;
        int maxXZack2 = 1300;
        int minXZack2 = 800;
        int maxZZack2 = 1300;
        int minZZack2 = 800;

        int maxXKillem = -800;
        int minXKillem = -1300;
        int maxZKillem = 1300;
        int minZKillem = 800;
        int maxXKillem2 = 1300;
        int minXKillem2 = 800;
        int maxZKillem2 = -800;
        int minZKillem2 = -1300;

        for (int i = 0; i < _meleeCount; i++)
        {
          if (i < _meleeCount / 2)
          {
            _zackMelee[i] = new ZackMelee("ZackMelee" + i, StaticModelManager.GetModel("BlackRanger"));
            _zackMelee[i].StaticModel.Position.X = random.Next(minXZack, maxXZack);
            if (_3d)
              _zackMelee[i].StaticModel.Position.Y = _terrain.Position.Y + 10 + random.Next(0, 1000);
            else
              _zackMelee[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
            _zackMelee[i].StaticModel.Position.Z = random.Next(minZZack, maxZZack);
            _zackMelee[i].StaticModel.Scale = new Vector(5, 5, 5);
            _zackMelee[i].StaticModel.Orientation = new Quaternion(0, 1, 0, 0);
            _octree.Add(_zackMelee[i]);

            _killemMelee[i] = new KillemMelee("KillemMelee" + i, StaticModelManager.GetModel("Tux"));
            _killemMelee[i].StaticModel.Position.X = random.Next(minXKillem, maxXKillem);
            if (_3d)
              _killemMelee[i].StaticModel.Position.Y = _terrain.Position.Y + 10 + random.Next(0, 1000);
            else
              _killemMelee[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
            _killemMelee[i].StaticModel.Position.Z = random.Next(minZKillem, maxZKillem);
            _killemMelee[i].StaticModel.Scale = new Vector(20, 20, 20);
            _killemMelee[i].StaticModel.Orientation = new Quaternion(0, 1, 0, -Calc.PiOverTwo);
            _octree.Add(_killemMelee[i]);
          }
          else
          {
            _zackMelee[i] = new ZackMelee("ZackMelee" + i, StaticModelManager.GetModel("BlackRanger"));
            _zackMelee[i].StaticModel.Position.X = random.Next(minXZack2, maxXZack2);
            if (_3d)
              _zackMelee[i].StaticModel.Position.Y = _terrain.Position.Y + 10 + random.Next(0, 1000);
            else
              _zackMelee[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
            _zackMelee[i].StaticModel.Position.Z = random.Next(minZZack2, maxZZack2);
            _zackMelee[i].StaticModel.Scale = new Vector(5, 5, 5);
            _zackMelee[i].StaticModel.Orientation = new Quaternion(0, 1, 0, 0);
            _octree.Add(_zackMelee[i]);

            _killemMelee[i] = new KillemMelee("KillemMelee" + i, StaticModelManager.GetModel("Tux"));
            _killemMelee[i].StaticModel.Position.X = random.Next(minXKillem2, maxXKillem2);
            if (_3d)
              _killemMelee[i].StaticModel.Position.Y = _terrain.Position.Y + 10 + random.Next(0, 1000);
            else
              _killemMelee[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
            _killemMelee[i].StaticModel.Position.Z = random.Next(minZKillem2, maxZKillem2);
            _killemMelee[i].StaticModel.Scale = new Vector(20, 20, 20);
            _killemMelee[i].StaticModel.Orientation = new Quaternion(0, 1, 0, -Calc.PiOverTwo);
            _octree.Add(_killemMelee[i]);
          }
        }

        for (int i = 0; i < _rangedCount; i++)
        {
          if (i < _rangedCount / 2)
          {
            _zackRanged[i] = new ZackRanged("ZackRanged" + i, StaticModelManager.GetModel("BlueRanger"));
            _zackRanged[i].StaticModel.Position.X = random.Next(minXZack, maxXZack);
            if (_3d)
              _zackRanged[i].StaticModel.Position.Y = _terrain.Position.Y + 10 + random.Next(0, 1000);
            else
              _zackRanged[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
            _zackRanged[i].StaticModel.Position.Z = random.Next(minZZack, maxZZack);
            _zackRanged[i].StaticModel.Scale = new Vector(5, 5, 5);
            _zackRanged[i].StaticModel.Orientation = new Quaternion(0, 1, 0, 0);
            _octree.Add(_zackRanged[i]);

            _killemRanged[i] = new KillemRanged("KillemRanged" + i, StaticModelManager.GetModel("TuxGreen"));
            _killemRanged[i].StaticModel.Position.X = random.Next(minXKillem, maxXKillem);
            if (_3d)
              _killemRanged[i].StaticModel.Position.Y = _terrain.Position.Y + 10 + random.Next(0, 1000);
            else
              _killemRanged[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
            _killemRanged[i].StaticModel.Position.Z = random.Next(minZKillem, maxZKillem);
            _killemRanged[i].StaticModel.Scale = new Vector(20, 20, 20);
            _killemRanged[i].StaticModel.Orientation = new Quaternion(0, 1, 0, -Calc.PiOverTwo);
            _killemRanged[i].Id = "Ranger" + i;
            _octree.Add(_killemRanged[i]);
          }
          else
          {
            _zackRanged[i] = new ZackRanged("ZackRanged" + i, StaticModelManager.GetModel("BlueRanger"));
            _zackRanged[i].StaticModel.Position.X = random.Next(minXZack2, maxXZack2);
            if (_3d)
              _zackRanged[i].StaticModel.Position.Y = _terrain.Position.Y + 10 + random.Next(0, 1000);
            else
              _zackRanged[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
            _zackRanged[i].StaticModel.Position.Z = random.Next(minZZack2, maxZZack2);
            _zackRanged[i].StaticModel.Scale = new Vector(5, 5, 5);
            _zackRanged[i].StaticModel.Orientation = new Quaternion(0, 1, 0, 0);
            _octree.Add(_zackRanged[i]);

            _killemRanged[i] = new KillemRanged("KillemRanged" + i, StaticModelManager.GetModel("TuxGreen"));
            _killemRanged[i].StaticModel.Position.X = random.Next(minXKillem2, maxXKillem2);
            if (_3d)
              _killemRanged[i].StaticModel.Position.Y = _terrain.Position.Y + 10 + random.Next(0, 1000);
            else
              _killemRanged[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
            _killemRanged[i].StaticModel.Position.Z = random.Next(minZKillem2, maxZKillem2);
            _killemRanged[i].StaticModel.Scale = new Vector(20, 20, 20);
            _killemRanged[i].StaticModel.Orientation = new Quaternion(0, 1, 0, -Calc.PiOverTwo);
            _killemRanged[i].Id = "Ranger" + i;
            _octree.Add(_killemRanged[i]);
          }
        }

        for (int i = 0; i < _kamakaziCount; i++)
        {
          if (i < _kamakaziCount / 2)
          {
            _zackKamakazi[i] = new ZackKamakazi("ZackKamakazi" + i, StaticModelManager.GetModel("RedRanger"));
            _zackKamakazi[i].StaticModel.Position.X = random.Next(minXZack, maxXZack);
            if (_3d)
              _zackKamakazi[i].StaticModel.Position.Y = _terrain.Position.Y + 10 + random.Next(0, 1000);
            else
              _zackKamakazi[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
            _zackKamakazi[i].StaticModel.Position.Z = random.Next(minZZack, maxZZack);
            _zackKamakazi[i].StaticModel.Scale = new Vector(5, 5, 5);
            _zackKamakazi[i].StaticModel.Orientation = new Quaternion(0, 1, 0, 0);
            _octree.Add(_zackKamakazi[i]);

            _killemKamakazi[i] = new KillemKamakazi("KillemKamakazi" + i, StaticModelManager.GetModel("TuxRed"));
            _killemKamakazi[i].StaticModel.Position.X = random.Next(minXKillem, maxXKillem);
            if (_3d)
              _killemKamakazi[i].StaticModel.Position.Y = _terrain.Position.Y + 10 + random.Next(0, 1000);
            else
              _killemKamakazi[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
            _killemKamakazi[i].StaticModel.Position.Z = random.Next(minZKillem, maxZKillem);
            _killemKamakazi[i].StaticModel.Scale = new Vector(20, 20, 20);
            _killemKamakazi[i].StaticModel.Orientation = new Quaternion(0, 1, 0, -Calc.PiOverTwo);
            _octree.Add(_killemKamakazi[i]);
          }
          else
          {
            _zackKamakazi[i] = new ZackKamakazi("ZackKamakazi" + i, StaticModelManager.GetModel("RedRanger"));
            _zackKamakazi[i].StaticModel.Position.X = random.Next(minXZack2, maxXZack2);
            if (_3d)
              _zackKamakazi[i].StaticModel.Position.Y = _terrain.Position.Y + 10 + random.Next(0, 1000);
            else
              _zackKamakazi[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
            _zackKamakazi[i].StaticModel.Position.Z = random.Next(minZZack2, maxZZack2);
            _zackKamakazi[i].StaticModel.Scale = new Vector(5, 5, 5);
            _zackKamakazi[i].StaticModel.Orientation = new Quaternion(0, 1, 0, 0);
            _octree.Add(_zackKamakazi[i]);

            _killemKamakazi[i] = new KillemKamakazi("KillemKamakazi" + i, StaticModelManager.GetModel("TuxRed"));
            _killemKamakazi[i].StaticModel.Position.X = random.Next(minXKillem2, maxXKillem2);
            if (_3d)
              _killemKamakazi[i].StaticModel.Position.Y = _terrain.Position.Y + 10 + random.Next(0, 1000);
            else
              _killemKamakazi[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
            _killemKamakazi[i].StaticModel.Position.Z = random.Next(minZKillem2, maxZKillem2);
            _killemKamakazi[i].StaticModel.Scale = new Vector(20, 20, 20);
            _killemKamakazi[i].StaticModel.Orientation = new Quaternion(0, 1, 0, -Calc.PiOverTwo);
            _octree.Add(_killemKamakazi[i]);
          }
        }
      }
      #endregion
      #region Map 2
      if (_map == 2)
      {
        int maxXZack = 250;
        int minXZack = -250;
        int maxZZack = 250;
        int minZZack = -250;

        int maxXKillem = 250;
        int minXKillem = -250;
        int maxZKillem = 250;
        int minZKillem = -250;

        for (int i = 0; i < _meleeCount; i++)
        {
          _zackMelee[i] = new ZackMelee("ZackMelee" + i, StaticModelManager.GetModel("BlackRanger"));
          _zackMelee[i].StaticModel.Position.X = random.Next(minXZack, maxXZack);
          if (_3d)
            _zackMelee[i].StaticModel.Position.Y = _terrain.Position.Y + 10 + random.Next(0, 1000);
          else
            _zackMelee[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
          _zackMelee[i].StaticModel.Position.Z = random.Next(minZZack, maxZZack);
          _zackMelee[i].StaticModel.Scale = new Vector(5, 5, 5);
          _zackMelee[i].StaticModel.Orientation = new Quaternion(0, 1, 0, 0);
          _octree.Add(_zackMelee[i]);

          _killemMelee[i] = new KillemMelee("KillemMelee" + i, StaticModelManager.GetModel("Tux"));
          _killemMelee[i].StaticModel.Position.X = random.Next(minXKillem, maxXKillem);
          if (_3d)
            _killemMelee[i].StaticModel.Position.Y = _terrain.Position.Y + 10 + random.Next(0, 1000);
          else
            _killemMelee[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
          _killemMelee[i].StaticModel.Position.Z = random.Next(minZKillem, maxZKillem);
          _killemMelee[i].StaticModel.Scale = new Vector(20, 20, 20);
          _killemMelee[i].StaticModel.Orientation = new Quaternion(0, 1, 0, -Calc.PiOverTwo);
          _octree.Add(_killemMelee[i]);
        }

        for (int i = 0; i < _rangedCount; i++)
        {
          _zackRanged[i] = new ZackRanged("ZackRanged" + i, StaticModelManager.GetModel("BlueRanger"));
          _zackRanged[i].StaticModel.Position.X = random.Next(minXZack, maxXZack);
          if (_3d)
            _zackRanged[i].StaticModel.Position.Y = _terrain.Position.Y + 10 + random.Next(0, 1000);
          else
            _zackRanged[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
          _zackRanged[i].StaticModel.Position.Z = random.Next(minZZack, maxZZack);
          _zackRanged[i].StaticModel.Scale = new Vector(5, 5, 5);
          _zackRanged[i].StaticModel.Orientation = new Quaternion(0, 1, 0, 0);
          _octree.Add(_zackRanged[i]);

          _killemRanged[i] = new KillemRanged("KillemRanged" + i, StaticModelManager.GetModel("TuxGreen"));
          _killemRanged[i].StaticModel.Position.X = random.Next(minXKillem, maxXKillem);
          if (_3d)
            _killemRanged[i].StaticModel.Position.Y = _terrain.Position.Y + 10 + random.Next(0, 1000);
          else
            _killemRanged[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
          _killemRanged[i].StaticModel.Position.Z = random.Next(minZKillem, maxZKillem);
          _killemRanged[i].StaticModel.Scale = new Vector(20, 20, 20);
          _killemRanged[i].StaticModel.Orientation = new Quaternion(0, 1, 0, -Calc.PiOverTwo);
          _killemRanged[i].Id = "Ranger" + i;
          _octree.Add(_killemRanged[i]);
        }

        // CIRCLE: radius ^ 2 = x ^ 2 + z ^ 2
        float radiusSquared = 1000000f;

        for (int i = 0; i < _kamakaziCount; i++)
        {
          float x = random.Next(-1000, 1000);

          _zackKamakazi[i] = new ZackKamakazi("ZackKamakazi" + i, StaticModelManager.GetModel("RedRanger"));
          _zackKamakazi[i].StaticModel.Position.X = x;
          if (_3d)
            _zackKamakazi[i].StaticModel.Position.Y = _terrain.Position.Y + 10 + random.Next(0, 1000);
          else
            _zackKamakazi[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
          _zackKamakazi[i].StaticModel.Position.Z = Calc.SquareRoot(radiusSquared - x * x);
          _zackKamakazi[i].StaticModel.Scale = new Vector(5, 5, 5);
          _zackKamakazi[i].StaticModel.Orientation = new Quaternion(0, 1, 0, 0);
          _octree.Add(_zackKamakazi[i]);

          _killemKamakazi[i] = new KillemKamakazi("KillemKamakazi" + i, StaticModelManager.GetModel("TuxRed"));
          _killemKamakazi[i].StaticModel.Position.X = x;
          if (_3d)
            _killemKamakazi[i].StaticModel.Position.Y = _terrain.Position.Y + 10 + random.Next(0, 1000);
          else
            _killemKamakazi[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
          _killemKamakazi[i].StaticModel.Position.Z = -Calc.SquareRoot(radiusSquared - x * x);
          _killemKamakazi[i].StaticModel.Scale = new Vector(20, 20, 20);
          _killemKamakazi[i].StaticModel.Orientation = new Quaternion(0, 1, 0, -Calc.PiOverTwo);
          _octree.Add(_killemKamakazi[i]);
        }
      }
      #endregion
      #region Map 3
      if (_map == 3)
      {
        int maxXZack = 2000;
        int minXZack = -2000;

        int maxZZack = 2000;
        int minZZack = -2000;

        int maxXKillem = 2000;
        int minXKillem = -2000;

        int maxZKillem = 2000;
        int minZKillem = -2000;

        for (int i = 0; i < _meleeCount; i++)
        {
          _zackMelee[i] = new ZackMelee("ZackMelee" + i, StaticModelManager.GetModel("BlackRanger"));
          _zackMelee[i].StaticModel.Position.X = random.Next(minXZack, maxXZack);
          if (_3d)
            _zackMelee[i].StaticModel.Position.Y = _terrain.Position.Y + 10 + random.Next(0, 1000);
          else
            _zackMelee[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
          _zackMelee[i].StaticModel.Position.Z = random.Next(minZZack, maxZZack);
          _zackMelee[i].StaticModel.Scale = new Vector(5, 5, 5);
          _zackMelee[i].StaticModel.Orientation = new Quaternion(0, 1, 0, 0);
          _octree.Add(_zackMelee[i]);

          _killemMelee[i] = new KillemMelee("KillemMelee" + i, StaticModelManager.GetModel("Tux"));
          _killemMelee[i].StaticModel.Position.X = random.Next(minXKillem, maxXKillem);
          if (_3d)
            _killemMelee[i].StaticModel.Position.Y = _terrain.Position.Y + 10 + random.Next(0, 1000);
          else
            _killemMelee[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
          _killemMelee[i].StaticModel.Position.Z = random.Next(minZKillem, maxZKillem);
          _killemMelee[i].StaticModel.Scale = new Vector(20, 20, 20);
          _killemMelee[i].StaticModel.Orientation = new Quaternion(0, 1, 0, -Calc.PiOverTwo);
          _octree.Add(_killemMelee[i]);
        }

        for (int i = 0; i < _rangedCount; i++)
        {
          _zackRanged[i] = new ZackRanged("ZackRanged" + i, StaticModelManager.GetModel("BlueRanger"));
          _zackRanged[i].StaticModel.Position.X = random.Next(minXZack, maxXZack);
          if (_3d)
            _zackRanged[i].StaticModel.Position.Y = _terrain.Position.Y + 10 + random.Next(0, 1000);
          else
            _zackRanged[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
          _zackRanged[i].StaticModel.Position.Z = random.Next(minZZack, maxZZack);
          _zackRanged[i].StaticModel.Scale = new Vector(5, 5, 5);
          _zackRanged[i].StaticModel.Orientation = new Quaternion(0, 1, 0, 0);
          _octree.Add(_zackRanged[i]);

          _killemRanged[i] = new KillemRanged("KillemRanged" + i, StaticModelManager.GetModel("TuxGreen"));
          _killemRanged[i].StaticModel.Position.X = random.Next(minXKillem, maxXKillem);
          if (_3d)
            _killemRanged[i].StaticModel.Position.Y = _terrain.Position.Y + 10 + random.Next(0, 1000);
          else
            _killemRanged[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
          _killemRanged[i].StaticModel.Position.Z = random.Next(minZKillem, maxZKillem);
          _killemRanged[i].StaticModel.Scale = new Vector(20, 20, 20);
          _killemRanged[i].StaticModel.Orientation = new Quaternion(0, 1, 0, -Calc.PiOverTwo);
          _killemRanged[i].Id = "Ranger" + i;
          _octree.Add(_killemRanged[i]);
        }

        for (int i = 0; i < _kamakaziCount; i++)
        {
          _zackKamakazi[i] = new ZackKamakazi("ZackKamakazi" + i, StaticModelManager.GetModel("RedRanger"));
          _zackKamakazi[i].StaticModel.Position.X = random.Next(minXZack, maxXZack);
          if (_3d)
            _zackKamakazi[i].StaticModel.Position.Y = _terrain.Position.Y + 10 + random.Next(0, 1000);
          else
            _zackKamakazi[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
          _zackKamakazi[i].StaticModel.Position.Z = random.Next(minZZack, maxZZack);
          _zackKamakazi[i].StaticModel.Scale = new Vector(5, 5, 5);
          _zackKamakazi[i].StaticModel.Orientation = new Quaternion(0, 1, 0, 0);
          _octree.Add(_zackKamakazi[i]);

          _killemKamakazi[i] = new KillemKamakazi("KillemKamakazi" + i, StaticModelManager.GetModel("TuxRed"));
          _killemKamakazi[i].StaticModel.Position.X = random.Next(minXKillem, maxXKillem);
          if (_3d)
            _killemKamakazi[i].StaticModel.Position.Y = _terrain.Position.Y + 10 + random.Next(0, 1000);
          else
            _killemKamakazi[i].StaticModel.Position.Y = _terrain.Position.Y + 10;
          _killemKamakazi[i].StaticModel.Position.Z = random.Next(minZKillem, maxZKillem);
          _killemKamakazi[i].StaticModel.Scale = new Vector(20, 20, 20);
          _killemKamakazi[i].StaticModel.Orientation = new Quaternion(0, 1, 0, -Calc.PiOverTwo);
          _octree.Add(_killemKamakazi[i]);
        }
      }
      #endregion
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
          if (!model.IsDead)
            Renderer.DrawStaticModel(model.StaticModel);
        },
        -100000, -100000, -100000, 100000, 100000, 100000);

      if (_showlines)
      {
        lines.Traverse
        (
          (Link3<Vector, Vector, Color> current) =>
          {
            Renderer.DrawLine(current.First, current.Second, current.Third);
          }
        );
      }

      explosions.Traverse
      (
        (Explosion current) =>
        {
          if (current.Model.Scale.X < 220)
          {
            Renderer.DrawStaticModel(current.Model);
            current.Model.Scale.X += 2.5f;
            current.Model.Scale.Y += 2.5f;
            current.Model.Scale.Z += 2.5f;
          }
        }
      );

      Renderer.DrawSkybox(_skybox);
      Renderer.DrawStaticModel(_terrain);
      Renderer.DrawStaticModel(_mountain);
      Renderer.DrawStaticModel(_mountain2);

      // EXAMPLE:
      // Renderer.RenderText("whatToWrite", x, y, size, rotation, color);
      Renderer.RenderText("Welcome To", 0f, 1f, 50f, 0, Color.Black);
      Renderer.RenderText("SevenEngine!", .15f, .95f, 50f, 0, Color.Teal);

      Renderer.RenderText("Battle Controls: Space, R, T, G, Y", .55f, .95f, 30f, 0, Color.Black);

      Renderer.RenderText("Map: " + _map, .85f, .85f, 30f, 0, Color.Black);
      if (_3d)
        Renderer.RenderText("Space: Yes", .85f, .9f, 30f, 0, Color.Black);
      else
        Renderer.RenderText("Space: No", .85f, .9f, 30f, 0, Color.Black);

      Renderer.RenderText("Unit Controls: z, x, c, v, b, n", .6f, .07f, 30f, 0, Color.Black);
      Renderer.RenderText("Unit Counts (M-R-K): " + _meleeCount + " " + _rangedCount + " " + _kamakaziCount, .6f, .12f, 30f, 0, Color.Black);

      Renderer.RenderText("Close: ESC", 0f, .2f, 30f, 0f, Color.White);
      Renderer.RenderText("Fullscreen: F1", 0f, .15f, 30f, 0, Color.SteelBlue);
      Renderer.RenderText("Camera Movement: w, a, s, d", 0f, .1f, 30f, 0, Color.Tomato);
      Renderer.RenderText("Camera Angle: j, k, l, i", 0f, .05f, 30f, 0, Color.Yellow);
    }

    #endregion

    #region Updating

    public string Update(float elapsedTime)
    {
      CameraControls();
      _skybox.Position.X = _camera.Position.X;
      _skybox.Position.Y = _camera.Position.Y;
      _skybox.Position.Z = _camera.Position.Z;

      if (InputManager.Keyboard.Zpressed)
        _meleeCount = (_meleeCount + 1) % 100;
      if (InputManager.Keyboard.Xpressed)
        if (_meleeCount > 0) _meleeCount--;
      if (InputManager.Keyboard.Cpressed)
        _rangedCount = (_rangedCount + 1) % 100;
      if (InputManager.Keyboard.Vpressed)
        if (_rangedCount > 0) _rangedCount--;
      if (InputManager.Keyboard.Bpressed)
        _kamakaziCount = (_kamakaziCount + 1) % 100;
      if (InputManager.Keyboard.Npressed)
        if (_kamakaziCount > 0) _kamakaziCount--;

      if (InputManager.Keyboard.Gpressed)
        _map = (_map + 1) % 4;

      if (InputManager.Keyboard.Rpressed)
        _showlines = !_showlines;

      if (InputManager.Keyboard.Ypressed)
        _3d = !_3d;

      if (InputManager.Keyboard.Tpressed)
        GenerateUnits();

      if (InputManager.Keyboard.Spacepressed)
        _paused = !_paused;

      if (!_paused)
      {
        _octree.Traverse((Unit model) => { model.AI(elapsedTime, _octree); }, -100000, -100000, -100000, 100000, 100000, 100000);

        OctreeLinked<Unit, string> octree = new OctreeLinked<Unit, string>(0, 0, 0, 1000000, 10, Unit.CompareTo, Unit.CompareTo);
        foreach (Unit unit in _zackMelee)
          octree.Add(unit);
        foreach (Unit unit in _zackRanged)
          octree.Add(unit);
        foreach (Unit unit in _zackKamakazi)
          octree.Add(unit);
        foreach (Unit unit in _killemMelee)
          octree.Add(unit);
        foreach (Unit unit in _killemRanged)
          octree.Add(unit);
        foreach (Unit unit in _killemKamakazi)
          octree.Add(unit);
        _octree = octree;
      }
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