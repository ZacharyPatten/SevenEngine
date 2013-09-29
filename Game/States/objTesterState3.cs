using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Engine;
using Engine.Textures;
using Engine.Models;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Game.States
{
  public class objTesterState3 : IGameObject
  {
    Renderer _renderer;
    //InputManager _inputManager;
    KeyboardDevice _keyBoard;
    MouseDevice _mouse;
    StaticModel _terrain;
    StaticModel _character;
    Camera _camera;

    public objTesterState3(//InputManager inputManager,
      KeyboardDevice keyBoard,
      MouseDevice mouse,
      StaticModelManager staticModelManager,
      TextureManager textureManager)
    {
      _renderer = new Renderer();
      _keyBoard = keyBoard;
      _mouse = mouse;

      _camera = new Camera();
      _camera.Scale = new Vector3d(1, 1, 1);

      //_inputManager = inputManager;

      _terrain = staticModelManager.Get("grass");
      _terrain.Scale = new Vector3d(20, 20, 20);
      _terrain.RotationAmmounts = new Vector3d(1, 0, 0);

      _character = staticModelManager.Get("yoda");
      _character.Texture = textureManager.Get("yoda");
      _character.RotationAmmounts = new Vector3d(1, 0, 0);
      _character.Scale = new Vector3d(100, 100, 100);
      _character.Position = new Vector3d(0, 200, 0);
    }

    public void Render()
    {
      //GL.MatrixMode(MatrixMode.Projection);
      //GL.Translate(_camera.Position);

      GL.MatrixMode(MatrixMode.Projection);
      GL.Translate(_camera.Position);
      GL.Scale(_camera.Scale);
      //GL.Rotate(_camera.RotationAngle, _camera.RotationAmmounts);

      _renderer.DrawStaticModel(_terrain);
      _renderer.DrawStaticModel(_character);
    }

    public void Update(double elapsedTime)
    {
      _terrain.RotationAngle++;
      _character.RotationAngle++;

      if (_keyBoard[OpenTK.Input.Key.A])
        _camera.Position = new Vector3d(_camera.Position.X - 5, _camera.Position.Y, _camera.Position.Z);
      if (_keyBoard[OpenTK.Input.Key.W])
        _camera.Position = new Vector3d(_camera.Position.X, _camera.Position.Y, _camera.Position.Z + 5);
      if (_keyBoard[OpenTK.Input.Key.S])
        _camera.Position = new Vector3d(_camera.Position.X, _camera.Position.Y, _camera.Position.Z - 5);
      if (_keyBoard[OpenTK.Input.Key.D])
        _camera.Position = new Vector3d(_camera.Position.X + 5, _camera.Position.Y, _camera.Position.Z);
      if (_keyBoard[OpenTK.Input.Key.Q])
        _camera.Position = new Vector3d(_camera.Position.X, _camera.Position.Y - 5, _camera.Position.Z);
      if (_keyBoard[OpenTK.Input.Key.E])
        _camera.Position = new Vector3d(_camera.Position.X, _camera.Position.Y + 5, _camera.Position.Z);
    }
  }
}