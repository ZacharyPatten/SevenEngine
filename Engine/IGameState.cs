namespace SevenEngine
{
  public interface IGameState
  {
    string Update(double elapsedTime);
    void Render();
  }
}