using System;

using SevenEngine;

namespace Game
{
  static class Program
  {
    [STAThread]
    static void Main()
    {
      using (Game game = new Game())
      {
        game.Run();
      }
    }
  }
}