using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

using System.IO;

using Engine.Models;

namespace Engine
{
  public class BufferExampleState : IGameObject
  {
    Renderer _renderer;
    RigidBodyPartModel _subModel;
    RigidBodyPartModel _subModel2;
    RigidBodyPartModel _subModel3;
    //InputManager _input;

    public BufferExampleState(TextureManager textureManager)//, InputManager input)
    {
      // Initialize the renderer for this instance
      _renderer = new Renderer();

      // Initialize the input for user controls of this state
      //_input = input;
      
      _subModel = new RigidBodyPartModel(textureManager, "guy",
        new float[] {
          // left
          -5, -5, -5,    -5, -5, 5,    -5, 5, -5,
          -5, -5, 5,    -5, 5, -5,    -5, 5, 5,
          
          // top
          -5, 5, -5,    -5, 5, 5,    5, 5, -5,
          -5, 5, 5,    5, 5, -5,    5, 5, 5,
        
          // right
          5, -5, -5,    5, -5, 5,    5, 5, -5,
          5, -5, 5,    5, 5, -5,    5, 5, 5,
        
          // bottom
          -5, -5, -5,    -5, -5, 5,    5, -5, -5,
          -5, -5, 5,    5, -5, -5,    5, -5, 5,

          // front
          -5, 5, 5,    -5, -5, 5,    5, 5, 5,
          -5, -5, 5,    5, 5, 5,    5, -5, 5,

          // back
          -5, 5, -5,    -5, -5, -5,    5, 5, -5,
          -5, -5, -5,    5, 5, -5,    5, -5, -5
        },
        null,
        new float[] {
          .25f, .25f, .75f, .25f, .25f, .75f,
          .75f, .25f, .25f, .75f, .75f, .75f,
          .25f, .25f, .75f, .25f, .25f, .75f,
          .75f, .25f, .25f, .75f, .75f, .75f,
          .25f, .25f, .75f, .25f, .25f, .75f,
          .75f, .25f, .25f, .75f, .75f, .75f,
          .25f, .25f, .75f, .25f, .25f, .75f,
          .75f, .25f, .25f, .75f, .75f, .75f,
          .25f, .25f, .75f, .25f, .25f, .75f,
          .75f, .25f, .25f, .75f, .75f, .75f,
          .25f, .25f, .75f, .25f, .25f, .75f,
          .75f, .25f, .25f, .75f, .75f, .75f},
          null,
          null);

      _subModel2 = new RigidBodyPartModel(textureManager, "toy",
        new float[] {
          // left
          -5, -5, -5,    -5, -5, 5,    -5, 5, -5,
          -5, -5, 5,    -5, 5, -5,    -5, 5, 5,
          
          // top
          -5, 5, -5,    -5, 5, 5,    5, 5, -5,
          -5, 5, 5,    5, 5, -5,    5, 5, 5,
        
          // right
          5, -5, -5,    5, -5, 5,    5, 5, -5,
          5, -5, 5,    5, 5, -5,    5, 5, 5,
        
          // bottom
          -5, -5, -5,    -5, -5, 5,    5, -5, -5,
          -5, -5, 5,    5, -5, -5,    5, -5, 5,

          // front
          -5, 5, 5,    -5, -5, 5,    5, 5, 5,
          -5, -5, 5,    5, 5, 5,    5, -5, 5,

          // back
          -5, 5, -5,    -5, -5, -5,    5, 5, -5,
          -5, -5, -5,    5, 5, -5,    5, -5, -5
        },
        null,
        new float[] {
          .25f, .25f, .75f, .25f, .25f, .75f,
          .75f, .25f, .25f, .75f, .75f, .75f,
          .25f, .25f, .75f, .25f, .25f, .75f,
          .75f, .25f, .25f, .75f, .75f, .75f,
          .25f, .25f, .75f, .25f, .25f, .75f,
          .75f, .25f, .25f, .75f, .75f, .75f,
          .25f, .25f, .75f, .25f, .25f, .75f,
          .75f, .25f, .25f, .75f, .75f, .75f,
          .25f, .25f, .75f, .25f, .25f, .75f,
          .75f, .25f, .25f, .75f, .75f, .75f,
          .25f, .25f, .75f, .25f, .25f, .75f,
          .75f, .25f, .25f, .75f, .75f, .75f},
          null,
          null);

      _subModel3 = new RigidBodyPartModel(textureManager, "guy",
        new float[] {
          -1, -1, -1,
          -1, -1, 1,
          -1, 1, -1,
          -1, 1, 1,
          1, -1, -1,
          1, -1, 1,
          1, 1, -1,
          1, 1, 1
        },
        null,
        new float[] {
          0, 0,   1f, 0,   0, 1f,   1f, 1f,

          //0, 0,   1f, 0,   0, 1f,   1f, 1f
        },
        null,
        new int[] {
          0, 1, 2,    1, 2, 3,
          2, 3, 6,    3, 6, 7,
          4, 5, 6,    5, 6, 7, 
          0, 1, 4,    1, 4, 5,
          3, 1, 7,    1, 7, 5,
          2, 0, 6,    0, 6, 4
        });

      _subModel.Scale = new Vector3d(25, 25, 25);
      _subModel2.Scale = new Vector3d(25, 25, 25);
      _subModel3.Scale = new Vector3d(100, 100, 100);

      _subModel.Position = new Vector3d(-250, 0, 0);
      _subModel2.Position = new Vector3d(250, 0, 0);
    }

    public void Update(double elapsedTime)
    {
      _subModel.temp++;
      _subModel2.temp+=4;
      _subModel3.temp+=2;
    }

    public void Render()
    {
      _subModel.Render();
      _subModel2.Render();
      _subModel3.Render();
    }
  }
}