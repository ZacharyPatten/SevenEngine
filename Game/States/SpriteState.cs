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
    Sprite _sprites2;
    Sprite _sprites3;
    StaticModel _terrain;

    double spriteTimer = -10;

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
      _sprites.Scale.X = 50;
      _sprites.Scale.Y = 50;
      _sprites2 = new Sprite(TextureManager.Get("Menu"));
      _sprites2.Scale.X = 50;
      _sprites2.Scale.Y = 50;
      _sprites3 = new Sprite(TextureManager.Get("Menu"));
      _sprites3.Scale.X = 50;
      _sprites3.Scale.Y = 50;
    }

    public void Render()
    {
      Renderer.CurrentCamera = _camera;

      // You will alter the projection matrix here. But I'm not finished with the TransformationManager class yet.
      Renderer.SetProjectionMatrix();

      Renderer.DrawSprite(_sprites);
      Renderer.DrawSprite(_sprites2);
      Renderer.DrawSprite(_sprites3);

      Renderer.DrawStaticModel(_terrain);
    }

    public string Update(double elapsedTime)
    {
      CameraControls();

      spriteTimer += .05;
      _sprites.Position.X = (Trigonometry.Sin(spriteTimer) * 100) + 100;
      _sprites.Position.Y = (Trigonometry.Cos(spriteTimer) * 100) + 100;

      _sprites2.Position.X = (Math.Sin(spriteTimer) * 100) - 100;
      _sprites2.Position.Y = (Math.Cos(spriteTimer) * 100) - 100;

      _sprites3.Position.X = (Trigonometry.Sin(spriteTimer) * 100);
      _sprites3.Position.Y = (Trigonometry.Cos(spriteTimer) * 100);

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