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
  public class Tween
  {
    float _original;
    float _distance;
    float _current;
    float _totalTimePassed = 0;
    float _totalDuration = 5;
    bool _finished = false;
    TweenFunction _tweenF = null;

    public delegate float TweenFunction(float timePassed, float start, float distance, float duration);

    public float Value() { return _current; }

    public bool IsFinished() { return _finished; }

    public Tween(float start, float end, float time) { Construct(start, end, time, Tween.Linear); }

    public Tween(float start, float end, float time, TweenFunction tweenF) { Construct(start, end, time, tweenF); }

    public void Construct(float start, float end, float time, TweenFunction tweenF)
    {
      _distance = end - start;
      _original = start;
      _current = start;
      _totalDuration = time;
      _tweenF = tweenF;
    }

    public void Update(float elapsedTime)
    {
      _totalTimePassed += elapsedTime;
      _current = _tweenF(_totalTimePassed, _original, _distance, _totalDuration);

      if (_totalTimePassed > _totalDuration)
      {
        _current = _original + _distance;
        _finished = true;
      }
    }

    public static float Linear(float timePassed, float start, float distance, float duration)
    {
      return distance * timePassed / duration + start;
    }

    public static float EaseOutExpo(float timePassed, float start, float distance, float duration)
    {
      if (timePassed == duration)
        return start + distance;
      return (float)(distance * (-Math.Pow(2, -10 * timePassed / duration) + 1) + start);
    }

    public static float EaseInExpo(float timePassed, float start, float distance, float duration)
    {
      if (timePassed == 0)
        return start;
      return (float)(distance * Math.Pow(2, 10 * (timePassed / duration - 1)) + start);
    }

    public static float EaseOutCirc(float timePassed, float start, float distance, float duration)
    {
      return (float)(distance * Math.Sqrt(1 - (timePassed = timePassed / duration - 1) * timePassed) + start);
    }

    public static float EaseInCirc(float timePassed, float start, float distance, float duration)
    {
      return (float)(-distance * (Math.Sqrt(1 - (timePassed /= duration) * timePassed) - 1) + start);
    }
  }
}