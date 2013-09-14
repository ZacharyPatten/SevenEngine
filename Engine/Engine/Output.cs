using System;

namespace Engine
{
  public static class Output
  {
    private static int _indent;

    public static void IncreaseIndent()
    {
      _indent = _indent + 2;
    }

    public static void DecreaseIndent()
    {
      _indent = _indent - 2;
    }

    public static void ClearIndent()
    {
      _indent = 0;
    }

    public static void Print(string output)
    {
      for (int i = 0; i < _indent; i++)
      {
        Console.Write(" ");
      }
      Console.Write(output);
      Console.WriteLine();
    }
  }
}
