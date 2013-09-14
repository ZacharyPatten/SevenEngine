using System;
using OpenTK;
using OpenTK.Graphics;
using Engine;

namespace Engine
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