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
    private const int _healthMin = 250;
    private const int _healthMax = 500;
    private const int _damageMin = 6;
    private const int _damageMax = 12;
    private const int _viewDistanceMin = 1;
    private const int _viewDistanceMax = 10000;
    private const int _moveSpeedMin = 50;
    private const int _moveSpeedMax = 70;

    public Melee(string id, StaticModel staticModel) : base(id, staticModel)
    {
      Random random = new Random();
      _health = random.Next(_healthMin, _healthMax);
      _damage = random.Next(_damageMin, _damageMax);
      _viewDistance = random.Next(_viewDistanceMin, _viewDistanceMax);
      _moveSpeed = random.Next(_moveSpeedMin, _moveSpeedMax) / 20f;
      _attackRange = 5;
    }

    protected bool Attack(Unit defending)
    {
      defending.Health -= _damage;
      AiBattle.lines.Add(new Link3<Vector, Vector, Color>(
        new Vector(Position.X, Position.Y, Position.Z),
        new Vector(defending.Position.X, defending.Position.Y, defending.Position.Z),
        Color.Violet));
      if (defending.Health <= 0)
      {
        defending.IsDead = true;
        return true;
      }
      return false;
    }
  }
}
