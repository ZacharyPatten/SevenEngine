// Seven
// https://github.com/53V3N1X/SevenEngine
// LISCENSE: See "LISCENSE.txt" in th root project directory.
// SUPPORT: See "README.txt" in the root project directory.

using System;
using Seven.Mathematics;
using Seven.Structures;

namespace SevenEngine.DynamicModels
{
  public class KeyFrame
  {
    private float _start, _end; // seconds
    private List<Matrix<float>> _tranformations;

    public float Start { get { return _start; } }
    public float End { get { return _end; } }

    public KeyFrame(List<Matrix<float>> transformations)
    {

    }
  }
}
