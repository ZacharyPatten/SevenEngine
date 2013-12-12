using System;
using SevenEngine;
using SevenEngine.Imaging;
using Game.States;
using SevenEngine.DataStructures;
using SevenEngine.StaticModels;
using SevenEngine.Mathematics;

namespace Game.Units
{
  public abstract class Ranged : Unit
  {
    private readonly int _healthMin = 1;
    private readonly int _healthMax = 1000;
    private readonly int _damageMin = 1;
    private readonly int _damageMax = 10;
    private readonly int _viewDistanceMin = 1;
    private readonly int _viewDistanceMax = 10000;
    private readonly int _attackRangeMin = 100;
    private readonly int _attackRangeMax = 150;
    private readonly int _moveSpeedMin = 0;
    private readonly int _moveSpeedMax = 10;

    public Ranged(string id, StaticModel staticModel) : base(id, staticModel)
    {
      Random random = new Random();
      _health = random.Next(_healthMin, _healthMax);
      _damage = random.Next(_damageMin, _damageMax);
      _viewDistance = random.Next(_viewDistanceMin, _viewDistanceMax) * GameState.MeterLength;
      _attackRange = random.Next(_attackRangeMin, _attackRangeMax) * GameState.MeterLength;
      _moveSpeed = random.Next(10 + _moveSpeedMin, 10 + _moveSpeedMax) / 20f * GameState.MeterLength;
    }

    protected bool Attack(Unit defending)
    {
      defending.Health -= _damage;
      AiBattle.lines.Add(new Link3<Vector, Vector, Color>(Position, defending.Position, Color.Blue));
      if (defending.Health <= 0)
      {
        defending.IsDead = true;
        return true;
      }
      return false;
    }
  }
}
