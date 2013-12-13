using System;

using Game.States;
using SevenEngine.Imaging;
using SevenEngine.Mathematics;
using SevenEngine.DataStructures;
using SevenEngine.StaticModels;

namespace Game.Units
{
  public class KillemMelee : Melee
  {
    Unit _target;
    float _time = 0;
    const float _delay = 4000;

    public KillemMelee(string id, StaticModel staticModel) : base(id, staticModel) { }

    public override void AI(float elapsedTime, OctreeLinked<Unit, string> octree)
    {
      if (_time < _delay)
        _time += elapsedTime;
      if (IsDead == false)
      {
        if (_target == null || _target.IsDead)
        {
          float shortest = float.MaxValue;
          octree.Traverse
          (
            (Unit current) =>
            {
              if ((current is ZackMelee || current is ZackRanged) && !current.IsDead)
              {
                if (_target == null || _target.IsDead || _target is ZackKamakazi)
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
              else if (current is ZackKamakazi && !current.IsDead)
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
          AiBattle.lines.Add(new Link3<Vector, Vector, Color>(
            new Vector(Position.X, Position.Y, Position.Z),
            new Vector(_target.Position.X, _target.Position.Y, _target.Position.Z),
            Color.Blue));
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
      }
    }
  }
}
