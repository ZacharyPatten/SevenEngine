// Seven
// https://github.com/53V3N1X/SevenEngine
// LISCENSE: See "LISCENSE.txt" in th root project directory.
// SUPPORT: See "README.txt" in the root project directory.

using Seven.Structures;

namespace SevenEngine
{
  /// <summary>INHERIT ME!</summary>
  public interface InterfaceGameState
  {
    string Id { get; set; }
    bool IsReady { get; }
    void Load();
    string Update(float elapsedTime);
    void Render();
  }
}