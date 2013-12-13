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

    KillemMelee _leader;
    public AllyState State { get { return _allyState; } set { _allyState = value; } }
    public KillemKamakazi(string id, StaticModel staticModel) : base(id, staticModel) 
    {
      _allyState = AllyState.Waiting;
    }

    public override void AI(float elapsedTime, OctreeLinked<Unit, string> octree)
    {
      if (_allyState == AllyState.MovingToSquad)
      {
          MoveTowards(_leader.Position);
      }

      Unit closeBomb = null;
      Unit closeRange = null;
      Unit closeMelee = null;
      octree.Traverse((Unit unit) =>
      {
        if (closeBomb == null || (unit is ZackKamakazi && this.DistanceTo(unit) < this.DistanceTo(closeBomb)))
        {
          closeBomb = unit;
        }
        else if (closeRange == null || (unit is ZackRanged && this.DistanceTo(unit) < this.DistanceTo(closeRange)))
        {
          closeRange = unit;
        }
        else if (closeMelee == null || (unit is ZackMelee && this.DistanceTo(unit) < this.DistanceTo(closeMelee)))
        {
          closeMelee = unit;
        }
      });

    }
    public void RegisterLeader(KillemMelee leader)
    {
      _allyState = AllyState.Matched;
      _leader = leader;
    }

    public void UnregisterLeader()
    {
      _allyState = AllyState.Waiting;
      _leader = null;
    }

    public void FindCloseUnits(OctreeLinked<Unit, string> octree)
    {
      Unit closeBomb = null;
      Unit closeRange = null;
      Unit closeMelee = null;
      octree.Traverse((Unit unit) =>
      {
        if (closeBomb == null || (unit is ZackKamakazi && this.DistanceTo(unit) < this.DistanceTo(closeBomb)))
        {
          closeBomb = unit;
        }
        else if (closeRange == null || (unit is ZackRanged && this.DistanceTo(unit) < this.DistanceTo(closeRange)))
        {
          closeRange = unit;
        }
        else if (closeMelee == null || (unit is ZackMelee && this.DistanceTo(unit) < this.DistanceTo(closeMelee)))
        {
          closeMelee = unit;
        }
      });

      _leader.RegisterClosest(closeBomb, closeRange, closeBomb);
    }


  }
}