namespace Engine
{
  public interface IGameState
  {
    void Update(double elapsedTime);
    void Render();
  }
}