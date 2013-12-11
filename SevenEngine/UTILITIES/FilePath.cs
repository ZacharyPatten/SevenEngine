// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use under the following condition: any code using or originally taken 
// from the SevenEngine project must include citation to its original author(s) located at the
// top of each source code file, or you may include a reference to the SevenEngine project as
// a whole but you must include the current SevenEngine official website URL and logo.
// - Thanks.  :)  (support: seven@sevenengine.com)

// Author(s):
// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com
// Last Edited: 11-16-13

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
