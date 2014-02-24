using System;
using Game.States;
using SevenEngine;
using SevenEngine.Imaging;
using SevenEngine.StaticModels;
using SevenEngine.Mathematics;
using SevenEngine.DataStructures;

namespace Game.Units
{
  public abstract class Melee : Unit
  {
    private const int _healthMin = 400;
    private const int _healthMax = 700;
    private const int _damageMin = 10;
    private const int _damageMax = 20;
    private const int _viewDistanceMin = 1;
    private const int _viewDistanceMax = 10000;
    private const int _moveSpeedMin = 70;
    private const int _moveSpeedMax = 110;

    public Melee(string id, StaticModel staticModel) : base(id, staticModel)
    {
      Random random = new Random();
      _health = random.Next(_healthMin, _healthMax);
      _damage = random.Next(_damageMin, _damageMax);
      _viewDistance = random.Next(_viewDistanceMin, _viewDistanceMax);
      _moveSpeed = random.Next(_moveSpeedMin, _moveSpeedMax) / 20f;
      _attackRange = 60;
      _attackRangedSquared = _attackRange * _attackRange;
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
