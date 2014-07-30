// Seven
// https://github.com/53V3N1X/SevenEngine
// LISCENSE: See "LISCENSE.txt" in th root project directory.
// SUPPORT: See "README.txt" in the root project directory.

using System;
using Seven.Mathematics;

namespace SevenEngine.Lighting
{
  public class Light
  {
    private Vector<float> _position;

    public Vector<float> position { get { return _position; } set { _position = value; } }

    public Light (Vector<float> position)
    {
      if (position.Dimensions != 3)
        throw new LightException("position vector must be a 3-component vector.");
      _position = position;
    }

    private class LightException : Exception { public LightException(string message) : base(message) { } }
  }
}

