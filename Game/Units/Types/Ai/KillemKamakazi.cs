using System;

using SevenEngine.Mathematics;
using SevenEngine.DataStructures;
using SevenEngine.StaticModels;

namespace Game.Units
{
  public class KillemKamakazi : Kamakazi
  {
    Unit _target;

    public KillemKamakazi(string id, StaticModel staticModel) : base(id, staticModel) { }

    public override void AI(Octree<Unit, string> octree)
    {

    }
  }
}