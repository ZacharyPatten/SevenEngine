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
    Octree<StaticModel> _octree = new Octree<StaticModel>(0, 0, 0, 1000000, 10);

    Camera _camera;
    StaticModel _terrain;
    StaticModel _mountain;
    StaticModel _mountain2;
    StaticModel[] _rangers;
    StaticModel[] _tuxes;

    SkyBox _skybox;

    public PowerRangerDNA()
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

      string[] colors = new string[] { "YellowRanger", "RedRanger", "BlueRanger", "BlackRanger", "PinkRanger" };

      Random random = new Random();
      _rangers = new StaticModel[80];
      for (int i = 0; i < _rangers.Length; i++)
      {
        _rangers[i] = StaticModelManager.GetModel(colors[random.Next(0, 5)]);
        _rangers[i].Position.X = -100;
        _rangers[i].Position.Y = _terrain.Position.Y + 10;
        _rangers[i].Position.Z = i * 50;
        _rangers[i].Scale = new Vector(5, 5, 5);
        _rangers[i].Orientation = new Quaternion(0, 1, 0, 0);
        _rangers[i].Orientation.W = i * 2;
        _rangers[i].Id = "Ranger" + i;
        _octree.Add(_rangers[i]);
      }

      _tuxes = new StaticModel[80];
      for (int i = 0; i < _tuxes.Length; i++)
      {
        _tuxes[i] = StaticModelManager.GetModel("Tux");
        _tuxes[i].Position.X = 100;
        _tuxes[i].Position.Y = _terrain.Position.Y + 10;
        _tuxes[i].Position.Z = i * 50;
        _tuxes[i].Scale = new Vector(25, 25, 25);
        _tuxes[i].Orientation = new Quaternion(0, 1, 0, 0);
        _tuxes[i].Orientation.W = i * 2;
        _tuxes[i].Id = "Tux" + i;
        _octree.Add(_tuxes[i]);
      }

      for (int i = 0; i < _rangers.Length; i+=2)
      {
        _rangers[i].Meshes.Remove("Body");
        //_octree.Remove("Ranger" + i);
        _tuxes[i].Meshes.Remove("Body");
      }
    }

    public void Render()
    {
      Renderer.CurrentCamera = _camera;

      // You will alter the projection matrix here. But I'm not finished with the TransformationManager class yet.
      Renderer.SetProjectionMatrix();

      List<StaticModel> items = _octree.Get(-10000, -10000, -1000, 10000, 500, 10000);
      items.Foreach(RenderModel);

      /*if (InputManager.Keyboard.Vdown)
      {
        foreach (StaticModel model in _rangers)
          Renderer.AddStaticModel(model);
        Renderer.Render();
      }
      else
      {
        foreach (StaticModel model in _rangers)
          Renderer.DrawStaticModel(model);
      }*/
      Renderer.DrawSkybox(_skybox);
      Renderer.DrawStaticModel(_terrain);
      Renderer.DrawStaticModel(_mountain);
      Renderer.DrawStaticModel(_mountain2);
    }

    private void RenderModel(StaticModel model)
    {
      Renderer.DrawStaticModel(model);
    }

    public string Update(float elapsedTime)
    {
      CameraControls();
      foreach (StaticModel model in _rangers)
        model.Orientation.W += 3;
      foreach (StaticModel model in _tuxes)
        model.Orientation.W += 3;
      _skybox.Position.X = _camera.Position.X;
      _skybox.Position.Y = _camera.Position.Y;
      _skybox.Position.Z = _camera.Position.Z;
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
  }
}