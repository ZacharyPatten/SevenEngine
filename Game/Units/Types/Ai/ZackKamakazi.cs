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

    public override void AI(OctreeLinked<Unit, string> octree)
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
        Attack(octree);
        _isDead = true;
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
