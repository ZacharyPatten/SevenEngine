namespace Engine
{
  public interface IGameState
  {
    string Update(double elapsedTime);
    void Render();
  }
}