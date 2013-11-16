// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use with the following condition: any code using or originally taken 
// from the SevenEngine project must include citation to its original author(s) located at the
// top of each source code file, or you may include a reference to the SevenEngine project as
// a whole but you must include the current SevenEngine official website URL and logo.
// - Thanks.  :)  (support: seven@sevenengine.com)

// Author(s):
// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com
// Last Edited: 11-16-13

using System.Diagnostics;

namespace SevenEngine
{
  /// <summary>Utility for the engine. Gets the alapsed time to be passed to the "Update()" functions of state or game objects.</summary>
  public class PreciseTimer
  {
    private Stopwatch _stopwatch;

    public PreciseTimer()
    {
      _stopwatch = new Stopwatch();
      _stopwatch.Reset();
    }

    public float GetElaspedMilliseconds()
    {
      float elapsed = _stopwatch.ElapsedMilliseconds;
      _stopwatch.Restart();
      return elapsed;
    }

    // The folowing code works for Windows OS only. I it most likely what the "Stopwatch" class is calling.
    // The stopwatch class is supported by Mono Develop, which is why I am using it. I thought I would keep this code as an example.
    #region Windows Code Only

    //[System.Security.SuppressUnmanagedCodeSecurity]
    //[DllImport("kernel32")]
    //private static extern bool QueryPerformanceFrequency(ref long PerformanceFrequency);

    //[System.Security.SuppressUnmanagedCodeSecurity]
    //[DllImport("kernel32")]
    //private static extern bool QueryPerformanceCounter(ref long PerformanceCount);

    ///// <summary>A measure of how fast the ticks are happening for the current system.</summary>
    //private long _ticksPerSecond = 0;
    ///// <summary>A memory of the previous call to determine timespan.</summary>
    //private long _previousElapsedTime = 0;

    ///// <summary>Creates an instance of a precise timer and initializes the the time.</summary>
    //public PreciseTimer()
    //{
    //  QueryPerformanceFrequency(ref _ticksPerSecond);
    //  // Clear the memory so the first call isn't garbage.
    //  GetElapsedTime();
    //}


    ///// <summary>Gets the elasped time since the last call (or since initialization if first time calling) in SECONDS.</summary>
    ///// <returns>Time since last call in SECONDS as the unit.</returns>
    //public float GetElapsedTime()
    //{
    //  long time = 0;
    //  QueryPerformanceCounter(ref time);

    //  float elapsedTime = (float)(time - _previousElapsedTime) / (float)_ticksPerSecond;
    //  _previousElapsedTime = time;

    //  return elapsedTime;
    //}

    #endregion
  }
}