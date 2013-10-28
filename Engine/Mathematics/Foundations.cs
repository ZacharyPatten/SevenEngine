// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use with the following condition: any code using or originally taken from the 
// SevenEngine project must include citation to its original author(s) located at the top of each
// source code file. Alternatively, you may include a reference to the SevenEngine project as a whole,
// but you must include the current SevenEngine official website URL and logo.
// - Thanks.  :)  (support: seven@sevenengine.com)

// Author(s):
// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com
// Last Edited: 10-26-13

using System;

namespace SevenEngine.Mathematics
{
  public static class Foundations
  {
    /// <summary>Returns the maximum of two numbers.</summary>
    /// <param name="first">First number in maximum comparison.</param>
    /// <param name="second">Second Number in maximum comparison.</param>
    /// <returns>The maximum of the parameters.</returns>
    public static float Max(float first, float second)
    {
      // If you want to use System.Math, just uncomment the following,
      // or you can try to optimize the function instead!
      //return Math.Max(first, second);
      if (second > first)
        return second;
      return first;
    }

    /// <summary>Returns the minimum of two numbers.</summary>
    /// <param name="first">First number in minimum comparison.</param>
    /// <param name="second">Second Number in minimum comparison.</param>
    /// <returns>The minimum of the parameters.</returns>
    public static float Min(float first, float second)
    {
      // If you want to use System.Math, just uncomment the following,
      // or you can try to optimize the function instead!
      //return Math.Min(first, second);
      if (second < first)
        return second;
      return first;
    }

    /// <summary>Returns the absolute value of a number.</summary>
    /// <param name="number">The number to absolute value.</param>
    /// <returns>The absolute value of the parameter.</returns>
    public static float Abs(float number)
    {
      // If you want to use System.Math, just uncomment the following,
      // or you can try to optimize the function instead!
      //return Math.Abs(number);
      if (number < 0)
        return -number;
      return number;
    }

    /// <summary>Determines whether a number is prime or not.</summary>
    /// <param name="candidate">The number to determine prime status.</param>
    /// <returns>True if prime, false if not prime.</returns>
    public static bool IsPrime(int candidate)
    {
      if (candidate == 2) return true;
      for (int divisor = 3; divisor <= SquareRoot(candidate); divisor += 2)
        if ((candidate % divisor) == 0)
          return false;
      return true;
    }

    /// <summary>Returns the square root of a number.
    /// (NOTE! I havn't written this method yet so I'm just calling Math.Sqrt until I get around to it.</summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static float SquareRoot(float number)
    {
      // If you want to use System.Math, just uncomment the following,
      // or you can try to optimize the function instead!
      return (float)Math.Sqrt(number);
      // I have not written my own version of this function yet, just use the System for now...
    }

    /// <summary>Clamps a float by a maximum and minimum value.</summary>
    /// <param name="value">The value to be clamped.</param>
    /// <param name="minimum">The minimum value of the clamp.</param>
    /// <param name="maximum">The maximum value of the clamp.</param>
    /// <returns>The clamped value.</returns>
    public static float Clamp(float value, float minimum, float maximum)
    {
      if (value < minimum)
        return minimum;
      if (value > maximum)
        return maximum;
      return value;
    }

    /// <summary>Raises a number by a power.</summary>
    /// <param name="number">The number to apply a power to.</param>
    /// <param name="power">The power to apply.</param>
    /// <returns>The result of raising the number by the power.</returns>
    public static float Power(float number, float power)
    {
      // If you want to use System.Math, just uncomment the following,
      // or you can try to optimize the function instead!
      return (float)Math.Pow(number, power);
      // I have not written my own version of this function yet, just use the System for now...
    }
  }
}