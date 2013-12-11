using System;

using Game.States;

using SevenEngine.StaticModels;

namespace Game.Units
{
  public abstract class Ranged : Unit
  {
    private readonly int _healthMin = 1;
    private readonly int _healthMax = 10;
    private readonly int _damageMin = 1;
    private readonly int _damageMax = 10;
    private readonly int _viewDistanceMin = 1;
    private readonly int _viewDistanceMax = 10000;
    private readonly int _attackRangeMin = 1;
    private readonly int _attackRangeMax = 100;
    private readonly int _moveSpeedMin = 1;
    private readonly int _moveSpeedMax = 100;

    public Ranged(string id, StaticModel staticModel) : base(id, staticModel)
    {
      Random random = new Random();
      _health = random.Next(_healthMin, _healthMax);
      _damage = random.Next(_damageMin, _damageMax);
      _viewDistance = random.Next(_viewDistanceMin, _viewDistanceMax) * GameState.MeterLength;
      _attackRange = random.Next(_attackRangeMin, _attackRangeMax) * GameState.MeterLength;
      _moveSpeed = random.Next(_moveSpeedMin, _moveSpeedMax) / 10000f * GameState.MeterLength;
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
