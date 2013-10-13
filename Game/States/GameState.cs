using System;

using Engine;
using Engine.DataStructures;
using Engine.Imaging;
using Engine.Models;
using Engine.Mathematics;

namespace Game.States
{
  public class GameState : IGameState
  {
    Camera _camera;

    StaticModel _terrain;
    StaticModel _redRanger;
    StaticModel _pinkRanger;
    StaticModel _blueRanger;
    StaticModel _blackRanger;
    StaticModel _yellowRanger;

    public GameState()
    {
      _camera = new Camera();
      _camera.PositionSpeed = 5;
      _camera.Move(_camera.Up, 400);
      _camera.Move(_camera.Backward, 1500);
      _camera.Move(_camera.Backward, 300);
      TransformationManager.CurrentCamera = _camera;

      _terrain = new StaticModel();
      _terrain.Meshes.Add(new Link<Texture, StaticMesh>(TextureManager.Get("grass"), StaticModelManager.Get("terrain")));
      _terrain.Scale = new Vector(500, 20, 500);
      _terrain.RotationAmmounts = new Vector(0, 0, 0);
      _terrain.Position = new Vector(0, 0, 0);

      _redRanger = new StaticModel();
      _redRanger.Meshes.Add(new Link<Texture, StaticMesh>(TextureManager.Get("RedRanger"), StaticModelManager.Get("RedRanger")));
      _redRanger.RotationAmmounts = new Vector(0, 1, 0);
      _redRanger.RotationAngle = 180f;
      _redRanger.Scale = new Vector(5, 5, 5);
      _redRanger.Position = new Vector(_terrain.Position.X + 200, _terrain.Position.Y + 130, _terrain.Position.Z);

      _yellowRanger = new StaticModel();
      _yellowRanger.Meshes.Add(new Link<Texture, StaticMesh>(TextureManager.Get("YellowRanger"), StaticModelManager.Get("RedRanger")));
      _yellowRanger.RotationAmmounts = new Vector(0, 1, 1);
      _yellowRanger.Scale = new Vector(10, 10, 10);
      _yellowRanger.Position = new Vector(_terrain.Position.X + 100, _terrain.Position.Y + 130, _terrain.Position.Z);

      _blackRanger = new StaticModel();
      _blackRanger.Meshes.Add(new Link<Texture, StaticMesh>(TextureManager.Get("BlackRanger"), StaticModelManager.Get("RedRanger")));
      _blackRanger.RotationAmmounts = new Vector(1, 1, 0);
      _blackRanger.Scale = new Vector(10, 10, 10);
      _blackRanger.Position = new Vector(_terrain.Position.X + 0, _terrain.Position.Y + 130, _terrain.Position.Z);

      _blueRanger = new StaticModel();
      _blueRanger.Meshes.Add(new Link<Texture, StaticMesh>(TextureManager.Get("BlueRanger"), StaticModelManager.Get("RedRanger")));
      _blueRanger.RotationAmmounts = new Vector(0, 1, 2);
      _blueRanger.Scale = new Vector(10, 10, 10);
      _blueRanger.Position = new Vector(_terrain.Position.X - 200, _terrain.Position.Y + 130, _terrain.Position.Z);

      _pinkRanger = new StaticModel();
      _pinkRanger.Meshes.Add(new Link<Texture, StaticMesh>(TextureManager.Get("PinkRanger"), StaticModelManager.Get("RedRanger")));
      _pinkRanger.RotationAmmounts = new Vector(0, 1, 0);
      _pinkRanger.Scale = new Vector(10, 10, 10);
      _pinkRanger.Position = new Vector(_terrain.Position.X - 100, _terrain.Position.Y + 130, _terrain.Position.Z);
    }

    public void Render()
    {
      // You will alter the projection matrix here. But I'm not finished with the TransformationManager class yet.
      TransformationManager.SetProjectionMatrix();

      Renderer.DrawStaticModel(_camera, _terrain);
      Renderer.DrawStaticModel(_camera, _redRanger);
      Renderer.DrawStaticModel(_camera, _blueRanger);
      Renderer.DrawStaticModel(_camera, _blackRanger);
      Renderer.DrawStaticModel(_camera, _pinkRanger);
      Renderer.DrawStaticModel(_camera, _yellowRanger);
    }

    public void Update(double elapsedTime)
    {
      CameraControls();

      if (InputManager.Rdown)
      {
        _redRanger.RotationAngle += 75;
        _yellowRanger.RotationAngle += 100;
        _blackRanger.RotationAngle += 200;
        _blueRanger.RotationAngle += 400;
        _pinkRanger.RotationAngle += 300;
      }

      _yellowRanger.RotationAngle++;
      _blackRanger.RotationAngle++;
      _blueRanger.RotationAngle++;
      _pinkRanger.RotationAngle++;
    }

    private void CameraControls()
    {
      // Camera position movement
      if (InputManager.Qdown)
        _camera.Move(_camera.Down, _camera.PositionSpeed);
      if (InputManager.Edown)
        _camera.Move(_camera.Up, _camera.PositionSpeed);
      if (InputManager.Adown)
        _camera.Move(_camera.Forward, _camera.PositionSpeed);
      if (InputManager.Wdown)
        _camera.Move(_camera.Backward, _camera.PositionSpeed);
      if (InputManager.Sdown)
        _camera.Move(_camera.Left, _camera.PositionSpeed);
      if (InputManager.Ddown)
        _camera.Move(_camera.Right, _camera.PositionSpeed);

      // Camera look angle adjustment
      // I haven't figured out angular camera changes yet
    }
  }
}