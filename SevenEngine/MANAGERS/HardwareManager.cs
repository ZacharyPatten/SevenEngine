// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use under the following condition: any code using or originally taken 
// from the SevenEngine project must include citation to its original author(s) located at the
// top of each source code file, or you may include a reference to the SevenEngine project as
// a whole but you must include the current SevenEngine official website URL and logo.
// - Thanks.  :)  (support: seven@sevenengine.com)

// Author(s):
// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com
// Last Edited: 12-6-13

using System;
using OpenTK;

namespace SevenEngine
{
  /// <summary>Contains hardware constants for dynamic optimizatiions.</summary>
  public static class HardwareManager
  {
    private static int _numberOfCores;
    private static int _monitorWidth;
    private static int _monitorHeight;

    /// <summary>Returns the number of logical cores on the current hardware.</summary>
    public static int LogicalCores { get { return _numberOfCores; } }
    /// <summary>Returns the width of the default monitor.</summary>
    public static int MonitorWidth { get { return _monitorWidth; } }
    /// <summary>Returns the height of the default monitor.</summary>
    public static int MonitorHeight { get { return _monitorHeight; } }

    public static void Initialize()
    {
      _numberOfCores = System.Environment.ProcessorCount;
      _monitorWidth = OpenTK.DisplayDevice.Default.Width;
      _monitorHeight = OpenTK.DisplayDevice.Default.Height;
    }
  }
}