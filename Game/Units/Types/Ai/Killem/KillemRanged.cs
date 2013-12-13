using System;

using Game.States;
using SevenEngine.Imaging;
using SevenEngine.Mathematics;
using SevenEngine.DataStructures;
using SevenEngine.StaticModels;

namespace Game.Units
{
  public class KillemRanged : Ranged
  {
    Unit _target;
    AllyState _allyState = AllyState.Waiting;
    EnemyState _enemyState = EnemyState.Normal;
    VectorV _towards;
    KillemMelee _leader;
    public AllyState State { get { return _allyState; } set { _allyState = value; } }
    public Unit Target { get { return _target; } }
    public KillemRanged(string id, StaticModel staticModel) : base(id, staticModel) 
    {
      _allyState = AllyState.Waiting;
    }

    public override void AI(float elapsedTime, OctreeLinked<Unit, string> octree)
    {
      if (_allyState == AllyState.MovingToSquad  /*&& _enemyState != EnemyState.Attacking*/)
      {
        MoveTowards(_leader.Position);
      }
      else if (_enemyState == EnemyState.Attacking)
      {
        MoveTowards(_target.Position);
        if ((Position - _target.Position).Length < _attackRange)
        {
          Attack(_target);
        }

        if (_target.IsDead)
        {
          _enemyState = EnemyState.Normal;
          if (_leader != null)
            _leader.ReportDeath(_target);
          _target = null;
        }
      }
      if(_leader != null)
      {
        FindCloseUnits(octree);
      }
    }



    public void FindCloseUnits(OctreeLinked<Unit, string> octree)
    {
      Unit closeBomb = null;
      Unit closeRange = null;
      Unit closeMelee = null;
      octree.Traverse((Unit unit) =>
      {
        if (unit != null)
        {
          if (closeBomb == null || (!unit.IsDead && unit is ZackKamakazi && this.DistanceTo(unit) < this.DistanceTo(closeBomb)))
          {
            closeBomb = unit;
          }
          else if (closeRange == null || (!unit.IsDead && unit is ZackRanged && this.DistanceTo(unit) < this.DistanceTo(closeRange)))
          {
            closeRange = unit;
          }
          else if (closeMelee == null || (!unit.IsDead && unit is ZackMelee && this.DistanceTo(unit) < this.DistanceTo(closeMelee)))
          {
            closeMelee = unit;
          }
        }
      });

      _leader.RegisterClosest(closeBomb, closeRange, closeBomb);
    }


    public void RegisterLeader(KillemMelee leader)
    {
      _allyState = AllyState.Matched;
      _leader = leader;
    }

    public void SetTarget(Unit target)
    {
      _enemyState = EnemyState.Attacking;
      _target = target;
    }

    public void UnregisterLeader()
    {
      _allyState = AllyState.Waiting;
      _leader = null;
    }
  }
}
