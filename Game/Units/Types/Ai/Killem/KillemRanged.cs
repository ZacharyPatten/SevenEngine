using System;

using SevenEngine.Mathematics;
using SevenEngine.DataStructures;
using SevenEngine.StaticModels;

namespace Game.Units
{
  public class KillemRanged : Ranged
  {
    Unit _target;
    AllyState _allyState = AllyState.Waiting;
    VectorV _towards;
    KillemMelee _leader;
    public AllyState State { get { return _allyState; } set { _allyState = value; } }
    public KillemRanged(string id, StaticModel staticModel) : base(id, staticModel) 
    {
      _allyState = AllyState.Waiting;
    }

    public override void AI(float elapsedTime, OctreeLinked<Unit, string> octree)
    {
    MoveTowards(new Vector(0, 0, -10000));
      if (_allyState == AllyState.Matched)
      {
      //  MoveTowards(_leader.Position);
      }
    }


    public void RegisterLeader(KillemMelee leader)
    {
      _allyState = AllyState.Matched;
      _leader = leader;
    }

  }
}
