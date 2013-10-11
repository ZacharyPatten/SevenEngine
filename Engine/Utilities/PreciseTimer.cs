using System.Runtime.InteropServices;

namespace Engine.Utilities
{
  /// <summary>Utility for the engine. Gets the alapsed time to be passed to the "Update()" functions of state or game objects.</summary>
  public class PreciseTimer
  {
    [System.Security.SuppressUnmanagedCodeSecurity]
    [DllImport("kernel32")]
    private static extern bool QueryPerformanceFrequency(ref long PerformanceFrequency);

    [System.Security.SuppressUnmanagedCodeSecurity]
    [DllImport("kernel32")]
    private static extern bool QueryPerformanceCounter(ref long PerformanceCount);

    /// <summary>A measure of how fast the ticks are happening for the current system.</summary>
    protected long _ticksPerSecond = 0;
    // 
    protected long _previousElapsedTime = 0;


    /// <summary>Creates an instance of a precise timer and initializes the the time.</summary>
    public PreciseTimer()
    {
      QueryPerformanceFrequency(ref _ticksPerSecond);
      // Clear the memory so the first call isn't garbage.
      GetElapsedTime();
    }


    /// <summary>Gets the elasped time since the last call (or since initialization if first time calling) in SECONDS.</summary>
    /// <returns>Time since last call in SECONDS as the unit.</returns>
    public double GetElapsedTime()
    {
      long time = 0;
      QueryPerformanceCounter(ref time);

      double elapsedTime = (double)(time - _previousElapsedTime) / (double)_ticksPerSecond;
      _previousElapsedTime = time;

      return elapsedTime;
    }
  }
}