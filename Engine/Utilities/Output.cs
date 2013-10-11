using System;

namespace Engine
{
  /// <summary>Utility for the engine. Prints important tasks and errors without crashing the entire game.</summary>
  public static class Output
  {
    /// <summary>The current indention level of writing output.</summary>
    private static int _indent = 0;

    /// <summary>Increases the number of spaces before writing by 2.</summary>
    public static void IncreaseIndent() { _indent = _indent + 2; }
    /// <summary>Decreases the number of spaces before writing by 2.</summary>
    public static void DecreaseIndent() { _indent = _indent - 2; }
    /// <summary>Clears indention level so no spaces will show before writing.</summary>
    public static void ClearIndent() { _indent = 0; }

    /// <summary>Writes a message to the console. Places spaces before the output equal to the current indention level.</summary>
    /// <param name="output">The string desired to write.</param>
    public static void Write(string output)
    {
      for (int i = 0; i < _indent; i++)
        Console.Write(" ");
      Console.Write(output);
      Console.WriteLine();
    }
  }
}