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
  public class MultipleModelState : IGameObject
  {
    KeyboardDevice _keyboard;
    Camera _camera;
    Renderer _renderer;

    Matrix4 _matrix = Matrix4.Identity;

    StaticModel _terrain;
    //StaticModel _yoda;
    StaticModel _redRanger;
    StaticModel _pinkRanger;
    StaticModel _blueRanger;
    StaticModel _blackRanger;
    StaticModel _yellowRanger;

    Sprite _sprite;

    int momentum;
    int lastmomentum;
    int mousex = 0;
    int mousey = 0;
    double time = 0;

    public MultipleModelState(KeyboardDevice keyboard, StaticModelManager staticModelManager, TextureManager textureManager)
    {
      _keyboard = keyboard;

      _renderer = new Renderer();

      Vector temp = new Vector(6, 6, 5);
      _camera = new Camera();

      //_camera.Up = new Vector(0, 1, 0);
      //_camera.Forward = new Vector(0, 0, 1);
      //_camera.Position = new Vector(0, 200, 1000);

      _terrain = new StaticModel();
      _terrain.Meshes.Add(new Tuple<Texture, StaticMesh>(textureManager.Get("grass"), staticModelManager.Get("terrain")));
      _terrain.Scale = new Vector(500, 20, 500);
      _terrain.RotationAmmounts = new Vector(0, 0, 0);
      _terrain.Position = new Vector(0, 0, 0);

      _sprite = new Sprite(textureManager.Get("YellowRanger"));

      //_terrain = new StaticModel();
      //_terrain.Meshes.Add(new Tuple<Texture, StaticMesh>(textureManager.Get("grass"), staticModelManager.Get("grass")));
      //_terrain.Scale = new Vector(20, 20, 20);
      //_terrain.RotationAmmounts = new Vector(0, 0, 0);
      //_terrain.Position = new Vector(0, 0, 0);

      //_yoda = new StaticModel();
      //_yoda.Meshes.Add(new Tuple<Texture, StaticMesh>(textureManager.Get("yoda"), staticModelManager.Get("yoda")));
      //_yoda.RotationAmmounts = new Vector(0, 1, 0);
      //_yoda.Scale = new Vector(100, 100, 100);
      //_yoda.Position = new Vector(_terrain.Position.X - 100, _terrain.Position.Y + 130, _terrain.Position.Z + 200);

      _redRanger = new StaticModel();
      _redRanger.Meshes.Add(new Tuple<Texture, StaticMesh>(textureManager.Get("RedRanger"), staticModelManager.Get("RedRanger")));
      _redRanger.RotationAmmounts = new Vector(0, 1, 0);
      _redRanger.RotationAngle = 180f;
      _redRanger.Scale = new Vector(5, 5, 5);
      _redRanger.Position = new Vector(_terrain.Position.X + 200, _terrain.Position.Y + 130, _terrain.Position.Z);

      _yellowRanger = new StaticModel();
      _yellowRanger.Meshes.Add(new Tuple<Texture, StaticMesh>(textureManager.Get("YellowRanger"), staticModelManager.Get("RedRanger")));
      _yellowRanger.RotationAmmounts = new Vector(0, 1, 1);
      _yellowRanger.Scale = new Vector(10, 10, 10);
      _yellowRanger.Position = new Vector(_terrain.Position.X + 100, _terrain.Position.Y + 130, _terrain.Position.Z);

      _blackRanger = new StaticModel();
      _blackRanger.Meshes.Add(new Tuple<Texture, StaticMesh>(textureManager.Get("BlackRanger"), staticModelManager.Get("RedRanger")));
      _blackRanger.RotationAmmounts = new Vector(1, 1, 0);
      _blackRanger.Scale = new Vector(10, 10, 10);
      _blackRanger.Position = new Vector(_terrain.Position.X + 0, _terrain.Position.Y + 130, _terrain.Position.Z);

      _blueRanger = new StaticModel();
      _blueRanger.Meshes.Add(new Tuple<Texture, StaticMesh>(textureManager.Get("BlueRanger"), staticModelManager.Get("RedRanger")));
      _blueRanger.RotationAmmounts = new Vector(0, 1, 2);
      _blueRanger.Scale = new Vector(10, 10, 10);
      _blueRanger.Position = new Vector(_terrain.Position.X - 200, _terrain.Position.Y + 130, _terrain.Position.Z);

      _pinkRanger = new StaticModel();
      _pinkRanger.Meshes.Add(new Tuple<Texture, StaticMesh>(textureManager.Get("PinkRanger"), staticModelManager.Get("RedRanger")));
      _pinkRanger.RotationAmmounts = new Vector(0, 1, 0);
      _pinkRanger.Scale = new Vector(10, 10, 10);
      _pinkRanger.Position = new Vector(_terrain.Position.X - 100, _terrain.Position.Y + 130, _terrain.Position.Z);
    }

    public void Render()
    {
      GL.MatrixMode(MatrixMode.Projection);
      GL.LoadIdentity();
      Matrix4 perspective = OpenTK.Matrix4.CreatePerspectiveFieldOfView(_camera.FieldOfView, (float)800 / (float)600, .1f, 10000f);
      GL.LoadMatrix(ref perspective);


      GL.Disable(EnableCap.CullFace);
      _renderer.DrawStaticModel(_camera, _terrain);
      GL.Enable(EnableCap.CullFace);
      
      //_renderer.DrawStaticModel(_yoda);
      _renderer.DrawStaticModel(_camera, _redRanger);
      _renderer.DrawStaticModel(_camera, _blueRanger);
      _renderer.DrawStaticModel(_camera, _blackRanger);
      _renderer.DrawStaticModel(_camera, _pinkRanger);
      _renderer.DrawStaticModel(_camera, _yellowRanger);

      //_renderer.DrawSprite(_sprite);
      //_renderer.Render();
    }

    public void Update(double elapsedTime)
    {
      time = elapsedTime;
      if (_keyboard[OpenTK.Input.Key.Number1])
      {
        if (momentum == 0)
        {
          momentum = 50;
        }
      }
      if (_keyboard[OpenTK.Input.Key.Number3])
      {
        _redRanger.RotationAngle += 20;
      }
      if (momentum != 0)
      {
        _camera.Position = new Vector(_camera.Position.X, _camera.Position.Y + momentum, _camera.Position.Z);
        momentum--;
        if (momentum == 0) lastmomentum = -50;
      }
      if (lastmomentum != 0)
      {
        _camera.Position = new Vector(_camera.Position.X, _camera.Position.Y + lastmomentum, _camera.Position.Z);
        lastmomentum++;
      }

      // Camera Controls
      if (_keyboard[OpenTK.Input.Key.A])
        _camera.Position.X -= 5;
      if (_keyboard[OpenTK.Input.Key.W])
        _camera.Position.Z -= 5;
      if (_keyboard[OpenTK.Input.Key.S])
        _camera.Position.Z += 5;
      if (_keyboard[OpenTK.Input.Key.D])
        _camera.Position.X += 5;
      if (_keyboard[OpenTK.Input.Key.Q])
        _camera.Position.Y -= 5;
      if (_keyboard[OpenTK.Input.Key.E])
        _camera.Position.Y += 5;
      if (_keyboard[OpenTK.Input.Key.J])
        _camera.RotationY += 2;
      if (_keyboard[OpenTK.Input.Key.L])
        _camera.RotationY -= 2;
      if (_keyboard[OpenTK.Input.Key.I])
        _camera.RotationX += 2;
      if (_keyboard[OpenTK.Input.Key.K])
        _camera.RotationX -= 2;
      if (_keyboard[OpenTK.Input.Key.U])
        _camera.RotationZ -= 2;
      if (_keyboard[OpenTK.Input.Key.O])
        _camera.RotationZ += 2;

      if (_keyboard[OpenTK.Input.Key.A] && (_keyboard[OpenTK.Input.Key.ShiftLeft] || _keyboard[OpenTK.Input.Key.ShiftRight]))
        _camera.Position.X -= 15;
      if (_keyboard[OpenTK.Input.Key.W] && (_keyboard[OpenTK.Input.Key.ShiftLeft] || _keyboard[OpenTK.Input.Key.ShiftRight]))
        _camera.Position.Z -= 15;
      if (_keyboard[OpenTK.Input.Key.S] && (_keyboard[OpenTK.Input.Key.ShiftLeft] || _keyboard[OpenTK.Input.Key.ShiftRight]))
        _camera.Position.Z += 15;
      if (_keyboard[OpenTK.Input.Key.D] && (_keyboard[OpenTK.Input.Key.ShiftLeft] || _keyboard[OpenTK.Input.Key.ShiftRight]))
        _camera.Position.X += 15;
      if (_keyboard[OpenTK.Input.Key.Q] && (_keyboard[OpenTK.Input.Key.ShiftLeft] || _keyboard[OpenTK.Input.Key.ShiftRight]))
        _camera.Position.Y -= 15;
      if (_keyboard[OpenTK.Input.Key.E] && (_keyboard[OpenTK.Input.Key.ShiftLeft] || _keyboard[OpenTK.Input.Key.ShiftRight]))
        _camera.Position.Y += 15;
      
      _yellowRanger.RotationAngle++;
      _blackRanger.RotationAngle++;
      _blueRanger.RotationAngle++;
      _pinkRanger.RotationAngle++;
    }
  }
}