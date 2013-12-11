using System;

using Game.States;
using SevenEngine.Mathematics;
using SevenEngine.DataStructures;
using SevenEngine.StaticModels;

namespace Game.Units
{
  public class ZackRanged : Ranged
  {
    Unit _target;

    public ZackRanged(string id, StaticModel staticModel) : base(id, staticModel) { }

    public override void AI(Octree<Unit, string> octree)
    {
      // Targeting
      if (_target == null || _target.IsDead)
      {
        octree.TraverseBreakable
        (
          (Unit current) =>
          {
            if ((current is KillemKamakazi || current is KillemMelee || current is KillemRanged) && !_target.IsDead)
              _target = current;
            return false;
          }
        );
      }
      // Attacking
      else if (Foundations.Abs((Position - _target.Position).Length) < _attackRange)
      {
        if (Attack(_target))
          _target = null;
      }
      // Moving
      else
      {

      }
    }
  }
}