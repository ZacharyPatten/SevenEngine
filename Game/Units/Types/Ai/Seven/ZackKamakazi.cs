using System;

using Seven.Mathematics;
using Seven.Structures;
using SevenEngine.StaticModels;

namespace Game.Units
{
  public class ZackKamakazi : Kamakazi
  {
    const bool near = true;
    Unit _target;
    int _move;

    public ZackKamakazi(string id, StaticModel staticModel) : base(id, staticModel) { }

    public override void AI(float elapsedTime, Omnitree<Unit, double> octree)
    {
      if (IsDead == false)
      {
        // Targeting
        if (near && (_target == null || _target.IsDead || _move > 20))
        {
          _move = 0;
          float nearest = float.MinValue;
          octree.Foreach
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
        else if (Logic.Abs((Position - _target.Position).LengthSquared()) < _attackRangedSquared / 2)
        {
          Attack(octree);
          _move = 0;
        }
        // Moving
        else
        {
          Position += (_target.Position - Position).Normalize() * MoveSpeed; 
          _move++;
        }
        StaticModel.Orientation.W += .1f;
        //StaticModel.Orientation.Z += .1f;
      }
    }
  }
}
