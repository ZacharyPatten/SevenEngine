using System;

using SevenEngine;
using SevenEngine.DataStructures;
using SevenEngine.Imaging;
using SevenEngine.Models;
using SevenEngine.Mathematics;

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

      _terrain = StaticModelManager.GetModel("Terrain");
      _terrain.Scale = new Vector(500, 20, 500);
      _terrain.RotationAmmounts = new Vector(0, 0, 0);
      _terrain.Position = new Vector(0, 0, 0);

      string[] colors = new string[] { "YellowRanger", "RedRanger", "BlueRanger", "BlackRanger", "PinkRanger" };

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

      for (int i = 0; i < _rangers.Length; i+=2)
      {
        _rangers[i].Meshes.Remove("Body");
      }
    }

    public void Render()
    {
      Renderer.CurrentCamera = _camera;

      // You will alter the projection matrix here. But I'm not finished with the TransformationManager class yet.
      Renderer.SetProjectionMatrix();

      if (InputManager.Keyboard.Vdown)
      {
        foreach (StaticModel model in _rangers)
          Renderer.AddStaticModel(model);
        Renderer.Render();
      }
      else
      {
        foreach (StaticModel model in _rangers)
          Renderer.DrawStaticModel(model);
      }

      Renderer.DrawStaticModel(_terrain);
    }

    public string Update(double elapsedTime)
    {
      CameraControls();
      foreach (StaticModel model in _rangers)
        model.RotationAngle += 3;
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