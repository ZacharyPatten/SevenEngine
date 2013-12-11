using System;

using SevenEngine.Mathematics;
using SevenEngine.DataStructures;
using SevenEngine.StaticModels;

namespace Game.Units
{
  public class KillemMelee : Melee
  {
    Unit _target;

    public KillemMelee(string id, StaticModel staticModel) : base(id, staticModel) { }

    public override void AI(Octree<Unit, string> octree)
    {

    }
  }
}
