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
      MoveTowards(new Vector(0, 0, 10000));

    }
  }
}