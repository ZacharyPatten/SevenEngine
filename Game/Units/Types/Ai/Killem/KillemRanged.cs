using System;

using SevenEngine.Mathematics;
using SevenEngine.DataStructures;
using SevenEngine.StaticModels;

namespace Game.Units
{
  public class KillemRanged : Ranged
  {
    Unit _target;

    public KillemRanged(string id, StaticModel staticModel) : base(id, staticModel) { }

    public override void AI(float elapsedTime, OctreeLinked<Unit, string> octree)
    {
      MoveTowards(new Vector(0, 0, 10000));

    }
  }
}
