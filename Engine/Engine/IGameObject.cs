namespace Engine
{
  public interface IGameObject
  {
    void Update(double elapsedTime);
    void Render();
  }
}