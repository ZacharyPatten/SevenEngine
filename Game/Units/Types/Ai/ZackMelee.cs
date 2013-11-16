using System;

using SevenEngine.Mathematics;
using SevenEngine.DataStructures;
using SevenEngine.StaticModels;

namespace Game.Units.Types.Ai
{
  public class ZackMelee : Melee
  {
    public ZackMelee(string id, StaticModel staticModel) : base(id, staticModel) { }

    public override void AI(ListArray<Unit> unitsInView)
    {

    }
  }
}
