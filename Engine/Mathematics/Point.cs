using System.Runtime.InteropServices;

namespace SevenEngine.Mathematics
{
  [StructLayout(LayoutKind.Sequential)]
  public class Point
  {
    private double _x;
    private double _y;

    public double X { get { return _x; } set { _x = value; } }
    public double Y { get { return _y; } set { _y = value; } }

    public Point()
    {
      _x = 0d;
      _y = 0d;
    }

    public Point(double x, double y)
    {
      X = x;
      Y = y;
    }
  }
}