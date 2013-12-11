using System;

using SevenEngine.DataStructures;
using SevenEngine.StaticModels;

namespace Game.Units
{
  public abstract class Kamakazi : Unit
  {
    private readonly int _healthMin = 1;
    private readonly int _healthMax = 10;
    private readonly int _damageMin = 1;
    private readonly int _damageMax = 10;
    private readonly int _viewDistanceMin = 1;
    private readonly int _viewDistanceMax = 10000;
    private readonly int _moveSpeedMin = 1;
    private readonly int _moveSpeedMax = 10;

    public Kamakazi(string id, StaticModel staticModel) : base(id, staticModel)
    {
      Random random = new Random();
      _health = random.Next(_healthMin, _healthMax);
      _damage = random.Next(_damageMin, _damageMax);
      _viewDistance = random.Next(_viewDistanceMin, _viewDistanceMax);
      _moveSpeed = random.Next(_moveSpeedMin, _moveSpeedMax) / 1000f;
    }

    protected void Attack(Octree<Unit, string> octree)
    {
      octree.Traverse
      (
        (Unit unit) => {  },
        _attackRange, _attackRange, _attackRange, _attackRange, _attackRange, _attackRange
      );
      _isDead = true;
    }
  }
}
