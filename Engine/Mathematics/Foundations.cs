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

using System;

namespace SevenEngine.Mathematics
{
  public static class Foundations
  {
    public static float Max(float first, float second)
    {
      //return Math.Max(first, second);
      if (second > first)
        return second;
      return first;
    }

    public static float Min(float first, float second)
    {
      //return Math.Min(first, second);
      if (second < first)
        return second;
      return first;
    }

    public static float Abs(float number)
    {
      //return Math.Abs(number);
      if (number < 0)
        return -number;
      return number;
    }

    public static bool IsPrime(int candidate)
    {
      if (candidate == 2) return true;
      for (int divisor = 3; divisor <= SquareRoot(candidate); divisor += 2)
        if ((candidate % divisor) == 0)
          return false;
      return true;
    }

    public static float SquareRoot(float number)
    {
      return (float)Math.Sqrt(number);
      // I have not written my own version of this function yet, just use the System for now...
    }

    public static float Clamp(float value, float minimum, float maximum)
    {
      if (value < minimum)
        return minimum;
      if (value > maximum)
        return maximum;
      return value;
    }

    public static float Power(float number, float power)
    {
      return (float)Math.Pow(number, power);
      // I have not written my own version of this function yet, just use the System for now...
    }
  }
}