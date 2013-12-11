using System;

using SevenEngine.StaticModels;

namespace Game.Units
{
  public abstract class Melee : Unit
  {
    private readonly int _healthMin = 1;
    private readonly int _healthMax = 10;
    private readonly int _damageMin = 1;
    private readonly int _damageMax = 10;
    private readonly int _viewDistanceMin = 1;
    private readonly int _viewDistanceMax = 10000;
    private readonly int _moveSpeedMin = 1;
    private readonly int _moveSpeedMax = 10;

    public Melee(string id, StaticModel staticModel) : base(id, staticModel)
    {
      Random random = new Random();
      _health = random.Next(_healthMin, _healthMax);
      _damage = random.Next(_damageMin, _damageMax);
      _viewDistance = random.Next(_viewDistanceMin, _viewDistanceMax);
      _moveSpeed = random.Next(_moveSpeedMin, _moveSpeedMax);
    }

    protected bool Attack(Unit defending)
    {
      defending.Health -= _damage;
      if (defending.Health <= 0)
      {
        defending.IsDead = true;
        return true;
      }
      return false;
    }
  }
}
