using System;
using SevenEngine;
using SevenEngine.DataStructures;
using SevenEngine.StaticModels;
using Game.States;

namespace Game.Units
{
  public class Explosion
  {
    StaticModel _explosion;
    float _time;

    public StaticModel Model { get { return _explosion; } }
    public float Time { get { return _time; } set { _time = value; } }

    public Explosion()
    {
      _time = 0;
      _explosion = StaticModelManager.GetModel("MushroomCloud");
    }
  }

  public abstract class Kamakazi : Unit
  {
    private readonly int _healthMin = 1;
    private readonly int _healthMax = 1000;
    private readonly int _damageMin = 10;
    private readonly int _damageMax = 100;
    private readonly int _viewDistanceMin = 1;
    private readonly int _viewDistanceMax = 10000;
    private readonly int _moveSpeedMin = 1;
    private readonly int _moveSpeedMax = 10;
    private readonly int _attackRangeMin = 20;
    private readonly int _attackRangeMax = 40;

    private bool _exploded = false; 

    public override bool IsDead
    {
      get { return _isDead; }
      set
      {
        if (value == true && !_exploded)
        {
          Explosion explosion = new Explosion();
          explosion.Model.Position = Position;
          AiBattle.explosions.Add(explosion);
          _exploded = true;
          Attack(AiBattle._octree);
        }
        _isDead = value;
      }
    }

    public Kamakazi(string id, StaticModel staticModel) : base(id, staticModel)
    {
      Random random = new Random();
      _attackRange = random.Next(_attackRangeMin, _attackRangeMax) * GameState.MeterLength;
      _health = random.Next(_healthMin, _healthMax);
      _damage = random.Next(_damageMin, _damageMax);
      _viewDistance = random.Next(_viewDistanceMin, _viewDistanceMax);
      _moveSpeed = random.Next(10 + _moveSpeedMin, 10 + _moveSpeedMax) / 20f * GameState.MeterLength;
    }

    protected void Attack(OctreeLinked<Unit, string> octree)
    {
      octree.Traverse
      (
        (Unit unit) =>
        {
          unit.Health -= _damage;
          if (unit.Health <= 0)
          {
            unit.IsDead = true;
          }
        },
        -_attackRange + Position.X, -_attackRange + Position.Y, -_attackRange + Position.Z, _attackRange + Position.X, _attackRange + Position.Y, _attackRange + Position.Z
      );
      _isDead = true;
    }
  }
}
