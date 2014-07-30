// Seven
// https://github.com/53V3N1X/SevenEngine
// LISCENSE: See "LISCENSE.txt" in th root project directory.
// SUPPORT: See "README.txt" in the root project directory.

using System;
using Seven.Structures;

namespace bD
{
  /// <summary>Represents a code library that can be altered at runtime.</summary>
  public class Dimension
  {
    // The base of the library
    private Map<object, string> _dB;

    /// <summary>Constructs an empty code library.</summary>
    public Dimension()
    {
      _dB = new Map_Linked<object, string>(
        (string left, string right) => { return left.Equals(right); },
        (string str) => { return str.GetHashCode(); });
    }

    /// <summary>Declares a value in the code library to it's default value.</summary>
    /// <typeparam name="T">The type of the value of the declaration.</typeparam>
    /// <param name="name">The name used for look-ups.</param>
    /// <exception cref="NONE">Good Luck :)</exception>
    public void Declare<T>(string name)
    {
      try { _dB.Add(name, default(T)); }
      catch { _dB[name] = default(T); }
    }

    /// <summary>Delegates (Assigns) a value in the code library.</summary>
    /// <typeparam name="T">The type of the value of the declaration.</typeparam>
    /// <param name="name">The name used for look-ups.</param>
    /// <param name="value">The value to store.</param>
    /// <exception cref="NONE">Good Luck :)</exception>
    public void Assignment<T>(string name, T value)
    {
      try { _dB.Add(name, value); }
      catch { _dB[name] = value; }
    }

    /// <summary>Dereferences (Gets) a value in the code library.</summary>
    /// <typeparam name="T">The type of the value to get.</typeparam>
    /// <param name="name">The name to look up the value for.</param>
    /// <returns>The value if found.</returns>
    /// <exception cref="NONE">Good Luck :)</exception>
    public T Get<T>(string name)
    {
      try { return (T)_dB[name]; }
      catch { return default(T); }
    }

    /// <summary>Deletes a value from the code library.</summary>
    /// <param name="name">The name of the variable to delete.</param>
    /// <exception cref="NONE">Good Luck :)</exception>
    public void Remove(string name)
    {
      try { _dB.Remove(name); }
      catch { }
    }

    /// <summary>Begins execution of a program given the starting point.</summary>
    /// <param name="entry">The starting method to begin execution.</param>
    /// <exception cref="NONE">Good Luck :)</exception>
    public bool Do(string daEntryPoint)
    {
      try { Get<Action>(daEntryPoint)(); return true; }
      catch { return false; }
    }
  }
}
