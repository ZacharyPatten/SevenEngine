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
    private const int _healthMin = 500;
    private const int _healthMax = 1000;
    private const int _damageMin = 100;
    private const int _damageMax = 200;
    private const int _viewDistanceMin = 1;
    private const int _viewDistanceMax = 10000;
    private const int _moveSpeedMin = 6;
    private const int _moveSpeedMax = 10;
    private const int _attackRangeMin = 50;
    private const int _attackRangeMax = 70;

    private bool _exploded; 

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
      _exploded = false;
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
          if (!unit.IsDead)
          {
            unit.Health -= _damage;
            if (unit.Health <= 0)
            {
              unit.IsDead = true;
            }
          }
        },
        -_attackRange + Position.X, -_attackRange + Position.Y, -_attackRange + Position.Z, _attackRange + Position.X, _attackRange + Position.Y, _attackRange + Position.Z
      );
      IsDead = true;
    }
  }
}
