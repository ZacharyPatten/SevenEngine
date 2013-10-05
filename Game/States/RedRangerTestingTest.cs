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
  public class RedRangerTestingTest : IGameObject
  {
    KeyboardDevice _keyboard;
    Camera _camera;
    Renderer _renderer;

    StaticModel _terrain;
    StaticModel _rangerHead;
    StaticModel _rangerTorso;
    StaticModel _rangerArmLeft;
    StaticModel _rangerArmRight;
    StaticModel _rangerLegLeft;
    StaticModel _rangerLegRight;

    double _time = 0;

    public RedRangerTestingTest(KeyboardDevice keyboard, StaticModelManager staticModelManager, TextureManager textureManager)
    {
      _keyboard = keyboard;

      _renderer = new Renderer();
      
      _camera = new Camera();
      _camera.Position = new Vector(0, 200, 1000);

      _terrain = new StaticModel();
      _terrain.Meshes.Add(new Tuple<Texture, StaticMesh>(textureManager.Get("grass"), staticModelManager.Get("grass")));
      _terrain.Scale = new Vector(20, 20, 20);
      _terrain.RotationAmmounts = new Vector(0, 0, 0);
      _terrain.Position = new Vector(0, 0, 0);
      
      _rangerHead = new StaticModel();
      _rangerHead.Meshes.Add(new Tuple<Texture, StaticMesh>(textureManager.Get("BlackRanger"), staticModelManager.Get("RangerHead")));
      _rangerHead.RotationAmmounts = new Vector(0, 1, 0);
      _rangerHead.Scale = new Vector(10, 10, 10);
      _rangerHead.Position = new Vector(_terrain.Position.X - 200, _terrain.Position.Y + 130, _terrain.Position.Z);

      _rangerLegLeft = new StaticModel();
      _rangerLegLeft.Meshes.Add(new Tuple<Texture, StaticMesh>(textureManager.Get("BlackRanger"), staticModelManager.Get("RangerLegLeft")));
      _rangerLegLeft.RotationAmmounts = new Vector(0, 1, 0);
      _rangerLegLeft.Scale = new Vector(10, 10, 10);
      _rangerLegLeft.Position = new Vector(_terrain.Position.X - 200, _terrain.Position.Y + 130, _terrain.Position.Z);

      _rangerLegRight = new StaticModel();
      _rangerLegRight.Meshes.Add(new Tuple<Texture, StaticMesh>(textureManager.Get("BlackRanger"), staticModelManager.Get("RangerLegRight")));
      _rangerLegRight.RotationAmmounts = new Vector(1, 0, 0);
      _rangerLegRight.Scale = new Vector(10, 10, 10);
      _rangerLegRight.Position = new Vector(_terrain.Position.X - 200, _terrain.Position.Y + 130, _terrain.Position.Z);

      _rangerTorso = new StaticModel();
      _rangerTorso.Meshes.Add(new Tuple<Texture, StaticMesh>(textureManager.Get("BlackRanger"), staticModelManager.Get("RangerTorso")));
      _rangerTorso.RotationAmmounts = new Vector(0, 1, 0);
      _rangerTorso.Scale = new Vector(10, 10, 10);
      _rangerTorso.Position = new Vector(_terrain.Position.X - 200, _terrain.Position.Y + 130, _terrain.Position.Z);

      _rangerArmLeft = new StaticModel();
      _rangerArmLeft.Meshes.Add(new Tuple<Texture, StaticMesh>(textureManager.Get("BlackRanger"), staticModelManager.Get("RangerArmLeft")));
      _rangerArmLeft.RotationAmmounts = new Vector(0, 1, 0);
      _rangerArmLeft.Scale = new Vector(10, 10, 10);
      _rangerArmLeft.Position = new Vector(_terrain.Position.X - 200, _terrain.Position.Y + 130, _terrain.Position.Z);

      _rangerArmRight = new StaticModel();
      _rangerArmRight.Meshes.Add(new Tuple<Texture, StaticMesh>(textureManager.Get("BlackRanger"), staticModelManager.Get("RangerArmRight")));
      _rangerArmRight.RotationAmmounts = new Vector(1, 0, 0);
      _rangerArmRight.Scale = new Vector(10, 10, 10);
      _rangerArmRight.Position = new Vector(_terrain.Position.X - 200, _terrain.Position.Y + 130, _terrain.Position.Z);
    }

    public void Render()
    {
      GL.MatrixMode(MatrixMode.Projection);
      GL.Translate(-_camera.Position.X, -_camera.Position.Y, -_camera.Position.Z);

      GL.MatrixMode(MatrixMode.Modelview);
      _renderer.DrawStaticModel(_camera, _terrain);
      _renderer.DrawStaticModel(_camera, _rangerHead);
      _renderer.DrawStaticModel(_camera, _rangerTorso);
      _renderer.DrawStaticModel(_camera, _rangerArmLeft);
      _renderer.DrawStaticModel(_camera, _rangerArmRight);
      _renderer.DrawStaticModel(_camera, _rangerLegLeft);
      _renderer.DrawStaticModel(_camera, _rangerLegRight);
    }

    public void Update(double elapsedTime)
    {
      if (_time == 0) _time = elapsedTime;
      if (_keyboard[OpenTK.Input.Key.A])
      {
        _camera.Position = new Vector(_camera.Position.X - 5, _camera.Position.Y, _camera.Position.Z);
        //_character.Position = new Vector3d(_character.Position.X - 5, _character.Position.Y, _character.Position.Z);
      }
      if (_keyboard[OpenTK.Input.Key.W])
      {
        _camera.Position = new Vector(_camera.Position.X, _camera.Position.Y, _camera.Position.Z - 5);
        //_character.Position = new Vector3d(_character.Position.X, _character.Position.Y, _character.Position.Z - 5);
      }
      if (_keyboard[OpenTK.Input.Key.S])
      {
        _camera.Position = new Vector(_camera.Position.X, _camera.Position.Y, _camera.Position.Z + 5);
        //_character.Position = new Vector3d(_character.Position.X, _character.Position.Y, _character.Position.Z + 5);
      }
      if (_keyboard[OpenTK.Input.Key.D])
      {
        _camera.Position = new Vector(_camera.Position.X + 5, _camera.Position.Y, _camera.Position.Z);
        //_character.Position = new Vector3d(_character.Position.X + 5, _character.Position.Y, _character.Position.Z);
      }
      if (_keyboard[OpenTK.Input.Key.Q])
      {
        _camera.Position = new Vector(_camera.Position.X, _camera.Position.Y - 5, _camera.Position.Z);
      }
      if (_keyboard[OpenTK.Input.Key.E])
      {
        _camera.Position = new Vector(_camera.Position.X, _camera.Position.Y + 5, _camera.Position.Z);
      }
      //_terrain.RotationAngle++;
      //_redRanger.RotationAngle++;
      _rangerHead.RotationAngle++;
      _rangerArmRight.RotationAngle++;
    }
  }
}