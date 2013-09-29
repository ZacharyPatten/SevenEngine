using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Engine;
using Engine.Textures;
using Engine.Models;

using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;

namespace Game.States
{
  public class objTesterState3 : IGameObject
  {
    KeyboardDevice _keyboard;
    Camera _camera;
    Renderer _renderer;
    StaticModel _terrain;
    StaticModel _redRanger;
    StaticModel _pinkRanger;
    StaticModel _blueRanger;
    StaticModel _blackRanger;
    StaticModel _yellowRanger;
    //StaticModel _yoda;
    //StaticModel _thief;

    public objTesterState3(KeyboardDevice keyboard, StaticModelManager staticModelManager, TextureManager textureManager)
    {
      _keyboard = keyboard;

      _renderer = new Renderer();
      
      _camera = new Camera();
      _camera.Scale = new Vector3d(1, 1, 1);
      _camera.Position = new Vector3d(50, 0, 0);

      _terrain = staticModelManager.Get("grass");
      _terrain.Scale = new Vector3d(20, 20, 20);
      _terrain.RotationAmmounts = new Vector3d(1, 0, 0);
      _terrain.Position = new Vector3d(0, -100, 0);

      /*_yoda = staticModelManager.Get("yoda");
      _yoda.Texture = textureManager.Get("yoda");
      _yoda.RotationAmmounts = new Vector3d(0, 1, 0);
      _yoda.Scale = new Vector3d(100, 100, 100);
      _yoda.Position = new Vector3d(_terrain.Position.X - 100, _terrain.Position.Y + 130, _terrain.Position.Z);*/

      _redRanger = staticModelManager.Get("RedRanger");
      _redRanger.Texture = textureManager.Get("RedRanger");
      _redRanger.RotationAmmounts = new Vector3d(0, 1, 0);
      _redRanger.Scale = new Vector3d(10, 10, 10);
      _redRanger.Position = new Vector3d(_terrain.Position.X + 200, _terrain.Position.Y + 130, _terrain.Position.Z);

      _yellowRanger = staticModelManager.Get("RedRanger");
      _yellowRanger.Texture = textureManager.Get("YellowRanger");
      _yellowRanger.RotationAmmounts = new Vector3d(0, 1, 0);
      _yellowRanger.Scale = new Vector3d(10, 10, 10);
      _yellowRanger.Position = new Vector3d(_terrain.Position.X + 100, _terrain.Position.Y + 130, _terrain.Position.Z);

      _blackRanger = staticModelManager.Get("RedRanger");
      _blackRanger.Texture = textureManager.Get("BlackRanger");
      _blackRanger.RotationAmmounts = new Vector3d(0, 1, 0);
      _blackRanger.Scale = new Vector3d(10, 10, 10);
      _blackRanger.Position = new Vector3d(_terrain.Position.X + 0, _terrain.Position.Y + 130, _terrain.Position.Z);

      _blueRanger = staticModelManager.Get("RedRanger");
      _blueRanger.Texture = textureManager.Get("BlueRanger");
      _blueRanger.RotationAmmounts = new Vector3d(0, 1, 0);
      _blueRanger.Scale = new Vector3d(10, 10, 10);
      _blueRanger.Position = new Vector3d(_terrain.Position.X - 100, _terrain.Position.Y + 130, _terrain.Position.Z);

      _pinkRanger = staticModelManager.Get("RedRanger");
      _pinkRanger.Texture = textureManager.Get("PinkRanger");
      _pinkRanger.RotationAmmounts = new Vector3d(0, 1, 0);
      _pinkRanger.Scale = new Vector3d(10, 10, 10);
      _pinkRanger.Position = new Vector3d(_terrain.Position.X - 200, _terrain.Position.Y + 130, _terrain.Position.Z);

      /*_thief = staticModelManager.Get("thief");
      _thief.Texture = textureManager.Get("thief");
      _thief.RotationAmmounts = new Vector3d(0, 1, 0);
      _thief.Scale = new Vector3d(100, 100, 100);
      _thief.Position = new Vector3d(_terrain.Position.X, _terrain.Position.Y + 130, _terrain.Position.Z);*/
    }

    public void Render()
    {
      GL.MatrixMode(MatrixMode.Projection);
      GL.Translate(-_camera.Position);
      GL.Rotate(_camera.RotationAngle, _camera.RotationAmmounts);

      _renderer.DrawStaticModel(_terrain);
      //_renderer.DrawStaticModel(_yoda);
      _renderer.DrawStaticModel(_redRanger);
      _renderer.DrawStaticModel(_blueRanger);
      _renderer.DrawStaticModel(_blackRanger);
      _renderer.DrawStaticModel(_pinkRanger);
      _renderer.DrawStaticModel(_yellowRanger);
      //_renderer.DrawStaticModel(_thief);
    }

    public void Update(double elapsedTime)
    {
      if (_keyboard[OpenTK.Input.Key.A])
      {
        _camera.Position = new Vector3d(_camera.Position.X - 5, _camera.Position.Y, _camera.Position.Z);
        //_character.Position = new Vector3d(_character.Position.X - 5, _character.Position.Y, _character.Position.Z);
      }
      if (_keyboard[OpenTK.Input.Key.W])
      {
        _camera.Position = new Vector3d(_camera.Position.X, _camera.Position.Y, _camera.Position.Z - 5);
        //_character.Position = new Vector3d(_character.Position.X, _character.Position.Y, _character.Position.Z - 5);
      }
      if (_keyboard[OpenTK.Input.Key.S])
      {
        _camera.Position = new Vector3d(_camera.Position.X, _camera.Position.Y, _camera.Position.Z + 5);
        //_character.Position = new Vector3d(_character.Position.X, _character.Position.Y, _character.Position.Z + 5);
      }
      if (_keyboard[OpenTK.Input.Key.D])
      {
        _camera.Position = new Vector3d(_camera.Position.X + 5, _camera.Position.Y, _camera.Position.Z);
        //_character.Position = new Vector3d(_character.Position.X + 5, _character.Position.Y, _character.Position.Z);
      }
      if (_keyboard[OpenTK.Input.Key.Q])
      {
        _camera.Position = new Vector3d(_camera.Position.X, _camera.Position.Y - 5, _camera.Position.Z);
      }
      if (_keyboard[OpenTK.Input.Key.E])
      {
        _camera.Position = new Vector3d(_camera.Position.X, _camera.Position.Y + 5, _camera.Position.Z);
      }
      if (_keyboard[OpenTK.Input.Key.Number1])
      {
        _camera.RotationAngle = 1;
        _camera.RotationAmmounts = new Vector3d(_camera.RotationAmmounts.X + 1, _camera.RotationAmmounts.Y + 1, _camera.RotationAmmounts.Z + 1);
      }
      if (_keyboard[OpenTK.Input.Key.Number3])
      {
        _camera.RotationAngle = 1;
        _camera.RotationAmmounts = new Vector3d(_camera.RotationAmmounts.X - 5, _camera.RotationAmmounts.Y - 1, _camera.RotationAmmounts.Z - 1);
      }
      //_terrain.RotationAngle++;
      _redRanger.RotationAngle++;
      _yellowRanger.RotationAngle++;
      _blackRanger.RotationAngle++;
      _blueRanger.RotationAngle++;
      _pinkRanger.RotationAngle++;
      //_yoda.RotationAngle++;
      //_thief.RotationAngle++;
    }
  }
}