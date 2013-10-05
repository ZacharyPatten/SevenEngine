using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Engine;
using Engine.Models;
using Engine.Textures;
using Game;

using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;

namespace Game.States
{
  public class SkyBoxTesting : IGameObject
  {
    KeyboardDevice _keyboard;
    Camera _camera;
    Renderer _renderer;

    StaticModel _skybox;


    public SkyBoxTesting(KeyboardDevice keyboard, StaticModelManager staticModelManager, TextureManager textureManager)
    {
      _keyboard = keyboard;

      _renderer = new Renderer();
      
      _camera = new Camera();
      //_camera.Scale = new Vector3d(1, 1, 1);
      _camera.Position = new Vector(0, 200, 1000);

      //_skybox = new SkyBox();
      //_skybox.Up = textureManager.Get("NightWalkerTop");
      //_skybox.Back = textureManager.Get("NightWalkerBack");
      //_skybox.Left = textureManager.Get("NightWalkerLeft");
      //_skybox.Right = textureManager.Get("NightWalkerRight");
      //_skybox.Front = textureManager.Get("NightWalkerFront");
      //_skybox.Scale = new Vector(100, 100, 100);

      _skybox = new StaticModel();
      _skybox.Meshes.Add(new Tuple<Texture, StaticMesh>(textureManager.Get("SkyBox"), staticModelManager.Get("SkyBox")));
      _skybox.Scale = new Vector(100, 100, 100);
    }

    public void Render()
    {
      GL.MatrixMode(MatrixMode.Projection);
      GL.Translate(-_camera.Position.X, -_camera.Position.Y, -_camera.Position.Z);
      //GL.Rotate(_camera.RotationAngle, _camera.RotationAmmounts);

      _renderer.DrawStaticModel(_camera, _skybox);
    }

    public void Update(double elapsedTime)
    {
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
    }
  }
}
