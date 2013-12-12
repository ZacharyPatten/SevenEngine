using System;

using SevenEngine.Mathematics;
using SevenEngine.DataStructures;
using SevenEngine.StaticModels;

namespace Game.Units
{
  public class ZackKamakazi : Kamakazi
  {
    Unit _target;

    public ZackKamakazi(string id, StaticModel staticModel) : base(id, staticModel) { }

    public override void AI(float elapsedTime, OctreeLinked<Unit, string> octree)
    {
      if (IsDead == false)
      {
        // Targeting
        if (_target == null || _target.IsDead)
        {
          octree.TraverseBreakable
          (
            (Unit current) =>
            {
              if ((current is KillemKamakazi || current is KillemMelee || current is KillemRanged) && !current.IsDead)
              {
                _target = current;
                return false;
              }
              return true;
            }
          );
        }
        // Attacking
        else if (Foundations.Abs((Position - _target.Position).Length) < _attackRange)
        {
          Attack(octree);
        }
        // Moving
        else
        {
          Vector direction = _target.Position - Position;
          Position.X += (direction.X / direction.Length) * (MoveSpeed + 5);
          Position.Y += (direction.Y / direction.Length) * (MoveSpeed + 5);
          Position.Z += (direction.Z / direction.Length) * (MoveSpeed + 5);
        }
      }
    }
  }
}
