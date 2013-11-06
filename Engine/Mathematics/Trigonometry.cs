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
  public static class Trigonometry
  {
    // This is as precise as float notation can represent the number PI (and common Pi fractions)
    /// <summary>3.14159265358979</summary>
    public static readonly float Pi = 3.14159265358979f;
    /// <summary>6.28318530717958</summary>
    public static readonly float TwoPi = 6.28318530717958f;
    /// <summary>1.57079632679489</summary>
    public static readonly float HalfPi = 1.57079632679489f;
    /// <summary>4.71238898038468</summary>
    public static readonly float ThreeHalvesPi = 4.71238898038468f;

    /// <summary>Converts a degrees measurement into randians.</summary>
    /// <param name="angle">The degrees of the angle.</param>
    /// <returns>The radian equivalent of the degrees measurement.</returns>
    public static float ToRadians(float angle) { return angle * Pi / 180; }

    /// <summary>Converts a randians measurtement into degrees.</summary>
    /// <param name="angle">The angle measurement to convert to degrees.</param>
    /// <returns>The degrees equivalent of a radian measurement.</returns>
    public static float ToDegrees(float angle) { return angle * 180 / Pi; }

    /// <summary>Calculates the sin of an angle using a taylor series.</summary>
    /// <param name="angle">The angle to calculate the sin function of IN RADIANS.</param>
    /// <returns>The calculated sin value of the given angle.</returns>
    public static float Sin(float angle)
    {
      // If you want to use System.Math, just use the following,
      // or you can try to optimize the function instead!
      return (float)Math.Sin(angle);

      // THE FOLLOWING IS PERSONAL SIN FUNCTION. IT WORKS BUT IT IS NOT AS FAST AS
      // THE SYSTEM FUNCTION IN ITS CURRENT STATE
      #region Custom Sin Function
      //// get the angle to be within the unit circle
      //angle = angle % (TwoPi);

      //// if the angle is negative, inverse it against the full unit circle
      //if (angle < 0)
      //  angle = TwoPi + angle;

      //// adjust for quadrants
      //// NOTE: if you want more accuracy, you can follow this pattern
      //// sin(x) = x - x^3/3! + x^5/5! - x^7/7! ...
      //// the more terms you include the more accurate it is
      //float angleCubed;
      //float angleToTheFifth;
      //// quadrant 1
      //if (angle <= HalfPi)
      //{
      //  angleCubed = angle * angle * angle;
      //  angleToTheFifth = angleCubed * angle * angle;
      //  return angle
      //    - ((angleCubed) / 6)
      //    + ((angleToTheFifth) / 120);
      //}
      //// quadrant 2
      //else if (angle <= Pi)
      //{
      //  angle = HalfPi - (angle % HalfPi);
      //  angleCubed = angle * angle * angle;
      //  angleToTheFifth = angleCubed * angle * angle;
      //  return angle
      //    - ((angleCubed) / 6)
      //    + ((angleToTheFifth) / 120);
      //}
      //// quadrant 3
      //else if (angle <= ThreeHalvesPi)
      //{
      //  angle = angle % Pi;
      //  angleCubed = angle * angle * angle;
      //  angleToTheFifth = angleCubed * angle * angle;
      //  return -(angle
      //      - ((angleCubed) / 6)
      //      + ((angleToTheFifth) / 120));
      //}
      //// quadrant 4  
      //else
      //{
      //  angle = HalfPi - (angle % HalfPi);
      //  angleCubed = angle * angle * angle;
      //  angleToTheFifth = angleCubed * angle * angle;
      //  return -(angle
      //      - ((angleCubed) / 6)
      //      + ((angleToTheFifth) / 120));
      //}
      #endregion
    }

    /// <summary>Calculates the sin of an angle using a taylor series.</summary>
    /// <param name="angle">The angle to calculate the sin function of IN RADIANS.</param>
    /// <returns>The calculated sin value of the given angle.</returns>
    public static float Cos(float angle)
    {
      // If you want to use System.Math, just use the following,
      // or you can try to optimize the function instead!
      return (float)Math.Cos(angle);

      // THE FOLLOWING IS MY PERSONAL FUNCTION. IT WORKS BUT IT IS NOT AS FAST AS
      // THE SYSTEM FUNCTION IN ITS CURRENT STATE

      #region Custom Cos Function
      //// If you wanted to be cheap, you could just use the following commented line...
      //// return Sin(angle + (Pi / 2));

      //// get the angle to be within the unit circle
      //angle = angle % (TwoPi);

      //// if the angle is negative, inverse it against the full unit circle
      //if (angle < 0)
      //  angle = TwoPi + angle;

      //// adjust for quadrants
      //// NOTE: if you want more accuracy, you can follow this pattern
      //// cos(x) = 1 - x^2/2! + x^4/4! - x^6/6! ...
      //// the terms you include the more accuracy it is
      //float angleSquared;
      //float angleToTheFourth;
      //float angleToTheSixth;
      //// quadrant 1
      //if (angle <= HalfPi)
      //{
      //  angleSquared = angle * angle;
      //  angleToTheFourth = angleSquared * angle * angle;
      //  angleToTheSixth = angleToTheFourth * angle * angle;
      //  return 1
      //    - (angleSquared / 2)
      //    + (angleToTheFourth / 24)
      //    - (angleToTheSixth / 720);
      //}
      //// quadrant 2
      //else if (angle <= Pi)
      //{
      //  angle = HalfPi - (angle % HalfPi);
      //  angleSquared = angle * angle;
      //  angleToTheFourth = angleSquared * angle * angle;
      //  angleToTheSixth = angleToTheFourth * angle * angle;
      //  return -(1
      //    - (angleSquared / 2)
      //    + (angleToTheFourth / 24)
      //    - (angleToTheSixth / 720));
      //}
      //// quadrant 3
      //else if (angle <= ThreeHalvesPi)
      //{
      //  angle = angle % Pi;
      //  angleSquared = angle * angle;
      //  angleToTheFourth = angleSquared * angle * angle;
      //  angleToTheSixth = angleToTheFourth * angle * angle;
      //  return -(1
      //    - (angleSquared / 2)
      //    + (angleToTheFourth / 24)
      //    - (angleToTheSixth / 720));
      //}
      //// quadrant 4  
      //else
      //{
      //  angle = HalfPi - (angle % HalfPi);
      //  angleSquared = angle * angle;
      //  angleToTheFourth = angleSquared * angle * angle;
      //  angleToTheSixth = angleToTheFourth * angle * angle;
      //  return 1
      //    - (angleSquared / 2)
      //    + (angleToTheFourth / 24)
      //    - (angleToTheSixth / 720);
      //}
      #endregion
    }

    /// <summary>Calculates the tan of an angle using a taylor series to FIVE DECIMAL PLACES.</summary>
    /// <param name="angle">The angle to calculate the tan function of IN RADIANS.</param>
    /// <returns>The calculated tan value of the given angle.</returns>
    public static float Tan(float angle)
    {
      // If you want to use System.Math, just use the following,
      // or you can try to optimize the function instead!
      return (float)Math.Tan(angle);

      // THE FOLLOWING IS MY PERSONAL FUNCTION. IT WORKS BUT IT IS NOT AS FAST AS
      // THE SYSTEM FUNCTION IN ITS CURRENT STATE

      #region Custom Tan Function
      //// "sin / cos" results in "opposite side / adjacent side", which is equal to tangent
      //return Sin(angle) / Cos(angle);
      #endregion
    }

    /// <summary>Finds the sec of a given angle.</summary>
    /// <param name="angle">The angle to find the sec of IN RADIANS.</param>
    /// <returns>The sec of the provided angle.</returns>
    public static float Sec(float angle)
    {
      // If you want to use System.Math, just use the following,
      // or you can try to optimize the function instead!
      return 1.0f / (float)Math.Cos(angle);

      // THE FOLLOWING IS MY PERSONAL FUNCTION. IT WORKS BUT IT IS NOT AS FAST AS
      // THE SYSTEM FUNCTION IN ITS CURRENT STATE
      #region Custom Sec Function
      //// by definition, sec is the reciprical of cos
      //return 1 / Cos(angle);
      #endregion
    }

    /// <summary>Finds the csc of a given angle.</summary>
    /// <param name="angle">The angle to find the csc of IN RADIANS.</param>
    /// <returns>The csc of the provided angle.</returns>
    public static float Csc(float angle)
    {
      // If you want to use System.Math, just use the following,
      // or you can try to optimize the function instead!
      return 1.0f / (float)Math.Sin(angle);

      // THE FOLLOWING IS MY PERSONAL FUNCTION. IT WORKS BUT IT IS NOT AS FAST AS
      // THE SYSTEM FUNCTION IN ITS CURRENT STATE
      #region Custom Csc Function
      //// by definition, csc is the reciprical of sin
      //return 1 / Sin(angle);
      #endregion
    }

    /// <summary>Finds the cot of a given angle.</summary>
    /// <param name="angle">The angle to find the cot of IN RADIANS.</param>
    /// <returns>The cot of the provided angle.</returns>
    public static float Cot(float angle)
    {
      // If you want to use System.Math, just use the following,
      // or you can try to optimize the function instead!
      return 1.0f / (float)Math.Tan(angle);

      // THE FOLLOWING IS MY PERSONAL FUNCTION. IT WORKS BUT IT IS NOT AS FAST AS
      // THE SYSTEM FUNCTION IN ITS CURRENT STATE
      #region Custom Cot Function
      //// by definition, cot is the reciprical of tan
      //return 1 / Tan(angle);
      #endregion
    }

    /// <summary>Calculates an angle from a sin ratio.</summary>
    /// <param name="sinRatio">The sin ratio.</param>
    /// <returns>The calculated angle IN RADIANS.</returns>
    public static float ArcSin(float sinRatio)
    {
      // If you want to use System.Math, just use the following,
      // or you can try to optimize the function instead!
      return (float)Math.Asin(sinRatio);

      //I haven't made a custom ArcSin function yet...
    }

    /// <summary>Calculates an angle from a cos ratio.</summary>
    /// <param name="cosRatio">The cos ratio.</param>
    /// <returns>The calculated angle IN RADIANS.</returns>
    public static float ArcCos(float cosRatio)
    {
      // If you want to use System.Math, just use the following,
      // or you can try to optimize the function instead!
      return (float)Math.Acos(cosRatio);

      //I haven't made a custom ArcCos function yet...
    }

    /// <summary>Calculates an angle from a tan ratio.</summary>
    /// <param name="tanRatio">The tan ratio.</param>
    /// <returns>The calculated angle IN RADIANS.</returns>
    public static float ArcTan(float tanRatio)
    {
      // If you want to use System.Math, just use the following,
      // or you can try to optimize the function instead!
      return (float)Math.Atan(tanRatio);

      //I haven't made a custom ArcTan function yet...
    }

    /// <summary>Calculates an angle from a csc ratio.</summary>
    /// <param name="cscRatio">The csc ratio.</param>
    /// <returns>The calculated angle IN RADIANS.</returns>
    public static float ArcCsc(float cscRatio)
    {
      // If you want to use System.Math, just use the following,
      // or you can try to optimize the function instead!
      return (float)Math.Asin(1.0f / cscRatio);

      //I haven't made a custom ArcCsc function yet...
    }

    /// <summary>Calculates an angle from a sec ratio.</summary>
    /// <param name="secRatio">The sec ratio.</param>
    /// <returns>The calculated angle IN RADIANS.</returns>
    public static float ArcSec(float secRatio)
    {
      // If you want to use System.Math, just use the following,
      // or you can try to optimize the function instead!
      return (float)Math.Acos(1.0f / secRatio);

      //I haven't made a custom ArcSec function yet...
    }

    /// <summary>Calculates an angle from a cot ratio.</summary>
    /// <param name="cotRatio">The cot ratio.</param>
    /// <returns>The calculated angle IN RADIANS.</returns>
    public static float ArcCot(float cotRatio)
    {
      // If you want to use System.Math, just use the following,
      // or you can try to optimize the function instead!
      return (float)Math.Atan(1.0f / cotRatio);

      //I haven't made a custom ArcCot function yet...
    }
  }
}