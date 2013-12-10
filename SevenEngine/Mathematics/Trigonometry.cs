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

namespace SevenEngine.Mathematics
{
  public static class Trigonometry
  {
    /// <summary>3.14159265358979</summary>
    public static readonly float Pi = 3.14159265358979f;
    /// <summary>6.28318530717958</summary>
    public static readonly float TwoPi = 6.28318530717958f;
    /// <summary>1.57079632679489</summary>
    public static readonly float HalfPi = 1.57079632679489f;
    /// <summary>4.71238898038468</summary>
    public static readonly float ThreeHalvesPi = 4.71238898038468f;

    public static float ToRadians(float angle) { return angle * Pi / 180f; }
    public static float ToDegrees(float angle) { return angle * 180f / Pi; }

    public static float Sin(float angle)
    {
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

    public static float Cos(float angle)
    {
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

    public static float Tan(float angle)
    {
      return (float)Math.Tan(angle);

      // THE FOLLOWING IS MY PERSONAL FUNCTION. IT WORKS BUT IT IS NOT AS FAST AS
      // THE SYSTEM FUNCTION IN ITS CURRENT STATE
      #region Custom Tan Function
      //// "sin / cos" results in "opposite side / adjacent side", which is equal to tangent
      //return Sin(angle) / Cos(angle);
      #endregion
    }

    public static float Sec(float angle)
    {
      return 1.0f / (float)Math.Cos(angle);

      // THE FOLLOWING IS MY PERSONAL FUNCTION. IT WORKS BUT IT IS NOT AS FAST AS
      // THE SYSTEM FUNCTION IN ITS CURRENT STATE
      #region Custom Sec Function
      //// by definition, sec is the reciprical of cos
      //return 1 / Cos(angle);
      #endregion
    }

    public static float Csc(float angle)
    {
      return 1.0f / (float)Math.Sin(angle);

      // THE FOLLOWING IS MY PERSONAL FUNCTION. IT WORKS BUT IT IS NOT AS FAST AS
      // THE SYSTEM FUNCTION IN ITS CURRENT STATE
      #region Custom Csc Function
      //// by definition, csc is the reciprical of sin
      //return 1 / Sin(angle);
      #endregion
    }

    public static float Cot(float angle)
    {
      return 1.0f / (float)Math.Tan(angle);

      // THE FOLLOWING IS MY PERSONAL FUNCTION. IT WORKS BUT IT IS NOT AS FAST AS
      // THE SYSTEM FUNCTION IN ITS CURRENT STATE
      #region Custom Cot Function
      //// by definition, cot is the reciprical of tan
      //return 1 / Tan(angle);
      #endregion
    }

    public static float ArcSin(float sinRatio)
    {
      return (float)Math.Asin(sinRatio);
      //I haven't made a custom ArcSin function yet...
    }

    public static float ArcCos(float cosRatio)
    {
      return (float)Math.Acos(cosRatio);
      //I haven't made a custom ArcCos function yet...
    }

    public static float ArcTan(float tanRatio)
    {
      return (float)Math.Atan(tanRatio);
      //I haven't made a custom ArcTan function yet...
    }

    public static float ArcCsc(float cscRatio)
    {
      return (float)Math.Asin(1.0f / cscRatio);
      //I haven't made a custom ArcCsc function yet...
    }

    public static float ArcSec(float secRatio)
    {
      return (float)Math.Acos(1.0f / secRatio);
      //I haven't made a custom ArcSec function yet...
    }

    public static float ArcCot(float cotRatio)
    {
      return (float)Math.Atan(1.0f / cotRatio);
      //I haven't made a custom ArcCot function yet...
    }
  }
}