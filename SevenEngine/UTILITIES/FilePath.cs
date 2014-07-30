// Seven
// https://github.com/53V3N1X/SevenEngine
// LISCENSE: See "LISCENSE.txt" in th root project directory.
// SUPPORT: See "README.txt" in the root project directory.

using System;
using System.IO;

namespace SevenEngine
{
  /// <summary>Will generate filepaths in a cross platform manor.</summary>
  public static class FilePath
  {
    /// <summary>Takes a string representing a file path with folder delimiters '\' or '/' 
    /// and puts in the correct delimiter based on the OS.</summary>
    /// <param name="path">The path to make cross platform.</param>
    /// <returns>The correctly formatted filepath string.</returns>
    public static string FromRelative(string path)
    {
      if ((path[0] == '\\' || path[0] == '/') && (path[path.Length - 1] == '\\' || path[path.Length - 1] == '/'))
        return Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + Path.Combine(path.Split('\\')) + Path.DirectorySeparatorChar;
      else if (path[0] == '\\' || path[0] == '/')
        return Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + Path.Combine(path.Split('\\'));
      else if (path[path.Length - 1] == '\\' || path[path.Length - 1] == '/')
        return Directory.GetCurrentDirectory() + Path.Combine(path.Split('\\')) + Path.DirectorySeparatorChar;
      else
        return Directory.GetCurrentDirectory() + Path.Combine(path.Split('\\'));
    }
  }
}
