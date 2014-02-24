using System;

using SevenEngine;
using SevenEngine.DataStructures;
using SevenEngine.Imaging;
using SevenEngine.StaticModels;
using SevenEngine.Mathematics;

namespace Game.States
{
  public class StaticModelState : InterfaceGameState
  {
    private string _id;
    private bool _isReady;

    public string Id { get { return _id; } set { _id = value; } }
    public bool IsReady { get { return _isReady; } }

    Camera _camera;

    StaticModel _terrain;
    StaticModel _redRanger;
    StaticModel _pinkRanger;
    StaticModel _blueRanger;
    StaticModel _blackRanger;
    StaticModel _yellowRanger;
    StaticModel _RedRangerTwo;

    public StaticModelState(string id)
    {
      _id = id;
      _isReady = false;
    }

    public void Load()
    {
      // Creates a camera and sets the initial positions
      _camera = new Camera();
      _camera.PositionSpeed = 5;
      _camera.Move(_camera.Up, 400);
      _camera.Move(_camera.Backward, 1500);
      _camera.Move(_camera.Backward, 300);

      // Gets a copy of the "Terrain" model and tracks the number of hardware instances of each component
      _terrain = StaticModelManager.GetModel("Terrain");
      _terrain.Scale = new Vector(500, 20, 500);
      _terrain.Orientation = new Quaternion(0, 0, 0, 0);
      _terrain.Position = new Vector(0, 0, 0);

      // Gets a copy of the "RedRanger" model and tracks the number of hardware instances of each component
      _redRanger = StaticModelManager.GetModel("RedRanger");
      _redRanger.Orientation = new Quaternion(0, 1, 0, 0);
      //_redRanger.RotationAngle = 180f;
      _redRanger.Scale = new Vector(5, 5, 5);
      _redRanger.Position = new Vector(_terrain.Position.X + 200, _terrain.Position.Y + 130, _terrain.Position.Z);

      // Gets a copy of the "YellowRanger" model and tracks the number of hardware instances of each component
      _yellowRanger = StaticModelManager.GetModel("YellowRanger");
      _yellowRanger.Orientation = new Quaternion(0, 1, 1, 0);
      _yellowRanger.Scale = new Vector(10, 10, 10);
      _yellowRanger.Position = new Vector(_terrain.Position.X + 100, _terrain.Position.Y + 130, _terrain.Position.Z);

      // Gets a copy of the "BlackRanger" model and tracks the number of hardware instances of each component
      _blackRanger = StaticModelManager.GetModel("BlackRanger");
      _blackRanger.Orientation = new Quaternion(1, 1, 0, 0);
      _blackRanger.Scale = new Vector(10, 10, 10);
      _blackRanger.Position = new Vector(_terrain.Position.X + 0, _terrain.Position.Y + 130, _terrain.Position.Z);

      // Gets a copy of the "BlueRanger" model and tracks the number of hardware instances of each component
      _blueRanger = StaticModelManager.GetModel("BlueRanger");
      _blueRanger.Orientation = new Quaternion(0, 1, 2, 0);
      _blueRanger.Scale = new Vector(10, 10, 10);
      _blueRanger.Position = new Vector(_terrain.Position.X - 200, _terrain.Position.Y + 130, _terrain.Position.Z);

      // Gets a copy of the "PinkRanger" model and tracks the number of hardware instances of each component
      _pinkRanger = StaticModelManager.GetModel("PinkRanger");
      _pinkRanger.Orientation = new Quaternion(0, 1, 0, 0);
      _pinkRanger.Scale = new Vector(10, 10, 10);
      _pinkRanger.Position = new Vector(_terrain.Position.X - 100, _terrain.Position.Y + 130, _terrain.Position.Z);

      // Gets a copy of the "RedRangerSeven" model and tracks the number of hardware instances of each component
      _RedRangerTwo = StaticModelManager.GetModel("RedRangerSeven");
      _RedRangerTwo.Orientation = new Quaternion(0, 1, 0, 0);
      _RedRangerTwo.Scale = new Vector(20, 20, 20);
      _RedRangerTwo.Position = new Vector(_terrain.Position.X - 500, _terrain.Position.Y + 130, _terrain.Position.Z + 700);

      _isReady = true;
    }

    public void Render()
    {
      // Set the camera for the rendering (alters the positions & orientations of objdects)
      Renderer.CurrentCamera = _camera;

      // You will alter the projection matrix here. Just use this for now, the Renderer is still under heavy development.
      Renderer.SetProjectionMatrix();

      Renderer.DrawStaticModel(_terrain);
      Renderer.DrawStaticModel(_redRanger);
      Renderer.DrawStaticModel(_blueRanger);
      Renderer.DrawStaticModel(_blackRanger);
      Renderer.DrawStaticModel(_pinkRanger);
      Renderer.DrawStaticModel(_yellowRanger);
      Renderer.DrawStaticModel(_RedRangerTwo);
    }

    public string Update(float elapsedTime)
    {
      CameraControls();

      if (InputManager.Keyboard.Rdown)
      {
        _redRanger.Orientation.W += 75;
        _yellowRanger.Orientation.W += 100;
        _blackRanger.Orientation.W += 200;
        _blueRanger.Orientation.W += 400;
        _pinkRanger.Orientation.W += 300;
      }

      _RedRangerTwo.Orientation.W++;
      _RedRangerTwo.Orientation.W++;

      _yellowRanger.Orientation.W++;
      _blackRanger.Orientation.W++;
      _blueRanger.Orientation.W++;
      _pinkRanger.Orientation.W++;
      return "Don't Change States";
    }

    private void CameraControls()
    {
      // Camera position movement
      if (InputManager.Keyboard.Qdown)
        _camera.Move(_camera.Down, _camera.PositionSpeed);
      if (InputManager.Keyboard.Edown)
        _camera.Move(_camera.Up, _camera.PositionSpeed);
      if (InputManager.Keyboard.Adown)
        _camera.Move(_camera.Left, _camera.PositionSpeed);
      if (InputManager.Keyboard.Wdown)
        _camera.Move(_camera.Forward, _camera.PositionSpeed);
      if (InputManager.Keyboard.Sdown)
        _camera.Move(_camera.Backward, _camera.PositionSpeed);
      if (InputManager.Keyboard.Ddown)
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
  }
}