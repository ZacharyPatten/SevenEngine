using System;

using SevenEngine.Mathematics;
using SevenEngine.DataStructures;
using SevenEngine.StaticModels;

namespace Game.Units
{
  public class KillemKamakazi : Kamakazi
  {
    const bool near = true;
    Unit _target;
    int _move;

   public KillemKamakazi(string id, StaticModel staticModel) : base(id, staticModel) { }

    public override void AI(float elapsedTime, OctreeLinked<Unit, string> octree)
    {
      if (IsDead == false)
      {
        // Targeting
        #region Farthest
        if (!near && (_target == null || _target.IsDead || _move > 20))
        {
          float longest = float.MinValue;
          _move = 0;
          octree.Traverse
          (
            (Unit current) =>
            {
              if ((current is KillemKamakazi || current is KillemMelee || current is KillemRanged) && !current.IsDead)
              {
                float length = (current.Position - Position).Length;
                if (_target == null || _target.IsDead)
                {
                  _target = current;
                  longest = length;
                }
                else if (length > longest)
                {
                  _target = current;
                  longest = length;
                }
              }
            }
          );
        }
        #endregion
        #region Nearest
        else if (near && (_target == null || _target.IsDead || _move > 20))
        {
          _move = 0;
          float nearest = float.MinValue;
          octree.Traverse
          (
            (Unit current) =>
            {
              if ((current is ZackKamakazi || current is ZackMelee || current is ZackRanged) && !current.IsDead)
              {
                float length = (current.Position - Position).Length;
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
        #endregion
        // Attacking
        else if (Foundations.Abs((Position - _target.Position).Length) < _attackRange / 2)
        {
          Attack(octree);
          _move = 0;
        }
        // Moving
        else
        {
          Vector direction = _target.Position - Position;
          Position.X += (direction.X / direction.Length) * MoveSpeed;
          Position.Y += (direction.Y / direction.Length) * MoveSpeed;
          Position.Z += (direction.Z / direction.Length) * MoveSpeed;
          _move++;
        }
        StaticModel.Orientation.W += .1f;
      }
    }
  }
}
