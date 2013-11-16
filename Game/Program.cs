using System;

namespace Game
{
  static class Program
  {
    [STAThread]
    static void Main()
    {
      // you can select the starting window size here
      using (Game game = new Game(800, 600)) { game.Run(); }
    }
  }
}