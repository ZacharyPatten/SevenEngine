using System;
using SevenEngine.Mathematics;

namespace SevenEngine.Lighting
{
  public class Light
  {
    private Vector _position;

    public Vector position { get { return _position; } set { _position = value; } }

    public Light (Vector position)
    {
      if (position.Dimensions != 3)
        throw new LightException("position vector must be a 3-component vector.");
      _position = position;
    }

    private class LightException : Exception { public LightException(string message) : base(message) { } }
  }
}

