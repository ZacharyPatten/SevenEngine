using System;

using SevenEngine.Mathematics;
using SevenEngine.DataStructures;
using SevenEngine.StaticModels;

namespace Game.Units
{
  public class ZackKamakazi : Kamakazi
  {
    const bool near = true;
    Unit _target;
    int _move;

    public ZackKamakazi(string id, StaticModel staticModel) : base(id, staticModel) { }

    public override void AI(float elapsedTime, OctreeLinked<Unit, string> octree)
    {
      if (IsDead == false)
      {
        // Targeting
        if (near && (_target == null || _target.IsDead || _move > 20))
        {
          _move = 0;
          float nearest = float.MinValue;
          octree.Traverse
          (
            (Unit current) =>
            {
              if ((current is KillemKamakazi || current is KillemMelee || current is KillemRanged) && !current.IsDead)
              {
                float length = (current.Position - Position).LengthSquared();
                if (_target == null || _target.IsDead)
                {
                  _target = current;
                  nearest = length;
                }
                else if (length < nearest)
                {
                  _target = current;
                  nearest = length;
                }
              }
            }
          );
        }
        // Attacking
        else if (Calc.Abs((Position - _target.Position).LengthSquared()) < _attackRangedSquared / 2)
        {
          Attack(octree);
          _move = 0;
        }
        // Moving
        else
        {
          Position = Vector.MoveTowardsPosition(Position, _target.Position, MoveSpeed);
          _move++;
        }
        StaticModel.Orientation.W += .1f;
      }
    }
  }
}
