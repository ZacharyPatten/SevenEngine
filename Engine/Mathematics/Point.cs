using System.Runtime.InteropServices;

namespace Engine.Mathematics
{
  [StructLayout(LayoutKind.Sequential)]
  public struct Point
  {
    public float X { get; set; }
    public float Y { get; set; }

    public Point(float x, float y) : this()
    {
      X = x;
      Y = y;
    }
  }
}