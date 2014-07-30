// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use under the following condition: any code using or originally taken 
// from the SevenEngine project must include citation to its original author(s) located at the
// top of each source code file, or you may include a reference to the SevenEngine project as
// a whole but you must include the current SevenEngine official website URL and logo.
// - Thanks.  :)  (support: seven@sevenengine.com)

// Author(s):
// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com
// Last Edited: 11-28-13

using System;
using Seven.Mathematics;

namespace SevenEngine.Physics
{
  public enum EnumMatterState { Solid, Liquid, Gas, Plasma }

  public class Matter
  {
    protected Material _substance;
    //protected Shape _space;

    protected Vector<float> _acceleration;
    protected Vector<float> _velocity;

    protected Quaternion<float> _angularAccelteration;
    protected Quaternion<float> _angularVelocity;

    protected float _tempurature;
  }
}