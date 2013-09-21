using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine
{
  public class ModelState2 : IGameObject
  {
    Renderer _renderer;
    SubModel _subModel;

    public ModelState2(TextureManager textureManager)
    {
      // Initialize the renderer for this instance
      _renderer = new Renderer();

      _subModel = new SubModel(textureManager,
        new float[] { -300, 0, -300, 300, 0, -300, -300, 300, -300, 300, 0, -300, -300, 300, -300, 300, 300, -300 },
        new float[] { 0, 0, 1, 0, 0, 1, 1, 0, 0, 1, 1, 1 });
    }

    public void Update(double elapsedTime) { }

    public void Render()
    {
      _subModel.Render();
    }
  }
}