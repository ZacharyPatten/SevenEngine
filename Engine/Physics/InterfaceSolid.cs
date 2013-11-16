// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use with the following condition: any code using or originally taken 
// from the SevenEngine project must include citation to its original author(s) located at the
// top of each source code file, or you may include a reference to the SevenEngine project as
// a whole but you must include the current SevenEngine official website URL and logo.
// - Thanks.  :)  (support: seven@sevenengine.com)

// Author(s):
// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com
// Last Edited: 11-16-13

using SevenEngine.Mathematics;

namespace SevenEngine.Physics
{
  /// <summary>Represents everything on a physical level (no graphic data here, just plain old physics).</summary>
  public interface InterfaceSolid
  {
    float Mass { get; }
    // int Element { get; }
    // int Compound { get; }
    // Shape Shape { get; }
    Vector Position { get; set; }
    Vector Veleocity { get; set; }
    Quaternion Orientation { get; set; }
    Quaternion AngularVelocity { get; set; }

    float Tempurature { get; set; }
    float Magnetism { get; }
    float Radioactivity { get; }
    float Conductivity { get; }
    float StructuralIntegrity { get; }
    float Elasticity { get; }
    // Vector AeroDynamicAngle { get; }
  }
}