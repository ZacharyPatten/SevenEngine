using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Engine.Mathematics;

namespace Engine.Models
{
  public class Bone
  {
    List<Bone> _children;
    Matrix _transformation;

    public Bone()
    {
      _children = new List<Bone>();
      _transformation = new Matrix();
    }
  }
}
