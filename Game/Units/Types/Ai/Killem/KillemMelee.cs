using System;

using SevenEngine.Mathematics;
using SevenEngine.DataStructures;
using SevenEngine.StaticModels;

namespace Game.Units
{
  public delegate Vector VectorV();
  public class KillemMelee : Melee
  {
    Unit _target;
    AllyState _rangedAlly;
    AllyState _bombAlly;
    public KillemMelee(string id, StaticModel staticModel) : base(id, staticModel)
    {
      _rangedAlly = AllyState.Finding;
      _bombAlly = AllyState.Finding;
    }
    public override void AI(float elapsedTime, OctreeLinked<Unit, string> octree)
    {
      MoveTowards(new Vector(0,0,10000));
      octree.Traverse((Unit unit) =>
      {
        if(_rangedAlly == AllyState.Finding && unit is KillemRanged)
          FindRanged((KillemRanged)unit);
        if(_bombAlly == AllyState.Finding && unit is KillemKamakazi) 
          FindBombed((KillemKamakazi)unit);
      });
    }

    public void FindRanged(KillemRanged unit)
    {
      if (unit.State == AllyState.Waiting)
      {
        _rangedAlly = AllyState.Matched;
        unit.RegisterLeader(this);
      }
    }

    public void FindBombed(KillemKamakazi unit)
    {
      if (unit.State == AllyState.Waiting)
      {
        _bombAlly = AllyState.Matched;
        unit.RegisterLeader(this);
      }
    }
  }
}
