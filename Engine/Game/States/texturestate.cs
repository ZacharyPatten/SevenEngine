using System;
using Engine;

namespace Engine
{
  class MultipleTexturesState : IGameObject
  {
    Sprite _toy;
    Sprite _toy2;
    Renderer _renderer;

    public MultipleTexturesState(TextureManager textureManager)
    {
      // Initialize fields
      _toy = new Sprite();
      _toy2 = new Sprite();
      _renderer = new Renderer();

      // Get assets here
      _toy.Texture = textureManager.Get("toy");
      _toy2.Texture = textureManager.Get("toy");

      // Initialize assets
      _toy.SetPosition(-400, 0);
    }

    public void Update(double elapsedTime) { }

    public void Render()
    {
      _renderer.DrawSprite(_toy);
      _renderer.DrawSprite(_toy2);
      _renderer.Render();
    }
  }
}
