using System;

using SevenEngine.Mathematics;
using SevenEngine.DataStructures;
using SevenEngine.Models;

namespace Game.Units.Types.Ai
{
  public class ZackRanged : Ranged
  {
    public ZackRanged(string id, StaticModel staticModel) : base(id, staticModel) { }

    public override void AI(ListArray<Unit> unitsInView)
    {
      for (int i = 0; i < unitsInView.Count; i++)
      {
        if (unitsInView[i] is KillemMelee)
        {
          Position += (unitsInView[i].Position - Position) * MoveSpeed;
        }
      }
    }
  }
}