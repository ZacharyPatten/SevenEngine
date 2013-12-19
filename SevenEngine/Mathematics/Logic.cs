using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenEngine.Mathematics.GENERAL
{
  public static class Logic
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
  }
}
