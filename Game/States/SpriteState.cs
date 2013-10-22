using System;

using SevenEngine;
using SevenEngine.DataStructures;
using SevenEngine.Imaging;
using SevenEngine.Models;
using SevenEngine.Mathematics;

namespace Game.States
{
  public class SpriteState : IGameState
  {
    Camera _camera;
    Sprite _sprites;
    StaticModel _terrain;

    double spriteX = 0;
    double spriteY = 0;

    public SpriteState()
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

      _sprites = new Sprite(TextureManager.Get("Menu"));
      _sprites.Scale.X = 100;
      _sprites.Scale.Y = 100;
    }

    public void Render()
    {
      Renderer.CurrentCamera = _camera;

      // You will alter the projection matrix here. But I'm not finished with the TransformationManager class yet.
      Renderer.SetProjectionMatrix();

      Renderer.DrawSprite(_sprites);

      Renderer.DrawStaticModel(_terrain);
    }

    public string Update(double elapsedTime)
    {
      CameraControls();

      spriteX += elapsedTime * 2;
      spriteY += elapsedTime * 2;
      _sprites.Position.X = Math.Sin(spriteX) * 100;
      _sprites.Position.Y = Math.Sin(spriteY) * 100;

      // You can return whatever you like, but you should use the return value to determine state changes
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