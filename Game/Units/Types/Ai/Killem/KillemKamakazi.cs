using System;

using SevenEngine.Mathematics;
using SevenEngine.DataStructures;
using SevenEngine.StaticModels;

namespace Game.Units
{
  public class KillemKamakazi : Kamakazi
  {
    Unit _target;
    EnemyState _enemyState;
    AllyState _allyState;

    public KillemKamakazi(string id, StaticModel staticModel) : base(id, staticModel) { }

    public override void AI(float elapsedTime, OctreeLinked<Unit, string> octree)
    {
      //MoveTowards(new Vector(-10000, 0, 0));
      if (IsDead == false)
      {
        // Targeting
        if (_target == null || _target.IsDead)
        {
          float longest = float.MinValue;
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
        // Attacking
        else if (Foundations.Abs((Position - _target.Position).Length) < _attackRange / 2)
        {
          Attack(octree);
        }
        // Moving
        else
        {
          Vector direction = _target.Position - Position;
          Position.X += (direction.X / direction.Length) * MoveSpeed;
          Position.Y += (direction.Y / direction.Length) * MoveSpeed;
          Position.Z += (direction.Z / direction.Length) * MoveSpeed;
        }
      }
    }
  }
}