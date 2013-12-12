using System;

using SevenEngine.Mathematics;
using SevenEngine.DataStructures;
using SevenEngine.StaticModels;

namespace Game.Units
{
  public class KillemMelee : Melee
  {
    Unit _target;
    float _time = 0;
    const float _delay = 4000;

    public KillemMelee(string id, StaticModel staticModel) : base(id, staticModel) { }

    public override void AI(float elapsedTime, OctreeLinked<Unit, string> octree)
    {
      MoveTowards(new Vector(-10000, 0, 0));
      
    }
  }
}
