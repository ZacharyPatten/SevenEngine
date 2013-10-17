using System;

using Engine;
using Engine.DataStructures;
using Engine.Imaging;
using Engine.Models;
using Engine.Mathematics;

namespace Game.States
{
  public class PowerRangerDNA : IGameState
  {
    Camera _camera;
    StaticModel _terrain;
    StaticModel[] _rangers;

    public PowerRangerDNA()
    {
      _camera = new Camera();
      _camera.PositionSpeed = 5;
      _camera.Move(_camera.Up, 400);
      _camera.Move(_camera.Backward, 1500);
      _camera.Move(_camera.Backward, 300);
      TransformationManager.CurrentCamera = _camera;

      _terrain = StaticModelManager.GetModel("Terrain");
      _terrain.Scale = new Vector(500, 20, 500);
      _terrain.RotationAmmounts = new Vector(0, 0, 0);
      _terrain.Position = new Vector(0, 0, 0);

      string[] colors = new string[] { "RedRanger", "YellowRanger", "BlueRanger", "BlackRanger", "PinkRanger"};

      Random random = new Random();
      _rangers = new StaticModel[300];
      for (int i = 0; i < _rangers.Length; i++)
      {
        _rangers[i] = StaticModelManager.GetModel(colors[random.Next(0, 5)]);
        _rangers[i].Position.X = i * 10 - 500;
        _rangers[i].Position.Y = _terrain.Position.Y + 130;
        _rangers[i].Position.Z = i * 10 - 530;
        _rangers[i].Scale = new Vector(5, 5, 5);
        _rangers[i].RotationAmmounts = new Vector(0, 1, 0);
        _rangers[i].RotationAngle = i * 2;
      }
    }

    public void Render()
    {
      // You will alter the projection matrix here. But I'm not finished with the TransformationManager class yet.
      TransformationManager.SetProjectionMatrix();

      if (InputManager.Vdown)
      {
        foreach (StaticModel model in _rangers)
          Renderer.AddStaticModel(model);
        Renderer.Render();
      }
      else
      {
        foreach (StaticModel model in _rangers)
          Renderer.DrawStaticModel(_camera, model);
      }
      Renderer.DrawStaticModel(_camera, _terrain);
    }

    public void Update(double elapsedTime)
    {
      CameraControls();
      foreach (StaticModel model in _rangers)
        model.RotationAngle+=5;
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