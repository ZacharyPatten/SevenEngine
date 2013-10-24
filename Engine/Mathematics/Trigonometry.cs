namespace SevenEngine.Mathematics
{
  public static class Trigonometry
  {
    // This is as precise as double notation can represent the number PI (and common Pi fractions)
    /// <summary>3.14159265358979</summary>
    public static readonly double Pi = 3.14159265358979;
    /// <summary>6.28318530717958</summary>
    public static readonly double TwoPi = 6.28318530717958;
    /// <summary>1.57079632679489</summary>
    public static readonly double HalfPi = 1.57079632679489;
    /// <summary>4.71238898038468</summary>
    public static readonly double ThreeHalvesPi = 4.71238898038468;

    /// <summary>Converts a degrees measurement into randians.</summary>
    /// <param name="angle">The degrees of the angle.</param>
    /// <returns>The radian equivalent of the degrees measurement.</returns>
    public static double ToRadians(double angle) { return angle * Pi / 180; }

    /// <summary>Converts a randians measurtement into degrees.</summary>
    /// <param name="angle">The angle measurement to convert to degrees.</param>
    /// <returns>The degrees equivalent of a radian measurement.</returns>
    public static double ToDegrees(double angle) { return angle * 180 / Pi; }

    /// <summary>Calculates the sin of an angle using a taylor series.</summary>
    /// <param name="angle">The angle to calculate the sin function of IN RADIANS.</param>
    /// <returns>The calculated sin value of the given angle.</returns>
    public static double Sin(double angle)
    {
      // get the angle to be within the unit circle
      angle = angle % (TwoPi);

      // if the angle is negative, inverse it against the full unit circle
      if (angle < 0)
        angle = TwoPi + angle;

      // adjust for quadrants
      // NOTE: if you want more accuracy, you can follow this pattern
      // sin(x) = x - x^3/3! + x^5/5! - x^7/7! ...
      // the more terms you include the more accurate it is
      double angleCubed;
      double angleToTheFifth;
      // quadrant 1
      if (angle <= HalfPi)
      {
        angleCubed = angle * angle * angle;
        angleToTheFifth = angleCubed * angle * angle;
        return angle
          - ((angleCubed) / 6)
          + ((angleToTheFifth) / 120);
      }
      // quadrant 2
      else if (angle <= Pi)
      {
        angle = HalfPi - (angle % HalfPi);
        angleCubed = angle * angle * angle;
        angleToTheFifth = angleCubed * angle * angle;
        return angle
          - ((angleCubed) / 6)
          + ((angleToTheFifth) / 120);
      }
      // quadrant 3
      else if (angle <= ThreeHalvesPi) 
      {
        angle = angle % Pi;
        angleCubed = angle * angle * angle;
        angleToTheFifth = angleCubed * angle * angle;
        return -(angle
            - ((angleCubed) / 6)
            + ((angleToTheFifth) / 120));
      }
      // quadrant 4  
      else
      {
        angle = HalfPi - (angle % HalfPi);
        angleCubed = angle * angle * angle;
        angleToTheFifth = angleCubed * angle * angle;
        return -(angle
            - ((angleCubed) / 6)
            + ((angleToTheFifth) / 120));
      }
    }

    /// <summary>Calculates the sin of an angle using a taylor series.</summary>
    /// <param name="angle">The angle to calculate the sin function of IN RADIANS.</param>
    /// <returns>The calculated sin value of the given angle.</returns>
    public static double Cos(double angle)
    {
      // If you wanted to be cheap, you could just use the following commented line...
      // return Sin(angle + (Pi / 2));

      // get the angle to be within the unit circle
      angle = angle % (TwoPi);

      // if the angle is negative, inverse it against the full unit circle
      if (angle < 0)
        angle = TwoPi + angle;

      // adjust for quadrants
      // NOTE: if you want more accuracy, you can follow this pattern
      // cos(x) = 1 - x^2/2! + x^4/4! - x^6/6! ...
      // the terms you include the more accuracy it is
      double angleSquared;
      double angleToTheFourth;
      double angleToTheSixth;
      // quadrant 1
      if (angle <= HalfPi)
      {
        angleSquared = angle * angle;
        angleToTheFourth = angleSquared * angle * angle;
        angleToTheSixth = angleToTheFourth * angle * angle;
        return 1
          - (angleSquared / 2)
          + (angleToTheFourth / 24)
          - (angleToTheSixth / 720);
      }
      // quadrant 2
      else if (angle <= Pi)
      {
        angle = HalfPi - (angle % HalfPi);
        angleSquared = angle * angle;
        angleToTheFourth = angleSquared * angle * angle;
        angleToTheSixth = angleToTheFourth * angle * angle;
        return -(1
          - (angleSquared / 2)
          + (angleToTheFourth / 24)
          - (angleToTheSixth / 720));
      }
      // quadrant 3
      else if (angle <= ThreeHalvesPi)
      {
        angle = angle % Pi;
        angleSquared = angle * angle;
        angleToTheFourth = angleSquared * angle * angle;
        angleToTheSixth = angleToTheFourth * angle * angle;
        return -(1
          - (angleSquared / 2)
          + (angleToTheFourth / 24)
          - (angleToTheSixth / 720));
      }
      // quadrant 4  
      else
      {
        angle = HalfPi - (angle % HalfPi);
        angleSquared = angle * angle;
        angleToTheFourth = angleSquared * angle * angle;
        angleToTheSixth = angleToTheFourth * angle * angle;
        return 1
          - (angleSquared / 2)
          + (angleToTheFourth / 24)
          - (angleToTheSixth / 720);
      }
    }

    /// <summary>Calculates the tan of an angle using a taylor series to FIVE DECIMAL PLACES.</summary>
    /// <param name="angle">The angle to calculate the tan function of IN RADIANS.</param>
    /// <returns>The calculated tan value of the given angle.</returns>
    public static double Tan(double angle)
    {
      // "sin / cos" results in "opposite side / adjacent side", which is equal to tangent
      return Sin(angle) / Cos(angle);

      // NOTE: I really should find a series to solve for Tan, but I just haven't done it yet :(
    }

    /// <summary>Finds the sec of a given angle.</summary>
    /// <param name="angle">The angle to find the sec of IN RADIANS.</param>
    /// <returns>The sec of the provided angle.</returns>
    public static double Sec(double angle)
    {
      // by definition, sec is the reciprical of cos
      return 1 / Cos(angle);
    }

    /// <summary>Finds the csc of a given angle.</summary>
    /// <param name="angle">The angle to find the csc of IN RADIANS.</param>
    /// <returns>The csc of the provided angle.</returns>
    public static double Csc(double angle)
    {
      // by definition, csc is the reciprical of sin
      return 1 / Sin(angle);
    }

    /// <summary>Finds the cot of a given angle.</summary>
    /// <param name="angle">The angle to find the cot of IN RADIANS.</param>
    /// <returns>The cot of the provided angle.</returns>
    public static double Cot(double angle)
    {
      // by definition, cot is the reciprical of tan
      return 1 / Tan(angle);
    }
  }
}