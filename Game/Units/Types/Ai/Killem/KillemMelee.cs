using System;

using Game.States;
using SevenEngine.Imaging;
using SevenEngine.Mathematics;
using SevenEngine.DataStructures;
using SevenEngine.StaticModels;

namespace Game.Units
{
  public delegate Vector VectorV();
  public class KillemMelee : Melee
  {
    Unit _target;
    AllyState _rangeAllyState;
    AllyState _bombAllyState;
    KillemKamakazi _bombAlly;
    KillemRanged _rangeAlly;
    private int _minAllyRange = 5000;
    static List<Unit> _targeted = new ListArray<Unit>(90);
    public Unit Target { get { return _target; } }
    public KillemMelee(string id, StaticModel staticModel)
      : base(id, staticModel)
    {
      _rangeAllyState = AllyState.Finding;
      _bombAllyState = AllyState.Finding;
    }
    public override void AI(float elapsedTime, OctreeLinked<Unit, string> octree)
    {
      octree.Traverse((Unit unit) =>
      {
        if (unit is KillemRanged)
          FindRanged((KillemRanged)unit);
        if (unit is KillemKamakazi)
          FindBombed((KillemKamakazi)unit);
      });
      if (_bombAlly != null && !AllyInRange(_bombAlly))
        _bombAlly.State = AllyState.MovingToSquad;
      if (_rangeAlly != null && !AllyInRange(_rangeAlly))
        _rangeAlly.State = AllyState.MovingToSquad;
      
      closeUnits.Traverse((Unit unit) => {
        if(!_targeted.Contains(unit))
        {
          if (_rangeAlly != null && _rangeAlly.Target == null)
          {
            _rangeAlly.SetTarget(unit);
          }
          else if (_rangeAlly != null && _rangeAlly.Target != null && 
            unit.DistanceTo(_rangeAlly) < _rangeAlly.DistanceTo(_rangeAlly.Target))
          {
            _rangeAlly.SetTarget(unit);
          }
          else if (_rangeAlly != null && _rangeAlly.Target == null)
          {
            _rangeAlly.SetTarget(unit);
          }
          else if (_rangeAlly != null)
          {

          }

        }
      });
    }

    List<Unit> closeUnits = new ListArray<Unit>(10);
    public void RegisterClosest(Unit unit1, Unit unit2, Unit unit3)
    {
      if(!closeUnits.Contains(unit1))
        closeUnits.Add(unit1);
      if(!closeUnits.Contains(unit2))
        closeUnits.Add(unit2);
      if(!closeUnits.Contains(unit3))
        closeUnits.Add(unit3);
    }


    public void FindRanged(KillemRanged unit)
    {
      if (_rangeAlly == null && unit.State == AllyState.Waiting)
      {
        _rangeAllyState = AllyState.Matched;
        _rangeAlly = unit;
        unit.RegisterLeader(this);
      }
      else if (unit.State == AllyState.Waiting && CloserMoveSpeed((Unit)unit, (Unit)_rangeAlly))
      {
        _rangeAlly.UnregisterLeader();
        unit.RegisterLeader(this);
        _rangeAlly = unit;
        unit.RegisterLeader(this);
      }
    }

    public void FindBombed(KillemKamakazi unit)
    {
      if (_bombAlly == null && unit.State == AllyState.Waiting)
      {
        _bombAllyState = AllyState.Matched;
        _bombAlly = unit;
        unit.RegisterLeader(this);
      }
      else if (unit.State == AllyState.Waiting && CloserMoveSpeed((Unit)unit, (Unit)_bombAlly))
      {
        Console.WriteLine(unit.MoveSpeed + " Is closer to " + _moveSpeed + "than " + _bombAlly.MoveSpeed);
        _bombAlly.UnregisterLeader();
        unit.RegisterLeader(this);
        _bombAlly = unit;
        unit.RegisterLeader(this);
      }
    }

    public void ReportDeath(Unit unit)
    {
      if(closeUnits.Contains(unit))
        closeUnits.RemoveFirst(unit);
    }
    private bool CloserMoveSpeed(Unit newUnit, Unit currentUnit)
    {
      return Math.Abs(newUnit.MoveSpeed - _moveSpeed) < Math.Abs(currentUnit.MoveSpeed - _moveSpeed);

    }

    private bool AllyInRange(Unit unit)
    {
      return DistanceTo(unit) < _minAllyRange;
    }

  }
}
