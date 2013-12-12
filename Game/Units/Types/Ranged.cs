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
    private const int _healthMin = 50;
    private const int _healthMax = 100;
    private const int _damageMin = 5;
    private const int _damageMax = 10;
    private const int _viewDistanceMin = 1;
    private const int _viewDistanceMax = 10000;
    private const int _attackRangeMin = 100;
    private const int _attackRangeMax = 150;
    private const int _moveSpeedMin = 1;
    private const int _moveSpeedMax = 5;

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
