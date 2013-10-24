using System;

namespace SevenEngine.Mathematics
{
  public static class Foundations
  {
    /// <summary>Returns the maximum of two numbers.</summary>
    /// <param name="first">First number in maximum comparison.</param>
    /// <param name="second">Second Number in maximum comparison.</param>
    /// <returns>The maximum of the parameters.</returns>
    public static double Max(double first, double second)
    {
      if (second > first)
        return second;
      return first;
    }

    /// <summary>Returns the minimum of two numbers.</summary>
    /// <param name="first">First number in minimum comparison.</param>
    /// <param name="second">Second Number in minimum comparison.</param>
    /// <returns>The minimum of the parameters.</returns>
    public static double Min(double first, double second)
    {
      if (second < first)
        return second;
      return first;
    }

    /// <summary>Returns the absolute value of a number.</summary>
    /// <param name="number">The number to absolute value.</param>
    /// <returns>The absolute value of the parameter.</returns>
    public static double Abs(double number)
    {
      if (number < 0)
        return -number;
      return number;
    }

    /// <summary>Determines whether a number is prime or not.</summary>
    /// <param name="candidate">The number to determine prime status.</param>
    /// <returns>True if prime, false if not prime.</returns>
    private static bool IsPrime(int candidate)
    {
      if (candidate == 2) return true;
      for (int divisor = 3; divisor <= Math.Sqrt(candidate); divisor += 2)
        if ((candidate % divisor) == 0)
          return false;
      return true;
    }
  }
}