using System;

using SevenEngine.Mathematics;
using SevenEngine.DataStructures;
using SevenEngine.StaticModels;

namespace Game.Units
{
  public class ZackMelee : Melee
  {
    Unit _target;
    float _time = 0;
    const float _delay = 4000;

    public ZackMelee(string id, StaticModel staticModel) : base(id, staticModel) { _time = 0; }

    public override void AI(float elapsedTime, OctreeLinked<Unit, string> octree)
    {
      if (_time < _delay)
        _time += elapsedTime;
      if (IsDead == false)
      {
        // Targeting
        /*if (_target == null || _target.IsDead)
        {
          float shortest = float.MaxValue;
          octree.Traverse
          (
            (Unit current) =>
            {
              if ((current is KillemKamakazi || current is KillemMelee || current is KillemRanged) && !current.IsDead)
              {
                if (!(current is KillemKamakazi))
                {
                  float length = (current.Position - Position).Length;
                  if (_target == null || _target.IsDead)
                  {
                    _target = current;
                    shortest = length;
                  }
                  else if (length < shortest)
                  {
                    _target = current;
                    shortest = length;
                  }
                }
                else
                {
                  if (_target == null || _target.IsDead)
                  {
                    float length = (current.Position - Position).Length;
                    if (_target == null || _target.IsDead)
                    {
                      _target = current;
                      shortest = length;
                    }
                    else if (length < shortest)
                    {
                      _target = current;
                      shortest = length;
                    }
                  }
                }
              }
            }
          );
        }*/
        if (_target == null || _target.IsDead)
        {
          float shortest = float.MaxValue;
          octree.Traverse
          (
            (Unit current) =>
            {
              if ((current is KillemMelee || current is KillemRanged) && !current.IsDead)
              {
                if (_target == null || _target.IsDead || _target is KillemKamakazi)
                {
                  _target = current;
                  shortest = (current.Position - Position).Length;
                }
                else
                {
                  float length = (current.Position - Position).Length;
                  if (length < shortest)
                  {
                    _target = current;
                    shortest = length;
                  }
                }
              }
              else if (current is KillemKamakazi && !current.IsDead)
              {
                if (_target == null || _target.IsDead)
                {
                  _target = current;
                  shortest = (current.Position - Position).Length;
                }
                else
                {
                  float length = (current.Position - Position).Length;
                  if (length < shortest)
                  {
                    _target = current;
                    shortest = length;
                  }
                }
              }
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
        else if (_time > _delay)
        {
          Vector direction = _target.Position - Position;
          Position.X += (direction.X / direction.Length) * MoveSpeed;
          Position.Y += (direction.Y / direction.Length) * MoveSpeed;
          Position.Z += (direction.Z / direction.Length) * MoveSpeed;
        }
        this.StaticModel.Orientation.W+=.1f;
      }
    }
  }
}
